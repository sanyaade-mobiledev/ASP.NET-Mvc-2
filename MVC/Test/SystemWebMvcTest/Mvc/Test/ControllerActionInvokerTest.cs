namespace System.Web.Mvc.Test {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Globalization;
    using System.Reflection;
    using System.Threading;
    using System.Web.Routing;
    using System.Web.TestUtil;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;

    [TestClass]
    public class ControllerActionInvokerTest {

        [TestMethod]
        public void ConstructorSetsProperties() {
            // Setup
            var controller = new FindMethodController();
            ControllerContext context = GetControllerContext(controller);

            // Execute
            ControllerActionInvokerHelper helper = new ControllerActionInvokerHelper(context);

            // Verify
            Assert.AreSame(context, helper.ControllerContext);
        }

        [TestMethod]
        public void ConstructorWithNullControllerContextThrows() {
            // Execute & verify
            ExceptionHelper.ExpectArgumentNullException(
                delegate {
                    new ControllerActionInvoker(null /* controllerContext */);
                },
                "controllerContext");
        }

        [TestMethod]
        public void ConvertParameterCanConvertDerivedTypeToBaseType() {
            // Setup
            Employee employee = new Employee();

            // Execute
            object person = ControllerActionInvoker.ConvertParameterType(employee, typeof(Person), "parameterName", "actionName");

            // Verify
            Assert.IsInstanceOfType(person, typeof(Person));
        }

        [TestMethod]
        public void ConvertParameterTypeUsesInvariantCultureForCanConvertFrom() {
            // DevDiv Bugs 197107: ControllerActionInvoker should use the invariant
            // culture to do parameter conversion

            // Setup
            var controller = new ParameterTestingController();
            ControllerContext context = GetControllerContext(controller);
            Dictionary<string, object> dict = new Dictionary<string, object>() {
                { "id", "2/1/2001" } // February 1, 2001
            };
            ControllerActionInvokerHelper helper = new ControllerActionInvokerHelper(context);
            MethodInfo mi = typeof(ParameterTestingController).GetMethod("TakesDateTime");
            ParameterInfo[] pis = mi.GetParameters();

            // Execute
            CultureInfo savedCulture = Thread.CurrentThread.CurrentCulture;
            object oValue;
            try {
                // In the en-GB culture, '2/1/2001' is shorthand for January 2, 2001
                Thread.CurrentThread.CurrentCulture = new CultureInfo("en-GB");
                oValue = helper.PublicGetParameterValue(pis[0], dict);
            }
            finally {
                Thread.CurrentThread.CurrentCulture = savedCulture;
            }

            // Verify
            Assert.AreEqual(new DateTime(2001, 2, 1), oValue);
        }

        [TestMethod]
        public void ConvertParameterTypeUsesInvariantCultureForCanConvertTo() {
            // DevDiv Bugs 197107: ControllerActionInvoker should use the invariant
            // culture to do parameter conversion

            // Setup
            var controller = new ParameterTestingController();
            ControllerContext context = GetControllerContext(controller);
            Dictionary<string, object> dict = new Dictionary<string, object>() {
                { "id", new DateTime(2001, 2, 1) } // February 1, 2001
            };
            ControllerActionInvokerHelper helper = new ControllerActionInvokerHelper(context);
            MethodInfo mi = typeof(ParameterTestingController).GetMethod("TakesString");
            ParameterInfo[] pis = mi.GetParameters();

            // Execute
            CultureInfo savedCulture = Thread.CurrentThread.CurrentCulture;
            object oValue;
            try {
                // In the en-GB culture, February 2, 2001 is written shorthand as '01/02/2001'.
                Thread.CurrentThread.CurrentCulture = new CultureInfo("en-GB");
                oValue = helper.PublicGetParameterValue(pis[0], dict);
            }
            finally {
                Thread.CurrentThread.CurrentCulture = savedCulture;
            }

            // Verify
            Assert.AreEqual("2001-02-01", oValue);
        }

        [TestMethod]
        public void FindActionMethodDoesNotMatchConstructor() {
            // FindActionMethod() shouldn't match special-named methods like type constructors.

            // Setup
            var controller = new FindMethodController();
            ControllerContext context = GetControllerContext(controller);
            ControllerActionInvokerHelper helper = new ControllerActionInvokerHelper(context);

            // Execute
            MethodInfo mi = helper.PublicFindActionMethod(".ctor", null /* values */);

            // Verify
            Assert.IsNull(mi);
        }

        [TestMethod]
        public void FindActionMethodDoesNotMatchEvent() {
            // FindActionMethod() should skip methods that aren't publicly visible.

            // Setup
            var controller = new FindMethodController();
            ControllerContext context = GetControllerContext(controller);
            ControllerActionInvokerHelper helper = new ControllerActionInvokerHelper(context);

            // Execute
            MethodInfo mi = helper.PublicFindActionMethod("add_Event", null /* values */);

            // Verify
            Assert.IsNull(mi);
        }

        [TestMethod]
        public void FindActionMethodDoesNotMatchInternalMethod() {
            // FindActionMethod() should skip methods that aren't publicly visible.

            // Setup
            var controller = new FindMethodController();
            ControllerContext context = GetControllerContext(controller);
            ControllerActionInvokerHelper helper = new ControllerActionInvokerHelper(context);

            // Execute
            MethodInfo mi = helper.PublicFindActionMethod("InternalMethod", null /* values */);

            // Verify
            Assert.IsNull(mi);
        }

        [TestMethod]
        public void FindActionMethodDoesNotMatchMethodsDefinedOnControllerType() {
            // FindActionMethod() shouldn't match methods originally defined on the Controller type, e.g. Dispose().

            // Setup
            var controller = new BlankController();
            ControllerContext context = GetControllerContext(controller);
            ControllerActionInvokerHelper helper = new ControllerActionInvokerHelper(context);
            var methods = typeof(Controller).GetMethods(BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);

            // Execute & verify
            foreach (var method in methods) {
                bool wasFound = true;
                try {
                    MethodInfo mi = helper.PublicFindActionMethod(method.Name, null /* values */);
                    wasFound = (mi != null);
                }
                finally {
                    Assert.IsFalse(wasFound, "FindActionMethod() should return false for methods defined on the Controller class: " + method);
                }
            }
        }

        [TestMethod]
        public void FindActionMethodDoesNotMatchMethodsDefinedOnObjectType() {
            // FindActionMethod() shouldn't match methods originally defined on the Object type, e.g. ToString().

            // Setup
            var controller = new FindMethodController();
            ControllerContext context = GetControllerContext(controller);
            ControllerActionInvokerHelper helper = new ControllerActionInvokerHelper(context);

            // Execute
            MethodInfo mi = helper.PublicFindActionMethod("ToString", null /* values */);

            // Verify
            Assert.IsNull(mi);
        }

        [TestMethod]
        public void FindActionMethodDoesNotMatchNonActionMethod() {
            // FindActionMethod() should respect the [NonAction] attribute.

            // Setup
            var controller = new FindMethodController();
            ControllerContext context = GetControllerContext(controller);
            ControllerActionInvokerHelper helper = new ControllerActionInvokerHelper(context);

            // Execute
            MethodInfo mi = helper.PublicFindActionMethod("NonActionMethod", null /* values */);

            // Verify
            Assert.IsNull(mi);
        }

        [TestMethod]
        public void FindActionMethodDoesNotMatchOverriddenNonActionMethod() {
            // FindActionMethod() should trace the method's inheritance chain looking for the [NonAction] attribute.

            // Setup
            var controller = new DerivedFindMethodController();
            ControllerContext context = GetControllerContext(controller);
            ControllerActionInvokerHelper helper = new ControllerActionInvokerHelper(context);

            // Execute
            MethodInfo mi = helper.PublicFindActionMethod("NonActionMethod", null /* values */);

            // Verify
            Assert.IsNull(mi);
        }

        [TestMethod]
        public void FindActionMethodDoesNotMatchPrivateMethod() {
            // FindActionMethod() should skip methods that aren't publicly visible.

            // Setup
            var controller = new FindMethodController();
            ControllerContext context = GetControllerContext(controller);
            ControllerActionInvokerHelper helper = new ControllerActionInvokerHelper(context);

            // Execute
            MethodInfo mi = helper.PublicFindActionMethod("PrivateMethod", null /* values */);

            // Verify
            Assert.IsNull(mi);
        }

        [TestMethod]
        public void FindActionMethodDoesNotMatchProperty() {
            // FindActionMethod() shouldn't match special-named methods like property getters.

            // Setup
            var controller = new FindMethodController();
            ControllerContext context = GetControllerContext(controller);
            ControllerActionInvokerHelper helper = new ControllerActionInvokerHelper(context);

            // Execute
            MethodInfo mi = helper.PublicFindActionMethod("get_Property", null /* values */);

            // Verify
            Assert.IsNull(mi);
        }

        [TestMethod]
        public void FindActionMethodDoesNotMatchProtectedMethod() {
            // FindActionMethod() should skip methods that aren't publicly visible.

            // Setup
            var controller = new FindMethodController();
            ControllerContext context = GetControllerContext(controller);
            ControllerActionInvokerHelper helper = new ControllerActionInvokerHelper(context);

            // Execute
            MethodInfo mi = helper.PublicFindActionMethod("ProtectedMethod", null /* values */);

            // Verify
            Assert.IsNull(mi);
        }

        [TestMethod]
        public void FindActionMethodIsCaseInsensitive() {
            // Setup
            var controller = new FindMethodController();
            ControllerContext context = GetControllerContext(controller);
            ControllerActionInvokerHelper helper = new ControllerActionInvokerHelper(context);
            MethodInfo expected = typeof(FindMethodController).GetMethod("ValidActionMethod");

            // Execute
            MethodInfo mi1 = helper.PublicFindActionMethod("validactionmethod", null /* values */);
            MethodInfo mi2 = helper.PublicFindActionMethod("VALIDACTIONMETHOD", null /* values */);

            // Verify
            Assert.AreEqual(expected, mi1);
            Assert.AreEqual(expected, mi2);
        }

        [TestMethod]
        public void FindActionMethodMatchesNewActionMethodsHidingNonActionMethods() {
            // FindActionMethod() should stop looking for [NonAction] in the method's inheritance chain when it sees
            // that a method in a derived class hides the a method in the base class.

            // Setup
            var controller = new DerivedFindMethodController();
            ControllerContext context = GetControllerContext(controller);
            ControllerActionInvokerHelper helper = new ControllerActionInvokerHelper(context);
            MethodInfo expected = typeof(DerivedFindMethodController).GetMethod("DerivedIsActionMethod");

            // Execute
            MethodInfo mi = helper.PublicFindActionMethod("DerivedIsActionMethod", null /* values */);

            // Verify
            Assert.AreEqual(expected, mi);
        }

        [TestMethod]
        public void FindActionMethodWithClosedGenerics() {
            // FindActionMethod() should work with generic methods as long as there are no open types.

            // Setup
            var controller = new GenericFindMethodController<int>();
            ControllerContext context = GetControllerContext(controller);
            ControllerActionInvokerHelper helper = new ControllerActionInvokerHelper(context);
            MethodInfo expected = typeof(GenericFindMethodController<int>).GetMethod("ClosedGenericMethod");

            // Execute
            MethodInfo mi = helper.PublicFindActionMethod("ClosedGenericMethod", null /* values */);

            // Verify
            Assert.AreEqual(expected, mi);
        }

        [TestMethod]
        public void FindActionMethodWithEmptyActionNameThrows() {
            // Setup
            var controller = new FindMethodController();
            ControllerContext context = GetControllerContext(controller);
            ControllerActionInvokerHelper helper = new ControllerActionInvokerHelper(context);

            // Execute & verify
            ExceptionHelper.ExpectArgumentExceptionNullOrEmpty(
                delegate {
                    helper.PublicFindActionMethod(String.Empty, null /* values */);
                },
                "actionName");
        }

        [TestMethod]
        public void FindActionMethodWithNullActionNameThrows() {
            // Setup
            var controller = new FindMethodController();
            ControllerContext context = GetControllerContext(controller);
            ControllerActionInvokerHelper helper = new ControllerActionInvokerHelper(context);

            // Execute & verify
            ExceptionHelper.ExpectArgumentExceptionNullOrEmpty(
                delegate {
                    helper.PublicFindActionMethod(null /* actionName */, null /* values */);
                },
                "actionName");
        }

        [TestMethod]
        public void FindActionMethodWithOpenGenericsThrows() {
            // FindActionMethod() should throw if matching on a generic method with open types.

            // Setup
            var controller = new GenericFindMethodController<int>();
            ControllerContext context = GetControllerContext(controller);
            ControllerActionInvokerHelper helper = new ControllerActionInvokerHelper(context);

            // Execute & verify
            ExceptionHelper.ExpectException<InvalidOperationException>(
                delegate {
                    helper.PublicFindActionMethod("OpenGenericMethod", null /* values */);
                },
                "Cannot call action 'OpenGenericMethod' since it is a generic method.");
        }

        [TestMethod]
        public void FindActionMethodWithOverloadsThrows() {
            // FindActionMethod() should throw if it encounters an overloaded method.

            // Setup
            var controller = new FindMethodController();
            ControllerContext context = GetControllerContext(controller);
            ControllerActionInvokerHelper helper = new ControllerActionInvokerHelper(context);

            // Execute & verify
            ExceptionHelper.ExpectException<InvalidOperationException>(
                delegate {
                    helper.PublicFindActionMethod("MethodOverloaded", null /* values */);
                },
                "More than one action named 'MethodOverloaded' was found on controller 'System.Web.Mvc.Test.ControllerActionInvokerTest+FindMethodController'.");
        }

        [TestMethod]
        public void FindActionMethodWithValidMethod() {
            // Test basic functionality of FindActionMethod() by giving it a known good case.

            // Setup
            var controller = new FindMethodController();
            ControllerContext context = GetControllerContext(controller);
            ControllerActionInvokerHelper helper = new ControllerActionInvokerHelper(context);
            MethodInfo expected = typeof(FindMethodController).GetMethod("ValidActionMethod");

            // Execute
            MethodInfo mi = helper.PublicFindActionMethod("ValidActionMethod", null /* values */);

            // Verify
            Assert.AreEqual(expected, mi);
        }

        [TestMethod]
        public void GetFiltersForActionMethod() {
            // Setup
            IController controller = new GetMemberChainSubderivedController();
            ControllerContext context = GetControllerContext(controller);
            MethodInfo methodInfo = typeof(GetMemberChainSubderivedController).GetMethod("SomeVirtual");
            ControllerActionInvokerHelper helper = new ControllerActionInvokerHelper(context);

            // Execute
            var filters = helper.PublicGetFiltersForActionMethod(methodInfo);

            // Verify
            Assert.AreEqual(3, filters.AuthorizationFilters.Count, "Wrong number of authorization filters.");
            Assert.AreSame(controller, filters.AuthorizationFilters[0]);
            Assert.AreEqual("BaseClass", ((KeyedFilterAttribute)filters.AuthorizationFilters[1]).Key);
            Assert.AreEqual("BaseMethod", ((KeyedFilterAttribute)filters.AuthorizationFilters[2]).Key);

            Assert.AreEqual(6, filters.ActionFilters.Count, "Wrong number of action filters.");
            Assert.AreSame(controller, filters.ActionFilters[0]);
            Assert.AreEqual("BaseClass", ((KeyedFilterAttribute)filters.ActionFilters[1]).Key);
            Assert.AreEqual("BaseMethod", ((KeyedFilterAttribute)filters.ActionFilters[2]).Key);
            Assert.AreEqual("DerivedClass", ((KeyedFilterAttribute)filters.ActionFilters[3]).Key);
            Assert.AreEqual("SubderivedClass", ((KeyedFilterAttribute)filters.ActionFilters[4]).Key);
            Assert.AreEqual("SubderivedMethod", ((KeyedFilterAttribute)filters.ActionFilters[5]).Key);

            Assert.AreEqual(6, filters.ResultFilters.Count, "Wrong number of result filters.");
            Assert.AreSame(controller, filters.ResultFilters[0]);
            Assert.AreEqual("BaseClass", ((KeyedFilterAttribute)filters.ResultFilters[1]).Key);
            Assert.AreEqual("BaseMethod", ((KeyedFilterAttribute)filters.ResultFilters[2]).Key);
            Assert.AreEqual("DerivedClass", ((KeyedFilterAttribute)filters.ResultFilters[3]).Key);
            Assert.AreEqual("SubderivedClass", ((KeyedFilterAttribute)filters.ResultFilters[4]).Key);
            Assert.AreEqual("SubderivedMethod", ((KeyedFilterAttribute)filters.ResultFilters[5]).Key);

            Assert.AreEqual(1, filters.ExceptionFilters.Count, "Wrong number of exception filters.");
            Assert.AreSame(controller, filters.ExceptionFilters[0]);
        }

        [TestMethod]
        public void GetFiltersForActionMethodGetsDerivedClassFiltersForBaseClassMethod() {
            // DevDiv 208062: Action filters specified on derived class won't run if the action method is on a base class

            // Setup
            IController controller = new GetMemberChainDerivedController();
            ControllerContext context = GetControllerContext(controller);
            MethodInfo methodInfo = typeof(GetMemberChainDerivedController).GetMethod("SomeVirtual");
            ControllerActionInvokerHelper helper = new ControllerActionInvokerHelper(context);

            // Execute
            var filters = helper.PublicGetFiltersForActionMethod(methodInfo);

            // Verify
            Assert.AreEqual(3, filters.AuthorizationFilters.Count, "Wrong number of authorization filters.");
            Assert.AreSame(controller, filters.AuthorizationFilters[0]);
            Assert.AreEqual("BaseClass", ((KeyedFilterAttribute)filters.AuthorizationFilters[1]).Key);
            Assert.AreEqual("BaseMethod", ((KeyedFilterAttribute)filters.AuthorizationFilters[2]).Key);

            Assert.AreEqual(4, filters.ActionFilters.Count, "Wrong number of action filters.");
            Assert.AreSame(controller, filters.ActionFilters[0]);
            Assert.AreEqual("BaseClass", ((KeyedFilterAttribute)filters.ActionFilters[1]).Key);
            Assert.AreEqual("BaseMethod", ((KeyedFilterAttribute)filters.ActionFilters[2]).Key);
            Assert.AreEqual("DerivedClass", ((KeyedFilterAttribute)filters.ActionFilters[3]).Key);

            Assert.AreEqual(4, filters.ResultFilters.Count, "Wrong number of result filters.");
            Assert.AreSame(controller, filters.ResultFilters[0]);
            Assert.AreEqual("BaseClass", ((KeyedFilterAttribute)filters.ResultFilters[1]).Key);
            Assert.AreEqual("BaseMethod", ((KeyedFilterAttribute)filters.ResultFilters[2]).Key);
            Assert.AreEqual("DerivedClass", ((KeyedFilterAttribute)filters.ResultFilters[3]).Key);

            Assert.AreEqual(1, filters.ExceptionFilters.Count, "Wrong number of exception filters.");
            Assert.AreSame(controller, filters.ExceptionFilters[0]);
        }

        [TestMethod]
        public void GetFiltersForActionMethodWithNullMethodInfoThrows() {
            // Setup
            IController controller = new GetMemberChainSubderivedController();
            ControllerContext context = GetControllerContext(controller);
            ControllerActionInvokerHelper helper = new ControllerActionInvokerHelper(context);

            // Execute & verify
            ExceptionHelper.ExpectArgumentNullException(
                delegate {
                    helper.PublicGetFiltersForActionMethod(null /* methodInfo */);
                }, "methodInfo");
        }

        [TestMethod]
        public void GetParameterValueCanConvertDateTimeToString() {
            // String's TypeConverter can't convert from DateTime, but DateTime's converter can convert to String.
            // This tests that we call the "to" converter if the "from" converter reports failure.

            // Setup
            var controller = new ParameterTestingController();
            ControllerContext context = GetControllerContext(controller);
            Dictionary<string, object> dict = new Dictionary<string, object>() {
                { "id", new DateTime(2001, 2, 1) } // February 1, 2001
            };
            ControllerActionInvokerHelper helper = new ControllerActionInvokerHelper(context);
            MethodInfo mi = typeof(ParameterTestingController).GetMethod("TakesString");
            ParameterInfo[] pis = mi.GetParameters();

            // Execute
            object oValue = helper.PublicGetParameterValue(pis[0], dict);

            // Verify
            Assert.AreEqual("2001-02-01", oValue);
        }

        [TestMethod]
        public void GetParameterValueCanConvertStringToDateTime() {
            // Setup
            var controller = new ParameterTestingController();
            ControllerContext context = GetControllerContext(controller);
            Dictionary<string, object> dict = new Dictionary<string, object>() {
                { "id", "2001-02-01" } // February 1, 2001
            };
            ControllerActionInvokerHelper helper = new ControllerActionInvokerHelper(context);
            MethodInfo mi = typeof(ParameterTestingController).GetMethod("TakesDateTime");
            ParameterInfo[] pis = mi.GetParameters();

            // Execute
            object oValue = helper.PublicGetParameterValue(pis[0], dict);

            // Verify
            Assert.AreEqual(new DateTime(2001, 2, 1), oValue);
        }

        [TestMethod]
        public void GetParameterValueCanHandleNullHttpContext() {
            // GetParameterValue() should give up if HttpContext is null.

            // Setup
            Mock<HttpContextBase> baseMock = new Mock<HttpContextBase>();
            baseMock.Expect(o => o.Request).Returns((HttpRequestBase)null);
            baseMock.Expect(o => o.Session).Returns((HttpSessionStateBase)null);

            var controller = new ParameterTestingController();
            ControllerContext context = new ControllerContext(baseMock.Object, new RouteData(), controller);
            typeof(ControllerContext).GetProperty("HttpContext").SetValue(context, null /* value */, null /* index */);
            ControllerActionInvokerHelper helper = new ControllerActionInvokerHelper(context);
            MethodInfo mi = typeof(ParameterTestingController).GetMethod("Foo");
            ParameterInfo[] pis = mi.GetParameters();

            // Execute
            object oFoo = helper.PublicGetParameterValue(pis[0], null /* values */);

            // Verify
            Assert.IsNull(oFoo);
        }

        [TestMethod]
        public void GetParameterValueCanHandleNullRouteData() {
            // GetParameterValue() should fall back to the Request if RouteData is null.

            // Setup
            Mock<HttpRequestBase> requestMock = new Mock<HttpRequestBase>();
            ControllerTest.AddRequestParams(requestMock, new { foo = "RequestFoo" });
            Mock<HttpContextBase> baseMock = new Mock<HttpContextBase>();
            baseMock.Expect(o => o.Request).Returns(requestMock.Object);
            baseMock.Expect(o => o.Session).Returns((HttpSessionStateBase)null);

            var controller = new ParameterTestingController();
            ControllerContext context = new ControllerContext(baseMock.Object, new RouteData(), controller);
            typeof(ControllerContext).GetProperty("RouteData").SetValue(context, null /* value */, null /* index */);
            ControllerActionInvokerHelper helper = new ControllerActionInvokerHelper(context);
            MethodInfo mi = typeof(ParameterTestingController).GetMethod("Foo");
            ParameterInfo[] pis = mi.GetParameters();

            // Execute
            object oFoo = helper.PublicGetParameterValue(pis[0], null /* values */);

            // Verify
            Assert.AreEqual("RequestFoo", oFoo);
        }

        [TestMethod]
        public void GetParameterValueCanHandleNullValuesDictionary() {
            // GetParameterValue() should fall back to the RouteData if the values dictionary is null.

            // Setup
            Dictionary<string, object> explicitValues = null;
            RouteData routeData = new RouteData();
            routeData.Values["foo"] = "RouteDataFoo";
            Mock<HttpRequestBase> requestMock = new Mock<HttpRequestBase>();
            Mock<HttpContextBase> baseMock = new Mock<HttpContextBase>();
            baseMock.Expect(o => o.Request).Returns(requestMock.Object);
            baseMock.Expect(o => o.Session).Returns((HttpSessionStateBase)null);

            var controller = new ParameterTestingController();
            ControllerContext context = new ControllerContext(baseMock.Object, routeData, controller);
            ControllerActionInvokerHelper helper = new ControllerActionInvokerHelper(context);
            MethodInfo mi = typeof(ParameterTestingController).GetMethod("Foo");
            ParameterInfo[] pis = mi.GetParameters();

            // Execute
            object oFoo = helper.PublicGetParameterValue(pis[0], explicitValues);

            // Verify
            Assert.AreEqual("RouteDataFoo", oFoo);
        }

        [TestMethod]
        public void GetParameterValueForCustomClassWithInvalidParamsReturnsNull() {
            // Setup
            var controller = new ParameterTestingController();
            ControllerContext context = GetControllerContext(controller);
            Dictionary<string, object> dict = new Dictionary<string, object>() {
                { "mp", "z" }
            };
            ControllerActionInvokerHelper helper = new ControllerActionInvokerHelper(context);
            MethodInfo mi = typeof(ParameterTestingController).GetMethod("CustomClassConverter");
            ParameterInfo[] pis = mi.GetParameters();

            // Execute
            object oValue = helper.PublicGetParameterValue(pis[0], dict);

            // Verify
            Assert.IsNull(oValue);
        }

        [TestMethod]
        public void GetParameterValueForCustomClassWithNoParamsReturnsNull() {
            // Setup
            var controller = new ParameterTestingController();
            ControllerContext context = GetControllerContext(controller);
            Dictionary<string, object> dict = new Dictionary<string, object>();
            ControllerActionInvokerHelper helper = new ControllerActionInvokerHelper(context);
            MethodInfo mi = typeof(ParameterTestingController).GetMethod("CustomClassConverter");
            ParameterInfo[] pis = mi.GetParameters();

            // Execute
            object oValue = helper.PublicGetParameterValue(pis[0], dict);

            // Verify
            Assert.IsNull(oValue);
        }

        [TestMethod]
        public void GetParameterValueForCustomClassWithValidParamsReturnsValue() {
            // Setup
            var controller = new ParameterTestingController();
            ControllerContext context = GetControllerContext(controller);
            Dictionary<string, object> dict = new Dictionary<string, object>() {
                { "mp", "c123" }
            };
            ControllerActionInvokerHelper helper = new ControllerActionInvokerHelper(context);
            MethodInfo mi = typeof(ParameterTestingController).GetMethod("CustomClassConverter");
            ParameterInfo[] pis = mi.GetParameters();

            // Execute
            MyParameterClass mpValue = (MyParameterClass)helper.PublicGetParameterValue(pis[0], dict);

            // Verify
            Assert.AreEqual('c', mpValue.Char);
            Assert.AreEqual(123, mpValue.Number);
        }

        [TestMethod]
        public void GetParameterValueForCustomNullableStructWithBadTypeConverterReturnsNull() {
            // Setup
            var controller = new ParameterTestingController();
            ControllerContext context = GetControllerContext(controller);
            Dictionary<string, object> dict = new Dictionary<string, object>() {
                { "mp", "z" }
            };
            ControllerActionInvokerHelper helper = new ControllerActionInvokerHelper(context);
            MethodInfo mi = typeof(ParameterTestingController).GetMethod("CustomNullableStructConverterFromString");
            ParameterInfo[] pis = mi.GetParameters();

            // Execute
            object oValue = helper.PublicGetParameterValue(pis[0], dict);

            // Verify
            Assert.IsNull(oValue);
        }

        [TestMethod]
        public void GetParameterValueForCustomNullableStructWithInvalidParamsReturnsNull() {
            // Setup
            var controller = new ParameterTestingController();
            ControllerContext context = GetControllerContext(controller);
            Dictionary<string, object> dict = new Dictionary<string, object>() {
                { "mp", "z" }
            };
            ControllerActionInvokerHelper helper = new ControllerActionInvokerHelper(context);
            MethodInfo mi = typeof(ParameterTestingController).GetMethod("CustomNullableStructConverter");
            ParameterInfo[] pis = mi.GetParameters();

            // Execute
            object oValue = helper.PublicGetParameterValue(pis[0], dict);

            // Verify
            Assert.IsNull(oValue);
        }

        [TestMethod]
        public void GetParameterValueForCustomStructWithNoParamsReturnsNull() {
            // Setup
            var controller = new ParameterTestingController();
            ControllerContext context = GetControllerContext(controller);
            Dictionary<string, object> dict = new Dictionary<string, object>();
            ControllerActionInvokerHelper helper = new ControllerActionInvokerHelper(context);
            MethodInfo mi = typeof(ParameterTestingController).GetMethod("CustomNullableStructConverter");
            ParameterInfo[] pis = mi.GetParameters();

            // Execute
            object oValue = helper.PublicGetParameterValue(pis[0], dict);

            // Verify
            Assert.IsNull(oValue);
        }

        [TestMethod]
        public void GetParameterValueForCustomNullableStructWithValidParamsReturnsValue() {
            // Setup
            var controller = new ParameterTestingController();
            ControllerContext context = GetControllerContext(controller);
            Dictionary<string, object> dict = new Dictionary<string, object>() {
                { "mp", "c123" }
            };
            ControllerActionInvokerHelper helper = new ControllerActionInvokerHelper(context);
            MethodInfo mi = typeof(ParameterTestingController).GetMethod("CustomNullableStructConverter");
            ParameterInfo[] pis = mi.GetParameters();

            // Execute
            MyParameterStruct mpValue = (MyParameterStruct)helper.PublicGetParameterValue(pis[0], dict);

            // Verify
            Assert.AreEqual('c', mpValue.Char);
            Assert.AreEqual(123, mpValue.Number);
        }

        [TestMethod]
        public void GetParameterValueForCustomStructWithBadTypeConverterThrows() {
            // Setup
            var controller = new ParameterTestingController();
            ControllerContext context = GetControllerContext(controller);
            Dictionary<string, object> dict = new Dictionary<string, object>() {
                { "mp", "z" }
            };
            ControllerActionInvokerHelper helper = new ControllerActionInvokerHelper(context);
            MethodInfo mi = typeof(ParameterTestingController).GetMethod("CustomStructConverterFromString");
            ParameterInfo[] pis = mi.GetParameters();

            // Execute & verify
            ExceptionHelper.ExpectException<InvalidOperationException>(
                delegate {
                    helper.PublicGetParameterValue(pis[0], dict);
                },
               "A value is required for parameter 'mp' in action 'CustomStructConverterFromString'. The parameter either has no value "
                    + "or its value could not be converted. To make a parameter optional its type should either be a reference type or a Nullable type.");
        }

        [TestMethod]
        public void GetParameterValueForCustomStructWithInvalidParamsThrows() {
            // Setup
            var controller = new ParameterTestingController();
            ControllerContext context = GetControllerContext(controller);
            Dictionary<string, object> dict = new Dictionary<string, object>() {
                { "mp", "z" }
            };
            ControllerActionInvokerHelper helper = new ControllerActionInvokerHelper(context);
            MethodInfo mi = typeof(ParameterTestingController).GetMethod("CustomStructConverter");
            ParameterInfo[] pis = mi.GetParameters();

            // Execute & verify
            ExceptionHelper.ExpectException<InvalidOperationException>(
                delegate {
                    helper.PublicGetParameterValue(pis[0], dict);
                },
               "A value is required for parameter 'mp' in action 'CustomStructConverter'. The parameter either has no value or its value "
                    + "could not be converted. To make a parameter optional its type should either be a reference type or a Nullable type.");
        }

        [TestMethod]
        public void GetParameterValueForCustomStructWithNoParamsThrows() {
            // Setup
            var controller = new ParameterTestingController();
            ControllerContext context = GetControllerContext(controller);
            Dictionary<string, object> dict = new Dictionary<string, object>();
            ControllerActionInvokerHelper helper = new ControllerActionInvokerHelper(context);
            MethodInfo mi = typeof(ParameterTestingController).GetMethod("CustomStructConverter");
            ParameterInfo[] pis = mi.GetParameters();

            // Execute & verify
            ExceptionHelper.ExpectException<InvalidOperationException>(
                delegate {
                    helper.PublicGetParameterValue(pis[0], dict);
                },
                "A value is required for parameter 'mp' in action 'CustomStructConverter'. The parameter either has no value or its "
                    + "value could not be converted. To make a parameter optional its type should either be a reference type or a Nullable type.");
        }

        [TestMethod]
        public void GetParameterValueForCustomStructWithValidParamsReturnsValue() {
            // Setup
            var controller = new ParameterTestingController();
            ControllerContext context = GetControllerContext(controller);
            Dictionary<string, object> dict = new Dictionary<string, object>() {
                { "mp", "c123" }
            };
            ControllerActionInvokerHelper helper = new ControllerActionInvokerHelper(context);
            MethodInfo mi = typeof(ParameterTestingController).GetMethod("CustomStructConverter");
            ParameterInfo[] pis = mi.GetParameters();

            // Execute
            MyParameterStruct mpValue = (MyParameterStruct)helper.PublicGetParameterValue(pis[0], dict);

            // Verify
            Assert.AreEqual('c', mpValue.Char);
            Assert.AreEqual(123, mpValue.Number);
        }

        [TestMethod]
        public void GetParameterValueHasCorrectOrderOfPrecedence() {
            // Order of precedence:
            //   1. Explicitly-provided extra parameters in the call to InvokeAction()
            //   2. Values from the RouteData (could be from the typed-in URL or from the route's default values)
            //   3. Request values (query string, form post data, cookie)

            // Setup
            Dictionary<string, object> explicitValues = new Dictionary<string, object>() {
                { "foo", "ExplicitFoo" }
            };
            RouteData routeData = new RouteData();
            routeData.Values["foo"] = "RouteDataFoo";
            routeData.Values["bar"] = "RouteDataBar";
            Mock<HttpRequestBase> requestMock = new Mock<HttpRequestBase>();
            ControllerTest.AddRequestParams(requestMock, new { foo = "RequestFoo", bar = "RequestBar", baz = "RequestBaz" });
            Mock<HttpContextBase> baseMock = new Mock<HttpContextBase>();
            baseMock.Expect(o => o.Request).Returns(requestMock.Object);
            baseMock.Expect(o => o.Session).Returns((HttpSessionStateBase)null);

            var controller = new ParameterTestingController();
            ControllerContext context = new ControllerContext(baseMock.Object, routeData, controller);
            ControllerActionInvokerHelper helper = new ControllerActionInvokerHelper(context);
            MethodInfo mi = typeof(ParameterTestingController).GetMethod("Foo");
            ParameterInfo[] pis = mi.GetParameters();

            // Execute
            object oFoo = helper.PublicGetParameterValue(pis[0], explicitValues);
            object oBar = helper.PublicGetParameterValue(pis[1], explicitValues);
            object oBaz = helper.PublicGetParameterValue(pis[2], explicitValues);

            // Verify
            Assert.AreEqual("ExplicitFoo", oFoo);
            Assert.AreEqual("RouteDataBar", oBar);
            Assert.AreEqual("RequestBaz", oBaz);
        }

        [TestMethod]
        public void GetParameterValueReturnsNullIfCannotConvertNonRequiredParameter() {
            // Setup
            var controller = new ParameterTestingController();
            ControllerContext context = GetControllerContext(controller);
            Dictionary<string, object> dict = new Dictionary<string, object>() {
                { "id", DateTime.Now } // cannot convert DateTime to Nullable<int>
            };
            ControllerActionInvokerHelper helper = new ControllerActionInvokerHelper(context);
            MethodInfo mi = typeof(ParameterTestingController).GetMethod("TakesNullableInt");
            ParameterInfo[] pis = mi.GetParameters();

            // Execute
            object oValue = helper.PublicGetParameterValue(pis[0], dict);

            // Verify
            Assert.IsNull(oValue);
        }

        [TestMethod]
        public void GetParameterValueReturnsNullIfNullableTypeValueNotFound() {
            // Setup
            var controller = new ParameterTestingController();
            ControllerContext context = GetControllerContext(controller);
            ControllerActionInvokerHelper helper = new ControllerActionInvokerHelper(context);
            MethodInfo mi = typeof(ParameterTestingController).GetMethod("TakesNullableInt");
            ParameterInfo[] pis = mi.GetParameters();

            // Execute
            object oValue = helper.PublicGetParameterValue(pis[0], null /* values */);

            // Verify
            Assert.IsNull(oValue);
        }

        [TestMethod]
        public void GetParameterValueReturnsNullIfReferenceTypeValueNotFound() {
            // Setup
            var controller = new ParameterTestingController();
            ControllerContext context = GetControllerContext(controller);
            ControllerActionInvokerHelper helper = new ControllerActionInvokerHelper(context);
            MethodInfo mi = typeof(ParameterTestingController).GetMethod("Foo");
            ParameterInfo[] pis = mi.GetParameters();

            // Execute
            object oValue = helper.PublicGetParameterValue(pis[0], null /* values */);

            // Verify
            Assert.IsNull(oValue);
        }

        [TestMethod]
        public void GetParameterValuesCallsGetParameterValue() {
            // Setup
            IController controller = new ParameterTestingController();
            IDictionary<string, object> dict = new Dictionary<string, object>();
            ControllerContext context = GetControllerContext(controller);
            MethodInfo mi = typeof(ParameterTestingController).GetMethod("Foo");
            ParameterInfo[] pis = mi.GetParameters();

            Mock<ControllerActionInvokerHelper> mockHelper = new Mock<ControllerActionInvokerHelper>(context);
            mockHelper.Expect(h => h.PublicGetParameterValue(pis[0], dict)).Returns("Myfoo").Verifiable();
            mockHelper.Expect(h => h.PublicGetParameterValue(pis[1], dict)).Returns("Mybar").Verifiable();
            mockHelper.Expect(h => h.PublicGetParameterValue(pis[2], dict)).Returns("Mybaz").Verifiable();
            ControllerActionInvokerHelper helper = mockHelper.Object;

            // Execute
            IDictionary<string, object> parameters = helper.PublicGetParameterValues(mi, dict);

            // Verify
            Assert.AreEqual(3, parameters.Count);
            Assert.AreEqual("Myfoo", parameters["foo"]);
            Assert.AreEqual("Mybar", parameters["bar"]);
            Assert.AreEqual("Mybaz", parameters["baz"]);
            mockHelper.Verify();
        }

        [TestMethod]
        public void GetParameterValuesReturnsEmptyArrayForParameterlessMethod() {
            // Setup
            var controller = new ParameterTestingController();
            ControllerContext context = GetControllerContext(controller);
            ControllerActionInvokerHelper helper = new ControllerActionInvokerHelper(context);
            MethodInfo mi = typeof(ParameterTestingController).GetMethod("Parameterless");

            // Execute
            IDictionary<string, object> parameters = helper.PublicGetParameterValues(mi, null /* values */);

            // Verify
            Assert.AreEqual(0, parameters.Count);
        }

        [TestMethod]
        public void GetParameterValuesReturnsValuesForParametersInOrder() {
            // We need to hook into GetParameterValue() to make sure that GetParameterValues() is calling it.

            // Setup
            var controller = new ParameterTestingController();
            ControllerContext context = GetControllerContext(controller);
            Dictionary<string, object> dict = new Dictionary<string, object>() {
                { "foo", "MyFoo" },
                { "bar", "MyBar" },
                { "baz", "MyBaz" }
            };
            ControllerActionInvokerHelper helper = new ControllerActionInvokerHelper(context);
            MethodInfo mi = typeof(ParameterTestingController).GetMethod("Foo");

            // Execute
            IDictionary<string, object> parameters = helper.PublicGetParameterValues(mi, dict);

            // Verify
            Assert.AreEqual(3, parameters.Count);
            Assert.AreEqual("MyFoo", parameters["foo"]);
            Assert.AreEqual("MyBar", parameters["bar"]);
            Assert.AreEqual("MyBaz", parameters["baz"]);
        }

        [TestMethod]
        public void GetParameterValuesWithNullMethodInfoThrows() {
            // Setup
            var controller = new ParameterTestingController();
            ControllerContext context = GetControllerContext(controller);
            ControllerActionInvokerHelper helper = new ControllerActionInvokerHelper(context);

            // Execute & verify
            ExceptionHelper.ExpectArgumentNullException(
                delegate {
                    helper.PublicGetParameterValues(null /* methodInfo */, null /* values */);
                },
                "methodInfo");
        }

        [TestMethod]
        public void GetParameterValuesWithOutParamThrows() {
            // Setup
            var controller = new ParameterTestingController();
            ControllerContext context = GetControllerContext(controller);
            ControllerActionInvokerHelper helper = new ControllerActionInvokerHelper(context);
            MethodInfo mi = typeof(ParameterTestingController).GetMethod("HasOutParam");

            // Execute & verify
            ExceptionHelper.ExpectException<InvalidOperationException>(
                delegate {
                    helper.PublicGetParameterValues(mi, null /* values */);
                },
                "Cannot set value for parameter 'foo' in action 'HasOutParam'. Parameters passed by reference are not supported in action methods.");
        }

        [TestMethod]
        public void GetParameterValuesWithRefParamThrows() {
            // Setup
            var controller = new ParameterTestingController();
            ControllerContext context = GetControllerContext(controller);
            ControllerActionInvokerHelper helper = new ControllerActionInvokerHelper(context);
            MethodInfo mi = typeof(ParameterTestingController).GetMethod("HasRefParam");

            // Execute & verify
            ExceptionHelper.ExpectException<InvalidOperationException>(
                delegate {
                    helper.PublicGetParameterValues(mi, null /* values */);
                },
                "Cannot set value for parameter 'foo' in action 'HasRefParam'. Parameters passed by reference are not supported in action methods.");
        }

        [TestMethod]
        public void GetParameterValueThrowsIfCannotConvertRequiredParameter() {
            // Setup
            var controller = new ParameterTestingController();
            ControllerContext context = GetControllerContext(controller);
            Dictionary<string, object> dict = new Dictionary<string, object>() {
                { "id", new DateTime(2001, 1, 1) } // cannot convert DateTime to int
            };
            ControllerActionInvokerHelper helper = new ControllerActionInvokerHelper(context);
            MethodInfo mi = typeof(ParameterTestingController).GetMethod("TakesInt");
            ParameterInfo[] pis = mi.GetParameters();

            // Execute & verify - use a manual try/catch so we can examine inner exception
            try {
                helper.PublicGetParameterValue(pis[0], dict);
            }
            catch (InvalidOperationException e) {
                Assert.AreEqual("A value is required for parameter 'id' in action 'TakesInt'. The parameter either has no value or its value could "
                    + "not be converted. To make a parameter optional its type should either be a reference type or a Nullable type.",
                    e.Message);
                Assert.IsInstanceOfType(e.InnerException, typeof(InvalidOperationException));
                Assert.AreEqual("Cannot convert parameter 'id' in action 'TakesInt' with value '1/1/2001 12:00:00 AM' to type 'System.Int32'.",
                    e.InnerException.Message);

                return; // success!
            }
            Assert.Fail("Should have thrown an InvalidOperationException.");
        }

        [TestMethod]
        public void GetParameterValueThrowsIfValueTypeValueNotFound() {
            // Setup
            var controller = new ParameterTestingController();
            ControllerContext context = GetControllerContext(controller);
            ControllerActionInvokerHelper helper = new ControllerActionInvokerHelper(context);
            MethodInfo mi = typeof(ParameterTestingController).GetMethod("TakesInt");
            ParameterInfo[] pis = mi.GetParameters();

            // Execute & verify
            ExceptionHelper.ExpectException<InvalidOperationException>(
                delegate {
                    helper.PublicGetParameterValue(pis[0], null /* values */);
                },
                "A value is required for parameter 'id' in action 'TakesInt'. The parameter either has no value or its value could "
                    + "not be converted. To make a parameter optional its type should either be a reference type or a Nullable type.");
        }

        [TestMethod]
        public void GetParameterValueWithNullParameterInfoThrows() {
            // Setup
            var controller = new ParameterTestingController();
            ControllerContext context = GetControllerContext(controller);
            ControllerActionInvokerHelper helper = new ControllerActionInvokerHelper(context);

            // Execute & verify
            ExceptionHelper.ExpectArgumentNullException(
                delegate {
                    helper.PublicGetParameterValue(null /* parameterInfo */, null /* values */);
                },
                "parameterInfo");
        }

        [TestMethod]
        public void InvokeAction() {
            // Setup
            IController controller = new Mock<IController>().Object;
            ControllerContext context = GetControllerContext(controller);
            IDictionary<string, object> values = new Dictionary<string, object>();
            IDictionary<string, object> paramValues = new Dictionary<string, object>();
            MethodInfo methodInfo = typeof(object).GetMethod("ToString");
            var filterInfo = new FilterInfo();
            ActionResult actionResult = new EmptyResult();
            ActionExecutedContext postContext = new ActionExecutedContext(context, methodInfo, false /* canceled */, null /* exception */) {
                Result = actionResult
            };
            AuthorizationContext authContext = new AuthorizationContext(context, methodInfo);

            Mock<ControllerActionInvokerHelper> mockHelper = new Mock<ControllerActionInvokerHelper>(context);
            mockHelper.Expect(h => h.PublicFindActionMethod("SomeMethod", values)).Returns(methodInfo).Verifiable();
            mockHelper.Expect(h => h.PublicGetParameterValues(methodInfo, values)).Returns(paramValues).Verifiable();
            mockHelper.Expect(h => h.PublicGetFiltersForActionMethod(methodInfo)).Returns(filterInfo).Verifiable();
            mockHelper.Expect(h => h.PublicInvokeAuthorizationFilters(methodInfo, filterInfo.AuthorizationFilters)).Returns(authContext).Verifiable();
            mockHelper.Expect(h => h.PublicInvokeActionMethodWithFilters(methodInfo, paramValues, filterInfo.ActionFilters)).Returns(postContext).Verifiable();
            mockHelper.Expect(h => h.PublicInvokeActionResultWithFilters(actionResult, filterInfo.ResultFilters)).Returns((ResultExecutedContext)null).Verifiable();
            ControllerActionInvokerHelper helper = mockHelper.Object;

            // Execute
            bool retVal = helper.InvokeAction("SomeMethod", values);
            Assert.IsTrue(retVal, "InvokeAction() should return True on success.");
            mockHelper.Verify();
        }

        [TestMethod]
        public void InvokeActionFiltersOrdersFiltersCorrectly() {
            // Setup
            List<string> actions = new List<string>();
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            ActionResult actionResult = new EmptyResult();
            ActionFilterImpl filter1 = new ActionFilterImpl() {
                OnActionExecutingImpl = delegate(ActionExecutingContext filterContext) {
                    actions.Add("OnActionExecuting1");
                },
                OnActionExecutedImpl = delegate(ActionExecutedContext filterContext) {
                    actions.Add("OnActionExecuted1");
                }
            };
            ActionFilterImpl filter2 = new ActionFilterImpl() {
                OnActionExecutingImpl = delegate(ActionExecutingContext filterContext) {
                    actions.Add("OnActionExecuting2");
                },
                OnActionExecutedImpl = delegate(ActionExecutedContext filterContext) {
                    actions.Add("OnActionExecuted2");
                }
            };
            Func<ActionResult> continuation = delegate {
                actions.Add("Continuation");
                return new EmptyResult();
            };
            IController controller = new ContinuationController(continuation);
            ControllerContext context = GetControllerContext(controller);
            ControllerActionInvokerHelper helper = new ControllerActionInvokerHelper(context);

            // Execute
            helper.PublicInvokeActionMethodWithFilters(ContinuationController.GoMethod, parameters,
                new List<IActionFilter>() { filter1, filter2 });

            // Verify
            Assert.AreEqual(5, actions.Count);
            Assert.AreEqual("OnActionExecuting1", actions[0]);
            Assert.AreEqual("OnActionExecuting2", actions[1]);
            Assert.AreEqual("Continuation", actions[2]);
            Assert.AreEqual("OnActionExecuted2", actions[3]);
            Assert.AreEqual("OnActionExecuted1", actions[4]);
        }

        [TestMethod]
        public void InvokeActionFiltersPassesArgumentsCorrectly() {
            // Setup
            bool wasCalled = false;
            MethodInfo mi = ContinuationController.GoMethod;
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            ActionResult actionResult = new EmptyResult();
            ActionFilterImpl filter = new ActionFilterImpl() {
                OnActionExecutingImpl = delegate(ActionExecutingContext filterContext) {
                    Assert.AreSame(mi, filterContext.ActionMethod);
                    Assert.AreSame(parameters, filterContext.ActionParameters);
                    Assert.IsFalse(wasCalled);
                    wasCalled = true;
                    filterContext.Cancel = true;
                    filterContext.Result = actionResult;
                }
            };
            Func<ActionResult> continuation = delegate {
                Assert.Fail("Continuation should not be called.");
                return null;
            };
            IController controller = new ContinuationController(continuation);
            ControllerContext context = GetControllerContext(controller);
            ControllerActionInvokerHelper helper = new ControllerActionInvokerHelper(context);

            // Execute
            ActionExecutedContext result = helper.PublicInvokeActionMethodWithFilters(mi, parameters,
                new List<IActionFilter>() { filter });

            // Verify
            Assert.IsTrue(wasCalled);
            Assert.AreSame(mi, result.ActionMethod);
            Assert.IsNull(result.Exception);
            Assert.IsFalse(result.ExceptionHandled);
            Assert.AreSame(actionResult, result.Result);
        }

        [TestMethod]
        public void InvokeActionFilterWhereContinuationThrowsExceptionAndIsHandled() {
            // Setup
            List<string> actions = new List<string>();
            MethodInfo mi = typeof(object).GetMethod("ToString");
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            Exception exception = new Exception();
            ActionFilterImpl filter = new ActionFilterImpl() {
                OnActionExecutingImpl = delegate(ActionExecutingContext filterContext) {
                    actions.Add("OnActionExecuting");
                },
                OnActionExecutedImpl = delegate(ActionExecutedContext filterContext) {
                    actions.Add("OnActionExecuted");
                    Assert.AreSame(mi, filterContext.ActionMethod);
                    Assert.AreSame(exception, filterContext.Exception);
                    Assert.IsFalse(filterContext.ExceptionHandled);
                    filterContext.ExceptionHandled = true;
                }
            };
            Func<ActionExecutedContext> continuation = delegate {
                actions.Add("Continuation");
                throw exception;
            };

            // Execute
            ActionExecutedContext result = ControllerActionInvoker.InvokeActionMethodFilter(filter, new ActionExecutingContext(ControllerContextTest.GetControllerContext(), mi, parameters), continuation);

            // Verify
            Assert.AreEqual(3, actions.Count);
            Assert.AreEqual("OnActionExecuting", actions[0]);
            Assert.AreEqual("Continuation", actions[1]);
            Assert.AreEqual("OnActionExecuted", actions[2]);
            Assert.AreSame(mi, result.ActionMethod);
            Assert.AreSame(exception, result.Exception);
            Assert.IsTrue(result.ExceptionHandled);
        }

        [TestMethod]
        public void InvokeActionFilterWhereContinuationThrowsExceptionAndIsNotHandled() {
            // Setup
            List<string> actions = new List<string>();
            MethodInfo mi = typeof(object).GetMethod("ToString");
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            ActionFilterImpl filter = new ActionFilterImpl() {
                OnActionExecutingImpl = delegate(ActionExecutingContext filterContext) {
                    actions.Add("OnActionExecuting");
                },
                OnActionExecutedImpl = delegate(ActionExecutedContext filterContext) {
                    actions.Add("OnActionExecuted");
                }
            };
            Func<ActionExecutedContext> continuation = delegate {
                actions.Add("Continuation");
                throw new Exception("Some exception message.");
            };

            // Execute & verify
            ExceptionHelper.ExpectException<Exception>(
                delegate {
                    ControllerActionInvoker.InvokeActionMethodFilter(filter, new ActionExecutingContext(ControllerContextTest.GetControllerContext(), mi, parameters), continuation);
                },
               "Some exception message.");
            Assert.AreEqual(3, actions.Count);
            Assert.AreEqual("OnActionExecuting", actions[0]);
            Assert.AreEqual("Continuation", actions[1]);
            Assert.AreEqual("OnActionExecuted", actions[2]);
        }

        [TestMethod]
        public void InvokeActionFilterWhereOnActionExecutingCancels() {
            // Setup
            bool wasCalled = false;
            MethodInfo mi = typeof(object).GetMethod("ToString");
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            ActionExecutedContext postContext = new ActionExecutedContext(ControllerContextTest.GetControllerContext(), mi, false /* canceled */, null /* exception */);
            ActionResult actionResult = new EmptyResult();
            ActionFilterImpl filter = new ActionFilterImpl() {
                OnActionExecutingImpl = delegate(ActionExecutingContext filterContext) {
                    Assert.IsFalse(wasCalled);
                    wasCalled = true;
                    filterContext.Cancel = true;
                    filterContext.Result = actionResult;
                },
            };
            Func<ActionExecutedContext> continuation = delegate {
                Assert.Fail("The continuation should not be called.");
                return null;
            };

            // Execute
            ActionExecutedContext result = ControllerActionInvoker.InvokeActionMethodFilter(filter, new ActionExecutingContext(ControllerContextTest.GetControllerContext(), mi, parameters), continuation);

            // Verify
            Assert.IsTrue(wasCalled);
            Assert.AreSame(mi, result.ActionMethod);
            Assert.IsNull(result.Exception);
            Assert.IsTrue(result.Canceled);
            Assert.AreSame(actionResult, result.Result);
        }

        [TestMethod]
        public void InvokeActionFilterWithNormalControlFlow() {
            // Setup
            List<string> actions = new List<string>();
            MethodInfo mi = typeof(object).GetMethod("ToString");
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            ActionExecutedContext postContext = new ActionExecutedContext(ControllerContextTest.GetControllerContext(), mi, false /* canceled */, null /* exception */);
            ActionFilterImpl filter = new ActionFilterImpl() {
                OnActionExecutingImpl = delegate(ActionExecutingContext filterContext) {
                    Assert.AreSame(mi, filterContext.ActionMethod);
                    Assert.AreSame(parameters, filterContext.ActionParameters);
                    Assert.IsFalse(filterContext.Cancel);
                    Assert.IsNull(filterContext.Result);
                    actions.Add("OnActionExecuting");
                },
                OnActionExecutedImpl = delegate(ActionExecutedContext filterContext) {
                    Assert.AreEqual(postContext, filterContext);
                    actions.Add("OnActionExecuted");
                }
            };
            Func<ActionExecutedContext> continuation = delegate {
                actions.Add("Continuation");
                return postContext;
            };

            // Execute
            ActionExecutedContext result = ControllerActionInvoker.InvokeActionMethodFilter(filter, new ActionExecutingContext(ControllerContextTest.GetControllerContext(), mi, parameters), continuation);

            // Verify
            Assert.AreEqual(3, actions.Count);
            Assert.AreEqual("OnActionExecuting", actions[0]);
            Assert.AreEqual("Continuation", actions[1]);
            Assert.AreEqual("OnActionExecuted", actions[2]);
            Assert.AreSame(result, postContext);
        }

        [TestMethod]
        public void InvokeActionInvokesEmptyResultIfAuthorizationFailsAndNoResultSpecified() {
            // Setup
            IController controller = new Mock<IController>().Object;
            ControllerContext context = GetControllerContext(controller);
            IDictionary<string, object> values = new Dictionary<string, object>();
            IDictionary<string, object> paramValues = new Dictionary<string, object>();
            MethodInfo methodInfo = typeof(object).GetMethod("ToString");
            var filterInfo = new FilterInfo();
            ActionResult actionResult = new EmptyResult();
            ActionExecutedContext postContext = new ActionExecutedContext(context, methodInfo, false /* canceled */, null /* exception */) {
                Result = actionResult
            };
            AuthorizationContext authContext = new AuthorizationContext(context, methodInfo) { Cancel = true };

            Mock<ControllerActionInvokerHelper> mockHelper = new Mock<ControllerActionInvokerHelper>(context);
            mockHelper.Expect(h => h.PublicFindActionMethod("SomeMethod", values)).Returns(methodInfo).Verifiable();
            mockHelper.Expect(h => h.PublicGetParameterValues(methodInfo, values)).Returns(paramValues).Verifiable();
            mockHelper.Expect(h => h.PublicGetFiltersForActionMethod(methodInfo)).Returns(filterInfo).Verifiable();
            mockHelper.Expect(h => h.PublicInvokeAuthorizationFilters(methodInfo, filterInfo.AuthorizationFilters)).Returns(authContext).Verifiable();
            mockHelper.Expect(h => h.PublicInvokeActionResult(EmptyResult.Instance)).Verifiable();
            ControllerActionInvokerHelper helper = mockHelper.Object;

            // Execute
            bool retVal = helper.InvokeAction("SomeMethod", values);
            Assert.IsTrue(retVal, "InvokeAction() should return True on success.");
            mockHelper.Verify();
        }

        [TestMethod]
        public void InvokeActionInvokesExceptionFiltersAndExecutesResultIfExceptionHandled() {
            // Setup
            IController controller = new Mock<IController>().Object;
            ControllerContext context = GetControllerContext(controller);
            IDictionary<string, object> values = new Dictionary<string, object>();
            IDictionary<string, object> paramValues = new Dictionary<string, object>();
            MethodInfo methodInfo = typeof(object).GetMethod("ToString");
            var filterInfo = new FilterInfo();
            Exception exception = new Exception();
            ActionResult actionResult = new EmptyResult();
            ExceptionContext exContext = new ExceptionContext(context, exception) {
                ExceptionHandled = true,
                Result = actionResult
            };

            Mock<ControllerActionInvokerHelper> mockHelper = new Mock<ControllerActionInvokerHelper>(context);
            mockHelper.Expect(h => h.PublicFindActionMethod("SomeMethod", values)).Returns(methodInfo).Verifiable();
            mockHelper.Expect(h => h.PublicGetParameterValues(methodInfo, values)).Returns(paramValues).Verifiable();
            mockHelper.Expect(h => h.PublicGetFiltersForActionMethod(methodInfo)).Returns(filterInfo).Verifiable();
            mockHelper.Expect(h => h.PublicInvokeAuthorizationFilters(methodInfo, filterInfo.AuthorizationFilters)).Throws(exception).Verifiable();
            mockHelper.Expect(h => h.PublicInvokeExceptionFilters(exception, filterInfo.ExceptionFilters)).Returns(exContext).Verifiable();
            mockHelper.Expect(h => h.PublicInvokeActionResult(actionResult)).Verifiable();
            ControllerActionInvokerHelper helper = mockHelper.Object;

            // Execute
            bool retVal = helper.InvokeAction("SomeMethod", values);
            Assert.IsTrue(retVal, "InvokeAction() should return True on success.");
            mockHelper.Verify();
        }

        [TestMethod]
        public void InvokeActionInvokesExceptionFiltersAndRethrowsExceptionIfNotHandled() {
            // Setup
            IController controller = new Mock<IController>().Object;
            ControllerContext context = GetControllerContext(controller);
            IDictionary<string, object> values = new Dictionary<string, object>();
            IDictionary<string, object> paramValues = new Dictionary<string, object>();
            MethodInfo methodInfo = typeof(object).GetMethod("ToString");
            var filterInfo = new FilterInfo();
            Exception exception = new Exception();
            ExceptionContext exContext = new ExceptionContext(context, exception);

            Mock<ControllerActionInvokerHelper> mockHelper = new Mock<ControllerActionInvokerHelper>(context);
            mockHelper.Expect(h => h.PublicFindActionMethod("SomeMethod", values)).Returns(methodInfo).Verifiable();
            mockHelper.Expect(h => h.PublicGetParameterValues(methodInfo, values)).Returns(paramValues).Verifiable();
            mockHelper.Expect(h => h.PublicGetFiltersForActionMethod(methodInfo)).Returns(filterInfo).Verifiable();
            mockHelper.Expect(h => h.PublicInvokeAuthorizationFilters(methodInfo, filterInfo.AuthorizationFilters)).Throws(exception).Verifiable();
            mockHelper.Expect(h => h.PublicInvokeExceptionFilters(exception, filterInfo.ExceptionFilters)).Returns(exContext).Verifiable();
            mockHelper.Expect(h => h.PublicInvokeActionResult(It.IsAny<ActionResult>())).Callback(delegate {
                Assert.Fail("InvokeActionResult() shouldn't be called if the exception was unhandled by filters.");
            });
            ControllerActionInvokerHelper helper = mockHelper.Object;

            // Execute
            Exception thrownException = ExceptionHelper.ExpectException<Exception>(
                delegate {
                    helper.InvokeAction("SomeMethod", values);
                });

            // Verify
            Assert.AreSame(exception, thrownException);
            mockHelper.Verify();
        }

        [TestMethod]
        public void InvokeActionInvokesResultIfAuthorizationFails() {
            // Setup
            IController controller = new Mock<IController>().Object;
            ControllerContext context = GetControllerContext(controller);
            IDictionary<string, object> values = new Dictionary<string, object>();
            IDictionary<string, object> paramValues = new Dictionary<string, object>();
            MethodInfo methodInfo = typeof(object).GetMethod("ToString");
            var filterInfo = new FilterInfo();
            ActionResult actionResult = new EmptyResult();
            ActionExecutedContext postContext = new ActionExecutedContext(context, methodInfo, false /* canceled */, null /* exception */) {
                Result = actionResult
            };
            AuthorizationContext authContext = new AuthorizationContext(context, methodInfo) { Cancel = true, Result = actionResult };

            Mock<ControllerActionInvokerHelper> mockHelper = new Mock<ControllerActionInvokerHelper>(context);
            mockHelper.Expect(h => h.PublicFindActionMethod("SomeMethod", values)).Returns(methodInfo).Verifiable();
            mockHelper.Expect(h => h.PublicGetParameterValues(methodInfo, values)).Returns(paramValues).Verifiable();
            mockHelper.Expect(h => h.PublicGetFiltersForActionMethod(methodInfo)).Returns(filterInfo).Verifiable();
            mockHelper.Expect(h => h.PublicInvokeAuthorizationFilters(methodInfo, filterInfo.AuthorizationFilters)).Returns(authContext).Verifiable();
            mockHelper.Expect(h => h.PublicInvokeActionResult(actionResult)).Verifiable();
            ControllerActionInvokerHelper helper = mockHelper.Object;

            // Execute
            bool retVal = helper.InvokeAction("SomeMethod", values);
            Assert.IsTrue(retVal, "InvokeAction() should return True on success.");
            mockHelper.Verify();
        }

        [TestMethod]
        public void InvokeActionMethodWithActionResultReturnValue() {
            // Setup
            var controller = new BasicMethodInvokeController();
            ControllerContext context = GetControllerContext(controller);
            ControllerActionInvokerHelper helper = new ControllerActionInvokerHelper(context);
            MethodInfo mi = typeof(BasicMethodInvokeController).GetMethod("ReturnsRenderView");
            object viewItem = new object();
            IDictionary<string, object> parameters = new Dictionary<string, object>() {
                { "viewItem", viewItem }
            };

            // Execute
            ViewResult result = helper.PublicInvokeActionMethod(mi, parameters) as ViewResult;

            // Verify (arg got passed to method + back correctly)
            Assert.AreEqual("ReturnsRenderView", result.ViewName);
            Assert.AreSame(viewItem, result.ViewData.Model);
        }

        [TestMethod]
        public void InvokeActionMethodWithFiltersWithNullFilterListThrows() {
            // Setup
            IController controller = new Mock<IController>().Object;
            ControllerContext context = GetControllerContext(controller);
            ControllerActionInvokerHelper helper = new ControllerActionInvokerHelper(context);

            // Execute & verify
            ExceptionHelper.ExpectArgumentNullException(
                delegate {
                    helper.PublicInvokeActionMethodWithFilters(typeof(object).GetMethod("ToString"), new Dictionary<string, object>(), null /* filters */);
                },
                "filters");
        }

        [TestMethod]
        public void InvokeActionMethodWithFiltersWithNullMethodInfoThrows() {
            // Setup
            IController controller = new Mock<IController>().Object;
            ControllerContext context = GetControllerContext(controller);
            ControllerActionInvokerHelper helper = new ControllerActionInvokerHelper(context);

            // Execute & verify
            ExceptionHelper.ExpectArgumentNullException(
                delegate {
                    helper.PublicInvokeActionMethodWithFilters(null /* methodInfo */, null /* parameters */, null /* filters */);
                },
                "methodInfo");
        }

        [TestMethod]
        public void InvokeActionMethodWithFiltersWithNullParametersDictionaryThrows() {
            // Setup
            IController controller = new Mock<IController>().Object;
            ControllerContext context = GetControllerContext(controller);
            ControllerActionInvokerHelper helper = new ControllerActionInvokerHelper(context);

            // Execute & verify
            ExceptionHelper.ExpectArgumentNullException(
                delegate {
                    helper.PublicInvokeActionMethodWithFilters(typeof(object).GetMethod("ToString"), null /* parameters */, null /* filters */);
                },
                "parameters");
        }

        [TestMethod]
        public void InvokeActionMethodWithNullMethodInfoThrows() {
            // Setup
            var controller = new BasicMethodInvokeController();
            ControllerContext context = GetControllerContext(controller);
            ControllerActionInvokerHelper helper = new ControllerActionInvokerHelper(context);

            // Execute & verify
            ExceptionHelper.ExpectArgumentNullException(
                delegate {
                    helper.PublicInvokeActionMethod(null /* methodInfo */, null);
                },
                "methodInfo");
        }

        [TestMethod]
        public void InvokeActionMethodWithNullParametersDictionaryThrows() {
            // Setup
            var controller = new BasicMethodInvokeController();
            ControllerContext context = GetControllerContext(controller);
            ControllerActionInvokerHelper helper = new ControllerActionInvokerHelper(context);
            MethodInfo mi = typeof(BasicMethodInvokeController).GetMethod("ReturnsNull");

            // Execute & verify
            ExceptionHelper.ExpectArgumentNullException(
                delegate {
                    helper.PublicInvokeActionMethod(mi, null /* parameters */);
                },
                "parameters");
        }

        [TestMethod]
        public void InvokeActionMethodWithNullReturnValue() {
            // InvokeActionMethod() should convert null return values to EmptyResult.

            // Setup
            var controller = new BasicMethodInvokeController();
            ControllerContext context = GetControllerContext(controller);
            ControllerActionInvokerHelper helper = new ControllerActionInvokerHelper(context);
            MethodInfo mi = typeof(BasicMethodInvokeController).GetMethod("ReturnsNull");

            // Execute
            ActionResult result = helper.PublicInvokeActionMethod(mi, new Dictionary<string, object>());

            // Verify
            Assert.IsInstanceOfType(result, typeof(EmptyResult));
        }

        [TestMethod]
        public void InvokeActionMethodWithObjectReturnType() {
            // InvokeActionMethod() should call Convert.ToString() on non-ActionResult return values.

            // Setup
            var controller = new BasicMethodInvokeController();
            ControllerContext context = GetControllerContext(controller);
            ControllerActionInvokerHelper helper = new ControllerActionInvokerHelper(context);
            MethodInfo mi = typeof(BasicMethodInvokeController).GetMethod("ReturnsInteger");

            // Execute
            ContentResult result = helper.PublicInvokeActionMethod(mi, new Dictionary<string, object>()) as ContentResult;

            // Verify
            Assert.IsNotNull(result, "Non-ActionResult return values should be converted to ContentResult.");
            Assert.AreEqual("42", result.Content);
            Assert.IsNull(result.ContentEncoding);
            Assert.IsNull(result.ContentType);
        }

        [TestMethod]
        public void InvokeActionMethodWithParametersDictionaryContainingNullableType() {
            // Setup
            var controller = new ParameterTestingController();
            ControllerContext context = GetControllerContext(controller);
            ControllerActionInvokerHelper helper = new ControllerActionInvokerHelper(context);
            MethodInfo mi = typeof(ParameterTestingController).GetMethod("TakesNullableInt");
            Dictionary<string, object> parameters = new Dictionary<string, object>() {
                { "id", null }
            };

            // Execute
            ActionResult result = helper.PublicInvokeActionMethod(mi, parameters);

            // Verify
            Assert.IsTrue(controller.Values.ContainsKey("id"));
            Assert.IsNull(controller.Values["id"]);
        }

        [TestMethod]
        public void InvokeActionMethodWithParametersDictionaryContainingNullValueTypeThrows() {
            // Setup
            var controller = new ParameterTestingController();
            ControllerContext context = GetControllerContext(controller);
            ControllerActionInvokerHelper helper = new ControllerActionInvokerHelper(context);
            MethodInfo mi = typeof(ParameterTestingController).GetMethod("TakesInt");
            Dictionary<string, object> parameters = new Dictionary<string, object>() {
                { "id", null }
            };

            // Execute & verify
            ExceptionHelper.ExpectException<InvalidOperationException>(
                delegate {
                    helper.PublicInvokeActionMethod(mi, parameters);
                },
                "The parameters specified in the dictionary do not match those of the method 'TakesInt'.");
        }

        [TestMethod]
        public void InvokeActionMethodWithParametersDictionaryContainingWrongTypesThrows() {
            // Setup
            var controller = new ParameterTestingController();
            ControllerContext context = GetControllerContext(controller);
            ControllerActionInvokerHelper helper = new ControllerActionInvokerHelper(context);
            MethodInfo mi = typeof(ParameterTestingController).GetMethod("TakesInt");
            Dictionary<string, object> parameters = new Dictionary<string, object>() {
                { "id", new object() }
            };

            // Execute & verify
            ExceptionHelper.ExpectException<InvalidOperationException>(
                delegate {
                    helper.PublicInvokeActionMethod(mi, parameters);
                },
                "The parameters specified in the dictionary do not match those of the method 'TakesInt'.");
        }

        [TestMethod]
        public void InvokeActionMethodWithParametersDictionaryMissingEntriesThrows() {
            // Setup
            var controller = new ParameterTestingController();
            ControllerContext context = GetControllerContext(controller);
            ControllerActionInvokerHelper helper = new ControllerActionInvokerHelper(context);
            MethodInfo mi = typeof(ParameterTestingController).GetMethod("TakesInt");
            Dictionary<string, object> parameters = new Dictionary<string, object>() {
                { "foo", "bar" }
            };

            // Execute & verify
            ExceptionHelper.ExpectException<InvalidOperationException>(
                delegate {
                    helper.PublicInvokeActionMethod(mi, parameters);
                },
                "The parameters specified in the dictionary do not match those of the method 'TakesInt'.");
        }

        [TestMethod]
        public void InvokeActionMethodWithParametersDictionaryWrongLengthThrows() {
            // Setup
            var controller = new BasicMethodInvokeController();
            ControllerContext context = GetControllerContext(controller);
            ControllerActionInvokerHelper helper = new ControllerActionInvokerHelper(context);
            MethodInfo mi = typeof(BasicMethodInvokeController).GetMethod("ReturnsNull");
            Dictionary<string, object> parameters = new Dictionary<string, object>() {
                { "foo", "bar" }
            };

            // Execute & verify
            ExceptionHelper.ExpectException<InvalidOperationException>(
                delegate {
                    helper.PublicInvokeActionMethod(mi, parameters);
                },
                "The parameters specified in the dictionary do not match those of the method 'ReturnsNull'.");
        }

        [TestMethod]
        public void InvokeActionMethodWithVoidReturnType() {
            // InvokeActionMethod() should return an EmptyResult for void action methods.

            // Setup
            var controller = new BasicMethodInvokeController();
            ControllerContext context = GetControllerContext(controller);
            ControllerActionInvokerHelper helper = new ControllerActionInvokerHelper(context);
            MethodInfo mi = typeof(BasicMethodInvokeController).GetMethod("VoidMethod");

            // Execute
            ActionResult result = helper.PublicInvokeActionMethod(mi, new Dictionary<string, object>());

            // Verify
            Assert.IsInstanceOfType(result, typeof(EmptyResult));
        }

        [TestMethod]
        public void InvokeActionResultWithFiltersPassesSameContextObjectToInnerFilters() {
            // Setup
            ResultExecutingContext storedContext = null;
            ActionResult result = new EmptyResult();
            List<IResultFilter> filters = new List<IResultFilter>() {
                new ActionFilterImpl() {
                    OnResultExecutingImpl = delegate(ResultExecutingContext ctx) {
                        storedContext = ctx;
                    },
                    OnResultExecutedImpl = delegate { }
                },
                new ActionFilterImpl() {
                    OnResultExecutingImpl = delegate(ResultExecutingContext ctx) {
                        Assert.AreSame(storedContext, ctx);
                    },
                    OnResultExecutedImpl = delegate { }
                },
            };
            ControllerActionInvokerHelper helper = new ControllerActionInvokerHelper(GetControllerContext(new Mock<IController>().Object));

            // Execute
            ResultExecutedContext postContext = helper.PublicInvokeActionResultWithFilters(result, filters);

            // Verify
            Assert.AreSame(result, postContext.Result);
        }

        [TestMethod]
        public void InvokeActionResultWithFiltersTracksChangesToActionResult() {
            // Setup
            ActionResult newResult = new EmptyResult();
            List<IResultFilter> filters = new List<IResultFilter>() {
                new ActionFilterImpl() {
                    OnResultExecutingImpl = delegate { },
                    OnResultExecutedImpl = delegate(ResultExecutedContext ctx) {
                        Assert.AreSame(newResult, ctx.Result);
                    }
                },
                new ActionFilterImpl() {
                    OnResultExecutingImpl = delegate(ResultExecutingContext ctx) {
                        ctx.Result = newResult;
                    },
                    OnResultExecutedImpl = delegate(ResultExecutedContext ctx) {
                        Assert.AreSame(newResult, ctx.Result);
                    }
                },
            };
            ControllerActionInvokerHelper helper = new ControllerActionInvokerHelper(GetControllerContext(new Mock<IController>().Object));

            // Execute
            ResultExecutedContext postContext = helper.PublicInvokeActionResultWithFilters(new EmptyResult(), filters);

            // Verify
            Assert.AreSame(newResult, postContext.Result);
        }

        [TestMethod]
        public void InvokeActionResultWithFiltersTracksChangesToActionResultWithException() {
            // Setup
            ActionResult newResult = new EmptyResult();
            List<IResultFilter> filters = new List<IResultFilter>() {
                new ActionFilterImpl() {
                    OnResultExecutingImpl = delegate { },
                    OnResultExecutedImpl = delegate(ResultExecutedContext ctx) {
                        Assert.AreSame(newResult, ctx.Result);
                        ctx.ExceptionHandled = true;
                    }
                },
                new ActionFilterImpl() {
                    OnResultExecutingImpl = delegate (ResultExecutingContext ctx) {
                        ctx.Result = newResult;
                        throw new Exception();
                    }
                },
            };
            ControllerActionInvokerHelper helper = new ControllerActionInvokerHelper(GetControllerContext(new Mock<IController>().Object));

            // Execute
            ResultExecutedContext postContext = helper.PublicInvokeActionResultWithFilters(new EmptyResult(), filters);

            // Verify
            Assert.AreSame(newResult, postContext.Result);
        }

        [TestMethod]
        public void InvokeActionResultWithFiltersTracksChangesToActionResultWithThreadAbortException() {
            // Setup
            ActionResult newResult = new EmptyResult();
            List<IResultFilter> filters = new List<IResultFilter>() {
                new ActionFilterImpl() {
                    OnResultExecutingImpl = delegate { },
                    OnResultExecutedImpl = delegate(ResultExecutedContext ctx) {
                        Thread.ResetAbort();
                        Assert.AreSame(newResult, ctx.Result);
                        ctx.ExceptionHandled = true;
                    }
                },
                new ActionFilterImpl() {
                    OnResultExecutingImpl = delegate (ResultExecutingContext ctx) {
                        ctx.Result = newResult;
                        Thread.CurrentThread.Abort();
                    }
                },
            };
            ControllerActionInvokerHelper helper = new ControllerActionInvokerHelper(GetControllerContext(new Mock<IController>().Object));

            // Execute & verify
            ExceptionHelper.ExpectException<ThreadAbortException>(
                delegate {
                    helper.PublicInvokeActionResultWithFilters(new EmptyResult(), filters);
                },
                "Thread was being aborted.");
        }

        [TestMethod]
        public void InvokeActionResultWithFiltersWithNullActionResultThrows() {
            // Setup
            IController controller = new Mock<IController>().Object;
            ControllerContext context = GetControllerContext(controller);
            ControllerActionInvokerHelper helper = new ControllerActionInvokerHelper(context);

            // Execute & verify
            ExceptionHelper.ExpectArgumentNullException(
                delegate {
                    helper.PublicInvokeActionResultWithFilters(null /* actionResult */, null /* filters */);
                },
                "actionResult");
        }

        [TestMethod]
        public void InvokeActionResultWithFiltersWithNullFilterListThrows() {
            // Setup
            IController controller = new Mock<IController>().Object;
            ControllerContext context = GetControllerContext(controller);
            ControllerActionInvokerHelper helper = new ControllerActionInvokerHelper(context);

            // Execute & verify
            ExceptionHelper.ExpectArgumentNullException(
                delegate {
                    helper.PublicInvokeActionResultWithFilters(new EmptyResult(), null /* filters */);
                },
                "filters");
        }

        [TestMethod]
        public void InvokeActionResultWithNullActionResultThrows() {
            // Setup
            IController controller = new Mock<IController>().Object;
            ControllerContext context = GetControllerContext(controller);
            ControllerActionInvokerHelper helper = new ControllerActionInvokerHelper(context);

            // Execute & verify
            ExceptionHelper.ExpectArgumentNullException(
                delegate {
                    helper.PublicInvokeActionResult(null /* actionResult */);
                },
                "actionResult");
        }

        [TestMethod]
        public void InvokeActionReturnsFalseIfMethodNotFound() {
            // Setup
            var controller = new BlankController();
            ControllerContext context = GetControllerContext(controller);
            ControllerActionInvoker invoker = new ControllerActionInvoker(context);

            // Execute
            bool retVal = invoker.InvokeAction("foo", null /* values */);

            // Verify
            Assert.IsFalse(retVal);
        }

        [TestMethod]
        public void InvokeActionWithEmptyActionNameThrows() {
            // Setup
            var controller = new BasicMethodInvokeController();
            ControllerContext context = GetControllerContext(controller);
            ControllerActionInvoker invoker = new ControllerActionInvoker(context);

            // Execute & verify
            ExceptionHelper.ExpectArgumentExceptionNullOrEmpty(
                delegate {
                    invoker.InvokeAction(String.Empty, null /* values */);
                },
                "actionName");
        }

        [TestMethod]
        public void InvokeActionWithNullActionNameThrows() {
            // Setup
            var controller = new BasicMethodInvokeController();
            ControllerContext context = GetControllerContext(controller);
            ControllerActionInvoker invoker = new ControllerActionInvoker(context);

            // Execute & verify
            ExceptionHelper.ExpectArgumentExceptionNullOrEmpty(
                delegate {
                    invoker.InvokeAction(null /* actionName */, null /* values */);
                },
                "actionName");
        }

        [TestMethod]
        public void InvokeActionWithResultExceptionInvokesExceptionFiltersAndExecutesResultIfExceptionHandled() {
            // Setup
            IController controller = new Mock<IController>().Object;
            ControllerContext context = GetControllerContext(controller);
            IDictionary<string, object> values = new Dictionary<string, object>();
            IDictionary<string, object> paramValues = new Dictionary<string, object>();
            MethodInfo methodInfo = typeof(object).GetMethod("ToString");
            var filterInfo = new FilterInfo();
            Exception exception = new Exception();
            ActionResult actionResult = new EmptyResult();
            ActionExecutedContext postContext = new ActionExecutedContext(context, methodInfo, false /* canceled */, null /* exception */) {
                Result = actionResult
            };
            ExceptionContext exContext = new ExceptionContext(context, exception) {
                ExceptionHandled = true,
                Result = actionResult
            };
            AuthorizationContext authContext = new AuthorizationContext(context, methodInfo);

            Mock<ControllerActionInvokerHelper> mockHelper = new Mock<ControllerActionInvokerHelper>(context);
            mockHelper.Expect(h => h.PublicFindActionMethod("SomeMethod", values)).Returns(methodInfo).Verifiable();
            mockHelper.Expect(h => h.PublicGetParameterValues(methodInfo, values)).Returns(paramValues).Verifiable();
            mockHelper.Expect(h => h.PublicGetFiltersForActionMethod(methodInfo)).Returns(filterInfo).Verifiable();
            mockHelper.Expect(h => h.PublicInvokeAuthorizationFilters(methodInfo, filterInfo.AuthorizationFilters)).Returns(authContext).Verifiable();
            mockHelper.Expect(h => h.PublicInvokeActionMethodWithFilters(methodInfo, paramValues, filterInfo.ActionFilters)).Returns(postContext).Verifiable();
            mockHelper.Expect(h => h.PublicInvokeActionResultWithFilters(actionResult, filterInfo.ResultFilters)).Throws(exception).Verifiable();
            mockHelper.Expect(h => h.PublicInvokeExceptionFilters(exception, filterInfo.ExceptionFilters)).Returns(exContext).Verifiable();
            mockHelper.Expect(h => h.PublicInvokeActionResult(actionResult)).Verifiable();
            ControllerActionInvokerHelper helper = mockHelper.Object;

            // Execute
            bool retVal = helper.InvokeAction("SomeMethod", values);
            Assert.IsTrue(retVal, "InvokeAction() should return True on success.");
            mockHelper.Verify();
        }

        [TestMethod]
        public void InvokeAuthorizationFilters() {
            // Setup
            IController controller = new Mock<IController>().Object;
            MethodInfo methodInfo = typeof(object).GetMethod("ToString");
            ControllerContext controllerContext = GetControllerContext(controller);
            ControllerActionInvokerHelper helper = new ControllerActionInvokerHelper(controllerContext);

            List<AuthorizationFilterHelper> callQueue = new List<AuthorizationFilterHelper>();
            AuthorizationFilterHelper filter1 = new AuthorizationFilterHelper(callQueue);
            AuthorizationFilterHelper filter2 = new AuthorizationFilterHelper(callQueue);

            // Execute
            AuthorizationContext postContext = helper.PublicInvokeAuthorizationFilters(methodInfo, new List<IAuthorizationFilter> { filter1, filter2 });

            // Verify
            Assert.AreSame(methodInfo, postContext.ActionMethod);
            Assert.AreEqual(2, callQueue.Count);
            Assert.AreSame(filter1, callQueue[0]);
            Assert.AreSame(filter2, callQueue[1]);
        }

        [TestMethod]
        public void InvokeAuthorizationFiltersStopsExecutingIfResultProvided() {
            // Setup
            IController controller = new Mock<IController>().Object;
            MethodInfo methodInfo = typeof(object).GetMethod("ToString");
            ControllerContext controllerContext = GetControllerContext(controller);
            ControllerActionInvokerHelper helper = new ControllerActionInvokerHelper(controllerContext);
            ActionResult result = new EmptyResult();

            List<AuthorizationFilterHelper> callQueue = new List<AuthorizationFilterHelper>();
            AuthorizationFilterHelper filter1 = new AuthorizationFilterHelper(callQueue) { ShouldCancel = true, ShortCircuitResult = result };
            AuthorizationFilterHelper filter2 = new AuthorizationFilterHelper(callQueue);

            // Execute
            AuthorizationContext postContext = helper.PublicInvokeAuthorizationFilters(methodInfo, new List<IAuthorizationFilter> { filter1, filter2 });

            // Verify
            Assert.AreSame(methodInfo, postContext.ActionMethod);
            Assert.IsTrue(postContext.Cancel);
            Assert.AreSame(result, postContext.Result);
            Assert.AreEqual(1, callQueue.Count);
            Assert.AreSame(filter1, callQueue[0]);
        }

        [TestMethod]
        public void InvokeAuthorizationFiltersWithNullFiltersThrows() {
            // Setup
            IController controller = new Mock<IController>().Object;
            ControllerContext context = GetControllerContext(controller);
            ControllerActionInvokerHelper helper = new ControllerActionInvokerHelper(context);

            // Execute & verify
            ExceptionHelper.ExpectArgumentNullException(
                delegate {
                    helper.PublicInvokeAuthorizationFilters(typeof(object).GetMethod("ToString"), null /* filters */);
                }, "filters");
        }

        [TestMethod]
        public void InvokeExceptionFiltersWithNullMethodInfoThrows() {
            // Setup
            IController controller = new Mock<IController>().Object;
            ControllerContext context = GetControllerContext(controller);
            ControllerActionInvokerHelper helper = new ControllerActionInvokerHelper(context);

            // Execute & verify
            ExceptionHelper.ExpectArgumentNullException(
                delegate {
                    helper.PublicInvokeAuthorizationFilters(null /* methodInfo */, null /* filters */);
                }, "methodInfo");
        }

        [TestMethod]
        public void InvokeExceptionFilters() {
            // Setup
            IController controller = new Mock<IController>().Object;
            Exception exception = new Exception();
            ControllerContext controllerContext = GetControllerContext(controller);
            ControllerActionInvokerHelper helper = new ControllerActionInvokerHelper(controllerContext);

            List<ExceptionFilterHelper> callQueue = new List<ExceptionFilterHelper>();
            ExceptionFilterHelper filter1 = new ExceptionFilterHelper(callQueue);
            ExceptionFilterHelper filter2 = new ExceptionFilterHelper(callQueue);

            // Execute
            ExceptionContext postContext = helper.PublicInvokeExceptionFilters(exception, new List<IExceptionFilter> { filter1, filter2 });

            // Verify
            Assert.AreSame(exception, postContext.Exception);
            Assert.IsFalse(postContext.ExceptionHandled);
            Assert.AreSame(filter1.ContextPassed, filter2.ContextPassed, "The same context should have been passed to each exception filter.");
            Assert.AreEqual(2, callQueue.Count);
            Assert.AreSame(filter1, callQueue[0]);
            Assert.AreSame(filter2, callQueue[1]);
        }

        [TestMethod]
        public void InvokeExceptionFiltersContinuesExecutingIfExceptionHandled() {
            // Setup
            IController controller = new Mock<IController>().Object;
            Exception exception = new Exception();
            ControllerContext controllerContext = GetControllerContext(controller);
            ControllerActionInvokerHelper helper = new ControllerActionInvokerHelper(controllerContext);

            List<ExceptionFilterHelper> callQueue = new List<ExceptionFilterHelper>();
            ExceptionFilterHelper filter1 = new ExceptionFilterHelper(callQueue) { ShouldHandleException = true };
            ExceptionFilterHelper filter2 = new ExceptionFilterHelper(callQueue);

            // Execute
            ExceptionContext postContext = helper.PublicInvokeExceptionFilters(exception, new List<IExceptionFilter> { filter1, filter2 });

            // Verify
            Assert.AreSame(exception, postContext.Exception);
            Assert.IsTrue(postContext.ExceptionHandled, "The exception should have been handled.");
            Assert.AreSame(filter1.ContextPassed, filter2.ContextPassed, "The same context should have been passed to each exception filter.");
            Assert.AreEqual(2, callQueue.Count);
            Assert.AreSame(filter1, callQueue[0]);
            Assert.AreSame(filter2, callQueue[1]);
        }

        [TestMethod]
        public void InvokeExceptionFiltersWithNullExceptionThrows() {
            // Setup
            IController controller = new Mock<IController>().Object;
            ControllerContext context = GetControllerContext(controller);
            ControllerActionInvokerHelper helper = new ControllerActionInvokerHelper(context);

            // Execute & verify
            ExceptionHelper.ExpectArgumentNullException(
                delegate {
                    helper.PublicInvokeExceptionFilters(null /* exception */, null /* filters */);
                }, "exception");
        }

        [TestMethod]
        public void InvokeExceptionFiltersWithNullFiltersThrows() {
            // Setup
            IController controller = new Mock<IController>().Object;
            ControllerContext context = GetControllerContext(controller);
            ControllerActionInvokerHelper helper = new ControllerActionInvokerHelper(context);

            // Execute & verify
            ExceptionHelper.ExpectArgumentNullException(
                delegate {
                    helper.PublicInvokeExceptionFilters(new Exception(), null /* filters */);
                }, "filters");
        }

        [TestMethod]
        public void InvokeResultFiltersOrdersFiltersCorrectly() {
            // Setup
            List<string> actions = new List<string>();
            ActionFilterImpl filter1 = new ActionFilterImpl() {
                OnResultExecutingImpl = delegate(ResultExecutingContext filterContext) {
                    actions.Add("OnResultExecuting1");
                },
                OnResultExecutedImpl = delegate(ResultExecutedContext filterContext) {
                    actions.Add("OnResultExecuted1");
                }
            };
            ActionFilterImpl filter2 = new ActionFilterImpl() {
                OnResultExecutingImpl = delegate(ResultExecutingContext filterContext) {
                    actions.Add("OnResultExecuting2");
                },
                OnResultExecutedImpl = delegate(ResultExecutedContext filterContext) {
                    actions.Add("OnResultExecuted2");
                }
            };
            Action continuation = delegate {
                actions.Add("Continuation");
            };
            ActionResult actionResult = new ContinuationResult(continuation);
            IController controller = new Mock<IController>().Object;
            ControllerContext context = GetControllerContext(controller);
            ControllerActionInvokerHelper helper = new ControllerActionInvokerHelper(context);

            // Execute
            helper.PublicInvokeActionResultWithFilters(actionResult, new List<IResultFilter>() { filter1, filter2 });

            // Verify
            Assert.AreEqual(5, actions.Count);
            Assert.AreEqual("OnResultExecuting1", actions[0]);
            Assert.AreEqual("OnResultExecuting2", actions[1]);
            Assert.AreEqual("Continuation", actions[2]);
            Assert.AreEqual("OnResultExecuted2", actions[3]);
            Assert.AreEqual("OnResultExecuted1", actions[4]);
        }

        [TestMethod]
        public void InvokeResultFiltersPassesArgumentsCorrectly() {
            // Setup
            bool wasCalled = false;
            Action continuation = delegate {
                Assert.Fail("Continuation should not be called.");
            };
            ActionResult actionResult = new ContinuationResult(continuation);
            IController controller = new Mock<IController>().Object;
            ControllerContext context = GetControllerContext(controller);
            ControllerActionInvokerHelper helper = new ControllerActionInvokerHelper(context);
            ActionFilterImpl filter = new ActionFilterImpl() {
                OnResultExecutingImpl = delegate(ResultExecutingContext filterContext) {
                    Assert.AreSame(actionResult, filterContext.Result);
                    Assert.IsFalse(wasCalled);
                    wasCalled = true;
                    filterContext.Cancel = true;
                }
            };

            // Execute
            ResultExecutedContext result = helper.PublicInvokeActionResultWithFilters(actionResult,
                new List<IResultFilter>() { filter });

            // Verify
            Assert.IsTrue(wasCalled);
            Assert.IsNull(result.Exception);
            Assert.IsFalse(result.ExceptionHandled);
            Assert.AreSame(actionResult, result.Result);
        }

        [TestMethod]
        public void InvokeResultFilterWhereContinuationThrowsExceptionAndIsHandled() {
            // Setup
            List<string> actions = new List<string>();
            ActionResult actionResult = new EmptyResult();
            Exception exception = new Exception();
            ActionFilterImpl filter = new ActionFilterImpl() {
                OnResultExecutingImpl = delegate(ResultExecutingContext filterContext) {
                    actions.Add("OnResultExecuting");
                },
                OnResultExecutedImpl = delegate(ResultExecutedContext filterContext) {
                    actions.Add("OnResultExecuted");
                    Assert.AreSame(actionResult, filterContext.Result);
                    Assert.AreSame(exception, filterContext.Exception);
                    Assert.IsFalse(filterContext.ExceptionHandled);
                    filterContext.ExceptionHandled = true;
                }
            };
            Func<ResultExecutedContext> continuation = delegate {
                actions.Add("Continuation");
                throw exception;
            };

            // Execute
            ResultExecutedContext result = ControllerActionInvoker.InvokeActionResultFilter(filter, new ResultExecutingContext(ControllerContextTest.GetControllerContext(), actionResult), continuation);

            // Verify
            Assert.AreEqual(3, actions.Count);
            Assert.AreEqual("OnResultExecuting", actions[0]);
            Assert.AreEqual("Continuation", actions[1]);
            Assert.AreEqual("OnResultExecuted", actions[2]);
            Assert.AreSame(exception, result.Exception);
            Assert.IsTrue(result.ExceptionHandled);
            Assert.AreSame(actionResult, result.Result);
        }

        [TestMethod]
        public void InvokeResultFilterWhereContinuationThrowsExceptionAndIsNotHandled() {
            // Setup
            List<string> actions = new List<string>();
            ActionResult actionResult = new EmptyResult();
            ActionFilterImpl filter = new ActionFilterImpl() {
                OnResultExecutingImpl = delegate(ResultExecutingContext filterContext) {
                    actions.Add("OnResultExecuting");
                },
                OnResultExecutedImpl = delegate(ResultExecutedContext filterContext) {
                    actions.Add("OnResultExecuted");
                }
            };
            Func<ResultExecutedContext> continuation = delegate {
                actions.Add("Continuation");
                throw new Exception("Some exception message.");
            };

            // Execute & verify
            ExceptionHelper.ExpectException<Exception>(
                delegate {
                    ControllerActionInvoker.InvokeActionResultFilter(filter, new ResultExecutingContext(ControllerContextTest.GetControllerContext(), actionResult), continuation);
                },
               "Some exception message.");
            Assert.AreEqual(3, actions.Count);
            Assert.AreEqual("OnResultExecuting", actions[0]);
            Assert.AreEqual("Continuation", actions[1]);
            Assert.AreEqual("OnResultExecuted", actions[2]);
        }

        [TestMethod]
        public void InvokeResultFilterWhereContinuationThrowsThreadAbortException() {
            // Setup
            List<string> actions = new List<string>();
            ActionResult actionResult = new EmptyResult();
            ActionFilterImpl filter = new ActionFilterImpl() {
                OnResultExecutingImpl = delegate(ResultExecutingContext filterContext) {
                    actions.Add("OnResultExecuting");
                },
                OnResultExecutedImpl = delegate(ResultExecutedContext filterContext) {
                    Thread.ResetAbort();
                    actions.Add("OnResultExecuted");
                    Assert.AreSame(actionResult, filterContext.Result);
                    Assert.IsNull(filterContext.Exception);
                    Assert.IsFalse(filterContext.ExceptionHandled);
                }
            };
            Func<ResultExecutedContext> continuation = delegate {
                actions.Add("Continuation");
                Thread.CurrentThread.Abort();
                return null;
            };

            // Execute & verify
            ExceptionHelper.ExpectException<ThreadAbortException>(
                delegate {
                    ControllerActionInvoker.InvokeActionResultFilter(filter, new ResultExecutingContext(ControllerContextTest.GetControllerContext(), actionResult), continuation);
                },
                "Thread was being aborted.");
            Assert.AreEqual(3, actions.Count);
            Assert.AreEqual("OnResultExecuting", actions[0]);
            Assert.AreEqual("Continuation", actions[1]);
            Assert.AreEqual("OnResultExecuted", actions[2]);
        }

        [TestMethod]
        public void InvokeResultFilterWhereOnResultExecutingCancels() {
            // Setup
            bool wasCalled = false;
            MethodInfo mi = typeof(object).GetMethod("ToString");
            object[] paramValues = new object[0];
            ActionResult actionResult = new EmptyResult();
            ActionFilterImpl filter = new ActionFilterImpl() {
                OnResultExecutingImpl = delegate(ResultExecutingContext filterContext) {
                    Assert.IsFalse(wasCalled);
                    wasCalled = true;
                    filterContext.Cancel = true;
                },
            };
            Func<ResultExecutedContext> continuation = delegate {
                Assert.Fail("The continuation should not be called.");
                return null;
            };

            // Execute
            ResultExecutedContext result = ControllerActionInvoker.InvokeActionResultFilter(filter, new ResultExecutingContext(ControllerContextTest.GetControllerContext(), actionResult), continuation);

            // Verify
            Assert.IsTrue(wasCalled);
            Assert.IsNull(result.Exception);
            Assert.IsTrue(result.Canceled);
            Assert.AreSame(actionResult, result.Result);
        }

        [TestMethod]
        public void InvokeResultFilterWithNormalControlFlow() {
            // Setup
            List<string> actions = new List<string>();
            ActionResult actionResult = new EmptyResult();
            ResultExecutedContext postContext = new ResultExecutedContext(ControllerContextTest.GetControllerContext(), actionResult, false /* canceled */, null /* exception */);
            ActionFilterImpl filter = new ActionFilterImpl() {
                OnResultExecutingImpl = delegate(ResultExecutingContext filterContext) {
                    Assert.AreSame(actionResult, filterContext.Result);
                    Assert.IsFalse(filterContext.Cancel);
                    actions.Add("OnResultExecuting");
                },
                OnResultExecutedImpl = delegate(ResultExecutedContext filterContext) {
                    Assert.AreEqual(postContext, filterContext);
                    actions.Add("OnResultExecuted");
                }
            };
            Func<ResultExecutedContext> continuation = delegate {
                actions.Add("Continuation");
                return postContext;
            };

            // Execute
            ResultExecutedContext result = ControllerActionInvoker.InvokeActionResultFilter(filter, new ResultExecutingContext(ControllerContextTest.GetControllerContext(), actionResult), continuation);

            // Verify
            Assert.AreEqual(3, actions.Count);
            Assert.AreEqual("OnResultExecuting", actions[0]);
            Assert.AreEqual("Continuation", actions[1]);
            Assert.AreEqual("OnResultExecuted", actions[2]);
            Assert.AreSame(result, postContext);
        }

        private static ControllerContext GetControllerContext(IController controller) {
            Mock<HttpContextBase> contextMock = new Mock<HttpContextBase>();
            contextMock.Expect(o => o.Request).Returns((HttpRequestBase)null); // make the TempDataDictionary happy
            contextMock.Expect(o => o.Session).Returns((HttpSessionStateBase)null);
            return new ControllerContext(contextMock.Object, new RouteData(), controller);
        }

        private class EmptyActionFilterAttribute : ActionFilterAttribute {
        }

        private abstract class KeyedFilterAttribute : FilterAttribute {
            public string Key {
                get;
                set;
            }
        }

        private class KeyedAuthorizationFilterAttribute : KeyedFilterAttribute, IAuthorizationFilter {
            public void OnAuthorization(AuthorizationContext filterContext) {
                throw new NotImplementedException();
            }
        }

        [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = true)]
        private class KeyedActionFilterAttribute : KeyedFilterAttribute, IActionFilter, IResultFilter {
            public void OnActionExecuting(ActionExecutingContext filterContext) {
                throw new NotImplementedException();
            }
            public void OnActionExecuted(ActionExecutedContext filterContext) {
                throw new NotImplementedException();
            }
            public void OnResultExecuting(ResultExecutingContext filterContext) {
                throw new NotImplementedException();
            }

            public void OnResultExecuted(ResultExecutedContext filterContext) {
                throw new NotImplementedException();
            }
        }

        private class ActionFilterImpl : IActionFilter, IResultFilter {

            public Action<ActionExecutingContext> OnActionExecutingImpl {
                get;
                set;
            }

            public void OnActionExecuting(ActionExecutingContext filterContext) {
                OnActionExecutingImpl(filterContext);
            }

            public Action<ActionExecutedContext> OnActionExecutedImpl {
                get;
                set;
            }

            public void OnActionExecuted(ActionExecutedContext filterContext) {
                OnActionExecutedImpl(filterContext);
            }

            public Action<ResultExecutingContext> OnResultExecutingImpl {
                get;
                set;
            }

            public void OnResultExecuting(ResultExecutingContext filterContext) {
                OnResultExecutingImpl(filterContext);
            }

            public Action<ResultExecutedContext> OnResultExecutedImpl {
                get;
                set;
            }

            public void OnResultExecuted(ResultExecutedContext filterContext) {
                OnResultExecutedImpl(filterContext);
            }

        }

        [KeyedActionFilter(Key = "BaseClass", Order = 0)]
        [KeyedAuthorizationFilter(Key = "BaseClass", Order = 0)]
        private class GetMemberChainController : Controller {

            [KeyedActionFilter(Key = "BaseMethod", Order = 0)]
            [KeyedAuthorizationFilter(Key = "BaseMethod", Order = 0)]
            public virtual void SomeVirtual() {
            }

        }

        [KeyedActionFilter(Key = "DerivedClass", Order = 1)]
        private class GetMemberChainDerivedController : GetMemberChainController {

        }

        [KeyedActionFilter(Key = "SubderivedClass", Order = 2)]
        private class GetMemberChainSubderivedController : GetMemberChainDerivedController {

            [KeyedActionFilter(Key = "SubderivedMethod", Order = 2)]
            public override void SomeVirtual() {
            }

        }

        // This controller serves only to test vanilla method invocation - nothing exciting here
        private class BasicMethodInvokeController : Controller {
            public ActionResult ReturnsRenderView(object viewItem) {
                return View("ReturnsRenderView", viewItem);
            }
            public void VoidMethod() {
            }
            public object ReturnsNull() {
                return null;
            }
            public int ReturnsInteger() {
                return 42;
            }
        }

        private class BlankController : Controller {
        }

        private class ContinuationController : Controller {

            private Func<ActionResult> _continuation;

            public ContinuationController(Func<ActionResult> continuation) {
                _continuation = continuation;
            }

            public ActionResult Go() {
                return _continuation();
            }

            public static MethodInfo GoMethod {
                get {
                    return typeof(ContinuationController).GetMethod("Go");
                }
            }

        }

        private class ContinuationResult : ActionResult {

            private Action _continuation;

            public ContinuationResult(Action continuation) {
                _continuation = continuation;
            }

            public override void ExecuteResult(ControllerContext context) {
                _continuation();
            }

        }

        // This controller serves to test the default action method matching mechanism
        private class FindMethodController : Controller {

            public ActionResult ValidActionMethod() {
                return null;
            }

            [NonAction]
            public virtual ActionResult NonActionMethod() {
                return null;
            }

            [NonAction]
            public ActionResult DerivedIsActionMethod() {
                return null;
            }

            public ActionResult MethodOverloaded() {
                return null;
            }

            public ActionResult MethodOverloaded(string s) {
                return null;
            }

            public void WrongReturnType() {
            }

            protected ActionResult ProtectedMethod() {
                return null;
            }

            private ActionResult PrivateMethod() {
                return null;
            }

            internal ActionResult InternalMethod() {
                return null;
            }

            public override string ToString() {
                // originally defined on Object
                return base.ToString();
            }

            public ActionResult Property {
                get {
                    return null;
                }
            }

#pragma warning disable 0067
            // CS0067: Event declared but never used. We use reflection to access this member.
            public event EventHandler Event;
#pragma warning restore 0067

        }

        private class DerivedFindMethodController : FindMethodController {

            public override ActionResult NonActionMethod() {
                return base.NonActionMethod();
            }

            // FindActionMethod() should accept this as a valid method since [NonAction] doesn't appear
            // in its inheritance chain.
            public new ActionResult DerivedIsActionMethod() {
                return base.DerivedIsActionMethod();
            }

        }

        // Similar to FindMethodController, but tests generics support specifically
        private class GenericFindMethodController<T> : Controller {

            public ActionResult ClosedGenericMethod(T t) {
                return null;
            }

            public ActionResult OpenGenericMethod<U>(U t) {
                return null;
            }

        }

        // Allows for testing parameter conversions, etc.
        private class ParameterTestingController : Controller {

            public ParameterTestingController() {
                Values = new Dictionary<string, object>();
            }

            public IDictionary<string, object> Values {
                get;
                private set;
            }

            public void Foo(string foo, string bar, string baz) {
                Values["foo"] = foo;
                Values["bar"] = bar;
                Values["baz"] = baz;
            }

            public void HasOutParam(out string foo) {
                foo = null;
            }

            public void HasRefParam(ref string foo) {
            }

            public void Parameterless() {
            }

            public void TakesInt(int id) {
                Values["id"] = id;
            }

            public ActionResult TakesNullableInt(int? id) {
                Values["id"] = id;
                return null;
            }

            public void TakesString(string id) {
            }

            public void TakesDateTime(DateTime id) {
            }

            public void CustomClassConverter(MyParameterClass mp) {
            }

            public void CustomStructConverter(MyParameterStruct mp) {
            }

            public void CustomStructConverterFromString(MyParameterStructBad mp) {
            }

            public void CustomNullableStructConverter(MyParameterStruct? mp) {
            }

            public void CustomNullableStructConverterFromString(MyParameterStructBad? mp) {
            }

        }

        // Custom class with custom type converter
        [TypeConverter(typeof(MyParameterClassConverter))]
        private class MyParameterClass {
            public int Number { get; set; }
            public char Char { get; set; }

            public override bool Equals(object obj) {
                MyParameterClass other = obj as MyParameterClass;
                if (other == null) {
                    return false;
                }
                return other.Char == Char && other.Number == Number;
            }

            public override int GetHashCode() {
                return Char.GetHashCode() ^ Number.GetHashCode();
            }
        }

        // Custom struct with custom type converter
        [TypeConverter(typeof(MyParameterStructConverter))]
        private struct MyParameterStruct {
            public int Number { get; set; }
            public char Char { get; set; }
        }

        // Custom struct with custom type converter that won't convert from strings
        [TypeConverter(typeof(MyParameterStructBadConverter))]
        private struct MyParameterStructBad {
            public int Number { get; set; }
            public char Char { get; set; }
        }

        private class MyParameterClassConverter : TypeConverter {
            public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType) {
                if (sourceType == typeof(string)) {
                    return true;
                }
                return base.CanConvertFrom(context, sourceType);
            }

            public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value) {
                string valueString = value as string;
                if (valueString != null) {
                    if (valueString.Length < 2) {
                        throw new InvalidOperationException("String too short!");
                    }
                    char c = valueString[0];
                    int n = Int32.Parse(valueString.Substring(1));
                    MyParameterClass myParam = new MyParameterClass { Char = c, Number = n };
                    return myParam;
                }
                return base.ConvertFrom(context, culture, value);
            }
        }

        private class MyParameterStructConverter : TypeConverter {
            public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType) {
                if (sourceType == typeof(string)) {
                    return true;
                }
                return base.CanConvertFrom(context, sourceType);
            }

            public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value) {
                string valueString = value as string;
                if (valueString != null) {
                    if (valueString.Length < 2) {
                        throw new InvalidOperationException("String too short!");
                    }
                    char c = valueString[0];
                    int n = Int32.Parse(valueString.Substring(1));
                    MyParameterStruct myParam = new MyParameterStruct { Char = c, Number = n };
                    return myParam;
                }
                return base.ConvertFrom(context, culture, value);
            }
        }

        private class MyParameterStructBadConverter : TypeConverter {
            public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType) {
                return false;
            }
        }

        // Provides access to the protected members of ControllerActionInvoker
        public class ControllerActionInvokerHelper : ControllerActionInvoker {

            public ControllerActionInvokerHelper(ControllerContext context)
                : base(context) {
            }

            protected override MethodInfo FindActionMethod(string actionName, IDictionary<string, object> values) {
                return PublicFindActionMethod(actionName, values);
            }

            public virtual MethodInfo PublicFindActionMethod(string actionName, IDictionary<string, object> values) {
                return base.FindActionMethod(actionName, values);
            }

            protected override FilterInfo GetFiltersForActionMethod(MethodInfo methodInfo) {
                return PublicGetFiltersForActionMethod(methodInfo);
            }

            public virtual FilterInfo PublicGetFiltersForActionMethod(MethodInfo methodInfo) {
                return base.GetFiltersForActionMethod(methodInfo);
            }

            protected override object GetParameterValue(ParameterInfo parameterInfo, IDictionary<string, object> values) {
                return PublicGetParameterValue(parameterInfo, values);
            }

            public virtual object PublicGetParameterValue(ParameterInfo parameterInfo, IDictionary<string, object> values) {
                return base.GetParameterValue(parameterInfo, values);
            }

            protected override IDictionary<string, object> GetParameterValues(MethodInfo methodInfo, IDictionary<string, object> values) {
                return PublicGetParameterValues(methodInfo, values);
            }

            public virtual IDictionary<string, object> PublicGetParameterValues(MethodInfo methodInfo, IDictionary<string, object> values) {
                return base.GetParameterValues(methodInfo, values);
            }

            protected override ActionResult InvokeActionMethod(MethodInfo methodInfo, IDictionary<string, object> parameters) {
                return PublicInvokeActionMethod(methodInfo, parameters);
            }

            public virtual ActionResult PublicInvokeActionMethod(MethodInfo methodInfo, IDictionary<string, object> parameters) {
                return base.InvokeActionMethod(methodInfo, parameters);
            }

            protected override ActionExecutedContext InvokeActionMethodWithFilters(MethodInfo methodInfo, IDictionary<string, object> parameters, IList<IActionFilter> filters) {
                return PublicInvokeActionMethodWithFilters(methodInfo, parameters, filters);
            }

            public virtual ActionExecutedContext PublicInvokeActionMethodWithFilters(MethodInfo methodInfo, IDictionary<string, object> parameters, IList<IActionFilter> filters) {
                return base.InvokeActionMethodWithFilters(methodInfo, parameters, filters);
            }

            protected override void InvokeActionResult(ActionResult actionResult) {
                PublicInvokeActionResult(actionResult);
            }

            public virtual void PublicInvokeActionResult(ActionResult actionResult) {
                base.InvokeActionResult(actionResult);
            }

            protected override ResultExecutedContext InvokeActionResultWithFilters(ActionResult actionResult, IList<IResultFilter> filters) {
                return PublicInvokeActionResultWithFilters(actionResult, filters);
            }

            public virtual ResultExecutedContext PublicInvokeActionResultWithFilters(ActionResult actionResult, IList<IResultFilter> filters) {
                return base.InvokeActionResultWithFilters(actionResult, filters);
            }

            protected override AuthorizationContext InvokeAuthorizationFilters(MethodInfo methodInfo, IList<IAuthorizationFilter> filters) {
                return PublicInvokeAuthorizationFilters(methodInfo, filters);
            }

            public virtual AuthorizationContext PublicInvokeAuthorizationFilters(MethodInfo methodInfo, IList<IAuthorizationFilter> filters) {
                return base.InvokeAuthorizationFilters(methodInfo, filters);
            }

            protected override ExceptionContext InvokeExceptionFilters(Exception exception, IList<IExceptionFilter> filters) {
                return PublicInvokeExceptionFilters(exception, filters);
            }

            public virtual ExceptionContext PublicInvokeExceptionFilters(Exception exception, IList<IExceptionFilter> filters) {
                return base.InvokeExceptionFilters(exception, filters);
            }

            public struct FilterInfoHelper {
                public IList<IActionFilter> ActionFilters;
                public IList<IAuthorizationFilter> AuthorizationFilters;
                public IList<IExceptionFilter> ExceptionFilters;
                public IList<IResultFilter> ResultFilters;
            }

        }

        public class AuthorizationFilterHelper : IAuthorizationFilter {

            private IList<AuthorizationFilterHelper> _callQueue;
            public bool ShouldCancel;
            public ActionResult ShortCircuitResult;

            public AuthorizationFilterHelper(IList<AuthorizationFilterHelper> callQueue) {
                _callQueue = callQueue;
            }

            public void OnAuthorization(AuthorizationContext filterContext) {
                _callQueue.Add(this);
                if (ShouldCancel) {
                    filterContext.Cancel = true;
                    filterContext.Result = ShortCircuitResult;
                }
            }
        }

        public class ExceptionFilterHelper : IExceptionFilter {

            private IList<ExceptionFilterHelper> _callQueue;
            public bool ShouldHandleException;
            public ExceptionContext ContextPassed;

            public ExceptionFilterHelper(IList<ExceptionFilterHelper> callQueue) {
                _callQueue = callQueue;
            }

            public void OnException(ExceptionContext filterContext) {
                _callQueue.Add(this);
                if (ShouldHandleException) {
                    filterContext.ExceptionHandled = true;
                }
                ContextPassed = filterContext;
            }
        }

        public class Person {
        }

        public class Employee : Person {
        }
    }
}
