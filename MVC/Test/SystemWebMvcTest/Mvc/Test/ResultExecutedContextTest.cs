namespace System.Web.Mvc.Test {
    using System;
    using System.Web.TestUtil;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class ResultExecutedContextTest {

        [TestMethod]
        public void ConstructorWithNullControllerContextThrows() {
            // Act & Assert
            ExceptionHelper.ExpectArgumentNullException(
                delegate {
                    new ResultExecutedContext(null /* controllerContext */, null /* result */, false /* canceled */, null /* exception */);
                },
                "controllerContext");
        }

        [TestMethod]
        public void ConstructorWithNullExceptionIsOk() {
            // Act
            ActionResult result = new EmptyResult();
            ResultExecutedContext context = new ResultExecutedContext(ControllerContextTest.GetControllerContext(), result, false /* canceled */, null /* exception */);

            // Assert
            Assert.IsNull(context.Exception);
        }

        [TestMethod]
        public void ConstructorWithNullResultThrows() {
            // Act & Assert
            ExceptionHelper.ExpectArgumentNullException(
                delegate {
                    new ResultExecutedContext(ControllerContextTest.GetControllerContext(), null /* result */, false /* canceled */, null /* exception */);
                },
                "result");
        }

        [TestMethod]
        public void GetCanceled() {
            // Arrange
            ActionResult result = new EmptyResult();
            ResultExecutedContext context1 = new ResultExecutedContext(ControllerContextTest.GetControllerContext(), result, true /* canceled */, null /* exception */);
            ResultExecutedContext context2 = new ResultExecutedContext(ControllerContextTest.GetControllerContext(), result, false /* canceled */, null /* exception */);

            // Act & Assert
            Assert.IsTrue(context1.Canceled);
            Assert.IsFalse(context2.Canceled);
        }

        [TestMethod]
        public void GetException() {
            // Arrange
            ActionResult result = new EmptyResult();
            Exception exception = new Exception();
            ResultExecutedContext context = new ResultExecutedContext(ControllerContextTest.GetControllerContext(), result, false /* canceled */, exception);

            // Act & Assert
            Assert.AreSame(exception, context.Exception);
        }

        [TestMethod]
        public void GetResult() {
            // Arrange
            ActionResult result = new EmptyResult();
            Exception exception = new Exception();
            ResultExecutedContext context = new ResultExecutedContext(ControllerContextTest.GetControllerContext(), result, false /* canceled */, exception);

            // Act & Assert
            Assert.AreSame(result, context.Result);
        }

        [TestMethod]
        public void SetExceptionHandled() {
            // Arrange
            ActionResult result = new EmptyResult();
            Exception exception = new Exception();
            ResultExecutedContext context = new ResultExecutedContext(ControllerContextTest.GetControllerContext(), result, false /* canceled */, exception);

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
