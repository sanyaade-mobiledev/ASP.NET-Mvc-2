namespace System.Web.Mvc.Test {
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
        public void NullFilterContextThrows() {
            // Arrange
            AuthorizeAttribute attr = new AuthorizeAttribute();

            // Act & Assert
            ExceptionHelper.ExpectArgumentNullException(
                delegate {
                    attr.OnAuthorization(null /* filterContext */);
                }, "filterContext");
        }

        [TestMethod]
        public void StringPropertiesReturnEmptyInsteadOfNull() {
            // Arrange
            AuthorizeAttribute attr = new AuthorizeAttribute();

            // Act & Assert
            MemberHelper.TestStringProperty(attr, "Roles", String.Empty, false /* testDefaultValue */);
            MemberHelper.TestStringProperty(attr, "Users", String.Empty, false /* testDefaultValue */);
        }

        [TestMethod]
        public void UserIsNotAuthenticatedThrows() {
            // Arrange
            AuthorizeAttribute attr = new AuthorizeAttribute();
            Mock<IIdentity> mockIdentity = new Mock<IIdentity>();
            mockIdentity.Expect(i => i.IsAuthenticated).Returns(false).Verifiable();
            Mock<IPrincipal> mockPrincipal = new Mock<IPrincipal>();
            mockPrincipal.Expect(p => p.Identity).Returns(mockIdentity.Object).Verifiable();
            AuthorizationContext filterContext = GetFilterContext(mockPrincipal.Object);

            // Act
            attr.OnAuthorization(filterContext);

            // Assert
            Assert.IsTrue(filterContext.Cancel, "Action should have been canceled.");
            Assert.IsInstanceOfType(filterContext.Result, typeof(HttpUnauthorizedResult));
            mockIdentity.Verify();
            mockPrincipal.Verify();
        }

        [TestMethod]
        public void UserIsWrongNameThrows() {
            // Arrange
            AuthorizeAttribute attr = new AuthorizeAttribute() { Users = "SomeName, YetAnotherName" };
            Mock<IIdentity> mockIdentity = new Mock<IIdentity>();
            mockIdentity.Expect(i => i.IsAuthenticated).Returns(true).Verifiable();
            mockIdentity.Expect(i => i.Name).Returns("SomeOtherName").Verifiable();
            Mock<IPrincipal> mockPrincipal = new Mock<IPrincipal>();
            mockPrincipal.Expect(p => p.Identity).Returns(mockIdentity.Object).Verifiable();
            AuthorizationContext filterContext = GetFilterContext(mockPrincipal.Object);

            // Act
            attr.OnAuthorization(filterContext);

            // Assert
            Assert.IsTrue(filterContext.Cancel, "Action should have been canceled.");
            Assert.IsInstanceOfType(filterContext.Result, typeof(HttpUnauthorizedResult));
            mockIdentity.Verify();
            mockPrincipal.Verify();
        }

        [TestMethod]
        public void UserNotInRoleThrows() {
            // Arrange
            AuthorizeAttribute attr = new AuthorizeAttribute() { Roles = "SomeRole, SomeOtherRole, , YetAnotherRole" };
            Mock<IIdentity> mockIdentity = new Mock<IIdentity>();
            mockIdentity.Expect(i => i.IsAuthenticated).Returns(true).Verifiable();
            Mock<IPrincipal> mockPrincipal = new Mock<IPrincipal>();
            mockPrincipal.Expect(p => p.Identity).Returns(mockIdentity.Object).Verifiable();
            mockPrincipal.Expect(p => p.IsInRole("SomeRole")).Returns(false).Verifiable();
            mockPrincipal.Expect(p => p.IsInRole("SomeOtherRole")).Returns(false).Verifiable();
            mockPrincipal.Expect(p => p.IsInRole("YetAnotherRole")).Returns(false).Verifiable();
            AuthorizationContext filterContext = GetFilterContext(mockPrincipal.Object);

            // Act
            attr.OnAuthorization(filterContext);

            // Assert
            Assert.IsTrue(filterContext.Cancel, "Action should have been canceled.");
            Assert.IsInstanceOfType(filterContext.Result, typeof(HttpUnauthorizedResult));
            mockIdentity.Verify();
            mockPrincipal.Verify();
        }

        [TestMethod]
        public void ValidUser() {
            // Arrange
            AuthorizeAttribute attr = new AuthorizeAttribute();
            Mock<IIdentity> mockIdentity = new Mock<IIdentity>();
            mockIdentity.Expect(i => i.IsAuthenticated).Returns(true).Verifiable();
            Mock<IPrincipal> mockPrincipal = new Mock<IPrincipal>();
            mockPrincipal.Expect(p => p.Identity).Returns(mockIdentity.Object).Verifiable();
            AuthorizationContext filterContext = GetFilterContext(mockPrincipal.Object);

            // Act
            attr.OnAuthorization(filterContext);

            // Assert
            Assert.IsFalse(filterContext.Cancel);
            mockIdentity.Verify();
            mockPrincipal.Verify();
        }

        [TestMethod]
        public void ValidUserWithNameAndRole() {
            // Arrange
            AuthorizeAttribute attr = new AuthorizeAttribute() {
                Roles = "SomeRole, SomeOtherRole, YetAnotherRole",
                Users = "somename, someothername"
            };
            Mock<IIdentity> mockIdentity = new Mock<IIdentity>();
            mockIdentity.Expect(i => i.IsAuthenticated).Returns(true).Verifiable();
            mockIdentity.Expect(i => i.Name).Returns("SomeName").Verifiable();
            Mock<IPrincipal> mockPrincipal = new Mock<IPrincipal>();
            mockPrincipal.Expect(p => p.Identity).Returns(mockIdentity.Object).Verifiable();
            mockPrincipal.Expect(p => p.IsInRole("SomeRole")).Returns(false).Verifiable();
            mockPrincipal.Expect(p => p.IsInRole("SomeOtherRole")).Returns(true).Verifiable();
            AuthorizationContext filterContext = GetFilterContext(mockPrincipal.Object);

            // Act
            attr.OnAuthorization(filterContext);

            // Assert
            Assert.IsFalse(filterContext.Cancel);
            mockIdentity.Verify();
            mockPrincipal.Verify();
        }

        private static AuthorizationContext GetFilterContext(IPrincipal user) {
            Mock<HttpContextBase> mockHttpContext = new Mock<HttpContextBase>();
            mockHttpContext.Expect(c => c.User).Returns(user);
            ControllerContext controllerContext = new ControllerContext(mockHttpContext.Object, new RouteData(),
                new Mock<ControllerBase>().Object);
            return new AuthorizationContext(controllerContext);
        }
    }
}
