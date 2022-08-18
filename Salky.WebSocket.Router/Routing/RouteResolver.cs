using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Salky.WebSockets.Contracts;
using Salky.WebSockets.Enums;
using Salky.WebSockets.Models;
using Salky.WebSockets.Router.Contracts;
using Salky.WebSockets.Router.Models;

namespace Salky.WebSockets.Router.Routing
{
    public class RouteResolver : IMessageHandler, IConnectionEventHandler
    {
        public ILogger<RouteResolver> logger { get; }
        public IRouteList routeList { get; }
        public IRouteParametersParser parameterParser { get; }
        public IServiceProvider serviceProvider { get; }

        public RouteResolver(
            ILogger<RouteResolver> logger,
            IRouteList routeFinder,
            IRouteParametersParser parameterParser,
            IServiceProvider serviceProvider)
        {
            this.logger = logger;
            routeList = routeFinder;
            this.parameterParser = parameterParser;
            this.serviceProvider = serviceProvider;
        }
        public async Task Route(ISalkyWebSocket connectionWs, MessageServer msg)
        {
            var found = routeList.Find(msg.GenRouteKey());
            if (!found.HasValue)
            {
                await connectionWs.SendErrorAsync($"Cannot resolve route for path : {msg.Path} and method : {msg.Method}", "error");
                return;
            }
            var route = found.Value;
            object[] parameters;
            try
            {
                parameters = parameterParser.Parse(route, msg);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Erro while parsing message data");
                await connectionWs.SendErrorAsync(ex.Message, "error", Method.POST);
                return;
            }
            await ExecuteRoute(connectionWs, route, parameters);
        }

        public async Task HandleOpen(ISalkyWebSocket socket)
        {
            await ForEachWebSocketRouteBase(socket, (route) => route.OnConnectAsync());
        }

        public async Task HandleClose(ISalkyWebSocket socket)
        {
            await ForEachWebSocketRouteBase(socket, (route) => route.OnDisconnectAsync());
        }
        public async Task HandleBinary(ISalkyWebSocket socket, MemoryStream data) => await HandleText(socket, data);
        public async Task HandleText(ISalkyWebSocket socket, MemoryStream data)
        {
            var str = await new StreamReader(data).ReadToEndAsync();
            MessageServer msg;
            try
            {
                msg = JsonSerializer.Deserialize<MessageServer>(str, DefaultJsonSerializerOptions) ?? throw new Exception("Invalid json message");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Invalid json message");
                await socket.SendErrorAsync("Invalid json message", "error");
                return;
            }
            try
            {
                await Route(socket, msg);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "");
                await socket.SendErrorAsync("Routing error", "error");
                return;
            }
        }

        private async Task ForEachWebSocketRouteBase(ISalkyWebSocket ws, Func<WebSocketRouteBase, Task> act)
        {
            foreach (var route in routeList.GetAll())
            {
                await act((WebSocketRouteBase)GetRouteInstance(ws, route));
            }
        }

        private async Task ExecuteRoute(ISalkyWebSocket connectionWs, RouteInfo route, object[] parameters)
        {
            object objtIstance = GetRouteInstance(connectionWs, route);
            var returned = route.Execute(objtIstance, parameters);
            await WaitIfIsTask(returned);
        }

        private object GetRouteInstance(ISalkyWebSocket connectionWs, RouteInfo route)
        {
            var objtIstance = serviceProvider.GetRequiredService(route.ClassType);
            var instanceCast = (WebSocketRouteBase)objtIstance;
            instanceCast.Constructor(connectionWs, serviceProvider.GetRequiredService<IConnectionPoolMannager>());
            instanceCast.Inject(route.RoutePath);
            return objtIstance;
        }

        private async Task WaitIfIsTask(object? value)
        {
            if (value != null)
            {
                var returnType = value.GetType();
                if (returnType.BaseType != null && returnType.BaseType.Equals(typeof(Task)))
                {
                    await (Task)value;
                }
            }
        }


    }
}
