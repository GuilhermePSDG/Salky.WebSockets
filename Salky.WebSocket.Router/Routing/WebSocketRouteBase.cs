
using Microsoft.Extensions.DependencyInjection;
using Salky.WebSockets.Contracts;
using Salky.WebSockets.Enums;
using Salky.WebSockets.Models;
using Salky.WebSockets.Router.Models;
using System.Security.Claims;

namespace Salky.WebSockets.Router.Routing
{
    public class WebSocketRouteBase
    {
        public virtual Task OnConnectAsync() => Task.CompletedTask;
        public virtual Task OnDisconnectAsync() => Task.CompletedTask;

        public WebSocketRouteBase() { }

        public IConnectionPoolMannager connectionPoolMannager;

        public SalkyWebSocket UserSocket { get; set; }
        public WebSocketUser User => UserSocket.User;
        public IStorage Storage => UserSocket.User.Storage;
        public Method CurrentRouteMethod { get; private set; }
        public string CurrentRoutePathClass { get; private set; }
        public string CurrentPath = "";
        public List<Claim> Claims => UserSocket.User.Claims;

        bool builded = false;
        internal void Constructor(SalkyWebSocket webSocket, IConnectionPoolMannager connectionPoolMannager)
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
            CurrentRouteMethod = CurrentPath.Method;
            CurrentRoutePathClass = CurrentPath.PathClass;
            this.CurrentPath = CurrentPath.Path;
        }
        public async Task SendBack<T>(T data, string path, Method method, Status status = Status.Success) where T : notnull
        {
            await UserSocket.SendMessageServer(new MessageServer(path, method, status, data));
        }

        public async Task SendErrorBack(string currentRoute, string message, Method method = Method.POST)
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

        public async Task<bool> DeletePool(Key PoolId)
        {
            return await connectionPoolMannager.DeletePool(PoolId);
        }
        public async Task<int> AddManyInPool(Key PoolId, params Key[] ClientsId)
        {
            return await connectionPoolMannager.AddManyInPool(PoolId, ClientsId);
        }
        public async Task<bool> AddOneInPool(Key PoolId, Key ClientKey)
        {
            return await connectionPoolMannager.AddOneInPool(PoolId, ClientKey);
        }
        public async Task<bool> RemoveOneFromPool(Key PoolKey, Key ClientKey)
        {
            return await connectionPoolMannager.RemoveOneFromPool(PoolKey, ClientKey) > 0;
        }
        public async Task<int> SendToAllInPool<T>(Key PoolKey, string Path, Method method, T data) where T : notnull
        {
            return await connectionPoolMannager.SendToAll(PoolKey, new MessageServer(Path, method, Status.Success, data));
        }
        public async Task<bool> SendToOneInPool<T>(Key PoolKey, Key ClientKey, string Path, Method method, T data) where T : notnull
        {
            return await connectionPoolMannager.SendToOne(PoolKey, ClientKey, new MessageServer(Path, method, Status.Success, data)) > 0;
        }


    }
}
