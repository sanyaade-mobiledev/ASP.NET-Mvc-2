namespace System.Web.Mvc.Test {
    using System;
    using System.Collections.Generic;
    using System.Web.Routing;
    using System.Web.TestUtil;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;

    [TestClass]
    public partial class HtmlHelperTest {
        public const string AppPathModifier = "/$(SESSION)";

        [TestMethod]
        public void ViewContextProperty() {
            // Setup
            ViewContext viewContext = GetViewContext();
            HtmlHelper htmlHelper = new HtmlHelper(viewContext, new Mock<IViewDataContainer>().Object);

            // Execute
            ViewContext value = htmlHelper.ViewContext;

            // Verify
            Assert.AreEqual(viewContext, value);
        }

        [TestMethod]
        public void ViewDataContainerProperty() {
            // Setup
            ViewContext viewContext = GetViewContext();
            IViewDataContainer container = new Mock<IViewDataContainer>().Object;
            HtmlHelper htmlHelper = new HtmlHelper(viewContext, container);

            // Execute
            IViewDataContainer value = htmlHelper.ViewDataContainer;

            // Verify
            Assert.AreEqual(container, value);
        }

        [TestMethod]
        public void ConstructorWithNullViewContextThrows() {
            // Verify
            ExceptionHelper.ExpectArgumentNullException(
                delegate {
                    new HtmlHelper(null, null);
                },
                "viewContext");
        }

        [TestMethod]
        public void ConstructorWithNullViewDataContainerThrows() {
            // Verify
            ExceptionHelper.ExpectArgumentNullException(
                delegate {
                    new HtmlHelper(GetViewContext(), null);
                },
                "viewDataContainer");
        }

        [TestMethod]
        public void ActionLink() {
            // Setup
            HtmlHelper htmlHelper = GetHtmlHelper();

            // Execute
            string html = htmlHelper.ActionLink("linktext", "newaction");

            // Verify
            Assert.AreEqual<string>(@"<a href=""" + AppPathModifier + @"/app/home/newaction"">linktext</a>", html);
        }

        [TestMethod]
        public void ActionLinkParametersNeedEscaping() {
            // Setup
            HtmlHelper htmlHelper = GetHtmlHelper();

            // Execute
            string html = htmlHelper.ActionLink("linktext<&>\"", "new action<&>\"");

            // Verify
            Assert.AreEqual<string>(@"<a href=""" + AppPathModifier + @"/app/home/new%20action%3C&amp;%3E%22"">linktext&lt;&amp;&gt;&quot;</a>", html);
        }

        [TestMethod]
        public void ActionLinkWithControllerName() {
            // Setup
            HtmlHelper htmlHelper = GetHtmlHelper();

            // Execute
            string html = htmlHelper.ActionLink("linktext", "newaction", "home2");

            // Verify
            Assert.AreEqual<string>(@"<a href=""" + AppPathModifier + @"/app/home2/newaction"">linktext</a>", html);
        }

        [TestMethod]
        public void ActionLinkWithControllerNameAndDictionary() {
            // Setup
            HtmlHelper htmlHelper = GetHtmlHelper();

            // Execute
            string html = htmlHelper.ActionLink("linktext", "newaction", "home2", new RouteValueDictionary(new { id = "someid" }));

            // Verify
            Assert.AreEqual<string>(@"<a href=""" + AppPathModifier + @"/app/home2/newaction/someid"">linktext</a>", html);
        }

        [TestMethod]
        public void ActionLinkWithControllerNameAndObjectProperties() {
            // Setup
            HtmlHelper htmlHelper = GetHtmlHelper();

            // Execute
            string html = htmlHelper.ActionLink("linktext", "newaction", "home2", new { id = "someid" });

            // Verify
            Assert.AreEqual<string>(@"<a href=""" + AppPathModifier + @"/app/home2/newaction/someid"">linktext</a>", html);
        }

        [TestMethod]
        public void ActionLinkWithDictionary() {
            // Setup
            HtmlHelper htmlHelper = GetHtmlHelper();

            // Execute
            string html = htmlHelper.ActionLink("linktext", "newaction", new RouteValueDictionary(new { Controller = "home2", id = "someid" }));

            // Verify
            Assert.AreEqual<string>(@"<a href=""" + AppPathModifier + @"/app/home2/newaction/someid"">linktext</a>", html);
        }

        [TestMethod]
        public void ActionLinkWithObjectProperties() {
            // Setup
            HtmlHelper htmlHelper = GetHtmlHelper();

            // Execute
            string html = htmlHelper.ActionLink("linktext", "newaction", new { Controller = "home2", id = "someid" });

            // Verify
            Assert.AreEqual<string>(@"<a href=""" + AppPathModifier + @"/app/home2/newaction/someid"">linktext</a>", html);
        }

        [TestMethod]
        public void AttributeEncodeObject() {
            //Setup
            HtmlHelper htmlHelper = GetHtmlHelper();

            //Execute
            string encodedHtml = htmlHelper.AttributeEncode((object)@"<"">");

            //Verify
            Assert.AreEqual(encodedHtml, "&lt;&quot;>", "Text is not being properly HTML attribute-encoded.");
        }

        [TestMethod]
        public void AttributeEncodeObjectNull() {
            //Setup
            HtmlHelper htmlHelper = GetHtmlHelper();

            //Execute
            string encodedHtml = htmlHelper.AttributeEncode((object)null);

            //Verify
            Assert.AreEqual("", encodedHtml);
        }

        [TestMethod]
        public void AttributeEncodeString() {
            //Setup
            HtmlHelper htmlHelper = GetHtmlHelper();

            //Execute
            string encodedHtml = htmlHelper.AttributeEncode(@"<"">");

            //Verify
            Assert.AreEqual(encodedHtml, "&lt;&quot;>", "Text is not being properly HTML attribute-encoded.");
        }

        [TestMethod]
        public void AttributeEncodeStringNull() {
            //Setup
            HtmlHelper htmlHelper = GetHtmlHelper();

            //Execute
            string encodedHtml = htmlHelper.AttributeEncode((string)null);

            //Verify
            Assert.AreEqual("", encodedHtml);
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
            Assert.AreEqual("", encodedHtml);
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
            Assert.AreEqual("", encodedHtml);
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
            Assert.AreEqual<string>(@"<a href=""" + AppPathModifier + @"/app/home2/newaction/someid"">linktext</a>", html);
        }

        [TestMethod]
        public void RouteLinkWithObjectProperties() {
            // Setup
            HtmlHelper htmlHelper = GetHtmlHelper();

            // Execute
            string html = htmlHelper.RouteLink("linktext", new { Action = "newaction", Controller = "home2", id = "someid" });

            // Verify
            Assert.AreEqual<string>(@"<a href=""" + AppPathModifier + @"/app/home2/newaction/someid"">linktext</a>", html);
        }

        [TestMethod]
        public void RouteLinkWithRouteNameAndDefaults() {
            // Setup
            HtmlHelper htmlHelper = GetHtmlHelper();

            // Execute
            string html = htmlHelper.RouteLink("linktext", "namedroute");

            // Verify
            Assert.AreEqual<string>(@"<a href=""" + AppPathModifier + @"/app/named/home/oldaction"">linktext</a>", html);
        }

        [TestMethod]
        public void RouteLinkWithRouteNameAndDictionary() {
            // Setup
            HtmlHelper htmlHelper = GetHtmlHelper();

            // Execute
            string html = htmlHelper.RouteLink("linktext", "namedroute", new RouteValueDictionary(new { Action = "newaction", Controller = "home2", id = "someid" }));

            // Verify
            Assert.AreEqual<string>(@"<a href=""" + AppPathModifier + @"/app/named/home2/newaction/someid"">linktext</a>", html);
        }

        [TestMethod]
        public void RouteLinkWithRouteNameAndObjectProperties() {
            // Setup
            HtmlHelper htmlHelper = GetHtmlHelper();

            // Execute
            string html = htmlHelper.RouteLink("linktext", "namedroute", new { Action = "newaction", Controller = "home2", id = "someid" });

            // Verify
            Assert.AreEqual<string>(@"<a href=""" + AppPathModifier + @"/app/named/home2/newaction/someid"">linktext</a>", html);
        }

        public static HttpContextBase GetHttpContext(string appPath, string requestPath, string httpMethod) {
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
            mockContext.Expect(o => o.Session).Returns((HttpSessionStateBase)null);

            Mock<HttpResponseBase> mockResponse = new Mock<HttpResponseBase>();
            mockResponse.Expect(o => o.ApplyAppPathModifier(It.IsAny<string>())).Returns<string>(r => AppPathModifier + r);
            mockContext.Expect(o => o.Response).Returns(mockResponse.Object);

            return mockContext.Object;
        }

        [TestMethod]
        public void ToStringDictionary() {
            // Setup
            Dictionary<int, object> original = new Dictionary<int, object> { { 1, 'a' }, { 2, 'b' } };

            // Execute
            Dictionary<string, string> newDictionary = HtmlHelper.ToStringDictionary(original);

            // Verify
            Assert.AreEqual(StringComparer.OrdinalIgnoreCase, newDictionary.Comparer);
            Assert.AreEqual(2, newDictionary.Count);
            Assert.AreEqual("a", newDictionary["1"]);
            Assert.AreEqual("b", newDictionary["2"]);
        }

        [TestMethod]
        public void ToStringDictionaryWithNullParameter() {
            // Execute
            Dictionary<string, string> newDictionary = HtmlHelper.ToStringDictionary((Dictionary<string, string>)null);

            // Verify
            Assert.AreEqual(StringComparer.OrdinalIgnoreCase, newDictionary.Comparer);
            Assert.AreEqual(0, newDictionary.Count);
        }

        [TestMethod]
        public void TryAddValueReturnsFalseIfKeyAlreadyExists() {
            // Setup
            Dictionary<string, string> dictionary = new Dictionary<string, string> { { "foo", "OldValue" } };

            // Execute
            bool wasAdded = HtmlHelper.TryAddValue(dictionary, "foo", "NewValue");

            // Verify
            Assert.IsFalse(wasAdded);
            Assert.AreEqual("OldValue", dictionary["foo"]);
        }

        [TestMethod]
        public void TryAddValueReturnsTrueIfKeyIsAdded() {
            // Setup
            Dictionary<string, string> dictionary = new Dictionary<string, string>();

            // Execute
            bool wasAdded = HtmlHelper.TryAddValue(dictionary, "foo", "NewValue");

            // Verify
            Assert.IsTrue(wasAdded);
            Assert.AreEqual("NewValue", dictionary["foo"]);
        }

        private static HtmlHelper GetHtmlHelper() {
            HttpContextBase httpcontext = GetHttpContext("/app/", null, null);
            RouteCollection rt = new RouteCollection();
            rt.Add(new Route("{controller}/{action}/{id}", null) { Defaults = new RouteValueDictionary(new { id = "defaultid" }) });
            rt.Add("namedroute", new Route("named/{controller}/{action}/{id}", null) { Defaults = new RouteValueDictionary(new { id = "defaultid" }) });
            RouteData rd = new RouteData();
            rd.Values.Add("controller", "home");
            rd.Values.Add("action", "oldaction");
            ViewContext context = new ViewContext(httpcontext, rd, new Mock<IController>().Object, "view", null, new ViewDataDictionary(), new TempDataDictionary(ControllerContextTest.GetEmptyContextForTempData()));
            HtmlHelper htmlHelper = new HtmlHelper(context, new Mock<IViewDataContainer>().Object);
            htmlHelper.RouteCollection = rt;
            return htmlHelper;
        }

        private static HtmlHelper GetHtmlHelper(ViewDataDictionary viewData) {
            ViewContext viewContext = GetViewContext();
            Mock<IViewDataContainer> mockContainer = new Mock<IViewDataContainer>();
            mockContainer.Expect(c => c.ViewData).Returns(viewData);
            IViewDataContainer container = mockContainer.Object;
            return new HtmlHelper(viewContext, container);
        }

        private static ViewContext GetViewContext() {
            ViewContext viewContext = new ViewContext(new Mock<HttpContextBase>().Object,
                new RouteData(),
                new Mock<IController>().Object,
                "view",
                null,
                new ViewDataDictionary(),
                new TempDataDictionary(ControllerContextTest.GetEmptyContextForTempData()));
            return viewContext;
        }
    }
}
