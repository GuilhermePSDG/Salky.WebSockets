using Microsoft.Extensions.DependencyInjection;
using Salky.WebSockets.Contracts;
using Salky.WebSockets.Implementations;

namespace Salky.WebSockets.Fluent
{
    public interface ISalkyOptions
    {
        public void SetAuthGuard<TConnectionGuard>() where TConnectionGuard : class, IConnectionAuthGuard;
        public IServiceCollection Services { get; }
        public void UseDefaultConnectionMannager(Action<ConnectionMannagerOptions> options);
    }

}
