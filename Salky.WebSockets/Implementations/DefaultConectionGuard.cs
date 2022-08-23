using Microsoft.AspNetCore.Http;
using Salky.WebSockets.Contracts;
using Salky.WebSockets.Models;

namespace Salky.WebSockets.Implementations
{
    public class DefaultConectionGuard : IConnectionAuthGuard, IConnectionEventHandler
    {
        public Task<WebSocketUser?> AuthenticateConnection(HttpContext httpContext)
        {
            return Task.FromResult<WebSocketUser?>(new WebSocketUser(Guid.NewGuid(), new List<System.Security.Claims.Claim>()));
        }

        public async Task HandleOpen(ISalkyWebSocket socket)
        {
            await socket.SendMessageServer(new MessageServer("connected", Enums.Method.POST, Enums.Status.Success, new
            {
                Message = "Connected successfully",
                ConnectionId = socket.User.UserId,
            }));
        }

        public Task HandleClose(ISalkyWebSocket socket) => Task.CompletedTask;
    }

}
