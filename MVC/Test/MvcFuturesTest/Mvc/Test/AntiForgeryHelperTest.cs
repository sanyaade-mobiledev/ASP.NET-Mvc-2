namespace MvcFuturesTest.Mvc.Test {
    using System;
    using System.Globalization;
    using System.Text.RegularExpressions;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Routing;
    using System.Web.TestUtil;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Microsoft.Web.Mvc;
    using Moq;

    [TestClass]
    public class AntiForgeryHelperTest {

        private static AntiForgeryHelper _helper = new SubclassedAntiForgeryHelper(new SubclassedAntiForgeryTokenSerializer());
        private const string _serializedValuePrefix = @"<input name=""__MVC_AntiForgeryToken"" type=""hidden"" value=""Creation: ";
        private const string _someValueSuffix = @", Value: some value, Salt: some other salt"" />";
        private readonly Regex _randomFormValueSuffixRegex = new Regex(@", Value: (?<value>[A-Za-z0-9/\+=]{24}), Salt: some other salt"" />$");
        private readonly Regex _randomCookieValueSuffixRegex = new Regex(@", Value: (?<value>[A-Za-z0-9/\+=]{24}), Salt: $");

        [TestMethod]
        public void AntiForgeryTokenThrowsIfHelperIsNull() {
            // Act & Assert
            ExceptionHelper.ExpectArgumentNullException(
                delegate {
                    _helper.AntiForgeryToken(null, null);
                }, "helper");
        }

        [TestMethod]
        public void AntiForgeryTokenSetsCookieValueIfDoesNotExist() {
            // Arrange
            HtmlHelper htmlHelper = GetHtmlHelper(null);

            // Act
            string formValue = _helper.AntiForgeryToken(htmlHelper, "some other salt");

            // Assert
            Assert.IsTrue(formValue.StartsWith(_serializedValuePrefix), "Form value prefix did not match.");

            Match formMatch = _randomFormValueSuffixRegex.Match(formValue);
            string formTokenValue = formMatch.Groups["value"].Value;

            HttpCookie cookie = htmlHelper.ViewContext.HttpContext.Response.Cookies["__MVC_AntiForgeryToken"];
            Assert.IsNotNull(cookie, "Cookie was not set correctly.");

            Match cookieMatch = _randomCookieValueSuffixRegex.Match(cookie.Value);
            string cookieTokenValue = cookieMatch.Groups["value"].Value;

            Assert.AreEqual(formTokenValue, cookieTokenValue, "Form and cookie token values did not match.");
        }

        [TestMethod]
        public void AntiForgeryTokenUsesCookieValueIfExists() {
            // Arrange
            HtmlHelper htmlHelper = GetHtmlHelper("2001-01-01:some value:some salt");

            // Act
            string formValue = _helper.AntiForgeryToken(htmlHelper, "some other salt");

            // Assert
            Assert.IsTrue(formValue.StartsWith(_serializedValuePrefix), "Form value prefix did not match.");
            Assert.IsTrue(formValue.EndsWith(_someValueSuffix), "Form value suffix did not match.");
            Assert.AreEqual(0, htmlHelper.ViewContext.HttpContext.Response.Cookies.Count, "Cookie should not have been added to response.");
        }

        [TestMethod]
        public void SerializerProperty() {
            // Arrange
            AntiForgeryHelper helper = new AntiForgeryHelper();
            AntiForgeryTokenSerializer defaultSerializer = DefaultAntiForgeryTokenSerializer.Instance;
            AntiForgeryTokenSerializer newSerializer = new Mock<AntiForgeryTokenSerializer>().Object;

            // Act & Assert
            MemberHelper.TestPropertyWithDefaultInstance(helper, "Serializer", newSerializer, defaultSerializer);
        }

        public static HtmlHelper GetHtmlHelper(string cookieValue) {
            Mock<HttpRequestBase> mockRequest = new Mock<HttpRequestBase>();
            HttpCookieCollection requestCookies = new HttpCookieCollection();
            mockRequest.Expect(r => r.Cookies).Returns(requestCookies);
            if (!String.IsNullOrEmpty(cookieValue)) {
                requestCookies.Set(new HttpCookie("__MVC_AntiForgeryToken", cookieValue));
            }

            Mock<HttpResponseBase> mockResponse = new Mock<HttpResponseBase>();
            HttpCookieCollection responseCookies = new HttpCookieCollection();
            mockResponse.Expect(r => r.Cookies).Returns(responseCookies);

            Mock<HttpContextBase> mockHttpContext = new Mock<HttpContextBase>();
            mockHttpContext.Expect(c => c.Request).Returns(mockRequest.Object);
            mockHttpContext.Expect(c => c.Response).Returns(mockResponse.Object);

            ViewContext viewContext = new ViewContext(mockHttpContext.Object, new RouteData(),
                new Mock<ControllerBase>().Object, "someView", new ViewDataDictionary(), new TempDataDictionary());
            return new HtmlHelper(viewContext, new Mock<IViewDataContainer>().Object);
        }

        private class SubclassedAntiForgeryHelper : AntiForgeryHelper {
            public SubclassedAntiForgeryHelper(AntiForgeryTokenSerializer serializer) {
                Serializer = serializer;
            }
        }

        public class SubclassedAntiForgeryTokenSerializer : AntiForgeryTokenSerializer {
            public override string Serialize(AntiForgeryToken token) {
                return String.Format(CultureInfo.InvariantCulture, "Creation: {0}, Value: {1}, Salt: {2}",
                        token.CreationDate, token.Value, token.Salt);
            }
            public override AntiForgeryToken Deserialize(string serializedToken) {
                string[] parts = serializedToken.Split(':');
                return new AntiForgeryToken() {
                    CreationDate = DateTime.Parse(parts[0], CultureInfo.InvariantCulture),
                    Value = parts[1],
                    Salt = parts[2]
                };
            }
        }

    }
}
