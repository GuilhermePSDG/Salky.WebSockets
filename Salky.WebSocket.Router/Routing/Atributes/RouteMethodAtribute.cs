using Salky.WebSockets.Enums;

namespace Salky.WebSockets.Router.Routing.Atributes
{
    [AttributeUsage(AttributeTargets.Method)]
    public class RouteMethodAtribute : Attribute
    {
        public string routePath { get; set; }
        public Method routeMethod { get; set; }

        public RouteMethodAtribute(string routePath, Method routeMethod)
        {
            this.routePath = routePath;
            this.routeMethod = routeMethod;
        }
    }
}
