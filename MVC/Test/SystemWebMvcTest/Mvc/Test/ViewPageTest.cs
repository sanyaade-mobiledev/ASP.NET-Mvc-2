namespace System.Web.Mvc.Test {
    using System.Web.TestUtil;
    using System.Web.UI;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class ViewPageTest {

        [TestMethod]
        public void SetViewItemOnBaseClassPropagatesToDerivedClass() {
            // Arrange
            ViewPage<object> vpInt = new ViewPage<object>();
            ViewPage vp = vpInt;
            object o = new object();

            // Act
            vp.ViewData.Model = o;

            // Assert
            Assert.AreEqual(o, vpInt.ViewData.Model);
            Assert.AreEqual(o, vp.ViewData.Model);
        }

        [TestMethod]
        public void SetViewItemOnDerivedClassPropagatesToBaseClass() {
            // Arrange
            ViewPage<object> vpInt = new ViewPage<object>();
            ViewPage vp = vpInt;
            object o = new object();

            // Act
            vpInt.ViewData.Model = o;

            // Assert
            Assert.AreEqual(o, vpInt.ViewData.Model);
            Assert.AreEqual(o, vp.ViewData.Model);
        }

        [TestMethod]
        public void SetViewItemToWrongTypeThrows() {
            // Arrange
            ViewPage vp = new ViewPage<string>();

            // Act & Assert
            ExceptionHelper.ExpectException<InvalidOperationException>(
                delegate {
                    vp.ViewData.Model = 50;
                },
                "The model item passed into the dictionary is of type 'System.Int32' but this dictionary requires a model item of type 'System.String'.");
        }

        private static void WriterSetCorrectlyInternal(bool throwException) {

            // Arrange
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

            // Act & Assert
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
