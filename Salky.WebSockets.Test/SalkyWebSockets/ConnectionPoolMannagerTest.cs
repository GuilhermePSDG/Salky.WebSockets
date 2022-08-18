using Salky.WebSockets.Contracts;
using Salky.WebSockets.Implementations;

namespace Salky.WebSockets.Test.SalkyWebSockets
{
    [TestClass]
    public class ConnectionPoolMannagerTest
    {
        private IConnectionMannager connectionMannager = new ConnectionMannager();
        private IConnectionPoolMannager connectionPoolMannager;

        internal FakeISalkyWebSocket FakeSocket1 { get; private set; }
        internal FakeISalkyWebSocket FakeSocket2 { get; private set; }
        internal FakeISalkyWebSocket FakeSocket3 { get; private set; }

        [TestInitialize]
        public void Initialize()
        {
            FakeSocket1 = new FakeISalkyWebSocket("1");
            FakeSocket2 = new FakeISalkyWebSocket("2");
            FakeSocket3 = new FakeISalkyWebSocket("3");
            connectionMannager.AddConnection(FakeSocket1.ConId, FakeSocket1);
            connectionMannager.AddConnection(FakeSocket2.ConId, FakeSocket2);
            connectionMannager.AddConnection(FakeSocket3.ConId, FakeSocket3);

            connectionPoolMannager = new ConnectionPoolMannager(null, connectionMannager);

            connectionPoolMannager.AddManyInPool("Group", new Key[] { "1", "2", "3" });
            connectionPoolMannager.AddManyInPool("Friends", new Key[] { "1", "2" });
        }

        [TestMethod]
        public void MustBeInPool()
        {
            Assert.IsTrue(connectionPoolMannager.IsInPool("Group", "1"));
            Assert.IsTrue(connectionPoolMannager.IsInPool("Group", "2"));
        }

        [TestMethod]
        public async Task RemoveFromPoolAndTestResponse()
        {
            Assert.AreEqual(await connectionPoolMannager.RemoveOneFromPool("Group", "3"), 1);
            Assert.IsFalse(connectionPoolMannager.IsInPool("Group", "3"));
            Assert.AreEqual(await connectionPoolMannager.RemoveOneFromPool("Group", "3"), 0);
        }
        [TestMethod]
        public async Task MustDeletePoolIfEmptyAndReturnMinus1()
        {
            Assert.AreEqual(await connectionPoolMannager.RemoveOneFromPool("Friends", "2"), 1);
            Assert.AreEqual(await connectionPoolMannager.RemoveOneFromPool("Friends", "1"), 1);
            //
            Assert.AreEqual(await connectionPoolMannager.RemoveOneFromPool("Friends", "1"), -1);
            Assert.AreEqual(await connectionPoolMannager.RemoveOneFromPool("Friends", "2"), -1);

        }

    }
}
