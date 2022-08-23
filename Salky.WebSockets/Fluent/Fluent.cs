using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Salky.WebSockets.Contracts;
using Salky.WebSockets.Implementations;
using System.Reflection;

namespace Salky.WebSockets.Fluent
{

    public static class Fluent
    {
        public static IServiceCollection AddSalkyWebSocket(this IServiceCollection services, Action<ISalkyOptions> options)
        {
            var builder = new SalkyWebSocketBuilder(services);
            options(builder);
            if (!builder.GuardSetted)
            {
                builder.SetAuthGuard<DefaultConectionGuard>();
                var previusColor = Console.ForegroundColor;
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"To use a custom auth guard you must use the method {nameof(ISalkyOptions.SetAuthGuard)}");
                Console.ForegroundColor = previusColor;
            }
            //
            services.AddSingleton<IConnectionMannager, ConnectionMannager>();
            services.AddSingleton<ISalkyWebSocketFactory, SalkyWebSocketFactory>();
            //
            services.MapEventsHandler();
            return services;
        }
        private static void MapEventsHandler(this IServiceCollection services)
        {
            var eventHandlerType = typeof(IConnectionEventHandler);
            var entryAssembly = Assembly.GetEntryAssembly();
            if (entryAssembly == null) return;
            var handlers = entryAssembly
                .GetTypes()
                .Where(x => x.IsAssignableTo(eventHandlerType)).ToArray();
            foreach (var eventHandler in handlers) services.AddScoped(eventHandlerType, eventHandler);
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
