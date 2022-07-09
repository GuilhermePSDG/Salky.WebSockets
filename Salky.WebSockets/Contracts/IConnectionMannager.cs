using Salky.WebSockets.Models;

namespace Salky.WebSockets.Contracts;

public interface IConnectionMannager
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="ClientKey"></param>
    /// <returns>true if is present, otherwise false</returns>
    public bool ContainsKey(Key ClientKey);
    /// <summary>
    /// </summary>
    /// <param name="ClientKey"></param>
    /// <returns><see langword="null"/> if not found</returns>
    public Task<SalkyWebSocket?> TryRemoveConnection(Key ClientKey);
    /// <summary>
    /// 
    /// </summary>
    /// <param name="ClientKey"></param>
    /// <param name="socket"></param>
    /// <returns>true if added, false if is already present</returns>
    public Task<bool> AddConnection(Key ClientKey, SalkyWebSocket socket);
    /// <summary>
    /// 
    /// </summary>
    /// <param name="ClientKey"></param>
    /// <param name="msg"></param>
    /// <returns>true if sended, otherwise false</returns>
    public Task<bool> SendToOne(Key ClientKey, MessageServer msg);
    /// <summary>
    /// 
    /// </summary>
    /// <param name="keys"></param>
    /// <param name="msg"></param>
    /// <returns>the total sendedthe total sended</returns>
    public Task<int> SendToMany(Key[] keys, MessageServer msg);
    /// <summary>
    /// 
    /// </summary>
    /// <param name="msg"></param>
    /// <returns>the total sended</returns>
    public Task<int> SendToAll(MessageServer msg);
    internal SalkyWebSocket? TryGetSocket(Key Key);

}
