using Currency_Converter.Domain.Core.Notifications;
using Currency_Converter.Domain.Interfaces;
using FluentValidation.Results;
using MediatR;
using FluentValidation;

namespace Currency_Converter.Domain.Handlers
{
    public abstract class CommandHandler
    {
        private readonly IUnitOfWork _uow;
        private readonly IMediatorHandler _mediator;
        private readonly DomainNotificationHandler _notifications;

        protected CommandHandler(IUnitOfWork uow, IMediatorHandler mediator, INotificationHandler<DomainNotification> notifications)
        {
            _uow = uow;
            _mediator = mediator;
            _notifications = (DomainNotificationHandler)notifications;
        }

        protected CommandHandler(IMediatorHandler mediator)
        {
            _mediator = mediator;
        }

        protected void NotififyValidationErrors(ValidationResult validationResult)
        {
            foreach (var error in validationResult.Errors)
            {
                _mediator.PublishEvent(new DomainNotification(error.PropertyName, error.ErrorMessage));
            }
        }

        protected bool Commit()
        {
            if (_notifications.HasNotifications()) return false;
            if (_uow.Commit()) return true;

            _mediator.PublishEvent(new DomainNotification("Commit", "An error has occurred trying to save the data"));
            return false;
        }
    }
    public class LocalErrorHandler
    {
        public string Error { get; set; }
        public LocalErrorHandler()
        {
            Error = string.Empty;
        }
    }
    public class LocalCustomValidator : AbstractValidator<LocalErrorHandler>
    {
        public LocalCustomValidator()
        {
            RuleFor(x => x.Error).Empty().WithMessage(x => x.Error);
        }
    }
}