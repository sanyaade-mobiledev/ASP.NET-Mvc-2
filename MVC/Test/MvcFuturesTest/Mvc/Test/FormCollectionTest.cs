namespace MvcFuturesTest.Mvc.Test {
    using System;
    using System.Collections.Specialized;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Routing;
    using System.Web.TestUtil;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Microsoft.Web.Mvc;
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
        public void CustomBinderGetValueReturnsFormCollection() {
            // Arrange
            NameValueCollection nvc = new NameValueCollection() { { "foo", "fooValue" }, { "bar", "barValue" } };
            IModelBinder binder = GetBinder();
            ControllerContext controllerContext = GetControllerContext(nvc);

            // Act
            FormCollection formCollection = binder.GetValue(controllerContext, null, typeof(FormCollection), null) as FormCollection;

            // Assert
            Assert.IsNotNull(formCollection, "GetValue() should have returned a FormCollection.");
            Assert.AreEqual(2, formCollection.Count);
            Assert.AreEqual("fooValue", nvc["foo"]);
            Assert.AreEqual("barValue", nvc["bar"]);
        }

        [TestMethod]
        public void CustomBinderGetValueThrowsIfControllerContextIsNull() {
            // Arrange
            IModelBinder binder = GetBinder();

            // Act & Assert
            ExceptionHelper.ExpectArgumentNullException(
                delegate {
                    binder.GetValue(null, null, null, null);
                }, "controllerContext");
        }

        [TestMethod]
        public void CustomBinderGetValueThrowsIfModelTypeIsIncorrect() {
            // Arrange
            IModelBinder binder = GetBinder();
            ControllerContext controllerContext = GetControllerContext(null);

            // Act & Assert
            ExceptionHelper.ExpectArgumentException(
                delegate {
                    binder.GetValue(controllerContext, null, typeof(int), null);
                },
                @"This model binder can only be used with models of type 'Microsoft.Web.Mvc.FormCollection'.
Parameter name: modelType");
        }

        [TestMethod]
        public void CustomBinderIsDeclaredOnTypeAsAttribute() {
            // Act
            bool exists = typeof(FormCollection).IsDefined(typeof(CustomModelBinderAttribute), true /* inherit */);

            // Assert
            Assert.IsTrue(exists, "FormCollection should have contained a CustomModelBinderAttribute.");
        }

        [TestMethod]
        public void DefaultConstructor() {
            // Act
            FormCollection formCollection = new FormCollection();

            // Assert
            Assert.AreEqual(0, formCollection.Count);
        }

        private static IModelBinder GetBinder() {
            CustomModelBinderAttribute attr = (CustomModelBinderAttribute)Attribute.GetCustomAttribute(typeof(FormCollection), typeof(CustomModelBinderAttribute));
            return attr.GetBinder();
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
