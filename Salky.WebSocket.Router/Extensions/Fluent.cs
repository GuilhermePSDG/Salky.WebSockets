using Microsoft.Extensions.DependencyInjection;
using Salky.WebSockets.Contracts;
using Salky.WebSockets.Fluent;
using Salky.WebSockets.Router.Contracts;
using Salky.WebSockets.Router.Routing;

namespace Salky.WebSockets.Router.Extensions;



public static class Fluent
{
    public static void UseRouter(this ISalkyOptions salkyWebSocketBuilder)
    {
        foreach (var @class in RouteMapper.AllWebSocketRoutesClass)
            salkyWebSocketBuilder.Services.AddScoped(@class);
        salkyWebSocketBuilder.Services
            .AddScoped<IRouteList, SingletonRouteList>()
            .AddScoped<IMessageHandler, RouteResolver>()
            .AddScoped<IConnectionEventHandler, RouteResolver>()
            .AddScoped<IRouteMapper, RouteMapper>()
            .AddScoped<IRouteParametersParser, RouteParameterParser>();
    }

}
