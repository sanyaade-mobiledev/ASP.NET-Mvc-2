namespace MvcFuturesTest.Mvc.Test {
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Mvc.Test;
    using System.Web.Routing;
    using System.Web.TestUtil;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Microsoft.Web.Mvc;
    using Moq;

    [TestClass]
    public class ImageExtensionsTest {
        [TestMethod]
        public void ImageWithEmptyRelativeUrlThrowsArgumentNullException() {
            HtmlHelper html = TestHelper.GetHtmlHelper(new ViewDataDictionary());
            ExceptionHelper.ExpectArgumentNullException(() => html.Image(null), "imageRelativeUrl");
        }

        [TestMethod]
        public void ImageStaticWithEmptyRelativeUrlThrowsArgumentNullException() {
            ExceptionHelper.ExpectArgumentNullException(() => ImageExtensions.Image((string)null, "alt", null), "imageRelativeUrl");
        }

        [TestMethod]
        public void ImageWithRelativeUrlRendersProperImageTag() {
            HtmlHelper html = TestHelper.GetHtmlHelper(new ViewDataDictionary());
            string imageResult = html.Image("/system/web/mvc.jpg");
            //NOTE: Although XHTML requires an alt tag, we don't construct one for you. Specify it yourself.
            Assert.AreEqual("<img src=\"/system/web/mvc.jpg\" />", imageResult);
        }

        [TestMethod]
        public void ImageWithAltValueRendersImageWithAltTag() {
            HtmlHelper html = TestHelper.GetHtmlHelper(new ViewDataDictionary());
            string imageResult = html.Image("/system/web/mvc.jpg", "this is an alt value");
            Assert.AreEqual("<img src=\"/system/web/mvc.jpg\" alt=\"this is an alt value\" />", imageResult);
        }

        [TestMethod]
        public void ImageWithAltValueInObjectDictionaryRendersImageWithAltTag() {
            HtmlHelper html = TestHelper.GetHtmlHelper(new ViewDataDictionary());
            string imageResult = html.Image("/system/web/mvc.jpg", new { alt = "this is an alt value" });
            Assert.AreEqual("<img alt=\"this is an alt value\" src=\"/system/web/mvc.jpg\" />", imageResult);
        }

        [TestMethod]
        public void ImageWithAltValueHtmlAttributeEncodesAltTag() {
            HtmlHelper html = TestHelper.GetHtmlHelper(new ViewDataDictionary());
            string imageResult = html.Image("/system/web/mvc.jpg", @"<"">");
            Assert.AreEqual("<img src=\"/system/web/mvc.jpg\" alt=\"&lt;&quot;>\" />", imageResult);
        }

        [TestMethod]
        public void ImageWithAltValueInObjectDictionaryHtmlAttributeEncodesAltTag() {
            HtmlHelper html = TestHelper.GetHtmlHelper(new ViewDataDictionary());
            string imageResult = html.Image("/system/web/mvc.jpg", new { alt = "this is an alt value" });
            Assert.AreEqual("<img alt=\"this is an alt value\" src=\"/system/web/mvc.jpg\" />", imageResult);
        }

        //TODO: Verify this behavior with others.
        [TestMethod]
        public void ImageWithAltSpecifiedAndInDictionaryRendersExplicit() {
            HtmlHelper html = TestHelper.GetHtmlHelper(new ViewDataDictionary());
            string imageResult = html.Image("/system/web/mvc.jpg", "specified-alt", new { alt = "object-dictionary-alt" });
            Assert.AreEqual("<img alt=\"object-dictionary-alt\" src=\"/system/web/mvc.jpg\" />", imageResult);
        }

        [TestMethod]
        public void ImageWithSrcSpecifiedAndInDictionaryRendersExplicit() {
            HtmlHelper html = TestHelper.GetHtmlHelper(new ViewDataDictionary());
            string imageResult = html.Image("/system/web/mvc.jpg", new { src = "explicit.jpg" });
            Assert.AreEqual("<img src=\"explicit.jpg\" />", imageResult);
        }


        [TestMethod]
        public void ImageWithOtherAttributesRendersThoseAttributesCaseSensitively() {
            HtmlHelper html = TestHelper.GetHtmlHelper(new ViewDataDictionary());
            string imageResult = html.Image("/system/web/mvc.jpg", new { width=100, Height=200 });
            Assert.AreEqual("<img width=\"100\" Height=\"200\" src=\"/system/web/mvc.jpg\" />", imageResult);
        }

        [TestMethod]
        public void ImageWithUrlAndDictionaryRendersAttributes() {
            HtmlHelper html = TestHelper.GetHtmlHelper(new ViewDataDictionary());
            var attributes = new RouteValueDictionary(new { width=125 });
            string imageResult = html.Image("/system/web/mvc.jpg", attributes);
            Assert.AreEqual("<img width=\"125\" src=\"/system/web/mvc.jpg\" />", imageResult);
        }

        [TestMethod]
        public void ImageWithTildePathAndAppPathResolvesCorrectly() {
            HtmlHelper html = TestHelper.GetHtmlHelper(new ViewDataDictionary(), "/app");
            string imageResult = html.Image("~/system/web/mvc.jpg");
            Assert.AreEqual("<img src=\"/app/system/web/mvc.jpg\" />", imageResult);
        }

        [TestMethod]
        public void ImageWithTildePathWithoutAppPathResolvesCorrectly() {
            HtmlHelper html = TestHelper.GetHtmlHelper(new ViewDataDictionary(), "/");
            string imageResult = html.Image("~/system/web/mvc.jpg");
            Assert.AreEqual("<img src=\"/system/web/mvc.jpg\" />", imageResult);
        }
    }
}
