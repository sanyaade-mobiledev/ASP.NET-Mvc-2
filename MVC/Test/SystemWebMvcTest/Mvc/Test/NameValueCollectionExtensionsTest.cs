namespace System.Web.Mvc.Test {
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.Web.Routing;
    using System.Web.TestUtil;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class NameValueCollectionExtensionsTest {

        [TestMethod]
        public void CopyTo() {
            // Setup
            NameValueCollection collection = GetCollection();
            IDictionary<string, object> dictionary = GetDictionary();

            // Execute
            collection.CopyTo(dictionary);

            // Verify
            Assert.AreEqual(3, dictionary.Count);
            Assert.AreEqual("FooDictionary", dictionary["foo"]);
            Assert.AreEqual("BarDictionary", dictionary["bar"]);
            Assert.AreEqual("BazCollection", dictionary["baz"]);
        }

        public void CopyToReplaceExisting() {
            // Setup
            NameValueCollection collection = GetCollection();
            IDictionary<string, object> dictionary = GetDictionary();

            // Execute
            collection.CopyTo(dictionary, true /* replaceExisting */);

            // Verify
            Assert.AreEqual(3, dictionary.Count);
            Assert.AreEqual("FooCollection", dictionary["foo"]);
            Assert.AreEqual("BarDictionary", dictionary["bar"]);
            Assert.AreEqual("BazCollection", dictionary["baz"]);
        }

        [TestMethod]
        public void CopyToWithNullCollectionThrows() {
            ExceptionHelper.ExpectArgumentNullException(
            delegate {
                NameValueCollectionExtensions.CopyTo(null /* collection */, null /* destination */);
            }, "collection");
        }

        [TestMethod]
        public void CopyToWithNullDestinationThrows() {
            // Setup
            NameValueCollection collection = GetCollection();

            // Execute & verify
            ExceptionHelper.ExpectArgumentNullException(
                delegate {
                    collection.CopyTo(null /* destination */);
                }, "destination");
        }

        private static NameValueCollection GetCollection() {
            return new NameValueCollection {
                { "Foo", "FooCollection" },
                { "Baz", "BazCollection" }
            };
        }

        private static IDictionary<string, object> GetDictionary() {
            return new RouteValueDictionary(new { Foo = "FooDictionary", Bar = "BarDictionary" });
        }
    }
}
