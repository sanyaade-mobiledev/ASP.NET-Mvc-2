namespace System.Web.Mvc.Test {
    using System.Web;
    using System.Web.Routing;
    using System.Web.TestUtil;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;

    [TestClass]
    public class HttpUnauthorizedResultTest {

        [TestMethod]
        public void ExecuteResult() {
            // Arrange
            Mock<HttpResponseBase> mockResponse = new Mock<HttpResponseBase>(MockBehavior.Strict);
            mockResponse.ExpectSetProperty(response => response.StatusCode, 401).Verifiable();
            ControllerContext controllerContext = GetControllerContext(mockResponse.Object);
            HttpUnauthorizedResult result = new HttpUnauthorizedResult();

            // Act
            result.ExecuteResult(controllerContext);

            // Assert
            mockResponse.Verify();
        }

        [TestMethod]
        public void ExecuteResultWithNullContextThrows() {
            ExceptionHelper.ExpectArgumentNullException(
                delegate {
                    new HttpUnauthorizedResult().ExecuteResult(null /* context */);
                }, "context");
        }

        private static ControllerContext GetControllerContext(HttpResponseBase httpResponse) {
            Mock<HttpContextBase> httpContext = new Mock<HttpContextBase>();
            httpContext.Expect(ctx => ctx.Response).Returns(httpResponse);
            return new ControllerContext(httpContext.Object, new RouteData(), new Mock<ControllerBase>().Object);
        }
    }
}
