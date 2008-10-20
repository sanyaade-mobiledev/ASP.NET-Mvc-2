namespace System.Web.Mvc.Test {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Reflection;
    using System.Threading;
    using System.Web.Mvc;
    using System.Web.Routing;
    using System.Web.TestUtil;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;

    [TestClass]
    [CLSCompliant(false)]
    public class ControllerActionInvokerTest {

        [TestMethod]
        public void FindActionMethodDoesNotMatchConstructor() {
            // FindActionMethod() shouldn't match special-named methods like type constructors.

            // Arrange
            var controller = new FindMethodController();
            ControllerContext context = GetControllerContext(controller);
            ControllerActionInvokerHelper helper = new ControllerActionInvokerHelper(context);

            // Act
            MethodInfo mi = helper.PublicFindActionMethod(".ctor");

            // Assert
            Assert.IsNull(mi);
        }

        [TestMethod]
        public void FindActionMethodDoesNotMatchEvent() {
            // FindActionMethod() should skip methods that aren't publicly visible.

            // Arrange
            var controller = new FindMethodController();
            ControllerContext context = GetControllerContext(controller);
            ControllerActionInvokerHelper helper = new ControllerActionInvokerHelper(context);

            // Act
            MethodInfo mi = helper.PublicFindActionMethod("add_Event");

            // Assert
            Assert.IsNull(mi);
        }

        [TestMethod]
        public void FindActionMethodDoesNotMatchInternalMethod() {
            // FindActionMethod() should skip methods that aren't publicly visible.

            // Arrange
            var controller = new FindMethodController();
            ControllerContext context = GetControllerContext(controller);
            ControllerActionInvokerHelper helper = new ControllerActionInvokerHelper(context);

            // Act
            MethodInfo mi = helper.PublicFindActionMethod("InternalMethod");

            // Assert
            Assert.IsNull(mi);
        }

        [TestMethod]
        public void FindActionMethodDoesNotMatchMethodsDefinedOnControllerType() {
            // FindActionMethod() shouldn't match methods originally defined on the Controller type, e.g. Dispose().

            // Arrange
            var controller = new BlankController();
            ControllerContext context = GetControllerContext(controller);
            ControllerActionInvokerHelper helper = new ControllerActionInvokerHelper(context);
            var methods = typeof(Controller).GetMethods(BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);

            // Act & Assert
            foreach (var method in methods) {
                bool wasFound = true;
                try {
                    MethodInfo mi = helper.PublicFindActionMethod(method.Name);
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

            // Arrange
            var controller = new FindMethodController();
            ControllerContext context = GetControllerContext(controller);
            ControllerActionInvokerHelper helper = new ControllerActionInvokerHelper(context);

            // Act
            MethodInfo mi = helper.PublicFindActionMethod("ToString");

            // Assert
            Assert.IsNull(mi);
        }

        [TestMethod]
        public void FindActionMethodDoesNotMatchNonActionMethod() {
            // FindActionMethod() should respect the [NonAction] attribute.

            // Arrange
            var controller = new FindMethodController();
            ControllerContext context = GetControllerContext(controller);
            ControllerActionInvokerHelper helper = new ControllerActionInvokerHelper(context);

            // Act
            MethodInfo mi = helper.PublicFindActionMethod("NonActionMethod");

            // Assert
            Assert.IsNull(mi);
        }

        [TestMethod]
        public void FindActionMethodDoesNotMatchOverriddenNonActionMethod() {
            // FindActionMethod() should trace the method's inheritance chain looking for the [NonAction] attribute.

            // Arrange
            var controller = new DerivedFindMethodController();
            ControllerContext context = GetControllerContext(controller);
            ControllerActionInvokerHelper helper = new ControllerActionInvokerHelper(context);

            // Act
            MethodInfo mi = helper.PublicFindActionMethod("NonActionMethod");

            // Assert
            Assert.IsNull(mi);
        }

        [TestMethod]
        public void FindActionMethodDoesNotMatchPrivateMethod() {
            // FindActionMethod() should skip methods that aren't publicly visible.

            // Arrange
            var controller = new FindMethodController();
            ControllerContext context = GetControllerContext(controller);
            ControllerActionInvokerHelper helper = new ControllerActionInvokerHelper(context);

            // Act
            MethodInfo mi = helper.PublicFindActionMethod("PrivateMethod");

            // Assert
            Assert.IsNull(mi);
        }

        [TestMethod]
        public void FindActionMethodDoesNotMatchProperty() {
            // FindActionMethod() shouldn't match special-named methods like property getters.

            // Arrange
            var controller = new FindMethodController();
            ControllerContext context = GetControllerContext(controller);
            ControllerActionInvokerHelper helper = new ControllerActionInvokerHelper(context);

            // Act
            MethodInfo mi = helper.PublicFindActionMethod("get_Property");

            // Assert
            Assert.IsNull(mi);
        }

        [TestMethod]
        public void FindActionMethodDoesNotMatchProtectedMethod() {
            // FindActionMethod() should skip methods that aren't publicly visible.

            // Arrange
            var controller = new FindMethodController();
            ControllerContext context = GetControllerContext(controller);
            ControllerActionInvokerHelper helper = new ControllerActionInvokerHelper(context);

            // Act
            MethodInfo mi = helper.PublicFindActionMethod("ProtectedMethod");

            // Assert
            Assert.IsNull(mi);
        }

        [TestMethod]
        public void FindActionMethodIsCaseInsensitive() {
            // Arrange
            var controller = new FindMethodController();
            ControllerContext context = GetControllerContext(controller);
            ControllerActionInvokerHelper helper = new ControllerActionInvokerHelper(context);
            MethodInfo expected = typeof(FindMethodController).GetMethod("ValidActionMethod");

            // Act
            MethodInfo mi1 = helper.PublicFindActionMethod("validactionmethod");
            MethodInfo mi2 = helper.PublicFindActionMethod("VALIDACTIONMETHOD");

            // Assert
            Assert.AreEqual(expected, mi1);
            Assert.AreEqual(expected, mi2);
        }

        [TestMethod]
        public void FindActionMethodMatchesNewActionMethodsHidingNonActionMethods() {
            // FindActionMethod() should stop looking for [NonAction] in the method's inheritance chain when it sees
            // that a method in a derived class hides the a method in the base class.

            // Arrange
            var controller = new DerivedFindMethodController();
            ControllerContext context = GetControllerContext(controller);
            ControllerActionInvokerHelper helper = new ControllerActionInvokerHelper(context);
            MethodInfo expected = typeof(DerivedFindMethodController).GetMethod("DerivedIsActionMethod");

            // Act
            MethodInfo mi = helper.PublicFindActionMethod("DerivedIsActionMethod");

            // Assert
            Assert.AreEqual(expected, mi);
        }

        [TestMethod]
        public void FindActionMethodWithClosedGenerics() {
            // FindActionMethod() should work with generic methods as long as there are no open types.

            // Arrange
            var controller = new GenericFindMethodController<int>();
            ControllerContext context = GetControllerContext(controller);
            ControllerActionInvokerHelper helper = new ControllerActionInvokerHelper(context);
            MethodInfo expected = typeof(GenericFindMethodController<int>).GetMethod("ClosedGenericMethod");

            // Act
            MethodInfo mi = helper.PublicFindActionMethod("ClosedGenericMethod");

            // Assert
            Assert.AreEqual(expected, mi);
        }

        [TestMethod]
        public void FindActionMethodWithEmptyActionNameThrows() {
            // Arrange
            var controller = new FindMethodController();
            ControllerContext context = GetControllerContext(controller);
            ControllerActionInvokerHelper helper = new ControllerActionInvokerHelper(context);

            // Act & Assert
            ExceptionHelper.ExpectArgumentExceptionNullOrEmpty(
                delegate {
                    helper.PublicFindActionMethod(String.Empty);
                },
                "actionName");
        }

        [TestMethod]
        public void FindActionMethodWithNullActionNameThrows() {
            // Arrange
            var controller = new FindMethodController();
            ControllerContext context = GetControllerContext(controller);
            ControllerActionInvokerHelper helper = new ControllerActionInvokerHelper(context);

            // Act & Assert
            ExceptionHelper.ExpectArgumentExceptionNullOrEmpty(
                delegate {
                    helper.PublicFindActionMethod(null /* actionName */);
                },
                "actionName");
        }

        [TestMethod]
        public void FindActionMethodWithOpenGenericsThrows() {
            // FindActionMethod() should throw if matching on a generic method with open types.

            // Arrange
            var controller = new GenericFindMethodController<int>();
            ControllerContext context = GetControllerContext(controller);
            ControllerActionInvokerHelper helper = new ControllerActionInvokerHelper(context);

            // Act & Assert
            ExceptionHelper.ExpectException<InvalidOperationException>(
                delegate {
                    helper.PublicFindActionMethod("OpenGenericMethod");
                },
                "Cannot call action method 'System.Web.Mvc.ActionResult OpenGenericMethod[U](U)' since it is a generic method.");
        }

        [TestMethod]
        public void FindActionMethodWithOverloadsThrows() {
            // FindActionMethod() should throw if it encounters an overloaded method.

            // Arrange
            var controller = new FindMethodController();
            ControllerContext context = GetControllerContext(controller);
            ControllerActionInvokerHelper helper = new ControllerActionInvokerHelper(context);

            // Act & Assert
            ExceptionHelper.ExpectException<InvalidOperationException>(
                delegate {
                    helper.PublicFindActionMethod("MethodOverloaded");
                },
                @"The current request for action 'MethodOverloaded' on controller type 'FindMethodController' is ambiguous between the following action methods:
System.Web.Mvc.ActionResult MethodOverloaded() on type System.Web.Mvc.Test.ControllerActionInvokerTest+FindMethodController
System.Web.Mvc.ActionResult MethodOverloaded(System.String) on type System.Web.Mvc.Test.ControllerActionInvokerTest+FindMethodController");
        }

        [TestMethod]
        public void FindActionMethodWithValidMethod() {
            // Test basic functionality of FindActionMethod() by giving it a known good case.

            // Arrange
            var controller = new FindMethodController();
            ControllerContext context = GetControllerContext(controller);
            ControllerActionInvokerHelper helper = new ControllerActionInvokerHelper(context);
            MethodInfo expected = typeof(FindMethodController).GetMethod("ValidActionMethod");

            // Act
            MethodInfo mi = helper.PublicFindActionMethod("ValidActionMethod");

            // Assert
            Assert.AreEqual(expected, mi);
        }

        [TestMethod]
        public void GetFiltersForActionMethod() {
            // Arrange
            ControllerBase controller = new GetMemberChainSubderivedController();
            ControllerContext context = GetControllerContext(controller);
            MethodInfo methodInfo = typeof(GetMemberChainSubderivedController).GetMethod("SomeVirtual");
            ControllerActionInvokerHelper helper = new ControllerActionInvokerHelper(context);

            // Act
            var filters = helper.PublicGetFiltersForActionMethod(methodInfo);

            // Assert
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

            // Arrange
            ControllerBase controller = new GetMemberChainDerivedController();
            ControllerContext context = GetControllerContext(controller);
            MethodInfo methodInfo = typeof(GetMemberChainDerivedController).GetMethod("SomeVirtual");
            ControllerActionInvokerHelper helper = new ControllerActionInvokerHelper(context);

            // Act
            var filters = helper.PublicGetFiltersForActionMethod(methodInfo);

            // Assert
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
            // Arrange
            ControllerBase controller = new GetMemberChainSubderivedController();
            ControllerContext context = GetControllerContext(controller);
            ControllerActionInvokerHelper helper = new ControllerActionInvokerHelper(context);

            // Act & Assert
            ExceptionHelper.ExpectArgumentNullException(
                delegate {
                    helper.PublicGetFiltersForActionMethod(null /* methodInfo */);
                }, "methodInfo");
        }

        [TestMethod]
        public void GetParameterValueAllowsAllSubpropertiesIfBindAttributeNotSpecified() {
            // Arrange
            CustomConverterController controller = new CustomConverterController() { ValueProvider = new Mock<IValueProvider>().Object };
            ControllerContext controllerContext = GetControllerContext(controller);
            ControllerActionInvokerHelper helper = new ControllerActionInvokerHelper(controllerContext);

            ParameterInfo paramWithoutBindAttribute = typeof(CustomConverterController).GetMethod("ParameterWithoutBindAttribute").GetParameters()[0];

            // Act
            object valueWithoutBindAttribute = helper.PublicGetParameterValue(paramWithoutBindAttribute);

            // Assert
            Assert.AreEqual("foo=True&bar=True", valueWithoutBindAttribute);
        }

        [TestMethod]
        public void GetParameterValueResolvesConvertersInCorrectOrderOfPrecedence() {
            // Order of precedence:
            //   1. Attributes on the parameter itself
            //   2. Query the global converter provider

            // Arrange
            CustomConverterController controller = new CustomConverterController();
            Dictionary<string, object> values = new Dictionary<string, object> { { "foo", "fooValue" } };
            ControllerContext controllerContext = GetControllerContext(controller, values);
            controller.ControllerContext = controllerContext;
            ControllerActionInvokerHelper helper = new ControllerActionInvokerHelper(controllerContext);

            ParameterInfo paramWithOneConverter = typeof(CustomConverterController).GetMethod("ParameterHasOneConverter").GetParameters()[0];
            ParameterInfo paramWithNoConverters = typeof(CustomConverterController).GetMethod("ParameterHasNoConverters").GetParameters()[0];

            // Act
            object valueWithOneConverter = helper.PublicGetParameterValue(paramWithOneConverter);
            object valueWithNoConverters = helper.PublicGetParameterValue(paramWithNoConverters);

            // Assert
            Assert.AreEqual("foo_String", valueWithOneConverter);
            Assert.AreEqual("fooValue", valueWithNoConverters);
        }

        [TestMethod]
        public void GetParameterValueRespectsBindAttribute() {
            // Arrange
            CustomConverterController controller = new CustomConverterController() { ValueProvider = new Mock<IValueProvider>().Object };
            ControllerContext controllerContext = GetControllerContext(controller);
            ControllerActionInvokerHelper helper = new ControllerActionInvokerHelper(controllerContext);

            ParameterInfo paramWithBindAttribute = typeof(CustomConverterController).GetMethod("ParameterHasBindAttribute").GetParameters()[0];

            // Act
            object valueWithBindAttribute = helper.PublicGetParameterValue(paramWithBindAttribute);

            // Assert
            Assert.AreEqual("foo=True&bar=False", valueWithBindAttribute);
        }

        [TestMethod]
        public void GetParameterValueRespectsBindAttributePrefix() {
            // Arrange
            CustomConverterController controller = new CustomConverterController();
            Dictionary<string, object> values = new Dictionary<string, object> { { "foo", "fooValue" }, { "bar", "barValue" } };
            ControllerContext controllerContext = GetControllerContext(controller, values);
            controller.ControllerContext = controllerContext;

            ControllerActionInvokerHelper helper = new ControllerActionInvokerHelper(controllerContext);

            ParameterInfo paramWithFieldPrefix = typeof(CustomConverterController).GetMethod("ParameterHasFieldPrefix").GetParameters()[0];

            // Act
            object parameterValue = helper.PublicGetParameterValue(paramWithFieldPrefix);

            // Assert
            Assert.AreEqual("barValue", parameterValue);
        }

        [TestMethod]
        public void GetParameterValueRespectsBindAttributeNullPrefix() {
            // Arrange
            CustomConverterController controller = new CustomConverterController();
            Dictionary<string, object> values = new Dictionary<string, object> { { "foo", "fooValue" }, { "bar", "barValue" } };
            ControllerContext controllerContext = GetControllerContext(controller, values);
            controller.ControllerContext = controllerContext;

            ControllerActionInvokerHelper helper = new ControllerActionInvokerHelper(controllerContext);

            ParameterInfo paramWithFieldPrefix = typeof(CustomConverterController).GetMethod("ParameterHasNullFieldPrefix").GetParameters()[0];

            // Act
            object parameterValue = helper.PublicGetParameterValue(paramWithFieldPrefix);

            // Assert
            Assert.AreEqual("fooValue", parameterValue);
        }

        [TestMethod]
        public void GetParameterValueRespectsBindAttributeEmptyPrefix() {
            // Arrange
            CustomConverterController controller = new CustomConverterController();
            Dictionary<string, object> values = new Dictionary<string, object> { { "foo", "fooValue" }, { "bar", "barValue" }, { "intprop", "123" }, { "stringprop", "hello" } };
            ControllerContext controllerContext = GetControllerContext(controller, values);
            controller.ControllerContext = controllerContext;

            ControllerActionInvokerHelper helper = new ControllerActionInvokerHelper(controllerContext);

            ParameterInfo paramWithFieldPrefix = typeof(CustomConverterController).GetMethod("ParameterHasEmptyFieldPrefix").GetParameters()[0];

            // Act
            MySimpleModel parameterValue = helper.PublicGetParameterValue(paramWithFieldPrefix) as MySimpleModel;

            // Assert
            Assert.IsNotNull(parameterValue);
            Assert.AreEqual<int>(123, parameterValue.IntProp);
            Assert.AreEqual<string>("hello", parameterValue.StringProp);
        }

        [TestMethod]
        public void GetParameterValueReturnsNullIfCannotConvertNonRequiredParameter() {
            // Arrange
            var controller = new ParameterTestingController();
            ControllerContext context = GetControllerContext(controller);
            controller.ControllerContext = context;

            Dictionary<string, object> dict = new Dictionary<string, object>() {
                { "id", DateTime.Now } // cannot convert DateTime to Nullable<int>
            };
            ControllerActionInvokerHelper helper = new ControllerActionInvokerHelper(context);
            MethodInfo mi = typeof(ParameterTestingController).GetMethod("TakesNullableInt");
            ParameterInfo[] pis = mi.GetParameters();

            // Act
            object oValue = helper.PublicGetParameterValue(pis[0]);

            // Assert
            Assert.IsNull(oValue);
        }

        [TestMethod]
        public void GetParameterValueReturnsNullIfNullableTypeValueNotFound() {
            // Arrange
            var controller = new ParameterTestingController();
            ControllerContext context = GetControllerContext(controller);
            controller.ControllerContext = context;

            ControllerActionInvokerHelper helper = new ControllerActionInvokerHelper(context);
            MethodInfo mi = typeof(ParameterTestingController).GetMethod("TakesNullableInt");
            ParameterInfo[] pis = mi.GetParameters();

            // Act
            object oValue = helper.PublicGetParameterValue(pis[0]);

            // Assert
            Assert.IsNull(oValue);
        }

        [TestMethod]
        public void GetParameterValueReturnsNullIfReferenceTypeValueNotFound() {
            // Arrange
            var controller = new ParameterTestingController();
            ControllerContext context = GetControllerContext(controller);
            controller.ControllerContext = context;

            ControllerActionInvokerHelper helper = new ControllerActionInvokerHelper(context);
            MethodInfo mi = typeof(ParameterTestingController).GetMethod("Foo");
            ParameterInfo[] pis = mi.GetParameters();

            // Act
            object oValue = helper.PublicGetParameterValue(pis[0]);

            // Assert
            Assert.IsNull(oValue);
        }

        [TestMethod]
        public void GetParameterValuesCallsGetParameterValue() {
            // Arrange
            ControllerBase controller = new ParameterTestingController();
            IDictionary<string, object> dict = new Dictionary<string, object>();
            ControllerContext context = GetControllerContext(controller);
            MethodInfo mi = typeof(ParameterTestingController).GetMethod("Foo");
            ParameterInfo[] pis = mi.GetParameters();

            Mock<ControllerActionInvokerHelper> mockHelper = new Mock<ControllerActionInvokerHelper>(context);
            mockHelper.CallBase = true;
            mockHelper.Expect(h => h.PublicGetParameterValue(pis[0])).Returns("Myfoo").Verifiable();
            mockHelper.Expect(h => h.PublicGetParameterValue(pis[1])).Returns("Mybar").Verifiable();
            mockHelper.Expect(h => h.PublicGetParameterValue(pis[2])).Returns("Mybaz").Verifiable();
            ControllerActionInvokerHelper helper = mockHelper.Object;

            // Act
            IDictionary<string, object> parameters = helper.PublicGetParameterValues(mi);

            // Assert
            Assert.AreEqual(3, parameters.Count);
            Assert.AreEqual("Myfoo", parameters["foo"]);
            Assert.AreEqual("Mybar", parameters["bar"]);
            Assert.AreEqual("Mybaz", parameters["baz"]);
            mockHelper.Verify();
        }

        [TestMethod]
        public void GetParameterValuesReturnsEmptyArrayForParameterlessMethod() {
            // Arrange
            var controller = new ParameterTestingController();
            ControllerContext context = GetControllerContext(controller);
            ControllerActionInvokerHelper helper = new ControllerActionInvokerHelper(context);
            MethodInfo mi = typeof(ParameterTestingController).GetMethod("Parameterless");

            // Act
            IDictionary<string, object> parameters = helper.PublicGetParameterValues(mi);

            // Assert
            Assert.AreEqual(0, parameters.Count);
        }

        [TestMethod]
        public void GetParameterValuesReturnsValuesForParametersInOrder() {
            // We need to hook into GetParameterValue() to make sure that GetParameterValues() is calling it.

            // Arrange
            var controller = new ParameterTestingController();
            Dictionary<string, object> dict = new Dictionary<string, object>() {
                { "foo", "MyFoo" },
                { "bar", "MyBar" },
                { "baz", "MyBaz" }
            };
            ControllerContext context = GetControllerContext(controller, dict);
            controller.ControllerContext = context;

            ControllerActionInvokerHelper helper = new ControllerActionInvokerHelper(context);
            MethodInfo mi = typeof(ParameterTestingController).GetMethod("Foo");

            // Act
            IDictionary<string, object> parameters = helper.PublicGetParameterValues(mi);

            // Assert
            Assert.AreEqual(3, parameters.Count);
            Assert.AreEqual("MyFoo", parameters["foo"]);
            Assert.AreEqual("MyBar", parameters["bar"]);
            Assert.AreEqual("MyBaz", parameters["baz"]);
        }

        [TestMethod]
        public void GetParameterValuesWithNullMethodInfoThrows() {
            // Arrange
            var controller = new ParameterTestingController();
            ControllerContext context = GetControllerContext(controller);
            ControllerActionInvokerHelper helper = new ControllerActionInvokerHelper(context);

            // Act & Assert
            ExceptionHelper.ExpectArgumentNullException(
                delegate {
                    helper.PublicGetParameterValues(null /* methodInfo */);
                },
                "methodInfo");
        }

        [TestMethod]
        public void GetParameterValuesWithOutParamThrows() {
            // Arrange
            var controller = new ParameterTestingController();
            ControllerContext context = GetControllerContext(controller);
            ControllerActionInvokerHelper helper = new ControllerActionInvokerHelper(context);
            MethodInfo mi = typeof(ParameterTestingController).GetMethod("HasOutParam");

            // Act & Assert
            ExceptionHelper.ExpectException<InvalidOperationException>(
                delegate {
                    helper.PublicGetParameterValues(mi);
                },
                "Cannot set value for parameter 'foo' in action 'HasOutParam'. Parameters passed by reference are not supported in action methods.");
        }

        [TestMethod]
        public void GetParameterValuesWithRefParamThrows() {
            // Arrange
            var controller = new ParameterTestingController();
            ControllerContext context = GetControllerContext(controller);
            ControllerActionInvokerHelper helper = new ControllerActionInvokerHelper(context);
            MethodInfo mi = typeof(ParameterTestingController).GetMethod("HasRefParam");

            // Act & Assert
            ExceptionHelper.ExpectException<InvalidOperationException>(
                delegate {
                    helper.PublicGetParameterValues(mi);
                },
                "Cannot set value for parameter 'foo' in action 'HasRefParam'. Parameters passed by reference are not supported in action methods.");
        }

        // TODO: This test is temporarily disabled now that converters can update ModelState. We should consider reenabling
        // this if invalid inputs to action methods become problematic.
        public void GetParameterValueThrowsIfCannotConvertRequiredParameter() {
            // Arrange
            var controller = new ParameterTestingController();
            ControllerContext context = GetControllerContext(controller);
            ControllerActionInvokerHelper helper = new ControllerActionInvokerHelper(context);
            MethodInfo mi = typeof(ParameterTestingController).GetMethod("TakesInt");
            ParameterInfo[] pis = mi.GetParameters();

            // Act & Assert
            ExceptionHelper.ExpectInvalidOperationException(
                delegate {
                    helper.PublicGetParameterValue(pis[0]);
                },
                "A value is required for parameter 'id' in action 'TakesInt'. The parameter either has no value or its value could"
                + " not be converted. To make a parameter optional its type should either be a reference type or a Nullable type.");
        }

        [TestMethod]
        public void GetParameterValueThrowsIfParameterHasMultipleConverters() {
            // Arrange
            CustomConverterController controller = new CustomConverterController();
            ControllerContext controllerContext = GetControllerContext(controller);
            ControllerActionInvokerHelper helper = new ControllerActionInvokerHelper(controllerContext);

            ParameterInfo paramWithTwoConverters = typeof(CustomConverterController).GetMethod("ParameterHasTwoConverters").GetParameters()[0];

            // Act & Assert
            ExceptionHelper.ExpectInvalidOperationException(
                delegate {
                    helper.PublicGetParameterValue(paramWithTwoConverters);
                },
                "The parameter 'foo' on method 'ParameterHasTwoConverters' contains multiple attributes inheriting from CustomModelBinderAttribute.");
        }

        [TestMethod]
        public void GetParameterValueUsesControllerValueProviderAsValueProvider() {
            // Arrange
            Mock<IValueProvider> mockValueProvider = new Mock<IValueProvider>();
            mockValueProvider.Expect(p => p.GetValue("foo")).Returns(new ValueProviderResult("fooValue", "fooValue", null));

            CustomConverterController controller = new CustomConverterController() { ValueProvider = mockValueProvider.Object };
            ControllerContext controllerContext = GetControllerContext(controller);
            ControllerActionInvokerHelper helper = new ControllerActionInvokerHelper(controllerContext);

            ParameterInfo parameter = typeof(CustomConverterController).GetMethod("ParameterHasNoConverters").GetParameters()[0];

            // Act
            object parameterValue = helper.PublicGetParameterValue(parameter);

            // Assert
            Assert.AreEqual("fooValue", parameterValue);
        }

        [TestMethod]
        public void GetParameterValueWithNullParameterInfoThrows() {
            // Arrange
            var controller = new ParameterTestingController();
            ControllerContext context = GetControllerContext(controller);
            ControllerActionInvokerHelper helper = new ControllerActionInvokerHelper(context);

            // Act & Assert
            ExceptionHelper.ExpectArgumentNullException(
                delegate {
                    helper.PublicGetParameterValue(null /* parameterInfo */);
                },
                "parameterInfo");
        }

        [TestMethod]
        public void InvokeAction() {
            // Arrange
            ControllerBase controller = new Mock<ControllerBase>().Object;
            ControllerContext context = GetControllerContext(controller);
            IDictionary<string, object> paramValues = new Dictionary<string, object>();
            MethodInfo methodInfo = typeof(object).GetMethod("ToString");
            var filterInfo = new FilterInfo();
            ActionResult actionResult = new EmptyResult();
            ActionExecutedContext postContext = new ActionExecutedContext(context, false /* canceled */, null /* exception */) {
                Result = actionResult
            };
            AuthorizationContext authContext = new AuthorizationContext(context);

            Mock<ControllerActionInvokerHelper> mockHelper = new Mock<ControllerActionInvokerHelper>();
            mockHelper.CallBase = true;
            mockHelper.Expect(h => h.PublicFindActionMethod("SomeMethod")).Returns(methodInfo).Verifiable();
            mockHelper.Expect(h => h.PublicGetParameterValues(methodInfo)).Returns(paramValues).Verifiable();
            mockHelper.Expect(h => h.PublicGetFiltersForActionMethod(methodInfo)).Returns(filterInfo).Verifiable();
            mockHelper.Expect(h => h.PublicInvokeAuthorizationFilters(methodInfo, filterInfo.AuthorizationFilters)).Returns(authContext).Verifiable();
            mockHelper.Expect(h => h.PublicInvokeActionMethodWithFilters(methodInfo, paramValues, filterInfo.ActionFilters)).Returns(postContext).Verifiable();
            mockHelper.Expect(h => h.PublicInvokeActionResultWithFilters(actionResult, filterInfo.ResultFilters)).Returns((ResultExecutedContext)null).Verifiable();
            ControllerActionInvokerHelper helper = mockHelper.Object;

            // Act
            bool retVal = helper.InvokeAction(context, "SomeMethod");
            Assert.IsTrue(retVal, "InvokeAction() should return True on success.");
            Assert.AreSame(context, helper.ControllerContext, "ControllerContext property should have been set by InvokeAction");
            mockHelper.Verify();
        }

        [TestMethod]
        public void InvokeActionFiltersOrdersFiltersCorrectly() {
            // Arrange
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
            ControllerBase controller = new ContinuationController(continuation);
            ControllerContext context = GetControllerContext(controller);
            ControllerActionInvokerHelper helper = new ControllerActionInvokerHelper(context);

            // Act
            helper.PublicInvokeActionMethodWithFilters(ContinuationController.GoMethod, parameters,
                new List<IActionFilter>() { filter1, filter2 });

            // Assert
            Assert.AreEqual(5, actions.Count);
            Assert.AreEqual("OnActionExecuting1", actions[0]);
            Assert.AreEqual("OnActionExecuting2", actions[1]);
            Assert.AreEqual("Continuation", actions[2]);
            Assert.AreEqual("OnActionExecuted2", actions[3]);
            Assert.AreEqual("OnActionExecuted1", actions[4]);
        }

        [TestMethod]
        public void InvokeActionFiltersPassesArgumentsCorrectly() {
            // Arrange
            bool wasCalled = false;
            MethodInfo mi = ContinuationController.GoMethod;
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            ActionResult actionResult = new EmptyResult();
            ActionFilterImpl filter = new ActionFilterImpl() {
                OnActionExecutingImpl = delegate(ActionExecutingContext filterContext) {
                    Assert.AreSame(parameters, filterContext.ActionParameters);
                    Assert.IsFalse(wasCalled);
                    wasCalled = true;
                    filterContext.Result = actionResult;
                }
            };
            Func<ActionResult> continuation = delegate {
                Assert.Fail("Continuation should not be called.");
                return null;
            };
            ControllerBase controller = new ContinuationController(continuation);
            ControllerContext context = GetControllerContext(controller);
            ControllerActionInvokerHelper helper = new ControllerActionInvokerHelper(context);

            // Act
            ActionExecutedContext result = helper.PublicInvokeActionMethodWithFilters(mi, parameters,
                new List<IActionFilter>() { filter });

            // Assert
            Assert.IsTrue(wasCalled);
            Assert.IsNull(result.Exception);
            Assert.IsFalse(result.ExceptionHandled);
            Assert.AreSame(actionResult, result.Result);
        }

        [TestMethod]
        public void InvokeActionFilterWhereContinuationThrowsExceptionAndIsHandled() {
            // Arrange
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
                    Assert.AreSame(exception, filterContext.Exception);
                    Assert.IsFalse(filterContext.ExceptionHandled);
                    filterContext.ExceptionHandled = true;
                }
            };
            Func<ActionExecutedContext> continuation = delegate {
                actions.Add("Continuation");
                throw exception;
            };

            // Act
            ActionExecutedContext result = ControllerActionInvoker.InvokeActionMethodFilter(filter, new ActionExecutingContext(ControllerContextTest.GetControllerContext(), parameters), continuation);

            // Assert
            Assert.AreEqual(3, actions.Count);
            Assert.AreEqual("OnActionExecuting", actions[0]);
            Assert.AreEqual("Continuation", actions[1]);
            Assert.AreEqual("OnActionExecuted", actions[2]);
            Assert.AreSame(exception, result.Exception);
            Assert.IsTrue(result.ExceptionHandled);
        }

        [TestMethod]
        public void InvokeActionFilterWhereContinuationThrowsExceptionAndIsNotHandled() {
            // Arrange
            List<string> actions = new List<string>();
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

            // Act & Assert
            ExceptionHelper.ExpectException<Exception>(
                delegate {
                    ControllerActionInvoker.InvokeActionMethodFilter(filter, new ActionExecutingContext(ControllerContextTest.GetControllerContext(), parameters), continuation);
                },
               "Some exception message.");
            Assert.AreEqual(3, actions.Count);
            Assert.AreEqual("OnActionExecuting", actions[0]);
            Assert.AreEqual("Continuation", actions[1]);
            Assert.AreEqual("OnActionExecuted", actions[2]);
        }

        [TestMethod]
        public void InvokeActionFilterWhereOnActionExecutingCancels() {
            // Arrange
            bool wasCalled = false;
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            ActionExecutedContext postContext = new ActionExecutedContext(ControllerContextTest.GetControllerContext(), false /* canceled */, null /* exception */);
            ActionResult actionResult = new EmptyResult();
            ActionFilterImpl filter = new ActionFilterImpl() {
                OnActionExecutingImpl = delegate(ActionExecutingContext filterContext) {
                    Assert.IsFalse(wasCalled);
                    wasCalled = true;
                    filterContext.Result = actionResult;
                },
            };
            Func<ActionExecutedContext> continuation = delegate {
                Assert.Fail("The continuation should not be called.");
                return null;
            };

            // Act
            ActionExecutedContext result = ControllerActionInvoker.InvokeActionMethodFilter(filter, new ActionExecutingContext(ControllerContextTest.GetControllerContext(), parameters), continuation);

            // Assert
            Assert.IsTrue(wasCalled);
            Assert.IsNull(result.Exception);
            Assert.IsTrue(result.Canceled);
            Assert.AreSame(actionResult, result.Result);
        }

        [TestMethod]
        public void InvokeActionFilterWithNormalControlFlow() {
            // Arrange
            List<string> actions = new List<string>();
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            ActionExecutedContext postContext = new ActionExecutedContext(ControllerContextTest.GetControllerContext(), false /* canceled */, null /* exception */);
            ActionFilterImpl filter = new ActionFilterImpl() {
                OnActionExecutingImpl = delegate(ActionExecutingContext filterContext) {
                    Assert.AreSame(parameters, filterContext.ActionParameters);
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

            // Act
            ActionExecutedContext result = ControllerActionInvoker.InvokeActionMethodFilter(filter, new ActionExecutingContext(ControllerContextTest.GetControllerContext(), parameters), continuation);

            // Assert
            Assert.AreEqual(3, actions.Count);
            Assert.AreEqual("OnActionExecuting", actions[0]);
            Assert.AreEqual("Continuation", actions[1]);
            Assert.AreEqual("OnActionExecuted", actions[2]);
            Assert.AreSame(result, postContext);
        }

        [TestMethod]
        public void InvokeActionInvokesEmptyResultIfAuthorizationFailsAndNoResultSpecified() {
            // Arrange
            ControllerBase controller = new Mock<ControllerBase>().Object;
            ControllerContext context = GetControllerContext(controller);
            IDictionary<string, object> values = new Dictionary<string, object>();
            IDictionary<string, object> paramValues = new Dictionary<string, object>();
            MethodInfo methodInfo = typeof(object).GetMethod("ToString");
            var filterInfo = new FilterInfo();
            ActionResult actionResult = new EmptyResult();
            ActionExecutedContext postContext = new ActionExecutedContext(context, false /* canceled */, null /* exception */) {
                Result = actionResult
            };
            AuthorizationContext authContext = new AuthorizationContext(context) { Cancel = true };

            Mock<ControllerActionInvokerHelper> mockHelper = new Mock<ControllerActionInvokerHelper>();
            mockHelper.CallBase = true;
            mockHelper.Expect(h => h.PublicFindActionMethod("SomeMethod")).Returns(methodInfo).Verifiable();
            mockHelper.Expect(h => h.PublicGetParameterValues(methodInfo)).Returns(paramValues).Verifiable();
            mockHelper.Expect(h => h.PublicGetFiltersForActionMethod(methodInfo)).Returns(filterInfo).Verifiable();
            mockHelper.Expect(h => h.PublicInvokeAuthorizationFilters(methodInfo, filterInfo.AuthorizationFilters)).Returns(authContext).Verifiable();
            mockHelper.Expect(h => h.PublicInvokeActionResult(EmptyResult.Instance)).Verifiable();
            ControllerActionInvokerHelper helper = mockHelper.Object;

            // Act
            bool retVal = helper.InvokeAction(context, "SomeMethod");
            Assert.IsTrue(retVal, "InvokeAction() should return True on success.");
            mockHelper.Verify();
        }

        [TestMethod]
        public void InvokeActionInvokesExceptionFiltersAndExecutesResultIfExceptionHandled() {
            // Arrange
            ControllerBase controller = new Mock<ControllerBase>().Object;
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

            Mock<ControllerActionInvokerHelper> mockHelper = new Mock<ControllerActionInvokerHelper>();
            mockHelper.CallBase = true;
            mockHelper.Expect(h => h.PublicFindActionMethod("SomeMethod")).Returns(methodInfo).Verifiable();
            mockHelper.Expect(h => h.PublicGetParameterValues(methodInfo)).Returns(paramValues).Verifiable();
            mockHelper.Expect(h => h.PublicGetFiltersForActionMethod(methodInfo)).Returns(filterInfo).Verifiable();
            mockHelper.Expect(h => h.PublicInvokeAuthorizationFilters(methodInfo, filterInfo.AuthorizationFilters)).Throws(exception).Verifiable();
            mockHelper.Expect(h => h.PublicInvokeExceptionFilters(exception, filterInfo.ExceptionFilters)).Returns(exContext).Verifiable();
            mockHelper.Expect(h => h.PublicInvokeActionResult(actionResult)).Verifiable();
            ControllerActionInvokerHelper helper = mockHelper.Object;

            // Act
            bool retVal = helper.InvokeAction(context, "SomeMethod");
            Assert.IsTrue(retVal, "InvokeAction() should return True on success.");
            mockHelper.Verify();
        }

        [TestMethod]
        public void InvokeActionInvokesExceptionFiltersAndRethrowsExceptionIfNotHandled() {
            // Arrange
            var controllerMock = new Mock<ControllerBase>();
            controllerMock.CallBase = true;
            ControllerBase controller = controllerMock.Object;
            ControllerContext context = GetControllerContext(controller);
            IDictionary<string, object> values = new Dictionary<string, object>();
            IDictionary<string, object> paramValues = new Dictionary<string, object>();
            MethodInfo methodInfo = typeof(object).GetMethod("ToString");
            var filterInfo = new FilterInfo();
            Exception exception = new Exception();
            ExceptionContext exContext = new ExceptionContext(context, exception);

            Mock<ControllerActionInvokerHelper> mockHelper = new Mock<ControllerActionInvokerHelper>();
            mockHelper.CallBase = true;
            mockHelper.Expect(h => h.PublicFindActionMethod("SomeMethod")).Returns(methodInfo).Verifiable();
            mockHelper.Expect(h => h.PublicGetParameterValues(methodInfo)).Returns(paramValues).Verifiable();
            mockHelper.Expect(h => h.PublicGetFiltersForActionMethod(methodInfo)).Returns(filterInfo).Verifiable();
            mockHelper.Expect(h => h.PublicInvokeAuthorizationFilters(methodInfo, filterInfo.AuthorizationFilters)).Throws(exception).Verifiable();
            mockHelper.Expect(h => h.PublicInvokeExceptionFilters(exception, filterInfo.ExceptionFilters)).Returns(exContext).Verifiable();
            mockHelper.Expect(h => h.PublicInvokeActionResult(It.IsAny<ActionResult>())).Callback(delegate {
                Assert.Fail("InvokeActionResult() shouldn't be called if the exception was unhandled by filters.");
            });
            ControllerActionInvokerHelper helper = mockHelper.Object;

            // Act
            Exception thrownException = ExceptionHelper.ExpectException<Exception>(
                delegate {
                    helper.InvokeAction(context, "SomeMethod");
                });

            // Assert
            Assert.AreSame(exception, thrownException);
            mockHelper.Verify();
        }

        [TestMethod]
        public void InvokeActionInvokesResultIfAuthorizationFails() {
            // Arrange
            var controllerMock = new Mock<ControllerBase> { CallBase = true };
            ControllerBase controller = controllerMock.Object;
            ControllerContext context = GetControllerContext(controller);
            IDictionary<string, object> values = new Dictionary<string, object>();
            IDictionary<string, object> paramValues = new Dictionary<string, object>();
            MethodInfo methodInfo = typeof(object).GetMethod("ToString");
            var filterInfo = new FilterInfo();
            ActionResult actionResult = new EmptyResult();
            ActionExecutedContext postContext = new ActionExecutedContext(context, false /* canceled */, null /* exception */) {
                Result = actionResult
            };
            AuthorizationContext authContext = new AuthorizationContext(context) { Cancel = true, Result = actionResult };

            Mock<ControllerActionInvokerHelper> mockHelper = new Mock<ControllerActionInvokerHelper>();
            mockHelper.CallBase = true;
            mockHelper.Expect(h => h.PublicFindActionMethod("SomeMethod")).Returns(methodInfo).Verifiable();
            mockHelper.Expect(h => h.PublicGetParameterValues(methodInfo)).Returns(paramValues).Verifiable();
            mockHelper.Expect(h => h.PublicGetFiltersForActionMethod(methodInfo)).Returns(filterInfo).Verifiable();
            mockHelper.Expect(h => h.PublicInvokeAuthorizationFilters(methodInfo, filterInfo.AuthorizationFilters)).Returns(authContext).Verifiable();
            mockHelper.Expect(h => h.PublicInvokeActionResult(actionResult)).Verifiable();
            ControllerActionInvokerHelper helper = mockHelper.Object;

            // Act
            bool retVal = helper.InvokeAction(context, "SomeMethod");
            Assert.IsTrue(retVal, "InvokeAction() should return True on success.");
            mockHelper.Verify();
        }

        [TestMethod]
        public void InvokeActionMethod() {
            // Arrange
            var controller = new BasicMethodInvokeController();
            ControllerContext context = GetControllerContext(controller);
            ControllerActionInvokerHelper helper = new ControllerActionInvokerHelper(context);
            MethodInfo mi = typeof(BasicMethodInvokeController).GetMethod("ReturnsRenderView");
            object viewItem = new object();
            IDictionary<string, object> parameters = new Dictionary<string, object>() {
                { "viewItem", viewItem }
            };

            // Act
            ViewResult result = helper.PublicInvokeActionMethod(mi, parameters) as ViewResult;

            // Assert (arg got passed to method + back correctly)
            Assert.AreEqual("ReturnsRenderView", result.ViewName);
            Assert.AreSame(viewItem, result.ViewData.Model);
        }

        [TestMethod]
        public void InvokeActionMethodWithFiltersWithNullFilterListThrows() {
            // Arrange
            ControllerBase controller = new Mock<ControllerBase>().Object;
            ControllerContext context = GetControllerContext(controller);
            ControllerActionInvokerHelper helper = new ControllerActionInvokerHelper(context);

            // Act & Assert
            ExceptionHelper.ExpectArgumentNullException(
                delegate {
                    helper.PublicInvokeActionMethodWithFilters(typeof(object).GetMethod("ToString"), new Dictionary<string, object>(), null /* filters */);
                },
                "filters");
        }

        [TestMethod]
        public void InvokeActionMethodWithFiltersWithNullMethodInfoThrows() {
            // Arrange
            ControllerBase controller = new Mock<ControllerBase>().Object;
            ControllerContext context = GetControllerContext(controller);
            ControllerActionInvokerHelper helper = new ControllerActionInvokerHelper(context);

            // Act & Assert
            ExceptionHelper.ExpectArgumentNullException(
                delegate {
                    helper.PublicInvokeActionMethodWithFilters(null /* methodInfo */, null /* parameters */, null /* filters */);
                },
                "methodInfo");
        }

        [TestMethod]
        public void InvokeActionMethodWithFiltersWithNullParametersDictionaryThrows() {
            // Arrange
            ControllerBase controller = new Mock<ControllerBase>().Object;
            ControllerContext context = GetControllerContext(controller);
            ControllerActionInvokerHelper helper = new ControllerActionInvokerHelper(context);

            // Act & Assert
            ExceptionHelper.ExpectArgumentNullException(
                delegate {
                    helper.PublicInvokeActionMethodWithFilters(typeof(object).GetMethod("ToString"), null /* parameters */, null /* filters */);
                },
                "parameters");
        }

        [TestMethod]
        public void InvokeActionMethodWithNullMethodInfoThrows() {
            // Arrange
            var controller = new BasicMethodInvokeController();
            ControllerContext context = GetControllerContext(controller);
            ControllerActionInvokerHelper helper = new ControllerActionInvokerHelper(context);

            // Act & Assert
            ExceptionHelper.ExpectArgumentNullException(
                delegate {
                    helper.PublicInvokeActionMethod(null /* methodInfo */, null);
                },
                "methodInfo");
        }

        [TestMethod]
        public void InvokeActionMethodWithNullParametersDictionaryThrows() {
            // Arrange
            var controller = new BasicMethodInvokeController();
            ControllerContext context = GetControllerContext(controller);
            ControllerActionInvokerHelper helper = new ControllerActionInvokerHelper(context);
            MethodInfo mi = typeof(BasicMethodInvokeController).GetMethod("ReturnsRenderView");

            // Act & Assert
            ExceptionHelper.ExpectArgumentNullException(
                delegate {
                    helper.PublicInvokeActionMethod(mi, null /* parameters */);
                },
                "parameters");
        }

        [TestMethod]
        public void InvokeActionMethodWithParametersDictionaryContainingNullableType() {
            // Arrange
            var controller = new ParameterTestingController();
            ControllerContext context = GetControllerContext(controller);
            ControllerActionInvokerHelper helper = new ControllerActionInvokerHelper(context);
            MethodInfo mi = typeof(ParameterTestingController).GetMethod("TakesNullableInt");
            Dictionary<string, object> parameters = new Dictionary<string, object>() {
                { "id", null }
            };

            // Act
            ActionResult result = helper.PublicInvokeActionMethod(mi, parameters);

            // Assert
            Assert.IsTrue(controller.Values.ContainsKey("id"));
            Assert.IsNull(controller.Values["id"]);
        }

        [TestMethod]
        public void InvokeActionMethodWithParametersDictionaryContainingNullValueTypeThrows() {
            // Arrange
            var controller = new ParameterTestingController();
            ControllerContext context = GetControllerContext(controller);
            ControllerActionInvokerHelper helper = new ControllerActionInvokerHelper(context);
            MethodInfo mi = typeof(ParameterTestingController).GetMethod("TakesInt");
            Dictionary<string, object> parameters = new Dictionary<string, object>() {
                { "id", null }
            };

            // Act & Assert
            ExceptionHelper.ExpectException<InvalidOperationException>(
                delegate {
                    helper.PublicInvokeActionMethod(mi, parameters);
                },
                "The parameters dictionary does not contain a valid value of type 'System.Int32' for parameter 'id'"
                + " which is required for method 'Void TakesInt(Int32)' in 'System.Web.Mvc.Test.ControllerActionInvokerTest+ParameterTestingController'."
                + " To make a parameter optional its type should either be a reference type or a Nullable type.");
        }

        [TestMethod]
        public void InvokeActionMethodWithParametersDictionaryContainingWrongTypesThrows() {
            // Arrange
            var controller = new ParameterTestingController();
            ControllerContext context = GetControllerContext(controller);
            ControllerActionInvokerHelper helper = new ControllerActionInvokerHelper(context);
            MethodInfo mi = typeof(ParameterTestingController).GetMethod("TakesInt");
            Dictionary<string, object> parameters = new Dictionary<string, object>() {
                { "id", new object() }
            };

            // Act & Assert
            ExceptionHelper.ExpectException<InvalidOperationException>(
                delegate {
                    helper.PublicInvokeActionMethod(mi, parameters);
                },
                "The parameters dictionary does not contain a valid value of type 'System.Int32' for parameter 'id'"
                + " which is required for method 'Void TakesInt(Int32)' in 'System.Web.Mvc.Test.ControllerActionInvokerTest+ParameterTestingController'."
                + " To make a parameter optional its type should either be a reference type or a Nullable type.");
        }

        [TestMethod]
        public void InvokeActionMethodWithParametersDictionaryMissingEntriesThrows() {
            // Arrange
            var controller = new ParameterTestingController();
            ControllerContext context = GetControllerContext(controller);
            ControllerActionInvokerHelper helper = new ControllerActionInvokerHelper(context);
            MethodInfo mi = typeof(ParameterTestingController).GetMethod("TakesInt");
            Dictionary<string, object> parameters = new Dictionary<string, object>() {
                { "foo", "bar" }
            };

            // Act & Assert
            ExceptionHelper.ExpectException<InvalidOperationException>(
                delegate {
                    helper.PublicInvokeActionMethod(mi, parameters);
                },
                "The parameters dictionary does not contain a valid value of type 'System.Int32' for parameter 'id'"
                + " which is required for method 'Void TakesInt(Int32)' in 'System.Web.Mvc.Test.ControllerActionInvokerTest+ParameterTestingController'."
                + " To make a parameter optional its type should either be a reference type or a Nullable type.");
        }

        [TestMethod]
        public void InvokeActionMethodWithParametersDictionaryWrongLengthThrows() {
            // Arrange
            var controller = new BasicMethodInvokeController();
            ControllerContext context = GetControllerContext(controller);
            ControllerActionInvokerHelper helper = new ControllerActionInvokerHelper(context);
            MethodInfo mi = typeof(BasicMethodInvokeController).GetMethod("ReturnsRenderView");
            Dictionary<string, object> parameters = new Dictionary<string, object>() {
                { "foo", "bar" },
                { "baz", "quux" }
            };

            // Act & Assert
            ExceptionHelper.ExpectException<InvalidOperationException>(
                delegate {
                    helper.PublicInvokeActionMethod(mi, parameters);
                },
                "The parameter dictionary contains an incorrect number of entries for method 'System.Web.Mvc.ActionResult ReturnsRenderView(System.Object)'.");
        }

        [TestMethod]
        public void InvokeActionResultWithFiltersPassesSameContextObjectToInnerFilters() {
            // Arrange
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
            ControllerActionInvokerHelper helper = new ControllerActionInvokerHelper(GetControllerContext(new Mock<ControllerBase>().Object));

            // Act
            ResultExecutedContext postContext = helper.PublicInvokeActionResultWithFilters(result, filters);

            // Assert
            Assert.AreSame(result, postContext.Result);
        }

        [TestMethod]
        public void InvokeActionResultWithFiltersWithNullActionResultThrows() {
            // Arrange
            ControllerBase controller = new Mock<ControllerBase>().Object;
            ControllerContext context = GetControllerContext(controller);
            ControllerActionInvokerHelper helper = new ControllerActionInvokerHelper(context);

            // Act & Assert
            ExceptionHelper.ExpectArgumentNullException(
                delegate {
                    helper.PublicInvokeActionResultWithFilters(null /* actionResult */, null /* filters */);
                },
                "actionResult");
        }

        [TestMethod]
        public void InvokeActionResultWithFiltersWithNullFilterListThrows() {
            // Arrange
            ControllerBase controller = new Mock<ControllerBase>().Object;
            ControllerContext context = GetControllerContext(controller);
            ControllerActionInvokerHelper helper = new ControllerActionInvokerHelper(context);

            // Act & Assert
            ExceptionHelper.ExpectArgumentNullException(
                delegate {
                    helper.PublicInvokeActionResultWithFilters(new EmptyResult(), null /* filters */);
                },
                "filters");
        }

        [TestMethod]
        public void InvokeActionResultWithNullActionResultThrows() {
            // Arrange
            ControllerBase controller = new Mock<ControllerBase>().Object;
            ControllerContext context = GetControllerContext(controller);
            ControllerActionInvokerHelper helper = new ControllerActionInvokerHelper(context);

            // Act & Assert
            ExceptionHelper.ExpectArgumentNullException(
                delegate {
                    helper.PublicInvokeActionResult(null /* actionResult */);
                },
                "actionResult");
        }

        [TestMethod]
        public void InvokeActionReturnsFalseIfMethodNotFound() {
            // Arrange
            var controller = new BlankController();
            ControllerContext context = GetControllerContext(controller);
            ControllerActionInvoker invoker = new ControllerActionInvoker();

            // Act
            bool retVal = invoker.InvokeAction(context, "foo");

            // Assert
            Assert.IsFalse(retVal);
        }

        [TestMethod]
        public void InvokeActionThrowsIfControllerContextIsNull() {
            // Arrange
            ControllerActionInvoker invoker = new ControllerActionInvoker();

            // Act & Assert
            ExceptionHelper.ExpectArgumentNullException(
                delegate {
                    invoker.InvokeAction(null, "actionName");
                }, "controllerContext");
        }

        [TestMethod]
        public void InvokeActionWithEmptyActionNameThrows() {
            // Arrange
            var controller = new BasicMethodInvokeController();
            ControllerContext context = GetControllerContext(controller);
            ControllerActionInvoker invoker = new ControllerActionInvoker();

            // Act & Assert
            ExceptionHelper.ExpectArgumentExceptionNullOrEmpty(
                delegate {
                    invoker.InvokeAction(context, String.Empty);
                },
                "actionName");
        }

        [TestMethod]
        public void InvokeActionWithNullActionNameThrows() {
            // Arrange
            var controller = new BasicMethodInvokeController();
            ControllerContext context = GetControllerContext(controller);
            ControllerActionInvoker invoker = new ControllerActionInvoker();

            // Act & Assert
            ExceptionHelper.ExpectArgumentExceptionNullOrEmpty(
                delegate {
                    invoker.InvokeAction(context, null /* actionName */);
                },
                "actionName");
        }

        [TestMethod]
        public void InvokeActionWithResultExceptionInvokesExceptionFiltersAndExecutesResultIfExceptionHandled() {
            // Arrange
            ControllerBase controller = new Mock<ControllerBase>().Object;
            ControllerContext context = GetControllerContext(controller);
            IDictionary<string, object> values = new Dictionary<string, object>();
            IDictionary<string, object> paramValues = new Dictionary<string, object>();
            MethodInfo methodInfo = typeof(object).GetMethod("ToString");
            var filterInfo = new FilterInfo();
            Exception exception = new Exception();
            ActionResult actionResult = new EmptyResult();
            ActionExecutedContext postContext = new ActionExecutedContext(context, false /* canceled */, null /* exception */) {
                Result = actionResult
            };
            ExceptionContext exContext = new ExceptionContext(context, exception) {
                ExceptionHandled = true,
                Result = actionResult
            };
            AuthorizationContext authContext = new AuthorizationContext(context);

            Mock<ControllerActionInvokerHelper> mockHelper = new Mock<ControllerActionInvokerHelper>();
            mockHelper.CallBase = true;
            mockHelper.Expect(h => h.PublicFindActionMethod("SomeMethod")).Returns(methodInfo).Verifiable();
            mockHelper.Expect(h => h.PublicGetParameterValues(methodInfo)).Returns(paramValues).Verifiable();
            mockHelper.Expect(h => h.PublicGetFiltersForActionMethod(methodInfo)).Returns(filterInfo).Verifiable();
            mockHelper.Expect(h => h.PublicInvokeAuthorizationFilters(methodInfo, filterInfo.AuthorizationFilters)).Returns(authContext).Verifiable();
            mockHelper.Expect(h => h.PublicInvokeActionMethodWithFilters(methodInfo, paramValues, filterInfo.ActionFilters)).Returns(postContext).Verifiable();
            mockHelper.Expect(h => h.PublicInvokeActionResultWithFilters(actionResult, filterInfo.ResultFilters)).Throws(exception).Verifiable();
            mockHelper.Expect(h => h.PublicInvokeExceptionFilters(exception, filterInfo.ExceptionFilters)).Returns(exContext).Verifiable();
            mockHelper.Expect(h => h.PublicInvokeActionResult(actionResult)).Verifiable();
            ControllerActionInvokerHelper helper = mockHelper.Object;

            // Act
            bool retVal = helper.InvokeAction(context, "SomeMethod");
            Assert.IsTrue(retVal, "InvokeAction() should return True on success.");
            mockHelper.Verify();
        }

        [TestMethod]
        public void InvokeAuthorizationFilters() {
            // Arrange
            ControllerBase controller = new Mock<ControllerBase>().Object;
            MethodInfo methodInfo = typeof(object).GetMethod("ToString");
            ControllerContext controllerContext = GetControllerContext(controller);
            ControllerActionInvokerHelper helper = new ControllerActionInvokerHelper(controllerContext);

            List<AuthorizationFilterHelper> callQueue = new List<AuthorizationFilterHelper>();
            AuthorizationFilterHelper filter1 = new AuthorizationFilterHelper(callQueue);
            AuthorizationFilterHelper filter2 = new AuthorizationFilterHelper(callQueue);

            // Act
            AuthorizationContext postContext = helper.PublicInvokeAuthorizationFilters(methodInfo, new List<IAuthorizationFilter> { filter1, filter2 });

            // Assert
            Assert.AreEqual(2, callQueue.Count);
            Assert.AreSame(filter1, callQueue[0]);
            Assert.AreSame(filter2, callQueue[1]);
        }

        [TestMethod]
        public void InvokeAuthorizationFiltersStopsExecutingIfResultProvided() {
            // Arrange
            ControllerBase controller = new Mock<ControllerBase>().Object;
            MethodInfo methodInfo = typeof(object).GetMethod("ToString");
            ControllerContext controllerContext = GetControllerContext(controller);
            ControllerActionInvokerHelper helper = new ControllerActionInvokerHelper(controllerContext);
            ActionResult result = new EmptyResult();

            List<AuthorizationFilterHelper> callQueue = new List<AuthorizationFilterHelper>();
            AuthorizationFilterHelper filter1 = new AuthorizationFilterHelper(callQueue) { ShouldCancel = true, ShortCircuitResult = result };
            AuthorizationFilterHelper filter2 = new AuthorizationFilterHelper(callQueue);

            // Act
            AuthorizationContext postContext = helper.PublicInvokeAuthorizationFilters(methodInfo, new List<IAuthorizationFilter> { filter1, filter2 });

            // Assert
            Assert.IsTrue(postContext.Cancel);
            Assert.AreSame(result, postContext.Result);
            Assert.AreEqual(1, callQueue.Count);
            Assert.AreSame(filter1, callQueue[0]);
        }

        [TestMethod]
        public void InvokeAuthorizationFiltersWithNullFiltersThrows() {
            // Arrange
            ControllerBase controller = new Mock<ControllerBase>().Object;
            ControllerContext context = GetControllerContext(controller);
            ControllerActionInvokerHelper helper = new ControllerActionInvokerHelper(context);

            // Act & Assert
            ExceptionHelper.ExpectArgumentNullException(
                delegate {
                    helper.PublicInvokeAuthorizationFilters(typeof(object).GetMethod("ToString"), null /* filters */);
                }, "filters");
        }

        [TestMethod]
        public void InvokeExceptionFiltersWithNullMethodInfoThrows() {
            // Arrange
            ControllerBase controller = new Mock<ControllerBase>().Object;
            ControllerContext context = GetControllerContext(controller);
            ControllerActionInvokerHelper helper = new ControllerActionInvokerHelper(context);

            // Act & Assert
            ExceptionHelper.ExpectArgumentNullException(
                delegate {
                    helper.PublicInvokeAuthorizationFilters(null /* methodInfo */, null /* filters */);
                }, "methodInfo");
        }

        [TestMethod]
        public void InvokeExceptionFilters() {
            // Arrange
            ControllerBase controller = new Mock<ControllerBase>().Object;
            Exception exception = new Exception();
            ControllerContext controllerContext = GetControllerContext(controller);
            ControllerActionInvokerHelper helper = new ControllerActionInvokerHelper(controllerContext);

            List<ExceptionFilterHelper> callQueue = new List<ExceptionFilterHelper>();
            ExceptionFilterHelper filter1 = new ExceptionFilterHelper(callQueue);
            ExceptionFilterHelper filter2 = new ExceptionFilterHelper(callQueue);

            // Act
            ExceptionContext postContext = helper.PublicInvokeExceptionFilters(exception, new List<IExceptionFilter> { filter1, filter2 });

            // Assert
            Assert.AreSame(exception, postContext.Exception);
            Assert.IsFalse(postContext.ExceptionHandled);
            Assert.AreSame(filter1.ContextPassed, filter2.ContextPassed, "The same context should have been passed to each exception filter.");
            Assert.AreEqual(2, callQueue.Count);
            Assert.AreSame(filter1, callQueue[0]);
            Assert.AreSame(filter2, callQueue[1]);
        }

        [TestMethod]
        public void InvokeExceptionFiltersContinuesExecutingIfExceptionHandled() {
            // Arrange
            ControllerBase controller = new Mock<ControllerBase>().Object;
            Exception exception = new Exception();
            ControllerContext controllerContext = GetControllerContext(controller);
            ControllerActionInvokerHelper helper = new ControllerActionInvokerHelper(controllerContext);

            List<ExceptionFilterHelper> callQueue = new List<ExceptionFilterHelper>();
            ExceptionFilterHelper filter1 = new ExceptionFilterHelper(callQueue) { ShouldHandleException = true };
            ExceptionFilterHelper filter2 = new ExceptionFilterHelper(callQueue);

            // Act
            ExceptionContext postContext = helper.PublicInvokeExceptionFilters(exception, new List<IExceptionFilter> { filter1, filter2 });

            // Assert
            Assert.AreSame(exception, postContext.Exception);
            Assert.IsTrue(postContext.ExceptionHandled, "The exception should have been handled.");
            Assert.AreSame(filter1.ContextPassed, filter2.ContextPassed, "The same context should have been passed to each exception filter.");
            Assert.AreEqual(2, callQueue.Count);
            Assert.AreSame(filter1, callQueue[0]);
            Assert.AreSame(filter2, callQueue[1]);
        }

        [TestMethod]
        public void InvokeExceptionFiltersWithNullExceptionThrows() {
            // Arrange
            ControllerBase controller = new Mock<ControllerBase>().Object;
            ControllerContext context = GetControllerContext(controller);
            ControllerActionInvokerHelper helper = new ControllerActionInvokerHelper(context);

            // Act & Assert
            ExceptionHelper.ExpectArgumentNullException(
                delegate {
                    helper.PublicInvokeExceptionFilters(null /* exception */, null /* filters */);
                }, "exception");
        }

        [TestMethod]
        public void InvokeExceptionFiltersWithNullFiltersThrows() {
            // Arrange
            ControllerBase controller = new Mock<ControllerBase>().Object;
            ControllerContext context = GetControllerContext(controller);
            ControllerActionInvokerHelper helper = new ControllerActionInvokerHelper(context);

            // Act & Assert
            ExceptionHelper.ExpectArgumentNullException(
                delegate {
                    helper.PublicInvokeExceptionFilters(new Exception(), null /* filters */);
                }, "filters");
        }

        [TestMethod]
        public void InvokeResultFiltersOrdersFiltersCorrectly() {
            // Arrange
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
            ControllerBase controller = new Mock<ControllerBase>().Object;
            ControllerContext context = GetControllerContext(controller);
            ControllerActionInvokerHelper helper = new ControllerActionInvokerHelper(context);

            // Act
            helper.PublicInvokeActionResultWithFilters(actionResult, new List<IResultFilter>() { filter1, filter2 });

            // Assert
            Assert.AreEqual(5, actions.Count);
            Assert.AreEqual("OnResultExecuting1", actions[0]);
            Assert.AreEqual("OnResultExecuting2", actions[1]);
            Assert.AreEqual("Continuation", actions[2]);
            Assert.AreEqual("OnResultExecuted2", actions[3]);
            Assert.AreEqual("OnResultExecuted1", actions[4]);
        }

        [TestMethod]
        public void InvokeResultFiltersPassesArgumentsCorrectly() {
            // Arrange
            bool wasCalled = false;
            Action continuation = delegate {
                Assert.Fail("Continuation should not be called.");
            };
            ActionResult actionResult = new ContinuationResult(continuation);
            ControllerBase controller = new Mock<ControllerBase>().Object;
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

            // Act
            ResultExecutedContext result = helper.PublicInvokeActionResultWithFilters(actionResult,
                new List<IResultFilter>() { filter });

            // Assert
            Assert.IsTrue(wasCalled);
            Assert.IsNull(result.Exception);
            Assert.IsFalse(result.ExceptionHandled);
            Assert.AreSame(actionResult, result.Result);
        }

        [TestMethod]
        public void InvokeResultFilterWhereContinuationThrowsExceptionAndIsHandled() {
            // Arrange
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

            // Act
            ResultExecutedContext result = ControllerActionInvoker.InvokeActionResultFilter(filter, new ResultExecutingContext(ControllerContextTest.GetControllerContext(), actionResult), continuation);

            // Assert
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
            // Arrange
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

            // Act & Assert
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
            // Arrange
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

            // Act & Assert
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
            // Arrange
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

            // Act
            ResultExecutedContext result = ControllerActionInvoker.InvokeActionResultFilter(filter, new ResultExecutingContext(ControllerContextTest.GetControllerContext(), actionResult), continuation);

            // Assert
            Assert.IsTrue(wasCalled);
            Assert.IsNull(result.Exception);
            Assert.IsTrue(result.Canceled);
            Assert.AreSame(actionResult, result.Result);
        }

        [TestMethod]
        public void InvokeResultFilterWithNormalControlFlow() {
            // Arrange
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

            // Act
            ResultExecutedContext result = ControllerActionInvoker.InvokeActionResultFilter(filter, new ResultExecutingContext(ControllerContextTest.GetControllerContext(), actionResult), continuation);

            // Assert
            Assert.AreEqual(3, actions.Count);
            Assert.AreEqual("OnResultExecuting", actions[0]);
            Assert.AreEqual("Continuation", actions[1]);
            Assert.AreEqual("OnResultExecuted", actions[2]);
            Assert.AreSame(result, postContext);
        }

        [TestMethod]
        public void CreateActionResultWithActionResultParameterReturnsParameterUnchanged() {
            // Arrange
            ControllerActionInvokerHelper invoker = new ControllerActionInvokerHelper();
            ActionResult originalResult = new JsonResult();

            // Act
            ActionResult returnedActionResult = invoker.PublicCreateActionResult(originalResult);

            // Assert
            Assert.AreSame(originalResult, returnedActionResult);
        }

        [TestMethod]
        public void CreateActionResultWithNullParameterReturnsEmptyResult() {
            // Arrange
            ControllerActionInvokerHelper invoker = new ControllerActionInvokerHelper();

            // Act
            ActionResult returnedActionResult = invoker.PublicCreateActionResult(null);

            // Assert
            Assert.IsInstanceOfType(returnedActionResult, typeof(EmptyResult));
        }

        [TestMethod]
        public void CreateActionResultWithObjectParameterReturnsContentResult() {
            // Arrange
            ControllerActionInvokerHelper invoker = new ControllerActionInvokerHelper();
            object originalReturnValue = new CultureReflector();

            // Act
            ActionResult returnedActionResult = invoker.PublicCreateActionResult(originalReturnValue);

            // Assert
            Assert.IsInstanceOfType(returnedActionResult, typeof(ContentResult));
            ContentResult contentResult = (ContentResult)returnedActionResult;
            Assert.AreEqual("IVL", contentResult.Content);
        }

        [TestMethod]
        public void InvokeMethodCallsOverriddenCreateActionResult() {
            // Arrange
            CustomResultInvokerController controller = new CustomResultInvokerController();
            ControllerContext context = GetControllerContext(controller);
            CustomResultInvoker helper = new CustomResultInvoker(context);
            MethodInfo mi = typeof(CustomResultInvokerController).GetMethod("ReturnCustomResult");
            IDictionary<string, object> parameters = new Dictionary<string, object>();

            // Act
            ActionResult actionResult = helper.PublicInvokeActionMethod(mi, parameters);

            // Assert (arg got passed to method + back correctly)
            Assert.IsInstanceOfType(actionResult, typeof(CustomResult));
            CustomResult customResult = (CustomResult)actionResult;
            Assert.AreEqual("abc123", customResult.ReturnValue);
        }

        private static ControllerContext GetControllerContext(ControllerBase controller) {
            return GetControllerContext(controller, null);
        }

        private static ControllerContext GetControllerContext(ControllerBase controller, IDictionary<string, object> values) {
            RouteData routeData = new RouteData();
            if (values != null) {
                foreach (var entry in values) {
                    routeData.Values[entry.Key] = entry.Value;
                }
            }
            Mock<HttpContextBase> contextMock = new Mock<HttpContextBase>();
            contextMock.Expect(o => o.Request).Returns((HttpRequestBase)null); // make the TempDataDictionary happy
            contextMock.Expect(o => o.Session).Returns((HttpSessionStateBase)null);
            return new ControllerContext(contextMock.Object, routeData, controller);
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
        }

        private class BlankController : Controller {
        }

        private sealed class CustomResult : ActionResult {
            public object ReturnValue {
                get;
                set;
            }

            public override void ExecuteResult(ControllerContext context) {
                throw new NotImplementedException();
            }
        }

        private sealed class CustomResultInvokerController : Controller {
            public object ReturnCustomResult() {
                return "abc123";
            }

        }

        private sealed class CustomResultInvoker : ControllerActionInvokerHelper {
            public CustomResultInvoker(ControllerContext controllerContext)
                : base(controllerContext) {
            }

            protected override ActionResult CreateActionResult(object actionReturnValue) {
                return new CustomResult {
                    ReturnValue = actionReturnValue
                };
            }
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

        }

        // Provides access to the protected members of ControllerActionInvoker
        public class ControllerActionInvokerHelper : ControllerActionInvoker {

            public ControllerActionInvokerHelper() {
                // set instance caches to prevent modifying global test application state
                DispatcherCache = new ActionMethodDispatcherCache();
                SelectorCache = new ActionMethodSelectorCache();
            }

            public ControllerActionInvokerHelper(ControllerContext controllerContext)
                : this() {
                ControllerContext = controllerContext;
            }

            protected override MethodInfo FindActionMethod(string actionName) {
                return PublicFindActionMethod(actionName);
            }

            public virtual MethodInfo PublicFindActionMethod(string actionName) {
                return base.FindActionMethod(actionName);
            }

            protected override FilterInfo GetFiltersForActionMethod(MethodInfo methodInfo) {
                return PublicGetFiltersForActionMethod(methodInfo);
            }

            public virtual FilterInfo PublicGetFiltersForActionMethod(MethodInfo methodInfo) {
                return base.GetFiltersForActionMethod(methodInfo);
            }

            protected override object GetParameterValue(ParameterInfo parameterInfo) {
                return PublicGetParameterValue(parameterInfo);
            }

            public virtual object PublicGetParameterValue(ParameterInfo parameterInfo) {
                return base.GetParameterValue(parameterInfo);
            }

            protected override IDictionary<string, object> GetParameterValues(MethodInfo methodInfo) {
                return PublicGetParameterValues(methodInfo);
            }

            public virtual IDictionary<string, object> PublicGetParameterValues(MethodInfo methodInfo) {
                return base.GetParameterValues(methodInfo);
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

            public ActionResult PublicCreateActionResult(object result) {
                return CreateActionResult(result);
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

        private class CustomConverterController : Controller {

            public void ParameterWithoutBindAttribute([PredicateReflector] string someParam) {
            }

            public void ParameterHasBindAttribute([Bind(Include = "foo"), PredicateReflector] string someParam) {
            }

            public void ParameterHasFieldPrefix([Bind(Prefix = "bar")] string foo) {
            }

            public void ParameterHasNullFieldPrefix([Bind(Include = "whatever")] string foo) {
            }

            public void ParameterHasEmptyFieldPrefix([Bind(Prefix = "")] MySimpleModel foo) {
            }

            public void ParameterHasNoConverters(string foo) {
            }

            public void ParameterHasOneConverter([MyCustomConverter] string foo) {
            }

            public void ParameterHasTwoConverters([MyCustomConverter, MyCustomConverter] string foo) {
            }
        }

        public class MySimpleModel {
            public int IntProp { get; set; }
            public string StringProp { get; set; }
        }

        [AttributeUsage(AttributeTargets.Parameter, AllowMultiple = true, Inherited = false)]
        private class PredicateReflectorAttribute : CustomModelBinderAttribute {
            public override IModelBinder GetBinder() {
                return new MyConverter();
            }
            private class MyConverter : IModelBinder {
                public ModelBinderResult BindModel(ModelBindingContext bindingContext) {
                    string s = String.Format("foo={0}&bar={1}", bindingContext.ShouldUpdateProperty("foo"), bindingContext.ShouldUpdateProperty("bar"));
                    return new ModelBinderResult(s);
                }
            }
        }

        [AttributeUsage(AttributeTargets.Parameter, AllowMultiple = true, Inherited = false)]
        private class MyCustomConverterAttribute : CustomModelBinderAttribute {
            public override IModelBinder GetBinder() {
                return new MyConverter();
            }
            private class MyConverter : IModelBinder {
                public ModelBinderResult BindModel(ModelBindingContext bindingContext) {
                    string s = bindingContext.ModelName + "_" + bindingContext.ModelType.Name;
                    return new ModelBinderResult(s);
                }
            }
        }

    }
}
