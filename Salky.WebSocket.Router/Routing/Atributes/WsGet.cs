using Salky.WebSockets.Enums;

namespace Salky.WebSockets.Router.Routing.Atributes
{
    public sealed class WsGet : RouteMethodAtribute
    {
        public WsGet() : this("") { }
        public WsGet(string routePath) : base(routePath, Method.GET) { }
    }

}
