using Microsoft.Extensions.Logging;
using Salky.WebSockets.Contracts;
using Salky.WebSockets.Models;

namespace Salky.WebSockets.Implementations;

public class ConnectionPoolMannager_WithClientPresenceRecording : ConnectionPoolMannager, IConnectionPoolMannager
{
    public ConnectionPoolMannager_WithClientPresenceRecording(ILogger<ConnectionPoolMannager> logger, IConnectionMannager RootConnectionMannager) : base(logger, RootConnectionMannager)
    {
        logger.LogWarning("This class may have performance issues");
    }

    public Dictionary<string, HashSet<string>> ClientKeyAndPoolList = new();

    private void _tryaddpoolToClient(Key PoolId, Key ClientKey)
    {
        if (!ClientKeyAndPoolList.TryGetValue(ClientKey, out var hashSet))
        {
            ClientKeyAndPoolList[ClientKey] = new() { PoolId };
        }
        else
        {
            hashSet.Add(PoolId);
        }
    }
    private void _tryremovepoolToClient(Key PoolId, Key ClientKey)
    {
        if (ClientKeyAndPoolList.TryGetValue(ClientKey, out var hashSet))
        {
            hashSet.Remove(PoolId);
        }
    }

    public override async Task<bool> AddOneInPool(Key PoolId, Key ClientKey)
    {
        var added = await base.AddOneInPool(PoolId, ClientKey);
        if (added)
        {
            _tryaddpoolToClient(PoolId, ClientKey);
        }
        return added;
    }

    public override async Task<int> RemoveOneFromPool(Key Poolid, Key ClientKey)
    {
        var result = await base.RemoveOneFromPool(Poolid, ClientKey);
        if (result == 1)
        {
            _tryremovepoolToClient(Poolid, ClientKey);
        }
        return result;
    }

    public override async Task<int> AddManyInPool(Key PoolId, Key[] ClientKeys)
    {
        var result = await base.AddManyInPool(PoolId, ClientKeys);
        if (result > 0)
        {
            foreach (var clientKey in ClientKeys)
            {
                _tryaddpoolToClient(PoolId, clientKey);
            }
        }
        return result;
    }


    public override async Task<bool> DeletePool(Key PoolId)
    {
        var deleted = await base.DeletePool(PoolId);
        if (deleted)
        {
            foreach (var x in ClientKeyAndPoolList.Values)
            {
                x.Remove(PoolId);
            }
        }
        return deleted;
    }
}
