using System.Threading.Tasks;
using Currency_Converter.Domain.Core.Commands;
using Currency_Converter.Domain.Core.Events;
using Currency_Converter.Domain.Interfaces;
using MediatR;

namespace Currency_Converter.Domain.Handlers
{
    public class MediatorHandler : IMediatorHandler
    {
        private readonly IMediator _mediator;
        private readonly IEventStore _eventStore;

        public MediatorHandler(IMediator mediator, IEventStore eventStore)
        {
            _mediator = mediator;
            _eventStore = eventStore;
        }

        public async Task SendCommand<T>(T comand) where T : Command
        {
            await _mediator.Send(comand);
        }

        public async Task PublishEvent<T>(T _event) where T : Event
        {
            if (!_event.MessageType.Equals("DomainNotification"))
                _eventStore?.SaveEvent(_event);

            await _mediator.Publish(_event);
        }
    }
}