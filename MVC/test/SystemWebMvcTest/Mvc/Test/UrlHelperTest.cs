namespace System.Web.Mvc.Test {
    using System;
    using System.Web.Routing;
    using System.Web.TestUtil;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;

    [TestClass]
    public class UrlHelperTest {
        [TestMethod]
        public void RequestContextProperty() {
            // Arrange
            RequestContext requestContext = new RequestContext(new Mock<HttpContextBase>().Object, new RouteData());
            UrlHelper urlHelper = new UrlHelper(requestContext);

            // Assert
            Assert.AreEqual(requestContext, urlHelper.RequestContext);
        }

        [TestMethod]
        public void ConstructorWithNullRequestContextThrows() {
            // Assert
            ExceptionHelper.ExpectArgumentNullException(
                delegate {
                    new UrlHelper(null);
                },
                "requestContext");
        }

        [TestMethod]
        public void ConstructorWithNullRouteCollectionThrows() {
            // Assert
            ExceptionHelper.ExpectArgumentNullException(
                delegate {
                    new UrlHelper(GetRequestContext(), null);
                },
                "routeCollection");
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
        public void ActionWithNullActionName() {
            // Arrange
            UrlHelper urlHelper = GetUrlHelper();

            // Act
            string url = urlHelper.Action(null);

            // Assert
            Assert.AreEqual(HtmlHelperTest.AppPathModifier + "/app/home/oldaction", url);
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
        public void ActionParameterOverridesObjectProperties() {
            // Arrange
            UrlHelper urlHelper = GetUrlHelper();

            // Act
            string url = urlHelper.Action("newaction", new { Action = "action" });

            // Assert
            Assert.AreEqual<string>(HtmlHelperTest.AppPathModifier + "/app/home/newaction", url);
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
        public void ContentWithAppRelativePathAndQueryStringResolvesClientUrl() {
            // Arrange
            UrlHelper urlHelper = GetUrlHelper();

            // Act
            string returnedPath = urlHelper.Content("~/somepath?foo=bar");

            // Assert
            Assert.AreEqual("somepath?foo=bar", returnedPath);
        }

        [TestMethod]
        public void ContentWithAppRelativePathResolvesClientUrl() {
            // Arrange
            UrlHelper urlHelper = GetUrlHelper();

            // Act
            string returnedPath = urlHelper.Content("~/somepath");

            // Assert
            Assert.AreEqual("somepath", returnedPath);
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
        public void ContentWithNonAppRelativePathReturnsPathUnmodified() {
            // Arrange
            UrlHelper urlHelper = GetUrlHelper();

            // Act
            string returnedPath = urlHelper.Content("../somepath");

            // Assert
            Assert.AreEqual("../somepath", returnedPath);
        }

        [TestMethod]
        public void ContentWithNullPathThrows() {
            // Arrange
            UrlHelper urlHelper = GetUrlHelper();

            // Act & Assert
            ExceptionHelper.ExpectArgumentExceptionNullOrEmpty(
                delegate() {
                    urlHelper.Content(null);
                },
                "contentPath");
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
        public void MakeRelativeUrlCorrectlyResolvesLinkRelativeUrlToCurrentDirectory() {
            // DevDiv 209502
            // The VirtualPathUtility.MakeRelative() method will sometimes return an empty string instead of a dot
            // when it means to create a relative path to the current directory. We need to watch for this and
            // replace it.

            // Arrange
            string fromPath = "/Home";
            string toPath = "/";

            // Act
            string relativePath = UrlHelper.MakeRelativeUrl(fromPath, toPath);

            // Assert
            Assert.AreEqual("./", relativePath);
        }

        [TestMethod]
        public void MakeRelativeUrlCorrectlyResolvesLinkRelativeUrlToCurrentDirectoryWithQueryString() {
            // DevDiv 209502
            // The VirtualPathUtility.MakeRelative() method will sometimes return an empty string instead of a dot
            // when it means to create a relative path to the current directory. We need to watch for this and
            // replace it.

            // Arrange
            string fromPath = "/Home";
            string toPath = "/?foo=bar";

            // Act
            string relativePath = UrlHelper.MakeRelativeUrl(fromPath, toPath);

            // Assert
            Assert.AreEqual("./?foo=bar", relativePath);
        }

        [TestMethod]
        public void RouteUrlReturnsOriginalUrlIfDestinationNotInAppPath() {
            // Arrange
            UrlHelper urlHelper = GetUrlHelper();

            // Act
            string url = urlHelper.RouteUrl("namedroute", new { Action = "newaction", Controller = "home2", id = "someid" });

            // Assert
            Assert.AreEqual(HtmlHelperTest.AppPathModifier + "/app/named/home2/newaction/someid", url);
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
            Assert.AreEqual(0, valuesDictionary.Count);
            Assert.IsFalse(valuesDictionary.ContainsKey("action"));
        }

        private static RequestContext GetRequestContext() {
            HttpContextBase httpcontext = HtmlHelperTest.GetHttpContext("/app/", null, null);
            RouteData rd = new RouteData();
            return new RequestContext(httpcontext, rd);
        }

        private static UrlHelper GetUrlHelper() {
            HttpContextBase httpcontext = HtmlHelperTest.GetHttpContext("/app/", "~/", null);
            RouteCollection rt = new RouteCollection();
            rt.Add(new Route("{controller}/{action}/{id}", null) { Defaults = new RouteValueDictionary(new { id = "defaultid" }) });
            rt.Add("namedroute", new Route("named/{controller}/{action}/{id}", null) { Defaults = new RouteValueDictionary(new { id = "defaultid" }) });
            RouteData rd = new RouteData();
            rd.Values.Add("controller", "home");
            rd.Values.Add("action", "oldaction");

            UrlHelper urlHelper = new UrlHelper(new RequestContext(httpcontext, rd), rt);
            return urlHelper;
        }

    }
}