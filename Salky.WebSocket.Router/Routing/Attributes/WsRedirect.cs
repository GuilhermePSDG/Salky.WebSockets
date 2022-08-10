using Salky.WebSockets.Enums;

namespace Salky.WebSockets.Router.Routing.Atributes
{
    public sealed class WsRedirect : RouteMethodAttribute
    {
        public WsRedirect() : base(Method.REDIRECT) { }
        public WsRedirect(string routePath) : base(routePath, Method.REDIRECT) { }
    }
}
