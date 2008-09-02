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
            // Arrange
            ViewContext viewContext = new ViewContext(
                new Mock<HttpContextBase>().Object,
                new RouteData(),
                new Mock<ControllerBase>().Object,
                "view",
                new ViewDataDictionary(),
                new TempDataDictionary());
            UrlHelper urlHelper = new UrlHelper(viewContext);

            // Assert
            Assert.AreEqual(urlHelper.ViewContext, viewContext);
        }

        [TestMethod]
        public void ConstructorWithNullViewContextThrows() {
            // Assert
            ExceptionHelper.ExpectArgumentNullException(
                delegate {
                    new UrlHelper(null);
                },
                "viewContext");
        }

        [TestMethod]
        public void Action() {
            // Arrange
            UrlHelper urlHelper = GetUrlHelper();

            // Act
            string url = urlHelper.Action("newaction");

            // Assert
            Assert.AreEqual<string>(HtmlHelperTest.AppPathModifier + "/app/home/newaction", url);
        }

        [TestMethod]
        public void ActionWithControllerName() {
            // Arrange
            UrlHelper urlHelper = GetUrlHelper();

            // Act
            string url = urlHelper.Action("newaction", "home2");

            // Assert
            Assert.AreEqual<string>(HtmlHelperTest.AppPathModifier + "/app/home2/newaction", url);
        }

        [TestMethod]
        public void ActionWithControllerNameAndDictionary() {
            // Arrange
            UrlHelper urlHelper = GetUrlHelper();

            // Act
            string url = urlHelper.Action("newaction", "home2", new RouteValueDictionary(new { id = "someid" }));

            // Assert
            Assert.AreEqual<string>(HtmlHelperTest.AppPathModifier + "/app/home2/newaction/someid", url);
        }

        [TestMethod]
        public void ActionWithControllerNameAndObjectProperties() {
            // Arrange
            UrlHelper urlHelper = GetUrlHelper();

            // Act
            string url = urlHelper.Action("newaction", "home2", new { id = "someid" });

            // Assert
            Assert.AreEqual<string>(HtmlHelperTest.AppPathModifier + "/app/home2/newaction/someid", url);
        }

        [TestMethod]
        public void ActionWithDictionary() {
            // Arrange
            UrlHelper urlHelper = GetUrlHelper();

            // Act
            string url = urlHelper.Action("newaction", new RouteValueDictionary(new { Controller = "home2", id = "someid" }));

            // Assert
            Assert.AreEqual<string>(HtmlHelperTest.AppPathModifier + "/app/home2/newaction/someid", url);
        }

        [TestMethod]
        public void ActionWithNullProtocol() {
            // Arrange
            UrlHelper urlHelper = GetUrlHelper();

            // Act
            string url = urlHelper.Action("newaction", "home2", new { id = "someid" }, null /* protocol */);

            // Assert
            Assert.AreEqual<string>(HtmlHelperTest.AppPathModifier + "/app/home2/newaction/someid", url);
        }

        [TestMethod]
        public void ActionWithObjectProperties() {
            // Arrange
            UrlHelper urlHelper = GetUrlHelper();

            // Act
            string url = urlHelper.Action("newaction", new { Controller = "home2", id = "someid" });

            // Assert
            Assert.AreEqual<string>(HtmlHelperTest.AppPathModifier + "/app/home2/newaction/someid", url);
        }

        [TestMethod]
        public void ActionWithProtocol() {
            // Arrange
            UrlHelper urlHelper = GetUrlHelper();

            // Act
            string url = urlHelper.Action("newaction", "home2", new { id = "someid" }, "https");

            // Assert
            Assert.AreEqual<string>("https://localhost" + HtmlHelperTest.AppPathModifier + "/app/home2/newaction/someid", url);
        }

        [TestMethod]
        public void ContentWithAbsolutePath() {
            // Arrange
            UrlHelper urlHelper = GetUrlHelper();

            // Act
            string url = urlHelper.Content("/Content/Image.jpg");

            // Assert
            Assert.AreEqual("/Content/Image.jpg", url);
        }

        [TestMethod]
        public void ContentWithAppRelativePath() {
            // Arrange
            UrlHelper urlHelper = GetUrlHelper();

            // Act
            string url = urlHelper.Content("~/Content/Image.jpg");

            // Assert
            Assert.AreEqual("/app/Content/Image.jpg", url);
        }

        [TestMethod]
        public void ContentWithEmptyPathThrows() {
            // Arrange
            UrlHelper urlHelper = GetUrlHelper();

            // Act & Assert
            ExceptionHelper.ExpectArgumentExceptionNullOrEmpty(
                delegate() {
                    urlHelper.Content(String.Empty);
                },
                "contentPath");
        }

        [TestMethod]
        public void ContentWithRelativePath() {
            // Arrange
            UrlHelper urlHelper = GetUrlHelper();

            // Act
            string url = urlHelper.Content("Content/Image.jpg");

            // Assert
            Assert.AreEqual("Content/Image.jpg", url);
        }

        [TestMethod]
        public void ContentWithExternalUrl() {
            // Arrange
            UrlHelper urlHelper = GetUrlHelper();

            // Act
            string url = urlHelper.Content("http://www.asp.net/App_Themes/Standard/i/logo.png");

            // Assert
            Assert.AreEqual("http://www.asp.net/App_Themes/Standard/i/logo.png", url);
        }

        [TestMethod]
        public void Encode() {
            // Arrange
            UrlHelper urlHelper = GetUrlHelper();

            // Act
            string encodedUrl = urlHelper.Encode(@"SomeUrl /+\");

            // Assert
            Assert.AreEqual(encodedUrl, "SomeUrl+%2f%2b%5c");
        }

        [TestMethod]
        public void NullDictionaryParameterThrows() {
            // Arrange
            UrlHelper urlHelper = GetUrlHelper();
            GenericDelegate[] tests = new GenericDelegate[] {
                () => urlHelper.Action("actionName", (RouteValueDictionary)null),
                () => urlHelper.Action("actionName", "controllerName", (RouteValueDictionary)null),
                () => urlHelper.RouteUrl((RouteValueDictionary)null),
                () => urlHelper.RouteUrl("routeName", (RouteValueDictionary)null)
            };

            // Act & Assert
            foreach (GenericDelegate test in tests) {
                ExceptionHelper.ExpectArgumentNullException(test, "valuesDictionary");
            }
        }

        [TestMethod]
        public void NullOrEmptyStringParameterThrows() {
            // Arrange
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

            // Act & Assert
            foreach (var test in tests) {
                ExceptionHelper.ExpectArgumentExceptionNullOrEmpty(test.Action, test.Parameter);
            }
        }

        [TestMethod]
        public void RouteUrlWithDictionary() {
            // Arrange
            UrlHelper urlHelper = GetUrlHelper();

            // Act
            string url = urlHelper.RouteUrl(new RouteValueDictionary(new { Action = "newaction", Controller = "home2", id = "someid" }));

            // Assert
            Assert.AreEqual<string>(HtmlHelperTest.AppPathModifier + "/app/home2/newaction/someid", url);
        }

        [TestMethod]
        public void RouteUrlWithEmptyHostName() {
            // Arrange
            UrlHelper urlHelper = GetUrlHelper();

            // Act
            string url = urlHelper.RouteUrl("namedroute", new RouteValueDictionary(new { Action = "newaction", Controller = "home2", id = "someid" }), "http", String.Empty /* hostName */);

            // Assert
            Assert.AreEqual<string>("http://localhost" + HtmlHelperTest.AppPathModifier + "/app/named/home2/newaction/someid", url);
        }

        [TestMethod]
        public void RouteUrlWithEmptyProtocol() {
            // Arrange
            UrlHelper urlHelper = GetUrlHelper();

            // Act
            string url = urlHelper.RouteUrl("namedroute", new RouteValueDictionary(new { Action = "newaction", Controller = "home2", id = "someid" }), String.Empty /* protocol */, "foo.bar.com");

            // Assert
            Assert.AreEqual<string>("http://foo.bar.com" + HtmlHelperTest.AppPathModifier + "/app/named/home2/newaction/someid", url);
        }

        [TestMethod]
        public void RouteUrlWithNullProtocol() {
            // Arrange
            UrlHelper urlHelper = GetUrlHelper();

            // Act
            string url = urlHelper.RouteUrl("namedroute", new RouteValueDictionary(new { Action = "newaction", Controller = "home2", id = "someid" }), null /* protocol */, "foo.bar.com");

            // Assert
            Assert.AreEqual<string>("http://foo.bar.com" + HtmlHelperTest.AppPathModifier + "/app/named/home2/newaction/someid", url);
        }       

        [TestMethod]
        public void RouteUrlWithNullProtocolAndNullHostName() {
            // Arrange
            UrlHelper urlHelper = GetUrlHelper();

            // Act
            string url = urlHelper.RouteUrl("namedroute", new RouteValueDictionary(new { Action = "newaction", Controller = "home2", id = "someid" }), null /* protocol */, null /* hostName */);

            // Assert
            Assert.AreEqual<string>(HtmlHelperTest.AppPathModifier + "/app/named/home2/newaction/someid", url);
        }

        [TestMethod]
        public void RouteUrlWithObjectProperties() {
            // Arrange
            UrlHelper urlHelper = GetUrlHelper();

            // Act
            string url = urlHelper.RouteUrl(new { Action = "newaction", Controller = "home2", id = "someid" });

            // Assert
            Assert.AreEqual<string>(HtmlHelperTest.AppPathModifier + "/app/home2/newaction/someid", url);
        }

        [TestMethod]
        public void RouteUrlWithProtocol() {
            // Arrange
            UrlHelper urlHelper = GetUrlHelper();

            // Act
            string url = urlHelper.RouteUrl("namedroute", new { Action = "newaction", Controller = "home2", id = "someid" }, "https");

            // Assert
            Assert.AreEqual<string>("https://localhost" + HtmlHelperTest.AppPathModifier + "/app/named/home2/newaction/someid", url);
        }

        [TestMethod]
        public void RouteUrlWithRouteNameAndDefaults() {
            // Arrange
            UrlHelper urlHelper = GetUrlHelper();

            // Act
            string url = urlHelper.RouteUrl("namedroute");

            // Assert
            Assert.AreEqual<string>(HtmlHelperTest.AppPathModifier + "/app/named/home/oldaction", url);
        }

        [TestMethod]
        public void RouteUrlWithRouteNameAndDictionary() {
            // Arrange
            UrlHelper urlHelper = GetUrlHelper();

            // Act
            string url = urlHelper.RouteUrl("namedroute", new RouteValueDictionary(new { Action = "newaction", Controller = "home2", id = "someid" }));

            // Assert
            Assert.AreEqual<string>(HtmlHelperTest.AppPathModifier + "/app/named/home2/newaction/someid", url);
        }

        [TestMethod]
        public void RouteUrlWithRouteNameAndObjectProperties() {
            // Arrange
            UrlHelper urlHelper = GetUrlHelper();

            // Act
            string url = urlHelper.RouteUrl("namedroute", new { Action = "newaction", Controller = "home2", id = "someid" });

            // Assert
            Assert.AreEqual<string>(HtmlHelperTest.AppPathModifier + "/app/named/home2/newaction/someid", url);
        }

        [TestMethod]
        public void UrlGenerationDoesNotChangeProvidedDictionary() {
            // Arrange
            UrlHelper urlHelper = GetUrlHelper();
            RouteValueDictionary valuesDictionary = new RouteValueDictionary();

            // Act
            urlHelper.Action("actionName", valuesDictionary);

            // Assert
            Assert.IsFalse(valuesDictionary.ContainsKey("action"));
        }

        [TestMethod]
        public void UrlGenerationThrowsIfDictionaryAlreadyContainsActionName() {
            // Arrange
            UrlHelper urlHelper = GetUrlHelper();
            RouteValueDictionary valuesDictionary = new RouteValueDictionary(new { Action = "someaction" });

            // Act & Assert
            ExceptionHelper.ExpectArgumentException(
                delegate() {
                    urlHelper.Action("action", valuesDictionary);
                },
                "The provided object or dictionary already contains a definition for 'action'.\r\nParameter name: actionName");
        }

        [TestMethod]
        public void UrlGenerationThrowsIfDictionaryAlreadyContainsControllerName() {
            // Arrange
            UrlHelper urlHelper = GetUrlHelper();
            RouteValueDictionary valuesDictionary = new RouteValueDictionary(new { Controller = "somecontroller" });

            // Act & Assert
            ExceptionHelper.ExpectArgumentException(
                delegate() {
                    urlHelper.Action("action", "controller", valuesDictionary);
                },
                "The provided object or dictionary already contains a definition for 'controller'.\r\nParameter name: controllerName");
        }

        private static UrlHelper GetUrlHelper() {
            HttpContextBase httpcontext = HtmlHelperTest.GetHttpContext("/app/", null, null);
            RouteCollection rt = new RouteCollection();
            rt.Add(new Route("{controller}/{action}/{id}", null) { Defaults = new RouteValueDictionary(new { id = "defaultid" }) });
            rt.Add("namedroute", new Route("named/{controller}/{action}/{id}", null) { Defaults = new RouteValueDictionary(new { id = "defaultid" }) });
            RouteData rd = new RouteData();
            rd.Values.Add("controller", "home");
            rd.Values.Add("action", "oldaction");
            ViewContext context = new ViewContext(httpcontext, rd, new Mock<ControllerBase>().Object, "view", new ViewDataDictionary(), new TempDataDictionary());
            UrlHelper urlHelper = new UrlHelper(context);
            urlHelper.RouteCollection = rt;
            return urlHelper;
        }
    }
}
