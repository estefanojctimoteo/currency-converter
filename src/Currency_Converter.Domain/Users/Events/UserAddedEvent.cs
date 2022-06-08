using System;
using Currency_Converter.Domain.Core.Events;

namespace Currency_Converter.Domain.Users.Events
{
    public class UserAddedEvent : Event
    {
        public Guid Id { get; private set; }
        public string Email { get; private set; }

        public UserAddedEvent(Guid id, string email)
        {
            Id = id;
            Email = email;
        }
    }
}