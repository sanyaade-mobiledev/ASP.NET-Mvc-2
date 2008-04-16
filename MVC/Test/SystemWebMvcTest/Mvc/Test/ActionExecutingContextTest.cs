namespace System.Web.Mvc.Test {
    using System.Collections.Generic;
    using System.Reflection;
    using System.Web.TestUtil;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class ActionExecutingContextTest {

        [TestMethod]
        public void ConstructorWithNullActionMethodThrows() {
            // Execute & verify
            ExceptionHelper.ExpectArgumentNullException(
                delegate {
                    new ActionExecutingContext(ControllerContextTest.GetControllerContext(), null /* actionMethod */, null /* actionParameters */);
                },
                "actionMethod");
        }

        [TestMethod]
        public void ConstructorWithNullActionParametersThrows() {
            // Execute & verify
            ExceptionHelper.ExpectArgumentNullException(
                delegate {
                    new ActionExecutingContext(ControllerContextTest.GetControllerContext(), typeof(object).GetMethod("ToString"), null /* actionParameters */);
                },
                "actionParameters");
        }
        
        [TestMethod]
        public void ConstructorWithNullControllerContextThrows() {
            // Execute & verify
            ExceptionHelper.ExpectArgumentNullException(
                delegate {
                    new ActionExecutingContext(null /* controllerContext */, null /* actionMethod */, null /* actionParameters */);
                },
                "controllerContext");
        }

        [TestMethod]
        public void GetActionMethod() {
            // Setup
            MethodInfo actionMethod = typeof(object).GetMethod("ToString");
            Dictionary<string, object> actionParameters = new Dictionary<string, object>();
            ActionExecutingContext context = new ActionExecutingContext(ControllerContextTest.GetControllerContext(), actionMethod, actionParameters);

            // Execute & verify
            Assert.AreSame(actionMethod, context.ActionMethod);
        }

        [TestMethod]
        public void GetActionParameters() {
            // Setup
            MethodInfo actionMethod = typeof(object).GetMethod("ToString");
            Dictionary<string, object> actionParameters = new Dictionary<string, object>();
            ActionExecutingContext context = new ActionExecutingContext(ControllerContextTest.GetControllerContext(), actionMethod, actionParameters);

            // Execute & verify
            Assert.AreSame(actionParameters, context.ActionParameters);
        }

        [TestMethod]
        public void SetCancel() {
            // Setup
            MethodInfo actionMethod = typeof(object).GetMethod("ToString");
            Dictionary<string, object> actionParameters = new Dictionary<string, object>();
            ActionExecutingContext context = new ActionExecutingContext(ControllerContextTest.GetControllerContext(), actionMethod, actionParameters);

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
            MethodInfo actionMethod = typeof(object).GetMethod("ToString");
            Dictionary<string, object> actionParameters = new Dictionary<string, object>();
            ActionExecutingContext context = new ActionExecutingContext(ControllerContextTest.GetControllerContext(), actionMethod, actionParameters);
            EmptyResult result = new EmptyResult();

            // Execute
            ActionResult origVal = context.Result;
            context.Result = result;
            ActionResult newVal = context.Result;

            // Verify
            Assert.IsNull(origVal);
            Assert.AreSame(result, newVal);
        }

    }
}
