using System;
using System.Net;

using MediatR;
using AutoMapper;

using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

using Currency_Converter.Services.Api.ViewModels;

using Currency_Converter.Domain.Conversions.Repository;
using Currency_Converter.Domain.Conversions.Commands;

using Currency_Converter.Domain.Core.Notifications;

using Currency_Converter.Domain.Users.Repository;
using Currency_Converter.Domain.Users.Commands;

using Currency_Converter.Domain.Interfaces;
using System.Threading.Tasks;

using Microsoft.Extensions.Options;
using static Currency_Converter.Services.Api.Startup;
using RestSharp;
using System.Threading;

using Newtonsoft.Json;
using System.Linq;
using Microsoft.Extensions.Logging;

namespace Currency_Converter.Services.Api.Controllers
{    
    public class ConverterController : BaseController
    {
        #region Interfaces

        private readonly IMapper _mapper;        
        private readonly IMediatorHandler _mediator;
        private readonly IConversionRepository _conversionRepository;
        private readonly IUserRepository _userRepository;

        #endregion

        #region Constructor

        public ConverterController(INotificationHandler<DomainNotification> notifications,
                                 IMapper mapper,
                                 ILoggerFactory loggerFactory,
                                 IMediatorHandler mediator,
                                 IOptions<ApiKeys> apiKeys,
                                 IUserRepository userRepository,
                                 IConversionRepository conversionRepository) : base(notifications, mediator, apiKeys)
        {
            _mapper = mapper;
            _mediator = mediator;            
            _userRepository = userRepository;
            _conversionRepository = conversionRepository;
        }

        #endregion

        #region REST Methods

        #region Users

        #region Get user by email

        [HttpGet]
        [Route("users")]
        public IActionResult GetUserByEmail(string email)
        {
            var user = _mapper.Map<UserViewModel>(_userRepository.GetByEmail(email));
            return user == null ? StatusCode(404) : Response("", -1, user);
        }

        #endregion

        #region Post user

        [HttpPost]
        [Route("users")]
        public IActionResult PostUser([FromBody] AddUserViewModel addUserViewModel)
        {
            #region ModelStateIsValid?

            if (!ModelStateIsValid())
            {
                return Response_BadRequest("The model state is invalid!");
            }

            #endregion

            #region SendCommand

            var command = _mapper.Map<AddUserCommand>(addUserViewModel);
            _mediator.SendCommand(command);

            #endregion

            return Response("users", (int)HttpStatusCode.Created, command);
        }

        #endregion

        #endregion

        #region Conversions

        private readonly string exchangeratesapi = "exchangeratesapi";

        #region Get symbols

        [HttpGet]
        [Route("symbols")]
        public async Task<IActionResult> GetSymbols()
        {
            #region Get apiKeyData from appsettings

            var apiKeyData = GetApikeyDataFromId(exchangeratesapi);
            if (apiKeyData == null)
            {
                return Response_BadRequest(string.Format("ApiKey \"{0}\" not found!", exchangeratesapi));
            }

            #endregion

            #region requestUri, client, request

            var requestUri = CreateRequestUri("symbols", apiKeyData, null);
            var client = CreateRestClient(requestUri);
            var request = CreateRestRequest(Method.GET, apiKeyData.ApiKey);

            #endregion

            Object obj = await ExecuteRequest(client, request, "symbols");
            return obj == null ? StatusCode(404) : Response("", -1, obj);
        }

        #endregion

        #region Post conversion

        public Api_Key GetApikeyData(string id)
        {
            return GetApikeyDataFromId(id);
        }

        [HttpPost]
        [Route("conversions")]
        public async Task<IActionResult> PostConversion([FromBody] AddConversionViewModel conversionViewModel)
        {
            #region ModelStateIsValid?

            if (!ModelStateIsValid())
            {
                return Response_BadRequest("The model state is invalid!");
            }            

            #endregion

            #region Get apiKeyData from appsettings

            var apiKeyData = GetApikeyData(exchangeratesapi);
            if (apiKeyData == null)
            {
                return Response_BadRequest(string.Format("ApiKey \"{0}\" not found!", exchangeratesapi));
            }

            #endregion

            #region requestUri, client, request

            var requestUri = CreateRequestUri("convert", apiKeyData, conversionViewModel);
            var client = CreateRestClient(requestUri);            
            var request = CreateRestRequest(Method.GET, apiKeyData.ApiKey);            

            #endregion

            #region Call ExecuteRequest

            ConvertSuccess convertSuccess = new ConvertSuccess();
            Object obj = await ExecuteRequest(client, request, "convert");
            var type = obj.GetType();

            if (type == typeof(string))
            {
                return Response_BadRequest(obj.ToString());
            }
            else if (type == typeof(ConvertSuccess))
            {
                convertSuccess = (ConvertSuccess)obj;
            }

            #endregion

            #region SendCommand

            try
            {
                var command = _mapper.Map<AddConversionCommand>(conversionViewModel);

                #region Complete the command

                command.AmountTo = convertSuccess.Result;
                command.Fee = convertSuccess.Info.Rate;
                command.DateTimeUtc = CalcDateTimeUtcFromTimestamp(convertSuccess.Info.Timestamp);

                #endregion

                await _mediator.SendCommand(command);                

                return Response("conversions", (int)HttpStatusCode.Created, command);

            }
            catch (Exception exc)
            {
                return Response_BadRequest(exc.Message);
            }

            #endregion
        }

