using System.Threading;
using System.Threading.Tasks;
using MediatR;

namespace Currency_Converter.Domain.Users.Events
{
    public class UserEventHandler :         
        INotificationHandler<UserAddedEvent>
    {
        public Task Handle(UserAddedEvent message, CancellationToken cancellationToken)
        {            
            return Task.CompletedTask;
        }
    }
}