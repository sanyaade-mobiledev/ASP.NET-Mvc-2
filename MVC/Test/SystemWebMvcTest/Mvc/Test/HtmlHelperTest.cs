namespace System.Web.Mvc.Test {
    using System;
    using System.Web.Routing;
    using System.Web.TestUtil;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;

    [TestClass]
    public class HtmlHelperTest {
        [TestMethod]
        public void ViewContextProperty() {
            // Setup
            ViewContext viewContext = new ViewContext(new Mock<HttpContextBase>().Object,
                new RouteData(),
                new Mock<IController>().Object,
                "view",
                null,
                null,
                new TempDataDictionary(ControllerContextTest.GetEmptyContextForTempData()));
            HtmlHelper htmlHelper = new HtmlHelper(viewContext);

            // Verify
            Assert.AreEqual(htmlHelper.ViewContext, viewContext);
        }

        [TestMethod]
        public void ConstructorWithNullViewContextThrows() {
            // Verify
            ExceptionHelper.ExpectArgumentNullException(
                delegate {
                    new HtmlHelper(null);
                },
                "viewContext");
        }

        [TestMethod]
        public void ActionLink() {
            // Setup
            HtmlHelper htmlHelper = GetHtmlHelper();

            // Execute
            string html = htmlHelper.ActionLink("linktext", "newaction");

            // Verify
            Assert.AreEqual<string>(@"<a href=""/app/home/newaction"">linktext</a>", html);
        }

        [TestMethod]
        public void ActionLinkParametersNeedEscaping() {
            // Setup
            HtmlHelper htmlHelper = GetHtmlHelper();

            // Execute
            string html = htmlHelper.ActionLink("linktext<&>\"", "new action<&>\"");

            // Verify
            Assert.AreEqual<string>(@"<a href=""/app/home/new%20action%3C&amp;%3E%22"">linktext&lt;&amp;&gt;&quot;</a>", html);
        }

        [TestMethod]
        public void ActionLinkWithControllerName() {
            // Setup
            HtmlHelper htmlHelper = GetHtmlHelper();

            // Execute
            string html = htmlHelper.ActionLink("linktext", "newaction", "home2");

            // Verify
            Assert.AreEqual<string>(@"<a href=""/app/home2/newaction"">linktext</a>", html);
        }

        [TestMethod]
        public void ActionLinkWithControllerNameAndDictionary() {
            // Setup
            HtmlHelper htmlHelper = GetHtmlHelper();

            // Execute
            string html = htmlHelper.ActionLink("linktext", "newaction", "home2", new RouteValueDictionary(new { id = "someid" }));

            // Verify
            Assert.AreEqual<string>(@"<a href=""/app/home2/newaction/someid"">linktext</a>", html);
        }

        [TestMethod]
        public void ActionLinkWithControllerNameAndObjectProperties() {
            // Setup
            HtmlHelper htmlHelper = GetHtmlHelper();

            // Execute
            string html = htmlHelper.ActionLink("linktext", "newaction", "home2", new { id = "someid" });

            // Verify
            Assert.AreEqual<string>(@"<a href=""/app/home2/newaction/someid"">linktext</a>", html);
        }

        [TestMethod]
        public void ActionLinkWithDictionary() {
            // Setup
            HtmlHelper htmlHelper = GetHtmlHelper();

            // Execute
            string html = htmlHelper.ActionLink("linktext", "newaction", new RouteValueDictionary(new { Controller = "home2", id = "someid" }));

            // Verify
            Assert.AreEqual<string>(@"<a href=""/app/home2/newaction/someid"">linktext</a>", html);
        }

        [TestMethod]
        public void ActionLinkWithObjectProperties() {
            // Setup
            HtmlHelper htmlHelper = GetHtmlHelper();

            // Execute
            string html = htmlHelper.ActionLink("linktext", "newaction", new { Controller = "home2", id = "someid" });

            // Verify
            Assert.AreEqual<string>(@"<a href=""/app/home2/newaction/someid"">linktext</a>", html);
        }

        [TestMethod]
        public void EncodeObject() {
            //Setup
            HtmlHelper htmlHelper = GetHtmlHelper();

            //Execute
            string encodedHtml = htmlHelper.Encode((object)"<br />");

            //Verify
            Assert.AreEqual(encodedHtml, "&lt;br /&gt;", "Text is not being properly HTML-encoded.");
        }

        [TestMethod]
        public void EncodeObjectNull() {
            //Setup
            HtmlHelper htmlHelper = GetHtmlHelper();

            //Execute
            string encodedHtml = htmlHelper.Encode((object)null);

            //Verify
            Assert.IsNull(encodedHtml);
        }

        [TestMethod]
        public void EncodeString() {
            //Setup
            HtmlHelper htmlHelper = GetHtmlHelper();

            //Execute
            string encodedHtml = htmlHelper.Encode("<br />");

            //Verify
            Assert.AreEqual(encodedHtml, "&lt;br /&gt;", "Text is not being properly HTML-encoded.");
        }

        [TestMethod]
        public void EncodeStringNull() {
            //Setup
            HtmlHelper htmlHelper = GetHtmlHelper();

            //Execute
            string encodedHtml = htmlHelper.Encode((string)null);

            //Verify
            Assert.IsNull(encodedHtml);
        }

        [TestMethod]
        public void LinkGenerationDoesNotChangeProvidedDictionary() {
            // Setup
            HtmlHelper htmlHelper = GetHtmlHelper();
            RouteValueDictionary valuesDictionary = new RouteValueDictionary();

            // Execute
            htmlHelper.ActionLink("linkText", "actionName", valuesDictionary);

            // Verify
            Assert.IsFalse(valuesDictionary.ContainsKey("action"));
        }

        [TestMethod]
        public void LinkGenerationThrowsIfDictionaryAlreadyContainsActionName() {
            // Setup
            HtmlHelper htmlHelper = GetHtmlHelper();
            RouteValueDictionary valuesDictionary = new RouteValueDictionary(new { Action = "someaction" });

            // Execute & verify
            ExceptionHelper.ExpectArgumentException(
                delegate() {
                    htmlHelper.ActionLink("linkText", "action", valuesDictionary);
                },
                "The provided object or dictionary already contains a definition for 'action'.\r\nParameter name: actionName");
        }

        [TestMethod]
        public void LinkGenerationThrowsIfDictionaryAlreadyContainsControllerName() {
            // Setup
            HtmlHelper htmlHelper = GetHtmlHelper();
            RouteValueDictionary valuesDictionary = new RouteValueDictionary(new { Controller = "somecontroller" });

            // Execute & verify
            ExceptionHelper.ExpectArgumentException(
                delegate() {
                    htmlHelper.ActionLink("linkText", "action", "controller", valuesDictionary);
                },
                "The provided object or dictionary already contains a definition for 'controller'.\r\nParameter name: controllerName");
        }

        [TestMethod]
        public void NullDictionaryParameterThrows() {
            // Setup
            HtmlHelper htmlHelper = GetHtmlHelper();
            GenericDelegate[] tests = new GenericDelegate[] {
                () => htmlHelper.ActionLink("linkText", "actionName", (RouteValueDictionary)null),
                () => htmlHelper.ActionLink("linkText", "actionName", "controllerName", (RouteValueDictionary)null),
                () => htmlHelper.RouteLink("linkText", (RouteValueDictionary)null),
                () => htmlHelper.RouteLink("linkText", "routeName", (RouteValueDictionary)null)
            };

            // Execute & verify
            foreach (GenericDelegate test in tests) {
                ExceptionHelper.ExpectArgumentNullException(test, "valuesDictionary");
            }
        }

        [TestMethod]
        public void NullOrEmptyStringParameterThrows() {
            // Setup
            HtmlHelper htmlHelper = GetHtmlHelper();
            Func<Action, GenericDelegate> Wrap = action => new GenericDelegate(() => action());
            var tests = new[] {
                // ActionLink(string linkText, string actionName)
                new { Parameter = "linkText", Action = Wrap(() => htmlHelper.ActionLink(String.Empty, "actionName")) },
                new { Parameter = "actionName", Action = Wrap(() => htmlHelper.ActionLink("linkText", String.Empty)) },

                // ActionLink(string linkText, string actionName, object values)
                new { Parameter = "linkText", Action = Wrap(() => htmlHelper.ActionLink(String.Empty, "actionName", new Object())) },
                new { Parameter = "actionName", Action = Wrap(() => htmlHelper.ActionLink("linkText", String.Empty, new Object())) },

                // ActionLink(string linkText, string actionName, RouteValueDictionary valuesDictionary)
                new { Parameter = "linkText", Action = Wrap(() => htmlHelper.ActionLink(String.Empty, "actionName", new RouteValueDictionary())) },
                new { Parameter = "actionName", Action = Wrap(() => htmlHelper.ActionLink("linkText", String.Empty, new RouteValueDictionary())) },

                // ActionLink(string linkText, string actionName, string controllerName)
                new { Parameter = "linkText", Action = Wrap(() => htmlHelper.ActionLink(String.Empty, "actionName", "controllerName")) },
                new { Parameter = "actionName", Action = Wrap(() => htmlHelper.ActionLink("linkText", String.Empty, "controllerName")) },
                new { Parameter = "controllerName", Action = Wrap(() => htmlHelper.ActionLink("linkText", "actionName", String.Empty)) },

                // ActionLink(string linkText, string actionName, string controllerName, object values)
                new { Parameter = "linkText", Action = Wrap(() => htmlHelper.ActionLink(String.Empty, "actionName", "controllerName", new Object())) },
                new { Parameter = "actionName", Action = Wrap(() => htmlHelper.ActionLink("linkText", String.Empty, "controllerName", new Object())) },
                new { Parameter = "controllerName", Action = Wrap(() => htmlHelper.ActionLink("linkText", "actionName", String.Empty, new Object())) },

                // ActionLink(string linkText, string actionName, string controllerName, RouteValueDictionary valuesDictionary)
                new { Parameter = "linkText", Action = Wrap(() => htmlHelper.ActionLink(String.Empty, "actionName", "controllerName", new RouteValueDictionary())) },
                new { Parameter = "actionName", Action = Wrap(() => htmlHelper.ActionLink("linkText", String.Empty, "controllerName", new RouteValueDictionary())) },
                new { Parameter = "controllerName", Action = Wrap(() => htmlHelper.ActionLink("linkText", "actionName", String.Empty, new RouteValueDictionary())) },

                // RouteLink(string linkText, object values)
                new { Parameter = "linkText", Action = Wrap(() => htmlHelper.RouteLink(String.Empty, new Object())) },

                // RouteLink(string linkText, RouteValueDictionary valuesDictionary)
                new { Parameter = "linkText", Action = Wrap(() => htmlHelper.RouteLink(String.Empty, new RouteValueDictionary())) },

                // RouteLink(string linkText, string routeName)
                new { Parameter = "linkText", Action = Wrap(() => htmlHelper.RouteLink(String.Empty, "routeName")) },
                new { Parameter = "routeName", Action = Wrap(() => htmlHelper.RouteLink("linkText", String.Empty)) },

                // RouteLink(string linkText, string routeName, object values)
                new { Parameter = "linkText", Action = Wrap(() => htmlHelper.RouteLink(String.Empty, "routeName", new Object())) },
                new { Parameter = "routeName", Action = Wrap(() => htmlHelper.RouteLink("linkText", String.Empty, new Object())) },

                // RouteLink(string linkText, string routeName, RouteValueDictionary valuesDictionary)
                new { Parameter = "linkText", Action = Wrap(() => htmlHelper.RouteLink(String.Empty, "routeName", new RouteValueDictionary())) },
                new { Parameter = "routeName", Action = Wrap(() => htmlHelper.RouteLink("linkText", String.Empty, new RouteValueDictionary())) }
            };

            // Execute & verify
            foreach (var test in tests) {
                ExceptionHelper.ExpectArgumentExceptionNullOrEmpty(test.Action, test.Parameter);
            }
        }

        [TestMethod]
        public void RouteLinkWithDictionary() {
            // Setup
            HtmlHelper htmlHelper = GetHtmlHelper();

            // Execute
            string html = htmlHelper.RouteLink("linktext", new RouteValueDictionary(new { Action = "newaction", Controller = "home2", id = "someid" }));

            // Verify
            Assert.AreEqual<string>(@"<a href=""/app/home2/newaction/someid"">linktext</a>", html);
        }

        [TestMethod]
        public void RouteLinkWithObjectProperties() {
            // Setup
            HtmlHelper htmlHelper = GetHtmlHelper();

            // Execute
            string html = htmlHelper.RouteLink("linktext", new { Action = "newaction", Controller = "home2", id = "someid" });

            // Verify
            Assert.AreEqual<string>(@"<a href=""/app/home2/newaction/someid"">linktext</a>", html);
        }

        [TestMethod]
        public void RouteLinkWithRouteNameAndDefaults() {
            // Setup
            HtmlHelper htmlHelper = GetHtmlHelper();

            // Execute
            string html = htmlHelper.RouteLink("linktext", "namedroute");

            // Verify
            Assert.AreEqual<string>(@"<a href=""/app/named/home/oldaction"">linktext</a>", html);
        }

        [TestMethod]
        public void RouteLinkWithRouteNameAndDictionary() {
            // Setup
            HtmlHelper htmlHelper = GetHtmlHelper();

            // Execute
            string html = htmlHelper.RouteLink("linktext", "namedroute", new RouteValueDictionary(new { Action = "newaction", Controller = "home2", id = "someid" }));

            // Verify
            Assert.AreEqual<string>(@"<a href=""/app/named/home2/newaction/someid"">linktext</a>", html);
        }

        [TestMethod]
        public void RouteLinkWithRouteNameAndObjectProperties() {
            // Setup
            HtmlHelper htmlHelper = GetHtmlHelper();

            // Execute
            string html = htmlHelper.RouteLink("linktext", "namedroute", new { Action = "newaction", Controller = "home2", id = "someid" });

            // Verify
            Assert.AreEqual<string>(@"<a href=""/app/named/home2/newaction/someid"">linktext</a>", html);
        }

        private static HttpContextBase GetHttpContext(string appPath, string requestPath, string httpMethod) {
            Mock<HttpContextBase> mockContext = new Mock<HttpContextBase>();
            Mock<HttpRequestBase> mockRequest = new Mock<HttpRequestBase>();
            if (!String.IsNullOrEmpty(appPath)) {
                mockRequest.Expect(o => o.ApplicationPath).Returns(appPath);
            }
            if (!String.IsNullOrEmpty(requestPath)) {
                mockRequest.Expect(o => o.AppRelativeCurrentExecutionFilePath).Returns(requestPath);
            }
            mockRequest.Expect(o => o.PathInfo).Returns(String.Empty);
            if (!String.IsNullOrEmpty(httpMethod)) {
                mockRequest.Expect(o => o.HttpMethod).Returns(httpMethod);
            }
            mockContext.Expect(o => o.Request).Returns(mockRequest.Object);

            return mockContext.Object;
        }

        private static HtmlHelper GetHtmlHelper() {
            HttpContextBase httpcontext = GetHttpContext("/app/", null, null);
            RouteCollection rt = new RouteCollection();
            rt.Add(new Route("{controller}/{action}/{id}", null) { Defaults = new RouteValueDictionary(new { id = "defaultid" }) });
            rt.Add("namedroute", new Route("named/{controller}/{action}/{id}", null) { Defaults = new RouteValueDictionary(new { id = "defaultid" }) });
            RouteData rd = new RouteData();
            rd.Values.Add("controller", "home");
            rd.Values.Add("action", "oldaction");
            ViewContext context = new ViewContext(httpcontext, rd, new Mock<IController>().Object, "view", null, null, new TempDataDictionary(ControllerContextTest.GetEmptyContextForTempData()));
            HtmlHelper htmlHelper = new HtmlHelper(context);
            htmlHelper.RouteCollection = rt;
            return htmlHelper;
        }
    }
}
