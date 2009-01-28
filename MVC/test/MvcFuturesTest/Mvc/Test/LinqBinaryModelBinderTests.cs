namespace Microsoft.Web.Mvc.Test {
    using System;
    using System.Data.Linq;
    using System.Web.Mvc;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Microsoft.Web.Mvc;

    [TestClass]
    public class LinqBinaryModelBinderTests {
        [TestMethod]
        public void BindModelWithNonExistentValueReturnsNull() {
            //arrange
            ValueProviderDictionary dict = new ValueProviderDictionary(null) {
                { "foo", new ValueProviderResult(null, null, null) }
            };

            ModelBindingContext bindingContext = new ModelBindingContext() {
                ModelName = "foo",
                ValueProvider = dict
            };

            LinqBinaryModelBinder binder = new LinqBinaryModelBinder();

            // act
            var binderResult = binder.BindModel(null, bindingContext);

            //assert
            Assert.IsNull(binderResult);
        }

        [TestMethod]
        public void BinderWithEmptyStringValueReturnsNull() {
            //arrange
            ValueProviderDictionary dict = new ValueProviderDictionary(null) {
                { "foo", new ValueProviderResult(String.Empty, null, null) }
            };

            ModelBindingContext bindingContext = new ModelBindingContext() {
                ModelName = "foo",
                ValueProvider = dict
            };

            LinqBinaryModelBinder binder = new LinqBinaryModelBinder();

            // act
            var binderResult = binder.BindModel(null, bindingContext);

            //assert
            Assert.IsNull(binderResult);
        }

        [TestMethod]
        public void BindModelWithBase64QuotedValueReturnsBinary() {
            //arrange
            string base64Value = "AAAAAAAAPpA=";
            ValueProviderDictionary dict = new ValueProviderDictionary(null) {
                { "foo", new ValueProviderResult("\"" + base64Value + "\"", "\"" + base64Value + "\"", null) }
            };

            ModelBindingContext bindingContext = new ModelBindingContext() {
                ModelName = "foo",
                ValueProvider = dict
            };

            LinqBinaryModelBinder binder = new LinqBinaryModelBinder();

            // act
            var boundValue = binder.BindModel(null, bindingContext) as Binary;

            //assert
            Assert.IsNotNull(boundValue);
            string result = Convert.ToBase64String(boundValue.ToArray());
            Assert.AreEqual(base64Value, result);
        }

        [TestMethod]
        public void BindModelWithBase64UnquotedValueReturnsBinary() {
            //arrange
            string base64Value = "AAAAAAAAPpA=";
            ValueProviderDictionary dict = new ValueProviderDictionary(null) {
                { "foo", new ValueProviderResult(base64Value, base64Value, null) }
            };

            ModelBindingContext bindingContext = new ModelBindingContext() {
                ModelName = "foo",
                ValueProvider = dict
            };

            LinqBinaryModelBinder binder = new LinqBinaryModelBinder();

            // act
            var boundValue = binder.BindModel(null, bindingContext) as Binary;

            //assert
            Assert.IsNotNull(boundValue);
            string result = Convert.ToBase64String(boundValue.ToArray());
            Assert.AreEqual(base64Value, result);
        }
    }
}
