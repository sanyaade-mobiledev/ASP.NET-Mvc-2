namespace System.Web.Mvc.Test {
    using System.Web.TestUtil;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class ResultExecutingContextTest {
        [TestMethod]
        public void ConstructorWithNullControllerContextThrows() {
            // Execute & verify
            ExceptionHelper.ExpectArgumentNullException(
                delegate {
                    new ResultExecutingContext(null /* controllerContext */, null /* result */);
                },
                "controllerContext");
        }

        [TestMethod]
        public void ConstructorWithNullResultThrows() {
            // Execute & verify
            ExceptionHelper.ExpectArgumentNullException(
                delegate {
                    new ResultExecutingContext(ControllerContextTest.GetControllerContext(), null /* result */);
                },
                "result");
        }

        [TestMethod]
        public void SetCancel() {
            // Setup
            ActionResult result = new EmptyResult();
            ResultExecutingContext context = new ResultExecutingContext(ControllerContextTest.GetControllerContext(), result);

            // Execute
            bool origVal = context.Cancel;
            context.Cancel = true;
            bool newVal = context.Cancel;

            // Verify
            Assert.IsFalse(origVal);
            Assert.IsTrue(newVal);
        }

        [TestMethod]
        public void SetResult() {
            // Setup
            ActionResult origResult1 = new EmptyResult();
            ActionResult origResult2 = new EmptyResult();
            ResultExecutingContext context = new ResultExecutingContext(ControllerContextTest.GetControllerContext(), origResult1);

            // Execute
            ActionResult result1 = context.Result;
            context.Result = origResult2;
            ActionResult result2 = context.Result;

            // Verify
            Assert.AreSame(origResult1, result1);
            Assert.AreSame(origResult2, result2);
        }

        [TestMethod]
        public void SetResultThrowsIfNull() {
            // Setup
            ResultExecutingContext context = new ResultExecutingContext(ControllerContextTest.GetControllerContext(), new EmptyResult());

            // Execute & verify
            ExceptionHelper.ExpectArgumentNullException(
                delegate {
                    context.Result = null;
                },
                "value");
        }

    }
}
