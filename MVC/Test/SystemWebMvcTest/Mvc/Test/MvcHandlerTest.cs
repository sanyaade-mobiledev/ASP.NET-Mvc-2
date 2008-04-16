namespace System.Web.Mvc.Test {
    using System;
    using System.Web.Routing;
    using System.Web.TestUtil;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;

    [TestClass]
    public class MvcHandlerTest {
        [TestMethod]
        public void ConstructorWithNullRequestContextThrows() {
            ExceptionHelper.ExpectArgumentNullException(
                delegate {
                    new MvcHandler(null);
                },
                "requestContext");
        }

        [TestMethod]
        public void ProcessRequestWithRouteWithoutControllerThrows() {
            // Setup
            Mock<HttpContextBase> contextMock = new Mock<HttpContextBase>();
            RouteData rd = new RouteData();
            MvcHandler mvcHandler = new MvcHandler(new RequestContext(contextMock.Object, rd));

            // Execute
            ExceptionHelper.ExpectException<InvalidOperationException>(
                delegate {
                    mvcHandler.ProcessRequest(contextMock.Object);
                },
                "The RouteData must contain an item named 'controller' with a non-empty string value.");
        }

        [TestMethod]
        public void ProcessRequestCallsExecute() {
            // Setup
            Mock<HttpContextBase> contextMock = new Mock<HttpContextBase>();
            RouteData rd = new RouteData();
            rd.Values.Add("controller", "foo");
            MvcHandler mvcHandler = new MvcHandler(new RequestContext(contextMock.Object, rd));

            Mock<IController> controllerMock = new Mock<IController>();
            controllerMock.Expect(o => o.Execute(It.IsAny<ControllerContext>())).Verifiable();

            ControllerBuilder cb = new ControllerBuilder();
            Mock<IControllerFactory> controllerFactoryMock = new Mock<IControllerFactory>();
            controllerFactoryMock.Expect(o => o.CreateController(It.IsAny<RequestContext>(), "foo")).Returns(controllerMock.Object);
            controllerFactoryMock.Expect(o => o.DisposeController(controllerMock.Object));
            cb.SetControllerFactory(controllerFactoryMock.Object);
            mvcHandler.ControllerBuilder = cb;

            // Execute
            mvcHandler.ProcessRequest(contextMock.Object);

            // Verify
            controllerMock.Verify();
        }
    }
}
