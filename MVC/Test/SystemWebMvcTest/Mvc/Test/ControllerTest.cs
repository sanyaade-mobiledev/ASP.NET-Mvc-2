namespace System.Web.Mvc.Test {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;
    using System.Reflection;
    using System.Security.Principal;
    using System.Web.Routing;
    using System.Web.TestUtil;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;

    [TestClass]
    public class ControllerTest {
        [TestMethod]
        public void ContextProperty() {
            var controller = new EmptyController();
            Mock<HttpContextBase> httpContextMock = new Mock<HttpContextBase>();
            MemberHelper.TestPropertyValue(controller, "ControllerContext", new ControllerContext(httpContextMock.Object, new RouteData(), controller));
        }

        [TestMethod]
        public void HttpContextProperty() {
            var c = new EmptyController();
            Assert.IsNull(c.HttpContext, "Property should be null before Context is set");
            Mock<HttpContextBase> httpContextMock = new Mock<HttpContextBase>();
            c.ControllerContext = new ControllerContext(httpContextMock.Object, new RouteData(), c);
            Assert.AreEqual<HttpContextBase>(httpContextMock.Object, c.HttpContext, "Property should equal the value on the Context.");
        }

        [TestMethod]
        public void RequestProperty() {
            var c = new EmptyController();
            Assert.IsNull(c.Request, "Property should be null before Context is set");

            Mock<HttpContextBase> contextMock = new Mock<HttpContextBase>();
            Mock<HttpRequestBase> mockRequest = new Mock<HttpRequestBase>();
            contextMock.Expect(o => o.Request).Returns(mockRequest.Object);
            c.ControllerContext = new ControllerContext(contextMock.Object, new RouteData(), c);
            Assert.AreEqual<HttpRequestBase>(mockRequest.Object, c.Request, "Property should equal the value on the Context.");
        }

        [TestMethod]
        public void ResponseProperty() {
            var c = new EmptyController();
            Assert.IsNull(c.Response, "Property should be null before Context is set");

            Mock<HttpContextBase> contextMock = new Mock<HttpContextBase>();
            Mock<HttpResponseBase> mockResponse = new Mock<HttpResponseBase>();
            contextMock.Expect(o => o.Response).Returns(mockResponse.Object);
            c.ControllerContext = new ControllerContext(contextMock.Object, new RouteData(), c);
            Assert.AreEqual<HttpResponseBase>(mockResponse.Object, c.Response, "Property should equal the value on the Context.");
        }

        [TestMethod]
        public void ServerProperty() {
            var c = new EmptyController();
            Assert.IsNull(c.Server, "Property should be null before Context is set");

            Mock<HttpContextBase> contextMock = new Mock<HttpContextBase>();
            Mock<HttpServerUtilityBase> mockServer = new Mock<HttpServerUtilityBase>();
            contextMock.Expect(o => o.Server).Returns(mockServer.Object);
            c.ControllerContext = new ControllerContext(contextMock.Object, new RouteData(), c);
            Assert.AreEqual<HttpServerUtilityBase>(mockServer.Object, c.Server, "Property should equal the value on the Context.");
        }

        [TestMethod]
        public void UserProperty() {
            var c = new EmptyController();
            Assert.IsNull(c.User, "Property should be null before Context is set");

            Mock<HttpContextBase> contextMock = new Mock<HttpContextBase>();
            Mock<IPrincipal> mockUser = new Mock<IPrincipal>();
            contextMock.Expect(o => o.User).Returns(mockUser.Object);
            c.ControllerContext = new ControllerContext(contextMock.Object, new RouteData(), c);
            Assert.AreEqual<IPrincipal>(mockUser.Object, c.User, "Property should equal the value on the Context.");
        }

        [TestMethod]
        public void RouteDataProperty() {
            var c = new EmptyController();
            Assert.IsNull(c.RouteData, "Property should be null before Context is set");
            Mock<HttpContextBase> httpContextMock = new Mock<HttpContextBase>();
            RouteData routedata = new RouteData();
            c.ControllerContext = new ControllerContext(httpContextMock.Object, routedata, c);
            Assert.AreEqual<RouteData>(routedata, c.RouteData, "Property should equal the value on the Context.");

        }

        [TestMethod]
        public void ViewDataDictionaryIsCaseInsensitive() {
            // DevDiv Bugs 195982: The ViewData dictionary property on Controller should be case insensitive

            // Setup
            Controller controller = new EmptyController();
            object o = new object();

            // Execute
            controller.ViewData["Foo"] = o;

            // Verify
            Assert.AreSame(o, controller.ViewData["FOO"]);
        }

        [TestMethod]
        public void ViewDataProperty() {
            var controller = new EmptyController();
            // test returns empty dictionary by default
            IDictionary<string, object> viewData = controller.ViewData;
            Assert.AreNotEqual(viewData, null);
            Assert.AreEqual(viewData.Count, 0);
        }

        [TestMethod]
        public void ViewEngineProperty() {
            Mock<IViewEngine> viewEngineMock = new Mock<IViewEngine>();
            MemberHelper.TestPropertyValue(new EmptyController(), "ViewEngine", viewEngineMock.Object);
        }

        [TestMethod]
        public void ViewEnginePropertyCachesWebFormViewEngine() {
            // DevDiv Bugs 194190: When using the default view engine, cache the instance so we don't keep
            // returning a new instance every time.

            // Setup
            Controller controller = new EmptyController();

            // Execute
            IViewEngine viewEngine1 = controller.ViewEngine;
            IViewEngine viewEngine2 = controller.ViewEngine;

            // Verify
            Assert.AreSame(viewEngine1, viewEngine2);
        }

        [TestMethod]
        public void ViewEnginePropertyGetReturnsWebFormViewEnginebyDefault() {
            // Setup
            Controller controller = new EmptyController();

            // Execute
            IViewEngine viewEngine = controller.ViewEngine;

            // Verify
            Assert.IsInstanceOfType(viewEngine, typeof(WebFormViewEngine));
        }

        [TestMethod]
        public void ViewEnginePropertySetToNullThrows() {
            var controller = new EmptyController();
            ExceptionHelper.ExpectArgumentNullException(
                delegate {
                    controller.ViewEngine = null;
                },
                "value");
        }

        [TestMethod]
        public void ControllerMethodsDoNotHaveNonActionAttribute() {
            var methods = typeof(Controller).GetMethods(BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
            foreach (var method in methods) {
                var attrs = method.GetCustomAttributes(typeof(NonActionAttribute), true /* inherit */);
                Assert.AreEqual(0, attrs.Length, "Methods on the Controller class should not be marked [NonAction]: " + method);
            }
        }

        [TestMethod]
        public void ExecuteWithUnknownAction() {
            // Setup
            UnknownActionController controller = new UnknownActionController();
            ControllerContext context = GetControllerContext("Foo");

            Mock<ControllerActionInvoker> mockInvoker = new Mock<ControllerActionInvoker>(context);
            mockInvoker.Expect(o => o.InvokeAction("Foo", null)).Returns(false);
            controller.ActionInvoker = mockInvoker.Object;

            // Execute
            controller.Execute(context);

            // Verify
            Assert.IsTrue(controller.WasCalled);
        }

        [TestMethod]
        public void ExecuteWithNullContextThrows() {
            var controller = new EmptyController();
            ExceptionHelper.ExpectArgumentNullException(
                delegate {
                    controller.Execute(null);
                },
                "controllerContext");
        }

        [TestMethod]
        public void HandleUnknownActionThrows() {
            var controller = new EmptyController();
            ExceptionHelper.ExpectException<HttpException>(
                delegate {
                    controller.HandleUnknownAction("UnknownAction");
                },
                "A public action method 'UnknownAction' could not be found on controller 'System.Web.Mvc.Test.ControllerTest+EmptyController'.");
        }

        [TestMethod]
        public void RedirectToActionThrowsOnNullOrEmptyStringParameters() {
            // Use reflection to get all RedirectToAction overloads, then hammer away at the string parameters.
            MemberInfo[] memberInfos = typeof(Controller).GetMember("RedirectToAction", BindingFlags.Instance | BindingFlags.NonPublic);
            foreach (MethodInfo mi in memberInfos) {
                foreach (ParameterInfo parameterInfo in mi.GetParameters()) {
                    if (parameterInfo.ParameterType == typeof(string)) {
                        EnsureMethodThrowsOnNullOrEmptyStringParameter(mi, parameterInfo.Name);
                    }
                }
            }
        }

        [TestMethod]
        public void RedirectToActionWithActionNameAndControllerName() {
            // Setup
            Controller controller = GetEmptyController();

            // Execute
            ActionRedirectResult result = controller.RedirectToAction("MyAction", "MyController");

            // Verify
            Assert.AreEqual(2, result.Values.Count);
            Assert.AreEqual("MyAction", result.Values["Action"]);
            Assert.AreEqual("MyController", result.Values["Controller"]);
        }

        [TestMethod]
        public void RedirectToActionWithActionNameAndObjectDictionary() {
            // Setup
            Controller controller = GetEmptyController();
            var values = new { Foo = "MyFoo" };

            // Execute
            ActionRedirectResult result = controller.RedirectToAction("MyAction", values);

            // Verify
            Assert.AreEqual(2, result.Values.Count);
            Assert.AreEqual("MyFoo", result.Values["Foo"]);
            Assert.AreEqual("MyAction", result.Values["Action"]);
        }

        [TestMethod]
        public void RedirectToActionWithActionNameAndObjectDictionaryThrowsOnDuplicateAction() {
            // Setup
            Controller controller = GetEmptyController();
            var values = new { Action = "SomeAction" };

            // Execute & verify
            ExceptionHelper.ExpectArgumentException(
                delegate {
                    ActionRedirectResult result = controller.RedirectToAction("MyAction", values);
                },
                "The provided object or dictionary already contains a definition for 'action'.\r\nParameter name: actionName");
        }

        [TestMethod]
        public void RedirectToActionWithActionNameAndNullRouteValueDictionary() {
            // Setup
            Controller controller = GetEmptyController();

            // Execute
            ActionRedirectResult result = controller.RedirectToAction("MyAction", (RouteValueDictionary)null);

            // Verify
            Assert.AreEqual(1, result.Values.Count);
            Assert.AreEqual("MyAction", result.Values["Action"]);
        }

        [TestMethod]
        public void RedirectToActionWithActionNameAndRouteValueDictionary() {
            // Setup
            Controller controller = GetEmptyController();
            RouteValueDictionary values = new RouteValueDictionary() { { "Foo", "MyFoo" } };

            // Execute
            ActionRedirectResult result = controller.RedirectToAction("MyAction", values);

            // Verify
            Assert.AreEqual(2, result.Values.Count);
            Assert.AreEqual("MyFoo", result.Values["Foo"]);
            Assert.AreEqual("MyAction", result.Values["Action"]);
            Assert.AreNotSame(values, result.Values);
        }

        [TestMethod]
        public void RedirectToActionWithActionNameAndRouteValueDictionaryThrowsOnDuplicateAction() {
            // Setup
            Controller controller = GetEmptyController();
            RouteValueDictionary values = new RouteValueDictionary() { { "Action", "SomeAction" } };

            // Execute & verify
            ExceptionHelper.ExpectArgumentException(
                delegate {
                    ActionRedirectResult result = controller.RedirectToAction("MyAction", values);
                },
                "The provided object or dictionary already contains a definition for 'action'.\r\nParameter name: actionName");
        }

        [TestMethod]
        public void RedirectToActionWithActionNameControllerNameAndNullRouteValueDictionary() {
            // Setup
            Controller controller = GetEmptyController();

            // Execute
            ActionRedirectResult result = controller.RedirectToAction("MyAction", "MyController", (RouteValueDictionary)null);

            // Verify
            Assert.AreEqual(2, result.Values.Count);
            Assert.AreEqual("MyAction", result.Values["Action"]);
            Assert.AreEqual("MyController", result.Values["Controller"]);
        }

        [TestMethod]
        public void RedirectToActionWithActionNameControllerNameAndObjectDictionary() {
            // Setup
            Controller controller = GetEmptyController();
            var values = new { Foo = "MyFoo" };

            // Execute
            ActionRedirectResult result = controller.RedirectToAction("MyAction", "MyController", values);

            // Verify
            Assert.AreEqual(3, result.Values.Count);
            Assert.AreEqual("MyFoo", result.Values["Foo"]);
            Assert.AreEqual("MyAction", result.Values["Action"]);
            Assert.AreEqual("MyController", result.Values["Controller"]);
        }

        [TestMethod]
        public void RedirectToActionWithActionNameControllerNameAndObjectDictionaryThrowsOnDuplicateAction() {
            // Setup
            Controller controller = GetEmptyController();
            var values = new { Action = "SomeAction" };

            // Execute & verify
            ExceptionHelper.ExpectArgumentException(
                delegate {
                    ActionRedirectResult result = controller.RedirectToAction("MyAction", "MyController", values);
                },
                "The provided object or dictionary already contains a definition for 'action'.\r\nParameter name: actionName");
        }

        [TestMethod]
        public void RedirectToActionWithActionNameControllerNameAndObjectDictionaryThrowsOnDuplicateController() {
            // Setup
            Controller controller = GetEmptyController();
            var values = new { Controller = "SomeController" };

            // Execute & verify
            ExceptionHelper.ExpectArgumentException(
                delegate {
                    ActionRedirectResult result = controller.RedirectToAction("MyAction", "MyController", values);
                },
                "The provided object or dictionary already contains a definition for 'controller'.\r\nParameter name: controllerName");
        }

        [TestMethod]
        public void RedirectToActionWithActionNameControllerNameAndRouteValueDictionary() {
            // Setup
            Controller controller = GetEmptyController();
            RouteValueDictionary values = new RouteValueDictionary() { { "Foo", "MyFoo" } };

            // Execute
            ActionRedirectResult result = controller.RedirectToAction("MyAction", "MyController", values);

            // Verify
            Assert.AreEqual(3, result.Values.Count);
            Assert.AreEqual("MyFoo", result.Values["Foo"]);
            Assert.AreEqual("MyAction", result.Values["Action"]);
            Assert.AreEqual("MyController", result.Values["Controller"]);
            Assert.AreNotSame(values, result.Values);
        }

        [TestMethod]
        public void RedirectToActionWithActionNameControllerNameAndRouteValueDictionaryThrowsOnDuplicateAction() {
            // Setup
            Controller controller = GetEmptyController();
            RouteValueDictionary values = new RouteValueDictionary() { { "Action", "SomeAction" } };

            // Execute & verify
            ExceptionHelper.ExpectArgumentException(
                delegate {
                    ActionRedirectResult result = controller.RedirectToAction("MyAction", "MyController", values);
                },
                "The provided object or dictionary already contains a definition for 'action'.\r\nParameter name: actionName");
        }

        [TestMethod]
        public void RedirectToActionWithActionNameControllerNameAndRouteValueDictionaryThrowsOnDuplicateController() {
            // Setup
            Controller controller = GetEmptyController();
            RouteValueDictionary values = new RouteValueDictionary() { { "Controller", "SomeController" } };

            // Execute & verify
            ExceptionHelper.ExpectArgumentException(
                delegate {
                    ActionRedirectResult result = controller.RedirectToAction("MyAction", "MyController", values);
                },
                "The provided object or dictionary already contains a definition for 'controller'.\r\nParameter name: controllerName");
        }

        [TestMethod]
        public void RedirectToActionWithNullRouteValueDictionary() {
            // Setup
            Controller controller = GetEmptyController();

            // Execute
            ActionRedirectResult result = controller.RedirectToAction((RouteValueDictionary)null);

            // Verify
            Assert.AreEqual(0, result.Values.Count);
        }

        [TestMethod]
        public void RedirectToActionWithObjectDictionary() {
            // Setup
            Controller controller = GetEmptyController();
            var values = new { Foo = "MyFoo" };

            // Execute
            ActionRedirectResult result = controller.RedirectToAction(values);

            // Verify
            Assert.AreEqual(1, result.Values.Count);
            Assert.AreEqual("MyFoo", result.Values["Foo"]);
        }

        [TestMethod]
        public void RedirectToActionWithRouteValueDictionary() {
            // Setup
            Controller controller = GetEmptyController();
            RouteValueDictionary values = new RouteValueDictionary() { { "Foo", "MyFoo" } };

            // Execute
            ActionRedirectResult result = controller.RedirectToAction(values);

            // Verify
            Assert.AreEqual(1, result.Values.Count);
            Assert.AreEqual("MyFoo", result.Values["Foo"]);
            Assert.AreNotSame(values, result.Values);
        }

        private void RedirectToActionHelper(Controller controller, string action, string appPath, bool useMvcExtensions, string expectedRedirect) {
            Mock<HttpContextBase> mockContext = new Mock<HttpContextBase>();
            Mock<HttpResponseBase> mockResponse = new Mock<HttpResponseBase>();
            Mock<HttpRequestBase> mockRequest = new Mock<HttpRequestBase>();
            mockRequest.Expect(o => o.ApplicationPath).Returns(appPath);
            mockResponse.Expect(o => o.Redirect(expectedRedirect));
            RouteCollection rc = new RouteCollection();
            rc.Add(new Route(useMvcExtensions ? "{controller}.mvc/{action}/{id}" : "{controller}/{action}/{id}", null) { Defaults = new RouteValueDictionary(new { action = "Index", id = (string)null }) });
            controller.RouteCollection = rc;
            mockContext.Expect(o => o.Response).Returns(mockResponse.Object);
            mockContext.Expect(o => o.Request).Returns(mockRequest.Object);
            mockContext.Expect(o => o.Session).Returns((HttpSessionStateBase)null);
            RouteData routeData = new RouteData();
            routeData.Values.Add("controller", controller.GetType().Name.Substring(0, controller.GetType().Name.Length - 10));
            routeData.Values.Add("action", action);
            routeData.Values.Add("id", null);
            controller.Execute(new ControllerContext(mockContext.Object, routeData, controller));
            mockResponse.Verify();
        }

        [TestMethod]
        public void RedirectToActionWithStandardControllerName() {
            RedirectController controller = new RedirectController();
            RedirectToActionHelper(controller, "GotoShowPerson", "appPath/", true, "appPath/Redirect.mvc/ShowPerson");
        }

        [TestMethod]
        public void RedirectToActionUsingMvcExtension() {
            SampleController controller = new SampleController();
            RedirectToActionHelper(controller, "GotoShowPerson", "appPath/", false, "appPath/Sample/ShowPerson");
        }

        [TestMethod]
        public void RedirectToActionWithoutMvcExtension() {
            SampleController controller = new SampleController();
            RedirectToActionHelper(controller, "GotoShowPerson", "appPath/", true, "appPath/Sample.mvc/ShowPerson");
        }

        [TestMethod]
        public void RedirectToActionWithAppPathEndingWithBackSlash() {
            RedirectController controller = new RedirectController();
            RedirectToActionHelper(controller, "GotoShowPerson", "appPath/", false, "appPath/Redirect/ShowPerson");
        }

        [TestMethod]
        public void RedirectToActionWithDifferentController() {
            SampleController controller = new SampleController();
            RedirectToActionHelper(controller, "GotoShowPersonWithController", "appPath/", false, "appPath/Cart/ShowPerson");
        }

        [TestMethod]
        public void RedirectToActionWithDifferentControllerAndId() {
            SampleController controller = new SampleController();
            RedirectToActionHelper(controller, "GotoShowPersonWithValues", "appPath/", false, "appPath/Cart/ShowPerson/123");
        }

        [TestMethod]
        public void RedirectReturnsCorrectActionResult() {
            // Setup
            Controller controller = GetEmptyController();

            // Execute & verify
            var result = controller.Redirect("http://www.contoso.com/");

            // Verify
            Assert.AreEqual("http://www.contoso.com/", result.Url);
        }

        [TestMethod]
        public void RedirectWithEmptyUrlThrows() {
            // Setup
            Controller controller = GetEmptyController();

            // Execute & verify
            ExceptionHelper.ExpectArgumentExceptionNullOrEmpty(
                delegate {
                    controller.Redirect(String.Empty);
                },
                "url");
        }

        [TestMethod]
        public void RedirectWithNullUrlThrows() {
            // Setup
            Controller controller = GetEmptyController();

            // Execute & verify
            ExceptionHelper.ExpectArgumentExceptionNullOrEmpty(
                delegate {
                    controller.Redirect(null /* url */);
                },
                "url");
        }

        [TestMethod]
        public void RenderView0_SetsProperties() {
            // Setup
            Controller controller = GetEmptyController();

            // Execute
            RenderViewResult result = controller.RenderView();

            // Verify
            Assert.IsNull(result.ViewName);
            Assert.IsNull(result.MasterName);
            Assert.AreSame(controller.ViewData, result.ViewData);
            Assert.AreSame(controller.ViewEngine, result.ViewEngine);
            Assert.AreSame(controller.TempData, result.TempData);
        }

        [TestMethod]
        public void RenderView1_obj_SetsProperties() {
            // Setup
            Controller controller = GetEmptyController();
            object o = new object();

            // Execute
            RenderViewResult result = controller.RenderView(o);

            // Verify
            Assert.IsNull(result.ViewName);
            Assert.IsNull(result.MasterName);
            Assert.AreSame(o, result.ViewData);
            Assert.AreSame(controller.ViewEngine, result.ViewEngine);
            Assert.AreSame(controller.TempData, result.TempData);
        }

        [TestMethod]
        public void RenderView1_str_SetsProperties() {
            // Setup
            Controller controller = GetEmptyController();

            // Execute
            RenderViewResult result = controller.RenderView("Foo");

            // Verify
            Assert.AreEqual("Foo", result.ViewName);
            Assert.IsNull(result.MasterName);
            Assert.AreSame(controller.ViewData, result.ViewData);
            Assert.AreSame(controller.ViewEngine, result.ViewEngine);
            Assert.AreSame(controller.TempData, result.TempData);
        }

        [TestMethod]
        public void RenderView2_str_obj_SetsProperties() {
            // Setup
            Controller controller = GetEmptyController();
            object o = new object();

            // Execute
            RenderViewResult result = controller.RenderView("Foo", o);

            // Verify
            Assert.AreEqual("Foo", result.ViewName);
            Assert.IsNull(result.MasterName);
            Assert.AreSame(o, result.ViewData);
            Assert.AreSame(controller.ViewEngine, result.ViewEngine);
            Assert.AreSame(controller.TempData, result.TempData);
        }

        [TestMethod]
        public void RenderView2_str_str_SetsProperties() {
            // Setup
            Controller controller = GetEmptyController();

            // Execute
            RenderViewResult result = controller.RenderView("Foo", "Bar");

            // Verify
            Assert.AreEqual("Foo", result.ViewName);
            Assert.AreEqual("Bar", result.MasterName);
            Assert.AreSame(controller.ViewData, result.ViewData);
            Assert.AreSame(controller.ViewEngine, result.ViewEngine);
            Assert.AreSame(controller.TempData, result.TempData);
        }

        [TestMethod]
        public void RenderView3_str_str_obj_SetsProperties() {
            // Setup
            Controller controller = GetEmptyController();
            object o = new object();

            // Execute
            RenderViewResult result = controller.RenderView("Foo", "Bar", o);

            // Verify
            Assert.AreEqual("Foo", result.ViewName);
            Assert.AreEqual("Bar", result.MasterName);
            Assert.AreSame(o, result.ViewData);
            Assert.AreSame(controller.ViewEngine, result.ViewEngine);
            Assert.AreSame(controller.TempData, result.TempData);
        }

        internal static void AddRequestParams(Mock<HttpRequestBase> requestMock, object paramValues) {
            PropertyDescriptorCollection props = TypeDescriptor.GetProperties(paramValues);
            foreach (PropertyDescriptor prop in props) {
                requestMock.Expect(o => o[It.Is<string>(item => String.Equals(prop.Name, item, StringComparison.OrdinalIgnoreCase))]).Returns((string)prop.GetValue(paramValues));
            }
        }

        [TestMethod]
        public void TempDataMovedToPreviousTempDataInDestinationController() {
            // Setup
            Mock<Controller> mockController = new Mock<Controller>();
            Mock<HttpContextBase> mockContext = new Mock<HttpContextBase>();
            HttpSessionStateBase session = GetEmptySession();
            mockContext.Expect(o => o.Session).Returns(session);
            mockController.Object.ControllerContext = new ControllerContext(mockContext.Object, new RouteData(), mockController.Object);
            mockController.Object.TempData = new TempDataDictionary(mockController.Object.HttpContext);

            // Execute
            mockController.Object.TempData["Key"] = "Value";

            // Verify
            Assert.IsTrue(mockController.Object.TempData.ContainsKey("Key"), "The key should still exist in the old TempData");

            // Instantiate "destination" controller with the same session state and see that it gets the temp data
            Mock<Controller> mockDestinationController = new Mock<Controller>();
            Mock<HttpContextBase> mockDestinationContext = new Mock<HttpContextBase>();
            mockDestinationContext.Expect(o => o.Session).Returns(session);
            mockDestinationController.Object.ControllerContext = new ControllerContext(mockDestinationContext.Object, new RouteData(), mockDestinationController.Object);
            mockDestinationController.Object.TempData = new TempDataDictionary(mockDestinationController.Object.HttpContext);

            // Verify
            Assert.AreEqual("Value", mockDestinationController.Object.TempData["Key"], "The key should exist in the new TempData");

            // Execute
            mockDestinationController.Object.TempData["NewKey"] = "NewValue";
            Assert.AreEqual("NewValue", mockDestinationController.Object.TempData["NewKey"], "The new key should exist in the new TempData");

            // Instantiate "second destination" controller with the same session state and see that it gets the temp data
            Mock<Controller> mockSecondDestinationController = new Mock<Controller>();
            Mock<HttpContextBase> mockSecondDestinationContext = new Mock<HttpContextBase>();
            mockSecondDestinationContext.Expect(o => o.Session).Returns(session);
            mockSecondDestinationController.Object.ControllerContext = new ControllerContext(mockSecondDestinationContext.Object, new RouteData(), mockSecondDestinationController.Object);
            mockSecondDestinationController.Object.TempData = new TempDataDictionary(mockSecondDestinationController.Object.HttpContext);

            // Verify
            Assert.IsFalse(mockSecondDestinationController.Object.TempData.ContainsKey("Key"), "The key should not exist in the new TempData");
            Assert.AreEqual("NewValue", mockSecondDestinationController.Object.TempData["NewKey"], "The new key should exist in the new TempData");
        }

        [TestMethod]
        public void TempDataValidForSingleControllerWhenSessionStateDisabled() {
            // Setup
            Mock<Controller> mockController = new Mock<Controller>();
            Mock<HttpContextBase> mockContext = new Mock<HttpContextBase>();
            HttpSessionStateBase session = null;
            mockContext.Expect(o => o.Session).Returns(session);
            mockController.Object.ControllerContext = new ControllerContext(mockContext.Object, new RouteData(), mockController.Object);
            mockController.Object.TempData = new TempDataDictionary(mockController.Object.HttpContext);

            // Execute
            mockController.Object.TempData["Key"] = "Value";

            // Verify
            Assert.IsTrue(mockController.Object.TempData.ContainsKey("Key"), "The key should exist in TempData, even with SessionState disabled.");
        }

        private static ControllerContext GetControllerContext(string actionName) {
            Mock<HttpContextBase> mockContext = new Mock<HttpContextBase>();
            mockContext.Expect(o => o.Session).Returns((HttpSessionStateBase)null);
            RouteData rd = new RouteData();
            rd.Values["action"] = actionName;
            return new ControllerContext(mockContext.Object, rd, new Mock<IController>().Object);
        }

        private HttpContextBase GetMockHttpContextWithSession() {
            Hashtable sessionData = new Hashtable(StringComparer.OrdinalIgnoreCase);
            Mock<HttpContextBase> mockContext = new Mock<HttpContextBase>();
            mockContext.Expect(o => o.Session).Returns(GetEmptySession());
            return mockContext.Object;
        }

        private static Controller GetEmptyController() {
            ControllerContext context = GetControllerContext("Foo");
            var controller = new EmptyController() {
                ControllerContext = context,
                RouteCollection = new RouteCollection(),
                TempData = new TempDataDictionary(context.HttpContext)
            };
            return controller;
        }

        private static HttpSessionStateBase GetEmptySession() {
            HttpSessionStateMock mockSession = new HttpSessionStateMock();
            return mockSession;
        }

        private static void EnsureMethodThrowsOnNullOrEmptyStringParameter(MethodInfo methodInfo, string parameterName) {
            // Setup
            Dictionary<Type, object> defaults = new Dictionary<Type, object>() {
                { typeof(string), "Default string" },
                { typeof(object), new object() },
                { typeof(RouteValueDictionary), new RouteValueDictionary() }
            };

            // Locate the parameter of interest
            ParameterInfo[] parameterInfos = methodInfo.GetParameters();
            ParameterInfo targetParameter;
            try {
                targetParameter = parameterInfos.Single(pi => pi.Name == parameterName);
            }
            catch (InvalidOperationException ex) {
                throw new Exception(String.Format("The method '{0}' doesn't contain a parameter named '{1}'.", methodInfo, parameterName), ex);
            }

            // Invoke the method
            Controller controller = GetEmptyController();
            object[] parameters = parameterInfos.Select(pi => (pi == targetParameter) ? null : defaults[pi.ParameterType]).ToArray();
            try {
                methodInfo.Invoke(controller, parameters);
            }
            catch (TargetInvocationException ex) {
                Assert.IsInstanceOfType(ex.InnerException, typeof(ArgumentException),
                    String.Format("The method '{0}' didn't throw an ArgumentException when given a null parameter '{1}'.", methodInfo, parameterName));
                Assert.AreEqual("Value cannot be null or empty.\r\nParameter name: " + parameterName, ex.InnerException.Message);
            }
            parameters = parameterInfos.Select(pi => (pi == targetParameter) ? String.Empty : defaults[pi.ParameterType]).ToArray();
            try {
                methodInfo.Invoke(controller, parameters);
            }
            catch (TargetInvocationException ex) {
                Assert.IsInstanceOfType(ex.InnerException, typeof(ArgumentException),
                    String.Format("The method '{0}' didn't throw an ArgumentException when given an empty parameter '{1}'.", methodInfo, parameterName));
                Assert.AreEqual("Value cannot be null or empty.\r\nParameter name: " + parameterName, ex.InnerException.Message);
            }
        }

        private sealed class HttpSessionStateMock : HttpSessionStateBase {
            private Hashtable _sessionData = new Hashtable(StringComparer.OrdinalIgnoreCase);

            public override void Remove(string name) {
                Assert.AreEqual<string>(TempDataDictionary.TempDataSessionStateKey, name);
                _sessionData.Remove(name);
            }

            public override object this[string name] {
                get {
                    Assert.AreEqual<string>(TempDataDictionary.TempDataSessionStateKey, name);
                    return _sessionData[name];
                }
                set {
                    Assert.AreEqual<string>(TempDataDictionary.TempDataSessionStateKey, name);
                    _sessionData[name] = value;
                }
            }
        }

        public class RedirectController : Controller {
            public ActionResult GotoShowPerson() {
                return RedirectToAction("ShowPerson");
            }
        }

        public class RenderViewController : Controller {
            public ViewContext ResultViewContext { get; set; }

            public RenderViewController() {
                ViewEngine = new MyViewEngine(this);
            }

            [NonAction]
            public void RenderViewPublic(string viewName) {
                RenderView(viewName);
            }

            [NonAction]
            public void RenderViewPublic(string viewName, object data) {
                RenderView(viewName, data);
            }

            [NonAction]
            public void RenderViewPublic(string viewName, string layoutName) {
                RenderView(viewName, layoutName);
            }

            [NonAction]
            public void RenderViewPublic(string viewName, string layoutName, object data) {
                RenderView(viewName, layoutName, data);
            }

            private sealed class MyViewEngine : IViewEngine {
                private RenderViewController _rvc;

                public MyViewEngine(RenderViewController rvc) {
                    _rvc = rvc;
                }

                #region IViewEngine Members
                public void RenderView(ViewContext viewContext) {
                    _rvc.ResultViewContext = viewContext;
                }
                #endregion
            }
        }

        [CLSCompliant(false)]
        public class SampleController : Controller {
            private IDictionary<string, object> _results = new Dictionary<string, object>();

            public SampleController() {
                Mock<HttpContextBase> mockHttpContext = new Mock<HttpContextBase>();
                ControllerContext = new ControllerContext(mockHttpContext.Object, new RouteData(), this);
            }

            public IViewEngine ViewEngineToUse {
                get;
                set;
            }

            public IDictionary<string, object> Results {
                get {
                    return _results;
                }
            }

            public bool DiceyActionThrows {
                get;
                set;
            }

            public bool OnErrorHandled {
                get;
                set;
            }

            public bool DiscontinueAction {
                get;
                set;
            }

            public void ValidAction() {
                Results["ValidActionCalled"] = true;
            }
            [NonAction]
            public void validAction() {
                // Conflicting method name with NonActionAttribute
            }
            public void ConflictingAction() {
                Results["ConflictingActionCalled"] = true;
            }
            public void conflictingAction() {
                Results["conflictingActionCalled"] = true;
            }

            public void DiceyAction() {
                Results["DiceyActionCalled"] = true;
                Results["DiceyActionCalledAfterOnActionExecuting"] = TryGetValue(Results, "OnActionExecutingCalled", false);
                if (DiceyActionThrows) {
                    throw new Exception();
                }
            }
            private void PrivateAction() {
                Results["PrivateActionCalled"] = true;
            }
            internal void InternalAction() {
                Results["InternalActionCalled"] = true;
            }
            protected void ProtectedAction() {
                Results["ProtectedActionCalled"] = true;
            }

            public void ShowStuff() {
                Results["ShowStuffActionCalled"] = true;
                ViewData["data"] = "somedata";
                ViewEngine = ViewEngineToUse;
                RenderView("ShowStuffView");
            }

            public void ShowPerson() {
                Results["ShowPersonActionCalled"] = true;
                Person person = new Person() { Name = "Bart", Age = 9 };
                ViewEngine = ViewEngineToUse;
                RenderView("ShowPersonView", person);
            }

            public ActionResult GotoShowPerson() {
                return RedirectToAction("ShowPerson");
            }

            public ActionResult GotoShowPersonWithController() {
                return RedirectToAction("ShowPerson", new { controller = "Cart" });
            }

            public ActionResult GotoShowPersonWithValues() {
                return RedirectToAction(new RouteValueDictionary(new { Action = "ShowPerson", Controller = "Cart", Id = "123" }));
            }

            [NonAction]
            public void SomeNonActionMethod() {
                Results["SomeNonActionMethodCalled"] = true;
            }
            protected override void HandleUnknownAction(string actionName) {
                Results["HandleUnknownActionCalled"] = true;
                Results["UnknownActionName"] = actionName;
            }

            //protected override void RenderView(string viewName, string layoutName, object viewData) {
            //    Results["ViewNameInRenderView"] = viewName;
            //    Results["ViewDataInRenderView"] = viewData;
            //    base.RenderView(viewName, layoutName, viewData);
            //}

            private object TryGetValue(IDictionary<string, object> dict, string key, object defaultValue) {
                return dict.ContainsKey(key) ? dict[key] : defaultValue;
            }
        }

        public class Person {
            public string Name { get; set; }
            public int Age { get; set; }
        }

        private class EmptyController : Controller {
            public new void HandleUnknownAction(string actionName) {
                base.HandleUnknownAction(actionName);
            }
        }

        private sealed class UnknownActionController : Controller {
            public bool WasCalled;

            protected override void HandleUnknownAction(string actionName) {
                WasCalled = true;
            }
        }
    }
}
