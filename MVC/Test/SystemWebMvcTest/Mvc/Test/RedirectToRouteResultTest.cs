namespace System.Web.Mvc.Test {
    using System;
    using System.Web.Routing;
    using System.Web.TestUtil;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;

    [TestClass]
    public class RedirectToRouteResultTest {

        [TestMethod]
        public void ConstructorWithNullValuesDictionary() {
            // Act
            var result = new RedirectToRouteResult(null /* values */);

            // Assert
            Assert.IsNotNull(result.Values);
            Assert.AreEqual<int>(0, result.Values.Count);
            Assert.AreEqual<string>(String.Empty, result.RouteName);
        }

        [TestMethod]
        public void ConstructorSetsValuesDictionary() {
            // Arrange
            RouteValueDictionary dict = new RouteValueDictionary();

            // Act
            var result = new RedirectToRouteResult(dict);

            // Assert
            Assert.AreSame(dict, result.Values);
            Assert.AreEqual<string>(String.Empty, result.RouteName);
        }

        [TestMethod]
        public void ConstructorSetsValuesDictionaryAndEmptyName() {
            // Arrange
            RouteValueDictionary dict = new RouteValueDictionary();

            // Act
            var result = new RedirectToRouteResult(null, dict);

            // Assert
            Assert.AreSame(dict, result.Values);
            Assert.AreEqual<string>(String.Empty, result.RouteName);
        }

        [TestMethod]
        public void ConstructorSetsValuesDictionaryAndName() {
            // Arrange
            RouteValueDictionary dict = new RouteValueDictionary();

            // Act
            var result = new RedirectToRouteResult("foo", dict);

            // Assert
            Assert.AreSame(dict, result.Values);
            Assert.AreEqual<string>("foo", result.RouteName);
        }

        [TestMethod]
        public void ExecuteResult() {
            // Arrange
            Mock<HttpRequestBase> mockRequest = new Mock<HttpRequestBase>();
            mockRequest.Expect(r => r.ApplicationPath).Returns("/somepath");
            Mock<HttpResponseBase> mockResponse = new Mock<HttpResponseBase>();
            mockResponse.Expect(r => r.ApplyAppPathModifier(It.IsAny<string>())).Returns((string s) => s);
            mockResponse.Expect(r => r.Redirect("/somepath/c/a/i", false)).Verifiable();
            Mock<HttpContextBase> mockHttpContext = new Mock<HttpContextBase>();
            mockHttpContext.Expect(c => c.Request).Returns(mockRequest.Object);
            mockHttpContext.Expect(c => c.Response).Returns(mockResponse.Object);

            ControllerContext context = new ControllerContext(mockHttpContext.Object, new RouteData(), new Mock<ControllerBase>().Object);
            var values = new { Controller = "c", Action = "a", Id = "i" };
            RedirectToRouteResult result = new RedirectToRouteResult(new RouteValueDictionary(values)) {
                Routes = new RouteCollection() { new Route("{controller}/{action}/{id}", null) },
            };

            // Act
            result.ExecuteResult(context);

            // Assert
            mockResponse.Verify();
        }

        [TestMethod]
        public void ExecuteResultThrowsIfVirtualPathDataIsNull() {
            // Arrange
            ControllerContext context = new ControllerContext(new Mock<HttpContextBase>().Object, new RouteData(), new Mock<ControllerBase>().Object);
            var result = new RedirectToRouteResult(null) {
                Routes = new RouteCollection()
            };

            // Act & Assert
            ExceptionHelper.ExpectException<InvalidOperationException>(
                delegate {
                    result.ExecuteResult(context);
                },
                "No route in the route table matches the supplied values.");
        }

        [TestMethod]
        public void ExecuteResultWithNullControllerContextThrows() {
            // Arrange
            var result = new RedirectToRouteResult(null);

            // Act & Assert
            ExceptionHelper.ExpectArgumentNullException(
                delegate {
                    result.ExecuteResult(null /* context */);
                },
                "context");
        }

        [TestMethod]
        public void RoutesPropertyDefaultsToGlobalRouteTable() {
            // Act
            var result = new RedirectToRouteResult(new RouteValueDictionary());

            // Assert
            Assert.AreSame(RouteTable.Routes, result.Routes);
        }
    }
}
