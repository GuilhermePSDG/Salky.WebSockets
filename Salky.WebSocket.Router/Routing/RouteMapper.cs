using Salky.WebSockets.Router.Contracts;
using Salky.WebSockets.Router.Exceptions;
using Salky.WebSockets.Router.Extensions;
using Salky.WebSockets.Router.Models;
using Salky.WebSockets.Router.Routing.Atributes;
using System.Reflection;

namespace Salky.WebSockets.Router.Routing
{
    public class RouteMapper : IRouteMapper
    {
        public IEnumerable<RouteInfo> MapRouteInfo()
        {
            Dictionary<string, RouteInfo> MappedRoutes = new();
            foreach (var @class in AllWebSocketRoutesClass)
            {
                foreach (var method in @class.GetMethodsWithAttribute<RouteMethodAttribute>())
                {
                    RouteInfo routeInfo = CreateRouteInfo(@class, method);
                    var key = routeInfo.RoutePath.GenRouteKey();
                    if (MappedRoutes.ContainsKey(key))
                        throw new DuplicatedRouteKeyException($"Duplicated WebSocoket Route, Class : {@class.FullName} , Method : {method.Name}");
                    MappedRoutes.Add(key, routeInfo);
                }
            }
            return MappedRoutes.Values;
        }
        public IEnumerable<Type> MapWsRoutes() => AllWebSocketRoutesClass;

        public static IEnumerable<Type> AllWebSocketRoutesClass =>
            ReflectionExtensions
            .GetAllTypesInCurrentAssembly(x => x.GetCustomAttribute(typeof(WebSocketRoute)) != null && x.IsAssignableTo(typeof(WebSocketRouteBase)));

        private RouteInfo CreateRouteInfo(Type @class, MethodInfo method)
        {
            var parameters = method.GetParameters();
            var path = GenerateRoutePath(@class, method);
            return new RouteInfo(
                methodInfo: method,
                classType: @class,
                routePath: path,
                parameterType: parameters
                );
        }

        private RoutePath GenerateRoutePath(Type @class, MethodInfo method)
        {
            var methodAtribute = method.GetRequiredAtribute<RouteMethodAttribute>();
            var classAtribute = @class.GetRequiredAtribute<WebSocketRoute>();
            var classPath = classAtribute.routeName ?? @class.Name.ToLower().Split("route")[0];
            return new RoutePath(classPath, methodAtribute.routePath, methodAtribute.routeMethod);
        }


    }

}
