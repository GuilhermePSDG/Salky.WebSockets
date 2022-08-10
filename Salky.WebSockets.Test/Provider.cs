using Microsoft.Extensions.DependencyInjection;
using Salky.WebSockets.Contracts;
using Salky.WebSockets.Implementations;

namespace Salky.WebSockets.Test
{
    public static class Provider
    {
        static Provider()
        {
            var services = new ServiceCollection();
            services.AddSingleton<IConnectionMannager, ConnectionMannager>();
            services.AddSingleton<IConnectionPoolMannager, ConnectionPoolMannager>();
            services.AddSingleton<IStorageFactory, DefaultStorageFactory>();
            services.AddSingleton<ISalkyWebSocketFactory, SalkyWebSocketFactory>();
        }
        private static IServiceProvider provider;
        public static IServiceScope CreateScope()
        {
            return provider.CreateScope();
        }


    }
}
