namespace MvcFuturesTest.Mvc.Test {
    using System.Web.Mvc;
    using System.Web.Routing;
    using System.Web.TestUtil;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Microsoft.Web.Mvc;

    [TestClass]
    public class ButtonTests {
        [TestMethod]
        public void ButtonWithNullNameThrowsArgumentNullException() {
            HtmlHelper html = TestHelper.GetHtmlHelper(new ViewDataDictionary());
            ExceptionHelper.ExpectArgumentNullException(() => html.Button(null, "text", "onclick"), "name");
        }

        [TestMethod]
        public void ButtonRendersBaseAttributes() {
            HtmlHelper html = TestHelper.GetHtmlHelper(new ViewDataDictionary());
            string result = html.Button("nameAttr", "textAttr", "onclickAttr");
            Assert.AreEqual("<button name=\"nameAttr\" onclick=\"onclickAttr\" value=\"textAttr\" />", result);
        }
        
        [TestMethod]
        public void ButtonRendersExplicitAttributes() {
            HtmlHelper html = TestHelper.GetHtmlHelper(new ViewDataDictionary());
            string result = html.Button("nameAttr", "textAttr", "onclickAttr", new { title="the-title" });
            Assert.AreEqual("<button name=\"nameAttr\" onclick=\"onclickAttr\" title=\"the-title\" value=\"textAttr\" />", result);
        }

        [TestMethod]
        public void ButtonRendersExplicitDictionaryAttributes() {
            HtmlHelper html = TestHelper.GetHtmlHelper(new ViewDataDictionary());
            string result = html.Button("nameAttr", "textAttr", "onclickAttr", new RouteValueDictionary(new { title = "the-title" }));
            Assert.AreEqual("<button name=\"nameAttr\" onclick=\"onclickAttr\" title=\"the-title\" value=\"textAttr\" />", result);
        }
    }
}
