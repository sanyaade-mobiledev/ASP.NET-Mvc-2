namespace System.Web.Mvc.Test {
    using System.Web.TestUtil;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class ResultExecutingContextTest {
        [TestMethod]
        public void ConstructorWithNullControllerContextThrows() {
            // Act & Assert
            ExceptionHelper.ExpectArgumentNullException(
                delegate {
                    new ResultExecutingContext(null /* controllerContext */, null /* result */);
                },
                "controllerContext");
        }

        [TestMethod]
        public void ConstructorWithNullResultThrows() {
            // Act & Assert
            ExceptionHelper.ExpectArgumentNullException(
                delegate {
                    new ResultExecutingContext(ControllerContextTest.GetControllerContext(), null /* result */);
                },
                "result");
        }

        [TestMethod]
        public void GetResult() {
            // Arrange
            ActionResult result = new EmptyResult();
            ResultExecutingContext context = new ResultExecutingContext(ControllerContextTest.GetControllerContext(), result);

            // Act & Assert
            Assert.AreSame(result, context.Result);
        }

        [TestMethod]
        public void SetCancel() {
            // Arrange
            ActionResult result = new EmptyResult();
            ResultExecutingContext context = new ResultExecutingContext(ControllerContextTest.GetControllerContext(), result);

            // Act
            bool origVal = context.Cancel;
            context.Cancel = true;
            bool newVal = context.Cancel;

            // Assert
            Assert.IsFalse(origVal);
            Assert.IsTrue(newVal);
        }

    }
}
