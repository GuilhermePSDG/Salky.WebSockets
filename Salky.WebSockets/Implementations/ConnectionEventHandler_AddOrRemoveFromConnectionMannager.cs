using Microsoft.Extensions.Logging;
using Salky.WebSockets.Contracts;
using Salky.WebSockets.Models;

namespace Salky.WebSockets.Implementations;
public class ConnectionEventHandler_AddOrRemoveFromConnectionMannager : IConnectionEventHandler
{
    private readonly ILogger<IConnectionEventHandler> logger;
    private readonly IConnectionMannager connectionMannager;
    public ConnectionEventHandler_AddOrRemoveFromConnectionMannager(ILogger<IConnectionEventHandler> logger, IConnectionMannager connectionMannager)
    {
        this.logger = logger;
        this.connectionMannager = connectionMannager;
    }
    public async Task HandleClose(SalkyWebSocket ws)
    {
        if (await connectionMannager.TryRemoveConnection(ws.User.UserId) == null)
        {
            logger.LogWarning($"Connection not removed from {nameof(IConnectionMannager)} after conection closed");
        }
    }
    public async Task HandleOpen(SalkyWebSocket ws)
    {
        if (!await connectionMannager.AddConnection(ws.User.UserId, ws))
        {
            logger.LogWarning($"Connection not added in {nameof(IConnectionMannager)} after conection open");
        }
    }
}
