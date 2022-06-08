using Currency_Converter.Domain.Core.Events;
using Currency_Converter.Domain.Interfaces;
using Currency_Converter.Infra.Data.Repository.EventSourcing;
using Newtonsoft.Json;

namespace Currency_Converter.Infra.Data.EventSourcing
{
    public class SqlEventStore : IEventStore
    {
        private readonly IEventStoreRepository _eventStoreRepository;        

        public SqlEventStore(IEventStoreRepository eventStoreRepository)
        {
            _eventStoreRepository = eventStoreRepository;            
        }

        public void SaveEvent<T>(T _event) where T : Event
        {
            var serializedData = JsonConvert.SerializeObject(_event);

            var storedEvent = new StoredEvent(
                _event,
                serializedData);

            _eventStoreRepository.Store(storedEvent);
        }
    }
}