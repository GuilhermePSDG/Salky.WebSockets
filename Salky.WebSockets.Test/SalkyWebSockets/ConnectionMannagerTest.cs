using Salky.WebSockets.Contracts;
using Salky.WebSockets.Implementations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Salky.WebSockets.Test.SalkyWebSockets
{
    [TestClass]
    public class ConnectionMannagerTest
    {
        private IConnectionMannager connectionMannager = new ConnectionMannager();

        internal FakeSalkySocket FakeSocket1 { get; private set; }
        internal FakeSalkySocket FakeSocket2 { get; private set; }
        internal FakeSalkySocket FakeSocket3 { get; private set; }

        [TestInitialize]
        public void Initialize()
        {
            FakeSocket1 = new FakeSalkySocket("1");
            FakeSocket2 = new FakeSalkySocket("2");
            FakeSocket3 = new FakeSalkySocket("3");
            connectionMannager.AddConnection(FakeSocket1.ConId, FakeSocket1);
            connectionMannager.AddConnection(FakeSocket2.ConId, FakeSocket2);
            connectionMannager.AddConnection(FakeSocket3.ConId, FakeSocket3);
        }
        [TestMethod]
        public async Task MustSendToAll()
        {
            await connectionMannager.SendToAll(new MessageServer());
            Assert.IsTrue(FakeSocket1.TotalMessageSended == 1);
            Assert.IsTrue(FakeSocket2.TotalMessageSended == 1);
            Assert.IsTrue(FakeSocket3.TotalMessageSended >= 1);
        }

        [TestMethod]
        public void MustContains()
        {
            Assert.IsTrue(connectionMannager.ContainsKey("2"));
        }
        [TestMethod]
        public void MustRemove()
        {
            Assert.IsNotNull(connectionMannager.TryRemoveConnection("1"));
            Assert.IsFalse(connectionMannager.ContainsKey("1"));
        }
        [TestMethod]
        public async Task MustSendToOne()
        {
            await connectionMannager.SendToOne("3", new());
            await connectionMannager.SendToOne("3", new());
            await connectionMannager.SendToOne("3", new());
            Assert.IsTrue(FakeSocket1.TotalMessageSended <= 1);
            Assert.IsTrue(FakeSocket2.TotalMessageSended <= 1);
            Assert.AreEqual(FakeSocket3.TotalMessageSended, 3);

        }
    }
}
