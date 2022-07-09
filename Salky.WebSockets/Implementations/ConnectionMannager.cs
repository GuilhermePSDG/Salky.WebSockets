// configure
using Salky.WebSockets.Contracts;
using Salky.WebSockets.Models;
using System.Collections.Concurrent;

namespace Salky.WebSockets.Implementations;
public class ConnectionMannager : IConnectionMannager
{
    public Key Key { get; }
    public ConnectionMannager(Key Key)
    {
        this.Key = Key;
    }
    public ConnectionMannager()
    {
        Key = "root";
    }
    public ConcurrentDictionary<Key, SalkyWebSocket> Connections = new();
    public Task<bool> AddConnection(Key key, SalkyWebSocket socket)
    {
        return Task.FromResult(Connections.TryAdd(key, socket));
    }
    public async Task<SalkyWebSocket?> TryRemoveConnection(Key key)
    {
        if (!Connections.TryRemove(key, out var sock))
            return null;
        return await Task.FromResult(sock);
    }
    public SalkyWebSocket? TryGetSocket(Key Key)
    {
        Connections.TryGetValue(Key, out var sock);
        return sock;
    }
    public bool ContainsKey(Key ClientKey) => Connections.ContainsKey(ClientKey);
    public async Task<int> SendToMany(Key[] keys, MessageServer msg)
    {
        var tasks = new List<Task>(keys.Length);
        foreach (var key in keys)
            if (Connections.TryGetValue(key, out var sock))
                tasks.Add(sock.SendMessageServer(msg));
        await Task.WhenAll(tasks);
        return tasks.Count;
    }
    public async Task<bool> SendToOne(Key ClientKey, MessageServer msg)
    {
        if (!Connections.TryGetValue(ClientKey, out var sock))
            return false;
        await sock.SendMessageServer(msg);
        return true;
    }

    public async Task<int> SendToAll(MessageServer msg)
    {
        foreach (var con in Connections.Values)
            await con.SendMessageServer(msg);
        return Connections.Count;
    }

}
