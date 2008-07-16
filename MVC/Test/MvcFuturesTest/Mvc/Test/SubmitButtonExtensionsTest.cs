namespace MvcFuturesTest.Mvc.Test {
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Routing;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Microsoft.Web.Mvc;
    using Moq;

    [TestClass]
    public class SubmitButtonExtensionsTest {
        [TestMethod]
        public void SubmitButtonRendersWithJustTypeAttribute() {
            HtmlHelper html = TestHelper.GetHtmlHelper(new ViewDataDictionary());
            string button = html.SubmitButton();
            Assert.AreEqual("<input type=\"submit\" />", button);
        }
        
        [TestMethod]
        public void SubmitButtonWithNameRendersButtonWithNameAttribute() {
            HtmlHelper html = TestHelper.GetHtmlHelper(new ViewDataDictionary());
            string button = html.SubmitButton("button-name");
            Assert.AreEqual("<input type=\"submit\" id=\"button-name\" name=\"button-name\" />", button);
        }

        [TestMethod]
        public void SubmitButtonWithNameAndTextRendersAttributes() {
            HtmlHelper html = TestHelper.GetHtmlHelper(new ViewDataDictionary());
            string button = html.SubmitButton("button-name", "button-text");
            Assert.AreEqual("<input type=\"submit\" id=\"button-name\" name=\"button-name\" value=\"button-text\" />", button);
        }

        [TestMethod]
        public void SubmitButtonWithNameAndValueRendersBothAttributes() {
            HtmlHelper html = TestHelper.GetHtmlHelper(new ViewDataDictionary());
            string button = html.SubmitButton("button-name", "button-value", new { id = "button-id" });
            Assert.AreEqual("<input id=\"button-id\" type=\"submit\" name=\"button-name\" value=\"button-value\" />", button);
        }

        [TestMethod]
        public void SubmitButtonWithNameAndIdRendersBothAttributesCorrectly() {
            HtmlHelper html = TestHelper.GetHtmlHelper(new ViewDataDictionary());
            string button = html.SubmitButton("button-name", "button-value", new { id = "button-id" });
            Assert.AreEqual("<input id=\"button-id\" type=\"submit\" name=\"button-name\" value=\"button-value\" />", button);
        }

        [TestMethod]
        public void SubmitButtonWithTypeAttributeRendersCorrectType() {
            HtmlHelper html = TestHelper.GetHtmlHelper(new ViewDataDictionary());
            string button = html.SubmitButton("specified-name", "button-value", new {type="not-submit"});
            Assert.AreEqual("<input type=\"not-submit\" id=\"specified-name\" name=\"specified-name\" value=\"button-value\" />", button);
        }

        [TestMethod]
        public void SubmitButtonWithNameAndValueSpecifiedAndPassedInAsAttributeChoosesSpecified() {
            HtmlHelper html = TestHelper.GetHtmlHelper(new ViewDataDictionary());
            string button = html.SubmitButton("specified-name", "button-value"
                , new RouteValueDictionary(new { name = "name-attribute-value", value="value-attribute" }));
            Assert.AreEqual("<input name=\"name-attribute-value\" value=\"value-attribute\" type=\"submit\" id=\"specified-name\" />", button);
        }
    }
}
