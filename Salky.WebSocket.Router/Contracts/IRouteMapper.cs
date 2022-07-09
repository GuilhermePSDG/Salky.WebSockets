using Salky.WebSockets.Router.Models;

namespace Salky.WebSockets.Router.Contracts
{
    public interface IRouteMapper
    {
        public List<RouteInfo> Map();
    }
}
