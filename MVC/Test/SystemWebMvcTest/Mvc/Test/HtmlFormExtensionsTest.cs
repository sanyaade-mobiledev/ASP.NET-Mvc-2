namespace System.Web.Mvc.Html.Test {
    using System;
    using System.Web.Mvc.Test;
    using System.Web.Routing;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;

    [TestClass]
    public class HtmlFormExtensionsTest {
        private static void BeginFormHelper(Func<HtmlHelper, MvcForm> beginForm, string expectedFormTag) {
            // Arrange
            Mock<HttpResponseBase> mockHttpResponse;
            HtmlHelper htmlHelper = GetFormHelper(expectedFormTag, @"</form>", out mockHttpResponse);

            // Act
            IDisposable formDisposable = beginForm(htmlHelper);
            formDisposable.Dispose();

            // Assert
            mockHttpResponse.Verify();
        }

        [TestMethod]
        public void BeginFormWithActionControllerInvalidFormMethodHtmlValues() {
            BeginFormHelper(
                htmlHelper => htmlHelper.BeginForm("bar", "foo", (FormMethod)2, new RouteValueDictionary(new { baz = "baz" })),
                @"<form action=""" + HtmlHelperTest.AppPathModifier + @"/foo/bar"" baz=""baz"" method=""post"">");
        }

        [TestMethod]
        public void BeginFormWithActionController() {
            BeginFormHelper(
                htmlHelper => htmlHelper.BeginForm("bar", "foo"),
                @"<form action=""" + HtmlHelperTest.AppPathModifier + @"/foo/bar"" method=""post"">");
        }

        [TestMethod]
        public void BeginFormWithActionControllerFormMethodHtmlDictionary() {
            BeginFormHelper(
                htmlHelper => htmlHelper.BeginForm("bar", "foo", FormMethod.Get, new RouteValueDictionary(new { baz = "baz" })),
                @"<form action=""" + HtmlHelperTest.AppPathModifier + @"/foo/bar"" baz=""baz"" method=""get"">");
        }

        [TestMethod]
        public void BeginFormWithActionControllerFormMethodHtmlValues() {
            BeginFormHelper(
                htmlHelper => htmlHelper.BeginForm("bar", "foo", FormMethod.Get, new { baz = "baz" }),
                @"<form action=""" + HtmlHelperTest.AppPathModifier + @"/foo/bar"" baz=""baz"" method=""get"">");
        }

        [TestMethod]
        public void BeginFormWithActionControllerRouteDictionaryFormMethodHtmlDictionary() {
            BeginFormHelper(
                htmlHelper => htmlHelper.BeginForm("bar", "foo", new RouteValueDictionary(new { id = "id" }), FormMethod.Get, new RouteValueDictionary(new { baz = "baz" })),
                @"<form action=""" + HtmlHelperTest.AppPathModifier + @"/foo/bar/id"" baz=""baz"" method=""get"">");
        }

        [TestMethod]
        public void BeginFormWithActionControllerRouteValuesFormMethodHtmlValues() {
            BeginFormHelper(
                htmlHelper => htmlHelper.BeginForm("bar", "foo", new { id = "id" }, FormMethod.Get, new { baz = "baz" }),
                @"<form action=""" + HtmlHelperTest.AppPathModifier + @"/foo/bar/id"" baz=""baz"" method=""get"">");
        }

        [TestMethod]
        public void BeginFormWithActionControllerNullRouteValuesFormMethodNullHtmlValues() {
            BeginFormHelper(
                htmlHelper => htmlHelper.BeginForm("bar", "foo", null, FormMethod.Get, null),
                @"<form action=""" + HtmlHelperTest.AppPathModifier + @"/foo/bar"" method=""get"">");
        }

        [TestMethod]
        public void BeginFormWithRouteValues() {
            BeginFormHelper(
                htmlHelper => htmlHelper.BeginForm(new { action = "someOtherAction", id = "id" }),
                @"<form action=""" + HtmlHelperTest.AppPathModifier + @"/home/someOtherAction/id"" method=""post"">");
        }

        [TestMethod]
        public void BeginFormWithRouteDictionary() {
            BeginFormHelper(
                htmlHelper => htmlHelper.BeginForm(new RouteValueDictionary { { "action", "someOtherAction" }, { "id", "id" } }),
                @"<form action=""" + HtmlHelperTest.AppPathModifier + @"/home/someOtherAction/id"" method=""post"">");
        }

        [TestMethod]
        public void BeginFormWithActionControllerRouteValues() {
            BeginFormHelper(
                htmlHelper => htmlHelper.BeginForm("myAction", "myController", new { id = "id", pageNum = "123" }),
                @"<form action=""" + HtmlHelperTest.AppPathModifier + @"/myController/myAction/id?pageNum=123"" method=""post"">");
        }

        [TestMethod]
        public void BeginFormWithActionControllerRouteDictionary() {
            BeginFormHelper(
                htmlHelper => htmlHelper.BeginForm("myAction", "myController", new RouteValueDictionary { { "pageNum", "123" }, { "id", "id" } }),
                @"<form action=""" + HtmlHelperTest.AppPathModifier + @"/myController/myAction/id?pageNum=123"" method=""post"">");
        }

        [TestMethod]
        public void BeginFormWithActionControllerMethod() {
            BeginFormHelper(
                htmlHelper => htmlHelper.BeginForm("myAction", "myController", FormMethod.Get),
                @"<form action=""" + HtmlHelperTest.AppPathModifier + @"/myController/myAction"" method=""get"">");
        }

        [TestMethod]
        public void BeginFormWithActionControllerRouteValuesMethod() {
            BeginFormHelper(
                htmlHelper => htmlHelper.BeginForm("myAction", "myController", new { id = "id", pageNum = "123" }, FormMethod.Get),
                @"<form action=""" + HtmlHelperTest.AppPathModifier + @"/myController/myAction/id?pageNum=123"" method=""get"">");
        }

        [TestMethod]
        public void BeginFormWithActionControllerRouteDictionaryMethod() {
            BeginFormHelper(
                htmlHelper => htmlHelper.BeginForm("myAction", "myController", new RouteValueDictionary { { "pageNum", "123" }, { "id", "id" } }, FormMethod.Get),
                @"<form action=""" + HtmlHelperTest.AppPathModifier + @"/myController/myAction/id?pageNum=123"" method=""get"">");
        }

        [TestMethod]
        public void BeginFormWithNoParams() {
            BeginFormHelper(
                htmlHelper => htmlHelper.BeginForm(),
                @"<form action=""http://www.contoso.com/some/path"" method=""post"">");
        }

        [TestMethod]
        public void EndFormWritesCloseTag() {
            // Arrange
            Mock<HttpResponseBase> mockHttpResponse = new Mock<HttpResponseBase>(MockBehavior.Strict);
            mockHttpResponse.Expect(r => r.Write("</form>")).Verifiable();

            Mock<HttpContextBase> mockHttpContext = new Mock<HttpContextBase>();
            mockHttpContext.Expect(c => c.Response).Returns(mockHttpResponse.Object);
            HtmlHelper htmlHelper = new HtmlHelper(
                new ViewContext(mockHttpContext.Object, new RouteData(), new Mock<ControllerBase>().Object, new Mock<IView>().Object, new ViewDataDictionary(), new TempDataDictionary()),
                new Mock<IViewDataContainer>().Object,
                new RouteCollection());

            // Act
            htmlHelper.EndForm();

            // Assert
            mockHttpResponse.Verify();
        }

        private static HtmlHelper GetFormHelper(string formOpenTagExpectation, string formCloseTagExpectation, out Mock<HttpResponseBase> mockHttpResponse) {
            Mock<HttpRequestBase> mockHttpRequest = new Mock<HttpRequestBase>();
            mockHttpRequest.Expect(r => r.Url).Returns(new Uri("http://www.contoso.com/some/path"));
            mockHttpResponse = new Mock<HttpResponseBase>(MockBehavior.Strict);
            mockHttpResponse.Expect(r => r.Write(formOpenTagExpectation)).Verifiable();

            if (!String.IsNullOrEmpty(formCloseTagExpectation)) {
                mockHttpResponse.Expect(r => r.Write(formCloseTagExpectation)).AtMostOnce().Verifiable();
            }

            mockHttpResponse.Expect(r => r.ApplyAppPathModifier(It.IsAny<string>())).Returns<string>(r => HtmlHelperTest.AppPathModifier + r);
            Mock<HttpContextBase> mockHttpContext = new Mock<HttpContextBase>();
            mockHttpContext.Expect(c => c.Request).Returns(mockHttpRequest.Object);
            mockHttpContext.Expect(c => c.Response).Returns(mockHttpResponse.Object);
            RouteCollection rt = new RouteCollection();
            rt.Add(new Route("{controller}/{action}/{id}", null) { Defaults = new RouteValueDictionary(new { id = "defaultid" }) });
            rt.Add("namedroute", new Route("named/{controller}/{action}/{id}", null) { Defaults = new RouteValueDictionary(new { id = "defaultid" }) });
            RouteData rd = new RouteData();
            rd.Values.Add("controller", "home");
            rd.Values.Add("action", "oldaction");
            HtmlHelper helper = new HtmlHelper(
                new ViewContext(mockHttpContext.Object, rd, new Mock<ControllerBase>().Object, new Mock<IView>().Object, new ViewDataDictionary(), new TempDataDictionary()),
                new Mock<IViewDataContainer>().Object,
                rt);
            return helper;
        }
    }
}
