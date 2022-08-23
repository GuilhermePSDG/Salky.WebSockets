using Salky.WebSockets.Enums;
using Salky.WebSockets.Router.Routing;
using Salky.WebSockets.Router.Routing.Atributes;

namespace Example.WsRoutes
{
    [WebSocketRoute]
    public class AnotherRoute : WebSocketRouteBase
    {
        public override async Task OnConnectAsync()
        {
            await SendBack("Hi 2 2 ", "connected", Method.POST);
            await base.OnConnectAsync();
        }
    }
}
