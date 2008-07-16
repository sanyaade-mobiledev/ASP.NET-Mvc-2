namespace System.Web.Mvc.Test {
    using System.Reflection;
    using System.Web.Mvc;
    using System.Web.TestUtil;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class AuthorizationContextTest {

        [TestMethod]
        public void ConstructorWithNullActionMethodThrows() {
            // Execute & verify
            ExceptionHelper.ExpectArgumentNullException(
                delegate {
                    new AuthorizationContext(ControllerContextTest.GetControllerContext(), null /* actionMethod */);
                },
                "actionMethod");
        }

        [TestMethod]
        public void ActionMethodProperty() {
            // Setup
            MethodInfo actionMethod = typeof(object).GetMethod("ToString");
            AuthorizationContext context = new AuthorizationContext(ControllerContextTest.GetControllerContext(), actionMethod);

            // Execute & verify
            Assert.AreSame(actionMethod, context.ActionMethod);
        }
    }
}
