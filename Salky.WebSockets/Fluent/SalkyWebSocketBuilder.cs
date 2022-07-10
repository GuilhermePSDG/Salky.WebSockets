using Microsoft.Extensions.DependencyInjection;
using Salky.WebSockets.Contracts;
using Salky.WebSockets.Implementations;

namespace Salky.WebSockets.Fluent
{
    public class SalkyWebSocketBuilder : ISalkyAuthGuardBuilder
    {
        private static ClaimsIdSelector? IdSelector;
        public bool GuardSetted
        {
            get => __GuardSetted;
            set
            {
                if (__GuardSetted)
                    throw new InvalidOperationException($"{nameof(IConnectionAuthGuard)} is already setted");
                __GuardSetted = value;
            }
        }
        public bool __GuardSetted = false;
        public IServiceCollection Services { get; }
        public bool _IgnoreAllServices { get; private set; }

        public bool CanInject_ConnectionEventHandler_AddOrRemoveFromConnectionMannager = true;

        public void DisableAutoConnection()
        {
            CanInject_ConnectionEventHandler_AddOrRemoveFromConnectionMannager = false;
        }
        public bool IsAspNetAuth = false;
        public void UseAspNetAuth(ClaimsIdSelector IdSelector)
        {
            if (IdSelector is null) throw new ArgumentNullException(nameof(IdSelector));
            SalkyWebSocketBuilder.IdSelector = IdSelector;
            this.Services.AddScoped<IConnectionAuthGuard, AspNetDefaultAuthGuard>
                (x => new AspNetDefaultAuthGuard(SalkyWebSocketBuilder.IdSelector));
            GuardSetted = true;
            this.IsAspNetAuth = true;
        }

        public void IgnoreAllServices()
        {
            this._IgnoreAllServices = true;
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
