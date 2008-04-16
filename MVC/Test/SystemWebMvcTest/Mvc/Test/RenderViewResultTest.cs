namespace System.Web.Mvc.Test {
    using System;
    using System.Web.Routing;
    using System.Web.TestUtil;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;

    [TestClass]
    public class RenderViewResultTest {

        private static string _viewName = "My cool view.";
        private static string _masterName = "My cool master.";
        private static object _viewData = new object();

        [TestMethod]
        public void ExecuteResultCorrectlyPassesDataToViewEngine() {
            // Setup
            IController controller = new Mock<IController>().Object;
            ControllerContext context = new ControllerContext(new Mock<HttpContextBase>().Object, new RouteData(), controller);
            TempDataDictionary tempData = GetTempDataDictionary();
            Mock<IViewEngine> viewEngineMock = new Mock<IViewEngine>();
            viewEngineMock.Expect(o => o.RenderView(It.IsAny<ViewContext>())).Callback<ViewContext>(delegate(ViewContext viewContext) {
                Assert.AreSame(_viewName, viewContext.ViewName);
                Assert.AreSame(_masterName, viewContext.MasterName);
                Assert.AreSame(_viewData, viewContext.ViewData);
                Assert.AreSame(tempData, viewContext.TempData);
                Assert.AreSame(controller, viewContext.Controller);
            });
            RenderViewResult result = new RenderViewResult() {
                ViewName = _viewName,
                MasterName = _masterName,
                ViewData = _viewData,
                ViewEngine = viewEngineMock.Object,
                TempData = tempData
            };

            // Execute
            result.ExecuteResult(context);

            // Verify
            viewEngineMock.Verify();
        }

        [TestMethod]
        public void ExecuteResultWithNullControllerContextThrows() {
            // Setup
            IViewEngine viewEngine = new Mock<IViewEngine>().Object;
            TempDataDictionary tempData = GetTempDataDictionary();
            RenderViewResult result = new RenderViewResult();

            // Execute & verify
            ExceptionHelper.ExpectArgumentNullException(
                delegate {
                    result.ExecuteResult(null /* context */);
                },
                "context");
        }

        [TestMethod]
        public void ExecuteResultWithNullViewEngineThrows() {
            // Setup
            RenderViewResult result = new RenderViewResult();
            IController controller = new Mock<IController>().Object;
            ControllerContext context = new ControllerContext(new Mock<HttpContextBase>().Object, new RouteData(), controller);

            // Execute & verify
            ExceptionHelper.ExpectException<InvalidOperationException>(
                delegate {
                    result.ExecuteResult(context);
                },
                "The property 'ViewEngine' cannot be null or empty.");
        }

        [TestMethod]
        public void ViewNameComesFromControllerContextIfEmptyViewName() {
            // Setup
            IController controller = new Mock<IController>().Object;
            RouteData rd = new RouteData();
            rd.Values["action"] = "FooAction";
            ControllerContext context = new ControllerContext(new Mock<HttpContextBase>().Object, rd, controller);
            Mock<IViewEngine> viewEngineMock = new Mock<IViewEngine>();
            viewEngineMock.Expect(o => o.RenderView(It.IsAny<ViewContext>())).Callback<ViewContext>(delegate(ViewContext viewContext) {
                Assert.AreSame("FooAction", viewContext.ViewName);
            });
            RenderViewResult result = new RenderViewResult() { ViewName = "", ViewEngine = viewEngineMock.Object };

            // Execute
            result.ExecuteResult(context);

            // Verify
            viewEngineMock.Verify();
        }

        [TestMethod]
        public void ViewNameComesFromControllerContextIfNullViewName() {
            // Setup
            IController controller = new Mock<IController>().Object;
            RouteData rd = new RouteData();
            rd.Values["action"] = "FooAction";
            ControllerContext context = new ControllerContext(new Mock<HttpContextBase>().Object, rd, controller);
            Mock<IViewEngine> viewEngineMock = new Mock<IViewEngine>();
            viewEngineMock.Expect(o => o.RenderView(It.IsAny<ViewContext>())).Callback<ViewContext>(delegate(ViewContext viewContext) {
                Assert.AreSame("FooAction", viewContext.ViewName);
            });
            RenderViewResult result = new RenderViewResult() { ViewEngine = viewEngineMock.Object };

            // Execute
            result.ExecuteResult(context);

            // Verify
            viewEngineMock.Verify();
        }

        private static TempDataDictionary GetTempDataDictionary() {
            Mock<HttpContextBase> mockContext = new Mock<HttpContextBase>();
            mockContext.Expect(o => o.Session).Returns((HttpSessionStateBase)null);
            return new TempDataDictionary(mockContext.Object);
        }
    }
}
