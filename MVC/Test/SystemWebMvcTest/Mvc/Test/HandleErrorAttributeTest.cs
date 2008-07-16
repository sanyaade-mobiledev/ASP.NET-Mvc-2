namespace System.Web.Mvc.Test {
    using System;
    using System.Reflection;
    using System.Web.Routing;
    using System.Web.TestUtil;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;

    [TestClass]
    public class HandleErrorAttributeTest {

        [TestMethod]
        public void ExceptionTypeProperty() {
            // Setup
            HandleErrorAttribute attr = new HandleErrorAttribute();

            // Execute
            Type origType = attr.ExceptionType;
            attr.ExceptionType = typeof(SystemException);
            Type newType = attr.ExceptionType;

            // Verify
            Assert.AreEqual(typeof(Exception), origType);
            Assert.AreEqual(typeof(SystemException), attr.ExceptionType);
        }

        [TestMethod]
        public void ExceptionTypePropertyWithNonExceptionTypeThrows() {
            // Setup
            HandleErrorAttribute attr = new HandleErrorAttribute();

            // Execute & verify
            ExceptionHelper.ExpectArgumentException(
                delegate {
                    attr.ExceptionType = typeof(string);
                },
                "The type 'System.String' does not inherit from Exception.");
        }

        [TestMethod]
        public void ExceptionTypePropertyWithNullValueThrows() {
            // Setup
            HandleErrorAttribute attr = new HandleErrorAttribute();

            // Execute & verify
            ExceptionHelper.ExpectArgumentNullException(
                delegate {
                    attr.ExceptionType = null;
                }, "value");
        }

        [TestMethod]
        public void OnException() {
            // Setup
            HandleErrorAttribute attr = new HandleErrorAttribute() { View = "SomeView", ExceptionType = typeof(ArgumentException) };
            Exception exception = new ArgumentNullException();

            Mock<HttpContextBase> mockHttpContext = new Mock<HttpContextBase>();
            mockHttpContext.Expect(c => c.Session).Returns((HttpSessionStateBase)null);
            Mock<HttpResponseBase> mockHttpResponse = new Mock<HttpResponseBase>();
            mockHttpResponse.Expect(r => r.Clear()).Verifiable();
            mockHttpResponse.ExpectSetProperty(r => r.StatusCode, 500).Verifiable();
            mockHttpContext.Expect(c => c.Response).Returns(mockHttpResponse.Object);
            TempDataDictionary tempData = new TempDataDictionary();
            IViewEngine viewEngine = new Mock<IViewEngine>().Object;
            Controller controller = new Mock<Controller>().Object;
            controller.TempData = tempData;
            controller.ViewEngine = viewEngine;

            ExceptionContext context = GetFilterContext(mockHttpContext.Object, controller, exception);

            // Exception
            attr.OnException(context);

            // Verify
            mockHttpResponse.Verify();
            ViewResult viewResult = context.Result as ViewResult;
            Assert.IsNotNull(viewResult, "The Result property should have been set to an instance of ViewResult.");
            Assert.AreEqual(viewEngine, viewResult.ViewEngine);
            Assert.AreEqual(tempData, viewResult.TempData);
            Assert.AreEqual("SomeView", viewResult.ViewName);

            HandleErrorInfo viewData = viewResult.ViewData.Model as HandleErrorInfo;
            Assert.IsNotNull(viewData, "The ViewData model should have been set to an instance of ExceptionViewData.");
            Assert.AreSame(exception, viewData.Exception);
            Assert.AreEqual("SomeController", viewData.Controller);
            Assert.AreEqual("SomeAction", viewData.Action);
        }

        [TestMethod]
        public void OnExceptionChecksInnerExceptionForTargetInvocationException() {
            // Setup
            HandleErrorAttribute attr = new HandleErrorAttribute() { View = "SomeView", ExceptionType = typeof(ArgumentException) };
            Exception innerException = new ArgumentNullException();
            Exception outerException = new TargetInvocationException(innerException);

            Mock<HttpContextBase> mockHttpContext = new Mock<HttpContextBase>();
            mockHttpContext.Expect(c => c.Session).Returns((HttpSessionStateBase)null);
            Mock<HttpResponseBase> mockHttpResponse = new Mock<HttpResponseBase>();
            mockHttpResponse.Expect(r => r.Clear()).Verifiable();
            mockHttpResponse.ExpectSetProperty(r => r.StatusCode, 500).Verifiable();
            mockHttpContext.Expect(c => c.Response).Returns(mockHttpResponse.Object);
            TempDataDictionary tempData = new TempDataDictionary();
            IViewEngine viewEngine = new Mock<IViewEngine>().Object;
            Controller controller = new Mock<Controller>().Object;
            controller.TempData = tempData;
            controller.ViewEngine = viewEngine;

            ExceptionContext context = GetFilterContext(mockHttpContext.Object, controller, outerException);

            // Exception
            attr.OnException(context);

            // Verify
            mockHttpResponse.Verify();
            ViewResult viewResult = context.Result as ViewResult;
            Assert.IsNotNull(viewResult, "The Result property should have been set to an instance of ViewResult.");
            Assert.AreEqual(viewEngine, viewResult.ViewEngine);
            Assert.AreEqual(tempData, viewResult.TempData);
            Assert.AreEqual("SomeView", viewResult.ViewName);

            HandleErrorInfo viewData = viewResult.ViewData.Model as HandleErrorInfo;
            Assert.IsNotNull(viewData, "The ViewData model should have been set to an instance of ExceptionViewData.");
            Assert.AreSame(outerException, viewData.Exception);
            Assert.AreEqual("SomeController", viewData.Controller);
            Assert.AreEqual("SomeAction", viewData.Action);
        }

        [TestMethod]
        public void OnExceptionWithExceptionHandledDoesNothing() {
            // Setup
            HandleErrorAttribute attr = new HandleErrorAttribute();
            ActionResult result = new EmptyResult();
            ExceptionContext context = GetFilterContext(new Mock<HttpContextBase>().Object, new Mock<Controller>().Object, new Exception());
            context.Result = result;
            context.ExceptionHandled = true;

            // Exception
            attr.OnException(context);

            // Verify
            Assert.AreSame(result, context.Result, "The context's Result property should have remain unchanged.");
        }

        [TestMethod]
        public void OnExceptionWithNon404ExceptionDoesNothing() {
            // Setup
            HandleErrorAttribute attr = new HandleErrorAttribute();
            ActionResult result = new EmptyResult();
            ExceptionContext context = GetFilterContext(new Mock<HttpContextBase>().Object, new Mock<Controller>().Object, new HttpException(404, "Some Exception"));
            context.Result = result;

            // Exception
            attr.OnException(context);

            // Verify
            Assert.AreSame(result, context.Result, "The context's Result property should have remain unchanged.");
        }

        [TestMethod]
        public void OnExceptionWithNonControllerDoesNothing() {
            // Setup
            HandleErrorAttribute attr = new HandleErrorAttribute();
            ActionResult result = new EmptyResult();
            ExceptionContext context = GetFilterContext(new Mock<HttpContextBase>().Object, new Mock<IController>().Object, new Exception());
            context.Result = result;

            // Exception
            attr.OnException(context);

            // Verify
            Assert.AreSame(result, context.Result, "The context's Result property should have remain unchanged.");
        }

        [TestMethod]
        public void OnExceptionWithNullFilterContextThrows() {
            // Setup
            HandleErrorAttribute attr = new HandleErrorAttribute();

            // Execute & verify
            ExceptionHelper.ExpectArgumentNullException(
                delegate {
                    attr.OnException(null /* filterContext */);
                }, "filterContext");
        }

        [TestMethod]
        public void OnExceptionWithWrongExceptionTypeDoesNothing() {
            // Setup
            HandleErrorAttribute attr = new HandleErrorAttribute() { ExceptionType = typeof(ArgumentException) };
            ActionResult result = new EmptyResult();
            ExceptionContext context = GetFilterContext(new Mock<HttpContextBase>().Object, new Mock<Controller>().Object, new InvalidCastException());
            context.Result = result;

            // Exception
            attr.OnException(context);

            // Verify
            Assert.AreSame(result, context.Result, "The context's Result property should have remain unchanged.");
        }

        [TestMethod]
        public void ViewProperty() {
            // Setup
            HandleErrorAttribute attr = new HandleErrorAttribute();

            // Execute & verify
            MemberHelper.TestStringProperty(attr, "View", "Error", false /* testDefaultValue */, true /* allowNullAndEmpty */, "Error");
        }

        private static ExceptionContext GetFilterContext(HttpContextBase httpContext, IController controller, Exception exception) {
            RouteData rd = new RouteData();
            rd.Values["controller"] = "SomeController";
            rd.Values["action"] = "SomeAction";
            ControllerContext controllerContext = new ControllerContext(httpContext, rd, controller);
            return new ExceptionContext(controllerContext, exception);
        }

    }
}
