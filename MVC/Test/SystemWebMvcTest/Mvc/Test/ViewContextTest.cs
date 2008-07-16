namespace System.Web.Mvc.Test {
    using System;
    using System.Web.Routing;
    using System.Web.TestUtil;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;

    [TestClass]
    public class ViewContextTest {
        [TestMethod]
        public void ConstructorWithNullControllerContextThrows() {
            ExceptionHelper.ExpectArgumentNullException(
                delegate {
                    new ViewContext(null, "view", null, new ViewDataDictionary(), new TempDataDictionary());
                },
                "controllerContext");
        }

        [TestMethod]
        public void ConstructorSetsProperties() {
            // Setup
            HttpContextBase httpContext = new Mock<HttpContextBase>().Object;
            IController controller = new Mock<IController>().Object;
            RouteData routeData = new RouteData();
            ViewDataDictionary viewData = new ViewDataDictionary();
            TempDataDictionary tempData = new TempDataDictionary();

            // Execute
            ViewContext vc = new ViewContext(httpContext, routeData, controller, "view", "master", viewData, tempData);

            // Verify
            Assert.AreEqual<HttpContextBase>(httpContext, vc.HttpContext);
            Assert.AreEqual<RouteData>(routeData, vc.RouteData);
            Assert.AreEqual<IController>(controller, vc.Controller);
            Assert.AreEqual("view", vc.ViewName);
            Assert.AreEqual("master", vc.MasterName);
            Assert.AreEqual(viewData, vc.ViewData);
            Assert.AreEqual<TempDataDictionary>(tempData, vc.TempData);
        }

        [TestMethod]
        public void ConstructorSetsProperties2() {
            // Setup
            HttpContextBase httpContext = new Mock<HttpContextBase>().Object;
            IController controller = new Mock<IController>().Object;
            RouteData routeData = new RouteData();
            ViewDataDictionary viewData = new ViewDataDictionary();
            TempDataDictionary tempData = new TempDataDictionary();

            // Execute
            ViewContext vc = new ViewContext(new ControllerContext(httpContext, routeData, controller), "view", "master", viewData, tempData);

            // Verify
            Assert.AreEqual<HttpContextBase>(httpContext, vc.HttpContext);
            Assert.AreEqual<RouteData>(routeData, vc.RouteData);
            Assert.AreEqual<IController>(controller, vc.Controller);
            Assert.AreEqual("view", vc.ViewName);
            Assert.AreEqual("master", vc.MasterName);
            Assert.AreEqual(viewData, vc.ViewData);
            Assert.AreEqual<TempDataDictionary>(tempData, vc.TempData);
        }
    }
}
