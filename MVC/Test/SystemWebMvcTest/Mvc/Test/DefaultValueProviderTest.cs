namespace System.Web.Mvc.Test {
    using System;
    using System.Collections.Specialized;
    using System.Globalization;
    using System.Threading;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Routing;
    using System.Web.TestUtil;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;

    [TestClass]
    public class DefaultValueProviderTest {

        [TestMethod]
        public void ConstructorThrowsIfControllerContextIsNull() {
            // Act & assert
            ExceptionHelper.ExpectArgumentNullException(
                delegate {
                    new DefaultValueProvider(null);
                }, "controllerContext");
        }

        [TestMethod]
        public void ControllerContextProperty() {
            // Arrange
            ControllerContext controllerContext = GetControllerContext();
            DefaultValueProvider provider = new DefaultValueProvider(controllerContext);

            // Act & assert
            Assert.AreSame(controllerContext, provider.ControllerContext);
        }

        [TestMethod]
        public void GetValueChecksFirstInRouteData() {
            // Arrange
            DefaultValueProvider provider = GetValueProvider();

            // Act
            ValueProviderResult result;
            using (ReplaceCurrentCulture("fr-FR")) {
                result = provider.GetValue("foo");
            }

            // Assert
            Assert.AreEqual(CultureInfo.InvariantCulture, result.Culture);
            Assert.AreEqual("fooRouteData", result.AttemptedValue);
            Assert.AreEqual("fooRouteData", result.RawValue);
        }

        [TestMethod]
        public void GetValueChecksSecondInQueryString() {
            // Arrange
            DefaultValueProvider provider = GetValueProvider();

            // Act
            ValueProviderResult result;
            using (ReplaceCurrentCulture("fr-FR")) {
                result = provider.GetValue("bar");
            }

            // Assert
            Assert.AreEqual(CultureInfo.InvariantCulture, result.Culture);
            Assert.AreEqual("barQuery", result.AttemptedValue);

            string[] rawValue = (string[])result.RawValue;
            Assert.AreEqual(1, rawValue.Length);
            Assert.AreEqual("barQuery", rawValue[0]);
        }

        [TestMethod]
        public void GetValueChecksThirdInForm() {
            // Arrange
            DefaultValueProvider provider = GetValueProvider();

            // Act
            ValueProviderResult result;
            using (ReplaceCurrentCulture("fr-FR")) {
                result = provider.GetValue("baz");
            }

            // Assert
            Assert.AreEqual(CultureInfo.GetCultureInfo("fr-FR"), result.Culture);
            Assert.AreEqual("bazForm", result.AttemptedValue);

            string[] rawValue = (string[])result.RawValue;
            Assert.AreEqual(1, rawValue.Length);
            Assert.AreEqual("bazForm", rawValue[0]);
        }

        [TestMethod]
        public void GetValueReturnsNullResultIfValueNotFound() {
            // Arrange
            DefaultValueProvider provider = GetValueProvider();

            // Act
            ValueProviderResult result = provider.GetValue("valueNotFound");

            // Assert
            Assert.IsNull(result, "GetValue() should return null if value not found.");
        }

        [TestMethod]
        public void GetValueThrowsIfNameIsEmpty() {
            // Arrange
            DefaultValueProvider provider = GetValueProvider();

            // Act & assert
            ExceptionHelper.ExpectArgumentExceptionNullOrEmpty(
                delegate {
                    provider.GetValue(String.Empty);
                }, "name");
        }

        [TestMethod]
        public void GetValueThrowsIfNameIsName() {
            // Arrange
            DefaultValueProvider provider = GetValueProvider();

            // Act & assert
            ExceptionHelper.ExpectArgumentExceptionNullOrEmpty(
                delegate {
                    provider.GetValue(null);
                }, "name");
        }

        private static ControllerContext GetControllerContext() {
            NameValueCollection formCollection = new NameValueCollection {
                { "foo", "fooForm" },
                { "bar", "barForm" },
                { "baz", "bazForm" }
            };
            NameValueCollection queryCollection = new NameValueCollection {
                { "foo", "fooQuery" },
                { "bar", "barQuery" }
            };
            Mock<HttpRequestBase> mockRequest = new Mock<HttpRequestBase>();
            mockRequest.Expect(r => r.Form).Returns(formCollection);
            mockRequest.Expect(r => r.QueryString).Returns(queryCollection);

            Mock<HttpContextBase> mockHttpContext = new Mock<HttpContextBase>();
            mockHttpContext.Expect(c => c.Request).Returns(mockRequest.Object);

            RouteData routeData = new RouteData();
            routeData.Values["foo"] = "fooRouteData";

            return new ControllerContext(mockHttpContext.Object, routeData, new Mock<ControllerBase>().Object);
        }

        private static DefaultValueProvider GetValueProvider() {
            return new DefaultValueProvider(GetControllerContext());
        }

        public static IDisposable ReplaceCurrentCulture(string culture) {
            CultureInfo newCulture = CultureInfo.GetCultureInfo(culture);
            CultureInfo originalCulture = Thread.CurrentThread.CurrentCulture;
            Thread.CurrentThread.CurrentCulture = newCulture;
            return new CultureReplacement { OriginalCulture = originalCulture };
        }

        private class CultureReplacement : IDisposable {
            public CultureInfo OriginalCulture;
            public void Dispose() {
                Thread.CurrentThread.CurrentCulture = OriginalCulture;
            }
        }

    }
}
