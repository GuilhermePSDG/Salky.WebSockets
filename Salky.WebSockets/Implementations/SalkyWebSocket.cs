// configure
using Salky.WebSockets.Contracts;
using Salky.WebSockets.Enums;
using Salky.WebSockets.Models;
using System.Net.WebSockets;
using System.Text;

namespace Salky.WebSockets.Implementations;


public class SalkyWebSocket : ISalkyWebSocket
{
    private WebSocket webSocket;
    public readonly Key UniqueRandomId;
    public WebSocketUser User { get; set; }
    public SalkyWebSocket(WebSocket socket, WebSocketUser user, IStorage storage)
    {
        webSocket = socket;
        UniqueRandomId = Guid.NewGuid();
        User = user;
        user.Storage = storage;
    }

    //public bool CanClose => State == WebSocketState.Open || State == WebSocketState.CloseReceived;
    public string? CloseStatusDescription => webSocket.CloseStatusDescription;
    public WebSocketCloseStatus? CloseStatus => webSocket.CloseStatus;
    public WebSocketState State => webSocket.State;
    //public bool ConnectionsIsOpen => State == WebSocketState.Open;
    public async Task CloseOutputAsync(WebSocketCloseStatus closeStatus, CloseDescription statusDescription) =>
        await webSocket.CloseOutputAsync(closeStatus, statusDescription.ToString(), CancellationToken.None);

    public void Dispose()
    {
        TRY(() =>
        {
            if ((this.webSocket.State != WebSocketState.Closed && this.webSocket.State != WebSocketState.Aborted)) 
                this.webSocket.Abort();
        });
        TRY(webSocket.Dispose);
        TRY(User.Clear);
    }
    private void TRY(Action act)
    {
        try { act(); } finally { }
    }

    public async Task<WebSocketReceiveResult> ReceiveAsync(ArraySegment<byte> buffer, CancellationToken cancellationToken)
    {
        return await webSocket.ReceiveAsync(buffer, cancellationToken);
    }

    public async Task SendMessageServer(MessageServer messageServer)
    {
        var json = JsonSerializer.Serialize(messageServer, DefaultJsonSerializerOptions);
        var buffer = Encoding.UTF8.GetBytes(json);
        await webSocket.SendAsync(buffer, WebSocketMessageType.Text, true, CancellationToken.None); ;
    }

    public async Task SendErrorAsync(string message, string path, Method method = Method.POST)
    {
        await SendMessageServer(new MessageServer(path, method, Status.Error, message));
    }

}
