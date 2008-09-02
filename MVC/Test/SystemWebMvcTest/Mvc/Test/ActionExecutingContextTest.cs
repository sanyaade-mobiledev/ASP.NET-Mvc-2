namespace System.Web.Mvc.Test {
    using System.Collections.Generic;
    using System.Web.Mvc;
    using System.Web.TestUtil;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class ActionExecutingContextTest {

        [TestMethod]
        public void ActionParametersProperty() {
            // Arrange
            Dictionary<string, object> actionParameters = new Dictionary<string, object>();
            ActionExecutingContext context = new ActionExecutingContext(ControllerContextTest.GetControllerContext(), actionParameters);

            // Act & Assert
            Assert.AreSame(actionParameters, context.ActionParameters);
        }

        [TestMethod]
        public void ConstructorThrowsIfActionParametersIsNull() {
            // Act & Assert
            ExceptionHelper.ExpectArgumentNullException(
                delegate {
                    new ActionExecutingContext(ControllerContextTest.GetControllerContext(), null /* actionParameters */);
                },
                "actionParameters");
        }
        
        [TestMethod]
        public void ConstructorThrowsIfControllerContextIsNull() {
            // Act & Assert
            ExceptionHelper.ExpectArgumentNullException(
                delegate {
                    new ActionExecutingContext(null /* controllerContext */, null /* actionParameters */);
                },
                "controllerContext");
        }

    }
}
