using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Salky.WebSockets.Contracts;
using Salky.WebSockets.Implementations;
using Salky.WebSockets.Models;

namespace Salky.WebSockets.Fluent
{

    public class DefaultConectionGuard : IConnectionAuthGuard, IConnectionEventHandler
    {
        public Task<WebSocketUser?> AuthenticateConnection(HttpContext httpContext)
        {
            return Task.FromResult<WebSocketUser?>(new WebSocketUser(Guid.NewGuid(), new List<System.Security.Claims.Claim>()));
        }

        public async Task HandleClose(ISalkyWebSocket socket)
        {
            await socket.SendMessageServer(new MessageServer("connected", Enums.Method.POST, Enums.Status.Success, new
            {
                Message = "Connected successfully",
                ConnectionId = socket.User.UserId,
            }));
        }

        public Task HandleOpen(ISalkyWebSocket socket) => Task.CompletedTask;
    }

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
