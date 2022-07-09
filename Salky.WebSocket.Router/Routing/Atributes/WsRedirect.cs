using Salky.WebSockets.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Salky.WebSockets.Router.Routing.Atributes
{
    public sealed class WsRedirect : RouteMethodAtribute
    {
        public WsRedirect() : this("") { }
        public WsRedirect(string routePath) : base(routePath, Method.REDIRECT) { }
    }
}
