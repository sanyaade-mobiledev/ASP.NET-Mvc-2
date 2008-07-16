namespace System.Web.Mvc.Test {
    using System.Collections.Generic;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Routing;
    using System.Web.TestUtil;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;

    [TestClass]
    public class AjaxHelperTest {
        private const string AjaxFormWithDefaultController = @"<form action=""" + AppPathModifier + @"/app/home/Action"" onsubmit=""Sys.Mvc.AsyncForm.handleSubmit(this, { insertionMode: 0, updateTargetId: '' }); return false;"">";
        private const string AjaxFormWithId = @"<form action=""" + AppPathModifier + @"/app/Controller/Action/5"" onsubmit=""Sys.Mvc.AsyncForm.handleSubmit(this, { insertionMode: 0, updateTargetId: '' }); return false;"">";
        private const string AjaxFormWithIdAndHtmlAttributes = @"<form action=""" + AppPathModifier + @"/app/Controller/Action/5"" onsubmit=""Sys.Mvc.AsyncForm.handleSubmit(this, { insertionMode: 0, updateTargetId: '' }); return false;"" method=""post"">";
        private const string AjaxFormWithEmptyOptions = @"<form action=""" + AppPathModifier + @"/app/Controller/Action"" onsubmit=""Sys.Mvc.AsyncForm.handleSubmit(this, { insertionMode: 0, updateTargetId: '' }); return false;"">";
        private const string AjaxFormWithTargetId = @"<form action=""" + AppPathModifier + @"/app/Controller/Action"" onsubmit=""Sys.Mvc.AsyncForm.handleSubmit(this, { insertionMode: 0, updateTargetId: 'some-id' }); return false;"">";
        private const string AjaxFormWithHtmlAttributes = @"<form action=""" + AppPathModifier + @"/app/Controller/Action"" onsubmit=""Sys.Mvc.AsyncForm.handleSubmit(this, { insertionMode: 0, updateTargetId: 'some-id' }); return false;"" method=""post"">";
        private const string AjaxFormClose = "</form>";

        internal const string AppPathModifier = HtmlHelperTest.AppPathModifier;

        private static readonly object _valuesObjectDictionary = new { id = 5 };
        private static readonly object _attributesObjectDictionary = new { method = "post" };

        [TestMethod]
        public void ConstructorWithNullViewContextThrows() {
            // Setup, execute and verify
            ExceptionHelper.ExpectArgumentNullException(
                delegate {
                    AjaxHelper ajaxHelper = new AjaxHelper(null);
                },
                "viewContext");
        }

        [TestMethod]
        public void ActionLinkWithNullActionThrows() {
            // Setup
            AjaxHelper ajaxHelper = GetAjaxHelper();

            // Execute & Verify
            ExceptionHelper.ExpectArgumentExceptionNullOrEmpty(
                delegate {
                    string actionLink = ajaxHelper.ActionLink("linkText", null/* actionName */, null, null, null, null);
                },
                "actionName");
        }

        [TestMethod]
        public void ActionLinkWithEmptyActionThrows() {
            // Setup
            AjaxHelper ajaxHelper = GetAjaxHelper();

            // Execute & Verify
            ExceptionHelper.ExpectArgumentExceptionNullOrEmpty(
                delegate {
                    string actionLink = ajaxHelper.ActionLink("linkText", String.Empty, null, null, null, null);
                },
                "actionName");
        }

        [TestMethod]
        public void ActionLinkWithNullOrEmptyLinkTextThrows() {
            // Setup
            AjaxHelper ajaxHelper = GetAjaxHelper();

            // Execute & Verify
            ExceptionHelper.ExpectArgumentExceptionNullOrEmpty(
                delegate {
                    string actionLink = ajaxHelper.ActionLink(String.Empty, String.Empty, null, null, null, null);
                },
                "linkText");
        }

        [TestMethod]
        public void ActionLinkWithNullOptionsThrows() {
            // Setup
            AjaxHelper ajaxHelper = GetAjaxHelper();

            // Execute & Verify
            ExceptionHelper.ExpectArgumentNullException(
                delegate {
                    string actionLink = ajaxHelper.ActionLink("linkText", "someAction", null, null, null/* ajaxOptions */, null);
                },
                "ajaxOptions");
        }

        [TestMethod]
        public void ActionLink() {
            // Setup
            AjaxHelper ajaxHelper = GetAjaxHelper();

            // Execute
            string actionLink = ajaxHelper.ActionLink("linkText", "Action", GetEmptyOptions());

            // Verify
            Assert.AreEqual(@"<a href=""" + AppPathModifier + @"/app/home/Action"" onclick=""Sys.Mvc.AsyncHyperlink.handleClick(this, { insertionMode: 0, updateTargetId: '' }); return false;"">linkText</a>", actionLink);
        }

        [TestMethod]
        public void ActionLinkAnonymousValues() {
            // Setup
            AjaxHelper helper = GetAjaxHelper();
            object values = new { controller = "Controller" };
            AjaxOptions options = new AjaxOptions { UpdateTargetId = "update-div" };

            // Execute
            string actionLink = helper.ActionLink("Some Text", "Action", values, options);

            // Verify
            Assert.AreEqual(@"<a href=""" + AppPathModifier + @"/app/Controller/Action"" onclick=""Sys.Mvc.AsyncHyperlink.handleClick(this, { insertionMode: 0, updateTargetId: 'update-div' }); return false;"">Some Text</a>", actionLink);
        }

        [TestMethod]
        public void ActionLinkAnonymousValuesAndAttributes() {
            // Setup
            AjaxHelper helper = GetAjaxHelper();
            object htmlAttributes = new { foo = "bar", baz = "quux" };
            object values = new { controller = "Controller" };
            AjaxOptions options = new AjaxOptions { UpdateTargetId = "update-div" };

            // Execute
            string actionLink = helper.ActionLink("Some Text", "Action", values, options, htmlAttributes);

            // Verify
            Assert.AreEqual(@"<a foo=""bar"" baz=""quux"" href=""" + AppPathModifier + @"/app/Controller/Action"" onclick=""Sys.Mvc.AsyncHyperlink.handleClick(this, { insertionMode: 0, updateTargetId: 'update-div' }); return false;"">Some Text</a>", actionLink);
        }

        [TestMethod]
        public void ActionLinkTypedValues() {
            // Setup
            AjaxHelper helper = GetAjaxHelper();
            RouteValueDictionary values = new RouteValueDictionary {
                { "controller", "Controller" }
            };

            AjaxOptions options = new AjaxOptions { UpdateTargetId = "update-div" };

            // Execute
            string actionLink = helper.ActionLink("Some Text", "Action", values, options);

            // Verify
            Assert.AreEqual(@"<a href=""" + AppPathModifier + @"/app/Controller/Action"" onclick=""Sys.Mvc.AsyncHyperlink.handleClick(this, { insertionMode: 0, updateTargetId: 'update-div' }); return false;"">Some Text</a>", actionLink);
        }

        [TestMethod]
        public void ActionLinkTypedValuesAndAttributes() {
            // Setup
            AjaxHelper helper = GetAjaxHelper();
            RouteValueDictionary values = new RouteValueDictionary {
                { "controller", "Controller" }
            };
            Dictionary<string, object> htmlAttributes = new Dictionary<string, object> {
                { "foo", "bar" },
                { "baz", "quux" }
            };
            AjaxOptions options = new AjaxOptions { UpdateTargetId = "update-div" };

            // Execute
            string actionLink = helper.ActionLink("Some Text", "Action", values, options, htmlAttributes);

            // Verify
            Assert.AreEqual(@"<a foo=""bar"" baz=""quux"" href=""" + AppPathModifier + @"/app/Controller/Action"" onclick=""Sys.Mvc.AsyncHyperlink.handleClick(this, { insertionMode: 0, updateTargetId: 'update-div' }); return false;"">Some Text</a>", actionLink);
        }

        [TestMethod]
        public void ActionLinkController() {
            // Setup
            AjaxHelper ajaxHelper = GetAjaxHelper();

            // Execute
            string actionLink = ajaxHelper.ActionLink("linkText", "Action", "Controller", GetEmptyOptions());

            // Verify
            Assert.AreEqual(@"<a href=""" + AppPathModifier + @"/app/Controller/Action"" onclick=""Sys.Mvc.AsyncHyperlink.handleClick(this, { insertionMode: 0, updateTargetId: '' }); return false;"">linkText</a>", actionLink);
        }

        [TestMethod]
        public void ActionLinkControllerAnonymousValues() {
            // Setup
            AjaxHelper helper = GetAjaxHelper();
            object values = new { id = 5 };
            AjaxOptions options = new AjaxOptions { UpdateTargetId = "update-div" };

            // Execute
            string actionLink = helper.ActionLink("Some Text", "Action", "Controller", values, options);

            // Verify
            Assert.AreEqual(@"<a href=""" + AppPathModifier + @"/app/Controller/Action/5"" onclick=""Sys.Mvc.AsyncHyperlink.handleClick(this, { insertionMode: 0, updateTargetId: 'update-div' }); return false;"">Some Text</a>", actionLink);
        }

        [TestMethod]
        public void ActionLinkControllerAnonymousValuesAndAttributes() {
            // Setup
            AjaxHelper helper = GetAjaxHelper();
            object htmlAttributes = new { foo = "bar", baz = "quux" };
            object values = new { id = 5 };
            AjaxOptions options = new AjaxOptions { UpdateTargetId = "update-div" };

            // Execute
            string actionLink = helper.ActionLink("Some Text", "Action", "Controller", values, options, htmlAttributes);

            // Verify
            Assert.AreEqual(@"<a foo=""bar"" baz=""quux"" href=""" + AppPathModifier + @"/app/Controller/Action/5"" onclick=""Sys.Mvc.AsyncHyperlink.handleClick(this, { insertionMode: 0, updateTargetId: 'update-div' }); return false;"">Some Text</a>", actionLink);
        }

        [TestMethod]
        public void ActionLinkControllerTypedValues() {
            // Setup
            AjaxHelper helper = GetAjaxHelper();
            RouteValueDictionary values = new RouteValueDictionary {
                { "id", 5 }
            };
            AjaxOptions options = new AjaxOptions { UpdateTargetId = "update-div" };

            // Execute
            string actionLink = helper.ActionLink("Some Text", "Action", "Controller", values, options);

            // Verify
            Assert.AreEqual(@"<a href=""" + AppPathModifier + @"/app/Controller/Action/5"" onclick=""Sys.Mvc.AsyncHyperlink.handleClick(this, { insertionMode: 0, updateTargetId: 'update-div' }); return false;"">Some Text</a>", actionLink);
        }

        [TestMethod]
        public void ActionLinkControllerTypedValuesAndAttributes() {
            // Setup
            AjaxHelper helper = GetAjaxHelper();
            RouteValueDictionary values = new RouteValueDictionary {
                { "id",5}
            };
            Dictionary<string, object> htmlAttributes = new Dictionary<string, object> {
                { "foo", "bar" },
                { "baz", "quux" }
            };
            AjaxOptions options = new AjaxOptions { UpdateTargetId = "update-div" };

            // Execute
            string actionLink = helper.ActionLink("Some Text", "Action", "Controller", values, options, htmlAttributes);

            // Verify
            Assert.AreEqual(@"<a foo=""bar"" baz=""quux"" href=""" + AppPathModifier + @"/app/Controller/Action/5"" onclick=""Sys.Mvc.AsyncHyperlink.handleClick(this, { insertionMode: 0, updateTargetId: 'update-div' }); return false;"">Some Text</a>", actionLink);
        }

        [TestMethod]
        public void ActionLinkWithOptions() {
            // Setup
            AjaxHelper ajaxHelper = GetAjaxHelper();

            // Execute
            string actionLink = ajaxHelper.ActionLink("linkText", "Action", "Controller", new AjaxOptions { UpdateTargetId = "some-id" });

            // Verify
            Assert.AreEqual(@"<a href=""" + AppPathModifier + @"/app/Controller/Action"" onclick=""Sys.Mvc.AsyncHyperlink.handleClick(this, { insertionMode: 0, updateTargetId: 'some-id' }); return false;"">linkText</a>", actionLink);
        }

        [TestMethod]
        public void FormWithNullActionNameThrows() {
            // Setup
            AjaxHelper ajaxHelper = GetAjaxHelper();

            // Execute & Verify
            ExceptionHelper.ExpectArgumentExceptionNullOrEmpty(
                delegate {
                    IDisposable form = ajaxHelper.Form(null, new AjaxOptions());
                },
                "actionName");
        }

        [TestMethod]
        public void FormWithEmptyActionNameThrows() {
            // Setup
            AjaxHelper ajaxHelper = GetAjaxHelper();

            // Execute & Verify
            ExceptionHelper.ExpectArgumentExceptionNullOrEmpty(
                delegate {
                    IDisposable form = ajaxHelper.Form(String.Empty, new AjaxOptions());
                },
                "actionName");
        }

        [TestMethod]
        public void FormWithNullOptionsThrows() {
            // Setup
            AjaxHelper ajaxHelper = GetAjaxHelper();

            // Execute & Verify
            ExceptionHelper.ExpectArgumentNullException(
                delegate {
                    IDisposable form = ajaxHelper.Form("someAction", "someController", null);
                },
                "ajaxOptions");
        }

        [TestMethod]
        public void Form() {
            // Setup
            Mock<HttpResponseBase> mockResponse = new Mock<HttpResponseBase>(MockBehavior.Strict);
            AjaxHelper ajaxHelper = GetAjaxHelper(mockResponse);
            AjaxOptions ajaxOptions = GetEmptyOptions();

            // Setup expectations
            mockResponse.Expect(response => response.Write(AjaxFormWithDefaultController)).Verifiable();

            // Execute
            IDisposable form = ajaxHelper.Form("Action", ajaxOptions);

            // Verify
            mockResponse.Verify();
        }

        [TestMethod]
        public void FormAnonymousValues() {
            // Setup
            Mock<HttpResponseBase> mockResponse = new Mock<HttpResponseBase>(MockBehavior.Strict);
            AjaxHelper ajaxHelper = GetAjaxHelper(mockResponse);
            AjaxOptions ajaxOptions = GetEmptyOptions();
            object values = new { controller = "Controller" };

            // Setup expectations
            mockResponse.Expect(response => response.Write(AjaxFormWithEmptyOptions)).Verifiable();

            // Execute
            IDisposable form = ajaxHelper.Form("Action", values, ajaxOptions);

            // Verify
            mockResponse.Verify();
        }

        [TestMethod]
        public void FormAnonymousValuesAndAttributes() {
            // Setup
            Mock<HttpResponseBase> mockResponse = new Mock<HttpResponseBase>(MockBehavior.Strict);
            AjaxHelper ajaxHelper = GetAjaxHelper(mockResponse);
            AjaxOptions ajaxOptions = new AjaxOptions { UpdateTargetId = "some-id" };
            object values = new { controller = "Controller" };
            object htmlAttributes = new { method = "post" };

            // Setup expectations
            mockResponse.Expect(response => response.Write(AjaxFormWithHtmlAttributes)).Verifiable();

            // Execute
            IDisposable form = ajaxHelper.Form("Action", values, ajaxOptions, htmlAttributes);

            // Verify
            mockResponse.Verify();
        }

        [TestMethod]
        public void FormTypedValues() {
            // Setup
            Mock<HttpResponseBase> mockResponse = new Mock<HttpResponseBase>(MockBehavior.Strict);
            AjaxHelper ajaxHelper = GetAjaxHelper(mockResponse);
            AjaxOptions ajaxOptions = GetEmptyOptions();
            RouteValueDictionary values = new RouteValueDictionary {
                { "controller", "Controller" }
            };

            // Setup expectations
            mockResponse.Expect(response => response.Write(AjaxFormWithEmptyOptions)).Verifiable();

            // Execute
            IDisposable form = ajaxHelper.Form("Action", values, ajaxOptions);

            // Verify
            mockResponse.Verify();
        }

        [TestMethod]
        public void FormTypedValuesAndAttributes() {
            // Setup
            Mock<HttpResponseBase> mockResponse = new Mock<HttpResponseBase>(MockBehavior.Strict);
            AjaxHelper ajaxHelper = GetAjaxHelper(mockResponse);
            AjaxOptions ajaxOptions = new AjaxOptions { UpdateTargetId = "some-id" };
            RouteValueDictionary values = new RouteValueDictionary {
                { "controller", "Controller" }
            };
            Dictionary<string, object> htmlAttributes = new Dictionary<string, object> {
                { "method", "post"}
            };

            // Setup expectations
            mockResponse.Expect(response => response.Write(AjaxFormWithHtmlAttributes)).Verifiable();

            // Execute
            IDisposable form = ajaxHelper.Form("Action", values, ajaxOptions, htmlAttributes);

            // Verify
            mockResponse.Verify();
        }

        [TestMethod]
        public void FormController() {
            // Setup
            Mock<HttpResponseBase> mockResponse = new Mock<HttpResponseBase>(MockBehavior.Strict);
            AjaxHelper ajaxHelper = GetAjaxHelper(mockResponse);
            AjaxOptions ajaxOptions = GetEmptyOptions();

            // Setup expectations
            mockResponse.Expect(response => response.Write(AjaxFormWithEmptyOptions)).Verifiable();

            // Execute
            IDisposable form = ajaxHelper.Form("Action", "Controller", ajaxOptions);

            // Verify
            mockResponse.Verify();
        }

        [TestMethod]
        public void FormControllerAnonymousValues() {
            // Setup
            Mock<HttpResponseBase> mockResponse = new Mock<HttpResponseBase>(MockBehavior.Strict);
            AjaxHelper ajaxHelper = GetAjaxHelper(mockResponse);
            AjaxOptions ajaxOptions = GetEmptyOptions();
            object values = new { id = 5 };

            // Setup expectations
            mockResponse.Expect(response => response.Write(AjaxFormWithId)).Verifiable();

            // Execute
            IDisposable form = ajaxHelper.Form("Action", "Controller", values, ajaxOptions);

            // Verify
            mockResponse.Verify();
        }

        [TestMethod]
        public void FormControllerAnonymousValuesAndAttributes() {
            // Setup
            Mock<HttpResponseBase> mockResponse = new Mock<HttpResponseBase>(MockBehavior.Strict);
            AjaxHelper ajaxHelper = GetAjaxHelper(mockResponse);
            AjaxOptions ajaxOptions = GetEmptyOptions();
            object values = new { id = 5 };
            object htmlAttributes = new { method = "post" };

            // Setup expectations
            mockResponse.Expect(response => response.Write(AjaxFormWithIdAndHtmlAttributes)).Verifiable();

            // Execute
            IDisposable form = ajaxHelper.Form("Action", "Controller", values, ajaxOptions, htmlAttributes);

            // Verify
            mockResponse.Verify();
        }

        [TestMethod]
        public void FormControllerTypedValues() {
            // Setup
            Mock<HttpResponseBase> mockResponse = new Mock<HttpResponseBase>(MockBehavior.Strict);
            AjaxHelper ajaxHelper = GetAjaxHelper(mockResponse);
            AjaxOptions ajaxOptions = GetEmptyOptions();
            RouteValueDictionary values = new RouteValueDictionary {
                { "id", 5 }
            };

            // Setup expectations
            mockResponse.Expect(response => response.Write(AjaxFormWithId)).Verifiable();

            // Execute
            IDisposable form = ajaxHelper.Form("Action", "Controller", values, ajaxOptions);

            // Verify
            mockResponse.Verify();
        }

        [TestMethod]
        public void FormControllerTypedValuesAndAttributes() {
            // Setup
            Mock<HttpResponseBase> mockResponse = new Mock<HttpResponseBase>(MockBehavior.Strict);
            AjaxHelper ajaxHelper = GetAjaxHelper(mockResponse);
            AjaxOptions ajaxOptions = GetEmptyOptions();
            RouteValueDictionary values = new RouteValueDictionary {
                { "id", 5 }
            };
            Dictionary<string, object> htmlAttributes = new Dictionary<string, object> {
                { "method", "post"}
            };

            // Setup expectations
            mockResponse.Expect(response => response.Write(AjaxFormWithIdAndHtmlAttributes)).Verifiable();

            // Execute
            IDisposable form = ajaxHelper.Form("Action", "Controller", values, ajaxOptions, htmlAttributes);

            // Verify
            mockResponse.Verify();
        }

        [TestMethod]
        public void FormWithTargetId() {
            // Setup
            Mock<HttpResponseBase> mockResponse = new Mock<HttpResponseBase>(MockBehavior.Strict);
            AjaxHelper ajaxHelper = GetAjaxHelper(mockResponse);
            AjaxOptions ajaxOptions = new AjaxOptions { UpdateTargetId = "some-id" };

            // Setup expectations
            mockResponse.Expect(response => response.Write(AjaxFormWithTargetId)).Verifiable();

            // Execute
            IDisposable form = ajaxHelper.Form("Action", "Controller", ajaxOptions);

            // Verify
            mockResponse.Verify();
        }

        [TestMethod]
        public void DisposeWritesClosingFormTag() {
            // Setup
            Mock<HttpResponseBase> mockResponse = new Mock<HttpResponseBase>(MockBehavior.Strict);
            AjaxHelper ajaxHelper = GetAjaxHelper(mockResponse);
            AjaxOptions ajaxOptions = new AjaxOptions { UpdateTargetId = "some-id" };

            // Setup expectations
            mockResponse.Expect(response => response.Write(AjaxFormWithTargetId)).Verifiable();
            mockResponse.Expect(response => response.Write(AjaxFormClose)).Verifiable();

            // Execute
            IDisposable form = ajaxHelper.Form("Action", "Controller", ajaxOptions);
            form.Dispose();

            // Verify
            mockResponse.Verify();
        }

        [TestMethod]
        public void InsertionModeToString() {
            // Setup, Execute & Verify
            Assert.AreEqual(AjaxHelper.InsertionModeToString(InsertionMode.Replace), "Sys.Mvc.InsertionMode.Replace");
            Assert.AreEqual(AjaxHelper.InsertionModeToString(InsertionMode.InsertAfter), "Sys.Mvc.InsertionMode.InsertAfter");
            Assert.AreEqual(AjaxHelper.InsertionModeToString(InsertionMode.InsertBefore), "Sys.Mvc.InsertionMode.InsertBefore");
            Assert.AreEqual(AjaxHelper.InsertionModeToString((InsertionMode)4), "4");
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
            ViewContext context = new ViewContext(httpcontext, rd, new Mock<IController>().Object, "view", null, new ViewDataDictionary(), new TempDataDictionary());
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
