using Salky.WebSockets.Router.Contracts;
using Salky.WebSockets.Router.Models;

namespace Salky.WebSockets.Router.Routing;

public class SingletonRouteList : IRouteList
{
    private static Dictionary<string, RouteInfo> routes;
    private static List<Type> RoutesClass { get; set; }

    public SingletonRouteList(IRouteMapper routeMapper)
    {
        if (routes == null)
        {
            routes = new(routeMapper.MapRouteInfo().ToDictionary(x => x.RoutePath.GenRouteKey(), x => x));
            RoutesClass = routeMapper.MapWsRoutes().ToList();
        }
    }
    public RouteInfo? Find(string Key) =>
        routes.TryGetValue(Key, out var route) ? route : null;
    public IEnumerable<RouteInfo> GetAllRoutes() => routes.Values;

    public IEnumerable<Type> GetAllWsClass() => RoutesClass;



}
