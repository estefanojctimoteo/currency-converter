using System;
using Currency_Converter.Domain.Core.Commands;

namespace Currency_Converter.Domain.Users.Commands
{
    public class AddUserCommand : Command
    {
        public Guid Id { get; private set; }
        public string Email { get; private set; }
        
        public AddUserCommand(string email)
        {
            Id = Guid.NewGuid();
            Email = email;
        }
    }
}