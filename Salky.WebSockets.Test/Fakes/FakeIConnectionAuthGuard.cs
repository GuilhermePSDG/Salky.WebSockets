using Microsoft.AspNetCore.Http;
using Salky.WebSockets.Contracts;

namespace Salky.WebSockets.Test.Fakes
{
    public class FakeIConnectionAuthGuard : IConnectionAuthGuard
    {
        public bool CanAutorize = true;
        public int AutenticateConnectionCount = 0;
        public async Task<WebSocketUser?> AuthenticateConnection(HttpContext httpContext)
        {
            AutenticateConnectionCount++;
            if (CanAutorize)
            {
                await httpContext.WebSockets.AcceptWebSocketAsync();
                return await Task.FromResult(new WebSocketUser("", new()));

            }
            else
                return null;
        }
    }
}
