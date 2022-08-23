using Salky.WebSockets.Enums;
using Salky.WebSockets.Router.Routing;
using Salky.WebSockets.Router.Routing.Atributes;

namespace Example.WsRoutes
{
    [WebSocketRoute]
    public class TesteRoute : WebSocketRouteBase
    {

        public override async Task OnConnectAsync()
        {
            await SendBack("Hi", "connected", Method.POST);
            await base.OnConnectAsync();
        }



        [WsPost("ping")]
        public async Task Ping()
        {
            await SendBack("pong", CurrentPath, CurrentRouteMethod);
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
            await AddOneInPool(PoolKey, User.UserId);
        }

        [WsRedirect("message/all")]
        public async Task RouteToAllClients(object objt)
        {
            var msg = new
            {
                sender = new
                {
                    Id = User.UserId,
                    Name = User.Claims.FirstOrDefault(x => x.Type == "name")?.Value ?? "Unknow",
                },
                data = objt,
            };
            await SendToAllInPool("root", CurrentPath, CurrentRouteMethod, msg);
        }
        [WsListener("leave")]
        public async Task RemoveFromPool(string PoolKey)
        {
            await RemoveOneFromPool(PoolKey, User.UserId);
        }
        [WsRedirect("message")]
        public async Task RouteToAllInPool(Message msg)
        {
            await SendToAllInPool(msg.PoolKey, "", Method.POST, msg.Content);
        }

    }
    public record Message(string PoolKey, string Content);
    public record ModelTest(string Name, DateTime Date)
    {
        public bool valid()
        {
            return Name != null && Name.Length > 0 && Date != DateTime.MinValue;
        }
    };

}
