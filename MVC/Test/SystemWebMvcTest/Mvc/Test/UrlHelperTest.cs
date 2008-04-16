namespace System.Web.Mvc.Test {
    using System;
    using System.Web.Routing;
    using System.Web.TestUtil;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;

    [TestClass]
    public class UrlHelperTest {
        [TestMethod]
        public void ViewContextProperty() {
            // Setup
            ViewContext viewContext = new ViewContext(
                new Mock<HttpContextBase>().Object,
                new RouteData(),
                new Mock<IController>().Object,
                "view",
                "master",
                null,
                new TempDataDictionary(ControllerContextTest.GetEmptyContextForTempData()));
            UrlHelper urlHelper = new UrlHelper(viewContext);

            // Verify
            Assert.AreEqual(urlHelper.ViewContext, viewContext);
        }

        [TestMethod]
        public void ConstructorWithNullViewContextThrows() {
            // Verify
            ExceptionHelper.ExpectArgumentNullException(
                delegate {
                    new UrlHelper(null);
                },
                "viewContext");
        }

        [TestMethod]
        public void Action() {
            // Setup
            UrlHelper urlHelper = GetUrlHelper();

            // Execute
            string url = urlHelper.Action("newaction");

            // Verify
            Assert.AreEqual<string>("/app/home/newaction", url);
        }

        [TestMethod]
        public void ActionWithControllerName() {
            // Setup
            UrlHelper urlHelper = GetUrlHelper();

            // Execute
            string url = urlHelper.Action("newaction", "home2");

            // Verify
            Assert.AreEqual<string>("/app/home2/newaction", url);
        }

        [TestMethod]
        public void ActionWithControllerNameAndDictionary() {
            // Setup
            UrlHelper urlHelper = GetUrlHelper();

            // Execute
            string url = urlHelper.Action("newaction", "home2", new RouteValueDictionary(new { id = "someid" }));

            // Verify
            Assert.AreEqual<string>("/app/home2/newaction/someid", url);
        }

        [TestMethod]
        public void ActionWithControllerNameAndObjectProperties() {
            // Setup
            UrlHelper urlHelper = GetUrlHelper();

            // Execute
            string url = urlHelper.Action("newaction", "home2", new { id = "someid" });

            // Verify
            Assert.AreEqual<string>("/app/home2/newaction/someid", url);
        }

        [TestMethod]
        public void ActionWithDictionary() {
            // Setup
            UrlHelper urlHelper = GetUrlHelper();

            // Execute
            string url = urlHelper.Action("newaction", new RouteValueDictionary(new { Controller = "home2", id = "someid" }));

            // Verify
            Assert.AreEqual<string>("/app/home2/newaction/someid", url);
        }

        [TestMethod]
        public void ActionWithObjectProperties() {
            // Setup
            UrlHelper urlHelper = GetUrlHelper();

            // Execute
            string url = urlHelper.Action("newaction", new { Controller = "home2", id = "someid" });

            // Verify
            Assert.AreEqual<string>("/app/home2/newaction/someid", url);
        }

        [TestMethod]
        public void ContentWithAbsolutePath() {
            // Setup
            UrlHelper urlHelper = GetUrlHelper();

            // Execute
            string url = urlHelper.Content("/Content/Image.jpg");

            // Verify
            Assert.AreEqual("/app/Content/Image.jpg", url);
        }

        [TestMethod]
        public void ContentWithAppRelativePath() {
            // Setup
            UrlHelper urlHelper = GetUrlHelper();

            // Execute
            string url = urlHelper.Content("~/Content/Image.jpg");

            // Verify
            Assert.AreEqual("/app/Content/Image.jpg", url);
        }

        [TestMethod]
        public void ContentWithEmptyPathThrows() {
            // Setup
            UrlHelper urlHelper = GetUrlHelper();

            // Execute & verify
            ExceptionHelper.ExpectArgumentExceptionNullOrEmpty(
                delegate() {
                    urlHelper.Content(String.Empty);
                },
                "contentPath");
        }

        [TestMethod]
        public void ContentWithRelativePath() {
            // Setup
            UrlHelper urlHelper = GetUrlHelper();

            // Execute
            string url = urlHelper.Content("Content/Image.jpg");

            // Verify
            Assert.AreEqual("/app/Content/Image.jpg", url);
        }

        [TestMethod]
        public void Encode() {
            // Setup
            UrlHelper urlHelper = GetUrlHelper();

            // Execute
            string encodedUrl = urlHelper.Encode(@"SomeUrl /+\");

            // Verify
            Assert.AreEqual(encodedUrl, "SomeUrl+%2f%2b%5c");
        }

        [TestMethod]
        public void NullDictionaryParameterThrows() {
            // Setup
            UrlHelper urlHelper = GetUrlHelper();
            GenericDelegate[] tests = new GenericDelegate[] {
                () => urlHelper.Action("actionName", (RouteValueDictionary)null),
                () => urlHelper.Action("actionName", "controllerName", (RouteValueDictionary)null),
                () => urlHelper.RouteUrl((RouteValueDictionary)null),
                () => urlHelper.RouteUrl("routeName", (RouteValueDictionary)null)
            };

            // Execute & verify
            foreach (GenericDelegate test in tests) {
                ExceptionHelper.ExpectArgumentNullException(test, "valuesDictionary");
            }
        }

        [TestMethod]
        public void NullOrEmptyStringParameterThrows() {
            // Setup
            UrlHelper urlHelper = GetUrlHelper();
            Func<Action, GenericDelegate> Wrap = action => new GenericDelegate(() => action());
            var tests = new[] {
                // Action(string actionName)
                new { Parameter = "actionName", Action = Wrap(() => urlHelper.Action(String.Empty)) },

                // Action(string actionName, object values)
                new { Parameter = "actionName", Action = Wrap(() => urlHelper.Action(String.Empty, new Object())) },

                // Action(string actionName, RouteValueDictionary valuesDictionary)
                new { Parameter = "actionName", Action = Wrap(() => urlHelper.Action(String.Empty, new RouteValueDictionary())) },

                // Action(string actionName, string controllerName)
                new { Parameter = "actionName", Action = Wrap(() => urlHelper.Action(String.Empty, "controllerName")) },
                new { Parameter = "controllerName", Action = Wrap(() => urlHelper.Action("actionName", (string)null)) },

                // Action(string actionName, string controllerName, object values)
                new { Parameter = "actionName", Action = Wrap(() => urlHelper.Action(String.Empty, "controllerName", new Object())) },
                new { Parameter = "controllerName", Action = Wrap(() => urlHelper.Action("actionName", String.Empty, new Object())) },

                // Action(string actionName, string controllerName, RouteValueDictionary valuesDictionary)
                new { Parameter = "actionName", Action = Wrap(() => urlHelper.Action(String.Empty, "controllerName", new RouteValueDictionary())) },
                new { Parameter = "controllerName", Action = Wrap(() => urlHelper.Action("actionName", String.Empty, new RouteValueDictionary())) },

                // RouteUrl(string routeName)
                new { Parameter = "routeName", Action = Wrap(() => urlHelper.RouteUrl(String.Empty)) },
                
                // RouteUrl(string routeName, object values)
                new { Parameter = "routeName", Action = Wrap(() => urlHelper.RouteUrl(String.Empty, new Object())) },

                // RouteUrl(string routeName, RouteValueDictionary valuesDictionary)
                new { Parameter = "routeName", Action = Wrap(() => urlHelper.RouteUrl(String.Empty, new RouteValueDictionary())) }
            };

            // Execute & verify
            foreach (var test in tests) {
                ExceptionHelper.ExpectArgumentExceptionNullOrEmpty(test.Action, test.Parameter);
            }
        }

        [TestMethod]
        public void RouteUrlWithDictionary() {
            // Setup
            UrlHelper urlHelper = GetUrlHelper();

            // Execute
            string url = urlHelper.RouteUrl(new RouteValueDictionary(new { Action = "newaction", Controller = "home2", id = "someid" }));

            // Verify
            Assert.AreEqual<string>("/app/home2/newaction/someid", url);
        }

        [TestMethod]
        public void RouteUrlWithObjectProperties() {
            // Setup
            UrlHelper urlHelper = GetUrlHelper();

            // Execute
            string url = urlHelper.RouteUrl(new { Action = "newaction", Controller = "home2", id = "someid" });

            // Verify
            Assert.AreEqual<string>("/app/home2/newaction/someid", url);
        }

        [TestMethod]
        public void RouteUrlWithRouteNameAndDefaults() {
            // Setup
            UrlHelper urlHelper = GetUrlHelper();

            // Execute
            string url = urlHelper.RouteUrl("namedroute");

            // Verify
            Assert.AreEqual<string>("/app/named/home/oldaction", url);
        }

        [TestMethod]
        public void RouteUrlWithRouteNameAndDictionary() {
            // Setup
            UrlHelper urlHelper = GetUrlHelper();

            // Execute
            string url = urlHelper.RouteUrl("namedroute", new RouteValueDictionary(new { Action = "newaction", Controller = "home2", id = "someid" }));

            // Verify
            Assert.AreEqual<string>("/app/named/home2/newaction/someid", url);
        }

        [TestMethod]
        public void RouteUrlWithRouteNameAndObjectProperties() {
            // Setup
            UrlHelper urlHelper = GetUrlHelper();

            // Execute
            string url = urlHelper.RouteUrl("namedroute", new { Action = "newaction", Controller = "home2", id = "someid" });

            // Verify
            Assert.AreEqual<string>("/app/named/home2/newaction/someid", url);
        }

        [TestMethod]
        public void UrlGenerationDoesNotChangeProvidedDictionary() {
            // Setup
            UrlHelper urlHelper = GetUrlHelper();
            RouteValueDictionary valuesDictionary = new RouteValueDictionary();

            // Execute
            urlHelper.Action("actionName", valuesDictionary);

            // Verify
            Assert.IsFalse(valuesDictionary.ContainsKey("action"));
        }

        [TestMethod]
        public void UrlGenerationThrowsIfDictionaryAlreadyContainsActionName() {
            // Setup
            UrlHelper urlHelper = GetUrlHelper();
            RouteValueDictionary valuesDictionary = new RouteValueDictionary(new { Action = "someaction" });

            // Execute & verify
            ExceptionHelper.ExpectArgumentException(
                delegate() {
                    urlHelper.Action("action", valuesDictionary);
                },
                "The provided object or dictionary already contains a definition for 'action'.\r\nParameter name: actionName");
        }

        [TestMethod]
        public void UrlGenerationThrowsIfDictionaryAlreadyContainsControllerName() {
            // Setup
            UrlHelper urlHelper = GetUrlHelper();
            RouteValueDictionary valuesDictionary = new RouteValueDictionary(new { Controller = "somecontroller" });

            // Execute & verify
            ExceptionHelper.ExpectArgumentException(
                delegate() {
                    urlHelper.Action("action", "controller", valuesDictionary);
                },
                "The provided object or dictionary already contains a definition for 'controller'.\r\nParameter name: controllerName");
        }

        private static HttpContextBase GetHttpContext(string appPath, string requestPath, string httpMethod) {
            Mock<HttpContextBase> mockContext = new Mock<HttpContextBase>();
            Mock<HttpRequestBase> mockRequest = new Mock<HttpRequestBase>();
            if (!String.IsNullOrEmpty(appPath)) {
                mockRequest.Expect(o => o.ApplicationPath).Returns(appPath);
            }
            if (!String.IsNullOrEmpty(requestPath)) {
                mockRequest.Expect(o => o.AppRelativeCurrentExecutionFilePath).Returns( requestPath);
            }
            mockRequest.Expect(o => o.PathInfo).Returns(String.Empty);
            if (!String.IsNullOrEmpty(httpMethod)) {
                mockRequest.Expect(o => o.HttpMethod).Returns(httpMethod);
            }
            mockContext.Expect(o => o.Request).Returns(mockRequest.Object);
            mockContext.Expect(o => o.Session).Returns((HttpSessionStateBase)null);

            return mockContext.Object;
        }

        private static UrlHelper GetUrlHelper() {
            HttpContextBase httpcontext = GetHttpContext("/app/", null, null);
            RouteCollection rt = new RouteCollection();
            rt.Add(new Route("{controller}/{action}/{id}", null) { Defaults = new RouteValueDictionary(new { id = "defaultid" }) });
            rt.Add("namedroute", new Route("named/{controller}/{action}/{id}", null) { Defaults = new RouteValueDictionary(new { id = "defaultid" }) });
            RouteData rd = new RouteData();
            rd.Values.Add("controller", "home");
            rd.Values.Add("action", "oldaction");
            ViewContext context = new ViewContext(httpcontext, rd, new Mock<IController>().Object, "view", "master", null, new TempDataDictionary(ControllerContextTest.GetEmptyContextForTempData()));
            UrlHelper urlHelper = new UrlHelper(context);
            urlHelper.RouteCollection = rt;
            return urlHelper;
        }
    }
}
