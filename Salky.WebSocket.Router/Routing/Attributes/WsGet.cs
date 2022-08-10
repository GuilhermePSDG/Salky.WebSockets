using Salky.WebSockets.Enums;

namespace Salky.WebSockets.Router.Routing.Atributes
{
    public sealed class WsGet : RouteMethodAttribute
    {
        public WsGet() : base(Method.GET) { }
        public WsGet(string routePath) : base(routePath, Method.GET) { }
    }

}
