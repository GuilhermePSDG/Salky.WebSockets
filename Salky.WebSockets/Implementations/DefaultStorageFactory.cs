using Salky.WebSockets.Contracts;

namespace Salky.WebSockets.Implementations
{
    public class DefaultStorageFactory : IStorageFactory
    {
        public IStorage CreateNew() => new Storage();
    }
}
