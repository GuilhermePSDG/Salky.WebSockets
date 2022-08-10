using Microsoft.AspNetCore.Http;
using Salky.WebSockets.Implementations;


namespace Salky.WebSockets.Test.SalkyWebSockets
{
    [TestClass]
    public class SalkyMidleWareTest
    {
        private RequestDelegate next;
        private SalkyMidleWare MidleWare;
        private FakeHttpContext ctx;
        private FakeLogger<SalkyMidleWare> log;
        private FakeIMessageHandler msgHandler;
        private List<FakeIConnectionEventHandler> conEventsHandler;
        private FakeIConnectionAuthGuard conGuard;
        private FakeISalkyWebSocketFactory conFactory;
        private DefaultStorageFactory storageFactory;

        [TestInitialize]
        public void Init()
        {
            this.next = Next;
            this.ctx = new FakeHttpContext();
            this.log = new FakeLogger<SalkyMidleWare>();
            this.msgHandler = new FakeIMessageHandler();
            this.conEventsHandler = new List<FakeIConnectionEventHandler>() { new FakeIConnectionEventHandler() };
            this.conGuard = new FakeIConnectionAuthGuard();
            this.conFactory = new FakeISalkyWebSocketFactory();
            this.storageFactory = new DefaultStorageFactory();
            this.MidleWare = new SalkyMidleWare(this.next, this.log, this.storageFactory);
        }
        private int NextCalledCount = 0;
        private Task Next(HttpContext ctx)
        {
            NextCalledCount++;
            return Task.CompletedTask;
        }


        [TestMethod]
        public async Task TestInvoke()
        {
            this.conGuard.CanAutorize = true;
            var invokeTask = this.MidleWare.InvokeAsync(ctx,log,msgHandler, conEventsHandler, conGuard, conFactory);
            await Task.Delay(2000);
            AutorizedConnectionTests(1);
            this.conGuard.CanAutorize = false;
            invokeTask = this.MidleWare.InvokeAsync(ctx, log, msgHandler, conEventsHandler, conGuard, conFactory);
            await Task.Delay(2000);
            AutenticationCountMustBeEqual(2);
            UnautorizedConnectionTests(2);
        }

        [TestMethod]
        public async Task TestReceiveMessage()
        {
            var invokeTask = this.MidleWare.InvokeAsync(ctx, log, msgHandler, conEventsHandler, conGuard, conFactory);
            await Task.Delay(2000);
            FakeISalkyWebSocket socket = this.conEventsHandler[0].sockets.First() as FakeISalkyWebSocket ?? throw new NullReferenceException();

            var msgSended = new MessageServer("Teste", Method.POST, Status.Success, "Data");
            socket.EmulateReceiveMessage(msgSended);
            await Task.Delay(2000);
            Assert.AreEqual(this.msgHandler.HandleTextCount, 1);
            var msgReiceved = this.msgHandler.MessageServers.Last();
            Assert.AreEqual(msgReiceved.GenRouteKey(),msgSended.GenRouteKey());
        }

        
        public void UnautorizedConnectionTests(int expectedCount)
        {
            AutenticationCountMustBeEqual(expectedCount);
            AcceptCountMustBeEqual((expectedCount-1));
            HandleOpenCountMustBeEqual((expectedCount - 1));
        }
        public void AutorizedConnectionTests(int expectedCount)
        {
            AutenticationCountMustBeEqual(expectedCount);
            AcceptCountMustBeEqual(expectedCount);
            HandleOpenCountMustBeEqual(expectedCount);
        }
        void AcceptCountMustBeEqual(int expectedCount)
        {
            Assert.AreEqual(this.ctx.WebSockets.AcceptWebSocketAsyncCounter, expectedCount);
        }
     
        void AutenticationCountMustBeEqual(int expectedCount)
        {
            Assert.AreEqual(this.conGuard.AutenticateConnectionCount, expectedCount);
        }
        void HandleOpenCountMustBeEqual(int expectedCount)
        {
            Assert.IsFalse(this.conEventsHandler.Any(x => x.HandleOpenCount != expectedCount));
        }
    }


}
