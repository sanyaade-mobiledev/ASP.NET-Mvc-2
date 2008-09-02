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
                    new ViewContext(null, "view", new ViewDataDictionary(), new TempDataDictionary());
                },
                "controllerContext");
        }

        [TestMethod]
        public void ConstructorSetsProperties() {
            // Arrange
            HttpContextBase httpContext = new Mock<HttpContextBase>().Object;
            ControllerBase controller = new Mock<ControllerBase>().Object;
            RouteData routeData = new RouteData();
            ViewDataDictionary viewData = new ViewDataDictionary();
            TempDataDictionary tempData = new TempDataDictionary();

            // Act
            ViewContext vc = new ViewContext(httpContext, routeData, controller, "view", viewData, tempData);

            // Assert
            Assert.AreEqual(httpContext, vc.HttpContext);
            Assert.AreEqual(routeData, vc.RouteData);
            Assert.AreEqual(controller, vc.Controller);
            Assert.AreEqual("view", vc.ViewName);
            Assert.AreEqual(viewData, vc.ViewData);
            Assert.AreEqual(tempData, vc.TempData);
        }

        [TestMethod]
        public void ConstructorSetsProperties2() {
            // Arrange
            HttpContextBase httpContext = new Mock<HttpContextBase>().Object;
            ControllerBase controller = new Mock<ControllerBase>().Object;
            RouteData routeData = new RouteData();
            ViewDataDictionary viewData = new ViewDataDictionary();
            TempDataDictionary tempData = new TempDataDictionary();

            // Act
            ViewContext vc = new ViewContext(new ControllerContext(httpContext, routeData, controller), "view", viewData, tempData);

            // Assert
            Assert.AreEqual(httpContext, vc.HttpContext);
            Assert.AreEqual(routeData, vc.RouteData);
            Assert.AreEqual(controller, vc.Controller);
            Assert.AreEqual("view", vc.ViewName);
            Assert.AreEqual(viewData, vc.ViewData);
            Assert.AreEqual(tempData, vc.TempData);
        }
    }
}
