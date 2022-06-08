using System.Threading;
using System.Threading.Tasks;
using MediatR;

namespace Currency_Converter.Domain.Conversions.Events
{
    public class ConversionEventHandler :         
        INotificationHandler<ConversionAddedEvent>
    {
        public Task Handle(ConversionAddedEvent message, CancellationToken cancellationToken)
        {            
            return Task.CompletedTask;
        }
    }
}