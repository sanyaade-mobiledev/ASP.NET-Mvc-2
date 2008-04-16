namespace System.Web.Mvc.Test {
    using System;
    using System.Web.Routing;
    using System.Web.TestUtil;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;

    [TestClass]
    public class ActionRedirectResultTest {

        [TestMethod]
        public void ConstructorWithNullValuesDictionary() {
            // Execute
            var result = new ActionRedirectResult(null /* values */);

            // Verify
            Assert.IsNull(result.Values);
        }

        [TestMethod]
        public void ConstructorSetsValuesDictionary() {
            // Setup
            RouteValueDictionary dict = new RouteValueDictionary();

            // Execute
            var result = new ActionRedirectResult(dict);

            // Verify
            Assert.AreSame(dict, result.Values);
        }

        [TestMethod]
        public void ExecuteResultThrowsIfVirtualPathDataIsNull() {
            // Setup
            ControllerContext context = new ControllerContext(new Mock<HttpContextBase>().Object, new RouteData(), new Mock<IController>().Object);
            var result = new ActionRedirectResult(null) {
                Routes = new RouteCollection()
            };

            // Execute & verify
            ExceptionHelper.ExpectException<InvalidOperationException>(
                delegate {
                    result.ExecuteResult(context);
                },
                "No route in the route table matches the supplied values.");
        }

        [TestMethod]
        public void ExecuteResultWithNullControllerContextThrows() {
            // Setup
            var result = new ActionRedirectResult(null);

            // Execute & verify
            ExceptionHelper.ExpectArgumentNullException(
                delegate {
                    result.ExecuteResult(null /* context */);
                },
                "context");
        }

        [TestMethod]
        public void RoutesPropertyDefaultsToGlobalRouteTable() {
            // Execute
            var result = new ActionRedirectResult(new RouteValueDictionary());

            // Verify
            Assert.AreSame(RouteTable.Routes, result.Routes);
        }

    }
}
