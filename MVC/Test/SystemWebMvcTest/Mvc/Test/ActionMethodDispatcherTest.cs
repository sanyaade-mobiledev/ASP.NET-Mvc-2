namespace System.Web.Mvc.Test {
    using System;
    using System.Reflection;
    using System.Web.Mvc;
    using System.Web.TestUtil;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class ActionMethodDispatcherTest {

        [TestMethod]
        public void ExecuteWithNormalActionMethod() {
            // Arrange
            DispatcherController controller = new DispatcherController();
            object[] parameters = new object[] { 5, "some string", new DateTime(2001, 1, 1) };
            MethodInfo methodInfo = typeof(DispatcherController).GetMethod("NormalAction");
            ActionMethodDispatcher dispatcher = new ActionMethodDispatcher(methodInfo);

            // Act
            ActionResult result = dispatcher.Execute(controller, parameters);

            // Assert
            Assert.IsInstanceOfType(result, typeof(ContentResult));
            ContentResult contentResult = (ContentResult)result;
            Assert.AreEqual("Hello from NormalAction!", contentResult.Content);

            Assert.AreEqual(5, controller._i);
            Assert.AreEqual("some string", controller._s);
            Assert.AreEqual(new DateTime(2001, 1, 1), controller._dt);
        }

        [TestMethod]
        public void ExecuteWithParameterlessActionMethod() {
            // Arrange
            DispatcherController controller = new DispatcherController();
            object[] parameters = new object[] { 5, "some string", new DateTime(2001, 1, 1) };
            MethodInfo methodInfo = typeof(DispatcherController).GetMethod("ParameterlessAction");
            ActionMethodDispatcher dispatcher = new ActionMethodDispatcher(methodInfo);

            // Act
            ActionResult result = dispatcher.Execute(controller, parameters);

            // Assert
            Assert.IsInstanceOfType(result, typeof(ContentResult));
            ContentResult contentResult = (ContentResult)result;
            Assert.AreEqual("53", contentResult.Content);
        }

        [TestMethod]
        public void ExecuteWithVoidActionMethod() {
            // Arrange
            DispatcherController controller = new DispatcherController();
            object[] parameters = new object[] { 5, "some string", new DateTime(2001, 1, 1) };
            MethodInfo methodInfo = typeof(DispatcherController).GetMethod("VoidAction");
            ActionMethodDispatcher dispatcher = new ActionMethodDispatcher(methodInfo);

            // Act
            ActionResult result = dispatcher.Execute(controller, parameters);

            // Assert
            Assert.IsInstanceOfType(result, typeof(EmptyResult));
            Assert.AreEqual(5, controller._i);
            Assert.AreEqual("some string", controller._s);
            Assert.AreEqual(new DateTime(2001, 1, 1), controller._dt);
        }

        [TestMethod]
        public void MethodInfoProperty() {
            // Arrange
            MethodInfo original = typeof(object).GetMethod("ToString");
            ActionMethodDispatcher dispatcher = new ActionMethodDispatcher(original);

            // Act
            MethodInfo returned = dispatcher.MethodInfo;

            // Assert
            Assert.AreSame(original, returned);
        }

        [TestMethod]
        public void ObjectToActionResultWithActionResultParameterReturnsParameterUnchanged() {
            // Arrange
            ActionResult original = new JsonResult();

            // Act
            ActionResult returned = ActionMethodDispatcher.ObjectToActionResult(original);

            // Assert
            Assert.AreSame(original, returned);
        }

        [TestMethod]
        public void ObjectToActionResultWithNullParameterReturnsEmptyResult() {
            // Act
            ActionResult actionResult = ActionMethodDispatcher.ObjectToActionResult(null);

            // Assert
            Assert.IsInstanceOfType(actionResult, typeof(EmptyResult));
        }

        [TestMethod]
        public void ObjectToActionResultWithObjectParameterReturnsContentResult() {
            // Arrange
            object original = new CultureReflector();

            // Act
            ActionResult returned = ActionMethodDispatcher.ObjectToActionResult(original);

            // Assert
            Assert.IsInstanceOfType(returned, typeof(ContentResult));
            ContentResult contentResult = (ContentResult)returned;
            Assert.AreEqual("IVL", contentResult.Content);
        }

        private class DispatcherController : Controller {

            public int _i;
            public string _s;
            public DateTime _dt;

            public object NormalAction(int i, string s, DateTime dt) {
                VoidAction(i, s, dt);
                return "Hello from NormalAction!";
            }

            public int ParameterlessAction() {
                return 53;
            }

            public void VoidAction(int i, string s, DateTime dt) {
                _i = i;
                _s = s;
                _dt = dt;
            }

        }

    }
}
