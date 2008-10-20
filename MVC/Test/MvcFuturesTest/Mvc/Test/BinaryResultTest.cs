namespace MvcFuturesTest.Mvc.Test {
    using System;
    using System.Net.Mime;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Routing;
    using System.Web.TestUtil;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Microsoft.Web.Mvc;
    using Moq;

    [TestClass]
    public class BinaryResultTest {

        [TestMethod]
        public void ContentTypeProperty() {
            // Arrange
            BinaryResult result = new EmptyBinaryResult();
            string defaultContentType = MediaTypeNames.Application.Octet;

            // Act & assert
            MemberHelper.TestStringProperty(result, "ContentType", defaultContentType, false /* testDefaultValue */, true /* allowNullAndEmpty */, defaultContentType);
        }

        [TestMethod]
        public void ExecuteResultSetsContentDispositionIfSpecified() {
            // Arrange
            Mock<HttpResponseBase> mockResponse = new Mock<HttpResponseBase>(MockBehavior.Strict);
            mockResponse.ExpectSetProperty(r => r.ContentType, "application/my-type").Verifiable();
            mockResponse.Expect(r => r.AddHeader("Content-Disposition", "attachment; filename=filename.ext")).Verifiable();

            ControllerContext context = GetControllerContext(mockResponse.Object);
            BinaryResult result = new EmptyBinaryResult() {
                ContentType = "application/my-type",
                FileDownloadName = "filename.ext" 
            };

            // Act
            result.ExecuteResult(context);

            // Assert
            mockResponse.Verify();
        }

        [TestMethod]
        public void ExecuteResultThrowsIfContextIsNull() {
            // Arrange
            BinaryResult result = new EmptyBinaryResult();

            // Act & assert
            ExceptionHelper.ExpectArgumentNullException(
                delegate {
                    result.ExecuteResult(null);
                }, "context");
        }

        [TestMethod]
        public void FileDownloadNameProperty() {
            // Arrange
            BinaryResult result = new EmptyBinaryResult();

            // Act & assert
            MemberHelper.TestStringProperty(result, "FileDownloadName", String.Empty, false /* testDefaultValue */, true /* allowNullAndEmpty */);
        }

        public static ControllerContext GetControllerContext(HttpResponseBase httpResponse) {
            Mock<HttpContextBase> httpContext = new Mock<HttpContextBase>();
            httpContext.Expect(ctx => ctx.Response).Returns(httpResponse);
            return new ControllerContext(httpContext.Object, new RouteData(), new Mock<ControllerBase>().Object);
        }

        private class EmptyBinaryResult : BinaryResult {
        }

    }
}
