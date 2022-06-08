using AutoMapper;
using Currency_Converter.Infra.CrossCutting.AspNetFilters;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using Elmah.Io.Extensions.Logging;
using Microsoft.Extensions.Logging;
using Currency_Converter.Services.Api.Configurations;
using MediatR;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.AspNetCore.Http.Features;

using System.Collections.Generic;
using System.Globalization;
using Microsoft.AspNetCore.Localization;

using System;

namespace Currency_Converter.Services.Api
{
    public class Startup
    {
        public Startup(IWebHostEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();

            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<RequestLocalizationOptions>(options =>
            {
                options.DefaultRequestCulture = new Microsoft.AspNetCore.Localization.RequestCulture("pt-PT");
                options.SupportedCultures = new List<CultureInfo> { new CultureInfo("pt-PT") };
                options.RequestCultureProviders = new List<IRequestCultureProvider>();                                
            });


            // Options for customized configurations
            services.AddOptions();
            
            services.AddMvc(options =>
            {
                options.OutputFormatters.Remove(new XmlDataContractSerializerOutputFormatter());
                options.Filters.Add(new ServiceFilterAttribute(typeof(GlobalActionLogger)));
            });

            services.Configure<FormOptions>(x =>
            {
                x.ValueLengthLimit = int.MaxValue;
                x.MultipartBodyLengthLimit = int.MaxValue; // In case of multipart
            });

            // AutoMapper
            services.AddAutoMapper();

            // The following line enables Application Insights telemetry collection.
            services.AddApplicationInsightsTelemetry();

            services.AddMvc(options => options.EnableEndpointRouting = false);

            // Swagger
            services.AddSwaggerConfig();

            // MediatR
            services.AddMediatR(typeof(Startup));

            // DI Configuration
            services.AddDIConfiguration();

            services.Configure<ApiKeys>(Configuration.GetSection("ApiKeys"));
        }
        public class ApiKeys
        {
            public Api_Key[] Keys { get; set; }
        }
        public class Api_Key
        {
            public string Id { get; set; }
            public string ApiKey { get; set; }
            public string BaseAddress { get; set; }
        }
        public class H2Core_ApiKey
        {
            public string ApiKey { get; set; }
        }        
        public void Configure(IApplicationBuilder app,
                              ILoggerFactory loggerFactory)
        {
            #region Logging

            var elmahIoOptions = new ElmahIoProviderOptions
            {
                OnMessage = message =>
                {
                    message.Version = "v1.0";
                    message.Application = "Currency_Converter";                    
                },
            };
            
            loggerFactory.AddElmahIo("ELMAH_API_KEY_GOES_HERE", Guid.NewGuid(), elmahIoOptions);
            
            #endregion

            #region MVC configurations

            app.UseCors(c =>
            {
                c.AllowAnyHeader();
                c.AllowAnyMethod();
                c.AllowAnyOrigin();
            });

            app.UseStaticFiles();
            app.UseElmahIoExtensionsLogging();
            app.UseMvc();

            #endregion

            #region Swagger

            app.UseSwagger();
            app.UseSwaggerUI(s =>
            {
                s.SwaggerEndpoint("../swagger/v1/swagger.json", "Currency Currencies Converter API v1.0");
            });

            #endregion
        }
    }
}
