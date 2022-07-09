using Microsoft.Extensions.DependencyInjection;
using Salky.WebSockets.Contracts;

namespace Salky.WebSockets.Fluent
{
    public class SalkyWebSocketBuilder : ISalkyAuthGuardBuilder
    {
        public bool GuardSetted = false;
        public IServiceCollection Services { get; }
        public bool CanInject_ConnectionEventHandler_AddOrRemoveFromConnectionMannager = true;

        public void DisableAutoConnection()
        {
            CanInject_ConnectionEventHandler_AddOrRemoveFromConnectionMannager = false;
        }
        public SalkyWebSocketBuilder(IServiceCollection services)
        {
            Services = services;
        }

        public void SetAuthGuard<TConnectionGuard>() where TConnectionGuard : class, IConnectionAuthGuard
        {
            Services.AddScoped<IConnectionAuthGuard, TConnectionGuard>();
            GuardSetted = true;
        }
    }

}
