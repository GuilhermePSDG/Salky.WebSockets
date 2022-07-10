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
    public class ConnectionPoolMannagerTest
    {
        private IConnectionMannager connectionMannager = new ConnectionMannager();
        private IConnectionPoolMannager connectionPoolMannager;

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

            connectionPoolMannager = new ConnectionPoolMannager(connectionMannager);

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
        public async Task RemoveManyFromPoolAndTestEmptyResponse()
        {
            Assert.AreEqual(await connectionPoolMannager.RemoveOneFromPool("Friends", "2"), 1);
            Assert.AreEqual(await connectionPoolMannager.RemoveOneFromPool("Friends", "2"), 0);
            //
            Assert.AreEqual(await connectionPoolMannager.RemoveOneFromPool("Friends", "1"), 1);
            //
            Assert.AreEqual(await connectionPoolMannager.RemoveOneFromPool("Friends", "1"), -1);
            Assert.AreEqual(await connectionPoolMannager.RemoveOneFromPool("Friends", "2"), -1);

        }

    }
}
