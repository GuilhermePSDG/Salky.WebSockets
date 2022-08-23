using Salky.WebSockets.Router.Models;

namespace Salky.WebSockets.Router.Contracts
{
    public interface IRouteMapper
    {
        public IEnumerable<RouteInfo> MapRouteInfo();
        public IEnumerable<Type> MapWsRoutes();

    }
}
