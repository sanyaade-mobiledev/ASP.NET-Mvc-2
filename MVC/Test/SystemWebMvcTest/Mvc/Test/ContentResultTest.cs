namespace System.Web.Mvc.Test {
    using System.Linq.Expressions;
    using System.Reflection;
    using System.Text;
    using System.Web;
    using System.Web.Routing;
    using System.Web.TestUtil;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using Moq.Language.Flow;

    [TestClass]
    public class ContentResultTest {

        [TestMethod]
        public void AllPropertiesDefaultToNull() {
            // Act
            ContentResult result = new ContentResult();

            // Assert
            Assert.IsNull(result.Content);
            Assert.IsNull(result.ContentEncoding);
            Assert.IsNull(result.ContentType);
        }

        [TestMethod]
        public void EmptyContentTypeIsNotOutput() {
            // Arrange
            string content = "Some content.";
            Encoding contentEncoding = Encoding.UTF8;
            Mock<HttpResponseBase> mockResponse = new Mock<HttpResponseBase>(MockBehavior.Strict);

            // Arrange expectations
            mockResponse.ExpectSetProperty(response => response.ContentEncoding, contentEncoding).Verifiable();
            mockResponse.Expect(response => response.Write(content)).Verifiable();

            ControllerContext controllerContext = GetControllerContext(mockResponse.Object);
            ContentResult result = new ContentResult {
                Content = content,
                ContentType = String.Empty,
                ContentEncoding = contentEncoding
            };

            // Act
            result.ExecuteResult(controllerContext);

            // Assert
            mockResponse.Verify();
        }

        [TestMethod]
        public void ExecuteResult() {
            // Arrange
            string content = "Some content.";
            string contentType = "Some content type.";
            Encoding contentEncoding = Encoding.UTF8;
            Mock<HttpResponseBase> mockResponse = new Mock<HttpResponseBase>(MockBehavior.Strict);
            
            // Arrange expectations
            mockResponse.ExpectSetProperty(response => response.ContentType, contentType).Verifiable();
            mockResponse.ExpectSetProperty(response => response.ContentEncoding, contentEncoding).Verifiable();
            mockResponse.Expect(response => response.Write(content)).Verifiable();

            ControllerContext controllerContext = GetControllerContext(mockResponse.Object);
            ContentResult result = new ContentResult {
                Content = content,
                ContentType = contentType,
                ContentEncoding = contentEncoding
            };

            // Act
            result.ExecuteResult(controllerContext);

            // Assert
            mockResponse.Verify();
        }

        [TestMethod]
        public void ExecuteResultWithNullContextThrows() {
            ExceptionHelper.ExpectArgumentNullException(
                delegate {
                    new ContentResult().ExecuteResult(null /* context */);
                }, "context");
        }

        [TestMethod]
        public void NullContentIsNotOutput() {
            // Arrange
            string contentType = "Some content type.";
            Encoding contentEncoding = Encoding.UTF8;
            Mock<HttpResponseBase> mockResponse = new Mock<HttpResponseBase>(MockBehavior.Strict);

            // Arrange expectations
            mockResponse.ExpectSetProperty(response => response.ContentType, contentType).Verifiable();
            mockResponse.ExpectSetProperty(response => response.ContentEncoding, contentEncoding).Verifiable();

            ControllerContext controllerContext = GetControllerContext(mockResponse.Object);
            ContentResult result = new ContentResult {
                ContentType = contentType,
                ContentEncoding = contentEncoding
            };

            // Act
            result.ExecuteResult(controllerContext);

            // Assert
            mockResponse.Verify();
        }

        [TestMethod]
        public void NullContentEncodingIsNotOutput() {
            // Arrange
            string content = "Some content.";
            string contentType = "Some content type.";
            Mock<HttpResponseBase> mockResponse = new Mock<HttpResponseBase>(MockBehavior.Strict);

            // Arrange expectations
            mockResponse.ExpectSetProperty(response => response.ContentType, contentType).Verifiable();
            mockResponse.Expect(response => response.Write(content)).Verifiable();

            ControllerContext controllerContext = GetControllerContext(mockResponse.Object);
            ContentResult result = new ContentResult {
                Content = content,
                ContentType = contentType,
            };

            // Act
            result.ExecuteResult(controllerContext);

            // Assert
            mockResponse.Verify();
        }

        [TestMethod]
        public void NullContentTypeIsNotOutput() {
            // Arrange
            string content = "Some content.";
            Encoding contentEncoding = Encoding.UTF8;
            Mock<HttpResponseBase> mockResponse = new Mock<HttpResponseBase>(MockBehavior.Strict);

            // Arrange expectations
            mockResponse.ExpectSetProperty(response => response.ContentEncoding, contentEncoding).Verifiable();
            mockResponse.Expect(response => response.Write(content)).Verifiable();

            ControllerContext controllerContext = GetControllerContext(mockResponse.Object);
            ContentResult result = new ContentResult {
                Content = content,
                ContentEncoding = contentEncoding
            };

            // Act
            result.ExecuteResult(controllerContext);

            // Assert
            mockResponse.Verify();
        }

        private static ControllerContext GetControllerContext(HttpResponseBase httpResponse) {
            Mock<HttpContextBase> httpContext = new Mock<HttpContextBase>();
            httpContext.Expect(ctx => ctx.Response).Returns(httpResponse);
            return new ControllerContext(httpContext.Object, new RouteData(), new Mock<ControllerBase>().Object);
        }
    }
}
