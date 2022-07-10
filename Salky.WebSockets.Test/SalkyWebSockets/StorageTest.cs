using Salky.WebSockets.Contracts;
using Salky.WebSockets.Implementations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Salky.WebSockets.Test.SalkyWebSockets
{
    [TestClass]
    public class StorageTest
    {
        private IStorage storage;
        [TestInitialize]
        public void Initialize()
        {
            storage = new Storage();
            storage.Add("a", 1);
            storage.Add("b", 2);
        }
        [TestMethod]
        public void AddTest()
        {
            storage.Add("AddTestkey", "AddTestvalue");
            Assert.IsTrue(storage.ContainsKey("AddTestkey"));
        }
        [TestMethod]
        public void RemoveTest()
        {
            Assert.IsTrue(storage.ContainsKey("a"));
            storage.Remove("a");
            Assert.IsFalse(storage.ContainsKey("a"));
        }

        [TestMethod]
        public void GetTest()
        {
            var res = storage.Get("b");
            Assert.IsNotNull(res);
            Assert.IsInstanceOfType(res, typeof(int));
            Assert.AreEqual(res, 2);
        }

        [TestMethod]
        public void AddOrUpdate()
        {
            storage.AddOrUpdate("b", "modified");
            var res = storage.Get("b");
            Assert.IsNotNull(res);
            Assert.IsInstanceOfType(res, typeof(string));
            Assert.AreEqual(res, "modified");
        }

    }
}
