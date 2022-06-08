using MediatR;
using AutoMapper;

using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using Currency_Converter.Domain.Core.Notifications;
using Currency_Converter.Domain.Handlers;
using Currency_Converter.Domain.Interfaces;

using Currency_Converter.Domain.Users.Repository;
using Currency_Converter.Domain.Users.Commands;
using Currency_Converter.Domain.Users.Events;

using Currency_Converter.Domain.Conversions.Repository;
using Currency_Converter.Domain.Conversions.Commands;
using Currency_Converter.Domain.Conversions.Events;

using Currency_Converter.Infra.CrossCutting.AspNetFilters;

using Currency_Converter.Infra.Data.Repository;
using Currency_Converter.Infra.Data.Context;
using Currency_Converter.Infra.Data.UoW;
using Currency_Converter.Infra.Data.Repository.EventSourcing;
using Currency_Converter.Infra.Data.EventSourcing;

namespace Currency_Converter.Infra.CrossCutting.IoC
{
    public class NativeInjectorBootStrapper
    {
        public static void RegisterServices(IServiceCollection services)
        {
            // ASPNET
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddSingleton(Mapper.Configuration);
            services.AddScoped<IMapper>(sp => new Mapper(sp.GetRequiredService<IConfigurationProvider>(), sp.GetService));


            // Domain Bus (Mediator)
            services.AddScoped<IMediatorHandler, MediatorHandler>();


            // Domain - Commands
            services.AddScoped<IRequestHandler<AddUserCommand>, UserCommandHandler>();
            services.AddScoped<IRequestHandler<AddConversionCommand>, ConversionCommandHandler>();


            // Domain - Events
            services.AddScoped<INotificationHandler<DomainNotification>, DomainNotificationHandler>();
            services.AddScoped<INotificationHandler<UserAddedEvent>, UserEventHandler>();
            services.AddScoped<INotificationHandler<ConversionAddedEvent>, ConversionEventHandler>();


            // Infra - Data
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IConversionRepository, ConversionRepository>();


            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<ConversionContext>();            
            services.AddSingleton<ContextManager>();

            // Infra - Data EventSourcing
            services.AddScoped<IEventStoreRepository, EventStoreSQLRepository>();
            services.AddScoped<IEventStore, SqlEventStore>();
            services.AddScoped<EventStoreSQLContext>();


            // Infra
            services.AddScoped<ILogger<GlobalExceptionHandlingFilter>, Logger<GlobalExceptionHandlingFilter>>();
            services.AddScoped<ILogger<GlobalActionLogger>, Logger<GlobalActionLogger>>();
            services.AddScoped<GlobalExceptionHandlingFilter>();
            services.AddScoped<GlobalActionLogger>();
        }
    }
}