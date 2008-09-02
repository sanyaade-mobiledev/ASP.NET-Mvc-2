namespace MvcFuturesTest.Mvc.Test {
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Routing;
    using System.Web.TestUtil;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Microsoft.Web.Mvc;
    using Moq;

    [TestClass]
    public class ValidateInputAttributeTests {

        [TestMethod]
        public void OnAuthorizationCallsValidateInput() {
            // Arrange
            ValidateInputAttribute attr = new ValidateInputAttribute();

            Mock<HttpContextBase> httpContextMock = new Mock<HttpContextBase>();
            Mock<HttpRequestBase> httpRequestMock = new Mock<HttpRequestBase>();
            httpContextMock.Expect(c => c.Request).Returns(httpRequestMock.Object);
            httpRequestMock.Expect(r => r.ValidateInput()).Verifiable();
            ControllerContext controllerContext = new ControllerContext(httpContextMock.Object, new RouteData(), new Mock<ControllerBase>().Object);
            AuthorizationContext filterContext = new AuthorizationContext(controllerContext);

            // Act
            attr.OnAuthorization(filterContext);

            // Assert
            httpRequestMock.Verify();
        }

        [TestMethod]
        public void OnAuthorizationThrowsIfFilterContextIsNull() {
            // Arrange
            ValidateInputAttribute attr = new ValidateInputAttribute();

            // Act & Assert
            ExceptionHelper.ExpectArgumentNullException(
                delegate {
                    attr.OnAuthorization(null);
                }, "filterContext");
        }

    }
}