        #endregion

        #region Get conversions by user

        [HttpGet]
        [Route("conversions/user/{userId:guid}")]
        public IEnumerable<ConversionViewModel> ObterCategorias(Guid userId)
        {            
            return _mapper.Map<IEnumerable<ConversionViewModel>>(_conversionRepository.Search(c => c.UserId == userId).OrderByDescending(c => c.DateTimeUtc));
        }

        #endregion

        #region Aux methods

        #region CalcDateTimeUtcFromTimestamp

        private static DateTime CalcDateTimeUtcFromTimestamp(double timestamp)
        {
            DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            return dateTime.AddSeconds(timestamp);
        }

        #endregion

        #region ExecuteRequest

        private static async Task<object> ExecuteRequest(RestClient client, RestRequest request, string endpoint)
        {            
            try
            {
                var cancellationTokenSource = new CancellationTokenSource();
                var restResponse = await client.ExecuteTaskAsync(request, cancellationTokenSource.Token);

                switch (endpoint)
                {
                    case "symbols":
                        var symbolsSuccess = JsonConvert.DeserializeObject<SymbolsSuccess>(restResponse.Content);
                        if (symbolsSuccess.Success)
                            return symbolsSuccess;
                        break;

                    case "convert":
                        var convertSuccess = JsonConvert.DeserializeObject<ConvertSuccess>(restResponse.Content);
                        if (convertSuccess.Success)
                            return convertSuccess;
                        break;
                }

                var responseError = JsonConvert.DeserializeObject<ResponseError>(restResponse.Content);
                if (!string.IsNullOrWhiteSpace(responseError.Error.Message))
                    throw new Exception(responseError.Error.Message);
                throw new Exception("Something went wrong while requesting the exchange rates data");
            }
            catch (Exception exc) 
            {
                return exc.Message;
            }            
        }

        #endregion

        #region CreateRequestUri

        private static string CreateRequestUri(string endpoint, Api_Key apiKeyData, AddConversionViewModel conversionViewModel = null)
        {
            return endpoint switch
            {
                "convert" => string.Format("{0}/{1}?from={2}&to={3}&amount={4}",
                               apiKeyData.BaseAddress, endpoint, conversionViewModel.CurrencyFrom.ToUpper(), conversionViewModel.CurrencyTo.ToUpper(),
                               conversionViewModel.AmountFrom.ToString().Replace(",", ".")),

                _ => string.Format("{0}/{1}", apiKeyData.BaseAddress, endpoint),
            };
        }

        #endregion

        #region CreateRestClient

        private static RestClient CreateRestClient(string requestUri)
        {
            var client = new RestClient(requestUri)
            {
                Timeout = -1
            };
            return client;
        }

        #endregion

        #region CreateRestRequest

        private static RestRequest CreateRestRequest(Method method, string apiKey)
        {
            var request = new RestRequest(method);
            request.AddHeader("apikey", apiKey);
            return request;
        }

        #endregion

        #region structs

        private struct SymbolsSuccess
        {
            public bool Success { get; set; }
            public Dictionary<string, string> Symbols { get; set; }
        }
        private struct ResponseError
        {
            public Error Error { get; set; }
        }
        private struct Error
        {
            public string Code { get; set; }
            public string Message { get; set; }
        }
        private struct Info
        {
            public double Timestamp { get; set; }
            public decimal Rate { get; set; }
        }
        private struct Query
        {
            public string From { get; set; }
            public string To { get; set; }
            public decimal Amount { get; set; }
        }
        private struct ConvertSuccess
        {
            public bool Success { get; set; }
            public Query Query { get; set; }
            public Info Info { get; set; }
            public string Date { get; set; }
            public decimal Result { get; set; }
        }

        #endregion

        #endregion

        #endregion

        #endregion

        #region ModelState

        private bool ModelStateIsValid()
        {
            if (ModelState.IsValid) return true;

            NotificateError_ModelInvalid();
            return false;
        }

        #endregion
    }
}
