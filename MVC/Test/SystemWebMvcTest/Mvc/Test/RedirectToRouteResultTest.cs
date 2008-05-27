namespace System.Web.Mvc.Test {
    using System;
    using System.Web.Routing;
    using System.Web.TestUtil;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;

    [TestClass]
    public class RedirectToRouteResultTest {

        [TestMethod]
        public void ConstructorWithNullValuesDictionary() {
            // Execute
            var result = new RedirectToRouteResult(null /* values */);

            // Verify
            Assert.IsNotNull(result.Values);
            Assert.AreEqual<int>(0, result.Values.Count);
            Assert.AreEqual<string>(String.Empty, result.RouteName);
        }

        [TestMethod]
        public void ConstructorSetsValuesDictionary() {
            // Setup
            RouteValueDictionary dict = new RouteValueDictionary();

            // Execute
            var result = new RedirectToRouteResult(dict);

            // Verify
            Assert.AreSame(dict, result.Values);
            Assert.AreEqual<string>(String.Empty, result.RouteName);
        }

        [TestMethod]
        public void ConstructorSetsValuesDictionaryAndEmptyName() {
            // Setup
            RouteValueDictionary dict = new RouteValueDictionary();

            // Execute
            var result = new RedirectToRouteResult(null, dict);

            // Verify
            Assert.AreSame(dict, result.Values);
            Assert.AreEqual<string>(String.Empty, result.RouteName);
        }

        [TestMethod]
        public void ConstructorSetsValuesDictionaryAndName() {
            // Setup
            RouteValueDictionary dict = new RouteValueDictionary();

            // Execute
            var result = new RedirectToRouteResult("foo", dict);

            // Verify
            Assert.AreSame(dict, result.Values);
            Assert.AreEqual<string>("foo", result.RouteName);
        }

        [TestMethod]
        public void ExecuteResultThrowsIfVirtualPathDataIsNull() {
            // Setup
            ControllerContext context = new ControllerContext(new Mock<HttpContextBase>().Object, new RouteData(), new Mock<IController>().Object);
            var result = new RedirectToRouteResult(null) {
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
            var result = new RedirectToRouteResult(null);

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
            var result = new RedirectToRouteResult(new RouteValueDictionary());

            // Verify
            Assert.AreSame(RouteTable.Routes, result.Routes);
        }
    }
}
