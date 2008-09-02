namespace MvcFuturesTest.Mvc.Test {
    using System;
    using System.Collections.Specialized;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Routing;
    using System.Web.TestUtil;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Microsoft.Web.Mvc;
    using Moq;

    [TestClass]
    public class ValidateAntiForgeryTokenAttributeTest {

        [TestMethod]
        public void OnAuthorizationDoesNothingIfTokensAreValid() {
            // Arrange
            AuthorizationContext authContext = GetAuthorizationContext("2001-01-01:some value:", "2001-01-02:some value:the real salt");
            ValidateAntiForgeryTokenAttribute attribute = GetAttribute();

            // Act
            attribute.OnAuthorization(authContext);

            // Assert
            // If we got to this point, no exception was thrown, so success.
        }

        [TestMethod]
        public void OnAuthorizationThrowsIfCookieMissing() {
            ExpectValidationException(null, "2001-01-01:some other value:the real salt");
        }

        [TestMethod]
        public void OnAuthorizationThrowsIfCookieValueDoesNotMatchFormValue() {
            ExpectValidationException("2001-01-01:some value:the real salt", "2001-01-01:some other value:the real salt");
        }

        [TestMethod]
        public void OnAuthorizationThrowsIfFilterContextIsNull() {
            // Arrange
            ValidateAntiForgeryTokenAttribute attribute = new ValidateAntiForgeryTokenAttribute();

            // Act & Assert
            ExceptionHelper.ExpectArgumentNullException(
                delegate {
                    attribute.OnAuthorization(null);
                }, "filterContext");
        }

        [TestMethod]
        public void OnAuthorizationThrowsIfFormSaltDoesNotMatchAttributeSalt() {
            ExpectValidationException("2001-01-01:some value:some salt", "2001-01-01:some value:some other salt");
        }

        [TestMethod]
        public void OnAuthorizationThrowsIfFormValueMissing() {
            ExpectValidationException("2001-01-01:some value:the real salt", null);
        }

        [TestMethod]
        public void SaltProperty() {
            // Arrange
            ValidateAntiForgeryTokenAttribute attribute = new ValidateAntiForgeryTokenAttribute();

            // Act & Assert
            MemberHelper.TestStringProperty(attribute, "Salt", String.Empty, false /* testDefaultValue */, true /* allowNullAndEmpty */);
        }

        [TestMethod]
        public void SerializerProperty() {
            // Arrange
            ValidateAntiForgeryTokenAttribute attribute = new ValidateAntiForgeryTokenAttribute();
            AntiForgeryTokenSerializer defaultSerializer = DefaultAntiForgeryTokenSerializer.Instance;
            AntiForgeryTokenSerializer newSerializer = new Mock<AntiForgeryTokenSerializer>().Object;

            // Act & Assert
            MemberHelper.TestPropertyWithDefaultInstance(attribute, "Serializer", newSerializer, defaultSerializer);
        }

        public static AuthorizationContext GetAuthorizationContext(string cookieValue, string formValue) {
            Mock<HttpRequestBase> mockRequest = new Mock<HttpRequestBase>();
            HttpCookieCollection requestCookies = new HttpCookieCollection();
            mockRequest.Expect(r => r.Cookies).Returns(requestCookies);
            if (!String.IsNullOrEmpty(cookieValue)) {
                requestCookies.Set(new HttpCookie("__MVC_AntiForgeryToken", cookieValue));
            }

            NameValueCollection formCollection = new NameValueCollection();
            if (!String.IsNullOrEmpty(formValue)) {
                formCollection.Set("__MVC_AntiForgeryToken", formValue);
            }
            mockRequest.Expect(r => r.Form).Returns(formCollection);

            Mock<HttpContextBase> mockHttpContext = new Mock<HttpContextBase>();
            mockHttpContext.Expect(c => c.Request).Returns(mockRequest.Object);

            ControllerContext controllerContext = new ControllerContext(mockHttpContext.Object, new RouteData(), new Mock<ControllerBase>().Object);
            return new AuthorizationContext(controllerContext);
        }

        private static ValidateAntiForgeryTokenAttribute GetAttribute() {
            AntiForgeryTokenSerializer serializer = new AntiForgeryHelperTest.SubclassedAntiForgeryTokenSerializer();
            return new SubclassedValidateAntiForgeryTokenAttribute(serializer);
        }

        private class SubclassedValidateAntiForgeryTokenAttribute : ValidateAntiForgeryTokenAttribute {
            public SubclassedValidateAntiForgeryTokenAttribute(AntiForgeryTokenSerializer serializer) {
                Salt = "the real salt";
                Serializer = serializer;
            }
        }

        private static void ExpectValidationException(string cookieValue, string formValue) {
            // Arrange
            ValidateAntiForgeryTokenAttribute attribute = GetAttribute();
            AuthorizationContext authContext = GetAuthorizationContext(cookieValue, formValue);

            // Act & Assert
            ExceptionHelper.ExpectException<AntiForgeryTokenValidationException>(delegate {
                attribute.OnAuthorization(authContext);
                },
                "A required anti-forgery token was not supplied or was invalid.");
        }

    }
}
