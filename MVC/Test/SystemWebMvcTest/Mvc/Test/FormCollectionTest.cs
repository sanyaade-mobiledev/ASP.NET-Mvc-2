namespace System.Web.Mvc.Test {
    using System;
    using System.Collections.Specialized;
    using System.Globalization;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Routing;
    using System.Web.TestUtil;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;

    [TestClass]
    public class FormCollectionTest {

        [TestMethod]
        public void ConstructorCopiesProvidedCollection() {
            // Arrange
            NameValueCollection nvc = new NameValueCollection() {
                { "foo", "fooValue" },
                { "bar", "barValue" }
            };

            // Act
            FormCollection formCollection = new FormCollection(nvc);

            // Assert
            Assert.AreEqual(2, formCollection.Count);
            Assert.AreEqual("fooValue", formCollection["foo"]);
            Assert.AreEqual("barValue", formCollection["bar"]);
        }

        [TestMethod]
        public void ConstructorThrowsIfCollectionIsNull() {
            // Act & Assert
            ExceptionHelper.ExpectArgumentNullException(
                delegate {
                    new FormCollection(null);
                }, "collection");
        }

        [TestMethod]
        public void CustomBinderBindModelReturnsFormCollection() {
            // Arrange
            NameValueCollection nvc = new NameValueCollection() { { "foo", "fooValue" }, { "bar", "barValue" } };
            IModelBinder binder = ModelBinders.GetBinder(typeof(FormCollection));
            ControllerContext controllerContext = GetControllerContext(nvc);
            ModelBindingContextHelper bcHelper = new ModelBindingContextHelper() { ControllerContext = controllerContext };

            // Act
            FormCollection formCollection = (FormCollection)(binder.BindModel(bcHelper.CreateBindingContext()).Value);

            // Assert
            Assert.IsNotNull(formCollection, "BindModel() should have returned a FormCollection.");
            Assert.AreEqual(2, formCollection.Count);
            Assert.AreEqual("fooValue", nvc["foo"]);
            Assert.AreEqual("barValue", nvc["bar"]);
        }

        [TestMethod]
        public void CustomBinderBindModelThrowsIfBindingContextIsNull() {
            // Arrange
            IModelBinder binder = ModelBinders.GetBinder(typeof(FormCollection));

            // Act & assert
            ExceptionHelper.ExpectArgumentNullException(
                delegate {
                    binder.BindModel(null);
                }, "bindingContext");
        }

        [TestMethod]
        public void GetValueReturnsNullIfValueNotFound() {
            // Arrange
            IValueProvider provider = new FormCollection();

            // Act
            ValueProviderResult result = provider.GetValue("foo");

            // Assert
            Assert.IsNull(result);
        }

        [TestMethod]
        public void GetValueReturnsResultIfValueFound() {
            // Arrange
            IValueProvider provider = new FormCollection() {
                { "foo", "fooValue0" },
                { "foo", "fooValue1" }
            };

            // Act
            ValueProviderResult result;
            using (DefaultValueProviderTest.ReplaceCurrentCulture("fr-FR")) {
                result = provider.GetValue("foo");
            }

            // Assert
            string[] rawValue = (string[])result.RawValue;
            Assert.AreEqual(2, rawValue.Length);
            Assert.AreEqual("fooValue0", rawValue[0]);
            Assert.AreEqual("fooValue1", rawValue[1]);

            Assert.AreEqual("fooValue0,fooValue1", result.AttemptedValue);
            Assert.AreEqual(CultureInfo.GetCultureInfo("fr-FR"), result.Culture);
        }

        [TestMethod]
        public void GetValueThrowsIfNameIsEmpty() {
            // Arrange
            IValueProvider provider = new FormCollection();

            // Act & assert
            ExceptionHelper.ExpectArgumentExceptionNullOrEmpty(
                delegate {
                    provider.GetValue(String.Empty);
                }, "name");
        }

        [TestMethod]
        public void GetValueThrowsIfNameIsNull() {
            // Arrange
            IValueProvider provider = new FormCollection();

            // Act & assert
            ExceptionHelper.ExpectArgumentExceptionNullOrEmpty(
                delegate {
                    provider.GetValue(null);
                }, "name");
        }

        private static ControllerContext GetControllerContext(NameValueCollection form) {
            Mock<HttpRequestBase> mockRequest = new Mock<HttpRequestBase>();
            mockRequest.Expect(r => r.Form).Returns(form);
            Mock<HttpContextBase> mockHttpContext = new Mock<HttpContextBase>();
            mockHttpContext.Expect(c => c.Request).Returns(mockRequest.Object);
            return new ControllerContext(mockHttpContext.Object, new RouteData(), new Mock<ControllerBase>().Object);
        }

    }
}
