namespace MvcFuturesTest.Mvc.Test {
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Routing;
    using System.Web.TestUtil;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Microsoft.Web.Mvc;
    using Moq;

    [TestClass]
    public class SubmitImageExtensionsTest {
        [TestMethod]
        public void SubmitImageWithEmptyImageSrcThrowsArgumentNullException() {
            HtmlHelper html = TestHelper.GetHtmlHelper(new ViewDataDictionary());
            ExceptionHelper.ExpectArgumentNullException(() => html.SubmitImage("name", null), "imageSrc");
        }

        [TestMethod]
        public void SubmitImageWithTypeAttributeRendersExplicitTypeAttribute() {
            HtmlHelper html = TestHelper.GetHtmlHelper(new ViewDataDictionary());
            string button = html.SubmitImage("specified-name", "/mvc.jpg", new { type = "not-image" });
            Assert.AreEqual("<input type=\"not-image\" id=\"specified-name\" name=\"specified-name\" src=\"/mvc.jpg\" />", button);
        }
        
        [TestMethod]
        public void SubmitImageWithNameAndImageUrlRendersNameAndSrcAttributes() {
            HtmlHelper html = TestHelper.GetHtmlHelper(new ViewDataDictionary());
            string button = html.SubmitImage("button-name", "/mvc.gif");
            Assert.AreEqual("<input type=\"image\" id=\"button-name\" name=\"button-name\" src=\"/mvc.gif\" />", button);
        }

        [TestMethod]
        public void SubmitImageWithImageUrlStartingWithTildeRendersAppPath() {
            HtmlHelper html = TestHelper.GetHtmlHelper(new ViewDataDictionary(), "/app");
            string button = html.SubmitImage("button-name", "~/mvc.gif");
            Assert.AreEqual("<input type=\"image\" id=\"button-name\" name=\"button-name\" src=\"/app/mvc.gif\" />", button);
        }

        [TestMethod]
        public void SubmitImageWithNameAndIdRendersBothAttributesCorrectly() {
            HtmlHelper html = TestHelper.GetHtmlHelper(new ViewDataDictionary());
            string button = html.SubmitImage("button-name", "/mvc.png", new { id = "button-id" });
            Assert.AreEqual("<input id=\"button-id\" type=\"image\" name=\"button-name\" src=\"/mvc.png\" />", button);
        }

        [TestMethod]
        public void SubmitButtonWithNameAndValueSpecifiedAndPassedInAsAttributeChoosesExplicitAttributes() {
            HtmlHelper html = TestHelper.GetHtmlHelper(new ViewDataDictionary());
            string button = html.SubmitImage("specified-name", "/specified-src.bmp"
                , new RouteValueDictionary(new { name = "name-attribute", src="src-attribute" }));
            Assert.AreEqual("<input name=\"name-attribute\" src=\"src-attribute\" type=\"image\" id=\"specified-name\" />", button);
        }
    }
}
