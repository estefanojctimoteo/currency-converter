using System;
using Microsoft.Extensions.DependencyInjection;

namespace Currency_Converter.Services.Api.Configurations
{
    public static class MvcConfiguration
    {
        public static void AddMvcSecurity(this IServiceCollection services)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));
        }
    }
}