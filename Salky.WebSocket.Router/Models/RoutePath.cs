using Salky.WebSockets.Enums;
using Salky.WebSockets.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Salky.WebSockets.Router.Models
{
    public class RoutePath : RoutePathBase
    {
        public string PathClass { get; }
        public string PathMethod { get; }
        public RoutePath(string PathClass, string PathMethod, Method method) : base($"{PathClass}/{PathMethod}", method)
            => (this.PathClass, this.PathMethod) = (PathClass, PathMethod);
    }
}
