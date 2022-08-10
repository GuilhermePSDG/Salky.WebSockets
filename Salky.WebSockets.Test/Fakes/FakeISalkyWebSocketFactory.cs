using Microsoft.AspNetCore.Http;
using Salky.WebSockets.Contracts;

namespace Salky.WebSockets.Test.Fakes
{
    public class FakeISalkyWebSocketFactory : ISalkyWebSocketFactory
    {
        public async Task<ISalkyWebSocket> CreateNewAsync(WebSocketManager webSocketManager, WebSocketUser user, IStorage storage)
        {
            return await Task.FromResult(new FakeISalkyWebSocket("abcd"));
        }
    }
}
