using Salky.WebSockets.Models;

namespace Salky.WebSockets.Contracts;
public interface IConnectionEventHandler
{
    public Task HandleOpen(SalkyWebSocket socket);
    public Task HandleClose(SalkyWebSocket socket);
}
