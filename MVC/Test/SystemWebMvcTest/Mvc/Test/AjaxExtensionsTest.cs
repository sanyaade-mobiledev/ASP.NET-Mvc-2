namespace System.Web.Mvc.Test {
    using System.Web;
    using System.Web.Mvc;
    using System.Web.TestUtil;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;

    [TestClass]
    public class AjaxExtensionsTest {

        [TestMethod]
        public void IsMvcAjaxRequestWithKeyIsTrue() {
            // Setup
            Mock<HttpRequestBase> mockRequest = new Mock<HttpRequestBase>();
            mockRequest.Expect(r => r["__MVCAJAX"]).Returns("true").Verifiable();
            HttpRequestBase request = mockRequest.Object;

            // Execute
            bool retVal = AjaxExtensions.IsMvcAjaxRequest(request);

            // Verify
            Assert.IsTrue(retVal);
            mockRequest.Verify();
        }

        [TestMethod]
        public void IsMvcAjaxRequestWithoutKeyIsFalse() {
            // Setup
            Mock<HttpRequestBase> mockRequest = new Mock<HttpRequestBase>();
            mockRequest.Expect(r => r["__MVCAJAX"]).Returns((string)null).Verifiable();
            HttpRequestBase request = mockRequest.Object;

            // Execute
            bool retVal = AjaxExtensions.IsMvcAjaxRequest(request);

            // Verify
            Assert.IsFalse(retVal);
            mockRequest.Verify();
        }

        [TestMethod]
        public void IsMvcAjaxRequestWithNullRequestThrows() {
            ExceptionHelper.ExpectArgumentNullException(
                delegate {
                    AjaxExtensions.IsMvcAjaxRequest(null);
                }, "request");
        }

    }
}
