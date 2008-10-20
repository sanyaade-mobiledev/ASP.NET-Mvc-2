namespace System.Web.Mvc.Test {
    using System.IO;
    using System.Web.Mvc;
    using System.Web.Routing;
    using System.Web.TestUtil;
    using System.Web.UI;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;

    [TestClass]
    public class ViewPageTest {
        private const string _fakeID = "fakeID";

        [TestMethod]
        public void RenderViewSetsID() {
            // Arrange
            ControllerBase controller = new Mock<ControllerBase>().Object;
            HttpContextBase httpContext = CreateHttpContext();
            RouteData routeData = new RouteData();
            ControllerContext context = new ControllerContext(httpContext, routeData, controller);
            Mock<IView> view = new Mock<IView>();
            ViewContext viewContext = new ViewContext(httpContext, routeData, new Mock<ControllerBase>().Object, view.Object, new ViewDataDictionary(), new TempDataDictionary());
            ViewPage viewPage = new IDViewPage();
            viewPage.ViewContext = viewContext;

            // Act
            viewPage.RenderView(viewContext);

            // Assert
            Assert.AreNotEqual(_fakeID, viewPage.ID);
        }

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

        private static HttpContextBase CreateHttpContext() {
            TextWriter writer = new Mock<TextWriter>().Object;
            Mock<HttpResponseBase> httpResponse = new Mock<HttpResponseBase>();
            httpResponse.Expect(r => r.Output).Returns(writer);
            Mock<HttpContextBase> result = new Mock<HttpContextBase>();
            result.Expect(c => c.Response).Returns(httpResponse.Object);
            return result.Object;
        }

        private sealed class IDViewPage : ViewPage {
           public override void ProcessRequest(HttpContext context) {
                // Ignore the request, We're only interested in the ID being set by RenderView.
            }
        }

        private sealed class MockViewPage : ViewPage {

            public MockViewPage() {
                ID = _fakeID;
            }

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
