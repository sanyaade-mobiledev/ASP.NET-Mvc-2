namespace System.Web.Mvc.Test {
    using System.Web.Mvc;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System.ComponentModel;
    using System.Linq;
    using System.Collections;
    using System.Collections.Generic;

    [TestClass]
    public class ViewDataDictionaryTest {

        [TestMethod]
        public void ComparerIsOrdinalIgnoreCase() {
            // Setup
            ViewDataDictionary dictionary = new ViewDataDictionary();
            object item = new object();

            // Execute
            dictionary["Foo"] = item;
            object value = dictionary["FOO"];

            // Verify
            Assert.AreSame(item, value);
        }

        [TestMethod]
        public void ItemWithKeyInDictionaryReturnsEntry() {
            // Setup
            ViewDataDictionary dictionary = new ViewDataDictionary();
            dictionary["Foo"] = "FooValue";

            // Execute
            object value = dictionary["Foo"];

            // Verify
            Assert.AreEqual("FooValue", value);
        }

        [TestMethod]
        public void ItemWithKeyNotInDictionaryReturnsNull() {
            // DevDiv Bugs 195981: ViewData should not throw an exception if the key does not exist

            // Setup
            ViewDataDictionary dictionary = new ViewDataDictionary();

            // Execute
            object value = dictionary["Foo"];

            // Verify
            Assert.IsNull(value);
        }

        [TestMethod]
        public void SubDataItemsComparerIsOrdinalIgnoreCase() {
            // Setup
            ViewDataDictionary dictionary = new ViewDataDictionary();
            ViewDataDictionary item = new ViewDataDictionary();

            // Execute
            dictionary.SubDataItems["Foo"] = item;
            object value = dictionary.SubDataItems["FOO"];

            // Verify
            Assert.AreSame(item, value);
        }


        [TestMethod]
        public void IndexerReturnsSimplePropertyValue() {
            var obj = new { Foo = "Bar" };
            ViewDataDictionary vdd = new ViewDataDictionary(obj);

            Assert.AreEqual("Bar", vdd["Foo"]);
        }

        [TestMethod]
        public void IndexerWithModelAndDictionaryPropertyEvaluatesDictionaryValue() {
            var obj = new { Foo = new Dictionary<string, object> { { "Bar", "Baz" } } };
            ViewDataDictionary vdd = new ViewDataDictionary(obj);

            Assert.AreEqual("Baz", vdd["Foo.Bar"]);
        }

        [TestMethod]
        public void IndexerEvaluatesDictionaryThenModel() {
            var obj = new { Foo = "NotBar" };
            ViewDataDictionary vdd = new ViewDataDictionary(obj);
            vdd.Add("Foo", "Bar");

            Assert.AreEqual("Bar", vdd["Foo"]);
        }

        [TestMethod]
        public void IndexerReturnsValueOfCompoundExpressionByFollowingObjectPath() {
            var obj = new { Foo = new { Bar = "Baz" } };
            ViewDataDictionary vdd = new ViewDataDictionary(obj);

            Assert.AreEqual("Baz", vdd["Foo.Bar"]);
        }

        [TestMethod]
        public void IndexerReturnsNullIfExpressionDoesNotMatch() {
            var obj = new { Foo = new { Biz = "Baz" } };
            ViewDataDictionary vdd = new ViewDataDictionary(obj);

            Assert.AreEqual(null, vdd["Foo.Bar"]);
        }

        [TestMethod]
        public void IndexerReturnsValueJustAdded() {
            ViewDataDictionary vdd = new ViewDataDictionary();
            vdd.Add("Foo", "Blah");

            Assert.AreEqual("Blah", vdd["Foo"]);
        }

        [TestMethod]
        public void IndexerWithCompoundExpressionReturnsIndexedValue() {
            ViewDataDictionary vdd = new ViewDataDictionary();
            vdd.Add("Foo.Bar", "Baz");

            Assert.AreEqual("Baz", vdd["Foo.Bar"]);
        }

        [TestMethod]
        public void IndexerWithCompoundExpressionReturnsPropertyOfAddedObject() {
            ViewDataDictionary vdd = new ViewDataDictionary();
            vdd.Add("Foo", new { Bar = "Baz" });

            Assert.AreEqual("Baz", vdd["Foo.Bar"]);
        }

        [TestMethod]
        public void IndexerWithCompoundIndexExpressionReturnsEval() {
            ViewDataDictionary vdd = new ViewDataDictionary();
            vdd.Add("Foo.Bar", new { Baz = "Quux" });

            Assert.AreEqual("Quux", vdd["Foo.Bar.Baz"]);
        }

        [TestMethod]
        public void IndexerWithCompoundIndexAndCompoundExpressionReturnsValue() {
            ViewDataDictionary vdd = new ViewDataDictionary();
            vdd.Add("Foo.Bar", new { Baz = new { Blah = "Quux" } });

            Assert.AreEqual("Quux", vdd["Foo.Bar.Baz.Blah"]);
        }

        /// <summary>
        /// Make sure that dict["foo.bar"] gets chosen before dict["foo"]["bar"]
        /// </summary>
        [TestMethod]
        public void IndexerChoosesValueInDictionaryOverOtherValue() {
            ViewDataDictionary vdd = new ViewDataDictionary();
            vdd.Add("Foo", new { Bar = "Not Baz" });
            vdd.Add("Foo.Bar", "Baz");

            Assert.AreEqual("Baz", vdd["Foo.Bar"]);
        }

        /// <summary>
        /// Make sure that dict["foo.bar"]["baz"] gets chosen before dict["foo"]["bar"]["baz"]
        /// </summary>
        [TestMethod]
        public void IndexerChoosesCompoundValueInDictionaryOverOtherValues() {
            ViewDataDictionary vdd = new ViewDataDictionary();
            vdd.Add("Foo", new { Bar = new { Baz = "Not Quux" } });
            vdd.Add("Foo.Bar", new { Baz = "Quux" });

            Assert.AreEqual("Quux", vdd["Foo.Bar.Baz"]);
        }

        /// <summary>
        /// Make sure that dict["foo.bar"]["baz"] gets chosen before dict["foo"]["bar.baz"]
        /// </summary>
        [TestMethod]
        public void IndexerChoosesCompoundValueInDictionaryOverOtherValuesWithCompoundProperty() {
            ViewDataDictionary vdd = new ViewDataDictionary();
            vdd.Add("Foo", new Person());
            vdd.Add("Foo.Bar", new { Baz = "Quux" });

            Assert.AreEqual("Quux", vdd["Foo.Bar.Baz"]);
        }

        [TestMethod]
        public void IndexerWithCompoundExpressionAndDictionarySubExpressionChoosesDictionaryValue() {
            ViewDataDictionary vdd = new ViewDataDictionary();
            vdd.Add("Foo", new Dictionary<string, object> { { "Bar", "Baz" } });

            Assert.AreEqual("Baz", vdd["Foo.Bar"]);
        }

        [TestMethod]
        public void IndexerWithDictionaryAndNoMatchReturnsNull() {
            ViewDataDictionary vdd = new ViewDataDictionary();
            vdd.Add("Foo", new Dictionary<string, object> { { "NotBar", "Baz" } });

            object result = vdd["Foo.Bar"];
            Assert.IsNull(result);
        }

        [TestMethod]
        public void IndexerWithNestedDictionariesEvalCorrectly() {
            ViewDataDictionary vdd = new ViewDataDictionary();
            vdd.Add("Foo", new Dictionary<string, object> { { "Bar", new Hashtable { { "Baz", "Quux" } } } });

            Assert.AreEqual("Quux", vdd["Foo.Bar.Baz"]);
        }

        public class Person : CustomTypeDescriptor {
            public override PropertyDescriptorCollection GetProperties() {
                return new PropertyDescriptorCollection(new PersonPropertyDescriptor[] { new PersonPropertyDescriptor() });
            }
        }

        public class PersonPropertyDescriptor : PropertyDescriptor {
            public PersonPropertyDescriptor()
                : base("Bar.Baz", null) {
            }

            public override object GetValue(object component) {
                return "Quux";
            }

            public override bool CanResetValue(object component) {
                return false;
            }

            public override Type ComponentType {
                get {
                    return typeof(Person);
                }
            }

            public override bool IsReadOnly {
                get {
                    return false;
                }
            }

            public override Type PropertyType {
                get {
                    return typeof(string);
                }
            }

            public override void ResetValue(object component) {
            }

            public override void SetValue(object component, object value) {
            }

            public override bool ShouldSerializeValue(object component) {
                return true;
            }
        }
    }
}
