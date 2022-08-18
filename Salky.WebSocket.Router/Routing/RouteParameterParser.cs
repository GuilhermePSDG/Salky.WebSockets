using Salky.WebSockets.Models;
using Salky.WebSockets.Router.Contracts;
using Salky.WebSockets.Router.Exceptions;
using Salky.WebSockets.Router.Models;

namespace Salky.WebSockets.Router.Routing
{
    public class RouteParameterParser : IRouteParametersParser
    {
        public object[] Parse(RouteInfo route, MessageServer messageServer)
        {
            if (route.Parameters == null || route.Parameters.Length == 0) return new object[0];
            var param = route.Parameters[0];
            var paramType = param.ParameterType;
            if (paramType == typeof(MessageServer))
            {
                return new object[] { messageServer };
            }
            switch (messageServer.Data)
            {

                case JsonElement jsonElement:
                    return new object[] { JsonElementParser(jsonElement, paramType) };
                case string json:
                    return new object[] { JsonStringParser(json, paramType) };
                default:
                    throw new InvalidRouteParammeterException("Unregonized route parammeter.");
            }
        }
        private object JsonElementParser(JsonElement element, Type targetType)
        {
            return element.Deserialize(targetType, DefaultJsonSerializerOptions) ?? throw new InvalidRouteParammeterException($"Cannot serialize message into {targetType.Name}");
        }
        private object JsonStringParser(string Json, Type targetType)
        {
            return JsonSerializer.Deserialize(Json, targetType, DefaultJsonSerializerOptions) ?? throw new InvalidRouteParammeterException($"Cannot serialize message into {targetType.Name}");
        }
    }

}
