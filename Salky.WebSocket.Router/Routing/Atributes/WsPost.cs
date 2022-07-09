using Salky.WebSockets.Enums;

namespace Salky.WebSockets.Router.Routing.Atributes
{
    public sealed class WsPost : RouteMethodAtribute
    {
        public WsPost() : this("") { }
        public WsPost(string routePath) : base(routePath, Method.POST) { }
    }

}
