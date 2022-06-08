using Currency_Converter.Domain.Core.Events;

namespace Currency_Converter.Domain.Interfaces
{
    public interface IEventStore
    {
        void SaveEvent<T>(T _event) where T : Event;
    }
}