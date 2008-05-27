namespace System.Web.Mvc.Test {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Reflection;
    using System.Security.Principal;
    using System.Text;
    using System.Web.Routing;
    using System.Web.TestUtil;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;

    [TestClass]
    public class ControllerTest {

        [TestMethod]
        public void ContentWithContentString() {
            // Setup
            Controller controller = new EmptyController();
            string content = "Some content";

            // Execute
            ContentResult result = controller.Content(content);

            // Verify
            Assert.AreEqual(content, result.Content);
        }

        public void ContentWithContentStringAndContentType() {
            // Setup
            Controller controller = new EmptyController();
            string content = "Some content";
            string contentType = "Some content type";

            // Execute
            ContentResult result = controller.Content(content, contentType);

            // Verify
            Assert.AreEqual(content, result.Content);
            Assert.AreEqual(contentType, result.ContentType);
        }

        public void ContentWithContentStringAndContentTypeAndEncoding() {
            // Setup
            Controller controller = new EmptyController();
            string content = "Some content";
            string contentType = "Some content type";
            Encoding contentEncoding = Encoding.UTF8;

            // Execute
            ContentResult result = controller.Content(content, contentType, contentEncoding);

            // Verify
            Assert.AreEqual(content, result.Content);
            Assert.AreEqual(contentType, result.ContentType);
            Assert.AreSame(contentEncoding, result.ContentEncoding);
        }

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
        public void SessionProperty() {
            var c = new EmptyController();
            Assert.IsNull(c.Session, "Property should be null before Context is set");

            Mock<HttpContextBase> contextMock = new Mock<HttpContextBase>();
            Mock<HttpSessionStateBase> sessionMock = new Mock<HttpSessionStateBase>();
            contextMock.Expect(o => o.Session).Returns(sessionMock.Object);
            c.ControllerContext = new ControllerContext(contextMock.Object, new RouteData(), c);
            Assert.AreSame(sessionMock.Object, c.Session, "Property should equal the value on the Context.");
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
        public void RedirectToActionClonesRouteValueDictionary() {
            // The RedirectToAction() method should clone the provided dictionary, then operate on the clone.
            // The original dictionary should remain unmodified throughout the helper's execution.

            // Setup
            Controller controller = GetEmptyController();
            RouteValueDictionary values = new RouteValueDictionary(new { Action = "SomeAction", Controller = "SomeController" });

            // Execute
            controller.RedirectToAction("SomeOtherAction", "SomeOtherController", values);

            // Verify
            Assert.AreEqual(2, values.Count);
            Assert.AreEqual("SomeAction", values["action"]);
            Assert.AreEqual("SomeController", values["controller"]);
        }

        [TestMethod]
        public void RedirectToActionPreservesControllerDictionaryKeyIfNotSpecified() {
            // Setup
            Controller controller = GetEmptyController();
            object values = new { Controller = "SomeController" };

            // Execute
            RedirectToRouteResult result = controller.RedirectToAction("SomeOtherAction", values);
            RouteValueDictionary newValues = result.Values;

            // Verify
            Assert.AreEqual("SomeController", newValues["controller"]);
        }

        [TestMethod]
        public void RedirectToActionOverwritesActionDictionaryKey() {
            // Setup
            Controller controller = GetEmptyController();
            object values = new { Action = "SomeAction" };

            // Execute
            RedirectToRouteResult result = controller.RedirectToAction("SomeOtherAction", values);
            RouteValueDictionary newValues = result.Values;

            // Verify
            Assert.AreEqual("SomeOtherAction", newValues["action"]);
        }

        [TestMethod]
        public void RedirectToActionOverwritesControllerDictionaryKeyIfSpecified() {
            // Setup
            Controller controller = GetEmptyController();
            object values = new { Action = "SomeAction", Controller = "SomeController" };

            // Execute
            RedirectToRouteResult result = controller.RedirectToAction("SomeOtherAction", "SomeOtherController", values);
            RouteValueDictionary newValues = result.Values;

            // Verify
            Assert.AreEqual("SomeOtherController", newValues["controller"]);
        }

        [TestMethod]
        public void RedirectToActionWithActionName() {
            // Setup
            Controller controller = GetEmptyController();

            // Execute
            RedirectToRouteResult result = controller.RedirectToAction("SomeOtherAction");

            // Verify
            Assert.AreEqual("", result.RouteName);
            Assert.AreEqual("SomeOtherAction", result.Values["action"]);
        }

        [TestMethod]
        public void RedirectToActionWithActionNameAndControllerName() {
            // Setup
            Controller controller = GetEmptyController();

            // Execute
            RedirectToRouteResult result = controller.RedirectToAction("SomeOtherAction", "SomeOtherController");

            // Verify
            Assert.AreEqual("", result.RouteName);
            Assert.AreEqual("SomeOtherAction", result.Values["action"]);
            Assert.AreEqual("SomeOtherController", result.Values["controller"]);
        }

        [TestMethod]
        public void RedirectToActionWithActionNameAndControllerNameAndValuesDictionary() {
            // Setup
            Controller controller = GetEmptyController();
            RouteValueDictionary values = new RouteValueDictionary(new { Foo = "SomeFoo" });

            // Execute
            RedirectToRouteResult result = controller.RedirectToAction("SomeOtherAction", "SomeOtherController", values);

            // Verify
            Assert.AreEqual("", result.RouteName);
            Assert.AreEqual("SomeOtherAction", result.Values["action"]);
            Assert.AreEqual("SomeOtherController", result.Values["controller"]);
            Assert.AreEqual("SomeFoo", result.Values["foo"]);
        }

        [TestMethod]
        public void RedirectToActionWithActionNameAndControllerNameAndValuesObject() {
            // Setup
            Controller controller = GetEmptyController();
            object values = new { Foo = "SomeFoo" };

            // Execute
            RedirectToRouteResult result = controller.RedirectToAction("SomeOtherAction", "SomeOtherController", values);

            // Verify
            Assert.AreEqual("", result.RouteName);
            Assert.AreEqual("SomeOtherAction", result.Values["action"]);
            Assert.AreEqual("SomeOtherController", result.Values["controller"]);
            Assert.AreEqual("SomeFoo", result.Values["foo"]);
        }

        [TestMethod]
        public void RedirectToActionWithActionNameAndValuesDictionary() {
            // Setup
            Controller controller = GetEmptyController();
            RouteValueDictionary values = new RouteValueDictionary(new { Foo = "SomeFoo" });

            // Execute
            RedirectToRouteResult result = controller.RedirectToAction("SomeOtherAction", values);

            // Verify
            Assert.AreEqual("", result.RouteName);
            Assert.AreEqual("SomeOtherAction", result.Values["action"]);
            Assert.AreEqual("SomeFoo", result.Values["foo"]);
        }

        [TestMethod]
        public void RedirectToActionWithActionNameAndValuesObject() {
            // Setup
            Controller controller = GetEmptyController();
            object values = new { Foo = "SomeFoo" };

            // Execute
            RedirectToRouteResult result = controller.RedirectToAction("SomeOtherAction", values);

            // Verify
            Assert.AreEqual("", result.RouteName);
            Assert.AreEqual("SomeOtherAction", result.Values["action"]);
            Assert.AreEqual("SomeFoo", result.Values["foo"]);
        }

        [TestMethod]
        public void RedirectToActionWithEmptyActionNameThrows() {
            // Setup
            Controller controller = GetEmptyController();
            
            // Execute & verify
            ExceptionHelper.ExpectArgumentExceptionNullOrEmpty(
                delegate {
                    controller.RedirectToAction(String.Empty);
                }, "actionName");
        }

        [TestMethod]
        public void RedirectToActionWithNullActionNameThrows() {
            // Setup
            Controller controller = GetEmptyController();

            // Execute & verify
            ExceptionHelper.ExpectArgumentExceptionNullOrEmpty(
                delegate {
                    controller.RedirectToAction(null /* actionName */);
                }, "actionName");
        }

        [TestMethod]
        public void RedirectToActionWithNullRouteValueDictionary() {
            // Setup
            Controller controller = GetEmptyController();

            // Execute
            RedirectToRouteResult result=controller.RedirectToAction("SomeOtherAction", (RouteValueDictionary)null);
            RouteValueDictionary newValues = result.Values;

            // Verify
            Assert.AreEqual(1, newValues.Count);
            Assert.AreEqual("SomeOtherAction", newValues["action"]);
        }

        [TestMethod]
        public void RedirectToRouteWithNullRouteValueDictionary() {
            // Setup
            Controller controller = GetEmptyController();

            // Execute
            RedirectToRouteResult result = controller.RedirectToRoute((RouteValueDictionary)null);

            // Verify
            Assert.AreEqual(0, result.Values.Count);
        }

        [TestMethod]
        public void RedirectToRouteWithObjectDictionary() {
            // Setup
            Controller controller = GetEmptyController();
            var values = new { Foo = "MyFoo" };

            // Execute
            RedirectToRouteResult result = controller.RedirectToRoute(values);

            // Verify
            Assert.AreEqual(1, result.Values.Count);
            Assert.AreEqual("MyFoo", result.Values["Foo"]);
        }

        [TestMethod]
        public void RedirectToRouteWithRouteValueDictionary() {
            // Setup
            Controller controller = GetEmptyController();
            RouteValueDictionary values = new RouteValueDictionary() { { "Foo", "MyFoo" } };

            // Execute
            RedirectToRouteResult result = controller.RedirectToRoute(values);

            // Verify
            Assert.AreEqual(1, result.Values.Count);
            Assert.AreEqual("MyFoo", result.Values["Foo"]);
            Assert.AreNotSame(values, result.Values);
        }

        [TestMethod]
        public void RedirectToRouteWithName() {
            // Setup
            Controller controller = GetEmptyController();

            // Execute
            RedirectToRouteResult result = controller.RedirectToRoute("foo");

            // Verify
            Assert.AreEqual(0, result.Values.Count);
            Assert.AreEqual("foo", result.RouteName);
        }

        [TestMethod]
        public void RedirectToRouteWithNameAndNullRouteValueDictionary() {
            // Setup
            Controller controller = GetEmptyController();

            // Execute
            RedirectToRouteResult result = controller.RedirectToRoute("foo", (RouteValueDictionary)null);

            // Verify
            Assert.AreEqual(0, result.Values.Count);
            Assert.AreEqual("foo", result.RouteName);
        }

        [TestMethod]
        public void RedirectToRouteWithNullNameAndNullRouteValueDictionary() {
            // Setup
            Controller controller = GetEmptyController();

            // Execute
            RedirectToRouteResult result = controller.RedirectToRoute(null, (RouteValueDictionary)null);

            // Verify
            Assert.AreEqual(0, result.Values.Count);
            Assert.AreEqual(String.Empty, result.RouteName);
        }

        [TestMethod]
        public void RedirectToRouteWithNameAndObjectDictionary() {
            // Setup
            Controller controller = GetEmptyController();
            var values = new { Foo = "MyFoo" };

            // Execute
            RedirectToRouteResult result = controller.RedirectToRoute("foo", values);

            // Verify
            Assert.AreEqual(1, result.Values.Count);
            Assert.AreEqual("MyFoo", result.Values["Foo"]);
            Assert.AreEqual("foo", result.RouteName);
        }

        [TestMethod]
        public void RedirectToRouteWithNameAndRouteValueDictionary() {
            // Setup
            Controller controller = GetEmptyController();
            RouteValueDictionary values = new RouteValueDictionary() { { "Foo", "MyFoo" } };

            // Execute
            RedirectToRouteResult result = controller.RedirectToRoute("foo", values);

            // Verify
            Assert.AreEqual(1, result.Values.Count);
            Assert.AreEqual("MyFoo", result.Values["Foo"]);
            Assert.AreNotSame(values, result.Values);
            Assert.AreEqual("foo", result.RouteName);
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
            ViewResult result = controller.View();

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
            object viewItem = new object();

            // Execute
            ViewResult result = controller.View(viewItem);

            // Verify
            Assert.IsNull(result.ViewName);
            Assert.IsNull(result.MasterName);
            Assert.AreSame(controller.ViewEngine, result.ViewEngine);
            Assert.AreSame(controller.TempData, result.TempData);
        }

        [TestMethod]
        public void RenderView1_str_SetsProperties() {
            // Setup
            Controller controller = GetEmptyController();

            // Execute
            ViewResult result = controller.View("Foo");

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
            object viewItem = new object();

            // Execute
            ViewResult result = controller.View("Foo", viewItem);

            // Verify
            Assert.AreEqual("Foo", result.ViewName);
            Assert.IsNull(result.MasterName);
            Assert.AreSame(viewItem, result.ViewData.Model);
            Assert.AreSame(controller.ViewEngine, result.ViewEngine);
            Assert.AreSame(controller.TempData, result.TempData);
        }

        [TestMethod]
        public void RenderView2_str_str_SetsProperties() {
            // Setup
            Controller controller = GetEmptyController();

            // Execute
            ViewResult result = controller.View("Foo", "Bar");

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
            object viewItem = new object();

            // Execute
            ViewResult result = controller.View("Foo", "Bar", viewItem);

            // Verify
            Assert.AreEqual("Foo", result.ViewName);
            Assert.AreEqual("Bar", result.MasterName);
            Assert.AreSame(viewItem, result.ViewData.Model);
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
