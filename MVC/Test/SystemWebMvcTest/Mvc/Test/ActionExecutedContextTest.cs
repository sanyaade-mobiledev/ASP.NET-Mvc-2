namespace System.Web.Mvc.Test {
    using System;
    using System.Reflection;
    using System.Web.TestUtil;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class ActionExecutedContextTest {

        [TestMethod]
        public void ConstructorWithNullActionMethodThrows() {
            // Execute & verify
            ExceptionHelper.ExpectArgumentNullException(
                delegate {
                    new ActionExecutedContext(ControllerContextTest.GetControllerContext(), null /* actionMethod */, false /* canceled */, null /* exception */);
                },
                "actionMethod");
        }

        [TestMethod]
        public void ConstructorWithNullControllerContextThrows() {
            // Execute & verify
            ExceptionHelper.ExpectArgumentNullException(
                delegate {
                    new ActionExecutedContext(null /* controllerContext */, null /* actionMethod */, false /* canceled */, null /* exception */);
                },
                "controllerContext");
        }

        [TestMethod]
        public void ConstructorWithNullExceptionIsOk() {
            // Execute
            ActionExecutedContext context = new ActionExecutedContext(ControllerContextTest.GetControllerContext(), typeof(object).GetMethod("ToString"), false /* canceled */, null /* exception */);

            // Verify
            Assert.IsNull(context.Exception);
        }

        [TestMethod]
        public void GetActionMethod() {
            // Setup
            MethodInfo actionMethod = typeof(object).GetMethod("ToString");
            Exception exception = new Exception();
            ActionExecutedContext context = new ActionExecutedContext(ControllerContextTest.GetControllerContext(), actionMethod, false /* canceled */, exception);

            // Execute & verify
            Assert.AreSame(actionMethod, context.ActionMethod);
        }

        [TestMethod]
        public void GetCanceled() {
            // Setup
            MethodInfo actionMethod = typeof(object).GetMethod("ToString");
            ActionExecutedContext context1 = new ActionExecutedContext(ControllerContextTest.GetControllerContext(), actionMethod, true /* canceled */, null /* exception */);
            ActionExecutedContext context2 = new ActionExecutedContext(ControllerContextTest.GetControllerContext(), actionMethod, false /* canceled */, null /* exception */);

            // Execute & verify
            Assert.IsTrue(context1.Canceled);
            Assert.IsFalse(context2.Canceled);
        }

        [TestMethod]
        public void GetException() {
            // Setup
            MethodInfo actionMethod = typeof(object).GetMethod("ToString");
            Exception exception = new Exception();
            ActionExecutedContext context = new ActionExecutedContext(ControllerContextTest.GetControllerContext(), actionMethod, false /* canceled */, exception);

            // Execute & verify
            Assert.AreSame(exception, context.Exception);
        }

        [TestMethod]
        public void ResultProperty() {
            // Setup
            MethodInfo actionMethod = typeof(object).GetMethod("ToString");
            Exception exception = new Exception();
            ActionExecutedContext context = new ActionExecutedContext(ControllerContextTest.GetControllerContext(), actionMethod, false /* canceled */, exception);
            ContentResult result = new ContentResult();

            // Execute
            ActionResult origVal = context.Result;
            context.Result = result;
            ActionResult newVal = context.Result;

            // Verify
            Assert.IsInstanceOfType(origVal, typeof(EmptyResult));
            Assert.AreSame(result, newVal);
        }

        [TestMethod]
        public void ResultPropertyReturnsSingletonEmptyResult() {
            // Setup
            MethodInfo actionMethod = typeof(object).GetMethod("ToString");
            Exception exception = new Exception();
            ActionExecutedContext context = new ActionExecutedContext(ControllerContextTest.GetControllerContext(), actionMethod, false /* canceled */, null /* exception */);

            // Execute
            ActionResult origVal = context.Result;
            context.Result = null;
            ActionResult newVal = context.Result;

            // Verify
            Assert.IsInstanceOfType(origVal, typeof(EmptyResult));
            Assert.AreSame(origVal, newVal);
        }

        [TestMethod]
        public void SetExceptionHandled() {
            // Setup
            MethodInfo actionMethod = typeof(object).GetMethod("ToString");
            Exception exception = new Exception();
            ActionExecutedContext context = new ActionExecutedContext(ControllerContextTest.GetControllerContext(), actionMethod, false /* canceled */, exception);

            // Execute
            bool origVal = context.ExceptionHandled;
            context.ExceptionHandled = true;
            bool newVal = context.ExceptionHandled;

            // Verify
            Assert.IsFalse(origVal);
            Assert.IsTrue(newVal);
        }
    }
}
