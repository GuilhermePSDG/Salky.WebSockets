using Microsoft.Extensions.DependencyInjection;
using Salky.WebSockets.Contracts;
using Salky.WebSockets.Implementations;

namespace Salky.WebSockets.Fluent
{
    public class SalkyWebSocketBuilder : ISalkyOptions
    {
        public bool GuardSetted = false;
        public IServiceCollection Services { get; }

        public SalkyWebSocketBuilder(IServiceCollection services)
        {
            Services = services;
        }

        public void SetAuthGuard<TConnectionGuard>() where TConnectionGuard : class, IConnectionAuthGuard
        {
            Services.AddScoped<IConnectionAuthGuard, TConnectionGuard>();
            GuardSetted = true;
        }

        public void UseDefaultConnectionMannager(Action<ConnectionMannagerOptions> options)
        {
            options(new ConnectionMannagerOptions(this.Services));
        }
    }
}
public class ConnectionMannagerOptions
{
    private readonly IServiceCollection collections;
    public ConnectionMannagerOptions(IServiceCollection collections)
    {
        this.collections = collections;
    }
    public void UseBasicConnectionRemotion()
    {
        collections.AddSingleton<IConnectionPoolMannager, ConnectionPoolMannager>();
        collections.Insert(0, new ServiceDescriptor(typeof(IConnectionEventHandler), typeof(ConnectionEventHandler_AddOrRemoveFromRootConnectionMannager), ServiceLifetime.Singleton));
    }
    public void UseFullyDeepConnectionRemotion()
    {
        collections.AddSingleton<IConnectionPoolMannager, ConnectionPoolMannager_WithClientPresenceRecording>();
        collections.Insert(0, new ServiceDescriptor(typeof(IConnectionEventHandler), typeof(ConnectionEventHandler_AddOrRemoveFromRootConnectionMannager), ServiceLifetime.Singleton));
        collections.Insert(1, new ServiceDescriptor(typeof(IConnectionEventHandler), typeof(ConnectionEventHandler_OnUserDisconnectRemoveHimFromEveryPools), ServiceLifetime.Singleton));
    }
}