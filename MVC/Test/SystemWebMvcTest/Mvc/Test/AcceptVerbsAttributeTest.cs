namespace System.Web.Mvc.Test {
    using System.Collections.ObjectModel;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Routing;
    using System.Web.TestUtil;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;

    [TestClass]
    public class AcceptVerbsAttributeTest {

        [TestMethod]
        public void ConstructorThrowsIfVerbsIsEmpty() {
            // Act & Assert
            ExceptionHelper.ExpectArgumentExceptionNullOrEmpty(
                delegate {
                    new AcceptVerbsAttribute(new string[0]);
                }, "verbs");
        }

        [TestMethod]
        public void ConstructorThrowsIfVerbsIsNull() {
            // Act & Assert
            ExceptionHelper.ExpectArgumentExceptionNullOrEmpty(
                delegate {
                    new AcceptVerbsAttribute((string[])null);
                }, "verbs");
        }

        [TestMethod]
        public void IsValidForRequestReturnsFalseIfHttpVerbIsNotInVerbsCollection() {
            // Arrange
            AcceptVerbsAttribute attr = new AcceptVerbsAttribute("get", "post");
            ControllerContext context = GetControllerContext("HEAD");

            // Act
            bool result = attr.IsValidForRequest(context, null);

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void IsValidForRequestReturnsTrueIfHttpVerbIsInVerbsCollection() {
            // Arrange
            AcceptVerbsAttribute attr = new AcceptVerbsAttribute("get", "post");
            ControllerContext context = GetControllerContext("POST");

            // Act
            bool result = attr.IsValidForRequest(context, null);

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void IsValidForRequestThrowsIfControllerContextIsNull() {
            // Arrange
            AcceptVerbsAttribute attr = new AcceptVerbsAttribute("get", "post");

            // Act & Assert
            ExceptionHelper.ExpectArgumentNullException(
                delegate {
                    attr.IsValidForRequest(null, null);
                }, "controllerContext");
        }

        [TestMethod]
        public void VerbsProperty() {
            // Arrange
            AcceptVerbsAttribute attr = new AcceptVerbsAttribute("get", "post");

            // Act
            ReadOnlyCollection<string> collection = attr.Verbs as ReadOnlyCollection<string>;

            // Assert
            Assert.IsNotNull(collection, "Verbs property should have returned read-only collection.");
            Assert.AreEqual(2, collection.Count);
            Assert.AreEqual("get", collection[0]);
            Assert.AreEqual("post", collection[1]);
        }

        private static ControllerContext GetControllerContext(string httpVerb) {
            Mock<HttpRequestBase> mockHttpRequest = new Mock<HttpRequestBase>();
            mockHttpRequest.Expect(r => r.HttpMethod).Returns(httpVerb);
            Mock<HttpContextBase> mockHttpContext = new Mock<HttpContextBase>();
            mockHttpContext.Expect(c => c.Request).Returns(mockHttpRequest.Object);

            return new ControllerContext(mockHttpContext.Object, new RouteData(), new Mock<ControllerBase>().Object);
        }

    }
}
