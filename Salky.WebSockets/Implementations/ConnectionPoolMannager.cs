using Salky.WebSockets.Contracts;
using Salky.WebSockets.Models;
using System.Collections.Concurrent;

namespace Salky.WebSockets.Implementations;

public class ConnectionPoolMannager : IConnectionPoolMannager
{
    private readonly IConnectionMannager RootConnectionMannager;
    private readonly ConcurrentDictionary<Key, ConnectionMannager> pools;

    public ConnectionPoolMannager(IConnectionMannager RootConnectionMannager)
    {
        this.RootConnectionMannager = RootConnectionMannager;
        pools = new();
    }

    public async Task<bool> AddOneInPool(Key PoolId, Key ClientKey)
    {
        var con = RootConnectionMannager.TryGetSocket(PoolId);
        if (con == null)
            return false;
        return await pools
            .GetOrAdd(PoolId, (x) => new ConnectionMannager(PoolId))
            .AddConnection(ClientKey, con);
    }
    public async Task<int> RemoveOneFromPool(Key Poolid, Key ClientKey)
    {
        if (!pools.TryGetValue(Poolid, out var pool))
            return -1;
        if (await pool.TryRemoveConnection(ClientKey) == null)
            return 0;
        RemovePoolIfEmpty(pool);
        return 1;
    }

    private void RemovePoolIfEmpty(ConnectionMannager connection)
    {
        if (connection.Connections.Count == 0)
        {
            pools.Remove(connection.Key, out _);
        }
    }

    public async Task<int> SendToOne(Key PoolId, Key ClientKey, MessageServer msg)
    {
        if (!pools.TryGetValue(PoolId, out var pool))
            return -1;
        return await pool.SendToOne(ClientKey, msg) ? 1 : 0;
    }

    public async Task<int> SendToMany(Key PoolId, Key[] keys, MessageServer msg)
    {
        if (!pools.TryGetValue(PoolId, out var pool))
            return -1;
        return await pool.SendToMany(keys, msg);
    }

    public async Task<int> AddManyInPool(Key PoolId, Key[] ClientKeys)
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

    public async Task<int> SendToAll(Key PoolId, MessageServer msg)
    {
        if (!pools.TryGetValue(PoolId, out var pool))
            return -1;
        return await pool.SendToAll(msg);
    }

    public async Task<bool> DeletePool(Key PoolId)
    {
        return await Task.FromResult(pools.TryRemove(PoolId, out _));
    }
}
