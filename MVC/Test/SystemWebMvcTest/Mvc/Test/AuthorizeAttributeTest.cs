namespace System.Web.Mvc.Test {
    using System.Security;
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
            // Setup
            AuthorizeAttribute attr = new AuthorizeAttribute();

            // Execute & verify
            ExceptionHelper.ExpectArgumentNullException(
                delegate {
                    attr.OnAuthorization(null /* filterContext */);
                }, "filterContext");
        }

        [TestMethod]
        public void StringPropertiesReturnEmptyInsteadOfNull() {
            // Setup
            AuthorizeAttribute attr = new AuthorizeAttribute();

            // Execute & verify
            MemberHelper.TestStringProperty(attr, "Roles", String.Empty, false /* testDefaultValue */);
            MemberHelper.TestStringProperty(attr, "Users", String.Empty, false /* testDefaultValue */);
        }

        [TestMethod]
        public void UserIsNotAuthenticatedThrows() {
            // Setup
            AuthorizeAttribute attr = new AuthorizeAttribute();
            Mock<IIdentity> mockIdentity = new Mock<IIdentity>();
            mockIdentity.Expect(i => i.IsAuthenticated).Returns(false).Verifiable();
            Mock<IPrincipal> mockPrincipal = new Mock<IPrincipal>();
            mockPrincipal.Expect(p => p.Identity).Returns(mockIdentity.Object).Verifiable();
            AuthorizationContext filterContext = GetFilterContext(mockPrincipal.Object);

            // Execute
            attr.OnAuthorization(filterContext);

            // Verify
            Assert.IsTrue(filterContext.Cancel, "Action should have been canceled.");
            Assert.IsInstanceOfType(filterContext.Result, typeof(HttpUnauthorizedResult));
            mockIdentity.Verify();
            mockPrincipal.Verify();
        }

        [TestMethod]
        public void UserIsWrongNameThrows() {
            // Setup
            AuthorizeAttribute attr = new AuthorizeAttribute() { Users = "SomeName, YetAnotherName" };
            Mock<IIdentity> mockIdentity = new Mock<IIdentity>();
            mockIdentity.Expect(i => i.IsAuthenticated).Returns(true).Verifiable();
            mockIdentity.Expect(i => i.Name).Returns("SomeOtherName").Verifiable();
            Mock<IPrincipal> mockPrincipal = new Mock<IPrincipal>();
            mockPrincipal.Expect(p => p.Identity).Returns(mockIdentity.Object).Verifiable();
            AuthorizationContext filterContext = GetFilterContext(mockPrincipal.Object);

            // Execute
            attr.OnAuthorization(filterContext);

            // Verify
            Assert.IsTrue(filterContext.Cancel, "Action should have been canceled.");
            Assert.IsInstanceOfType(filterContext.Result, typeof(HttpUnauthorizedResult));
            mockIdentity.Verify();
            mockPrincipal.Verify();
        }

        [TestMethod]
        public void UserNotInRoleThrows() {
            // Setup
            AuthorizeAttribute attr = new AuthorizeAttribute() { Roles = "SomeRole, SomeOtherRole, , YetAnotherRole" };
            Mock<IIdentity> mockIdentity = new Mock<IIdentity>();
            mockIdentity.Expect(i => i.IsAuthenticated).Returns(true).Verifiable();
            Mock<IPrincipal> mockPrincipal = new Mock<IPrincipal>();
            mockPrincipal.Expect(p => p.Identity).Returns(mockIdentity.Object).Verifiable();
            mockPrincipal.Expect(p => p.IsInRole("SomeRole")).Returns(false).Verifiable();
            mockPrincipal.Expect(p => p.IsInRole("SomeOtherRole")).Returns(false).Verifiable();
            mockPrincipal.Expect(p => p.IsInRole("YetAnotherRole")).Returns(false).Verifiable();
            AuthorizationContext filterContext = GetFilterContext(mockPrincipal.Object);

            // Execute
            attr.OnAuthorization(filterContext);

            // Verify
            Assert.IsTrue(filterContext.Cancel, "Action should have been canceled.");
            Assert.IsInstanceOfType(filterContext.Result, typeof(HttpUnauthorizedResult));
            mockIdentity.Verify();
            mockPrincipal.Verify();
        }

        [TestMethod]
        public void ValidUser() {
            // Setup
            AuthorizeAttribute attr = new AuthorizeAttribute();
            Mock<IIdentity> mockIdentity = new Mock<IIdentity>();
            mockIdentity.Expect(i => i.IsAuthenticated).Returns(true).Verifiable();
            Mock<IPrincipal> mockPrincipal = new Mock<IPrincipal>();
            mockPrincipal.Expect(p => p.Identity).Returns(mockIdentity.Object).Verifiable();
            AuthorizationContext filterContext = GetFilterContext(mockPrincipal.Object);

            // Execute
            attr.OnAuthorization(filterContext);

            // Verify
            Assert.IsFalse(filterContext.Cancel);
            mockIdentity.Verify();
            mockPrincipal.Verify();
        }

        [TestMethod]
        public void ValidUserWithNameAndRole() {
            // Setup
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

            // Execute
            attr.OnAuthorization(filterContext);

            // Verify
            Assert.IsFalse(filterContext.Cancel);
            mockIdentity.Verify();
            mockPrincipal.Verify();
        }

        private static AuthorizationContext GetFilterContext(IPrincipal user) {
            Mock<HttpContextBase> mockHttpContext = new Mock<HttpContextBase>();
            mockHttpContext.Expect(c => c.User).Returns(user);
            ControllerContext controllerContext = new ControllerContext(mockHttpContext.Object, new RouteData(),
                new Mock<IController>().Object);
            return new AuthorizationContext(controllerContext, typeof(object).GetMethod("ToString"));
        }
    }
}
