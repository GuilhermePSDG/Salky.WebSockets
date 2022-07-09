using Salky.WebSockets.Enums;
using Salky.WebSockets.Models;

namespace Salky.WebSockets.Router.Models
{
    public class RoutePath : RoutePathBase
    {
        public string PathClass { get; }
        public string PathMethod { get; }
        public RoutePath(string PathClass, string PathMethod, Method method) : base($"{PathClass.Trim('/', ' ')}/{PathMethod.Trim('/', ' ')}".Trim('/', ' ').ToLower(), method)
            => (this.PathClass, this.PathMethod) = (PathClass, PathMethod);
    }
}
