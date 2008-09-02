namespace System.Web.Mvc.Test {
    using System;
    using System.Web.Mvc;
    using System.Web.TestUtil;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class ActionExecutedContextTest {

        [TestMethod]
        public void ConstructorWithNullControllerContextThrows() {
            // Act & Assert
            ExceptionHelper.ExpectArgumentNullException(
                delegate {
                    new ActionExecutedContext(null /* controllerContext */, false /* canceled */, null /* exception */);
                },
                "controllerContext");
        }

        [TestMethod]
        public void ConstructorWithNullExceptionIsOk() {
            // Act
            ActionExecutedContext context = new ActionExecutedContext(ControllerContextTest.GetControllerContext(), false /* canceled */, null /* exception */);

            // Assert
            Assert.IsNull(context.Exception);
        }

        [TestMethod]
        public void GetCanceled() {
            // Arrange
            ActionExecutedContext context1 = new ActionExecutedContext(ControllerContextTest.GetControllerContext(), true /* canceled */, null /* exception */);
            ActionExecutedContext context2 = new ActionExecutedContext(ControllerContextTest.GetControllerContext(), false /* canceled */, null /* exception */);

            // Act & Assert
            Assert.IsTrue(context1.Canceled);
            Assert.IsFalse(context2.Canceled);
        }

        [TestMethod]
        public void GetException() {
            // Arrange
            Exception exception = new Exception();
            ActionExecutedContext context = new ActionExecutedContext(ControllerContextTest.GetControllerContext(), false /* canceled */, exception);

            // Act & Assert
            Assert.AreSame(exception, context.Exception);
        }

        [TestMethod]
        public void ResultProperty() {
            // Arrange
            Exception exception = new Exception();
            ActionExecutedContext context = new ActionExecutedContext(ControllerContextTest.GetControllerContext(), false /* canceled */, exception);
            ContentResult result = new ContentResult();

            // Act
            ActionResult origVal = context.Result;
            context.Result = result;
            ActionResult newVal = context.Result;

            // Assert
            Assert.IsInstanceOfType(origVal, typeof(EmptyResult));
            Assert.AreSame(result, newVal);
        }

        [TestMethod]
        public void ResultPropertyReturnsSingletonEmptyResult() {
            // Arrange
            Exception exception = new Exception();
            ActionExecutedContext context = new ActionExecutedContext(ControllerContextTest.GetControllerContext(), false /* canceled */, null /* exception */);

            // Act
            ActionResult origVal = context.Result;
            context.Result = null;
            ActionResult newVal = context.Result;

            // Assert
            Assert.IsInstanceOfType(origVal, typeof(EmptyResult));
            Assert.AreSame(origVal, newVal);
        }

        [TestMethod]
        public void SetExceptionHandled() {
            // Arrange
            Exception exception = new Exception();
            ActionExecutedContext context = new ActionExecutedContext(ControllerContextTest.GetControllerContext(), false /* canceled */, exception);

            // Act
            bool origVal = context.ExceptionHandled;
            context.ExceptionHandled = true;
            bool newVal = context.ExceptionHandled;

            // Assert
            Assert.IsFalse(origVal);
            Assert.IsTrue(newVal);
        }
    }
}
