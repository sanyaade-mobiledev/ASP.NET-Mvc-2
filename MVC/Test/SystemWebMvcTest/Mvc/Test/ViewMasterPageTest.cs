﻿namespace System.Web.Mvc.Test {
    using System.Web.Routing;
    using System.Web.TestUtil;
    using System.Web.UI;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;

    [TestClass]
    public class ViewMasterPageTest {
        [TestMethod]
        public void GetViewDataFromViewPage() {
            // Setup
            ViewMasterPage vmp = new ViewMasterPage();
            ViewPage vp = new ViewPage();
            vmp.Page = vp;
            vp.SetViewData(new { a = "123", b = "456" });

            // Verify
            Assert.AreEqual("123", vmp.ViewData["a"]);
            Assert.AreEqual("456", vmp.ViewData["b"]);
        }

        [TestMethod]
        public void GetViewDataFromViewPageTViewData() {
            // Setup
            MockViewMasterPageDummyViewData vmp = new MockViewMasterPageDummyViewData();
            MockViewPageDummyViewData vp = new MockViewPageDummyViewData();
            vmp.Page = vp;
            vp.SetViewData(new DummyViewData { MyInt = 123, MyString = "abc" });

            // Verify
            Assert.AreEqual(123, vmp.ViewData.MyInt);
            Assert.AreEqual("abc", vmp.ViewData.MyString);
        }

        [TestMethod]
        public void GetWriterFromViewPage() {
            // Setup
            bool triggered = false;
            HtmlTextWriter writer = new HtmlTextWriter(System.IO.TextWriter.Null);
            ViewMasterPage vmp = new ViewMasterPage();
            MockViewPage vp = new MockViewPage();
            vp.RenderCallback = delegate() {
                triggered = true;
                Assert.AreEqual(writer, vmp.Writer);
            };
            vmp.Page = vp;

            // Execute & verify
            Assert.IsNull(vmp.Writer);
            vp.RenderControl(writer);
            Assert.IsNull(vmp.Writer);
            Assert.IsTrue(triggered);
        }

        [TestMethod]
        public void GetViewDataFromPageThrows() {
            // Setup
            ViewMasterPage vmp = new ViewMasterPage();
            vmp.Page = new Page();

            // Verify
            ExceptionHelper.ExpectException<InvalidOperationException>(
                delegate {
                    object foo = vmp.ViewData["foo"];
                },
                "A ViewMasterPage can only be used with content pages that derive from ViewPage or ViewPage<TViewData>.");
        }

        [TestMethod]
        public void GetViewDataFromWrongGenericViewPageType() {
            // Setup
            MockViewMasterPageDummyViewData vmp = new MockViewMasterPageDummyViewData();
            MockViewPageBogusViewData vp = new MockViewPageBogusViewData();
            vmp.Page = vp;
            vp.SetViewData(123);

            // Verify
            ExceptionHelper.ExpectException<InvalidOperationException>(
                delegate {
                    int foo = vmp.ViewData.MyInt;
                },
                "A ViewMasterPage<TViewData> can only be used with content pages that derive from ViewPage<TViewData>.");
        }

        [TestMethod]
        public void GetViewDataFromNullPageThrows() {
            // Setup
            MockViewMasterPageDummyViewData vmp = new MockViewMasterPageDummyViewData();

            // Verify
            ExceptionHelper.ExpectException<InvalidOperationException>(
                delegate {
                    int foo = vmp.ViewData.MyInt;
                },
                "A ViewMasterPage<TViewData> can only be used with content pages that derive from ViewPage<TViewData>.");
        }

        [TestMethod]
        public void GetViewDataFromRegularPageThrows() {
            // Setup
            MockViewMasterPageDummyViewData vmp = new MockViewMasterPageDummyViewData();
            vmp.Page = new Page();

            // Verify
            ExceptionHelper.ExpectException<InvalidOperationException>(
                delegate {
                    int foo = vmp.ViewData.MyInt;
                },
                "A ViewMasterPage<TViewData> can only be used with content pages that derive from ViewPage<TViewData>.");
        }

        [TestMethod]
        public void GetHtmlHelperFromViewPage() {
            // Setup
            ViewMasterPage vmp = new ViewMasterPage();
            ViewPage vp = new ViewPage();
            vmp.Page = vp;
            ViewContext vc = new ViewContext(
                new Mock<HttpContextBase>().Object,
                new RouteData(),
                new Mock<IController>().Object,
                "view",
                null,
                null,
                new TempDataDictionary(ControllerContextTest.GetEmptyContextForTempData()));

            HtmlHelper htmlHelper = new HtmlHelper(vc);
            vp.Html = htmlHelper;

            // Verify
            Assert.AreEqual(vmp.Html, htmlHelper);
        }

        [TestMethod]
        public void GetUrlHelperFromViewPage() {
            // Setup
            ViewMasterPage vmp = new ViewMasterPage();
            ViewPage vp = new ViewPage();
            vmp.Page = vp;
            ViewContext vc = new ViewContext(
                new Mock<HttpContextBase>().Object,
                new RouteData(),
                new Mock<IController>().Object,
                "view",
                null,
                null,
                new TempDataDictionary(ControllerContextTest.GetEmptyContextForTempData()));
            UrlHelper urlHelper = new UrlHelper(vc);
            vp.Url = urlHelper;

            // Verify
            Assert.AreEqual(vmp.Url, urlHelper);
        }

        // Master page types
        private sealed class MockViewMasterPageBogusViewData : ViewMasterPage<int> {
        }

        private sealed class MockViewMasterPageDummyViewData : ViewMasterPage<DummyViewData> {
        }

        // View data types
        private sealed class DummyViewData {
            public int MyInt { get; set; }
            public string MyString { get; set; }
        }

        // Page types
        private sealed class MockViewPageBogusViewData : ViewPage<int> {
        }

        private sealed class MockViewPageDummyViewData : ViewPage<DummyViewData> {
        }

        private sealed class MockViewPage : ViewPage {

            public Action RenderCallback { get; set; }

            protected override void RenderChildren(HtmlTextWriter writer) {
                if (RenderCallback != null) {
                    RenderCallback();
                }
                base.RenderChildren(writer);
            }

        }
    }
}