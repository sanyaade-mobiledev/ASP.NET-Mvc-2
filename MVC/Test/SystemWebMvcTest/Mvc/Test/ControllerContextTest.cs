namespace System.Web.Mvc.Test {
    using System.Web.Routing;
    using System.Web.TestUtil;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;

    [TestClass]
    public class ControllerContextTest {
        [TestMethod]
        public void ConstructorWithNullRequestContextThrows() {
            ExceptionHelper.ExpectArgumentNullException(
                delegate {
                    new ControllerContext(null, new Mock<ControllerBase>().Object);
                },
                "requestContext");
        }

        [TestMethod]
        public void ConstructorWithNullControllerThrows() {
            ExceptionHelper.ExpectArgumentNullException(
                delegate {
                    new ControllerContext(new RequestContext(new Mock<HttpContextBase>().Object, new RouteData()), null);
                },
                "controller");
        }

        [TestMethod]
        public void ConstructorWithNullControllerContextThrows() {
            ExceptionHelper.ExpectArgumentNullException(
                delegate {
                    new ControllerContext((ControllerContext)null /* controllerContext */);
                },
                "controllerContext");
        }

        [TestMethod]
        public void ConstructorWithNullHttpContextThrows() {
            ExceptionHelper.ExpectArgumentNullException(
                delegate {
                    new ControllerContext(null, new RouteData(), new Mock<ControllerBase>().Object);
                },
                "httpContext");
        }

        [TestMethod]
        public void ConstructorWithNullRouteDataThrows() {
            ExceptionHelper.ExpectArgumentNullException(
                delegate {
                    new ControllerContext(new Mock<HttpContextBase>().Object, null, new Mock<ControllerBase>().Object);
                },
                "routeData");
        }

        [TestMethod]
        public void ConstructorWithNullControllerThrows2() {
            ExceptionHelper.ExpectArgumentNullException(
                delegate {
                    new ControllerContext(new Mock<HttpContextBase>().Object, new RouteData(), null);
                },
                "controller");
        }

        [TestMethod]
        public void ConstructorSetsProperties() {
            // Arrange
            HttpContextBase httpContext = GetEmptyContextForTempData();
            ControllerBase controller = new Mock<ControllerBase>().Object;
            RouteData routeData = new RouteData();

            // Act
            ControllerContext cc = new ControllerContext(httpContext, routeData, controller);

            // Assert
            Assert.AreEqual(httpContext, cc.HttpContext);
            Assert.AreEqual(routeData, cc.RouteData);
            Assert.AreEqual(controller, cc.Controller);
        }

        [TestMethod]
        public void ConstructorSetsProperties2() {
            // Arrange
            HttpContextBase httpContext = GetEmptyContextForTempData();
            ControllerBase controller = new Mock<ControllerBase>().Object;
            RouteData routeData = new RouteData();

            // Act
            ControllerContext cc = new ControllerContext(new RequestContext(httpContext, routeData), controller);

            // Assert
            Assert.AreEqual(httpContext, cc.HttpContext);
            Assert.AreEqual(routeData, cc.RouteData);
            Assert.AreEqual(controller, cc.Controller);
        }

        [TestMethod]
        public void ConstructorSetsProperties3() {
            // Arrange
            HttpContextBase httpContext = GetEmptyContextForTempData();
            ControllerBase controller = new Mock<ControllerBase>().Object;
            RouteData routeData = new RouteData();

            // Act
            ControllerContext cc1 = new ControllerContext(new RequestContext(httpContext, routeData), controller);
            ControllerContext cc2 = new ControllerContext(cc1);

            // Assert
            Assert.AreEqual(cc1.HttpContext, cc2.HttpContext);
            Assert.AreEqual(cc1.RouteData, cc2.RouteData);
            Assert.AreEqual(cc1.Controller, cc2.Controller);
        }

        internal static ControllerContext GetControllerContext() {
            return new ControllerContext(GetEmptyContextForTempData(), new RouteData(), new Mock<ControllerBase>().Object);
        }

        internal static HttpContextBase GetEmptyContextForTempData() {
            Mock<HttpContextBase> mockContext = new Mock<HttpContextBase>();
            mockContext.Expect(o => o.Session).Returns((HttpSessionStateBase)null);
            return mockContext.Object;
        }
    }
}
