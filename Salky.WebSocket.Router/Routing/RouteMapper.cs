﻿using System.Reflection;
using Salky.WebSockets.Router.Models;
using Salky.WebSockets.Router.Routing.Atributes;
using Salky.WebSockets.Router.Contracts;
using Salky.WebSockets.Router.Extensions;
using Salky.WebSockets.Router.Exceptions;

namespace Salky.WebSockets.Router.Routing
{
    public class RouteMapper : IRouteMapper
    {
        public List<RouteInfo> Map()
        {
            Dictionary<string, RouteInfo> InRoutes = new();
            foreach (var @class in AllWebSocketRoutesClass)
            {
                foreach (var method in @class.GetMethodsWithAtribute<RouteMethodAtribute>())
                {
                    RouteInfo routeInfo = CreateRouteInfo(@class, method);
                    var key = routeInfo.RoutePath.GenRouteKey();
                    if (InRoutes.ContainsKey(key))
                        throw new DuplicatedRouteKeyException($"Duplicated WebSocoket Route, Class : {@class.FullName} , Method : {method.Name}");
                    InRoutes.Add(key, routeInfo);
                }
            }
            return InRoutes.Values.ToList();
        }
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
            var methodAtribute = method.GetRequiredAtribute<RouteMethodAtribute>();
            var classAtribute = @class.GetRequiredAtribute<WebSocketRoute>();
            var classPath = classAtribute.routeName ?? @class.Name.ToLower().Split("route")[0];
            return new RoutePath(classPath, methodAtribute.routePath, methodAtribute.routeMethod);
        }



    }

}
