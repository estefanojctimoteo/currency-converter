using System;
using System.Collections.Generic;
using Currency_Converter.Domain.Core.Events;

namespace Currency_Converter.Infra.Data.Repository.EventSourcing
{
    public interface IEventStoreRepository : IDisposable
    {
        void Store(StoredEvent theEvent);
        IList<StoredEvent> All(Guid aggregateId);
    }
}