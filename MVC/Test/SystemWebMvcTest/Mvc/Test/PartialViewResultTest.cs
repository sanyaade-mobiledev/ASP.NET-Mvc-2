namespace System.Web.Mvc.Test {
    using System;
    using System.IO;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Routing;
    using System.Web.TestUtil;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;

    [TestClass]
    public class PartialViewResultTest {

        private const string _viewName = "My cool partial view.";

        [TestMethod]
        public void EmptyViewNameUsesActionNameAsViewName() {
            // Arrange
            ControllerBase controller = new Mock<ControllerBase>().Object;
            HttpContextBase httpContext = CreateHttpContext();
            RouteData routeData = new RouteData();
            routeData.Values["action"] = _viewName;
            ControllerContext context = new ControllerContext(httpContext, routeData, controller);
            Mock<IViewEngine> viewEngine = new Mock<IViewEngine>(MockBehavior.Strict);
            Mock<IView> view = new Mock<IView>(MockBehavior.Strict);
            PartialViewResult result = new PartialViewResultHelper { ViewEngine = viewEngine.Object };
            viewEngine
                .Expect(e => e.FindPartialView(It.IsAny<ControllerContext>(), _viewName))
                .Callback<ControllerContext, string>(
                    (controllerContext, viewName) => {
                        Assert.AreSame(httpContext, controllerContext.HttpContext);
                        Assert.AreSame(routeData, controllerContext.RouteData);
                    })
                .Returns(new ViewEngineResult(view.Object));
            view
                .Expect(o => o.Render(It.IsAny<ViewContext>(), httpContext.Response.Output))
                .Callback<ViewContext, TextWriter>(
                    (viewContext, writer) => {
                        Assert.AreEqual(_viewName, viewContext.ViewName);
                        Assert.AreSame(result.ViewData, viewContext.ViewData);
                        Assert.AreSame(result.TempData, viewContext.TempData);
                        Assert.AreSame(controller, viewContext.Controller);
                    });

            // Act
            result.ExecuteResult(context);

            // Assert
            viewEngine.Verify();
            view.Verify();
        }

        [TestMethod]
        public void EngineLookupFailureThrows() {
            // Arrange
            ControllerBase controller = new Mock<ControllerBase>().Object;
            HttpContextBase httpContext = CreateHttpContext();
            RouteData routeData = new RouteData();
            routeData.Values["action"] = _viewName;
            ControllerContext context = new ControllerContext(httpContext, routeData, controller);
            Mock<IViewEngine> viewEngine = new Mock<IViewEngine>(MockBehavior.Strict);
            PartialViewResult result = new PartialViewResultHelper { ViewEngine = viewEngine.Object };
            viewEngine
                .Expect(e => e.FindPartialView(It.IsAny<ControllerContext>(), _viewName))
                .Callback<ControllerContext, string>(
                    (controllerContext, viewName) => {
                        Assert.AreSame(httpContext, controllerContext.HttpContext);
                        Assert.AreSame(routeData, controllerContext.RouteData);
                    })
                .Returns(new ViewEngineResult(new[] { "location1", "location2" }));

            // Act & Assert
            ExceptionHelper.ExpectInvalidOperationException(
                () => result.ExecuteResult(context),
                "The partial view '" + _viewName + "' could not be found. The following locations were searched:\r\nlocation1\r\nlocation2");

            viewEngine.Verify();
        }

        [TestMethod]
        public void EngineLookupSuccessRendersView() {
            // Arrange
            ControllerBase controller = new Mock<ControllerBase>().Object;
            HttpContextBase httpContext = CreateHttpContext();
            RouteData routeData = new RouteData();
            ControllerContext context = new ControllerContext(httpContext, routeData, controller);
            Mock<IViewEngine> viewEngine = new Mock<IViewEngine>(MockBehavior.Strict);
            Mock<IView> view = new Mock<IView>(MockBehavior.Strict);
            PartialViewResult result = new PartialViewResultHelper { ViewName = _viewName, ViewEngine = viewEngine.Object };
            view
                .Expect(o => o.Render(It.IsAny<ViewContext>(), httpContext.Response.Output))
                .Callback<ViewContext, TextWriter>(
                    (viewContext, writer) => {
                        Assert.AreEqual(_viewName, viewContext.ViewName);
                        Assert.AreSame(result.ViewData, viewContext.ViewData);
                        Assert.AreSame(result.TempData, viewContext.TempData);
                        Assert.AreSame(controller, viewContext.Controller);
                    });
            viewEngine
                .Expect(e => e.FindPartialView(It.IsAny<ControllerContext>(), _viewName))
                .Callback<ControllerContext, string>(
                    (controllerContext, viewName) => {
                        Assert.AreSame(httpContext, controllerContext.HttpContext);
                        Assert.AreSame(routeData, controllerContext.RouteData);
                    })
                .Returns(new ViewEngineResult(view.Object));

            // Act
            result.ExecuteResult(context);

            // Assert
            viewEngine.Verify();
            view.Verify();
        }

        [TestMethod]
        public void ExecuteResultWithExplicitViewObject() {
            // Arrange
            ControllerBase controller = new Mock<ControllerBase>().Object;
            HttpContextBase httpContext = CreateHttpContext();
            RouteData routeData = new RouteData();
            routeData.Values["action"] = _viewName;
            ControllerContext context = new ControllerContext(httpContext, routeData, controller);
            Mock<IView> view = new Mock<IView>(MockBehavior.Strict);
            PartialViewResult result = new PartialViewResultHelper { View = view.Object };
            view
                .Expect(o => o.Render(It.IsAny<ViewContext>(), httpContext.Response.Output))
                .Callback<ViewContext, TextWriter>(
                    (viewContext, writer) => {
                        Assert.AreSame(_viewName, viewContext.ViewName);
                        Assert.AreSame(result.ViewData, viewContext.ViewData);
                        Assert.AreSame(result.TempData, viewContext.TempData);
                        Assert.AreSame(controller, viewContext.Controller);
                    });

            // Act
            result.ExecuteResult(context);

            // Assert
            view.Verify();
        }

        [TestMethod]
        public void ExecuteResultWithNullControllerContextThrows() {
            // Arrange
            PartialViewResult result = new PartialViewResultHelper();

            // Act & Assert
            ExceptionHelper.ExpectArgumentNullException(
                () => result.ExecuteResult(null),
                "context");
        }

        [TestMethod]
        public void TempDataProperty() {
            // Arrange
            TempDataDictionary newDict = new TempDataDictionary();
            PartialViewResult result = new PartialViewResult();

            // Act & Assert
            MemberHelper.TestPropertyWithDefaultInstance(result, "TempData", newDict);
        }

        [TestMethod]
        public void ViewDataProperty() {
            // Arrange
            ViewDataDictionary newDict = new ViewDataDictionary();
            PartialViewResult result = new PartialViewResult();

            // Act & Assert
            MemberHelper.TestPropertyWithDefaultInstance(result, "ViewData", newDict);
        }

        [TestMethod]
        public void ViewEngineProperty() {
            // Arrange
            IViewEngine newEngine = new Mock<IViewEngine>().Object;
            PartialViewResult result = new PartialViewResult();

            // Act & Assert
            MemberHelper.TestPropertyWithDefaultInstance(result, "ViewEngine", newEngine, ViewEngines.DefaultEngine);
        }

        [TestMethod]
        public void ViewNameProperty() {
            // Arrange
            PartialViewResult result = new PartialViewResult();

            // Act & Assert
            MemberHelper.TestStringProperty(result, "ViewName", String.Empty, false /* testDefaultValue */, true /* allowNullAndEmpty */);
        }

        private static HttpContextBase CreateHttpContext() {
            TextWriter writer = new Mock<TextWriter>().Object;
            Mock<HttpResponseBase> httpResponse = new Mock<HttpResponseBase>();
            httpResponse.Expect(r => r.Output).Returns(writer);
            Mock<HttpContextBase> result = new Mock<HttpContextBase>();
            result.Expect(c => c.Response).Returns(httpResponse.Object);
            return result.Object;
        }


        private class PartialViewResultHelper : PartialViewResult {
            public PartialViewResultHelper() {
                ViewEngine = new CompositeViewEngine(new ViewEngineCollection());
            }
        }

    }
}
