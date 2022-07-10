namespace Salky.WebSockets.Contracts;


public interface IMessageHandler
{

    /// <summary>
    /// <see cref="System.Net.WebSockets.WebSocketMessageType.Binary"/>
    /// </summary>
    /// <param name="socket"></param>
    /// <param name="data"></param>
    /// <returns></returns>
    public Task HandleBinary(ISalkyWebSocket socket, MemoryStream data);
    /// <summary>
    /// <see cref="System.Net.WebSockets.WebSocketMessageType.Text"/>
    /// </summary>
    /// <param name="socket"></param>
    /// <param name="data"></param>
    /// <returns></returns>
    public Task HandleText(ISalkyWebSocket socket, MemoryStream data);
}
