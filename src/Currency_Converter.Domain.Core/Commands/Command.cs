using System;
using Currency_Converter.Domain.Core.Events;
using MediatR;

namespace Currency_Converter.Domain.Core.Commands
{
    public class Command : Message, IRequest
    {        
        public Command() {}
    }
}