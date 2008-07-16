namespace MvcFuturesTest.Mvc.Test {
    using System;
    using System.Linq.Expressions;
    using System.Web.Mvc;
    using System.Web.Routing;
    using System.Web.TestUtil;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Microsoft.Web.Mvc.Internal;
    using System.Globalization;

    [TestClass]
    public class ExpressionHelperTests {
        [TestMethod]
        public void BuildRouteValueDictionaryWithNullExpressionThrowsArgumentNullException() {
            ExceptionHelper.ExpectArgumentNullException(() => ExpressionHelper.GetRouteValuesFromExpression<TestController>(null), "action");
        }

        [TestMethod]
        public void BuildRouteValueDictionaryWithNonMethodExpressionThrowsInvalidOperationException() {
            Expression<Action<TestController>> expression = c => new TestController();
            ExceptionHelper.ExpectArgumentException(() => ExpressionHelper.GetRouteValuesFromExpression<TestController>(expression), "Expression must be a method call" + Environment.NewLine + "Parameter name: action");
        }

        [TestMethod]
        public void BuildRouteValueDictionaryWithoutControllerSuffixThrowsInvalidOperationException() {
            Expression<Action<TestControllerNot>> index = (c => c.Index(123));
            ExceptionHelper.ExpectArgumentException(() => ExpressionHelper.GetRouteValuesFromExpression(index), "Controller name must end in 'Controller'" + Environment.NewLine + "Parameter name: action");
        }

        [TestMethod]
        public void BuildRouteValueDictionaryWithControllerBaseClassThrowsInvalidOperationException() {
            Expression<Action<Controller>> index = (c => c.Dispose());
            ExceptionHelper.ExpectArgumentException(() => ExpressionHelper.GetRouteValuesFromExpression(index), "Cannot route to class named 'Controller'" + Environment.NewLine + "Parameter name: action");
        }

        [TestMethod]
        public void BuildRouteValueDictionaryAddsControllerNameToDictionary() {
            Expression<Action<TestController>> index = (c => c.Index(123));
            RouteValueDictionary rvd = ExpressionHelper.GetRouteValuesFromExpression(index);
            Assert.AreEqual("Test", rvd["Controller"]);
        }

        [TestMethod]
        public void BuildRouteValueDictionaryFromExpressionReturnsCorrectDictionary() {
            Expression<Action<TestController>> index = (c => c.Index(123));
            RouteValueDictionary rvd = ExpressionHelper.GetRouteValuesFromExpression(index);
            Assert.AreEqual("Test", rvd["Controller"]);
            Assert.AreEqual("Index", rvd["Action"]);
            Assert.AreEqual(123, rvd["page"]);
        }

        [TestMethod]
        public void BuildRouteValueDictionaryFromNonConstantExpressionReturnsCorrectDictionary() {
            Expression<Action<TestController>> index = (c => c.About(Foo));
            RouteValueDictionary rvd = ExpressionHelper.GetRouteValuesFromExpression(index);
            Assert.AreEqual("Test", rvd["Controller"]);
            Assert.AreEqual("About", rvd["Action"]);
            Assert.AreEqual("FooValue", rvd["s"]);
        }

        public class TestController : Controller {
            public ActionResult Index(int page) {
                return null;
            }

            public string About(string s) {
                return "The value is " + s;
            }
        }

        public string Foo {
            get {
                return "FooValue";
            }
        }

        public class TestControllerNot : Controller {
            public ActionResult Index(int page) {
                return null;
            }
        }
    }
}
