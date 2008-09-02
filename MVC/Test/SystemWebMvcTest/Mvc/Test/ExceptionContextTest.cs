namespace System.Web.Mvc.Test {
    using System.Reflection;
    using System.Web.Mvc;
    using System.Web.TestUtil;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class ExceptionContextTest {

        [TestMethod]
        public void ConstructorWithNullExceptionThrows() {
            // Act & Assert
            ExceptionHelper.ExpectArgumentNullException(
                delegate {
                    new ExceptionContext(ControllerContextTest.GetControllerContext(), null /* exception */);
                },
                "exception");
        }

        [TestMethod]
        public void ExceptionProperty() {
            // Arrange
            Exception exception = new Exception();
            ExceptionContext context = new ExceptionContext(ControllerContextTest.GetControllerContext(), exception);

            // Act & Assert
            Assert.AreSame(exception, context.Exception);
        }

        [TestMethod]
        public void ResultProperty() {
            // Arrange
            ExceptionContext context = new ExceptionContext(ControllerContextTest.GetControllerContext(), new Exception());
            ActionResult newResult = new EmptyResult();

            // Act
            ActionResult result1 = context.Result;
            context.Result = newResult;
            ActionResult result2 = context.Result;
            context.Result = null;
            ActionResult result3 = context.Result;

            // Assert
            Assert.IsInstanceOfType(result1, typeof(EmptyResult), "ExceptionContext.Result should be EmptyResult by default.");
            Assert.AreSame(newResult, result2);
            Assert.AreSame(result1, result3, "ExceptionContext.Result should return EmptyResult singleton.");
        }
    }
}
