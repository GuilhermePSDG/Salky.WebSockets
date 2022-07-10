namespace Salky.WebSockets.Contracts;
public interface IConnectionEventHandler
{
    public Task HandleOpen(ISalkyWebSocket socket);
    public Task HandleClose(ISalkyWebSocket socket);
}
