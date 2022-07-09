using Salky.WebSockets.Enums;
using Salky.WebSockets.Models;
using Salky.WebSockets.Router.Routing;
using Salky.WebSockets.Router.Routing.Atributes;

namespace Example
{
    [WebSocketRoute]
    public class TesteRoute : WebSocketRouteBase
    {
        public record Message(Key PoolKey, string Content);
        public record ModelTest(string Name, DateTime Date)
        {
            public bool valid()
            {
                return Name != null && Name.Length > 0 && Date != DateTime.MinValue;
            }
        };
        [WsGet]
        public async Task Teste()
        {
            await SendBack("Teste ok", CurrentPath, CurrentRouteMethod);
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
        public async Task AddOnPool(Key PoolKey)
        {
            await AddOnPool(PoolKey);
        }
        [WsListener("leave")]
        public async Task RemoveFromPool(Key PoolKey)
        {
            await RemoveFromPool(PoolKey);
        }
        [WsRedirect("message")]
        public async Task RouteToAll(Message msg)
        {
            await SendToAllInPool(msg.PoolKey, "", Method.POST, msg.Content);
        }
    }
}
