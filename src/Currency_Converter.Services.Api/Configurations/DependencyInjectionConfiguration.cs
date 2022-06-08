using Currency_Converter.Infra.CrossCutting.IoC;
using Microsoft.Extensions.DependencyInjection;

namespace Currency_Converter.Services.Api.Configurations
{
    public static class DependencyInjectionConfiguration
    {
        public static void AddDIConfiguration(this IServiceCollection services)
        {
            NativeInjectorBootStrapper.RegisterServices(services);
        }
    }
}