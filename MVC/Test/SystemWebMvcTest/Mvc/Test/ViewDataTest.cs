namespace System.Web.Mvc.Test {
    using System.Collections;
    using System.Collections.Generic;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class ViewDataTest {
        [TestMethod]
        public void GetDataForObject() {
            ViewData data = new ViewData(new { MainProp = new { SubProp = "moo", OtherSubProp = "arf" }, OtherProp = 456 });

            Assert.AreEqual("moo", data["MainPROP.SUBprop"], "Should have been able to retrieve sub-property");
            Assert.AreEqual(456, data["OTHERprop"], "Should have been able to retrieve top-level property");
        }

        [TestMethod]
        public void GetDataForIDictionary() {
            IDictionary dict = new Hashtable();
            dict["foo"] = "moo";
            dict["foo.bar"] = 456;
            ViewData data = new ViewData(dict);

            Assert.AreEqual("moo", data["foo"]);
            Assert.AreEqual(456, data["foo.bar"]);
        }

        [TestMethod]
        public void GetDataForGenericDictionary() {
            Dictionary<string, object> dict = new Dictionary<string, object>();
            dict["foo"] = "moo";
            dict["foo.bar"] = 456;
            ViewData data = new ViewData(dict);

            Assert.AreEqual("moo", data["foo"]);
            Assert.AreEqual(456, data["foo.bar"]);
        }

        [TestMethod]
        public void GetDataWithInvalidKeyForDictionary() {
            // DevDiv Bugs 195981: ViewData should not throw an exception if the key does not exist

            Dictionary<string, object> dict = new Dictionary<string, object>();
            ViewData viewData = new ViewData(dict);

            Assert.IsNull(viewData["Foo"]);
        }

        [TestMethod]
        public void GetDataWithInvalidKeyForObject() {
            // DevDiv Bugs 195981: ViewData should not throw an exception if the key does not exist

            object o = new { Foo = "MyFoo" };
            ViewData viewData = new ViewData(o);

            Assert.IsNull(viewData["Bar"]);
        }

        [TestMethod]
        public void ContainsDataItemForObject() {
            ViewData viewData = new ViewData(new { Prop = "foo" });

            Assert.IsTrue(viewData.ContainsDataItem("Prop"));
            Assert.IsFalse(viewData.ContainsDataItem("NonExistantProp"));
        }

        [TestMethod]
        public void ContainsDataItemForDictionary() {
            IDictionary dict = new Hashtable();
            dict["Prop"] = "foo";
            ViewData viewData = new ViewData(dict);

            Assert.IsTrue(viewData.ContainsDataItem("Prop"));
            Assert.IsFalse(viewData.ContainsDataItem("NonExistantProp"));
        }

        [TestMethod]
        public void ContainsDataItemForGenericDictionary() {
            IDictionary<string,object> dict = new Dictionary<string,object>();
            dict["Prop"] = "foo";
            ViewData viewData = new ViewData(dict);

            Assert.IsTrue(viewData.ContainsDataItem("Prop"));
            Assert.IsFalse(viewData.ContainsDataItem("NonExistantProp"));
        }
    }
}
