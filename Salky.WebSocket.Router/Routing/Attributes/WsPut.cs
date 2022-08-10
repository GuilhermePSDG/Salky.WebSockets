using Salky.WebSockets.Enums;

namespace Salky.WebSockets.Router.Routing.Atributes
{
    public sealed class WsPut : RouteMethodAttribute
    {
        public WsPut() : base(Method.PUT) { }
        public WsPut(string routePath) : base(routePath, Method.PUT) { }
    }

}
