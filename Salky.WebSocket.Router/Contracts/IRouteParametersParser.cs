using Salky.WebSockets.Models;
using Salky.WebSockets.Router.Models;

namespace Salky.WebSockets.Router.Contracts
{
    public interface IRouteParametersParser
    {
        public object[] Parse(RouteInfo route, MessageServer messageServer);
    }

}
