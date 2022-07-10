namespace Salky.WebSockets.Test.SalkyWebSockets.Models
{
    [TestClass]
    public class RoutePathBaseTest
    {
        [TestMethod]
        public void MustBeEquals()
        {
            var route1 = new RoutePathBase("abcd", Method.POST);
            var route2 = new RoutePathBase("abcd", Method.POST);
            Assert.AreEqual(route1, route2);
        }
        [TestMethod]
        public void CannotBeEquals()
        {
            var route1 = new RoutePathBase("abcd", Method.LISTENER);
            var route2 = new RoutePathBase("abcd", Method.POST);
            Assert.AreNotEqual(route1, route2);


            var route3 = new RoutePathBase("abcd/fgs", Method.GET);
            var route4 = new RoutePathBase("abcd", Method.GET);
            Assert.AreNotEqual(route3, route4);
        }
        [TestMethod]
        public void MustIgnoreCase()
        {
            var route1 = new RoutePathBase("ABCD", Method.POST);
            var route2 = new RoutePathBase("abcd", Method.POST);
            Assert.AreEqual(route1, route2);
        }



    }

}
