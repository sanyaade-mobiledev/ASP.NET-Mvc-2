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
    using Moq.Protected;

    [TestClass]
    public class ControllerTest {

        [TestMethod]
        public void ActionInvokerProperty() {
            // Arrange
            Controller controller = new EmptyController();

            // Act & Assert
            MemberHelper.TestPropertyWithDefaultInstance(controller, "ActionInvoker", new ControllerActionInvoker());
        }

        [TestMethod]
        public void ContentWithContentString() {
            // Arrange
            Controller controller = new EmptyController();
            string content = "Some content";

            // Act
            ContentResult result = controller.Content(content);

            // Assert
            Assert.AreEqual(content, result.Content);
        }

        public void ContentWithContentStringAndContentType() {
            // Arrange
            Controller controller = new EmptyController();
            string content = "Some content";
            string contentType = "Some content type";

            // Act
            ContentResult result = controller.Content(content, contentType);

            // Assert
            Assert.AreEqual(content, result.Content);
            Assert.AreEqual(contentType, result.ContentType);
        }

        public void ContentWithContentStringAndContentTypeAndEncoding() {
            // Arrange
            Controller controller = new EmptyController();
            string content = "Some content";
            string contentType = "Some content type";
            Encoding contentEncoding = Encoding.UTF8;

            // Act
            ContentResult result = controller.Content(content, contentType, contentEncoding);

            // Assert
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
        public void ModelStateProperty() {
            // Arrange
            Controller controller = new EmptyController();

            // Act & assert
            Assert.AreSame(controller.ViewData.ModelState, controller.ModelState);
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
        public void UrlProperty() {
            // Arrange
            EmptyController controller = new EmptyController();
            RequestContext requestContext = new RequestContext(new Mock<HttpContextBase>().Object, new RouteData());

            // Act
            controller.PublicInitialize(requestContext);

            // Assert
            Assert.IsNotNull(controller.Url);
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
        public void ControllerMethodsDoNotHaveNonActionAttribute() {
            var methods = typeof(Controller).GetMethods(BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
            foreach (var method in methods) {
                var attrs = method.GetCustomAttributes(typeof(NonActionAttribute), true /* inherit */);
                Assert.AreEqual(0, attrs.Length, "Methods on the Controller class should not be marked [NonAction]: " + method);
            }
        }

        [TestMethod]
        public void DisposeCallsProtectedDisposingMethod() {
            // Arrange
            Mock<Controller> mockController = new Mock<Controller>();
            mockController.Protected().Expect("Dispose", true).Verifiable();
            Controller controller = mockController.Object;

            // Act
            controller.Dispose();

            // Assert
            mockController.Verify();
        }

        [TestMethod]
        public void ExecuteWithUnknownAction() {
            // Arrange
            UnknownActionController controller = new UnknownActionController();
            // We need a provider since Controller.Execute is called
            controller.TempDataProvider = new EmptyTempDataProvider();
            ControllerContext context = GetControllerContext("Foo");

            Mock<IActionInvoker> mockInvoker = new Mock<IActionInvoker>();
            mockInvoker.Expect(o => o.InvokeAction(context, "Foo")).Returns(false);
            controller.ActionInvoker = mockInvoker.Object;

            // Act
            ((IController)controller).Execute(context);

            // Assert
            Assert.IsTrue(controller.WasCalled);
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
        public void PartialView() {
            // Arrange
            Controller controller = GetEmptyController();

            // Act
            PartialViewResult result = controller.PartialView();

            // Assert
            Assert.AreSame(controller.TempData, result.TempData);
            Assert.AreSame(controller.ViewData, result.ViewData);
        }

        [TestMethod]
        public void PartialView_Model() {
            // Arrange
            Controller controller = GetEmptyController();
            object model = new object();

            // Act
            PartialViewResult result = controller.PartialView(model);

            // Assert
            Assert.AreSame(model, result.ViewData.Model);
            Assert.AreSame(controller.TempData, result.TempData);
            Assert.AreSame(controller.ViewData, result.ViewData);
        }

        [TestMethod]
        public void PartialView_ViewName() {
            // Arrange
            Controller controller = GetEmptyController();

            // Act
            PartialViewResult result = controller.PartialView("Some partial view");

            // Assert
            Assert.AreEqual("Some partial view", result.ViewName);
            Assert.AreSame(controller.TempData, result.TempData);
            Assert.AreSame(controller.ViewData, result.ViewData);
        }

        [TestMethod]
        public void PartialView_ViewName_Model() {
            // Arrange
            Controller controller = GetEmptyController();
            object model = new object();

            // Act
            PartialViewResult result = controller.PartialView("Some partial view", model);

            // Assert
            Assert.AreEqual("Some partial view", result.ViewName);
            Assert.AreSame(model, result.ViewData.Model);
            Assert.AreSame(controller.TempData, result.TempData);
            Assert.AreSame(controller.ViewData, result.ViewData);
        }

        [TestMethod]
        public void RedirectToActionClonesRouteValueDictionary() {
            // The RedirectToAction() method should clone the provided dictionary, then operate on the clone.
            // The original dictionary should remain unmodified throughout the helper's execution.

            // Arrange
            Controller controller = GetEmptyController();
            RouteValueDictionary values = new RouteValueDictionary(new { Action = "SomeAction", Controller = "SomeController" });

            // Act
            controller.RedirectToAction("SomeOtherAction", "SomeOtherController", values);

            // Assert
            Assert.AreEqual(2, values.Count);
            Assert.AreEqual("SomeAction", values["action"]);
            Assert.AreEqual("SomeController", values["controller"]);
        }

        [TestMethod]
        public void RedirectToActionPreservesControllerDictionaryKeyIfNotSpecified() {
            // Arrange
            Controller controller = GetEmptyController();
            object values = new { Controller = "SomeController" };

            // Act
            RedirectToRouteResult result = controller.RedirectToAction("SomeOtherAction", values);
            RouteValueDictionary newValues = result.Values;

            // Assert
            Assert.AreEqual("SomeController", newValues["controller"]);
        }

        [TestMethod]
        public void RedirectToActionDoesNotOverwritesActionDictionaryKey() {
            // Arrange
            Controller controller = GetEmptyController();
            object values = new { Action = "SomeAction" };

            // Act
            RedirectToRouteResult result = controller.RedirectToAction("SomeOtherAction", values);
            RouteValueDictionary newValues = result.Values;

            // Assert
            Assert.AreEqual("SomeAction", newValues["action"]);
        }

        [TestMethod]
        public void RedirectToActionDoesNotOverwritesControllerDictionaryKeyIfSpecified() {
            // Arrange
            Controller controller = GetEmptyController();
            object values = new { Action = "SomeAction", Controller = "SomeController" };

            // Act
            RedirectToRouteResult result = controller.RedirectToAction("SomeOtherAction", "SomeOtherController", values);
            RouteValueDictionary newValues = result.Values;

            // Assert
            Assert.AreEqual("SomeController", newValues["controller"]);
        }

        [TestMethod]
        public void RedirectToActionWithActionName() {
            // Arrange
            Controller controller = GetEmptyController();

            // Act
            RedirectToRouteResult result = controller.RedirectToAction("SomeOtherAction");

            // Assert
            Assert.AreEqual("", result.RouteName);
            Assert.AreEqual("SomeOtherAction", result.Values["action"]);
        }

        [TestMethod]
        public void RedirectToActionWithActionNameAndControllerName() {
            // Arrange
            Controller controller = GetEmptyController();

            // Act
            RedirectToRouteResult result = controller.RedirectToAction("SomeOtherAction", "SomeOtherController");

            // Assert
            Assert.AreEqual("", result.RouteName);
            Assert.AreEqual("SomeOtherAction", result.Values["action"]);
            Assert.AreEqual("SomeOtherController", result.Values["controller"]);
        }

        [TestMethod]
        public void RedirectToActionWithActionNameAndControllerNameAndValuesDictionary() {
            // Arrange
            Controller controller = GetEmptyController();
            RouteValueDictionary values = new RouteValueDictionary(new { Foo = "SomeFoo" });

            // Act
            RedirectToRouteResult result = controller.RedirectToAction("SomeOtherAction", "SomeOtherController", values);

            // Assert
            Assert.AreEqual("", result.RouteName);
            Assert.AreEqual("SomeOtherAction", result.Values["action"]);
            Assert.AreEqual("SomeOtherController", result.Values["controller"]);
            Assert.AreEqual("SomeFoo", result.Values["foo"]);
        }

        [TestMethod]
        public void RedirectToActionWithActionNameAndControllerNameAndValuesObject() {
            // Arrange
            Controller controller = GetEmptyController();
            object values = new { Foo = "SomeFoo" };

            // Act
            RedirectToRouteResult result = controller.RedirectToAction("SomeOtherAction", "SomeOtherController", values);

            // Assert
            Assert.AreEqual("", result.RouteName);
            Assert.AreEqual("SomeOtherAction", result.Values["action"]);
            Assert.AreEqual("SomeOtherController", result.Values["controller"]);
            Assert.AreEqual("SomeFoo", result.Values["foo"]);
        }

        [TestMethod]
        public void RedirectToActionSelectsCurrentControllerByDefault() {
            // Arrange
            TestRouteController controller = new TestRouteController();
            controller.ControllerContext = GetControllerContext("SomeAction", "TestRoute");

            // Act
            RedirectToRouteResult route = controller.Index() as RedirectToRouteResult;

            // Assert
            Assert.AreEqual("SomeAction", route.Values["action"]);
            Assert.AreEqual("TestRoute", route.Values["controller"]);
        }

        [TestMethod]
        public void RedirectToActionDictionaryOverridesDefaultControllerName() {
            // Arrange
            TestRouteController controller = new TestRouteController();
            object values = new { controller = "SomeOtherController" };
            controller.ControllerContext = GetControllerContext("SomeAction", "TestRoute");

            // Act
            RedirectToRouteResult route = controller.RedirectToAction("SomeOtherAction", values);

            // Assert
            Assert.AreEqual("SomeOtherAction", route.Values["action"]);
            Assert.AreEqual("SomeOtherController", route.Values["controller"]);
        }

        [TestMethod]
        public void RedirectToActionWithActionNameAndValuesDictionary() {
            // Arrange
            Controller controller = GetEmptyController();
            RouteValueDictionary values = new RouteValueDictionary(new { Foo = "SomeFoo" });

            // Act
            RedirectToRouteResult result = controller.RedirectToAction("SomeOtherAction", values);

            // Assert
            Assert.AreEqual("", result.RouteName);
            Assert.AreEqual("SomeOtherAction", result.Values["action"]);
            Assert.AreEqual("SomeFoo", result.Values["foo"]);
        }

        [TestMethod]
        public void RedirectToActionWithActionNameAndValuesObject() {
            // Arrange
            Controller controller = GetEmptyController();
            object values = new { Foo = "SomeFoo" };

            // Act
            RedirectToRouteResult result = controller.RedirectToAction("SomeOtherAction", values);

            // Assert
            Assert.AreEqual("", result.RouteName);
            Assert.AreEqual("SomeOtherAction", result.Values["action"]);
            Assert.AreEqual("SomeFoo", result.Values["foo"]);
        }

        [TestMethod]
        public void RedirectToActionWithEmptyActionNameThrows() {
            // Arrange
            Controller controller = GetEmptyController();

            // Act & Assert
            ExceptionHelper.ExpectArgumentExceptionNullOrEmpty(
                delegate {
                    controller.RedirectToAction(String.Empty);
                }, "actionName");
        }

        [TestMethod]
        public void RedirectToActionWithNullActionNameThrows() {
            // Arrange
            Controller controller = GetEmptyController();

            // Act & Assert
            ExceptionHelper.ExpectArgumentExceptionNullOrEmpty(
                delegate {
                    controller.RedirectToAction(null /* actionName */);
                }, "actionName");
        }

        [TestMethod]
        public void RedirectToActionWithNullRouteValueDictionary() {
            // Arrange
            Controller controller = GetEmptyController();

            // Act
            RedirectToRouteResult result = controller.RedirectToAction("SomeOtherAction", (RouteValueDictionary)null);
            RouteValueDictionary newValues = result.Values;

            // Assert
            Assert.AreEqual(1, newValues.Count);
            Assert.AreEqual("SomeOtherAction", newValues["action"]);
        }

        [TestMethod]
        public void RedirectToRouteWithNullRouteValueDictionary() {
            // Arrange
            Controller controller = GetEmptyController();

            // Act
            RedirectToRouteResult result = controller.RedirectToRoute((RouteValueDictionary)null);

            // Assert
            Assert.AreEqual(0, result.Values.Count);
        }

        [TestMethod]
        public void RedirectToRouteWithObjectDictionary() {
            // Arrange
            Controller controller = GetEmptyController();
            var values = new { Foo = "MyFoo" };

            // Act
            RedirectToRouteResult result = controller.RedirectToRoute(values);

            // Assert
            Assert.AreEqual(1, result.Values.Count);
            Assert.AreEqual("MyFoo", result.Values["Foo"]);
        }

        [TestMethod]
        public void RedirectToRouteWithRouteValueDictionary() {
            // Arrange
            Controller controller = GetEmptyController();
            RouteValueDictionary values = new RouteValueDictionary() { { "Foo", "MyFoo" } };

            // Act
            RedirectToRouteResult result = controller.RedirectToRoute(values);

            // Assert
            Assert.AreEqual(1, result.Values.Count);
            Assert.AreEqual("MyFoo", result.Values["Foo"]);
            Assert.AreNotSame(values, result.Values);
        }

        [TestMethod]
        public void RedirectToRouteWithName() {
            // Arrange
            Controller controller = GetEmptyController();

            // Act
            RedirectToRouteResult result = controller.RedirectToRoute("foo");

            // Assert
            Assert.AreEqual(0, result.Values.Count);
            Assert.AreEqual("foo", result.RouteName);
        }

        [TestMethod]
        public void RedirectToRouteWithNameAndNullRouteValueDictionary() {
            // Arrange
            Controller controller = GetEmptyController();

            // Act
            RedirectToRouteResult result = controller.RedirectToRoute("foo", (RouteValueDictionary)null);

            // Assert
            Assert.AreEqual(0, result.Values.Count);
            Assert.AreEqual("foo", result.RouteName);
        }

        [TestMethod]
        public void RedirectToRouteWithNullNameAndNullRouteValueDictionary() {
            // Arrange
            Controller controller = GetEmptyController();

            // Act
            RedirectToRouteResult result = controller.RedirectToRoute(null, (RouteValueDictionary)null);

            // Assert
            Assert.AreEqual(0, result.Values.Count);
            Assert.AreEqual(String.Empty, result.RouteName);
        }

        [TestMethod]
        public void RedirectToRouteWithNameAndObjectDictionary() {
            // Arrange
            Controller controller = GetEmptyController();
            var values = new { Foo = "MyFoo" };

            // Act
            RedirectToRouteResult result = controller.RedirectToRoute("foo", values);

            // Assert
            Assert.AreEqual(1, result.Values.Count);
            Assert.AreEqual("MyFoo", result.Values["Foo"]);
            Assert.AreEqual("foo", result.RouteName);
        }

        [TestMethod]
        public void RedirectToRouteWithNameAndRouteValueDictionary() {
            // Arrange
            Controller controller = GetEmptyController();
            RouteValueDictionary values = new RouteValueDictionary() { { "Foo", "MyFoo" } };

            // Act
            RedirectToRouteResult result = controller.RedirectToRoute("foo", values);

            // Assert
            Assert.AreEqual(1, result.Values.Count);
            Assert.AreEqual("MyFoo", result.Values["Foo"]);
            Assert.AreNotSame(values, result.Values);
            Assert.AreEqual("foo", result.RouteName);
        }

        [TestMethod]
        public void RedirectReturnsCorrectActionResult() {
            // Arrange
            Controller controller = GetEmptyController();

            // Act & Assert
            var result = controller.Redirect("http://www.contoso.com/");

            // Assert
            Assert.AreEqual("http://www.contoso.com/", result.Url);
        }

        [TestMethod]
        public void RedirectWithEmptyUrlThrows() {
            // Arrange
            Controller controller = GetEmptyController();

            // Act & Assert
            ExceptionHelper.ExpectArgumentExceptionNullOrEmpty(
                delegate {
                    controller.Redirect(String.Empty);
                },
                "url");
        }

        [TestMethod]
        public void RedirectWithNullUrlThrows() {
            // Arrange
            Controller controller = GetEmptyController();

            // Act & Assert
            ExceptionHelper.ExpectArgumentExceptionNullOrEmpty(
                delegate {
                    controller.Redirect(null /* url */);
                },
                "url");
        }

        [TestMethod]
        public void RenderView0_SetsProperties() {
            // Arrange
            Controller controller = GetEmptyController();

            // Act
            ViewResult result = controller.View();

            // Assert
            Assert.AreSame(controller.ViewData, result.ViewData);
            Assert.AreSame(controller.TempData, result.TempData);
        }

        [TestMethod]
        public void RenderView1_obj_SetsProperties() {
            // Arrange
            Controller controller = GetEmptyController();
            object viewItem = new object();

            // Act
            ViewResult result = controller.View(viewItem);

            // Assert
            Assert.AreSame(viewItem, result.ViewData.Model);
            Assert.AreSame(controller.TempData, result.TempData);
        }

        [TestMethod]
        public void RenderView1_str_SetsProperties() {
            // Arrange
            Controller controller = GetEmptyController();

            // Act
            ViewResult result = controller.View("Foo");

            // Assert
            Assert.AreEqual("Foo", result.ViewName);
            Assert.AreSame(controller.ViewData, result.ViewData);
            Assert.AreSame(controller.TempData, result.TempData);
        }

        [TestMethod]
        public void RenderView2_str_obj_SetsProperties() {
            // Arrange
            Controller controller = GetEmptyController();
            object viewItem = new object();

            // Act
            ViewResult result = controller.View("Foo", viewItem);

            // Assert
            Assert.AreEqual("Foo", result.ViewName);
            Assert.AreSame(viewItem, result.ViewData.Model);
            Assert.AreSame(controller.TempData, result.TempData);
        }

        [TestMethod]
        public void RenderView2_str_str_SetsProperties() {
            // Arrange
            Controller controller = GetEmptyController();

            // Act
            ViewResult result = controller.View("Foo", "Bar");

            // Assert
            Assert.AreEqual("Foo", result.ViewName);
            Assert.AreEqual("Bar", result.MasterName);
            Assert.AreSame(controller.ViewData, result.ViewData);
            Assert.AreSame(controller.TempData, result.TempData);
        }

        [TestMethod]
        public void RenderView3_str_str_obj_SetsProperties() {
            // Arrange
            Controller controller = GetEmptyController();
            object viewItem = new object();

            // Act
            ViewResult result = controller.View("Foo", "Bar", viewItem);

            // Assert
            Assert.AreEqual("Foo", result.ViewName);
            Assert.AreEqual("Bar", result.MasterName);
            Assert.AreSame(viewItem, result.ViewData.Model);
            Assert.AreSame(controller.TempData, result.TempData);
        }

        [TestMethod]
        public void RenderView4_view_SetsProperties() {
            // Arrange
            Controller controller = GetEmptyController();
            IView view = new Mock<IView>().Object;

            // Act
            ViewResult result = controller.View(view);

            // Assert
            Assert.AreSame(result.View, view);
            Assert.AreSame(controller.ViewData, result.ViewData);
            Assert.AreSame(controller.TempData, result.TempData);
        }

        [TestMethod]
        public void RenderView5_view_obj_SetsProperties() {
            // Arrange
            Controller controller = GetEmptyController();
            IView view = new Mock<IView>().Object;
            object model = new object();

            // Act
            ViewResult result = controller.View(view, model);

            // Assert
            Assert.AreSame(result.View, view);
            Assert.AreSame(controller.ViewData, result.ViewData);
            Assert.AreSame(controller.TempData, result.TempData);
            Assert.AreSame(model, result.ViewData.Model);
        }

        internal static void AddRequestParams(Mock<HttpRequestBase> requestMock, object paramValues) {
            PropertyDescriptorCollection props = TypeDescriptor.GetProperties(paramValues);
            foreach (PropertyDescriptor prop in props) {
                requestMock.Expect(o => o[It.Is<string>(item => String.Equals(prop.Name, item, StringComparison.OrdinalIgnoreCase))]).Returns((string)prop.GetValue(paramValues));
            }
        }

        [TestMethod]
        public void TempDataGreetUserWithNoUserIDRedirects() {
            // Arrange
            TempDataHomeController tempDataHomeController = new TempDataHomeController();

            // Act
            RedirectToRouteResult result = tempDataHomeController.GreetUser() as RedirectToRouteResult;
            RouteValueDictionary values = result.Values;

            // Assert
            Assert.IsTrue(values.ContainsKey("action"));
            Assert.AreEqual("ErrorPage", values["action"]);
            Assert.AreEqual(0, tempDataHomeController.TempData.Count);
        }

        [TestMethod]
        public void TempDataGreetUserWithUserIDCopiesToViewDataAndRenders() {
            // Arrange
            TempDataHomeController tempDataHomeController = new TempDataHomeController();
            tempDataHomeController.TempData["UserID"] = "TestUserID";

            // Act
            ViewResult result = tempDataHomeController.GreetUser() as ViewResult;
            ViewDataDictionary viewData = tempDataHomeController.ViewData;

            // Assert
            Assert.AreEqual("GreetUser", result.ViewName);
            Assert.IsNotNull(viewData);
            Assert.IsTrue(viewData.ContainsKey("NewUserID"));
            Assert.AreEqual("TestUserID", viewData["NewUserID"]);
        }

        [TestMethod]
        public void TempDataIndexSavesUserIDAndRedirects() {
            // Arrange
            TempDataHomeController tempDataHomeController = new TempDataHomeController();

            // Act
            RedirectToRouteResult result = tempDataHomeController.Index() as RedirectToRouteResult;
            RouteValueDictionary values = result.Values;

            // Assert
            Assert.IsTrue(values.ContainsKey("action"));
            Assert.AreEqual("GreetUser", values["action"]);

            Assert.IsTrue(tempDataHomeController.TempData.ContainsKey("UserID"));
            Assert.AreEqual("user123", tempDataHomeController.TempData["UserID"]);
        }

        [TestMethod]
        public void TempDataSavedWhenControllerThrows() {
            // Arrange
            BrokenController controller = new BrokenController();
            Mock<HttpContextBase> mockContext = new Mock<HttpContextBase>();
            HttpSessionStateBase session = GetEmptySession();
            mockContext.Expect(o => o.Session).Returns(session);
            RouteData rd = new RouteData();
            rd.Values.Add("action", "Crash");
            controller.ControllerContext = new ControllerContext(mockContext.Object, rd, controller);

            // Assert
            ExceptionHelper.ExpectException<InvalidOperationException>(
                delegate {
                    ((IController)controller).Execute(controller.ControllerContext);
                });
            Assert.AreNotEqual(mockContext.Object.Session[SessionStateTempDataProvider.TempDataSessionStateKey], null);
            TempDataDictionary tempData = new TempDataDictionary();
            tempData.Load(controller.ControllerContext, controller.TempDataProvider);
            Assert.AreEqual(tempData["Key1"], "Value1");
        }

        [TestMethod]
        public void TempDataMovedToPreviousTempDataInDestinationController() {
            // Arrange
            Mock<Controller> mockController = new Mock<Controller>() { CallBase = true };
            Mock<HttpContextBase> mockContext = new Mock<HttpContextBase>();
            HttpSessionStateBase session = GetEmptySession();
            mockContext.Expect(o => o.Session).Returns(session);
            mockController.Object.ControllerContext = new ControllerContext(mockContext.Object, new RouteData(), mockController.Object);

            // Act
            mockController.Object.TempData.Add("Key", "Value");
            mockController.Object.TempData.Save(mockController.Object.ControllerContext, mockController.Object.TempDataProvider);

            // Assert
            Assert.IsTrue(mockController.Object.TempData.ContainsKey("Key"), "The key should still exist in the old TempData");
            Assert.IsTrue(mockController.Object.TempData.ContainsValue("Value"), "The value should still exist in the old TempData");

            // Instantiate "destination" controller with the same session state and see that it gets the temp data
            Mock<Controller> mockDestinationController = new Mock<Controller>() { CallBase = true };
            Mock<HttpContextBase> mockDestinationContext = new Mock<HttpContextBase>();
            mockDestinationContext.Expect(o => o.Session).Returns(session);
            mockDestinationController.Object.ControllerContext = new ControllerContext(mockDestinationContext.Object, new RouteData(), mockDestinationController.Object);
            mockDestinationController.Object.TempData.Load(mockDestinationController.Object.ControllerContext, mockDestinationController.Object.TempDataProvider);

            // Assert
            Assert.AreEqual("Value", mockDestinationController.Object.TempData["Key"], "The key should exist in the new TempData");

            // Act
            mockDestinationController.Object.TempData["NewKey"] = "NewValue";
            Assert.AreEqual("NewValue", mockDestinationController.Object.TempData["NewKey"], "The new key should exist in the new TempData");
            mockDestinationController.Object.TempData.Save(mockDestinationController.Object.ControllerContext, mockDestinationController.Object.TempDataProvider);

            // Instantiate "second destination" controller with the same session state and see that it gets the temp data
            Mock<Controller> mockSecondDestinationController = new Mock<Controller>() { CallBase = true };
            Mock<HttpContextBase> mockSecondDestinationContext = new Mock<HttpContextBase>();
            mockSecondDestinationContext.Expect(o => o.Session).Returns(session);
            mockSecondDestinationController.Object.ControllerContext = new ControllerContext(mockSecondDestinationContext.Object, new RouteData(), mockSecondDestinationController.Object);
            mockSecondDestinationController.Object.TempData.Load(mockSecondDestinationController.Object.ControllerContext, mockSecondDestinationController.Object.TempDataProvider);

            // Assert
            Assert.IsFalse(mockSecondDestinationController.Object.TempData.ContainsKey("Key"), "The key should not exist in the new TempData");
            Assert.AreEqual("NewValue", mockSecondDestinationController.Object.TempData["NewKey"], "The new key should exist in the new TempData");
        }

        [TestMethod]
        public void TempDataValidForSingleControllerWhenSessionStateDisabled() {
            // Arrange
            Mock<Controller> mockController = new Mock<Controller>();
            Mock<HttpContextBase> mockContext = new Mock<HttpContextBase>();
            HttpSessionStateBase session = null;
            mockContext.Expect(o => o.Session).Returns(session);
            mockController.Object.ControllerContext = new ControllerContext(mockContext.Object, new RouteData(), mockController.Object);
            mockController.Object.TempData = new TempDataDictionary();

            // Act
            mockController.Object.TempData["Key"] = "Value";

            // Assert
            Assert.IsTrue(mockController.Object.TempData.ContainsKey("Key"), "The key should exist in TempData, even with SessionState disabled.");
        }

        [TestMethod]
        public void TryUpdateModelCallsModelBinderForModel() {
            // Arrange
            MyModel myModel = new MyModelSubclassed();
            IValueProvider valueProvider = new Mock<IValueProvider>().Object;

            Controller controller = new EmptyController();
            controller.ControllerContext = GetControllerContext("someAction");

            // Act
            bool returned = controller.TryUpdateModel(myModel, "somePrefix", new[] { "prop1", "prop2" }, null, valueProvider);

            // Assert
            Assert.IsTrue(returned);
            Assert.AreEqual(valueProvider, myModel.ValueProvider);
            Assert.AreEqual("somePrefix", myModel.ModelName);
            Assert.AreEqual(controller.ModelState, myModel.ModelState);
            Assert.AreEqual(typeof(MyModel), myModel.ModelType);
            Assert.IsTrue(myModel.PropertyFilter("prop1"), "Incorrect filter applied.");
            Assert.IsTrue(myModel.PropertyFilter("prop2"), "Incorrect filter applied.");
            Assert.IsFalse(myModel.PropertyFilter("prop3"), "Incorrect filter applied.");
        }

        [TestMethod]
        public void TryUpdateModelReturnsFalseIfModelStateInvalid() {
            // Arrange
            MyModel myModel = new MyModelSubclassed();
            IValueProvider valueProvider = new Mock<IValueProvider>().Object;

            Controller controller = new EmptyController();
            controller.ControllerContext = GetControllerContext("someAction");
            controller.ModelState.AddModelError("key", "some exception message");

            // Act
            bool returned = controller.TryUpdateModel(myModel);

            // Assert
            Assert.IsFalse(returned);
        }

        [TestMethod]
        public void TryUpdateModelSuppliesControllerValueProviderIfNoValueProviderSpecified() {
            // Arrange
            MyModel myModel = new MyModelSubclassed();
            IValueProvider valueProvider = new Mock<IValueProvider>().Object;

            Controller controller = new EmptyController();
            controller.ControllerContext = GetControllerContext("someAction");
            controller.ValueProvider = valueProvider;

            // Act
            bool returned = controller.TryUpdateModel(myModel, "somePrefix", new[] { "prop1", "prop2" });

            // Assert
            Assert.IsTrue(returned);
            Assert.AreEqual(valueProvider, myModel.ValueProvider);
        }

        [TestMethod]
        public void TryUpdateModelSuppliesEmptyModelNameIfNoPrefixSpecified() {
            // Arrange
            MyModel myModel = new MyModelSubclassed();
            IValueProvider valueProvider = new Mock<IValueProvider>().Object;

            Controller controller = new EmptyController();
            controller.ControllerContext = GetControllerContext("someAction");

            // Act
            bool returned = controller.TryUpdateModel(myModel, new[] { "prop1", "prop2" });

            // Assert
            Assert.IsTrue(returned);
            Assert.AreEqual(String.Empty, myModel.ModelName);
        }

        [TestMethod]
        public void TryUpdateModelThrowsIfModelIsNull() {
            // Arrange
            Controller controller = new EmptyController();

            // Act & Assert
            ExceptionHelper.ExpectArgumentNullException(
                delegate {
                    controller.TryUpdateModel<object>(null);
                }, "model");
        }

        [TestMethod]
        public void TryUpdateModelThrowsIfValueProviderIsNull() {
            // Arrange
            Controller controller = new EmptyController();

            // Act & Assert
            ExceptionHelper.ExpectArgumentNullException(
                delegate {
                    controller.TryUpdateModel(new object(), null, null, null, null);
                }, "valueProvider");
        }

        [TestMethod]
        public void UpdateModelReturnsIfModelStateValid() {
            // Arrange
            MyModel myModel = new MyModelSubclassed();
            IValueProvider valueProvider = new Mock<IValueProvider>().Object;

            Controller controller = new EmptyController();
            controller.ControllerContext = GetControllerContext("someAction");

            // Act
            controller.UpdateModel(myModel);

            // Assert
            // nothing to do - if we got here, the test passed
        }

        [TestMethod]
        public void TryUpdateModelWithoutBindPropertiesImpliesAllPropertiesAreUpdateable() {
            // Arrange
            MyModel myModel = new MyModelSubclassed();
            IValueProvider valueProvider = new Mock<IValueProvider>().Object;

            Controller controller = new EmptyController();
            controller.ControllerContext = GetControllerContext("someAction");

            // Act
            bool returned = controller.TryUpdateModel(myModel, "somePrefix");

            // Assert
            Assert.IsTrue(returned);
            Assert.IsTrue(myModel.PropertyFilter("prop1"), "Incorrect filter applied.");
            Assert.IsTrue(myModel.PropertyFilter("prop2"), "Incorrect filter applied.");
            Assert.IsTrue(myModel.PropertyFilter("prop3"), "Incorrect filter applied.");
        }

        [TestMethod]
        public void UpdateModelSuppliesControllerValueProviderIfNoValueProviderSpecified() {
            // Arrange
            MyModel myModel = new MyModelSubclassed();
            IValueProvider valueProvider = new Mock<IValueProvider>().Object;

            Controller controller = new EmptyController();
            controller.ControllerContext = GetControllerContext("someAction");
            controller.ValueProvider = valueProvider;

            // Act
            controller.UpdateModel(myModel, "somePrefix", new[] { "prop1", "prop2" });

            // Assert
            Assert.AreEqual(valueProvider, myModel.ValueProvider);
        }

        [TestMethod]
        public void UpdateModelThrowsIfModelStateInvalid() {
            // Arrange
            MyModel myModel = new MyModelSubclassed();
            IValueProvider valueProvider = new Mock<IValueProvider>().Object;

            Controller controller = new EmptyController();
            controller.ControllerContext = GetControllerContext("someAction");
            controller.ModelState.AddModelError("key", "some exception message");

            // Act & assert
            ExceptionHelper.ExpectInvalidOperationException(
                delegate {
                    controller.UpdateModel(myModel);
                },
                "The model of type 'System.Web.Mvc.Test.ControllerTest+MyModel' was not successfully updated.");
        }

        [TestMethod]
        public void UpdateModelWithoutBindPropertiesImpliesAllPropertiesAreUpdateable() {
            // Arrange
            MyModel myModel = new MyModelSubclassed();
            IValueProvider valueProvider = new Mock<IValueProvider>().Object;

            Controller controller = new EmptyController();
            controller.ControllerContext = GetControllerContext("someAction");

            // Act
            controller.UpdateModel(myModel, "somePrefix");

            // Assert
            Assert.IsTrue(myModel.PropertyFilter("prop1"), "Incorrect filter applied.");
            Assert.IsTrue(myModel.PropertyFilter("prop2"), "Incorrect filter applied.");
            Assert.IsTrue(myModel.PropertyFilter("prop3"), "Incorrect filter applied.");
        }

        private static ControllerContext GetControllerContext(string actionName) {
            Mock<HttpContextBase> mockContext = new Mock<HttpContextBase>();
            mockContext.Expect(o => o.Session).Returns((HttpSessionStateBase)null);
            RouteData rd = new RouteData();
            rd.Values["action"] = actionName;
            return new ControllerContext(mockContext.Object, rd, new Mock<ControllerBase>().Object);
        }

        private static ControllerContext GetControllerContext(string actionName, string controllerName) {
            Mock<HttpContextBase> mockContext = new Mock<HttpContextBase>();
            mockContext.Expect(o => o.Session).Returns((HttpSessionStateBase)null);
            RouteData rd = new RouteData();
            rd.Values["action"] = actionName;
            rd.Values["controller"] = controllerName;
            return new ControllerContext(mockContext.Object, rd, new Mock<ControllerBase>().Object);
        }

        private static Controller GetEmptyController() {
            ControllerContext context = GetControllerContext("Foo");
            var controller = new EmptyController() {
                ControllerContext = context,
                RouteCollection = new RouteCollection(),
                TempData = new TempDataDictionary(),
                TempDataProvider = new SessionStateTempDataProvider()
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
                Assert.AreEqual<string>(SessionStateTempDataProvider.TempDataSessionStateKey, name);
                _sessionData.Remove(name);
            }

            public override object this[string name] {
                get {
                    Assert.AreEqual<string>(SessionStateTempDataProvider.TempDataSessionStateKey, name);
                    return _sessionData[name];
                }
                set {
                    Assert.AreEqual<string>(SessionStateTempDataProvider.TempDataSessionStateKey, name);
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

            public void PublicInitialize(RequestContext requestContext) {
                base.Initialize(requestContext);
            }
        }

        private sealed class UnknownActionController : Controller {
            public bool WasCalled;

            protected override void HandleUnknownAction(string actionName) {
                WasCalled = true;
            }
        }

        private sealed class TempDataHomeController : Controller {
            public ActionResult Index() {
                // Save UserID into TempData and redirect to greeting page
                TempData["UserID"] = "user123";
                return RedirectToAction("GreetUser");
            }

            public ActionResult GreetUser() {
                // Check that the UserID is present. If it's not
                // there, redirect to error page. If it is, show
                // the greet user view.
                if (!TempData.ContainsKey("UserID")) {
                    return RedirectToAction("ErrorPage");
                }
                ViewData["NewUserID"] = TempData["UserID"];
                return View("GreetUser");
            }
        }

        public class BrokenController : Controller {
            public BrokenController() {
                ActionInvoker = new ControllerActionInvoker() {
                    DispatcherCache = new ActionMethodDispatcherCache()
                };
            }
            public ActionResult Crash() {
                TempData["Key1"] = "Value1";
                throw new InvalidOperationException("Crashing....");
            }
        }

        private sealed class TestRouteController : Controller {
            public ActionResult Index() {
                return RedirectToAction("SomeAction");
            }
        }

        [ModelBinder(typeof(MyModelBinder))]
        private class MyModel {
            public ControllerContext ControllerContext;
            public IValueProvider ValueProvider;
            public string ModelName;
            public Type ModelType;
            public ModelStateDictionary ModelState;
            public Predicate<string> PropertyFilter;
        }

        private class MyModelSubclassed : MyModel {
        }

        private class MyModelBinder : IModelBinder {
            public ModelBinderResult BindModel(ModelBindingContext bindingContext) {
                MyModel myModel = (MyModel)bindingContext.Model;
                myModel.ControllerContext = bindingContext;
                myModel.ValueProvider = bindingContext.ValueProvider;
                myModel.ModelName = bindingContext.ModelName;
                myModel.ModelType = bindingContext.ModelType;
                myModel.ModelState = bindingContext.ModelState;
                myModel.PropertyFilter = bindingContext.ShouldUpdateProperty;
                return new ModelBinderResult(myModel);
            }
        }
    }

    internal class EmptyTempDataProvider : ITempDataProvider {
        public void SaveTempData(ControllerContext controllerContext, IDictionary<string, object> values) {
        }

        public IDictionary<string, object> LoadTempData(ControllerContext controllerContext) {
            return new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);
        }
    }
}
