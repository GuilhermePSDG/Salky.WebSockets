using Salky.WebSockets.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Salky.WebSockets.Test.Fakes
{
    internal class FakeSalkySocket : ISalkyWebSocket
    {
        public string ConId = "";
        public FakeSalkySocket(string ConId)
        {
            this.ConId = ConId;
        }
        public WebSocketCloseStatus? CloseStatus => throw new NotImplementedException();

        public string? CloseStatusDescription => throw new NotImplementedException();

        public WebSocketState State => throw new NotImplementedException();

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
