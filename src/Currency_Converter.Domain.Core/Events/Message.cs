using System;
using MediatR;

namespace Currency_Converter.Domain.Core.Events
{
    public abstract class Message : INotification
    {
        public string MessageType { get; protected set; }        
        protected Message() 
        {
            MessageType = GetType().Name;
        }
    }
}