namespace System.Web.Mvc.Test {
    using System;
    using System.Web.Routing;
    using System.Web.TestUtil;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;

    [TestClass]
    public class RedirectResultTest {

        private static string _baseUrl = "http://www.contoso.com/";

        [TestMethod]
        public void ConstructorSetsUrl() {
            // Execute
            var result = new RedirectResult(_baseUrl);

            // Verify
            Assert.AreSame(_baseUrl, result.Url);
        }

        [TestMethod]
        public void ConstructorWithEmptyUrlThrows() {
            // Execute & verify
            ExceptionHelper.ExpectArgumentExceptionNullOrEmpty(
                delegate {
                    new RedirectResult(String.Empty);
                },
                "url");
        }

        [TestMethod]
        public void ConstructorWithNullUrlThrows() {
            // Execute & verify
            ExceptionHelper.ExpectArgumentExceptionNullOrEmpty(
                delegate {
                    new RedirectResult(null /* url */);
                },
                "url");
        }

        [TestMethod]
        public void ExecuteResultCallsResponseRedirect() {
            // Setup
            Mock<HttpResponseBase> mockResponse = new Mock<HttpResponseBase>();
            mockResponse.Expect(o => o.Redirect(_baseUrl));
            Mock<HttpContextBase> mockContext = new Mock<HttpContextBase>();
            mockContext.Expect(o => o.Response).Returns(mockResponse.Object);
            ControllerContext context = new ControllerContext(mockContext.Object, new RouteData(), new Mock<IController>().Object);
            var result = new RedirectResult(_baseUrl);

            // Execute
            result.ExecuteResult(context);

            // Verify
            mockResponse.Verify();
        }

        [TestMethod]
        public void ExecuteResultWithNullControllerContextThrows() {
            // Setup
            var result = new RedirectResult(_baseUrl);

            // Execute & verify
            ExceptionHelper.ExpectArgumentNullException(
                delegate {
                    result.ExecuteResult(null /* context */);
                },
                "context");
        }

    }
}
