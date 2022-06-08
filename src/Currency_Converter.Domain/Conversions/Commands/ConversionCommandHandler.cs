using System.Threading;
using System.Threading.Tasks;
using Currency_Converter.Domain.Core.Notifications;
using Currency_Converter.Domain.Handlers;
using Currency_Converter.Domain.Interfaces;
using Currency_Converter.Domain.Conversions.Events;
using Currency_Converter.Domain.Conversions.Repository;
using MediatR;
using Currency_Converter.Domain.Users.Repository;
using FluentValidation.Results;

namespace Currency_Converter.Domain.Conversions.Commands
{
    public class ConversionCommandHandler : CommandHandler,
             IRequestHandler<AddConversionCommand>
    {
        private readonly IMediatorHandler _mediator;
        private readonly IConversionRepository _conversionRepository;
        private readonly IUserRepository _userRepository;

        public ConversionCommandHandler(
            IUnitOfWork uow,
            INotificationHandler<DomainNotification> notifications,
            IConversionRepository conversionRepository, 
            IUserRepository userRepository,
            IMediatorHandler mediator) : base(uow, mediator, notifications)
        {
            _conversionRepository = conversionRepository;
            _userRepository = userRepository;
            _mediator = mediator;
        }

        public Task Handle(AddConversionCommand message, CancellationToken cancellationToken)
        {
            var conversion = new Conversion(message.Id, message.UserId, message.CurrencyFrom, message.AmountFrom, message.CurrencyTo, message.AmountTo, message.Fee, message.DateTimeUtc);

            LocalErrorHandler localErroHandler = new LocalErrorHandler();
            var validator = new LocalCustomValidator();
            var user = _userRepository.GetById(message.UserId);
            if (user == null)
            {
                localErroHandler.Error = "[UserId] is invalid.";
                ValidationResult results = validator.Validate(localErroHandler);
                NotififyValidationErrors(results);
                return Task.CompletedTask;
            }

            if (!conversion.IsValid())
            {
                NotififyValidationErrors(conversion.ValidationResult);
                return Task.CompletedTask;
            }

            _conversionRepository.Add(conversion);
            _mediator.PublishEvent(new ConversionAddedEvent(message.Id, message.UserId, message.CurrencyFrom, message.AmountFrom, message.CurrencyTo, message.AmountTo, message.Fee, message.DateTimeUtc));

            return Task.CompletedTask;
        }
    }
}