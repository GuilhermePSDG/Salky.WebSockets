using Salky.WebSockets.Contracts;
using System.Text.Json;

namespace Salky.WebSockets.Test.Fakes
{
    public class FakeIMessageHandler : IMessageHandler
    {

        public int HandleBinaryCount = 0;
        public int HandleTextCount = 0;
        public List<MessageServer> MessageServers= new List<MessageServer>();
        public Task HandleBinary(ISalkyWebSocket socket, MemoryStream data)
        {
            HandleBinaryCount ++;
            var msg = JsonSerializer.Deserialize<MessageServer>(data,new JsonSerializerOptions(JsonSerializerDefaults.Web));
            MessageServers.Add(msg ?? throw new NullReferenceException());
            return Task.CompletedTask;  
        }

        public Task HandleText(ISalkyWebSocket socket, MemoryStream data)
        {
            var msg = JsonSerializer.Deserialize<MessageServer>(data, new JsonSerializerOptions(JsonSerializerDefaults.Web));
            MessageServers.Add(msg ?? throw new NullReferenceException());
            HandleTextCount++;
            return Task.CompletedTask;
        }
    }
}
