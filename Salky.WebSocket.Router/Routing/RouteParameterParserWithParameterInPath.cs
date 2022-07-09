using Salky.WebSockets.Models;
using Salky.WebSockets.Router.Contracts;
using Salky.WebSockets.Router.Models;

namespace Salky.WebSockets.Router.Routing
{
    public class RouteParameterParserWithParameterInPath : IRouteParametersParser
    {
        public object[] Parse(RouteInfo route, MessageServer messageServer)
        {
            throw new NotImplementedException();
        }

        private string? MatchDoubleBracket(string path)
        {
            var arr = path.ToCharArray();
            int start = -1;
            int end = -1;
            for (int i = 0; i < arr.Length; i++)
            {
                if (start != -1)
                {
                    if (arr[i] == '}')
                    {
                        end = i;
                        break;
                    }
                }
                if (arr[i] == '{')
                {
                    if (start != -1) return null;
                    start = i + 1;
                }
            }
            return end != -1 ? new string(arr[start..end]) : null;
        }


    }

}
