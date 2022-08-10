using Salky.WebSockets.Contracts;
using System.Net.WebSockets;
using System.Text.Json;

namespace Salky.WebSockets.Test.Fakes
{
    internal class FakeISalkyWebSocket : ISalkyWebSocket
    {
        public string ConId = "";
        public FakeISalkyWebSocket(string ConId)
        {
            this.ConId = ConId;
        }
        public WebSocketCloseStatus? CloseStatus { get; set; }=WebSocketCloseStatus.NormalClosure;

        public string? CloseStatusDescription => "";

        public WebSocketState State { get; set; }= WebSocketState.Open;

        public WebSocketUser User => throw new NotImplementedException();

        public Task CloseOutputAsync(WebSocketCloseStatus closeStatus, CloseDescription statusDescription)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {

        }

        private Queue<MessageServer> Messages = new Queue<MessageServer>();

        public void EmulateReceiveMessage(MessageServer message)
        {
            this.Messages.Enqueue(message);
        }

        public async Task<WebSocketReceiveResult> ReceiveAsync(ArraySegment<byte> buffer, CancellationToken cancellationToken)
        {
            await Task.Run(() =>
            {
                while (true)
                {
                    if (Messages.Any())
                        break;
                    cancellationToken.ThrowIfCancellationRequested();
                }
            });
            var msg = Messages.Dequeue();
            var b = JsonSerializer.SerializeToUtf8Bytes(msg, new JsonSerializerOptions(JsonSerializerDefaults.Web));
            Array.Copy(b, buffer.Array ?? throw new(), (b.Length ));
            return new WebSocketReceiveResult(b.Length, WebSocketMessageType.Text, true);
        }

        public int TotalErrosSended=0; 
        public Task SendErrorAsync(string message, string path, Method method = Method.POST)
        {
            this.TotalErrosSended++;
            return Task.CompletedTask;
        }
        public int TotalMessageSended = 0;
        public Task SendMessageServer(MessageServer messageServer)
        {
            this.TotalMessageSended++;
            return Task.CompletedTask;
        }
    }
}
