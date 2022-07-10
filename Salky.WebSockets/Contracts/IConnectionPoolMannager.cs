using Salky.WebSockets.Models;

namespace Salky.WebSockets.Contracts;
public interface IConnectionPoolMannager
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="PoolId"></param>
    /// <param name="ClientKey"></param>
    /// <returns><see langword="true"/> if added, <see langword="false"/> if alredy present</returns>
    public Task<bool> AddOneInPool(Key PoolId, Key ClientKey);
    /// <summary>
    /// If the key is already present, will not add
    /// </summary>
    /// <param name="PoolId"></param>
    /// <param name="ClientKeys"></param>
    /// <returns>The amount of connections added</returns>
    public Task<int> AddManyInPool(Key PoolId, Key[] ClientKeys);
    /// <summary>
    /// 
    /// </summary>
    /// <param name="Poolid"></param>
    /// <param name="ClientKey"></param>
    /// <returns>-1 if pool not exist, 0 if client is not present, 1 if removed</returns>
    public Task<int> RemoveOneFromPool(Key Poolid, Key ClientKey);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="PoolId"></param>
    /// <param name="ClientKey"></param>
    /// <param name="msg"></param>
    /// <returns>-1 if pool not exist , 0 if client is not present, 1 if sended</returns>
    public Task<int> SendToOne(Key PoolId, Key ClientKey, MessageServer msg);
    bool IsInPool(Key PoolId, Key ClientKey);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="PoolId"></param>
    /// <param name="keys"></param>
    /// <param name="msg"></param>
    /// <returns>-1 if pool not exist or the total of clients received</returns>
    public Task<int> SendToMany(Key PoolId, Key[] keys, MessageServer msg);
    /// <summary>
    /// 
    /// </summary>
    /// <param name="PoolId"></param>
    /// <param name="msg"></param>
    /// <returns>The ammount of messages sended</returns>
    public Task<int> SendToAll(Key PoolId, MessageServer msg);
    /// <summary>
    /// 
    /// </summary>
    /// <param name="PoolId"></param>
    /// <returns>true if removed otherwise false</returns>
    public Task<bool> DeletePool(Key PoolId);

}
