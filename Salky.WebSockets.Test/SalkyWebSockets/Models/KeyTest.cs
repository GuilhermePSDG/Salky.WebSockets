namespace Salky.WebSockets.Test.SalkyWebSockets.Models
{
    [TestClass]
    public class KeyTest
    {
        [TestInitialize]
        public void Initialize()
        {

        }
        [TestMethod]
        public void MustBeEquals()
        {
            Key key1 = "abcd";
            Key key2 = "abcd";
            Assert.IsTrue(key1.Equals(key2));
            Assert.IsTrue(key1 == key2);
        }
        [TestMethod]
        public void MustBeEqualsIfTypeAreDiferenteButValueIsEqual()
        {
            var guid = Guid.NewGuid();
            var guidString = guid.ToString();
            Key key = guid;

            Assert.AreEqual(key, guid);
            Assert.AreEqual(key, guidString);

            Assert.AreEqual(key, guid);
            Assert.AreEqual(key, guidString);
        }
        [TestMethod]
        public void CannotBeEquals()
        {
            Key Key1 = "a";
            Key Key2 = "A";

            Assert.AreNotEqual(Key1, Key2);
            Assert.AreNotEqual(Key1, "A");
        }
        [TestMethod]
        public void HashCodeMustBeEquals()
        {
            Key key1 = "abcd";
            Key key2 = "abcd";
            Assert.IsTrue(key1.GetHashCode() == key2.GetHashCode());
        }
        [TestMethod]
        public void ValueCannotChange()
        {
            var value1 = "@asd33KSA-Xxf@!!ÇÇ^^^}}^~~^";
            Key key1 = value1;
            Assert.IsTrue(key1.Value == value1);
        }
        [TestMethod]
        public void MustBeGratter()
        {
            Key key1 = "aabcd";
            Key key2 = "abcd";
            Assert.IsTrue(key1 > key2);
        }
        [TestMethod]
        public void MustBeLower()
        {
            Key key1 = "bcd";
            Key key2 = "abcd";
            Assert.IsTrue(key1 < key2);
        }
    }
}