using Microsoft.Extensions.Logging;
using Salky.WebSockets.Contracts;

namespace Salky.WebSockets.Implementations;
public class ConnectionEventHandler_AddOrRemoveFromRootConnectionMannager : IConnectionEventHandler
{
    private readonly ILogger<IConnectionEventHandler> logger;
    private readonly IConnectionMannager connectionMannager;
    public ConnectionEventHandler_AddOrRemoveFromRootConnectionMannager(ILogger<IConnectionEventHandler> logger, IConnectionMannager connectionMannager)
    {
        this.logger = logger;
        this.connectionMannager = connectionMannager;
    }
    public async Task HandleClose(ISalkyWebSocket ws)
    {

        if (await connectionMannager.TryRemoveConnection(ws.User.UserId) == null)
        {
            logger.LogWarning($"Cannot remove from {nameof(IConnectionMannager)} after conection closed");
        }
        else
        {
            logger.LogInformation($"User removed from {nameof(IConnectionMannager)}");
        }
    }
    public async Task HandleOpen(ISalkyWebSocket ws)
    {
        if (!await connectionMannager.AddConnection(ws.User.UserId, ws))
        {
            logger.LogWarning($"Connection not added in {nameof(IConnectionMannager)} after conection open");
        }
        else
        {
            logger.LogInformation($"New user added at {nameof(IConnectionMannager)}");
        }
    }
}
