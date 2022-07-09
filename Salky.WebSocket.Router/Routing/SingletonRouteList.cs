using Salky.WebSockets.Router.Contracts;
using Salky.WebSockets.Router.Models;
using System.Collections.Concurrent;

namespace Salky.WebSockets.Router.Routing;

public class SingletonRouteList : IRouteList
{
    private static Dictionary<string, RouteInfo> routes;
    public SingletonRouteList(IRouteMapper routeMapper)
    {
        if (routes == null) routes = new(routeMapper.Map().ToDictionary(x => x.RoutePath.GenRouteKey(), x => x));
    }
    public RouteInfo? Find(string Key) =>
        routes.TryGetValue(Key, out var route) ? route : null;
    public IEnumerable<RouteInfo> GetAll() => routes.Values;
}
