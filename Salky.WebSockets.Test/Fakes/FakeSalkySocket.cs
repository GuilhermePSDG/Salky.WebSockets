using Salky.WebSockets.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
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

        public Task<WebSocketReceiveResult> ReceiveAsync(ArraySegment<byte> buffer, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
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
