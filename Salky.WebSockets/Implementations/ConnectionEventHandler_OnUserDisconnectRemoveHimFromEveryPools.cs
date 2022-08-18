using Microsoft.Extensions.Logging;
using Salky.WebSockets.Contracts;

namespace Salky.WebSockets.Implementations;

public class ConnectionEventHandler_OnUserDisconnectRemoveHimFromEveryPools : IConnectionEventHandler
{
    private readonly ConnectionPoolMannager_WithClientPresenceRecording ConnectionPoolMannagerWithClientPresence;

    public ConnectionEventHandler_OnUserDisconnectRemoveHimFromEveryPools(ILogger<ConnectionEventHandler_OnUserDisconnectRemoveHimFromEveryPools> logger, IConnectionPoolMannager connectionPoolMannager)
    {
        logger.LogWarning("This class may have performance issues");
        if (connectionPoolMannager is ConnectionPoolMannager_WithClientPresenceRecording con)
        {
            this.ConnectionPoolMannagerWithClientPresence = con;
        }
        else
        {
            throw new Exception($"To use this class you must provide the {typeof(IConnectionEventHandler).FullName} as {typeof(ConnectionPoolMannager_WithClientPresenceRecording).FullName}");
        }

    }
    public async Task HandleClose(ISalkyWebSocket socket)
    {
        var usrId = socket.User.UserId;
        if (this.ConnectionPoolMannagerWithClientPresence.ClientKeyAndPoolList.TryGetValue(usrId, out var hashSet))
        {
            await Task.WhenAll(hashSet.Select(poolid => this.ConnectionPoolMannagerWithClientPresence.RemoveOneFromPool(poolid, usrId)).ToArray());
        }
    }

    public Task HandleOpen(ISalkyWebSocket socket) => Task.CompletedTask;
}
