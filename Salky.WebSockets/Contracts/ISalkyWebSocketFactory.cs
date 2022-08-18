using Microsoft.AspNetCore.Http;
using Salky.WebSockets.Models;
using System.Net.WebSockets;

namespace Salky.WebSockets.Contracts
{
    public interface ISalkyWebSocketFactory
    {
        public Task<ISalkyWebSocket> CreateNewAsync(WebSocketManager webSocketManager, WebSocketUser user);
    }
}