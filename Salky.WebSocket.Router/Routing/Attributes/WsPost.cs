using Salky.WebSockets.Enums;

namespace Salky.WebSockets.Router.Routing.Atributes
{
    public sealed class WsPost : RouteMethodAttribute
    {
        public WsPost() : base(Method.POST) { }
        public WsPost(string routePath) : base(routePath, Method.POST) { }
    }

}
