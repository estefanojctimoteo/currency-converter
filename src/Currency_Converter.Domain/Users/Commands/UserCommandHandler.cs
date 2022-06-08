using System.Threading;
using System.Threading.Tasks;
using Currency_Converter.Domain.Core.Notifications;
using Currency_Converter.Domain.Handlers;
using Currency_Converter.Domain.Interfaces;
using Currency_Converter.Domain.Users.Events;
using Currency_Converter.Domain.Users.Repository;
using MediatR;
using FluentValidation.Results;

namespace Currency_Converter.Domain.Users.Commands
{
    public class UserCommandHandler : CommandHandler,
             IRequestHandler<AddUserCommand>
    {
        private readonly IMediatorHandler _mediator;
        private readonly IUserRepository _userRepository;

        public UserCommandHandler(
            IUnitOfWork uow,
            INotificationHandler<DomainNotification> notifications,
            IUserRepository userRepository, IMediatorHandler mediator) : base(uow, mediator, notifications)
        {
            _userRepository = userRepository;
            _mediator = mediator;
        }

        public Task Handle(AddUserCommand message, CancellationToken cancellationToken)
        {
            var user = new User(message.Id, message.Email);

            if (!user.IsValid())
            {
                NotififyValidationErrors(user.ValidationResult);
                return Task.CompletedTask;
            }

            LocalErrorHandler localErroHandler = new LocalErrorHandler();
            var validator = new LocalCustomValidator();
            var existingUser = _userRepository.GetByEmail(message.Email);
            if (existingUser != null)
            {
                localErroHandler.Error = "User email already registered.";
                ValidationResult results = validator.Validate(localErroHandler);
                NotififyValidationErrors(results);
                return Task.CompletedTask;
            }

            _userRepository.Add(user);
            _mediator.PublishEvent(new UserAddedEvent(user.Id, user.Email));

            return Task.CompletedTask;
        }
    }
}