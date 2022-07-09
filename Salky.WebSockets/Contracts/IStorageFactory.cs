namespace Salky.WebSockets.Contracts
{
    public interface IStorageFactory
    {
        public IStorage CreateNew();
    }
}
