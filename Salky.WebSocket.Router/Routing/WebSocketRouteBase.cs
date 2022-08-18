using Salky.WebSockets.Contracts;
using Salky.WebSockets.Enums;
using Salky.WebSockets.Models;
using Salky.WebSockets.Router.Models;
using System.Security.Claims;

namespace Salky.WebSockets.Router.Routing
{
    public class WebSocketRouteBase : IConnectionPoolMannager
    {
        public virtual Task OnConnectAsync() => Task.CompletedTask;
        public virtual Task OnDisconnectAsync() => Task.CompletedTask;

        public WebSocketRouteBase() { }

        public IConnectionPoolMannager connectionPoolMannager;
        public ISalkyWebSocket UserSocket { get; set; }
        public WebSocketUser User => UserSocket.User;
        public RoutePath CurrentRoutePath { get; private set; }
        public Method CurrentRouteMethod => CurrentRoutePath.Method;
        public string CurrentRoutePathClass => CurrentRoutePath.PathClass;
        public string CurrentPath => CurrentRoutePath.Path;
        public List<Claim> Claims => UserSocket.User.Claims;

        bool builded = false;
        internal void Constructor(ISalkyWebSocket webSocket, IConnectionPoolMannager connectionPoolMannager)
        {
            if (!builded)
            {
                this.connectionPoolMannager = connectionPoolMannager;
                UserSocket = webSocket;
                builded = true;
            }
        }

        internal void Inject(RoutePath CurrentPath)
        {
            this.CurrentRoutePath = CurrentPath;
        }
        public virtual async Task SendBack<T>(T data, string path, Method method, Status status = Status.Success) where T : notnull
        {
            await UserSocket.SendMessageServer(new MessageServer(path, method, status, data));
        }

        public virtual async Task SendErrorBack(string currentRoute, string message, Method method = Method.POST)
        {
            await SendBack(
                data: new
                {
                    message
                },
                path: currentRoute,
                method: method,
                status: Status.Error
                );
        }
        public virtual async Task<int> SendToAllInPool<T>(Key PoolKey, string Path, Method method, T data) where T : notnull
        {
            return await connectionPoolMannager.SendToAllInPool(PoolKey, new MessageServer(Path, method, Status.Success, data));
        }
        public virtual async Task<bool> SendToOneInPool<T>(Key PoolKey, Key ClientKey, string Path, Method method, T data) where T : notnull
        {
            return await connectionPoolMannager.SendToOneInPool(PoolKey, ClientKey, new MessageServer(Path, method, Status.Success, data)) > 0;
        }
        //
        public virtual async Task<bool> DeletePool(Key PoolId)
        {
            return await connectionPoolMannager.DeletePool(PoolId);
        }
        public virtual async Task<int> AddManyInPool(Key PoolId, params Key[] ClientsId)
        {
            return await connectionPoolMannager.AddManyInPool(PoolId, ClientsId);
        }
        public virtual async Task<bool> AddOneInPool(Key PoolId, Key ClientKey)
        {
            return await connectionPoolMannager.AddOneInPool(PoolId, ClientKey);
        }

        public virtual Task<int> RemoveOneFromPool(Key Poolid, Key ClientKey)
        {
            return connectionPoolMannager.RemoveOneFromPool(Poolid, ClientKey);
        }

        public Task<int> SendToOneInPool(Key PoolId, Key ClientKey, MessageServer msg)
        {
            return connectionPoolMannager.SendToOneInPool(PoolId, ClientKey, msg);
        }

        public bool IsInPool(Key PoolId, Key ClientKey)
        {
            return connectionPoolMannager.IsInPool(PoolId, ClientKey);
        }

        public Task<int> SendToManyInPool(Key PoolId, Key[] keys, MessageServer msg)
        {
            return connectionPoolMannager.SendToManyInPool(PoolId, keys, msg);
        }

        public Task<int> SendToAllInPool(Key PoolId, MessageServer msg)
        {
            return connectionPoolMannager.SendToAllInPool(PoolId, msg);
        }
    }
}
