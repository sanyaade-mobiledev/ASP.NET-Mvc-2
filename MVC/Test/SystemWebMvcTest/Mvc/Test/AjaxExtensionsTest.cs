namespace System.Web.Mvc.Ajax.Test {
    using System.Collections.Generic;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Mvc.Test;
    using System.Web.Routing;
    using System.Web.TestUtil;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;

    [TestClass]
    public class AjaxExtensionsTest {
        private const string AjaxFormWithDefaultController = @"<form action=""" + AppPathModifier + @"/app/home/Action"" method=""post"" onsubmit=""Sys.Mvc.AsyncForm.handleSubmit(this, new Sys.UI.DomEvent(event), { insertionMode: Sys.Mvc.InsertionMode.replace });"">";
        private const string AjaxFormWithId = @"<form action=""" + AppPathModifier + @"/app/Controller/Action/5"" method=""post"" onsubmit=""Sys.Mvc.AsyncForm.handleSubmit(this, new Sys.UI.DomEvent(event), { insertionMode: Sys.Mvc.InsertionMode.replace });"">";
        private const string AjaxFormWithIdAndHtmlAttributes = @"<form action=""" + AppPathModifier + @"/app/Controller/Action/5"" method=""get"" onsubmit=""Sys.Mvc.AsyncForm.handleSubmit(this, new Sys.UI.DomEvent(event), { insertionMode: Sys.Mvc.InsertionMode.replace });"">";
        private const string AjaxFormWithEmptyOptions = @"<form action=""" + AppPathModifier + @"/app/Controller/Action"" method=""post"" onsubmit=""Sys.Mvc.AsyncForm.handleSubmit(this, new Sys.UI.DomEvent(event), { insertionMode: Sys.Mvc.InsertionMode.replace });"">";
        private const string AjaxFormWithTargetId = @"<form action=""" + AppPathModifier + @"/app/Controller/Action"" method=""post"" onsubmit=""Sys.Mvc.AsyncForm.handleSubmit(this, new Sys.UI.DomEvent(event), { insertionMode: Sys.Mvc.InsertionMode.replace, updateTargetId: 'some-id' });"">";
        private const string AjaxFormWithHtmlAttributes = @"<form action=""" + AppPathModifier + @"/app/Controller/Action"" method=""get"" onsubmit=""Sys.Mvc.AsyncForm.handleSubmit(this, new Sys.UI.DomEvent(event), { insertionMode: Sys.Mvc.InsertionMode.replace, updateTargetId: 'some-id' });"">";
        private const string AjaxFormClose = "</form>";

        internal const string AppPathModifier = HtmlHelperTest.AppPathModifier;

        private static readonly object _valuesObjectDictionary = new { id = 5 };
        private static readonly object _attributesObjectDictionary = new { method = "post" };

        [TestMethod]
        public void IsMvcAjaxRequestWithKeyIsTrue() {
            // Arrange
            Mock<HttpRequestBase> mockRequest = new Mock<HttpRequestBase>();
            mockRequest.Expect(r => r["__MVCASYNCPOST"]).Returns("true").Verifiable();
            HttpRequestBase request = mockRequest.Object;

            // Act
            bool retVal = AjaxExtensions.IsMvcAjaxRequest(request);

            // Assert
            Assert.IsTrue(retVal);
            mockRequest.Verify();
        }

        [TestMethod]
        public void IsMvcAjaxRequestWithoutKeyIsFalse() {
            // Arrange
            Mock<HttpRequestBase> mockRequest = new Mock<HttpRequestBase>();
            mockRequest.Expect(r => r["__MVCASYNCPOST"]).Returns((string)null).Verifiable();
            HttpRequestBase request = mockRequest.Object;

            // Act
            bool retVal = AjaxExtensions.IsMvcAjaxRequest(request);

            // Assert
            Assert.IsFalse(retVal);
            mockRequest.Verify();
        }

        [TestMethod]
        public void IsMvcAjaxRequestWithNullRequestThrows() {
            ExceptionHelper.ExpectArgumentNullException(
                delegate {
                    AjaxExtensions.IsMvcAjaxRequest(null);
                }, "request");
        }

        [TestMethod]
        public void ActionLinkWithNullActionThrows() {
            // Arrange
            AjaxHelper ajaxHelper = GetAjaxHelper();

            // Act & Assert
            ExceptionHelper.ExpectArgumentExceptionNullOrEmpty(
                delegate {
                    string actionLink = ajaxHelper.ActionLink("linkText", null/* actionName */, null, null, null, null);
                },
                "actionName");
        }

        [TestMethod]
        public void ActionLinkWithEmptyActionThrows() {
            // Arrange
            AjaxHelper ajaxHelper = GetAjaxHelper();

            // Act & Assert
            ExceptionHelper.ExpectArgumentExceptionNullOrEmpty(
                delegate {
                    string actionLink = ajaxHelper.ActionLink("linkText", String.Empty, null, null, null, null);
                },
                "actionName");
        }

        [TestMethod]
        public void ActionLinkWithNullOrEmptyLinkTextThrows() {
            // Arrange
            AjaxHelper ajaxHelper = GetAjaxHelper();

            // Act & Assert
            ExceptionHelper.ExpectArgumentExceptionNullOrEmpty(
                delegate {
                    string actionLink = ajaxHelper.ActionLink(String.Empty, String.Empty, null, null, null, null);
                },
                "linkText");
        }

        [TestMethod]
        public void ActionLinkWithNullOptionsThrows() {
            // Arrange
            AjaxHelper ajaxHelper = GetAjaxHelper();

            // Act & Assert
            ExceptionHelper.ExpectArgumentNullException(
                delegate {
                    string actionLink = ajaxHelper.ActionLink("linkText", "someAction", null, null, null/* ajaxOptions */, null);
                },
                "ajaxOptions");
        }

        [TestMethod]
        public void ActionLink() {
            // Arrange
            AjaxHelper ajaxHelper = GetAjaxHelper();

            // Act
            string actionLink = ajaxHelper.ActionLink("linkText", "Action", GetEmptyOptions());

            // Assert
            Assert.AreEqual(@"<a href=""" + AppPathModifier + @"/app/home/Action"" onclick=""Sys.Mvc.AsyncHyperlink.handleClick(this, new Sys.UI.DomEvent(event), { insertionMode: Sys.Mvc.InsertionMode.replace });"">linkText</a>", actionLink);
        }

        [TestMethod]
        public void ActionLinkAnonymousValues() {
            // Arrange
            AjaxHelper helper = GetAjaxHelper();
            object values = new { controller = "Controller" };
            AjaxOptions options = new AjaxOptions { UpdateTargetId = "update-div" };

            // Act
            string actionLink = helper.ActionLink("Some Text", "Action", values, options);

            // Assert
            Assert.AreEqual(@"<a href=""" + AppPathModifier + @"/app/Controller/Action"" onclick=""Sys.Mvc.AsyncHyperlink.handleClick(this, new Sys.UI.DomEvent(event), { insertionMode: Sys.Mvc.InsertionMode.replace, updateTargetId: 'update-div' });"">Some Text</a>", actionLink);
        }

        [TestMethod]
        public void ActionLinkAnonymousValuesAndAttributes() {
            // Arrange
            AjaxHelper helper = GetAjaxHelper();
            object htmlAttributes = new { foo = "bar", baz = "quux" };
            object values = new { controller = "Controller" };
            AjaxOptions options = new AjaxOptions { UpdateTargetId = "update-div" };

            // Act
            string actionLink = helper.ActionLink("Some Text", "Action", values, options, htmlAttributes);

            // Assert
            Assert.AreEqual(@"<a baz=""quux"" foo=""bar"" href=""" + AppPathModifier + @"/app/Controller/Action"" onclick=""Sys.Mvc.AsyncHyperlink.handleClick(this, new Sys.UI.DomEvent(event), { insertionMode: Sys.Mvc.InsertionMode.replace, updateTargetId: 'update-div' });"">Some Text</a>", actionLink);
        }

        [TestMethod]
        public void ActionLinkTypedValues() {
            // Arrange
            AjaxHelper helper = GetAjaxHelper();
            RouteValueDictionary values = new RouteValueDictionary {
                { "controller", "Controller" }
            };

            AjaxOptions options = new AjaxOptions { UpdateTargetId = "update-div" };

            // Act
            string actionLink = helper.ActionLink("Some Text", "Action", values, options);

            // Assert
            Assert.AreEqual(@"<a href=""" + AppPathModifier + @"/app/Controller/Action"" onclick=""Sys.Mvc.AsyncHyperlink.handleClick(this, new Sys.UI.DomEvent(event), { insertionMode: Sys.Mvc.InsertionMode.replace, updateTargetId: 'update-div' });"">Some Text</a>", actionLink);
        }

        [TestMethod]
        public void ActionLinkTypedValuesAndAttributes() {
            // Arrange
            AjaxHelper helper = GetAjaxHelper();
            RouteValueDictionary values = new RouteValueDictionary {
                { "controller", "Controller" }
            };
            Dictionary<string, object> htmlAttributes = new Dictionary<string, object> {
                { "foo", "bar" },
                { "baz", "quux" }
            };
            AjaxOptions options = new AjaxOptions { UpdateTargetId = "update-div" };

            // Act
            string actionLink = helper.ActionLink("Some Text", "Action", values, options, htmlAttributes);

            // Assert
            Assert.AreEqual(@"<a baz=""quux"" foo=""bar"" href=""" + AppPathModifier + @"/app/Controller/Action"" onclick=""Sys.Mvc.AsyncHyperlink.handleClick(this, new Sys.UI.DomEvent(event), { insertionMode: Sys.Mvc.InsertionMode.replace, updateTargetId: 'update-div' });"">Some Text</a>", actionLink);
        }

        [TestMethod]
        public void ActionLinkController() {
            // Arrange
            AjaxHelper ajaxHelper = GetAjaxHelper();

            // Act
            string actionLink = ajaxHelper.ActionLink("linkText", "Action", "Controller", GetEmptyOptions());

            // Assert
            Assert.AreEqual(@"<a href=""" + AppPathModifier + @"/app/Controller/Action"" onclick=""Sys.Mvc.AsyncHyperlink.handleClick(this, new Sys.UI.DomEvent(event), { insertionMode: Sys.Mvc.InsertionMode.replace });"">linkText</a>", actionLink);
        }

        [TestMethod]
        public void ActionLinkControllerAnonymousValues() {
            // Arrange
            AjaxHelper helper = GetAjaxHelper();
            object values = new { id = 5 };
            AjaxOptions options = new AjaxOptions { UpdateTargetId = "update-div" };

            // Act
            string actionLink = helper.ActionLink("Some Text", "Action", "Controller", values, options);

            // Assert
            Assert.AreEqual(@"<a href=""" + AppPathModifier + @"/app/Controller/Action/5"" onclick=""Sys.Mvc.AsyncHyperlink.handleClick(this, new Sys.UI.DomEvent(event), { insertionMode: Sys.Mvc.InsertionMode.replace, updateTargetId: 'update-div' });"">Some Text</a>", actionLink);
        }

        [TestMethod]
        public void ActionLinkControllerAnonymousValuesAndAttributes() {
            // Arrange
            AjaxHelper helper = GetAjaxHelper();
            object htmlAttributes = new { foo = "bar", baz = "quux" };
            object values = new { id = 5 };
            AjaxOptions options = new AjaxOptions { UpdateTargetId = "update-div" };

            // Act
            string actionLink = helper.ActionLink("Some Text", "Action", "Controller", values, options, htmlAttributes);

            // Assert
            Assert.AreEqual(@"<a baz=""quux"" foo=""bar"" href=""" + AppPathModifier + @"/app/Controller/Action/5"" onclick=""Sys.Mvc.AsyncHyperlink.handleClick(this, new Sys.UI.DomEvent(event), { insertionMode: Sys.Mvc.InsertionMode.replace, updateTargetId: 'update-div' });"">Some Text</a>", actionLink);
        }

        [TestMethod]
        public void ActionLinkControllerTypedValues() {
            // Arrange
            AjaxHelper helper = GetAjaxHelper();
            RouteValueDictionary values = new RouteValueDictionary {
                { "id", 5 }
            };
            AjaxOptions options = new AjaxOptions { UpdateTargetId = "update-div" };

            // Act
            string actionLink = helper.ActionLink("Some Text", "Action", "Controller", values, options);

            // Assert
            Assert.AreEqual(@"<a href=""" + AppPathModifier + @"/app/Controller/Action/5"" onclick=""Sys.Mvc.AsyncHyperlink.handleClick(this, new Sys.UI.DomEvent(event), { insertionMode: Sys.Mvc.InsertionMode.replace, updateTargetId: 'update-div' });"">Some Text</a>", actionLink);
        }

        [TestMethod]
        public void ActionLinkControllerTypedValuesAndAttributes() {
            // Arrange
            AjaxHelper helper = GetAjaxHelper();
            RouteValueDictionary values = new RouteValueDictionary {
                { "id",5}
            };
            Dictionary<string, object> htmlAttributes = new Dictionary<string, object> {
                { "foo", "bar" },
                { "baz", "quux" }
            };
            AjaxOptions options = new AjaxOptions { UpdateTargetId = "update-div" };

            // Act
            string actionLink = helper.ActionLink("Some Text", "Action", "Controller", values, options, htmlAttributes);

            // Assert
            Assert.AreEqual(@"<a baz=""quux"" foo=""bar"" href=""" + AppPathModifier + @"/app/Controller/Action/5"" onclick=""Sys.Mvc.AsyncHyperlink.handleClick(this, new Sys.UI.DomEvent(event), { insertionMode: Sys.Mvc.InsertionMode.replace, updateTargetId: 'update-div' });"">Some Text</a>", actionLink);
        }

        [TestMethod]
        public void ActionLinkWithOptions() {
            // Arrange
            AjaxHelper ajaxHelper = GetAjaxHelper();

            // Act
            string actionLink = ajaxHelper.ActionLink("linkText", "Action", "Controller", new AjaxOptions { UpdateTargetId = "some-id" });

            // Assert
            Assert.AreEqual(@"<a href=""" + AppPathModifier + @"/app/Controller/Action"" onclick=""Sys.Mvc.AsyncHyperlink.handleClick(this, new Sys.UI.DomEvent(event), { insertionMode: Sys.Mvc.InsertionMode.replace, updateTargetId: 'some-id' });"">linkText</a>", actionLink);
        }

        [TestMethod]
        public void RouteLinkWithNullOrEmptyLinkTextThrows() {
            // Arrange
            AjaxHelper ajaxHelper = GetAjaxHelper();

            // Act & Assert
            ExceptionHelper.ExpectArgumentExceptionNullOrEmpty(
                delegate {
                    string actionLink = ajaxHelper.RouteLink(String.Empty, String.Empty, null, null, null);
                },
                "linkText");
        }

        [TestMethod]
        public void RouteLinkWithNullOptionsThrows() {
            // Arrange
            AjaxHelper ajaxHelper = GetAjaxHelper();

            // Act & Assert
            ExceptionHelper.ExpectArgumentNullException(
                delegate {
                    string actionLink = ajaxHelper.RouteLink("linkText", "someRoute", null, null /* ajaxOptions */, null);
                },
                "ajaxOptions");
        }

        [TestMethod]
        public void RouteLinkAnonymousValues() {
            // Arrange
            AjaxHelper helper = GetAjaxHelper();
            object values = new {
                action = "Action",
                controller = "Controller"
            };
            AjaxOptions options = new AjaxOptions { UpdateTargetId = "update-div" };

            // Act
            string routeLink = helper.RouteLink("Some Text", values, options);

            // Assert
            Assert.AreEqual(@"<a href=""" + AppPathModifier + @"/app/Controller/Action"" onclick=""Sys.Mvc.AsyncHyperlink.handleClick(this, new Sys.UI.DomEvent(event), { insertionMode: Sys.Mvc.InsertionMode.replace, updateTargetId: 'update-div' });"">Some Text</a>", routeLink);
        }

        [TestMethod]
        public void RouteLinkAnonymousValuesAndTypedAttributes() {
            // Arrange
            AjaxHelper helper = GetAjaxHelper();
            Dictionary<string, object> htmlAttributes = new Dictionary<string, object>() {
                {"foo", "bar"},
                {"baz", "quux"}
            };
            object values = new {
                action = "Action",
                controller = "Controller"
            };
            AjaxOptions options = new AjaxOptions { UpdateTargetId = "update-div" };

            // Act
            string actionLink = helper.RouteLink("Some Text", values, options, htmlAttributes);

            // Assert
            Assert.AreEqual(@"<a baz=""quux"" foo=""bar"" href=""" + AppPathModifier + @"/app/Controller/Action"" onclick=""Sys.Mvc.AsyncHyperlink.handleClick(this, new Sys.UI.DomEvent(event), { insertionMode: Sys.Mvc.InsertionMode.replace, updateTargetId: 'update-div' });"">Some Text</a>", actionLink);
        }

        [TestMethod]
        public void RouteLinkAnonymousValuesAndAttributes() {
            // Arrange
            AjaxHelper helper = GetAjaxHelper();
            object htmlAttributes = new {
                foo = "bar",
                baz = "quux"
            };
            object values = new {
                action = "Action",
                controller = "Controller"
            };
            AjaxOptions options = new AjaxOptions { UpdateTargetId = "update-div" };

            // Act
            string actionLink = helper.RouteLink("Some Text", values, options, htmlAttributes);

            // Assert
            Assert.AreEqual(@"<a baz=""quux"" foo=""bar"" href=""" + AppPathModifier + @"/app/Controller/Action"" onclick=""Sys.Mvc.AsyncHyperlink.handleClick(this, new Sys.UI.DomEvent(event), { insertionMode: Sys.Mvc.InsertionMode.replace, updateTargetId: 'update-div' });"">Some Text</a>", actionLink);
        }

        [TestMethod]
        public void RouteLinkTypedValues() {
            // Arrange
            AjaxHelper helper = GetAjaxHelper();
            RouteValueDictionary values = new RouteValueDictionary {
                { "controller", "Controller" },
                { "action", "Action" }
            };

            AjaxOptions options = new AjaxOptions { UpdateTargetId = "update-div" };

            // Act
            string actionLink = helper.RouteLink("Some Text", values, options);

            // Assert
            Assert.AreEqual(@"<a href=""" + AppPathModifier + @"/app/Controller/Action"" onclick=""Sys.Mvc.AsyncHyperlink.handleClick(this, new Sys.UI.DomEvent(event), { insertionMode: Sys.Mvc.InsertionMode.replace, updateTargetId: 'update-div' });"">Some Text</a>", actionLink);
        }

        [TestMethod]
        public void RouteLinkTypedValuesAndAttributes() {
            // Arrange
            AjaxHelper helper = GetAjaxHelper();
            RouteValueDictionary values = new RouteValueDictionary {
                { "controller", "Controller" },
                { "action", "Action" }
            };
            Dictionary<string, object> htmlAttributes = new Dictionary<string, object> {
                { "foo", "bar" },
                { "baz", "quux" }
            };
            AjaxOptions options = new AjaxOptions { UpdateTargetId = "update-div" };

            // Act
            string actionLink = helper.RouteLink("Some Text", values, options, htmlAttributes);

            // Assert
            Assert.AreEqual(@"<a baz=""quux"" foo=""bar"" href=""" + AppPathModifier + @"/app/Controller/Action"" onclick=""Sys.Mvc.AsyncHyperlink.handleClick(this, new Sys.UI.DomEvent(event), { insertionMode: Sys.Mvc.InsertionMode.replace, updateTargetId: 'update-div' });"">Some Text</a>", actionLink);
        }

        [TestMethod]
        public void RouteLinkTypedValuesAndAnonymousAttributes() {
            // Arrange
            AjaxHelper helper = GetAjaxHelper();
            RouteValueDictionary values = new RouteValueDictionary {
                { "controller", "Controller" },
                { "action", "Action" }
            };
            object htmlAttributes = new {
                foo = "bar",
                baz = "quux"
            };
            AjaxOptions options = new AjaxOptions { UpdateTargetId = "update-div" };

            // Act
            string actionLink = helper.RouteLink("Some Text", values, options, htmlAttributes);

            // Assert
            Assert.AreEqual(@"<a baz=""quux"" foo=""bar"" href=""" + AppPathModifier + @"/app/Controller/Action"" onclick=""Sys.Mvc.AsyncHyperlink.handleClick(this, new Sys.UI.DomEvent(event), { insertionMode: Sys.Mvc.InsertionMode.replace, updateTargetId: 'update-div' });"">Some Text</a>", actionLink);
        }

        [TestMethod]
        public void RouteLinkNamedRoute() {
            // Arrange
            AjaxHelper ajaxHelper = GetAjaxHelper();

            // Act
            string actionLink = ajaxHelper.RouteLink("linkText", "namedroute", GetEmptyOptions());

            // Assert
            Assert.AreEqual(@"<a href=""" + AppPathModifier + @"/app/named/home/oldaction"" onclick=""Sys.Mvc.AsyncHyperlink.handleClick(this, new Sys.UI.DomEvent(event), { insertionMode: Sys.Mvc.InsertionMode.replace });"">linkText</a>", actionLink);
        }

        [TestMethod]
        public void RouteLinkNamedRouteAnonymousAttributes() {
            // Arrange
            AjaxHelper ajaxHelper = GetAjaxHelper();
            object htmlAttributes = new {
                foo = "bar",
                baz = "quux"
            };
            AjaxOptions options = new AjaxOptions { UpdateTargetId = "update-div" };

            // Act
            string actionLink = ajaxHelper.RouteLink("Some Text", "namedroute", options, htmlAttributes);

            // Assert
            Assert.AreEqual(@"<a baz=""quux"" foo=""bar"" href=""" + AppPathModifier + @"/app/named/home/oldaction"" onclick=""Sys.Mvc.AsyncHyperlink.handleClick(this, new Sys.UI.DomEvent(event), { insertionMode: Sys.Mvc.InsertionMode.replace, updateTargetId: 'update-div' });"">Some Text</a>", actionLink);
        }

        [TestMethod]
        public void RouteLinkNamedRouteTypedAttributes() {
            // Arrange
            AjaxHelper ajaxHelper = GetAjaxHelper();
            Dictionary<string, object> htmlAttributes = new Dictionary<string, object> { { "foo", "bar" }, { "baz", "quux" } };
            AjaxOptions options = new AjaxOptions { UpdateTargetId = "update-div" };

            // Act
            string actionLink = ajaxHelper.RouteLink("Some Text", "namedroute", options, htmlAttributes);

            // Assert
            Assert.AreEqual(@"<a baz=""quux"" foo=""bar"" href=""" + AppPathModifier + @"/app/named/home/oldaction"" onclick=""Sys.Mvc.AsyncHyperlink.handleClick(this, new Sys.UI.DomEvent(event), { insertionMode: Sys.Mvc.InsertionMode.replace, updateTargetId: 'update-div' });"">Some Text</a>", actionLink);
        }

        [TestMethod]
        public void RouteLinkNamedRouteWithAnonymousValues() {
            // Arrange
            AjaxHelper ajaxHelper = GetAjaxHelper();
            object values = new {
                action = "Action",
                controller = "Controller"
            };

            // Act
            string actionLink = ajaxHelper.RouteLink("linkText", "namedroute", values, GetEmptyOptions());

            // Assert
            Assert.AreEqual(@"<a href=""" + AppPathModifier + @"/app/named/Controller/Action"" onclick=""Sys.Mvc.AsyncHyperlink.handleClick(this, new Sys.UI.DomEvent(event), { insertionMode: Sys.Mvc.InsertionMode.replace });"">linkText</a>", actionLink);
        }

        [TestMethod]
        public void RouteLinkNamedRouteAnonymousValuesAndAttributes() {
            // Arrange
            AjaxHelper ajaxHelper = GetAjaxHelper();
            object values = new {
                action = "Action",
                controller = "Controller"
            };

            object htmlAttributes = new {
                foo = "bar",
                baz = "quux"
            };
            AjaxOptions options = new AjaxOptions { UpdateTargetId = "update-div" };

            // Act
            string actionLink = ajaxHelper.RouteLink("Some Text", "namedroute", values, options, htmlAttributes);

            // Assert
            Assert.AreEqual(@"<a baz=""quux"" foo=""bar"" href=""" + AppPathModifier + @"/app/named/Controller/Action"" onclick=""Sys.Mvc.AsyncHyperlink.handleClick(this, new Sys.UI.DomEvent(event), { insertionMode: Sys.Mvc.InsertionMode.replace, updateTargetId: 'update-div' });"">Some Text</a>", actionLink);
        }

        [TestMethod]
        public void RouteLinkNamedRouteAnonymousValuesAndTypedAttributes() {
            // Arrange
            AjaxHelper ajaxHelper = GetAjaxHelper();
            object values = new {
                action = "Action",
                controller = "Controller"
            };

            Dictionary<string, object> htmlAttributes = new Dictionary<string, object> { { "foo", "bar" }, { "baz", "quux" } };
            AjaxOptions options = new AjaxOptions { UpdateTargetId = "update-div" };

            // Act
            string actionLink = ajaxHelper.RouteLink("Some Text", "namedroute", values, options, htmlAttributes);

            // Assert
            Assert.AreEqual(@"<a baz=""quux"" foo=""bar"" href=""" + AppPathModifier + @"/app/named/Controller/Action"" onclick=""Sys.Mvc.AsyncHyperlink.handleClick(this, new Sys.UI.DomEvent(event), { insertionMode: Sys.Mvc.InsertionMode.replace, updateTargetId: 'update-div' });"">Some Text</a>", actionLink);
        }

        [TestMethod]
        public void RouteLinkNamedRouteWithTypedValues() {
            // Arrange
            AjaxHelper ajaxHelper = GetAjaxHelper();
            RouteValueDictionary values = new RouteValueDictionary {
                { "controller", "Controller" },
                { "action", "Action" }
            };

            // Act
            string actionLink = ajaxHelper.RouteLink("linkText", "namedroute", values, GetEmptyOptions());

            // Assert
            Assert.AreEqual(@"<a href=""" + AppPathModifier + @"/app/named/Controller/Action"" onclick=""Sys.Mvc.AsyncHyperlink.handleClick(this, new Sys.UI.DomEvent(event), { insertionMode: Sys.Mvc.InsertionMode.replace });"">linkText</a>", actionLink);
        }

        [TestMethod]
        public void RouteLinkNamedRouteTypedValuesAndAnonymousAttributes() {
            // Arrange
            AjaxHelper ajaxHelper = GetAjaxHelper();
            RouteValueDictionary values = new RouteValueDictionary {
                { "controller", "Controller" },
                { "action", "Action" }
            };

            object htmlAttributes = new {
                foo = "bar",
                baz = "quux"
            };
            AjaxOptions options = new AjaxOptions { UpdateTargetId = "update-div" };

            // Act
            string actionLink = ajaxHelper.RouteLink("Some Text", "namedroute", values, options, htmlAttributes);

            // Assert
            Assert.AreEqual(@"<a baz=""quux"" foo=""bar"" href=""" + AppPathModifier + @"/app/named/Controller/Action"" onclick=""Sys.Mvc.AsyncHyperlink.handleClick(this, new Sys.UI.DomEvent(event), { insertionMode: Sys.Mvc.InsertionMode.replace, updateTargetId: 'update-div' });"">Some Text</a>", actionLink);
        }

        [TestMethod]
        public void RouteLinkNamedRouteTypedValuesAndAttributes() {
            // Arrange
            AjaxHelper ajaxHelper = GetAjaxHelper();
            RouteValueDictionary values = new RouteValueDictionary {
                { "controller", "Controller" },
                { "action", "Action" }
            };

            Dictionary<string, object> htmlAttributes = new Dictionary<string, object> { { "foo", "bar" }, { "baz", "quux" } };
            AjaxOptions options = new AjaxOptions { UpdateTargetId = "update-div" };

            // Act
            string actionLink = ajaxHelper.RouteLink("Some Text", "namedroute", values, options, htmlAttributes);

            // Assert
            Assert.AreEqual(@"<a baz=""quux"" foo=""bar"" href=""" + AppPathModifier + @"/app/named/Controller/Action"" onclick=""Sys.Mvc.AsyncHyperlink.handleClick(this, new Sys.UI.DomEvent(event), { insertionMode: Sys.Mvc.InsertionMode.replace, updateTargetId: 'update-div' });"">Some Text</a>", actionLink);
        }

        [TestMethod]
        public void RouteLinkNamedRouteNullValuesAndAttributes() {
            // Arrange
            AjaxHelper ajaxHelper = GetAjaxHelper();
            Dictionary<string, object> htmlAttributes = new Dictionary<string, object> { { "foo", "bar" }, { "baz", "quux" } };
            AjaxOptions options = new AjaxOptions { UpdateTargetId = "update-div" };

            // Act
            string actionLink = ajaxHelper.RouteLink("Some Text", "namedroute", null, options, htmlAttributes);

            // Assert
            Assert.AreEqual(@"<a baz=""quux"" foo=""bar"" href=""" + AppPathModifier + @"/app/named/home/oldaction"" onclick=""Sys.Mvc.AsyncHyperlink.handleClick(this, new Sys.UI.DomEvent(event), { insertionMode: Sys.Mvc.InsertionMode.replace, updateTargetId: 'update-div' });"">Some Text</a>", actionLink);
        }

        [TestMethod]
        public void FormWithNullActionNameThrows() {
            // Arrange
            AjaxHelper ajaxHelper = GetAjaxHelper();

            // Act & Assert
            ExceptionHelper.ExpectArgumentExceptionNullOrEmpty(
                delegate {
                    IDisposable form = ajaxHelper.Form(null, new AjaxOptions());
                },
                "actionName");
        }

        [TestMethod]
        public void FormWithEmptyActionNameThrows() {
            // Arrange
            AjaxHelper ajaxHelper = GetAjaxHelper();

            // Act & Assert
            ExceptionHelper.ExpectArgumentExceptionNullOrEmpty(
                delegate {
                    IDisposable form = ajaxHelper.Form(String.Empty, new AjaxOptions());
                },
                "actionName");
        }

        [TestMethod]
        public void FormWithNullOptionsThrows() {
            // Arrange
            AjaxHelper ajaxHelper = GetAjaxHelper();

            // Act & Assert
            ExceptionHelper.ExpectArgumentNullException(
                delegate {
                    IDisposable form = ajaxHelper.Form("someAction", "someController", null);
                },
                "ajaxOptions");
        }

        [TestMethod]
        public void Form() {
            // Arrange
            Mock<HttpResponseBase> mockResponse = new Mock<HttpResponseBase>(MockBehavior.Strict);
            AjaxHelper ajaxHelper = GetAjaxHelper(mockResponse);
            AjaxOptions ajaxOptions = GetEmptyOptions();

            // Arrange expectations
            mockResponse.Expect(response => response.Write(AjaxFormWithDefaultController)).Verifiable();

            // Act
            IDisposable form = ajaxHelper.Form("Action", ajaxOptions);

            // Assert
            mockResponse.Verify();
        }

        [TestMethod]
        public void FormAnonymousValues() {
            // Arrange
            Mock<HttpResponseBase> mockResponse = new Mock<HttpResponseBase>(MockBehavior.Strict);
            AjaxHelper ajaxHelper = GetAjaxHelper(mockResponse);
            AjaxOptions ajaxOptions = GetEmptyOptions();
            object values = new { controller = "Controller" };

            // Arrange expectations
            mockResponse.Expect(response => response.Write(AjaxFormWithEmptyOptions)).Verifiable();

            // Act
            IDisposable form = ajaxHelper.Form("Action", values, ajaxOptions);

            // Assert
            mockResponse.Verify();
        }

        [TestMethod]
        public void FormAnonymousValuesAndAttributes() {
            // Arrange
            Mock<HttpResponseBase> mockResponse = new Mock<HttpResponseBase>(MockBehavior.Strict);
            AjaxHelper ajaxHelper = GetAjaxHelper(mockResponse);
            AjaxOptions ajaxOptions = new AjaxOptions { UpdateTargetId = "some-id" };
            object values = new { controller = "Controller" };
            object htmlAttributes = new { method = "get" };

            // Arrange expectations
            mockResponse.Expect(response => response.Write(AjaxFormWithHtmlAttributes)).Verifiable();

            // Act
            IDisposable form = ajaxHelper.Form("Action", values, ajaxOptions, htmlAttributes);

            // Assert
            mockResponse.Verify();
        }

        [TestMethod]
        public void FormTypedValues() {
            // Arrange
            Mock<HttpResponseBase> mockResponse = new Mock<HttpResponseBase>(MockBehavior.Strict);
            AjaxHelper ajaxHelper = GetAjaxHelper(mockResponse);
            AjaxOptions ajaxOptions = GetEmptyOptions();
            RouteValueDictionary values = new RouteValueDictionary {
                { "controller", "Controller" }
            };

            // Arrange expectations
            mockResponse.Expect(response => response.Write(AjaxFormWithEmptyOptions)).Verifiable();

            // Act
            IDisposable form = ajaxHelper.Form("Action", values, ajaxOptions);

            // Assert
            mockResponse.Verify();
        }

        [TestMethod]
        public void FormTypedValuesAndAttributes() {
            // Arrange
            Mock<HttpResponseBase> mockResponse = new Mock<HttpResponseBase>(MockBehavior.Strict);
            AjaxHelper ajaxHelper = GetAjaxHelper(mockResponse);
            AjaxOptions ajaxOptions = new AjaxOptions { UpdateTargetId = "some-id" };
            RouteValueDictionary values = new RouteValueDictionary {
                { "controller", "Controller" }
            };
            Dictionary<string, object> htmlAttributes = new Dictionary<string, object> {
                { "method", "get"}
            };

            // Arrange expectations
            mockResponse.Expect(response => response.Write(AjaxFormWithHtmlAttributes)).Verifiable();

            // Act
            IDisposable form = ajaxHelper.Form("Action", values, ajaxOptions, htmlAttributes);

            // Assert
            mockResponse.Verify();
        }

        [TestMethod]
        public void FormController() {
            // Arrange
            Mock<HttpResponseBase> mockResponse = new Mock<HttpResponseBase>(MockBehavior.Strict);
            AjaxHelper ajaxHelper = GetAjaxHelper(mockResponse);
            AjaxOptions ajaxOptions = GetEmptyOptions();

            // Arrange expectations
            mockResponse.Expect(response => response.Write(AjaxFormWithEmptyOptions)).Verifiable();

            // Act
            IDisposable form = ajaxHelper.Form("Action", "Controller", ajaxOptions);

            // Assert
            mockResponse.Verify();
        }

        [TestMethod]
        public void FormControllerAnonymousValues() {
            // Arrange
            Mock<HttpResponseBase> mockResponse = new Mock<HttpResponseBase>(MockBehavior.Strict);
            AjaxHelper ajaxHelper = GetAjaxHelper(mockResponse);
            AjaxOptions ajaxOptions = GetEmptyOptions();
            object values = new { id = 5 };

            // Arrange expectations
            mockResponse.Expect(response => response.Write(AjaxFormWithId)).Verifiable();

            // Act
            IDisposable form = ajaxHelper.Form("Action", "Controller", values, ajaxOptions);

            // Assert
            mockResponse.Verify();
        }

        [TestMethod]
        public void FormControllerAnonymousValuesAndAttributes() {
            // Arrange
            Mock<HttpResponseBase> mockResponse = new Mock<HttpResponseBase>(MockBehavior.Strict);
            AjaxHelper ajaxHelper = GetAjaxHelper(mockResponse);
            AjaxOptions ajaxOptions = GetEmptyOptions();
            object values = new { id = 5 };
            object htmlAttributes = new { method = "get" };

            // Arrange expectations
            mockResponse.Expect(response => response.Write(AjaxFormWithIdAndHtmlAttributes)).Verifiable();

            // Act
            IDisposable form = ajaxHelper.Form("Action", "Controller", values, ajaxOptions, htmlAttributes);

            // Assert
            mockResponse.Verify();
        }

        [TestMethod]
        public void FormControllerTypedValues() {
            // Arrange
            Mock<HttpResponseBase> mockResponse = new Mock<HttpResponseBase>(MockBehavior.Strict);
            AjaxHelper ajaxHelper = GetAjaxHelper(mockResponse);
            AjaxOptions ajaxOptions = GetEmptyOptions();
            RouteValueDictionary values = new RouteValueDictionary {
                { "id", 5 }
            };

            // Arrange expectations
            mockResponse.Expect(response => response.Write(AjaxFormWithId)).Verifiable();

            // Act
            IDisposable form = ajaxHelper.Form("Action", "Controller", values, ajaxOptions);

            // Assert
            mockResponse.Verify();
        }

        [TestMethod]
        public void FormControllerTypedValuesAndAttributes() {
            // Arrange
            Mock<HttpResponseBase> mockResponse = new Mock<HttpResponseBase>(MockBehavior.Strict);
            AjaxHelper ajaxHelper = GetAjaxHelper(mockResponse);
            AjaxOptions ajaxOptions = GetEmptyOptions();
            RouteValueDictionary values = new RouteValueDictionary {
                { "id", 5 }
            };
            Dictionary<string, object> htmlAttributes = new Dictionary<string, object> {
                { "method", "get"}
            };

            // Arrange expectations
            mockResponse.Expect(response => response.Write(AjaxFormWithIdAndHtmlAttributes)).Verifiable();

            // Act
            IDisposable form = ajaxHelper.Form("Action", "Controller", values, ajaxOptions, htmlAttributes);

            // Assert
            mockResponse.Verify();
        }

        [TestMethod]
        public void FormWithTargetId() {
            // Arrange
            Mock<HttpResponseBase> mockResponse = new Mock<HttpResponseBase>(MockBehavior.Strict);
            AjaxHelper ajaxHelper = GetAjaxHelper(mockResponse);
            AjaxOptions ajaxOptions = new AjaxOptions { UpdateTargetId = "some-id" };

            // Arrange expectations
            mockResponse.Expect(response => response.Write(AjaxFormWithTargetId)).Verifiable();

            // Act
            IDisposable form = ajaxHelper.Form("Action", "Controller", ajaxOptions);

            // Assert
            mockResponse.Verify();
        }

        [TestMethod]
        public void DisposeWritesClosingFormTag() {
            // Arrange
            Mock<HttpResponseBase> mockResponse = new Mock<HttpResponseBase>(MockBehavior.Strict);
            AjaxHelper ajaxHelper = GetAjaxHelper(mockResponse);
            AjaxOptions ajaxOptions = new AjaxOptions { UpdateTargetId = "some-id" };

            // Arrange expectations
            mockResponse.Expect(response => response.Write(AjaxFormWithTargetId)).Verifiable();
            mockResponse.Expect(response => response.Write(AjaxFormClose)).Verifiable();

            // Act
            IDisposable form = ajaxHelper.Form("Action", "Controller", ajaxOptions);
            form.Dispose();

            // Assert
            mockResponse.Verify();
        }

        [TestMethod]
        public void InsertionModeToString() {
            // Act & Assert
            Assert.AreEqual(AjaxExtensions.InsertionModeToString(InsertionMode.Replace), "Sys.Mvc.InsertionMode.replace");
            Assert.AreEqual(AjaxExtensions.InsertionModeToString(InsertionMode.InsertAfter), "Sys.Mvc.InsertionMode.insertAfter");
            Assert.AreEqual(AjaxExtensions.InsertionModeToString(InsertionMode.InsertBefore), "Sys.Mvc.InsertionMode.insertBefore");
            Assert.AreEqual(AjaxExtensions.InsertionModeToString((InsertionMode)4), "4");
        }

        private static AjaxHelper GetAjaxHelper() {
            return GetAjaxHelper(new Mock<HttpResponseBase>());
        }

        private static AjaxHelper GetAjaxHelper(Mock<HttpResponseBase> mockResponse) {
            HttpContextBase httpcontext = GetHttpContext("/app/", mockResponse);
            RouteCollection rt = new RouteCollection();
            rt.Add(new Route("{controller}/{action}/{id}", null) { Defaults = new RouteValueDictionary(new { id = "defaultid" }) });
            rt.Add("namedroute", new Route("named/{controller}/{action}/{id}", null) { Defaults = new RouteValueDictionary(new { id = "defaultid" }) });
            RouteData rd = new RouteData();
            rd.Values.Add("controller", "home");
            rd.Values.Add("action", "oldaction");
            ViewContext context = new ViewContext(httpcontext, rd, new Mock<ControllerBase>().Object, "view", new ViewDataDictionary(), new TempDataDictionary());
            AjaxHelper ajaxHelper = new AjaxHelper(context);
            ajaxHelper.RouteCollection = rt;
            return ajaxHelper;
        }

        private static AjaxOptions GetEmptyOptions() {
            return new AjaxOptions();
        }

        private static HttpContextBase GetHttpContext(string appPath, Mock<HttpResponseBase> mockResponse) {
            Mock<HttpContextBase> mockContext = new Mock<HttpContextBase>();
            Mock<HttpRequestBase> mockRequest = new Mock<HttpRequestBase>();
            if (!String.IsNullOrEmpty(appPath)) {
                mockRequest.Expect(o => o.ApplicationPath).Returns(appPath);
            }
            mockRequest.Expect(o => o.PathInfo).Returns(String.Empty);
            mockContext.Expect(o => o.Request).Returns(mockRequest.Object);
            mockContext.Expect(o => o.Session).Returns((HttpSessionStateBase)null);

            mockResponse.Expect(o => o.ApplyAppPathModifier(It.IsAny<string>())).Returns<string>(r => AppPathModifier + r);
            mockContext.Expect(o => o.Response).Returns(mockResponse.Object);

            return mockContext.Object;
        }
    }
}
