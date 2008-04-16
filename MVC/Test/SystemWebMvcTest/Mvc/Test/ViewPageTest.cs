namespace System.Web.Mvc.Test {
    using System.Web.TestUtil;
    using System.Web.UI;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class ViewPageTest {
        [TestMethod]
        public void SetViewData() {
            ViewPage vp = new ViewPage();
            vp.SetViewData(new { a = "123", b = "456" });

            Assert.AreEqual("123", vp.ViewData["a"]);
            Assert.AreEqual("456", vp.ViewData["b"]);
        }

        [TestMethod]
        public void SetViewDataGeneric() {
            MockViewPageDummyViewData vp = new MockViewPageDummyViewData();
            vp.SetViewData(new DummyViewData { MyInt = 123, MyString = "abc" });

            Assert.AreEqual(123, vp.ViewData.MyInt);
            Assert.AreEqual("abc", vp.ViewData.MyString);
        }

        [TestMethod]
        public void SetNullViewDataDoesNothing() {
            MockViewPageDummyViewData vp = new MockViewPageDummyViewData();
            vp.SetViewData(null);
        }

        [TestMethod]
        public void SetViewDataWrongGenericTypeThrows() {
            MockViewPageBogusViewData vp = new MockViewPageBogusViewData();

            ExceptionHelper.ExpectArgumentException(
                delegate {
                    vp.SetViewData(new DummyViewData { MyInt = 123, MyString = "abc" });
                },
                "The view data passed into the page is of type 'System.Web.Mvc.Test.ViewPageTest+DummyViewData' but this page requires view data of type 'System.Int32'.\r\nParameter name: viewData");
        }

        private static void WriterSetCorrectlyInternal(bool throwException) {

            // Setup
            bool triggered = false;
            HtmlTextWriter writer = new HtmlTextWriter(System.IO.TextWriter.Null);
            MockViewPage vp = new MockViewPage();
            vp.RenderCallback = delegate() {
                triggered = true;
                Assert.AreEqual(writer, vp.Writer);
                if (throwException) {
                    throw new CallbackException();
                }
            };

            // Execute & verify
            Assert.IsNull(vp.Writer);
            try {
                vp.RenderControl(writer);
            }
            catch (CallbackException) { }
            Assert.IsNull(vp.Writer);
            Assert.IsTrue(triggered);

        }

        [TestMethod]
        public void WriterSetCorrectly() {
            WriterSetCorrectlyInternal(false /* throwException */);
        }

        [TestMethod]
        public void WriterSetCorrectlyThrowException() {
            WriterSetCorrectlyInternal(true /* throwException */);
        }

        private sealed class MockViewPageBogusViewData : ViewPage<int> {
        }

        private sealed class MockViewPageDummyViewData : ViewPage<DummyViewData> {
        }

        private sealed class DummyViewData {
            public int MyInt { get; set; }
            public string MyString { get; set; }
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

        private sealed class CallbackException : Exception {

        }
    }
}
