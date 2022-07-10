using Salky.WebSockets.Enums;

namespace Salky.WebSockets.Router.Routing.Atributes
{
    [AttributeUsage(AttributeTargets.Method)]
    public class RouteMethodAtribute : Attribute
    {
        public string? routePath { get; set; } = null;
        public Method routeMethod { get; set; }
        public RouteMethodAtribute(Method routeMethod):this(null,routeMethod)
        {

        }
        public RouteMethodAtribute(string? routePath, Method routeMethod)
        {
            this.routePath = routePath;
            this.routeMethod = routeMethod;
        }
    }
}
