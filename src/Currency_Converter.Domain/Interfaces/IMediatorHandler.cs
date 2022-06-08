using System.Threading.Tasks;
using Currency_Converter.Domain.Core.Commands;
using Currency_Converter.Domain.Core.Events;

namespace Currency_Converter.Domain.Interfaces
{
    public interface IMediatorHandler
    {
        Task PublishEvent<T>(T _event) where T : Event;
        Task SendCommand<T>(T command) where T : Command;
    }
}