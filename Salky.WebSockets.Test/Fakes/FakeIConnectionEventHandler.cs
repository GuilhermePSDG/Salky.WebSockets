using Salky.WebSockets.Contracts;

namespace Salky.WebSockets.Test.Fakes
{
    public class FakeIConnectionEventHandler : IConnectionEventHandler
    {
        public int HandleCloseCount = 0;
        public int HandleOpenCount = 0;

        public List<ISalkyWebSocket> sockets = new List<ISalkyWebSocket>();
        public Task HandleClose(ISalkyWebSocket socket)
        {
            this.sockets.Remove(socket);
            HandleCloseCount++;
            return Task.CompletedTask;
        }

        public Task HandleOpen(ISalkyWebSocket socket)
        {
            this.sockets.Add(socket);
            HandleOpenCount++;
            return Task.CompletedTask;
        }
    }
}
