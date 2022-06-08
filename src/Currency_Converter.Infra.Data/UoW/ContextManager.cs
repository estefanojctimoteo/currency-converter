using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using Currency_Converter.Infra.Data.Context;

namespace Currency_Converter.Infra.Data.UoW
{
    public class ContextManager
    {
        public const string _ContextKey = "ContextManager.Context.EF";
        private Dictionary<string, object> _dicConext = new Dictionary<string, object>();
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ContextManager(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public ConversionContext Context
        {
            get
            {                
                if (_httpContextAccessor.HttpContext.Items[_ContextKey] == null)
                {
                    _dicConext = new Dictionary<string, object>();
                    var ctx = new ConversionContext();
                    ctx.Database.BeginTransaction();
                    _dicConext.Add(_ContextKey, ctx);
                    _httpContextAccessor.HttpContext.Items[_ContextKey] = _dicConext;
                }
                else
                {
                    _dicConext = (Dictionary<string, object>)_httpContextAccessor.HttpContext.Items[_ContextKey];
                    _dicConext.Add(_ContextKey + _dicConext.Count, "");
                    _httpContextAccessor.HttpContext.Items[_ContextKey] = _dicConext;

                }
                var httpContext = (Dictionary<string, object>)_httpContextAccessor.HttpContext.Items[_ContextKey];
                
                return (ConversionContext)httpContext[_ContextKey];
            }
        }

        public bool IsFirst
        {
            get
            {
                var _dicConext = (Dictionary<string, object>)_httpContextAccessor.HttpContext.Items[_ContextKey];
                return _dicConext.Count == 1;
            }
        }

        public int RemoveContex
        {
            get
            {
                var _dicConext = (Dictionary<string, object>)_httpContextAccessor.HttpContext.Items[_ContextKey];
                if (_dicConext.Count > 1)
                {
                    _dicConext.Remove(_ContextKey + (_dicConext.Count - 1));
                    _httpContextAccessor.HttpContext.Items[_ContextKey] = _dicConext;
                }
                return 1;
            }
        }

    }
}
