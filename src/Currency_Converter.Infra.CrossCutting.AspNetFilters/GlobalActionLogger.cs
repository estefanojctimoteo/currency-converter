using System;
using System.Collections.Generic;
using System.Linq;
using Elmah.Io.Client;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;

namespace Currency_Converter.Infra.CrossCutting.AspNetFilters
{
    public class GlobalActionLogger : IActionFilter
    {        
        private readonly IHostingEnvironment _hostingEnviroment;

        public GlobalActionLogger(IHostingEnvironment hostingEnviroment)
        {            
            _hostingEnviroment = hostingEnviroment;            
        }

        public async void OnActionExecuted(ActionExecutedContext context)
        {
            if (_hostingEnviroment.IsDevelopment())
            {
                var message = new CreateMessage
                {
                    Version = "v1.0",
                    Application = "Currency_Converter",
                    Source = "GlobalActionLoggerFilter",
                    User = context.HttpContext.User.Identity.Name,
                    Hostname = context.HttpContext.Request.Host.Host,
                    Url = context.HttpContext.Request.GetDisplayUrl(),
                    DateTime = DateTime.Now,
                    Method = context.HttpContext.Request.Method,
                    StatusCode = context.HttpContext.Response.StatusCode,
                    Cookies = context.HttpContext.Request?.Cookies?.Keys.Select(k => new Item(k, context.HttpContext.Request.Cookies[k])).ToList(),
                    Form = Form(context.HttpContext),
                    ServerVariables = context.HttpContext.Request?.Headers?.Keys.Select(k => new Item(k, context.HttpContext.Request.Headers[k])).ToList(),
                    QueryString = context.HttpContext.Request?.Query?.Keys.Select(k => new Item(k, context.HttpContext.Request.Query[k])).ToList(),
                    Data = context.Exception?.ToDataList(),
                    Detail = JsonConvert.SerializeObject(new { ExtraDetail = "ExtraDetail", DataInfo = "It can be a Json" })
                };
                
                var client = ElmahioAPI.Create("ELMAH_API_KEY_GOES_HERE");
                await client.Messages.CreateAndNotifyAsync(Guid.NewGuid(), message);                
            }
        }
        private static List<Item> Form(HttpContext httpContext)
        {
            try
            {
                return httpContext.Request?.Form?.Keys.Select(k => new Item(k, httpContext.Request.Form[k])).ToList();
            }
            catch (InvalidOperationException)
            {
                // Request not a form POST or similar
            }

            return null;
        }
        public void OnActionExecuting(ActionExecutingContext context)
        {

        }
    }
}