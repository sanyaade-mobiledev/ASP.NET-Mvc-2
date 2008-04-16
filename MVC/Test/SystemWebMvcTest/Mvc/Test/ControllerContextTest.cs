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
                    new ControllerContext(null, new Mock<IController>().Object);
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
                    new ControllerContext(null, new RouteData(), new Mock<IController>().Object);
                },
                "httpContext");
        }

        [TestMethod]
        public void ConstructorWithNullRouteDataThrows() {
            ExceptionHelper.ExpectArgumentNullException(
                delegate {
                    new ControllerContext(new Mock<HttpContextBase>().Object, null, new Mock<IController>().Object);
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
            // Setup
            HttpContextBase httpContext = GetEmptyContextForTempData();
            IController controller = new Mock<IController>().Object;
            RouteData routeData = new RouteData();

            // Execute
            ControllerContext cc = new ControllerContext(httpContext, routeData, controller);

            // Verify
            Assert.AreEqual<HttpContextBase>(httpContext, cc.HttpContext);
            Assert.AreEqual<RouteData>(routeData, cc.RouteData);
            Assert.AreEqual<IController>(controller, cc.Controller);
        }

        [TestMethod]
        public void ConstructorSetsProperties2() {
            // Setup
            HttpContextBase httpContext = GetEmptyContextForTempData();
            IController controller = new Mock<IController>().Object;
            RouteData routeData = new RouteData();

            // Execute
            ControllerContext cc = new ControllerContext(new RequestContext(httpContext, routeData), controller);

            // Verify
            Assert.AreEqual<HttpContextBase>(httpContext, cc.HttpContext);
            Assert.AreEqual<RouteData>(routeData, cc.RouteData);
            Assert.AreEqual<IController>(controller, cc.Controller);
        }

        [TestMethod]
        public void ConstructorSetsProperties3() {
            // Setup
            HttpContextBase httpContext = GetEmptyContextForTempData();
            IController controller = new Mock<IController>().Object;
            RouteData routeData = new RouteData();

            // Execute
            ControllerContext cc1 = new ControllerContext(new RequestContext(httpContext, routeData), controller);
            ControllerContext cc2 = new ControllerContext(cc1);

            // Verify
            Assert.AreEqual(cc1.HttpContext, cc2.HttpContext);
            Assert.AreEqual(cc1.RouteData, cc2.RouteData);
            Assert.AreEqual(cc1.Controller, cc2.Controller);
        }

        internal static ControllerContext GetControllerContext() {
            return new ControllerContext(GetEmptyContextForTempData(), new RouteData(), new Mock<IController>().Object);
        }

        internal static HttpContextBase GetEmptyContextForTempData() {
            Mock<HttpContextBase> mockContext = new Mock<HttpContextBase>();
            mockContext.Expect(o => o.Session).Returns((HttpSessionStateBase)null);
            return mockContext.Object;
        }
    }
}
