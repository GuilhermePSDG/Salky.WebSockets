using Salky.WebSockets.Contracts;

namespace Example
{
    public class CustomConnectionEventHandler : IConnectionEventHandler
    {
        public CustomConnectionEventHandler()
        {

        }
        public Task HandleClose(ISalkyWebSocket socket)
        {
            Console.WriteLine("Custom handler close");
            return Task.CompletedTask;
        }

        public Task HandleOpen(ISalkyWebSocket socket)
        {
            Console.WriteLine("Custom handler Open");
            return Task.CompletedTask;
        }
    }
}
