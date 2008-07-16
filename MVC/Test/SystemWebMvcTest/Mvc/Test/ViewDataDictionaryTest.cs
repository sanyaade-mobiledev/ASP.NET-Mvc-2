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
            // DevDiv Bugs 195982: The ViewData dictionary property on Controller should be case insensitive

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
        public void ItemWithKeyNotInDictionaryDoesNotCheckModel() {
            // DevDiv Bugs 201014: Consider having ViewDataDictionary's indexer be just a dictionary
            // indexer and have a separate public Eval() method

            // Setup
            ViewDataDictionary dictionary = new ViewDataDictionary(new { Foo = "Bar" });

            // Execute
            object value = dictionary["Foo"];

            // Verify
            Assert.IsNull(value);
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
        public void EvalReturnsSimplePropertyValue() {
            var obj = new { Foo = "Bar" };
            ViewDataDictionary vdd = new ViewDataDictionary(obj);

            Assert.AreEqual("Bar", vdd.Eval("Foo"));
        }

        [TestMethod]
        public void EvalWithModelAndDictionaryPropertyEvaluatesDictionaryValue() {
            var obj = new { Foo = new Dictionary<string, object> { { "Bar", "Baz" } } };
            ViewDataDictionary vdd = new ViewDataDictionary(obj);

            Assert.AreEqual("Baz", vdd.Eval("Foo.Bar"));
        }

        [TestMethod]
        public void EvalEvaluatesDictionaryThenModel() {
            var obj = new { Foo = "NotBar" };
            ViewDataDictionary vdd = new ViewDataDictionary(obj);
            vdd.Add("Foo", "Bar");

            Assert.AreEqual("Bar", vdd.Eval("Foo"));
        }

        [TestMethod]
        public void EvalReturnsValueOfCompoundExpressionByFollowingObjectPath() {
            var obj = new { Foo = new { Bar = "Baz" } };
            ViewDataDictionary vdd = new ViewDataDictionary(obj);

            Assert.AreEqual("Baz", vdd.Eval("Foo.Bar"));
        }

        [TestMethod]
        public void EvalReturnsNullIfExpressionDoesNotMatch() {
            var obj = new { Foo = new { Biz = "Baz" } };
            ViewDataDictionary vdd = new ViewDataDictionary(obj);

            Assert.AreEqual(null, vdd.Eval("Foo.Bar"));
        }

        [TestMethod]
        public void EvalReturnsValueJustAdded() {
            ViewDataDictionary vdd = new ViewDataDictionary();
            vdd.Add("Foo", "Blah");

            Assert.AreEqual("Blah", vdd.Eval("Foo"));
        }

        [TestMethod]
        public void EvalWithCompoundExpressionReturnsIndexedValue() {
            ViewDataDictionary vdd = new ViewDataDictionary();
            vdd.Add("Foo.Bar", "Baz");

            Assert.AreEqual("Baz", vdd.Eval("Foo.Bar"));
        }

        [TestMethod]
        public void EvalWithCompoundExpressionReturnsPropertyOfAddedObject() {
            ViewDataDictionary vdd = new ViewDataDictionary();
            vdd.Add("Foo", new { Bar = "Baz" });

            Assert.AreEqual("Baz", vdd.Eval("Foo.Bar"));
        }

        [TestMethod]
        public void EvalWithCompoundIndexExpressionReturnsEval() {
            ViewDataDictionary vdd = new ViewDataDictionary();
            vdd.Add("Foo.Bar", new { Baz = "Quux" });

            Assert.AreEqual("Quux", vdd.Eval("Foo.Bar.Baz"));
        }

        [TestMethod]
        public void EvalWithCompoundIndexAndCompoundExpressionReturnsValue() {
            ViewDataDictionary vdd = new ViewDataDictionary();
            vdd.Add("Foo.Bar", new { Baz = new { Blah = "Quux" } });

            Assert.AreEqual("Quux", vdd.Eval("Foo.Bar.Baz.Blah"));
        }

        /// <summary>
        /// Make sure that dict["foo.bar"] gets chosen before dict["foo"]["bar"]
        /// </summary>
        [TestMethod]
        public void EvalChoosesValueInDictionaryOverOtherValue() {
            ViewDataDictionary vdd = new ViewDataDictionary();
            vdd.Add("Foo", new { Bar = "Not Baz" });
            vdd.Add("Foo.Bar", "Baz");

            Assert.AreEqual("Baz", vdd.Eval("Foo.Bar"));
        }

        /// <summary>
        /// Make sure that dict["foo.bar"]["baz"] gets chosen before dict["foo"]["bar"]["baz"]
        /// </summary>
        [TestMethod]
        public void EvalChoosesCompoundValueInDictionaryOverOtherValues() {
            ViewDataDictionary vdd = new ViewDataDictionary();
            vdd.Add("Foo", new { Bar = new { Baz = "Not Quux" } });
            vdd.Add("Foo.Bar", new { Baz = "Quux" });

            Assert.AreEqual("Quux", vdd.Eval("Foo.Bar.Baz"));
        }

        /// <summary>
        /// Make sure that dict["foo.bar"]["baz"] gets chosen before dict["foo"]["bar.baz"]
        /// </summary>
        [TestMethod]
        public void EvalChoosesCompoundValueInDictionaryOverOtherValuesWithCompoundProperty() {
            ViewDataDictionary vdd = new ViewDataDictionary();
            vdd.Add("Foo", new Person());
            vdd.Add("Foo.Bar", new { Baz = "Quux" });

            Assert.AreEqual("Quux", vdd.Eval("Foo.Bar.Baz"));
        }

        [TestMethod]
        public void EvalWithCompoundExpressionAndDictionarySubExpressionChoosesDictionaryValue() {
            ViewDataDictionary vdd = new ViewDataDictionary();
            vdd.Add("Foo", new Dictionary<string, object> { { "Bar", "Baz" } });

            Assert.AreEqual("Baz", vdd.Eval("Foo.Bar"));
        }

        [TestMethod]
        public void EvalWithDictionaryAndNoMatchReturnsNull() {
            ViewDataDictionary vdd = new ViewDataDictionary();
            vdd.Add("Foo", new Dictionary<string, object> { { "NotBar", "Baz" } });

            object result = vdd.Eval("Foo.Bar");
            Assert.IsNull(result);
        }

        [TestMethod]
        public void EvalWithNestedDictionariesEvalCorrectly() {
            ViewDataDictionary vdd = new ViewDataDictionary();
            vdd.Add("Foo", new Dictionary<string, object> { { "Bar", new Hashtable { { "Baz", "Quux" } } } });

            Assert.AreEqual("Quux", vdd.Eval("Foo.Bar.Baz"));
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
