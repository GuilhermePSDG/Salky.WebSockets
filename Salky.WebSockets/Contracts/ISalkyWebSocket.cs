// configure
using Salky.WebSockets.Enums;
using Salky.WebSockets.Models;
using System.Net.WebSockets;

namespace Salky.WebSockets.Contracts
{
    public interface ISalkyWebSocket
    {
        public WebSocketCloseStatus? CloseStatus { get; }
        public string? CloseStatusDescription { get; }
        public WebSocketState State { get; }
        public WebSocketUser User { get; }
        public Task CloseOutputAsync(WebSocketCloseStatus closeStatus, CloseDescription statusDescription);
        public Task<WebSocketReceiveResult> ReceiveAsync(ArraySegment<byte> buffer, CancellationToken cancellationToken);
        public Task SendErrorAsync(string message, string path, Method method = Method.POST);
        public Task SendMessageServer(MessageServer messageServer);
        public void Dispose();
    }
}