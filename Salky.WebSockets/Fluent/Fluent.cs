using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Salky.WebSockets.Contracts;
using Salky.WebSockets.Implementations;

namespace Salky.WebSockets.Fluent
{
    public static class Fluent
    {

        public static IServiceCollection AddSalkyWebSocket(this IServiceCollection services, Action<ISalkyAuthGuardBuilder> options)
        {
            var builder = new SalkyWebSocketBuilder(services);
            options(builder);
            if (!builder.GuardSetted)
                throw new InvalidOperationException($"Please use the method {nameof(ISalkyAuthGuardBuilder.SetAuthGuard)} You must provide a class who implements {nameof(IConnectionAuthGuard)}");
            //
            if (!builder._IgnoreAllServices)
            {
                services.AddSingleton<IConnectionMannager, ConnectionMannager>();
                services.AddSingleton<IConnectionPoolMannager, ConnectionPoolMannager>();
                services.AddSingleton<IStorageFactory, DefaultStorageFactory>();
                services.AddSingleton<ISalkyWebSocketFactory, SalkyWebSocketFactory>();
                if (builder.IsAspNetAuth)
                {
                    SalkyWebSocketFactory.Protocol = "Identifier";
                }
                if (builder.CanInject_ConnectionEventHandler_AddOrRemoveFromConnectionMannager)
                {
                    services.Insert(0, new ServiceDescriptor(typeof(IConnectionEventHandler), typeof(ConnectionEventHandler_AddOrRemoveFromConnectionMannager), ServiceLifetime.Singleton));
                }
            }
           
            //
            return services;
        }

        public static void UseSalkyWebSocket(this IApplicationBuilder app, Action<WebSocketOptions>? options = null)
        {
            if (options != null)
            {
                var opt = new WebSocketOptions();
                options(opt);
                app.UseWebSockets(opt);
            }
            else
            {
                app.UseWebSockets();
            }

            app.UseMiddleware(typeof(SalkyMidleWare));
        }
    }

}
