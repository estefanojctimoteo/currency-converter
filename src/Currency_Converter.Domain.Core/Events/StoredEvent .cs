using System;

namespace Currency_Converter.Domain.Core.Events
{
    public class StoredEvent : Event
    {
        public StoredEvent(Event evento, string data)
        {
            Id = Guid.NewGuid();            
            MessageType = evento.MessageType;
            Data = data;            
        }

        // EF Constructor
        protected StoredEvent() { }

        public Guid Id { get; private set; }

        public string Data { get; private set; }        
    }
}