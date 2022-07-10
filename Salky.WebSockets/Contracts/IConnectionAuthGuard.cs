using Microsoft.AspNetCore.Http;
using Salky.WebSockets.Models;

namespace Salky.WebSockets.Contracts;

public interface IConnectionAuthGuard
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="httpContext"></param>
    /// <returns>The <see cref="WebSocketUser"/> of the user if autorized  <see langword="null"/> if not </returns>
    public Task<WebSocketUser?> AuthenticateConnection(HttpContext httpContext);
}
