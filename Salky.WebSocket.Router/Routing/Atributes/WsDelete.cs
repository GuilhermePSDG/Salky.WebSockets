using Salky.WebSockets.Enums;

namespace Salky.WebSockets.Router.Routing.Atributes
{
    public sealed class WsDelete : RouteMethodAtribute
    {
        public WsDelete() : this("") { }
        public WsDelete(string routePath) : base(routePath, Method.DELETE) { }
    }
}
