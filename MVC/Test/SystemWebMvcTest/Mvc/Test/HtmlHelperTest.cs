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
            // Arrange
            ViewContext viewContext = GetViewContext();
            HtmlHelper htmlHelper = new HtmlHelper(viewContext, new Mock<IViewDataContainer>().Object);

            // Act
            ViewContext value = htmlHelper.ViewContext;

            // Assert
            Assert.AreEqual(viewContext, value);
        }

        [TestMethod]
        public void ViewDataContainerProperty() {
            // Arrange
            ViewContext viewContext = GetViewContext();
            IViewDataContainer container = new Mock<IViewDataContainer>().Object;
            HtmlHelper htmlHelper = new HtmlHelper(viewContext, container);

            // Act
            IViewDataContainer value = htmlHelper.ViewDataContainer;

            // Assert
            Assert.AreEqual(container, value);
        }

        [TestMethod]
        public void ConstructorWithNullViewContextThrows() {
            // Assert
            ExceptionHelper.ExpectArgumentNullException(
                delegate {
                    new HtmlHelper(null, null);
                },
                "viewContext");
        }

        [TestMethod]
        public void ConstructorWithNullViewDataContainerThrows() {
            // Assert
            ExceptionHelper.ExpectArgumentNullException(
                delegate {
                    new HtmlHelper(GetViewContext(), null);
                },
                "viewDataContainer");
        }

        [TestMethod]
        public void ActionLink() {
            // Arrange
            HtmlHelper htmlHelper = GetHtmlHelper();

            // Act
            string html = htmlHelper.ActionLink("linktext", "newaction");

            // Assert
            Assert.AreEqual<string>(@"<a href=""" + AppPathModifier + @"/app/home/newaction"">linktext</a>", html);
        }

        [TestMethod]
        public void ActionLinkParametersNeedEscaping() {
            // Arrange
            HtmlHelper htmlHelper = GetHtmlHelper();

            // Act
            string html = htmlHelper.ActionLink("linktext<&>\"", "new action<&>\"");

            // Assert
            Assert.AreEqual<string>(@"<a href=""" + AppPathModifier + @"/app/home/new%20action%3C&amp;%3E%22"">linktext&lt;&amp;&gt;&quot;</a>", html);
        }

        [TestMethod]
        public void ActionLinkWithActionNameAndValueDictionary() {
            // Arrange
            HtmlHelper htmlHelper = GetHtmlHelper();

            // Act
            string html = htmlHelper.ActionLink("linktext", "newaction", new RouteValueDictionary(new { controller = "home2" }));

            // Assert
            Assert.AreEqual<string>(@"<a href=""" + AppPathModifier + @"/app/home2/newaction"">linktext</a>", html);
        }

        [TestMethod]
        public void ActionLinkWithActionNameAndValueObject() {
            // Arrange
            HtmlHelper htmlHelper = GetHtmlHelper();

            // Act
            string html = htmlHelper.ActionLink("linktext", "newaction", new { controller = "home2" });

            // Assert
            Assert.AreEqual<string>(@"<a href=""" + AppPathModifier + @"/app/home2/newaction"">linktext</a>", html);
        }

        [TestMethod]
        public void ActionLinkWithControllerName() {
            // Arrange
            HtmlHelper htmlHelper = GetHtmlHelper();

            // Act
            string html = htmlHelper.ActionLink("linktext", "newaction", "home2");

            // Assert
            Assert.AreEqual<string>(@"<a href=""" + AppPathModifier + @"/app/home2/newaction"">linktext</a>", html);
        }

        [TestMethod]
        public void ActionLinkWithControllerNameAndDictionary() {
            // Arrange
            HtmlHelper htmlHelper = GetHtmlHelper();

            // Act
            string html = htmlHelper.ActionLink("linktext", "newaction", "home2", new RouteValueDictionary(new { id = "someid" }), new RouteValueDictionary(new { baz = "baz" }));

            // Assert
            Assert.AreEqual<string>(@"<a baz=""baz"" href=""" + AppPathModifier + @"/app/home2/newaction/someid"">linktext</a>", html);
        }

        [TestMethod]
        public void ActionLinkWithControllerNameAndObjectProperties() {
            // Arrange
            HtmlHelper htmlHelper = GetHtmlHelper();

            // Act
            string html = htmlHelper.ActionLink("linktext", "newaction", "home2", new { id = "someid" }, new { baz = "baz" });

            // Assert
            Assert.AreEqual<string>(@"<a baz=""baz"" href=""" + AppPathModifier + @"/app/home2/newaction/someid"">linktext</a>", html);
        }

        [TestMethod]
        public void ActionLinkWithDictionary() {
            // Arrange
            HtmlHelper htmlHelper = GetHtmlHelper();

            // Act
            string html = htmlHelper.ActionLink("linktext", "newaction", new RouteValueDictionary(new { Controller = "home2", id = "someid" }), new RouteValueDictionary(new { baz = "baz" }));

            // Assert
            Assert.AreEqual<string>(@"<a baz=""baz"" href=""" + AppPathModifier + @"/app/home2/newaction/someid"">linktext</a>", html);
        }

        [TestMethod]
        public void ActionLinkWithFragment() {
            // Arrange
            HtmlHelper htmlHelper = GetHtmlHelper();

            // Act
            string html = htmlHelper.ActionLink("linktext", "newaction", "home2", "http", "foo.bar.com", "foo", new { id = "someid" }, new { baz = "baz" });

            // Assert
            Assert.AreEqual<string>(@"<a baz=""baz"" href=""http://foo.bar.com" + AppPathModifier + @"/app/home2/newaction/someid#foo"">linktext</a>", html);
        }

        [TestMethod]
        public void ActionLinkWithNullHostname() {
            // Arrange
            HtmlHelper htmlHelper = GetHtmlHelper();

            // Act
            string html = htmlHelper.ActionLink("linktext", "newaction", "home2", "https", null /* hostName */, "foo", new { id = "someid" }, new { baz = "baz" });

            // Assert
            Assert.AreEqual<string>(@"<a baz=""baz"" href=""https://localhost" + AppPathModifier + @"/app/home2/newaction/someid#foo"">linktext</a>", html);
        }

        [TestMethod]
        public void ActionLinkWithNullProtocolAndFragment() {
            // Arrange
            HtmlHelper htmlHelper = GetHtmlHelper();

            // Act
            string html = htmlHelper.ActionLink("linktext", "newaction", "home2", null /* protocol */, "foo.bar.com", null /* fragment */, new { id = "someid" }, new { baz = "baz" });

            // Assert
            Assert.AreEqual<string>(@"<a baz=""baz"" href=""http://foo.bar.com" + AppPathModifier + @"/app/home2/newaction/someid"">linktext</a>", html);
        }

        [TestMethod]
        public void ActionLinkWithNullProtocolNullHostNameAndNullFragment() {
            // Arrange
            HtmlHelper htmlHelper = GetHtmlHelper();

            // Act
            string html = htmlHelper.ActionLink("linktext", "newaction", "home2", null /* protocol */, null /* hostName */, null /* fragment */, new { id = "someid" }, new { baz = "baz" });

            // Assert
            Assert.AreEqual<string>(@"<a baz=""baz"" href=""" + AppPathModifier + @"/app/home2/newaction/someid"">linktext</a>", html);
        }

        [TestMethod]
        public void ActionLinkWithObjectProperties() {
            // Arrange
            HtmlHelper htmlHelper = GetHtmlHelper();

            // Act
            string html = htmlHelper.ActionLink("linktext", "newaction", new { Controller = "home2", id = "someid" }, new { baz = "baz" });

            // Assert
            Assert.AreEqual<string>(@"<a baz=""baz"" href=""" + AppPathModifier + @"/app/home2/newaction/someid"">linktext</a>", html);
        }

        [TestMethod]
        public void ActionLinkWithProtocol() {
            // Arrange
            HtmlHelper htmlHelper = GetHtmlHelper();

            // Act
            string html = htmlHelper.ActionLink("linktext", "newaction", "home2", "https", "foo.bar.com", null /* fragment */, new { id = "someid" }, new { baz = "baz" });

            // Assert
            Assert.AreEqual<string>(@"<a baz=""baz"" href=""https://foo.bar.com" + AppPathModifier + @"/app/home2/newaction/someid"">linktext</a>", html);
        }

        [TestMethod]
        public void ActionLinkWithProtocolAndFragment() {
            // Arrange
            HtmlHelper htmlHelper = GetHtmlHelper();

            // Act
            string html = htmlHelper.ActionLink("linktext", "newaction", "home2", "https", "foo.bar.com", "foo", new { id = "someid" }, new { baz = "baz" });

            // Assert
            Assert.AreEqual<string>(@"<a baz=""baz"" href=""https://foo.bar.com" + AppPathModifier + @"/app/home2/newaction/someid#foo"">linktext</a>", html);
        }

        [TestMethod]
        public void AttributeEncodeObject() {
            //Setup
            HtmlHelper htmlHelper = GetHtmlHelper();

            //Execute
            string encodedHtml = htmlHelper.AttributeEncode((object)@"<"">");

            // Assert
            Assert.AreEqual(encodedHtml, "&lt;&quot;>", "Text is not being properly HTML attribute-encoded.");
        }

        [TestMethod]
        public void AttributeEncodeObjectNull() {
            //Setup
            HtmlHelper htmlHelper = GetHtmlHelper();

            //Execute
            string encodedHtml = htmlHelper.AttributeEncode((object)null);

            // Assert
            Assert.AreEqual("", encodedHtml);
        }

        [TestMethod]
        public void AttributeEncodeString() {
            //Setup
            HtmlHelper htmlHelper = GetHtmlHelper();

            //Execute
            string encodedHtml = htmlHelper.AttributeEncode(@"<"">");

            // Assert
            Assert.AreEqual(encodedHtml, "&lt;&quot;>", "Text is not being properly HTML attribute-encoded.");
        }

        [TestMethod]
        public void AttributeEncodeStringNull() {
            //Setup
            HtmlHelper htmlHelper = GetHtmlHelper();

            //Execute
            string encodedHtml = htmlHelper.AttributeEncode((string)null);

            // Assert
            Assert.AreEqual("", encodedHtml);
        }

        [TestMethod]
        public void EncodeObject() {
            //Setup
            HtmlHelper htmlHelper = GetHtmlHelper();

            //Execute
            string encodedHtml = htmlHelper.Encode((object)"<br />");

            // Assert
            Assert.AreEqual(encodedHtml, "&lt;br /&gt;", "Text is not being properly HTML-encoded.");
        }

        [TestMethod]
        public void EncodeObjectNull() {
            //Setup
            HtmlHelper htmlHelper = GetHtmlHelper();

            //Execute
            string encodedHtml = htmlHelper.Encode((object)null);

            // Assert
            Assert.AreEqual("", encodedHtml);
        }

        [TestMethod]
        public void EncodeString() {
            //Setup
            HtmlHelper htmlHelper = GetHtmlHelper();

            //Execute
            string encodedHtml = htmlHelper.Encode("<br />");

            // Assert
            Assert.AreEqual(encodedHtml, "&lt;br /&gt;", "Text is not being properly HTML-encoded.");
        }

        [TestMethod]
        public void EncodeStringNull() {
            //Setup
            HtmlHelper htmlHelper = GetHtmlHelper();

            //Execute
            string encodedHtml = htmlHelper.Encode((string)null);

            // Assert
            Assert.AreEqual("", encodedHtml);
        }

        [TestMethod]
        public void FormDisposableWritesFormCloseTagToOutputStream() {
            // Arrange
            Mock<HttpRequestBase> mockHttpRequest = new Mock<HttpRequestBase>();
            mockHttpRequest.Expect(r => r.Url).Returns(new Uri("http://www.contoso.com/some/path"));
            Mock<HttpResponseBase> mockHttpResponse = new Mock<HttpResponseBase>(MockBehavior.Strict);
            mockHttpResponse.Expect(r => r.Write(@"<form action=""http://www.contoso.com/some/path"" method=""post"">")).Verifiable();
            mockHttpResponse.Expect(r => r.Write(@"</form>")).AtMostOnce().Verifiable();
            Mock<HttpContextBase> mockHttpContext = new Mock<HttpContextBase>();
            mockHttpContext.Expect(c => c.Request).Returns(mockHttpRequest.Object);
            mockHttpContext.Expect(c => c.Response).Returns(mockHttpResponse.Object);

            HtmlHelper helper = new HtmlHelper(
                new ViewContext(mockHttpContext.Object, new RouteData(), new Mock<ControllerBase>().Object, "view", new ViewDataDictionary(), new TempDataDictionary()),
                new Mock<IViewDataContainer>().Object);

            // Act
            IDisposable formDisposable = helper.Form();
            formDisposable.Dispose();
            formDisposable.Dispose(); // should write to the output stream only once, even if called two times

            // Assert
            mockHttpResponse.Verify();
        }

        [TestMethod]
        public void FormWritesFormStartTagToOutputString() {
            // Arrange
            Mock<HttpRequestBase> mockHttpRequest = new Mock<HttpRequestBase>();
            mockHttpRequest.Expect(r => r.Url).Returns(new Uri("http://www.contoso.com/some/path"));
            Mock<HttpResponseBase> mockHttpResponse = new Mock<HttpResponseBase>(MockBehavior.Strict);
            mockHttpResponse.Expect(r => r.Write(@"<form action=""http://www.contoso.com/some/path"" method=""post"">")).Verifiable();
            Mock<HttpContextBase> mockHttpContext = new Mock<HttpContextBase>();
            mockHttpContext.Expect(c => c.Request).Returns(mockHttpRequest.Object);
            mockHttpContext.Expect(c => c.Response).Returns(mockHttpResponse.Object);

            HtmlHelper helper = new HtmlHelper(
                new ViewContext(mockHttpContext.Object, new RouteData(), new Mock<ControllerBase>().Object, "view", new ViewDataDictionary(), new TempDataDictionary()),
                new Mock<IViewDataContainer>().Object);

            // Act
            helper.Form();

            // Assert
            mockHttpResponse.Verify();
        }

        [TestMethod]
        public void LinkGenerationDoesNotChangeProvidedDictionary() {
            // Arrange
            HtmlHelper htmlHelper = GetHtmlHelper();
            RouteValueDictionary valuesDictionary = new RouteValueDictionary();

            // Act
            htmlHelper.ActionLink("linkText", "actionName", valuesDictionary, new RouteValueDictionary());

            // Assert
            Assert.IsFalse(valuesDictionary.ContainsKey("action"));
        }

        [TestMethod]
        public void LinkGenerationThrowsIfDictionaryAlreadyContainsActionName() {
            // Arrange
            HtmlHelper htmlHelper = GetHtmlHelper();
            RouteValueDictionary valuesDictionary = new RouteValueDictionary(new { Action = "someaction" });

            // Act & Assert
            ExceptionHelper.ExpectArgumentException(
                delegate() {
                    htmlHelper.ActionLink("linkText", "action", valuesDictionary, (RouteValueDictionary)null /* htmlAttributes */);
                },
                "The provided object or dictionary already contains a definition for 'action'.\r\nParameter name: actionName");
        }

        [TestMethod]
        public void LinkGenerationThrowsIfDictionaryAlreadyContainsControllerName() {
            // Arrange
            HtmlHelper htmlHelper = GetHtmlHelper();
            RouteValueDictionary valuesDictionary = new RouteValueDictionary(new { Controller = "somecontroller" });

            // Act & Assert
            ExceptionHelper.ExpectArgumentException(
                delegate() {
                    htmlHelper.ActionLink("linkText", "action", "controller", valuesDictionary, new RouteValueDictionary());
                },
                "The provided object or dictionary already contains a definition for 'controller'.\r\nParameter name: controllerName");
        }

        [TestMethod]
        public void NullDictionaryParameterThrows() {
            // Arrange
            HtmlHelper htmlHelper = GetHtmlHelper();
            GenericDelegate[] tests = new GenericDelegate[] {
                () => htmlHelper.ActionLink("linkText", "actionName", (RouteValueDictionary)null, (RouteValueDictionary)null),
                () => htmlHelper.ActionLink("linkText", "actionName", "controllerName", (RouteValueDictionary)null, (RouteValueDictionary)null),
                () => htmlHelper.RouteLink("linkText", (RouteValueDictionary)null, (RouteValueDictionary)null),
                () => htmlHelper.RouteLink("linkText", "routeName", (RouteValueDictionary)null, (RouteValueDictionary)null)
            };

            // Act & Assert
            foreach (GenericDelegate test in tests) {
                ExceptionHelper.ExpectArgumentNullException(test, "values");
            }
        }

        [TestMethod]
        public void NullOrEmptyStringParameterThrows() {
            // Arrange
            HtmlHelper htmlHelper = GetHtmlHelper();
            Func<Action, GenericDelegate> Wrap = action => new GenericDelegate(() => action());
            var tests = new[] {
                // ActionLink(string linkText, string actionName)
                new { Parameter = "linkText", Action = Wrap(() => htmlHelper.ActionLink(String.Empty, "actionName")) },
                new { Parameter = "actionName", Action = Wrap(() => htmlHelper.ActionLink("linkText", String.Empty)) },

                // ActionLink(string linkText, string actionName, object values, object htmlAttributes)
                new { Parameter = "linkText", Action = Wrap(() => htmlHelper.ActionLink(String.Empty, "actionName", new Object(), null /* htmlAttributes */)) },
                new { Parameter = "actionName", Action = Wrap(() => htmlHelper.ActionLink("linkText", String.Empty, new Object(), null /* htmlAttributes */)) },

                // ActionLink(string linkText, string actionName, RouteValueDictionary valuesDictionary, RouteValueDictionary htmlAttributes)
                new { Parameter = "linkText", Action = Wrap(() => htmlHelper.ActionLink(String.Empty, "actionName", new RouteValueDictionary(), new RouteValueDictionary())) },
                new { Parameter = "actionName", Action = Wrap(() => htmlHelper.ActionLink("linkText", String.Empty, new RouteValueDictionary(), new RouteValueDictionary())) },

                // ActionLink(string linkText, string actionName, string controllerName)
                new { Parameter = "linkText", Action = Wrap(() => htmlHelper.ActionLink(String.Empty, "actionName", "controllerName")) },
                new { Parameter = "actionName", Action = Wrap(() => htmlHelper.ActionLink("linkText", String.Empty, "controllerName")) },
                new { Parameter = "controllerName", Action = Wrap(() => htmlHelper.ActionLink("linkText", "actionName", String.Empty)) },

                // ActionLink(string linkText, string actionName, string controllerName, object values, object htmlAttributes)
                new { Parameter = "linkText", Action = Wrap(() => htmlHelper.ActionLink(String.Empty, "actionName", "controllerName", new Object(), null /* htmlAttributes */)) },
                new { Parameter = "actionName", Action = Wrap(() => htmlHelper.ActionLink("linkText", String.Empty, "controllerName", new Object(), null /* htmlAttributes */)) },
                new { Parameter = "controllerName", Action = Wrap(() => htmlHelper.ActionLink("linkText", "actionName", String.Empty, new Object(), null /* htmlAttributes */)) },

                // ActionLink(string linkText, string actionName, string controllerName, RouteValueDictionary valuesDictionary, RouteValueDictionary htmlAttributes)
                new { Parameter = "linkText", Action = Wrap(() => htmlHelper.ActionLink(String.Empty, "actionName", "controllerName", new RouteValueDictionary(), new RouteValueDictionary())) },
                new { Parameter = "actionName", Action = Wrap(() => htmlHelper.ActionLink("linkText", String.Empty, "controllerName", new RouteValueDictionary(), new RouteValueDictionary())) },
                new { Parameter = "controllerName", Action = Wrap(() => htmlHelper.ActionLink("linkText", "actionName", String.Empty, new RouteValueDictionary(), new RouteValueDictionary())) },

                // RouteLink(string linkText, object values, object htmlAttributes)
                new { Parameter = "linkText", Action = Wrap(() => htmlHelper.RouteLink(String.Empty, new Object(), null /* htmlAttributes */)) },

                // RouteLink(string linkText, RouteValueDictionary valuesDictionary, RouteValueDictionary htmlAttributes)
                new { Parameter = "linkText", Action = Wrap(() => htmlHelper.RouteLink(String.Empty, new RouteValueDictionary(), new RouteValueDictionary())) },

                // RouteLink(string linkText, string routeName, object htmlAttributes)
                new { Parameter = "linkText", Action = Wrap(() => htmlHelper.RouteLink(String.Empty, "routeName", null /* htmlAttributes */)) },
                new { Parameter = "routeName", Action = Wrap(() => htmlHelper.RouteLink("linkText", String.Empty, null /* htmlAttributes */)) },

                // RouteLink(string linkText, string routeName, object values, object htmlAttributes)
                new { Parameter = "linkText", Action = Wrap(() => htmlHelper.RouteLink(String.Empty, "routeName", new Object(), null /* htmlAttributes */)) },
                new { Parameter = "routeName", Action = Wrap(() => htmlHelper.RouteLink("linkText", String.Empty, new Object(), null /* htmlAttributes */)) },

                // RouteLink(string linkText, string routeName, RouteValueDictionary valuesDictionary, RouteValueDictionary htmlAttributes)
                new { Parameter = "linkText", Action = Wrap(() => htmlHelper.RouteLink(String.Empty, "routeName", new RouteValueDictionary(), new RouteValueDictionary())) },
                new { Parameter = "routeName", Action = Wrap(() => htmlHelper.RouteLink("linkText", String.Empty, new RouteValueDictionary(), new RouteValueDictionary())) }
            };

            // Act & Assert
            foreach (var test in tests) {
                ExceptionHelper.ExpectArgumentExceptionNullOrEmpty(test.Action, test.Parameter);
            }
        }

        [TestMethod]
        public void RouteLinkWithDictionary() {
            // Arrange
            HtmlHelper htmlHelper = GetHtmlHelper();

            // Act
            string html = htmlHelper.RouteLink("linktext", new RouteValueDictionary(new { Action = "newaction", Controller = "home2", id = "someid" }), new RouteValueDictionary(new { baz = "baz" }));

            // Assert
            Assert.AreEqual<string>(@"<a baz=""baz"" href=""" + AppPathModifier + @"/app/home2/newaction/someid"">linktext</a>", html);
        }

        [TestMethod]
        public void RouteLinkWithFragment() {
            // Arrange
            HtmlHelper htmlHelper = GetHtmlHelper();

            // Act
            string html = htmlHelper.RouteLink("linktext", "namedroute", "http", "foo.bar.com", "foo", new { Action = "newaction", Controller = "home2", id = "someid" }, new { baz = "baz" });

            // Assert
            Assert.AreEqual<string>(@"<a baz=""baz"" href=""http://foo.bar.com" + AppPathModifier + @"/app/named/home2/newaction/someid#foo"">linktext</a>", html);
        }

        [TestMethod]
        public void RouteLinkWithObjectProperties() {
            // Arrange
            HtmlHelper htmlHelper = GetHtmlHelper();

            // Act
            string html = htmlHelper.RouteLink("linktext", new { Action = "newaction", Controller = "home2", id = "someid" }, new { baz = "baz" });

            // Assert
            Assert.AreEqual<string>(@"<a baz=""baz"" href=""" + AppPathModifier + @"/app/home2/newaction/someid"">linktext</a>", html);
        }

        [TestMethod]
        public void RouteLinkWithProtocol() {
            // Arrange
            HtmlHelper htmlHelper = GetHtmlHelper();

            // Act
            string html = htmlHelper.RouteLink("linktext", "namedroute", "https", "foo.bar.com", null /* fragment */, new { Action = "newaction", Controller = "home2", id = "someid" }, new { baz = "baz" });

            // Assert
            Assert.AreEqual<string>(@"<a baz=""baz"" href=""https://foo.bar.com" + AppPathModifier + @"/app/named/home2/newaction/someid"">linktext</a>", html);
        }

        [TestMethod]
        public void RouteLinkWithProtocolAndFragment() {
            // Arrange
            HtmlHelper htmlHelper = GetHtmlHelper();

            // Act
            string html = htmlHelper.RouteLink("linktext", "namedroute", "https", "foo.bar.com", "foo", new { Action = "newaction", Controller = "home2", id = "someid" }, new { baz = "baz" });

            // Assert
            Assert.AreEqual<string>(@"<a baz=""baz"" href=""https://foo.bar.com" + AppPathModifier + @"/app/named/home2/newaction/someid#foo"">linktext</a>", html);
        }

        [TestMethod]
        public void RouteLinkWithRouteNameAndDefaults() {
            // Arrange
            HtmlHelper htmlHelper = GetHtmlHelper();

            // Act
            string html = htmlHelper.RouteLink("linktext", "namedroute", new { baz = "baz" });

            // Assert
            Assert.AreEqual<string>(@"<a baz=""baz"" href=""" + AppPathModifier + @"/app/named/home/oldaction"">linktext</a>", html);
        }

        [TestMethod]
        public void RouteLinkWithRouteNameAndDictionary() {
            // Arrange
            HtmlHelper htmlHelper = GetHtmlHelper();

            // Act
            string html = htmlHelper.RouteLink("linktext", "namedroute", new RouteValueDictionary(new { Action = "newaction", Controller = "home2", id = "someid" }), new RouteValueDictionary());

            // Assert
            Assert.AreEqual<string>(@"<a href=""" + AppPathModifier + @"/app/named/home2/newaction/someid"">linktext</a>", html);
        }

        [TestMethod]
        public void RouteLinkWithRouteNameAndObjectProperties() {
            // Arrange
            HtmlHelper htmlHelper = GetHtmlHelper();

            // Act
            string html = htmlHelper.RouteLink("linktext", "namedroute", new { Action = "newaction", Controller = "home2", id = "someid" }, new { baz = "baz" });

            // Assert
            Assert.AreEqual<string>(@"<a baz=""baz"" href=""" + AppPathModifier + @"/app/named/home2/newaction/someid"">linktext</a>", html);
        }

        [TestMethod]
        public void ValidationMessageWithEmptyNameThrows() {
            // Arrange
            HtmlHelper htmlHelper = GetHtmlHelper();

            // Act & Assert
            ExceptionHelper.ExpectArgumentExceptionNullOrEmpty(
                delegate {
                    htmlHelper.ValidationMessage(String.Empty);
                },
                "modelName");
        }

        [TestMethod]
        public void ValidationMessageReturnsFirstError() {
            // Arrange
            HtmlHelper htmlHelper = GetHtmlHelper(GetViewDataWithModelErrors());

            // Act 
            string html = htmlHelper.ValidationMessage("foo");

            // Assert
            Assert.AreEqual(@"<span class=""field-validation-error"">foo error &lt;1&gt;</span>", html);
        }

        [TestMethod]
        public void ValidationMessageReturnsNullForInvalidName() {
            // Arrange
            HtmlHelper htmlHelper = GetHtmlHelper(GetViewDataWithModelErrors());

            // Act
            string html = htmlHelper.ValidationMessage("boo");

            // Assert
            Assert.IsNull(html, "html should be null if name is invalid.");
        }

        [TestMethod]
        public void ValidationMessageReturnsWithObjectAttributes() {
            // Arrange
            HtmlHelper htmlHelper = GetHtmlHelper(GetViewDataWithModelErrors());

            // Act
            string html = htmlHelper.ValidationMessage("foo", new { bar = "bar" });

            // Assert
            Assert.AreEqual(@"<span bar=""bar"" class=""field-validation-error"">foo error &lt;1&gt;</span>", html);
        }

        [TestMethod]
        public void ValidationMessageReturnsWithCustomClassOverridesDefault() {
            // Arrange
            HtmlHelper htmlHelper = GetHtmlHelper(GetViewDataWithModelErrors());

            // Act
            string html = htmlHelper.ValidationMessage("foo", new { @class = "my-custom-css-class" });

            // Assert
            Assert.AreEqual(@"<span class=""my-custom-css-class"">foo error &lt;1&gt;</span>", html);
        }

        [TestMethod]
        public void ValidationMessageReturnsWithCustomMessage() {
            // Arrange
            HtmlHelper htmlHelper = GetHtmlHelper(GetViewDataWithModelErrors());

            // Act
            string html = htmlHelper.ValidationMessage("foo", "bar error");

            // Assert
            Assert.AreEqual(@"<span class=""field-validation-error"">bar error</span>", html);
        }

        [TestMethod]
        public void ValidationMessageReturnsWithCustomMessageAndObjectAttributes() {
            // Arrange
            HtmlHelper htmlHelper = GetHtmlHelper(GetViewDataWithModelErrors());

            // Act
            string html = htmlHelper.ValidationMessage("foo", "bar error", new { baz = "baz" });

            // Assert
            Assert.AreEqual(@"<span baz=""baz"" class=""field-validation-error"">bar error</span>", html);
        }

        [TestMethod]
        public void ValidationMessageWithModelStateAndNoErrors() {
            // Arrange
            HtmlHelper htmlHelper = GetHtmlHelper(GetViewDataWithModelErrors());

            // Act
            string html = htmlHelper.ValidationMessage("baz");

            // Assert
            Assert.IsNull(html, "html should be null if there are no errors");
        }

        [TestMethod]
        public void ValidationSummary() {
            // Arrange
            HtmlHelper htmlHelper = GetHtmlHelper(GetViewDataWithModelErrors());

            // Act
            string html = htmlHelper.ValidationSummary();

            // Assert
            Assert.AreEqual(@"<ul class=""validation-summary-errors""><li>foo error &lt;1&gt;</li>
<li>foo error 2</li>
<li>bar error &lt;1&gt;</li>
<li>bar error 2</li>
</ul>"
                , html);
        }

        [TestMethod]
        public void ValidationSummaryWithDictionary() {
            // Arrange
            HtmlHelper htmlHelper = GetHtmlHelper(GetViewDataWithModelErrors());
            RouteValueDictionary htmlAttributes = new RouteValueDictionary();
            htmlAttributes["class"] = "my-class";

            // Act
            string html = htmlHelper.ValidationSummary(htmlAttributes);

            // Assert
            Assert.AreEqual(@"<ul class=""my-class""><li>foo error &lt;1&gt;</li>
<li>foo error 2</li>
<li>bar error &lt;1&gt;</li>
<li>bar error 2</li>
</ul>"
                , html);
        }

        [TestMethod]
        public void ValidationSummaryWithNoErrorsReturnsNull() {
            // Arrange
            HtmlHelper htmlHelper = GetHtmlHelper(new ViewDataDictionary());

            // Act
            string html = htmlHelper.ValidationSummary();

            // Assert
            Assert.IsNull(html, "html should be null if there are no errors to report.");
        }

        [TestMethod]
        public void ValidationSummaryWithObjectAttributes() {
            // Arrange
            HtmlHelper htmlHelper = GetHtmlHelper(GetViewDataWithModelErrors());

            // Act
            string html = htmlHelper.ValidationSummary(new { baz = "baz" });

            // Assert
            Assert.AreEqual(@"<ul baz=""baz"" class=""validation-summary-errors""><li>foo error &lt;1&gt;</li>
<li>foo error 2</li>
<li>bar error &lt;1&gt;</li>
<li>bar error 2</li>
</ul>"
                , html);
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

            Uri uri = new Uri("http://localhost");
            mockRequest.Expect(o => o.Url).Returns(uri);

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

        private static HtmlHelper GetHtmlHelper() {
            HttpContextBase httpcontext = GetHttpContext("/app/", null, null);
            RouteCollection rt = new RouteCollection();
            rt.Add(new Route("{controller}/{action}/{id}", null) { Defaults = new RouteValueDictionary(new { id = "defaultid" }) });
            rt.Add("namedroute", new Route("named/{controller}/{action}/{id}", null) { Defaults = new RouteValueDictionary(new { id = "defaultid" }) });
            RouteData rd = new RouteData();
            rd.Values.Add("controller", "home");
            rd.Values.Add("action", "oldaction");

            ViewDataDictionary vdd = new ViewDataDictionary();
            ViewContext context = new ViewContext(httpcontext, rd, new Mock<ControllerBase>().Object, "view", vdd, new TempDataDictionary());
            Mock<IViewDataContainer> mockVdc = new Mock<IViewDataContainer>();
            mockVdc.Expect(vdc => vdc.ViewData).Returns(vdd);
            HtmlHelper htmlHelper = new HtmlHelper(context, mockVdc.Object);
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
                new Mock<ControllerBase>().Object,
                "view",
                new ViewDataDictionary(),
                new TempDataDictionary());
            return viewContext;
        }

        private static ViewDataDictionary GetViewDataWithModelErrors() {
            ViewDataDictionary viewData = new ViewDataDictionary();
            ModelState modelStateFoo = new ModelState();
            ModelState modelStateBar = new ModelState();
            ModelState modelStateBaz = new ModelState();
            modelStateFoo.Errors.Add(new ModelError("foo error <1>"));
            modelStateFoo.Errors.Add(new ModelError("foo error 2"));
            modelStateBar.Errors.Add(new ModelError("bar error <1>"));
            modelStateBar.Errors.Add(new ModelError("bar error 2"));
            viewData.ModelState["foo"] = modelStateFoo;
            viewData.ModelState["bar"] = modelStateBar;
            viewData.ModelState["baz"] = modelStateBaz;
            return viewData;
        }
    }
}
