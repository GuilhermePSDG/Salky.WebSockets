using Salky.WebSockets.Router.Models;

namespace Salky.WebSockets.Router.Contracts
{
    public interface IRouteList
    {
        public RouteInfo? Find(string Key);
        public IEnumerable<RouteInfo> GetAll();
    }

}
