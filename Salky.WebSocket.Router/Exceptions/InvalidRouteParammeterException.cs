using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Salky.WebSockets.Router.Exceptions
{
    public class InvalidRouteParammeterException : Exception
    {
        public InvalidRouteParammeterException(string message) : base(message)
        {

        }
    }
}
