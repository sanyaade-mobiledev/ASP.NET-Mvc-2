namespace System.Web.Mvc.Test {
    using System;
    using System.Web.Routing;
    using System.Web.TestUtil;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;

    [TestClass]
    public class HtmlHelperTest {
        public const string AppPathModifier = "/$(SESSION)";
        public static readonly RouteValueDictionary AttributesDictionary = new RouteValueDictionary(new { baz = "BazValue" });
        public static readonly object AttributesObjectDictionary = new { baz = "BazObjValue" };

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
        public void ConstructorWithNullRouteCollectionThrows() {
            // Assert
            ExceptionHelper.ExpectArgumentNullException(
                delegate {
                    new HtmlHelper(GetViewContext(), GetViewDataContainer(null), null);
                },
                "routeCollection");
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
        public void AttributeEncodeObject() {
            // Arrange
            HtmlHelper htmlHelper = GetHtmlHelper();

            // Act
            string encodedHtml = htmlHelper.AttributeEncode((object)@"<"">");

            // Assert
            Assert.AreEqual(encodedHtml, "&lt;&quot;>", "Text is not being properly HTML attribute-encoded.");
        }

        [TestMethod]
        public void AttributeEncodeObjectNull() {
            // Arrange
            HtmlHelper htmlHelper = GetHtmlHelper();

            // Act
            string encodedHtml = htmlHelper.AttributeEncode((object)null);

            // Assert
            Assert.AreEqual("", encodedHtml);
        }

        [TestMethod]
        public void AttributeEncodeString() {
            // Arrange
            HtmlHelper htmlHelper = GetHtmlHelper();

            // Act
            string encodedHtml = htmlHelper.AttributeEncode(@"<"">");

            // Assert
            Assert.AreEqual(encodedHtml, "&lt;&quot;>", "Text is not being properly HTML attribute-encoded.");
        }

        [TestMethod]
        public void AttributeEncodeStringNull() {
            // Arrange
            HtmlHelper htmlHelper = GetHtmlHelper();

            // Act
            string encodedHtml = htmlHelper.AttributeEncode((string)null);

            // Assert
            Assert.AreEqual("", encodedHtml);
        }

        [TestMethod]
        public void EncodeObject() {
            // Arrange
            HtmlHelper htmlHelper = GetHtmlHelper();

            // Act
            string encodedHtml = htmlHelper.Encode((object)"<br />");

            // Assert
            Assert.AreEqual(encodedHtml, "&lt;br /&gt;", "Text is not being properly HTML-encoded.");
        }

        [TestMethod]
        public void EncodeObjectNull() {
            // Arrange
            HtmlHelper htmlHelper = GetHtmlHelper();

            // Act
            string encodedHtml = htmlHelper.Encode((object)null);

            // Assert
            Assert.AreEqual("", encodedHtml);
        }

        [TestMethod]
        public void EncodeString() {
            // Arrange
            HtmlHelper htmlHelper = GetHtmlHelper();

            // Act
            string encodedHtml = htmlHelper.Encode("<br />");

            // Assert
            Assert.AreEqual(encodedHtml, "&lt;br /&gt;", "Text is not being properly HTML-encoded.");
        }

        [TestMethod]
        public void EncodeStringNull() {
            // Arrange
            HtmlHelper htmlHelper = GetHtmlHelper();

            // Act
            string encodedHtml = htmlHelper.Encode((string)null);

            // Assert
            Assert.AreEqual("", encodedHtml);
        }

        public static HttpContextBase GetHttpContext(string appPath, string requestPath, string httpMethod, string protocol, int port) {
            Mock<HttpContextBase> mockContext = new Mock<HttpContextBase>();
            Mock<HttpRequestBase> mockRequest = new Mock<HttpRequestBase>();
            if (!String.IsNullOrEmpty(appPath)) {
                mockRequest.Expect(o => o.ApplicationPath).Returns(appPath);
            }
            if (!String.IsNullOrEmpty(requestPath)) {
                mockRequest.Expect(o => o.AppRelativeCurrentExecutionFilePath).Returns(requestPath);
            }

            Uri uri;

            if (port >= 0) {
                uri = new Uri(protocol+"://localhost"+":"+Convert.ToString(port));
            }
            else {
                uri = new Uri(protocol+"://localhost");
            }
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

        public static HttpContextBase GetHttpContext(string appPath, string requestPath, string httpMethod) {
            return GetHttpContext(appPath, requestPath, httpMethod, Uri.UriSchemeHttp.ToString(), -1);
        }

        internal static HtmlHelper GetHtmlHelper() {
            HttpContextBase httpcontext = GetHttpContext("/app/", null, null);
            RouteCollection rt = new RouteCollection();
            rt.Add(new Route("{controller}/{action}/{id}", null) { Defaults = new RouteValueDictionary(new { id = "defaultid" }) });
            rt.Add("namedroute", new Route("named/{controller}/{action}/{id}", null) { Defaults = new RouteValueDictionary(new { id = "defaultid" }) });
            RouteData rd = new RouteData();
            rd.Values.Add("controller", "home");
            rd.Values.Add("action", "oldaction");

            ViewDataDictionary vdd = new ViewDataDictionary();
            ViewContext context = new ViewContext(httpcontext, rd, new Mock<ControllerBase>().Object, new Mock<IView>().Object, vdd, new TempDataDictionary());
            Mock<IViewDataContainer> mockVdc = new Mock<IViewDataContainer>();
            mockVdc.Expect(vdc => vdc.ViewData).Returns(vdd);
            HtmlHelper htmlHelper = new HtmlHelper(context, mockVdc.Object, rt);
            return htmlHelper;
        }

        internal static HtmlHelper GetHtmlHelper(string protocol, int port) {
            HttpContextBase httpcontext = GetHttpContext("/app/", null, null, protocol, port);
            RouteCollection rt = new RouteCollection();
            rt.Add(new Route("{controller}/{action}/{id}", null) { Defaults = new RouteValueDictionary(new { id = "defaultid" }) });
            rt.Add("namedroute", new Route("named/{controller}/{action}/{id}", null) { Defaults = new RouteValueDictionary(new { id = "defaultid" }) });
            RouteData rd = new RouteData();
            rd.Values.Add("controller", "home");
            rd.Values.Add("action", "oldaction");

            ViewDataDictionary vdd = new ViewDataDictionary();
            ViewContext context = new ViewContext(httpcontext, rd, new Mock<ControllerBase>().Object, new Mock<IView>().Object, vdd, new TempDataDictionary());
            Mock<IViewDataContainer> mockVdc = new Mock<IViewDataContainer>();
            mockVdc.Expect(vdc => vdc.ViewData).Returns(vdd);
            HtmlHelper htmlHelper = new HtmlHelper(context, mockVdc.Object, rt);
            return htmlHelper;
        }

        internal static HtmlHelper GetHtmlHelper(ViewDataDictionary viewData) {
            ViewContext viewContext = GetViewContext();
            IViewDataContainer container = GetViewDataContainer(viewData);
            return new HtmlHelper(viewContext, container);
        }

        private static ViewContext GetViewContext() {
            ViewContext viewContext = new ViewContext(new Mock<HttpContextBase>().Object,
                new RouteData(),
                new Mock<ControllerBase>().Object,
                new Mock<IView>().Object,
                new ViewDataDictionary(),
                new TempDataDictionary());
            return viewContext;
        }

        private static IViewDataContainer GetViewDataContainer(ViewDataDictionary viewData) {
            Mock<IViewDataContainer> mockContainer = new Mock<IViewDataContainer>();
            mockContainer.Expect(c => c.ViewData).Returns(viewData);
            return mockContainer.Object;
        }
    }
}
