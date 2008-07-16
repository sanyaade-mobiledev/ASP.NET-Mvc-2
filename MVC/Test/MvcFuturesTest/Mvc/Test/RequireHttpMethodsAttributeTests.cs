namespace MvcFuturesTest.Mvc.Test {
    using System.Collections.Generic;
    using System.Reflection;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Routing;
    using System.Web.TestUtil;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Microsoft.Web.Mvc;
    using Moq;

    [TestClass]
    public class RequireHttpMethodAttributeTests {
        [TestMethod]
        public void CanGetAndSetRequiredMethods() {
            var filter = new RequireHttpMethodAttribute("POST", "GET");
            Assert.IsTrue(filter.Methods.Contains("POST"));
            Assert.IsTrue(filter.Methods.Contains("GET"));
            Assert.IsFalse(filter.Methods.Contains("PUT"));
        }

        [TestMethod]
        public void HttpMethodFilterWithPostSpecifiedAllowsPostRequest() {
            MethodInfo method = typeof(TestController).GetMethod("Index");

            var mockHttpRequest = new Mock<HttpRequestBase>();
            mockHttpRequest.Expect(r => r.HttpMethod).Returns("POST");

            var mockHttpContext = new Mock<HttpContextBase>();
            mockHttpContext.Expect(c => c.Request).Returns(mockHttpRequest.Object);

            var controllerContext = new ControllerContext(new RequestContext(mockHttpContext.Object, new RouteData()), new TestController());
            var filterContext = new ActionExecutingContext(controllerContext, method, new Dictionary<string, object>());

            var filter = new RequireHttpMethodAttribute("POST");
            filter.OnActionExecuting(filterContext);

            mockHttpRequest.VerifyAll();
        }

        [TestMethod]
        public void HttpMethodFilterWithPostSpecifiedDoesNotAllowGetRequest() {
            MethodInfo method = typeof(TestController).GetMethod("Index");

            var mockHttpRequest = new Mock<HttpRequestBase>();
            mockHttpRequest.Expect(r => r.HttpMethod).Returns("GET");

            var mockHttpContext = new Mock<HttpContextBase>();
            mockHttpContext.Expect(c => c.Request).Returns(mockHttpRequest.Object);

            var controllerContext = new ControllerContext(new RequestContext(mockHttpContext.Object, new RouteData()), new TestController());
            var filterContext = new ActionExecutingContext(controllerContext, method, new Dictionary<string, object>());

            var filter = new RequireHttpMethodAttribute("POST");
            HttpException exception = ExceptionHelper.ExpectException<HttpException>(() => filter.OnActionExecuting(filterContext), true);
            Assert.AreEqual(405, exception.GetHttpCode());
        }

        [TestMethod]
        public void HttpMethodFilterWithPostAndGetSpecifiedDoesNotAllowPutRequest() {
            MethodInfo method = typeof(TestController).GetMethod("Index");

            var mockHttpRequest = new Mock<HttpRequestBase>();
            mockHttpRequest.Expect(r => r.HttpMethod).Returns("PUT");

            var mockHttpContext = new Mock<HttpContextBase>();
            mockHttpContext.Expect(c => c.Request).Returns(mockHttpRequest.Object);

            var controllerContext = new ControllerContext(new RequestContext(mockHttpContext.Object, new RouteData()), new TestController());
            var filterContext = new ActionExecutingContext(controllerContext, method, new Dictionary<string, object>());

            var filter = new RequireHttpMethodAttribute(" pOsT", null, " get ");
            HttpException exception = ExceptionHelper.ExpectException<HttpException>(() => filter.OnActionExecuting(filterContext), true);
            Assert.AreEqual(405, exception.GetHttpCode());
        }

        [TestMethod]
        public void HttpMethodFilterWithNothingSpecifiedDoesNotAllowGetRequest() {
            MethodInfo method = typeof(TestController).GetMethod("Index");

            var mockHttpRequest = new Mock<HttpRequestBase>();
            mockHttpRequest.Expect(r => r.HttpMethod).Returns("GET");

            var mockHttpContext = new Mock<HttpContextBase>();
            mockHttpContext.Expect(c => c.Request).Returns(mockHttpRequest.Object);

            var controllerContext = new ControllerContext(new RequestContext(mockHttpContext.Object, new RouteData()), new TestController());
            var filterContext = new ActionExecutingContext(controllerContext, method, new Dictionary<string, object>());

            var filter = new RequireHttpMethodAttribute();
            HttpException exception = ExceptionHelper.ExpectException<HttpException>(() => filter.OnActionExecuting(filterContext), true);
            Assert.AreEqual(405, exception.GetHttpCode());
        }

        [TestMethod]
        public void HttpMethodFilterWithNullSpecifiedDoesNotAllowGetRequest()
        {
            MethodInfo method = typeof(TestController).GetMethod("Index");

            var mockHttpRequest = new Mock<HttpRequestBase>();
            mockHttpRequest.Expect(r => r.HttpMethod).Returns("GET");

            var mockHttpContext = new Mock<HttpContextBase>();
            mockHttpContext.Expect(c => c.Request).Returns(mockHttpRequest.Object);

            var controllerContext = new ControllerContext(new RequestContext(mockHttpContext.Object, new RouteData()), new TestController());
            var filterContext = new ActionExecutingContext(controllerContext, method, new Dictionary<string, object>());

            var filter = new RequireHttpMethodAttribute(null);
            HttpException exception = ExceptionHelper.ExpectException<HttpException>(() => filter.OnActionExecuting(filterContext), true);
            Assert.AreEqual(405, exception.GetHttpCode());
        }

        public class TestController : Controller {
            public string Index() {
                return "Did it work?";
            }
        }
    }
}
