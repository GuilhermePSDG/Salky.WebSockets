using Microsoft.AspNetCore.Http;
using Salky.WebSockets.Contracts;
using Salky.WebSockets.Models;
using System.Net.WebSockets;

namespace Salky.WebSockets.Implementations
{
    public class SalkyWebSocketFactory : ISalkyWebSocketFactory
    {
        public static string? Protocol { get; set; } = null;
        public async Task<ISalkyWebSocket> CreateNewAsync(WebSocketManager ws, WebSocketUser user, IStorage storage)
        {
            return new SalkyWebSocket(await ws.AcceptWebSocketAsync(SalkyWebSocketFactory.Protocol), user, storage);
        }
    }

}
