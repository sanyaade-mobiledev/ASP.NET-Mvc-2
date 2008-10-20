namespace System.Web.Mvc.Test {
    using System;
    using System.Reflection;
    using System.Security.Principal;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Routing;
    using System.Web.TestUtil;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;

    [TestClass]
    public class AuthorizeAttributeTest {

        [TestMethod]
        public void AuthorizeCoreReturnsFalseIfNameDoesNotMatch() {
            // Arrange
            AuthorizeAttributeHelper helper = new AuthorizeAttributeHelper() { Users = "SomeName" };

            Mock<IIdentity> mockIdentity = new Mock<IIdentity>();
            mockIdentity.Expect(i => i.IsAuthenticated).Returns(true);
            mockIdentity.Expect(i => i.Name).Returns("SomeOtherName");
            Mock<IPrincipal> mockPrincipal = new Mock<IPrincipal>();
            mockPrincipal.Expect(p => p.Identity).Returns(mockIdentity.Object);

            // Act
            bool retVal = helper.PublicAuthorizeCore(mockPrincipal.Object);

            // Assert
            Assert.IsFalse(retVal);
        }

        [TestMethod]
        public void AuthorizeCoreReturnsFalseIfRoleDoesNotMatch() {
            // Arrange
            AuthorizeAttributeHelper helper = new AuthorizeAttributeHelper() { Roles = "SomeRole" };

            Mock<IIdentity> mockIdentity = new Mock<IIdentity>();
            mockIdentity.Expect(i => i.IsAuthenticated).Returns(true);
            Mock<IPrincipal> mockPrincipal = new Mock<IPrincipal>();
            mockPrincipal.Expect(p => p.IsInRole("SomeRole")).Returns(false).Verifiable();
            mockPrincipal.Expect(p => p.Identity).Returns(mockIdentity.Object);

            // Act
            bool retVal = helper.PublicAuthorizeCore(mockPrincipal.Object);

            // Assert
            Assert.IsFalse(retVal);
            mockPrincipal.Verify();
        }

        [TestMethod]
        public void AuthorizeCoreReturnsFalseIfUserIsUnauthenticated() {
            // Arrange
            AuthorizeAttributeHelper helper = new AuthorizeAttributeHelper();

            Mock<IIdentity> mockIdentity = new Mock<IIdentity>();
            mockIdentity.Expect(i => i.IsAuthenticated).Returns(false);
            Mock<IPrincipal> mockPrincipal = new Mock<IPrincipal>();
            mockPrincipal.Expect(p => p.Identity).Returns(mockIdentity.Object);
            
            // Act
            bool retVal = helper.PublicAuthorizeCore(mockPrincipal.Object);

            // Assert
            Assert.IsFalse(retVal);
        }

        [TestMethod]
        public void AuthorizeCoreReturnsTrueIfUserIsAuthenticatedAndNamesOrRolesSpecified() {
            // Arrange
            AuthorizeAttributeHelper helper = new AuthorizeAttributeHelper() { Users = "SomeUser, SomeOtherUser", Roles = "SomeRole, SomeOtherRole" };

            Mock<IIdentity> mockIdentity = new Mock<IIdentity>();
            mockIdentity.Expect(i => i.IsAuthenticated).Returns(true);
            mockIdentity.Expect(i => i.Name).Returns("SomeUser");
            Mock<IPrincipal> mockPrincipal = new Mock<IPrincipal>();
            mockPrincipal.Expect(p => p.Identity).Returns(mockIdentity.Object);
            mockPrincipal.Expect(p => p.IsInRole("SomeRole")).Returns(false).Verifiable();
            mockPrincipal.Expect(p => p.IsInRole("SomeOtherRole")).Returns(true).Verifiable();

            // Act
            bool retVal = helper.PublicAuthorizeCore(mockPrincipal.Object);

            // Assert
            Assert.IsTrue(retVal);
            mockPrincipal.Verify();
        }

        [TestMethod]
        public void AuthorizeCoreReturnsTrueIfUserIsAuthenticatedAndNoNamesOrRolesSpecified() {
            // Arrange
            AuthorizeAttributeHelper helper = new AuthorizeAttributeHelper();

            Mock<IIdentity> mockIdentity = new Mock<IIdentity>();
            mockIdentity.Expect(i => i.IsAuthenticated).Returns(true);
            Mock<IPrincipal> mockPrincipal = new Mock<IPrincipal>();
            mockPrincipal.Expect(p => p.Identity).Returns(mockIdentity.Object);

            // Act
            bool retVal = helper.PublicAuthorizeCore(mockPrincipal.Object);

            // Assert
            Assert.IsTrue(retVal);
        }

        [TestMethod]
        public void AuthorizeCoreThrowsIfUserIsNull() {
            // Arrange
            AuthorizeAttributeHelper helper = new AuthorizeAttributeHelper();

            // Act & assert
            ExceptionHelper.ExpectArgumentNullException(
                delegate {
                    helper.PublicAuthorizeCore(null);
                }, "user");
        }

        [TestMethod]
        public void OnAuthorizationCancelsRequestIfUserUnauthorized() {
            // Arrange
            Mock<AuthorizeAttributeHelper> mockHelper = new Mock<AuthorizeAttributeHelper>() { CallBase = true };
            mockHelper.Expect(h => h.PublicAuthorizeCore(It.IsAny<IPrincipal>())).Returns(false);
            AuthorizeAttributeHelper helper = mockHelper.Object;

            ControllerContext controllerContext = new ControllerContext(new Mock<HttpContextBase>().Object, new RouteData(), new Mock<ControllerBase>().Object);
            AuthorizationContext filterContext = new AuthorizationContext(controllerContext);

            // Act
            helper.OnAuthorization(filterContext);

            // Assert
            Assert.IsTrue(filterContext.Cancel);
            Assert.IsInstanceOfType(filterContext.Result, typeof(HttpUnauthorizedResult));
        }

        [TestMethod]
        public void OnAuthorizationHooksCacheValidationIfUserAuthorized() {
            // Arrange
            Mock<AuthorizeAttributeHelper> mockHelper = new Mock<AuthorizeAttributeHelper>() { CallBase = true };
            mockHelper.Expect(h => h.PublicAuthorizeCore(It.IsAny<IPrincipal>())).Returns(true);
            AuthorizeAttributeHelper helper = mockHelper.Object;

            MethodInfo callbackMethod = typeof(AuthorizeAttribute).GetMethod("CacheValidateHandler", BindingFlags.Instance | BindingFlags.NonPublic);
            Mock<HttpCachePolicyBase> mockCachePolicy = new Mock<HttpCachePolicyBase>();
            mockCachePolicy.Expect(c => c.SetProxyMaxAge(new TimeSpan(0))).Verifiable();
            mockCachePolicy.Expect(c => c.AddValidationCallback(It.IsAny<HttpCacheValidateHandler>(), null /* data */))
                .Callback(delegate(HttpCacheValidateHandler handler, object data) {
                    Assert.AreSame(helper, handler.Target);
                    Assert.AreSame(callbackMethod, handler.Method);
                })
                .Verifiable();

            Mock<HttpResponseBase> mockResponse = new Mock<HttpResponseBase>();
            mockResponse.Expect(r => r.Cache).Returns(mockCachePolicy.Object);
            Mock<HttpContextBase> mockHttpContext = new Mock<HttpContextBase>();
            mockHttpContext.Expect(c => c.Response).Returns(mockResponse.Object);
            ControllerContext controllerContext = new ControllerContext(mockHttpContext.Object, new RouteData(), new Mock<ControllerBase>().Object);
            AuthorizationContext filterContext = new AuthorizationContext(controllerContext);

            // Act
            helper.OnAuthorization(filterContext);

            // Assert
            mockCachePolicy.Verify();
        }

        [TestMethod]
        public void OnAuthorizationThrowsIfFilterContextIsNull() {
            // Arrange
            AuthorizeAttribute attr = new AuthorizeAttribute();

            // Act & assert
            ExceptionHelper.ExpectArgumentNullException(
                delegate {
                    attr.OnAuthorization(null);
                }, "filterContext");
        }

        [TestMethod]
        public void OnCacheAuthorizationReturnsIgnoreRequestIfUserIsUnauthorized() {
            // Arrange
            Mock<AuthorizeAttributeHelper> mockHelper = new Mock<AuthorizeAttributeHelper>() { CallBase = true };
            mockHelper.Expect(h => h.PublicAuthorizeCore(It.IsAny<IPrincipal>())).Returns(false);
            AuthorizeAttributeHelper helper = mockHelper.Object;

            Mock<HttpContextBase> mockHttpContext = new Mock<HttpContextBase>();
            mockHttpContext.Expect(c => c.User).Returns(new Mock<IPrincipal>().Object);

            // Act
            HttpValidationStatus validationStatus = helper.PublicOnCacheAuthorization(mockHttpContext.Object);

            // Assert
            Assert.AreEqual(HttpValidationStatus.IgnoreThisRequest, validationStatus);
        }

        [TestMethod]
        public void OnCacheAuthorizationReturnsValidIfUserIsAuthorized() {
            // Arrange
            Mock<AuthorizeAttributeHelper> mockHelper = new Mock<AuthorizeAttributeHelper>() { CallBase = true };
            mockHelper.Expect(h => h.PublicAuthorizeCore(It.IsAny<IPrincipal>())).Returns(true);
            AuthorizeAttributeHelper helper = mockHelper.Object;

            Mock<HttpContextBase> mockHttpContext = new Mock<HttpContextBase>();
            mockHttpContext.Expect(c => c.User).Returns(new Mock<IPrincipal>().Object);

            // Act
            HttpValidationStatus validationStatus = helper.PublicOnCacheAuthorization(mockHttpContext.Object);

            // Assert
            Assert.AreEqual(HttpValidationStatus.Valid, validationStatus);
        }

        [TestMethod]
        public void OnCacheAuthorizationThrowsIfHttpContextIsNull() {
            // Arrange
            AuthorizeAttributeHelper helper = new AuthorizeAttributeHelper();

            // Act & assert
            ExceptionHelper.ExpectArgumentNullException(
                delegate {
                    helper.PublicOnCacheAuthorization(null);
                }, "httpContext");
        }

        [TestMethod]
        public void RolesProperty() {
            // Arrange
            AuthorizeAttribute attr = new AuthorizeAttribute();

            // Act & assert
            MemberHelper.TestStringProperty(attr, "Roles", String.Empty, false /* testDefaultValue */, true /* allowNullAndEmpty */);
        }

        [TestMethod]
        public void UsersProperty() {
            // Arrange
            AuthorizeAttribute attr = new AuthorizeAttribute();

            // Act & assert
            MemberHelper.TestStringProperty(attr, "Users", String.Empty, false /* testDefaultValue */, true /* allowNullAndEmpty */);
        }

        public class AuthorizeAttributeHelper : AuthorizeAttribute {
            public virtual bool PublicAuthorizeCore(IPrincipal user) {
                return base.AuthorizeCore(user);
            }
            protected override bool AuthorizeCore(IPrincipal user) {
                return PublicAuthorizeCore(user);
            }
            public virtual HttpValidationStatus PublicOnCacheAuthorization(HttpContextBase httpContext) {
                return base.OnCacheAuthorization(httpContext);
            }
            protected override HttpValidationStatus OnCacheAuthorization(HttpContextBase httpContext) {
                return PublicOnCacheAuthorization(httpContext);
            }
        }

    }
}
