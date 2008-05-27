namespace System.Web.Mvc.Test {
    using System.Text;
    using System.Web;
    using System.Web.Routing;
    using System.Web.TestUtil;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;

    [TestClass]
    public class JsonResultTest {
        private static readonly object _jsonData = new object[] { 1, 2, "three", "four" };
        private static readonly string _jsonSerializedData = "[1,2,\"three\",\"four\"]";

        [TestMethod]
        public void AllPropertiesDefaultToNull() {
            // Setup & execute
            JsonResult result = new JsonResult();

            // Verify
            Assert.IsNull(result.Data);
            Assert.IsNull(result.ContentEncoding);
            Assert.IsNull(result.ContentType);
        }

        [TestMethod]
        public void EmptyContentTypeRendersDefault() {
            // Setup
            object data = _jsonData;
            Encoding contentEncoding = Encoding.UTF8;
            Mock<HttpResponseBase> mockResponse = new Mock<HttpResponseBase>(MockBehavior.Strict);

            // Setup expectations
            mockResponse.ExpectSetProperty(response => response.ContentType, "application/json").Verifiable();
            mockResponse.ExpectSetProperty(response => response.ContentEncoding, contentEncoding).Verifiable();
            mockResponse.Expect(response => response.Write(_jsonSerializedData)).Verifiable();

            ControllerContext controllerContext = GetControllerContext(mockResponse.Object);
            JsonResult result = new JsonResult {
                Data = data,
                ContentType = String.Empty,
                ContentEncoding = contentEncoding
            };

            // Execute
            result.ExecuteResult(controllerContext);

            // Verify
            mockResponse.Verify();
        }

        [TestMethod]
        public void ExecuteResult() {
            // Setup
            object data = _jsonData;
            string contentType = "Some content type.";
            Encoding contentEncoding = Encoding.UTF8;
            Mock<HttpResponseBase> mockResponse = new Mock<HttpResponseBase>(MockBehavior.Strict);

            // Setup expectations
            mockResponse.ExpectSetProperty(response => response.ContentType, contentType).Verifiable();
            mockResponse.ExpectSetProperty(response => response.ContentEncoding, contentEncoding).Verifiable();
            mockResponse.Expect(response => response.Write(_jsonSerializedData)).Verifiable();

            ControllerContext controllerContext = GetControllerContext(mockResponse.Object);
            JsonResult result = new JsonResult {
                Data = data,
                ContentType = contentType,
                ContentEncoding = contentEncoding
            };

            // Execute
            result.ExecuteResult(controllerContext);

            // Verify
            mockResponse.Verify();
        }

        [TestMethod]
        public void ExecuteResultWithNullContextThrows() {
            ExceptionHelper.ExpectArgumentNullException(
                delegate {
                    new JsonResult().ExecuteResult(null /* context */);
                }, "context");
        }

        [TestMethod]
        public void NullContentIsNotOutput() {
            // Setup
            string contentType = "Some content type.";
            Encoding contentEncoding = Encoding.UTF8;
            Mock<HttpResponseBase> mockResponse = new Mock<HttpResponseBase>(MockBehavior.Strict);

            // Setup expectations
            mockResponse.ExpectSetProperty(response => response.ContentType, contentType).Verifiable();
            mockResponse.ExpectSetProperty(response => response.ContentEncoding, contentEncoding).Verifiable();

            ControllerContext controllerContext = GetControllerContext(mockResponse.Object);
            JsonResult result = new JsonResult {
                ContentType = contentType,
                ContentEncoding = contentEncoding
            };

            // Execute
            result.ExecuteResult(controllerContext);

            // Verify
            mockResponse.Verify();
        }

        [TestMethod]
        public void NullContentEncodingIsNotOutput() {
            // Setup
            object data = _jsonData;
            string contentType = "Some content type.";
            Mock<HttpResponseBase> mockResponse = new Mock<HttpResponseBase>(MockBehavior.Strict);

            // Setup expectations
            mockResponse.ExpectSetProperty(response => response.ContentType, contentType).Verifiable();
            mockResponse.Expect(response => response.Write(_jsonSerializedData)).Verifiable();

            ControllerContext controllerContext = GetControllerContext(mockResponse.Object);
            JsonResult result = new JsonResult {
                Data = data,
                ContentType = contentType,
            };

            // Execute
            result.ExecuteResult(controllerContext);

            // Verify
            mockResponse.Verify();
        }

        [TestMethod]
        public void NullContentTypeRendersDefault() {
            // Setup
            object data = _jsonData;
            Encoding contentEncoding = Encoding.UTF8;
            Mock<HttpResponseBase> mockResponse = new Mock<HttpResponseBase>(MockBehavior.Strict);

            // Setup expectations
            mockResponse.ExpectSetProperty(response => response.ContentType, "application/json").Verifiable();
            mockResponse.ExpectSetProperty(response => response.ContentEncoding, contentEncoding).Verifiable();
            mockResponse.Expect(response => response.Write(_jsonSerializedData)).Verifiable();

            ControllerContext controllerContext = GetControllerContext(mockResponse.Object);
            JsonResult result = new JsonResult {
                Data = data,
                ContentEncoding = contentEncoding
            };

            // Execute
            result.ExecuteResult(controllerContext);

            // Verify
            mockResponse.Verify();
        }

        private static ControllerContext GetControllerContext(HttpResponseBase httpResponse) {
            Mock<HttpContextBase> httpContext = new Mock<HttpContextBase>();
            httpContext.Expect(ctx => ctx.Response).Returns(httpResponse);
            return new ControllerContext(httpContext.Object, new RouteData(), new Mock<IController>().Object);
        }
    }
}
