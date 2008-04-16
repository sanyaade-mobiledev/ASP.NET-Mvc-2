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
        public void FindActionMethodWithWrongReturnTypeThrows() {
            // FindActionMethod() should throw if it matches a method that doesn't return ActionResult.

            // Setup
            var controller = new FindMethodController();
            ControllerContext context = GetControllerContext(controller);
            ControllerActionInvokerHelper helper = new ControllerActionInvokerHelper(context);
            MethodInfo expected = typeof(FindMethodController).GetMethod("WrongReturnType");

            // Execute & verify
            ExceptionHelper.ExpectException<InvalidOperationException>(
                delegate {
                    helper.PublicFindActionMethod("WrongReturnType", null /* values */);
                },
                "The action method 'WrongReturnType' on controller 'System.Web.Mvc.Test.ControllerActionInvokerTest+FindMethodController' "
                    + "has return type 'System.Void'. Action methods must return an ActionResult.");
        }

        [TestMethod]
        public void GetActionFiltersForMemberSortsUnorderedBeforeOrdered() {
            // Setup
            var controller = new ActionFilterOrderingController();
            ControllerContext context = GetControllerContext(controller);
            ControllerActionInvokerHelper helper = new ControllerActionInvokerHelper(context);
            MethodInfo mi = typeof(ActionFilterOrderingController).GetMethod("OrderedAndUnordered");

            // Execute
            var filters = helper.PublicGetActionFiltersForMember(mi);

            // Verify
            Assert.AreEqual(6, filters.Count);
            Assert.AreEqual(-1, ((ActionFilterAttribute)filters[0]).Order);
            Assert.AreEqual(-1, ((ActionFilterAttribute)filters[1]).Order);
            Assert.AreEqual(-1, ((ActionFilterAttribute)filters[2]).Order);
            Assert.AreEqual(1, ((ActionFilterAttribute)filters[3]).Order);
            Assert.AreEqual(5, ((ActionFilterAttribute)filters[4]).Order);
            Assert.AreEqual(10, ((ActionFilterAttribute)filters[5]).Order);
        }

        [TestMethod]
        public void GetActionFiltersForMemberWithConflictingOrdersOnMethodThrows() {
            // Setup
            var controller = new ActionFilterOrderingController();
            ControllerContext context = GetControllerContext(controller);
            ControllerActionInvokerHelper helper = new ControllerActionInvokerHelper(context);
            MethodInfo mi = typeof(ActionFilterOrderingController).GetMethod("ConflictingOrders");

            // Execute & verify
            ExceptionHelper.ExpectException<InvalidOperationException>(
                delegate {
                    helper.PublicGetActionFiltersForMember(mi);
                },
                "The action method 'ConflictingOrders' on controller 'System.Web.Mvc.Test.ControllerActionInvokerTest+"
                    + "ActionFilterOrderingController' has two filter attributes with filter order 0. If a filter "
                    + "specifies an order of 0 or greater, no other filter on that action method may specify that same order.");
        }

        [TestMethod]
        public void GetActionFiltersForMemberWithConflictingOrdersOnTypeThrows() {
            // Setup
            var controller = new ActionFilterOrderingController();
            ControllerContext context = GetControllerContext(controller);
            ControllerActionInvokerHelper helper = new ControllerActionInvokerHelper(context);

            // Execute & verify
            ExceptionHelper.ExpectException<InvalidOperationException>(
                delegate {
                    helper.PublicGetActionFiltersForMember(typeof(ConflictingActionFilterOrderingController));
                },
                "Two filter attributes on controller 'System.Web.Mvc.Test.ControllerActionInvokerTest+"
                    + "ConflictingActionFilterOrderingController' have filter order 0. If a filter specifies an order of 0 "
                    + "or greater, no other filter on that type may specify that same order.");
        }

        [TestMethod]
        public void GetActionFiltersForMemberWithNoFilters() {
            // Setup
            var controller = new ActionFilterOrderingController();
            ControllerContext context = GetControllerContext(controller);
            ControllerActionInvokerHelper helper = new ControllerActionInvokerHelper(context);
            MethodInfo mi = typeof(ActionFilterOrderingController).GetMethod("NoFilters");

            // Execute
            var filters = helper.PublicGetActionFiltersForMember(mi);

            // Verify
            Assert.AreEqual(0, filters.Count);
        }

        [TestMethod]
        public void GetActionFiltersForMemberWithNullMemberInfoThrows() {
            // Setup
            var controller = new ActionFilterOrderingController();
            ControllerContext context = GetControllerContext(controller);
            ControllerActionInvokerHelper helper = new ControllerActionInvokerHelper(context);

            // Execute & verify
            ExceptionHelper.ExpectArgumentNullException(
                delegate {
                    helper.PublicGetActionFiltersForMember(null /* memberInfo */);
                },
                "memberInfo");
        }

        [TestMethod]
        public void GetActionFiltersForMemberWithSingleOrdered() {
            // Setup
            var controller = new ActionFilterOrderingController();
            ControllerContext context = GetControllerContext(controller);
            ControllerActionInvokerHelper helper = new ControllerActionInvokerHelper(context);
            MethodInfo mi = typeof(ActionFilterOrderingController).GetMethod("SingleOrdered");

            // Execute
            var filters = helper.PublicGetActionFiltersForMember(mi);

            // Verify
            Assert.AreEqual(1, filters.Count);
            Assert.AreEqual(10, ((ActionFilterAttribute)filters[0]).Order);
        }

        [TestMethod]
        public void GetActionFiltersForMemberWithSingleUnordered() {
            // Setup
            var controller = new ActionFilterOrderingController();
            ControllerContext context = GetControllerContext(controller);
            ControllerActionInvokerHelper helper = new ControllerActionInvokerHelper(context);
            MethodInfo mi = typeof(ActionFilterOrderingController).GetMethod("SingleUnordered");

            // Execute
            var filters = helper.PublicGetActionFiltersForMember(mi);

            // Verify
            Assert.AreEqual(1, filters.Count);
            Assert.AreEqual(-1, ((ActionFilterAttribute)filters[0]).Order);
        }

        [TestMethod]
        public void GetAllActionFiltersCallsGetActionFiltersForMember() {
            // GetAllActionFilters() should return filters in this order:
            //   1. The controller itself (if it implements IActionFilter)
            //   2. Attributes applied to the controller's base classes
            //   3. Attributes applied to the controller class
            //   4. Attributes applied to the target method's base declarations
            //   5. Attributes applied to the target method

            // Setup
            var controller = new DerivedActionFilterOrderingController();
            ControllerContext context = GetControllerContext(controller);
            MethodInfo mi = typeof(DerivedActionFilterOrderingController).GetMethod("OverriddenMethod");

            InvokerForFilters invoker = new InvokerForFilters(context);
            invoker.Expectations.Enqueue(mi); // PublicGetAllActionFilters
            invoker.Expectations.Enqueue(mi); // GetAllActionFilters
            invoker.Expectations.Enqueue(typeof(object)); // GetActionFiltersForMember
            invoker.Expectations.Enqueue(typeof(Controller)); // GetActionFiltersForMember
            invoker.Expectations.Enqueue(typeof(ActionFilterOrderingController)); // GetActionFiltersForMember
            invoker.Expectations.Enqueue(typeof(DerivedActionFilterOrderingController)); // GetActionFiltersForMember
            invoker.Expectations.Enqueue(typeof(ActionFilterOrderingController).GetMethod("OverriddenMethod")); // GetActionFiltersForMember
            invoker.Expectations.Enqueue(mi); // GetActionFiltersForMember

            // Execute
            IList<IActionFilter> filters = invoker.PublicGetAllActionFilters(mi);

            // Verify
            Assert.AreEqual(1, filters.Count);
            Assert.AreEqual<int>(0, invoker.Expectations.Count);
        }

        [TestMethod]
        public void GetAllActionFiltersGetsControllerTypeFromContext() {
            // DevDiv Bugs 193338: MVC: ControllerFilters are not executed if the target action is defined in a base type

            // If a ControllerContext points to a derived controller but the target action method
            // is in the base controller, we should still execute the derived controller's action filters since
            // we're calling the method on an instance of the derived controller.

            // Setup
            var controller = new DerivedActionFilterOrderingController();
            ControllerContext context = GetControllerContext(controller);
            ControllerActionInvokerHelper helper = new ControllerActionInvokerHelper(context);
            MethodInfo mi = typeof(DerivedActionFilterOrderingController).GetMethod("NoFilters");

            // Execute
            var filters = helper.PublicGetAllActionFilters(mi);

            // Verify
            Assert.AreEqual(3, filters.Count);
            Assert.AreSame(controller, filters[0]); // controller should be the first filter
            Assert.AreEqual("BaseClass", ((KeyedActionFilterAttribute)filters[1]).Key);
            Assert.AreEqual("DerivedClass", ((KeyedActionFilterAttribute)filters[2]).Key);
        }

        [TestMethod]
        public void GetAllActionFiltersDoesNotAddHiddenMembers() {
            // If a target method hides a method in the base class, GetAllActionFilters() shouldn't return the
            // filters declared on the base class method.

            // Setup
            var controller = new DerivedActionFilterOrderingController();
            ControllerContext context = GetControllerContext(controller);
            ControllerActionInvokerHelper helper = new ControllerActionInvokerHelper(context);
            MethodInfo mi = typeof(DerivedActionFilterOrderingController).GetMethod("HiddenMethod");

            // Execute
            var filters = helper.PublicGetAllActionFilters(mi);

            // Verify
            Assert.AreEqual(4, filters.Count);
            Assert.AreSame(controller, filters[0]); // controller should be the first filter
            Assert.AreEqual("BaseClass", ((KeyedActionFilterAttribute)filters[1]).Key);
            Assert.AreEqual("DerivedClass", ((KeyedActionFilterAttribute)filters[2]).Key);
            Assert.AreEqual("NewMethod", ((KeyedActionFilterAttribute)filters[3]).Key);
        }

        [TestMethod]
        public void GetAllActionFiltersReturnsBaseFiltersFirst() {
            // GetAllActionFilters() should return filters in this order:
            //   1. The controller itself (if it implements IActionFilter)
            //   2. Attributes applied to the controller's base classes
            //   3. Attributes applied to the controller class
            //   4. Attributes applied to the target method's base declarations
            //   5. Attributes applied to the target method

            // Setup
            var controller = new DerivedActionFilterOrderingController();
            ControllerContext context = GetControllerContext(controller);
            ControllerActionInvokerHelper helper = new ControllerActionInvokerHelper(context);
            MethodInfo mi = typeof(DerivedActionFilterOrderingController).GetMethod("OverriddenMethod");

            // Execute
            var filters = helper.PublicGetAllActionFilters(mi);

            // Verify
            Assert.AreEqual(5, filters.Count);
            Assert.AreSame(controller, filters[0]); // controller should be the first filter
            Assert.AreEqual("BaseClass", ((KeyedActionFilterAttribute)filters[1]).Key);
            Assert.AreEqual("DerivedClass", ((KeyedActionFilterAttribute)filters[2]).Key);
            Assert.AreEqual("BaseMethod", ((KeyedActionFilterAttribute)filters[3]).Key);
            Assert.AreEqual("OverrideMethod", ((KeyedActionFilterAttribute)filters[4]).Key);
        }

        [TestMethod]
        public void GetAllActionFiltersWithJumpsInMethodInfoInheritanceChain() {
            // DevDiv Bugs 193448: MVC: Filters from base virtual method are not triggered if it is overriden on the second inherited class

            // Setup
            var controller = new SubDerivedActionFilterOrderingController();
            ControllerContext context = GetControllerContext(controller);
            ControllerActionInvokerHelper helper = new ControllerActionInvokerHelper(context);
            MethodInfo mi = typeof(SubDerivedActionFilterOrderingController).GetMethod("SomeVirtual");

            // Execute
            var filters = helper.PublicGetAllActionFilters(mi);

            // Verify
            Assert.AreEqual(6, filters.Count);
            Assert.AreSame(controller, filters[0]); // controller should be the first filter
            Assert.AreEqual("BaseClass", ((KeyedActionFilterAttribute)filters[1]).Key);
            Assert.AreEqual("DerivedClass", ((KeyedActionFilterAttribute)filters[2]).Key);
            Assert.AreEqual("SubDerivedClass", ((KeyedActionFilterAttribute)filters[3]).Key);
            Assert.AreEqual("BaseVirtual", ((KeyedActionFilterAttribute)filters[4]).Key);
            Assert.AreEqual("SubDerivedVirtual", ((KeyedActionFilterAttribute)filters[5]).Key);
        }

        [TestMethod]
        public void GetAllActionFiltersWithNullMethodInfoThrows() {
            // Setup
            var controller = new DerivedActionFilterOrderingController();
            ControllerContext context = GetControllerContext(controller);
            ControllerActionInvokerHelper helper = new ControllerActionInvokerHelper(context);

            // Execute & verify
            ExceptionHelper.ExpectArgumentNullException(
                delegate {
                    helper.PublicGetAllActionFilters(null /* methodInfo */);
                },
                "methodInfo");
        }

        [TestMethod]
        public void GetParameterValueCanConvertDateTimeToString() {
            // String's TypeConverter can't convert from DateTime, but DateTime's converter can convert to String.
            // This tests that we call the "to" converter if the "from" converter reports failure.

            // Setup
            var controller = new ParameterTestingController();
            ControllerContext context = GetControllerContext(controller);
            Dictionary<string, object> dict = new Dictionary<string, object>() {
                { "id", new DateTime(2001, 1, 1) }
            };
            ControllerActionInvokerHelper helper = new ControllerActionInvokerHelper(context);
            MethodInfo mi = typeof(ParameterTestingController).GetMethod("TakesString");
            ParameterInfo[] pis = mi.GetParameters();

            // Execute
            object oValue = helper.PublicGetParameterValue(pis[0], dict);

            // Verify
            Assert.AreEqual("1/1/2001", oValue);
        }

        [TestMethod]
        public void GetParameterValueCanConvertStringToDateTime() {
            // Setup
            var controller = new ParameterTestingController();
            ControllerContext context = GetControllerContext(controller);
            Dictionary<string, object> dict = new Dictionary<string, object>() {
                { "id", "1/1/2001" }
            };
            ControllerActionInvokerHelper helper = new ControllerActionInvokerHelper(context);
            MethodInfo mi = typeof(ParameterTestingController).GetMethod("TakesDateTime");
            ParameterInfo[] pis = mi.GetParameters();

            // Execute
            object oValue = helper.PublicGetParameterValue(pis[0], dict);

            // Verify
            Assert.AreEqual(new DateTime(2001, 1, 1), oValue);
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
            var controller = new ParameterTestingController();
            IDictionary<string, object> dict = new Dictionary<string, object>();
            ControllerContext context = GetControllerContext(controller);
            MethodInfo mi = typeof(ParameterTestingController).GetMethod("Foo");
            ParameterInfo[] pis = mi.GetParameters();

            InvokerForParameters invoker = new InvokerForParameters(context);

            // Execute
            IDictionary<string, object> parameters = invoker.PublicGetParameterValues(mi, dict);

            // Verify
            Assert.AreEqual(3, parameters.Count);
            Assert.AreEqual("Myfoo", parameters["foo"]);
            Assert.AreEqual("Mybar", parameters["bar"]);
            Assert.AreEqual("Mybaz", parameters["baz"]);
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
            ActionExecutedContext postContext = new ActionExecutedContext(ControllerContextTest.GetControllerContext(), mi, null /* exception */);
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
            Assert.AreSame(actionResult, result.Result);
        }

        [TestMethod]
        public void InvokeActionFilterWithNormalControlFlow() {
            // Setup
            List<string> actions = new List<string>();
            MethodInfo mi = typeof(object).GetMethod("ToString");
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            ActionExecutedContext postContext = new ActionExecutedContext(ControllerContextTest.GetControllerContext(), mi, null /* exception */);
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
        public void InvokeActionMethodWithActionResultReturnValue() {
            // Setup
            var controller = new BasicMethodInvokeController();
            ControllerContext context = GetControllerContext(controller);
            ControllerActionInvokerHelper helper = new ControllerActionInvokerHelper(context);
            MethodInfo mi = typeof(BasicMethodInvokeController).GetMethod("ReturnsRenderView");
            object viewData = new object();
            IDictionary<string, object> parameters = new Dictionary<string, object>() {
                { "viewData", viewData }
            };

            // Execute
            RenderViewResult result = helper.PublicInvokeActionMethod(mi, parameters) as RenderViewResult;

            // Verify (arg got passed to method + back correctly)
            Assert.AreEqual("ReturnsRenderView", result.ViewName);
            Assert.AreSame(viewData, result.ViewData);
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
            // InvokeActionMethod() should allow null return values.

            // Setup
            var controller = new BasicMethodInvokeController();
            ControllerContext context = GetControllerContext(controller);
            ControllerActionInvokerHelper helper = new ControllerActionInvokerHelper(context);
            MethodInfo mi = typeof(BasicMethodInvokeController).GetMethod("ReturnsNull");

            // Execute
            ActionResult result = helper.PublicInvokeActionMethod(mi, new Dictionary<string, object>());

            // Verify
            Assert.IsNull(result);
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
        public void InvokeActionMethodWithWrongReturnTypeThrows() {
            // Setup
            var controller = new BasicMethodInvokeController();
            ControllerContext context = GetControllerContext(controller);
            ControllerActionInvokerHelper helper = new ControllerActionInvokerHelper(context);
            MethodInfo mi = typeof(BasicMethodInvokeController).GetMethod("ReturnsNonActionResult");

            // Execute & verify
            ExceptionHelper.ExpectException<InvalidOperationException>(
                delegate {
                    helper.PublicInvokeActionMethod(mi, new Dictionary<string, object>());
                },
                "The action method 'ReturnsNonActionResult' on controller 'System.Web.Mvc.Test.ControllerActionInvokerTest+BasicMethodInvokeController' "
                    + "returned a value of type 'System.Object'. Action methods must return an ActionResult.");
        }

        [TestMethod]
        public void InvokeActionResultWithFiltersPassesSameContextObjectToInnerFilters() {
            // Setup
            ResultExecutingContext storedContext = null;
            ActionResult result = new EmptyResult();
            List<IActionFilter> filters = new List<IActionFilter>() {
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
            List<IActionFilter> filters = new List<IActionFilter>() {
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
            List<IActionFilter> filters = new List<IActionFilter>() {
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
            List<IActionFilter> filters = new List<IActionFilter>() {
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
        public void InvokeActionReturnsTrueOnSuccess() {
            // Setup
            var controller = new Mock<IController>().Object;
            IDictionary<string, object> dict = new Dictionary<string, object>();
            ControllerContext context = GetControllerContext(controller);
            MethodInfo mi = typeof(object).GetMethod("ToString");
            IDictionary<string, object> parameterValues = new Dictionary<string, object>();
            InvokerForEverything invoker = new InvokerForEverything(context) {
                ExpectedMethodInfo = mi,
                ExpectedValues = dict,
                ExpectedParameterValues = parameterValues
            };

            // Execute
            bool retVal = invoker.InvokeAction("ReturnsNull", dict);

            // Verify
            Assert.IsTrue(retVal);
            Assert.AreEqual<int>(8, invoker.ExpectationCount);
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
            helper.PublicInvokeActionResultWithFilters(actionResult, new List<IActionFilter>() { filter1, filter2 });

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
                new List<IActionFilter>() { filter });

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
            Assert.AreSame(actionResult, result.Result);
        }

        [TestMethod]
        public void InvokeResultFilterWithNormalControlFlow() {
            // Setup
            List<string> actions = new List<string>();
            ActionResult actionResult = new EmptyResult();
            ResultExecutedContext postContext = new ResultExecutedContext(ControllerContextTest.GetControllerContext(), actionResult, null /* exception */);
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

        private class KeyedActionFilterAttribute : ActionFilterAttribute {

            public KeyedActionFilterAttribute(string key) {
                Key = key;
            }

            public string Key {
                get;
                private set;
            }

        }

        private class ActionFilterImpl : IActionFilter {

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

        [KeyedActionFilter("BaseClass", Order = 0)]
        private class ActionFilterOrderingController : Controller {

            [EmptyActionFilter]
            [EmptyActionFilter(Order = 1)]
            [EmptyActionFilter]
            [EmptyActionFilter(Order = 10)]
            [EmptyActionFilter]
            [EmptyActionFilter(Order = 5)]
            public void OrderedAndUnordered() {
            }

            [EmptyActionFilter]
            public void SingleUnordered() {
            }

            public void NoFilters() {
            }

            [EmptyActionFilter(Order = 10)]
            public void SingleOrdered() {
            }

            [EmptyActionFilter(Order = 0)]
            [EmptyActionFilter(Order = 0)]
            public void ConflictingOrders() {
            }

            [KeyedActionFilter("BaseMethod", Order = 0)]
            public virtual void HiddenMethod() {
            }

            [KeyedActionFilter("BaseMethod", Order = 0)]
            public virtual void OverriddenMethod() {
            }

            [KeyedActionFilter("BaseVirtual", Order = 0)]
            public virtual void SomeVirtual() {
            }

        }

        [KeyedActionFilter("DerivedClass", Order = 0)]
        private class DerivedActionFilterOrderingController : ActionFilterOrderingController {


            [KeyedActionFilter("NewMethod", Order = 0)]
            public new void HiddenMethod() {
            }

            [KeyedActionFilter("OverrideMethod", Order = 0)]
            public override void OverriddenMethod() {
            }

        }

        [KeyedActionFilter("SubDerivedClass", Order = 0)]
        private class SubDerivedActionFilterOrderingController : DerivedActionFilterOrderingController {

            [KeyedActionFilter("SubDerivedVirtual", Order = 0)]
            public override void SomeVirtual() {
            }

        }

        [EmptyActionFilter(Order = 0)]
        [EmptyActionFilter(Order = 0)]
        private class ConflictingActionFilterOrderingController : Controller {
        }

        // This controller serves only to test vanilla method invocation - nothing exciting here
        private class BasicMethodInvokeController : Controller {
            public ActionResult ReturnsRenderView(object viewData) {
                return RenderView("ReturnsRenderView", viewData);
            }
            public ActionResult ReturnsNull() {
                return null;
            }
            public object ReturnsNonActionResult() {
                return new object();
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

            public virtual MethodInfo PublicFindActionMethod(string actionName, IDictionary<string, object> values) {
                return FindActionMethod(actionName, values);
            }

            public virtual IList<IActionFilter> PublicGetActionFiltersForMember(MemberInfo memberInfo) {
                return GetActionFiltersForMember(memberInfo);
            }

            public virtual IList<IActionFilter> PublicGetAllActionFilters(MethodInfo methodInfo) {
                return GetAllActionFilters(methodInfo);
            }

            public virtual object PublicGetParameterValue(ParameterInfo parameterInfo, IDictionary<string, object> values) {
                return GetParameterValue(parameterInfo, values);
            }

            public virtual IDictionary<string, object> PublicGetParameterValues(MethodInfo methodInfo, IDictionary<string, object> values) {
                return GetParameterValues(methodInfo, values);
            }

            public virtual ActionResult PublicInvokeActionMethod(MethodInfo methodInfo, IDictionary<string, object> parameters) {
                return InvokeActionMethod(methodInfo, parameters);
            }

            public virtual ActionExecutedContext PublicInvokeActionMethodWithFilters(MethodInfo methodInfo, IDictionary<string, object> parameters, IList<IActionFilter> filters) {
                return InvokeActionMethodWithFilters(methodInfo, parameters, filters);
            }

            public virtual void PublicInvokeActionResult(ActionResult actionResult) {
                InvokeActionResult(actionResult);
            }

            public virtual ResultExecutedContext PublicInvokeActionResultWithFilters(ActionResult actionResult, IList<IActionFilter> filters) {
                return InvokeActionResultWithFilters(actionResult, filters);
            }

        }

        public class Person {
        }

        public class Employee : Person {
        }

        private sealed class InvokerForFilters : ControllerActionInvoker {
            public Queue<MemberInfo> Expectations = new Queue<MemberInfo>();

            public InvokerForFilters(ControllerContext context)
                : base(context) {
            }

            public IList<IActionFilter> PublicGetAllActionFilters(MethodInfo methodInfo) {
                MemberInfo next = Expectations.Dequeue();
                Assert.AreEqual(next, methodInfo);
                return GetAllActionFilters(methodInfo);
            }

            protected override IList<IActionFilter> GetAllActionFilters(MethodInfo methodInfo) {
                MemberInfo next = Expectations.Dequeue();
                Assert.AreEqual(next, methodInfo);
                return base.GetAllActionFilters(methodInfo);
            }

            protected override IList<IActionFilter> GetActionFiltersForMember(MemberInfo memberInfo) {
                MemberInfo next = Expectations.Dequeue();
                Assert.AreEqual(next, memberInfo);
                return new List<IActionFilter>();
            }
        }

        private sealed class InvokerForParameters : ControllerActionInvoker {
            public InvokerForParameters(ControllerContext context)
                : base(context) {
            }

            public IDictionary<string, object> PublicGetParameterValues(MethodInfo methodInfo, IDictionary<string, object> values) {
                return base.GetParameterValues(methodInfo, values);
            }

            protected override object GetParameterValue(ParameterInfo parameterInfo, IDictionary<string, object> values) {
                return "My" + parameterInfo.Name;
            }
        }

        private sealed class InvokerForEverything : ControllerActionInvoker {
            public MethodInfo ExpectedMethodInfo;
            public IDictionary<string, object> ExpectedValues;
            public IDictionary<string, object> ExpectedParameterValues;
            public int ExpectationCount;

            public InvokerForEverything(ControllerContext context)
                : base(context) {
            }

            public override bool InvokeAction(string actionName, IDictionary<string, object> values) {
                Assert.AreEqual(0, ExpectationCount);
                ExpectationCount++;
                Assert.AreEqual("ReturnsNull", actionName);
                Assert.AreEqual(ExpectedValues, values);
                return base.InvokeAction(actionName, values);
            }

            protected override MethodInfo FindActionMethod(string actionName, IDictionary<string, object> values) {
                Assert.AreEqual(1, ExpectationCount);
                ExpectationCount++;
                Assert.AreEqual("ReturnsNull", actionName);
                Assert.AreEqual(ExpectedValues, values);
                return ExpectedMethodInfo;
            }

            protected override IDictionary<string, object> GetParameterValues(MethodInfo methodInfo, IDictionary<string, object> values) {
                Assert.AreEqual(2, ExpectationCount);
                ExpectationCount++;
                Assert.AreEqual(ExpectedMethodInfo, methodInfo);
                Assert.AreEqual(ExpectedValues, values);
                return ExpectedParameterValues;
            }

            protected override IList<IActionFilter> GetAllActionFilters(MethodInfo methodInfo) {
                Assert.AreEqual(3, ExpectationCount);
                ExpectationCount++;
                Assert.AreEqual(ExpectedMethodInfo, methodInfo);
                return new List<IActionFilter>();
            }

            protected override ActionExecutedContext InvokeActionMethodWithFilters(MethodInfo methodInfo, IDictionary<string, object> parameters, IList<IActionFilter> filters) {
                Assert.AreEqual(4, ExpectationCount);
                ExpectationCount++;
                Assert.AreEqual(ExpectedMethodInfo, methodInfo);
                Assert.AreEqual(ExpectedParameterValues, parameters);
                return base.InvokeActionMethodWithFilters(methodInfo, parameters, filters);
            }

            protected override ActionResult InvokeActionMethod(MethodInfo methodInfo, IDictionary<string, object> parameters) {
                Assert.AreEqual(5, ExpectationCount);
                ExpectationCount++;
                Assert.AreEqual(ExpectedMethodInfo, methodInfo);
                Assert.AreEqual(ExpectedParameterValues, parameters);
                return null;
            }

            protected override ResultExecutedContext InvokeActionResultWithFilters(ActionResult actionResult, IList<IActionFilter> filters) {
                Assert.AreEqual(6, ExpectationCount);
                ExpectationCount++;
                return base.InvokeActionResultWithFilters(actionResult, filters);
            }

            protected override void InvokeActionResult(ActionResult actionResult) {
                Assert.AreEqual(7, ExpectationCount);
                ExpectationCount++;
                base.InvokeActionResult(actionResult);
            }
        }
    }
}
