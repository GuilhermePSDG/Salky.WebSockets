// configure
using Salky.WebSockets.Contracts;
using Salky.WebSockets.Enums;
using Salky.WebSockets.Models;
using System.Net.WebSockets;
using System.Security.Claims;

namespace Example;

public class ConnectionAuthGuard : IConnectionAuthGuard
{
    private readonly IConnectionMannager connectionMannager;
    public ConnectionAuthGuard(IConnectionMannager connectionMannager)
    {
        this.connectionMannager = connectionMannager;
    }
    public async Task<WebSocketUser?> AutorizeConnection(HttpContext ctx)
    {
        if (!ctx.Request.Path.HasValue)
        {
            Console.WriteLine("Usuario não enviou o id/token");
            return null;
        }
        var usrId = ctx.Request.Path.Value;
        if (connectionMannager.ContainsKey(usrId))
        {
            Console.WriteLine("O usuario já está connectado, removendo ..");
            var removed = await connectionMannager.TryRemoveConnection(usrId);
            if (removed == null)
            {
                Console.WriteLine("Usuario não removido.");
            }
            else
            {
                await removed.CloseOutputAsync(WebSocketCloseStatus.PolicyViolation, CloseDescription.DuplicatedConnection);
                Console.WriteLine("Usuario removido, conexão fechada.");
            }
            return null;
        }
        var claims = new List<Claim>() { new("id", usrId) };
        return new WebSocketUser(usrId, claims);
    }


}
