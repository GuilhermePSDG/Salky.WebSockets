using Salky.WebSockets.Enums;

namespace Salky.WebSockets.Router.Routing.Atributes
{
    [AttributeUsage(AttributeTargets.Method)]
    public class RouteMethodAttribute : Attribute
    {
        public string? routePath { get; set; } = null;
        public Method routeMethod { get; set; }
        public RouteMethodAttribute(Method routeMethod):this(null,routeMethod)
        {

        }
        public RouteMethodAttribute(string? routePath, Method routeMethod)
        {
            this.routePath = routePath;
            this.routeMethod = routeMethod;
        }
    }
}
