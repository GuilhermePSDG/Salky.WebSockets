using Microsoft.Extensions.Logging;
using Salky.WebSockets.Contracts;
using Salky.WebSockets.Models;
using System.Collections.Concurrent;

namespace Salky.WebSockets.Implementations;

public class ConnectionPoolMannager : IConnectionPoolMannager
{
    private readonly ILogger<ConnectionPoolMannager>? logger;
    private readonly IConnectionMannager RootConnectionMannager;
    private readonly ConcurrentDictionary<Key, IConnectionMannager> pools;

    public ConnectionPoolMannager(ILogger<ConnectionPoolMannager>? logger, IConnectionMannager RootConnectionMannager)
    {
        this.logger = logger;
        this.RootConnectionMannager = RootConnectionMannager;
        pools = new();
        pools["root"] = this.RootConnectionMannager;
    }

    public virtual async Task<bool> AddOneInPool(Key PoolId, Key ClientKey)
    {
        var con = RootConnectionMannager.TryGetSocket(ClientKey);
        if (con == null)
            return false;
        return await pools
            .GetOrAdd(PoolId, (x) => new ConnectionMannager(PoolId))
            .AddConnection(ClientKey, con);
    }
    public virtual async Task<int> RemoveOneFromPool(Key Poolid, Key ClientKey)
    {
        if (!pools.TryGetValue(Poolid, out var pool))
            return -1;
        if (await pool.TryRemoveConnection(ClientKey) == null)
            return 0;
        RemovePoolIfEmpty(pool);
        return 1;
    }
    private void RemovePoolIfEmpty(IConnectionMannager connection)
    {
        if (connection is ConnectionMannager conM)
        {
            if (!conM.Connections.Any())
                pools.Remove(conM.Key, out _);
        }
        else
        {
            this.logger!.LogWarning("Unable to evalatue if pool are empty");
        }

    }
    public virtual async Task<int> SendToOneInPool(Key PoolId, Key ClientKey, MessageServer msg)
    {
        if (!pools.TryGetValue(PoolId, out var pool))
            return -1;
        return await pool.SendToOne(ClientKey, msg) ? 1 : 0;
    }
    public virtual async Task<int> SendToManyInPool(Key PoolId, Key[] keys, MessageServer msg)
    {
        if (!pools.TryGetValue(PoolId, out var pool))
            return -1;
        return await pool.SendToMany(keys, msg);
    }
    public virtual async Task<int> AddManyInPool(Key PoolId, Key[] ClientKeys)
    {
        var pool = pools.GetOrAdd(PoolId, (x) => new ConnectionMannager(PoolId));
        int n = 0;
        foreach (var key in ClientKeys)
        {
            var res = RootConnectionMannager.TryGetSocket(key);
            if (res != null)
            {
                await pool.AddConnection(key, res);
                n++;
            }
        }
        return n;
    }
    public virtual async Task<int> SendToAllInPool(Key PoolId, MessageServer msg)
    {
        if (!pools.TryGetValue(PoolId, out var pool))
            return -1;
        return await pool.SendToAll(msg);
    }
    public virtual async Task<bool> DeletePool(Key PoolId)
    {
        return await Task.FromResult(pools.TryRemove(PoolId, out _));
    }
    public virtual bool IsInPool(Key PoolId, Key ClientKey)
    {
        return this.pools.TryGetValue(PoolId, out var pool) && pool.ContainsKey(ClientKey);
    }
}
