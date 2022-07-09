using Microsoft.Extensions.DependencyInjection;
using Salky.WebSockets.Contracts;

namespace Salky.WebSockets.Fluent
{
    public interface ISalkyAuthGuardBuilder
    {
        public void SetAuthGuard<TConnectionGuard>() where TConnectionGuard : class, IConnectionAuthGuard;
        public IServiceCollection Services { get; }
        /// <summary>
        /// Caso seja chamado irá fazer com que após uma conexão ser aberta ou fechada a mesma não sera adicionada nem removida automaticamento do <see cref="IConnectionMannager"/>
        /// <para>Sendo necessário a implementação do <see cref="IConnectionEventHandler"/> para substituição</para>
        /// </summary>
        public void DisableAutoConnection();

    }

}
