using Salky.WebSockets.Enums;

namespace Salky.WebSockets.Router.Routing.Atributes
{
    public sealed class WsListener : RouteMethodAtribute
    {
        public WsListener() : this("") { }
        public WsListener(string routePath) : base(routePath, Method.LISTENER) { }
    }

}
