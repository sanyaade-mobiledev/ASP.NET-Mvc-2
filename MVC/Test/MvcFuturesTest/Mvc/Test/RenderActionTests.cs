namespace MvcFuturesTest.Mvc.Test {
    using System.Text;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System.Web.Mvc;
    using Moq;
    using Microsoft.Web.Mvc;
    using System.Web.Routing;

    [TestClass]
    public class RenderActionTests {
        [TestMethod]
        public void RenderActionWithActionAndControllerSpecifiedRendersCorrectAction() {
            HtmlHelper html = TestHelper.GetHtmlHelper(new ViewDataDictionary(), "/");
            StringBuilder written = MockHelper.SwitchResponseMockOutputToStringBuilder(html.ViewContext.HttpContext.Response);

            SetupControllerForRenderAction(new TestController());

            html.RenderAction("index", "Test");
            Assert.AreEqual("It Worked!", written.ToString());
        }

        [TestMethod]
        public void RenderActionWithActionControllerAndParametersRendersCorrectAction() {
            HtmlHelper html = TestHelper.GetHtmlHelper(new ViewDataDictionary(), "/");
            StringBuilder written = MockHelper.SwitchResponseMockOutputToStringBuilder(html.ViewContext.HttpContext.Response);

            SetupControllerForRenderAction(new TestController());

            html.RenderAction("About", "Test", new { page=75 });
            Assert.AreEqual("This is page #75", written.ToString());
        }

        [TestMethod]
        public void RenderActionUsingExpressionRendersCorrectly() {
            HtmlHelper html = TestHelper.GetHtmlHelper(new ViewDataDictionary(), "/");
            StringBuilder written = MockHelper.SwitchResponseMockOutputToStringBuilder(html.ViewContext.HttpContext.Response);

            SetupControllerForRenderAction(new TestController());

            html.RenderAction<TestController>(c => c.About(76));
            Assert.AreEqual("This is page #76", written.ToString());
        }

        [TestMethod]
        public void RenderRouteWithActionAndControllerSpecifiedRendersCorrectAction() {
            HtmlHelper html = TestHelper.GetHtmlHelper(new ViewDataDictionary(), "/");
            StringBuilder written = MockHelper.SwitchResponseMockOutputToStringBuilder(html.ViewContext.HttpContext.Response);

            SetupControllerForRenderAction(new TestController());

            html.RenderRoute(new RouteValueDictionary(new {action="Index", controller="Test"}));
            Assert.AreEqual("It Worked!", written.ToString());
        }

        [TestMethod]
        public void RenderActionWithActionOnlySpecifiedAndControllerInRouteDataRendersCorrectAction() {
            HtmlHelper html = TestHelper.GetHtmlHelper(new ViewDataDictionary(), "/");
            html.ViewContext.RouteData.Values.Add("controller", "Test");
            StringBuilder written = MockHelper.SwitchResponseMockOutputToStringBuilder(html.ViewContext.HttpContext.Response);

            SetupControllerForRenderAction(new TestController());

            html.RenderAction("index");
            Assert.AreEqual("It Worked!", written.ToString());
        }

        private static void SetupControllerForRenderAction(Controller controller) {
            var factory = new Mock<IControllerFactory>();
            var tempDataProvider = new Mock<ITempDataProvider>();
            controller.TempDataProvider = tempDataProvider.Object;
            tempDataProvider.Expect(provider => provider.LoadTempData()).Returns(new TempDataDictionary());
            tempDataProvider.Expect(provider => provider.SaveTempData(It.IsAny<TempDataDictionary>()));

            factory.Expect(f => f.CreateController(It.IsAny<RequestContext>(), It.IsAny<string>())).Returns(controller);
            factory.Expect(f => f.DisposeController(It.IsAny<IController>()));

            ControllerBuilder.Current.SetControllerFactory(factory.Object);
        }

        public class TestController : Controller {
            public string Index() {
                return "It Worked!";
            }

            public string About(int page) {
                return "This is page #" + page;
            }
        }
    }
}
