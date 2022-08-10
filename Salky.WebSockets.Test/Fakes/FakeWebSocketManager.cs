using Microsoft.AspNetCore.Http;
using System.Net.WebSockets;

namespace Salky.WebSockets.Test.Fakes
{
    public class FakeWebSocketManager : WebSocketManager
    {
        public override bool IsWebSocketRequest => true;
        public override IList<string> WebSocketRequestedProtocols => throw new NotImplementedException();

        public int AcceptWebSocketAsyncCounter = 0;
        public override async Task<WebSocket> AcceptWebSocketAsync(string? subProtocol)
        {
            AcceptWebSocketAsyncCounter++;
            return await Task.FromResult(new FakeWebSocket());
        }
    }
}
