namespace MvcFuturesTest.Mvc.Test {
    using System;
    using System.Text;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Routing;
    using System.Web.TestUtil;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Microsoft.Web.Mvc;
    using Moq;

    [TestClass]
    public class FileResultTests {
        [TestMethod]
        public void CtorWithFilePathSetsFilePathAndNothingElse() {
            var result = new FileResult("foo.txt");
            Assert.AreEqual("foo.txt", result.FilePath);

            Assert.IsNull(result.ContentEncoding);
            Assert.IsNull(result.ContentType);
            Assert.IsNull(result.FileDownloadName);
        }

        [TestMethod]
        public void CtorWithNullFilePathThrowsException() {
            ExceptionHelper.ExpectArgumentException(
                delegate {
                    new FileResult(null);
                }, "Value cannot be null or empty." + Environment.NewLine + "Parameter name: filePath");
        }

        [TestMethod]
        public void ExecuteResultNullControllerContextThrowsException() {
            FileResult result = new FileResult(@"c:\foo");
            ExceptionHelper.ExpectArgumentNullException(() => result.ExecuteResult(null), "context");
        }

        [TestMethod]
        public void ExecuteResultWithFilePathTransmitsFile() {
            // Arrange
            var mockResponse = new Mock<HttpResponseBase>();

            // Arrange expectations
            string transmittedFilePath = null;
            mockResponse.Expect(response => response.TransmitFile(It.IsAny<string>())).Callback<string>(s => transmittedFilePath = s);

            ControllerContext controllerContext = GetControllerContext(mockResponse.Object);
            var result = new FileResult(@"c:\foo.txt");

            // Act
            result.ExecuteResult(controllerContext);

            Assert.AreEqual(@"c:\foo.txt", transmittedFilePath);
        }

        [TestMethod]
        public void WhenContentTypeSetExecuteResultSetsResponseContentType() {
            // Arrange
            var mockResponse = new Mock<HttpResponseBase>();

            // Arrange expectations
            mockResponse.ExpectSetProperty(r => r.ContentType, "text/xml").Verifiable();
            mockResponse.Expect(response => response.TransmitFile(It.IsAny<string>()));

            ControllerContext controllerContext = GetControllerContext(mockResponse.Object);
            var result = new FileResult(@"c:\bar.txt") { ContentType = "text/xml" };

            // Act
            result.ExecuteResult(controllerContext);

            mockResponse.Verify();
        }

        [TestMethod]
        public void WhenContentEncodingSetExecuteResultSetsResponseEncoding() {
            // Arrange
            var mockResponse = new Mock<HttpResponseBase>();

            // Arrange expectations
            mockResponse.ExpectSetProperty(r => r.ContentEncoding, Encoding.UTF32).Verifiable();
            mockResponse.Expect(response => response.TransmitFile(It.IsAny<string>()));

            ControllerContext controllerContext = GetControllerContext(mockResponse.Object);
            var result = new FileResult(@"c:\bar.txt") { ContentEncoding = Encoding.UTF32};

            // Act
            result.ExecuteResult(controllerContext);

            mockResponse.Verify();
        }

        [TestMethod]
        public void WhenFileDownloadNameSetResponseAddsContentDispositionHeader() {
            // Arrange
            var mockResponse = new Mock<HttpResponseBase>();

            // Arrange expectations
            mockResponse.Expect(r => r.AddHeader("Content-disposition", "attachment; filename=coolstuff.jpg"));
            mockResponse.Expect(response => response.TransmitFile(It.IsAny<string>()));

            ControllerContext controllerContext = GetControllerContext(mockResponse.Object);
            var result = new FileResult(@"c:\bar.txt") { FileDownloadName = "coolstuff.jpg" };

            // Act
            result.ExecuteResult(controllerContext);

            mockResponse.Verify();
        }

        [TestMethod]
        public void ExecuteResultWithNullContextThrows() {
            ExceptionHelper.ExpectArgumentNullException(
                delegate {
                    new ContentResult().ExecuteResult(null /* context */);
                }, "context");
        }

        private static ControllerContext GetControllerContext(HttpResponseBase httpResponse) {
            Mock<HttpContextBase> httpContext = new Mock<HttpContextBase>();
            httpContext.Expect(ctx => ctx.Response).Returns(httpResponse);
            return new ControllerContext(httpContext.Object, new RouteData(), new Mock<ControllerBase>().Object);
        }
    }
}
