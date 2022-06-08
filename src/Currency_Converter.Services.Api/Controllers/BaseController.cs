using System.Linq;
using Currency_Converter.Domain.Core.Notifications;
using Currency_Converter.Domain.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using static Currency_Converter.Services.Api.Startup;
using System.Net;

namespace Currency_Converter.Services.Api.Controllers
{    
    [Produces("application/json")]
    public abstract class BaseController : Controller
    {
        private readonly DomainNotificationHandler _notifications;
        private readonly IMediatorHandler _mediator;
        private readonly IOptions<ApiKeys> _apiKeys;

        protected BaseController(INotificationHandler<DomainNotification> notifications, 
                                 IMediatorHandler mediator,
                                 IOptions<ApiKeys> apiKeys)
        {
            _notifications = (DomainNotificationHandler)notifications;
            _mediator = mediator;
            _apiKeys = apiKeys;
        }

        protected IActionResult Response_BadRequest(string msgErro)
        {
            return BadRequest(new
            {
                success = false,
                errors = msgErro
            });
        }

        protected IActionResult Response_BadRequest(string msgErro, object result = null)
        {
            return BadRequest(new
            {
                success = false,
                errors = msgErro,
                data = result
            });
        }

        protected new IActionResult Response(string url="", int httpStatusCode=-1, object result = null)
        {
            if (OperationIsValid())
            {
                if (url == "" || httpStatusCode <= 0 || (url != "" && httpStatusCode == 200))
                    return Ok(new
                    {
                        success = true,
                        data = result
                    });

                switch (httpStatusCode)
                {
                    case 201:
                        return Created(url, result);
                }
            }

            return BadRequest(new
            {
                success = false,
                errors = _notifications.GetNotifications().Select(n=>n.Value)
            });
        }

        protected bool OperationIsValid()
        {
            return (!_notifications.HasNotifications());
        }

        protected void NotificateError_ModelInvalid()
        {
            var erros = ModelState.Values.SelectMany(v => v.Errors);
            foreach (var erro in erros)
            {
                var erroMsg = erro.Exception == null ? erro.ErrorMessage : erro.Exception.Message;
                NotificateError(string.Empty, erroMsg);
            }
        }

        protected void NotificateError(string cod, string msg)
        {
            _mediator.PublishEvent(new DomainNotification(cod, msg));
        }

        protected Api_Key GetApikeyDataFromId(string id)
        {
            var key = _apiKeys.Value.Keys.Where(k => k.Id == id).FirstOrDefault();
            return key;
        }
    }
}