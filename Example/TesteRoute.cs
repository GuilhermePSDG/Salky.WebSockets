using Salky.WebSockets.Enums;
using Salky.WebSockets.Router.Routing;
using Salky.WebSockets.Router.Routing.Atributes;

namespace Example
{
    [WebSocketRoute]
    public class TesteRoute : WebSocketRouteBase
    {
        public record Message(string PoolKey, string Content);
        public record ModelTest(string Name, DateTime Date)
        {
            public bool valid()
            {
                return Name != null && Name.Length > 0 && Date != DateTime.MinValue;
            }
        };
        [WsPost("ping")]
        public async Task Ping()
        {
            await SendBack("Pong", CurrentPath, CurrentRouteMethod);
        }

        [WsPost("data")]
        public async Task Teste(ModelTest teste)
        {
            if (teste != null && teste.valid())
            {
                await SendBack(teste, CurrentPath, CurrentRouteMethod);
            }
        }

        [WsListener("entry")]
        public async Task AddOnPool(string PoolKey)
        {
            await base.AddOneInPool(PoolKey, User.UserId);
        }
        [WsListener("leave")]
        public async Task RemoveFromPool(string PoolKey)
        {
            await base.RemoveOneFromPool(PoolKey, User.UserId);
        }
        [WsRedirect("message")]
        public async Task RouteToAll(Message msg)
        {
            await SendToAllInPool(msg.PoolKey, "", Method.POST, msg.Content);
        }
    }
}
