namespace System.Web.Mvc.Test {
    using System;
    using System.Web.Mvc;
    using System.Web.TestUtil;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class HandleErrorInfoTest {

        [TestMethod]
        public void ConstructorSetsProperties() {
            // Setup
            Exception exception = new Exception();
            string controller = "SomeController";
            string action = "SomeAction";

            // Execute
            HandleErrorInfo viewData = new HandleErrorInfo(exception, controller, action);

            // Verify
            Assert.AreSame(exception, viewData.Exception);
            Assert.AreEqual(controller, viewData.Controller);
            Assert.AreEqual(action, viewData.Action);
        }

        [TestMethod]
        public void ConstructorWithEmptyActionThrows() {
            ExceptionHelper.ExpectArgumentExceptionNullOrEmpty(
                delegate {
                    new HandleErrorInfo(new Exception(), "SomeController", String.Empty);
                }, "action");
        }

        [TestMethod]
        public void ConstructorWithEmptyControllerThrows() {
            ExceptionHelper.ExpectArgumentExceptionNullOrEmpty(
                delegate {
                    new HandleErrorInfo(new Exception(), String.Empty, "SomeAction");
                }, "controller");
        }

        [TestMethod]
        public void ConstructorWithNullActionThrows() {
            ExceptionHelper.ExpectArgumentExceptionNullOrEmpty(
                delegate {
                    new HandleErrorInfo(new Exception(), "SomeController", null /* action */);
                }, "action");
        }

        [TestMethod]
        public void ConstructorWithNullControllerThrows() {
            ExceptionHelper.ExpectArgumentExceptionNullOrEmpty(
                delegate {
                    new HandleErrorInfo(new Exception(), null /* controller */, "SomeAction");
                }, "controller");
        }

        [TestMethod]
        public void ConstructorWithNullExceptionThrows() {
            ExceptionHelper.ExpectArgumentNullException(
                delegate {
                    new HandleErrorInfo(null /* exception */, "SomeController", "SomeAction");
                }, "exception");
        }

    }
}
