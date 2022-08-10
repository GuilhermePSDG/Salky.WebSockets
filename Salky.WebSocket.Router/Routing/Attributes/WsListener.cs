using Salky.WebSockets.Enums;

namespace Salky.WebSockets.Router.Routing.Atributes
{
    public sealed class WsListener : RouteMethodAttribute
    {
        public WsListener() : base(Method.LISTENER) { }
        public WsListener(string routePath) : base(routePath, Method.LISTENER) { }
    }

}
