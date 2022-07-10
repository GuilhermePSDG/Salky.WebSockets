using Salky.WebSockets.Enums;
using Salky.WebSockets.Models;

namespace Salky.WebSockets.Router.Models
{
    public class RoutePath : RoutePathBase
    {
        public string PathClass { get; }
        public string? PathMethod { get; }
        public RoutePath(string PathClass, string? PathMethod, Method method) : base(Join(PathClass,PathMethod), method)
        {
            (this.PathClass, this.PathMethod) = (PathClass, PathMethod);
        }
        private static string Join(string PathClass, string? PathMethod)
        {
            return $"{PathClass}{(PathMethod == null ? "" : $"/{PathMethod}")}";
        }
    }
}
