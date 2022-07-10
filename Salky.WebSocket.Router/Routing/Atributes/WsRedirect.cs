using Salky.WebSockets.Enums;

namespace Salky.WebSockets.Router.Routing.Atributes
{
    public sealed class WsRedirect : RouteMethodAtribute
    {
        public WsRedirect() : base(Method.REDIRECT) { }
        public WsRedirect(string routePath) : base(routePath, Method.REDIRECT) { }
    }
}
