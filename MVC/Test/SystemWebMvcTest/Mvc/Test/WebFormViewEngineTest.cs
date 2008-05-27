namespace System.Web.Mvc.Test {
    using System;
    using System.Web.Routing;
    using System.Web.TestUtil;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;

    [TestClass]
    public class WebFormViewEngineTest {
        [TestMethod]
        public void RenderViewWithNullContextThrows() {
            // Setup
            IViewEngine vf = new WebFormViewEngine();

            // Execute
            ExceptionHelper.ExpectArgumentNullException(
                delegate {
                    vf.RenderView(null);
                },
                "viewContext");
        }

        [TestMethod]
        public void RenderViewWithPathNotFoundThrows() {
            // Setup
            ViewContext vc = GetViewContext(false);
            Mock<IViewLocator> viewLocatorMock = new Mock<IViewLocator>();
            viewLocatorMock.Expect(o => o.GetViewLocation(vc, "view")).Returns(String.Empty);
            WebFormViewEngine vf = new WebFormViewEngine();
            vf.ViewLocator = viewLocatorMock.Object;

            // Execute
            ExceptionHelper.ExpectException<InvalidOperationException>(
                delegate {
                    ((IViewEngine)vf).RenderView(vc);
                },
                "The view 'view' could not be found.");
        }

        [TestMethod]
        public void RenderViewWithNullViewInstanceThrows() {
            // Setup
            ViewContext vc = GetViewContext(false);
            Mock<IViewLocator> viewLocatorMock = new Mock<IViewLocator>();
            viewLocatorMock.Expect(o => o.GetViewLocation(vc, "view")).Returns("view path");

            MockBuildManager buildManagerMock = new MockBuildManager("view path", typeof(object), null);

            WebFormViewEngine vf = new WebFormViewEngine();
            vf.ViewLocator = viewLocatorMock.Object;
            vf.BuildManager = buildManagerMock;

            // Execute
            ExceptionHelper.ExpectException<InvalidOperationException>(
                delegate {
                    ((IViewEngine)vf).RenderView(vc);
                },
                "The view found at 'view path' could not be created.");
        }

        [TestMethod]
        public void RenderViewWithViewPageRendersView() {
            // Setup
            ViewContext vc = GetViewContext(false);
            Mock<IViewLocator> viewLocatorMock = new Mock<IViewLocator>();
            viewLocatorMock.Expect(o => o.GetViewLocation(vc, "view")).Returns("view path");

            ViewPageToRender viewPage = new ViewPageToRender();

            MockBuildManager buildManagerMock = new MockBuildManager("view path", typeof(object), viewPage);

            WebFormViewEngine vf = new WebFormViewEngine();
            vf.ViewLocator = viewLocatorMock.Object;
            vf.BuildManager = buildManagerMock;

            // Execute
            ((IViewEngine)vf).RenderView(vc);

            // Verify
            Assert.AreEqual<ViewContext>(vc, viewPage.ResultViewContext);
            Assert.AreEqual<string>(String.Empty, viewPage.MasterLocation);
        }

        [TestMethod]
        public void RenderViewWithViewPageAndMasterRendersView() {
            // Setup
            ViewContext vc = GetViewContext(true);
            Mock<IViewLocator> viewLocatorMock = new Mock<IViewLocator>();
            viewLocatorMock.Expect(o => o.GetViewLocation(vc, "view")).Returns("view path");
            viewLocatorMock.Expect(o => o.GetMasterLocation(vc, "master")).Returns("master path");

            ViewPageToRender viewPage = new ViewPageToRender();

            MockBuildManager buildManagerMock = new MockBuildManager("view path", typeof(object), viewPage);

            WebFormViewEngine vf = new WebFormViewEngine();
            vf.ViewLocator = viewLocatorMock.Object;
            vf.BuildManager = buildManagerMock;

            // Execute
            ((IViewEngine)vf).RenderView(vc);

            // Verify
            Assert.AreEqual<ViewContext>(vc, viewPage.ResultViewContext);
            Assert.AreEqual<string>("master path", viewPage.MasterLocation);
        }

        [TestMethod]
        public void RenderViewWithViewPageAndMasterNotFoundThrows() {
            // Setup
            ViewContext vc = GetViewContext(true);
            Mock<IViewLocator> viewLocatorMock = new Mock<IViewLocator>();
            viewLocatorMock.Expect(o => o.GetViewLocation(vc, "view")).Returns("view path");
            viewLocatorMock.Expect(o => o.GetMasterLocation(vc, "master")).Returns(String.Empty);

            MockBuildManager buildManagerMock = new MockBuildManager("view path", typeof(object), new DummyViewPage());

            WebFormViewEngine vf = new WebFormViewEngine();
            vf.ViewLocator = viewLocatorMock.Object;
            vf.BuildManager = buildManagerMock;

            // Execute
            ExceptionHelper.ExpectException<InvalidOperationException>(
                delegate {
                    ((IViewEngine)vf).RenderView(vc);
                },
                "The master 'master' could not be found.");
        }

        [TestMethod]
        public void RenderViewWithViewUserControlRendersView() {
            // Setup
            ViewContext vc = GetViewContext(false);
            Mock<IViewLocator> viewLocatorMock = new Mock<IViewLocator>();
            viewLocatorMock.Expect(o => o.GetViewLocation(vc, "view")).Returns("view path");

            ViewUserControlToRender viewUserControl = new ViewUserControlToRender();

            MockBuildManager buildManagerMock = new MockBuildManager("view path", typeof(object), viewUserControl);

            WebFormViewEngine vf = new WebFormViewEngine();
            vf.ViewLocator = viewLocatorMock.Object;
            vf.BuildManager = buildManagerMock;

            // Execute
            ((IViewEngine)vf).RenderView(vc);

            // Verify
            Assert.AreEqual<ViewContext>(vc, viewUserControl.ResultViewContext);
        }

        [TestMethod]
        public void RenderViewWithViewUserControlAndMasterThrows() {
            // Setup
            ViewContext vc = GetViewContext(true);
            Mock<IViewLocator> viewLocatorMock = new Mock<IViewLocator>();
            viewLocatorMock.Expect(o => o.GetViewLocation(vc, "view")).Returns("view path");

            MockBuildManager buildManagerMock = new MockBuildManager("view path", typeof(object), new DummyViewUserControl());

            WebFormViewEngine vf = new WebFormViewEngine();
            vf.ViewLocator = viewLocatorMock.Object;
            vf.BuildManager = buildManagerMock;

            // Execute
            ExceptionHelper.ExpectException<InvalidOperationException>(
                delegate {
                    ((IViewEngine)vf).RenderView(vc);
                },
                "A master name cannot be specified when the view is a ViewUserControl.");
        }

        [TestMethod]
        public void RenderViewWithUnknownTypeThrows() {
            // Setup
            ViewContext vc = GetViewContext(false);
            Mock<IViewLocator> viewLocatorMock = new Mock<IViewLocator>();
            viewLocatorMock.Expect(o => o.GetViewLocation(vc, "view")).Returns("view path");

            MockBuildManager buildManagerMock = new MockBuildManager("view path", typeof(object), 12345);

            WebFormViewEngine vf = new WebFormViewEngine();
            vf.ViewLocator = viewLocatorMock.Object;
            vf.BuildManager = buildManagerMock;

            // Execute
            ExceptionHelper.ExpectException<InvalidOperationException>(
                delegate {
                    ((IViewEngine)vf).RenderView(vc);
                },
                "The view at 'view path' must derive from ViewPage, ViewPage<TViewData>, ViewUserControl, or ViewUserControl<TViewData>.");
        }

        private static ViewContext GetViewContext(bool useMaster) {
            Mock<HttpContextBase> contextMock = new Mock<HttpContextBase>();
            return new ViewContext(contextMock.Object, new RouteData(), new Mock<IController>().Object, "view", useMaster ? "master" : String.Empty, new ViewDataDictionary(), null);
        }

        public sealed class DummyViewPage : ViewPage {
        }

        public sealed class DummyViewUserControl : ViewUserControl {
        }

        public sealed class ViewPageToRender : ViewPage {
            public ViewContext ResultViewContext;

            public override void RenderView(ViewContext viewContext) {
                ResultViewContext = viewContext;
            }
        }

        public sealed class ViewUserControlToRender : ViewUserControl {
            public ViewContext ResultViewContext;

            public override void RenderView(ViewContext viewContext) {
                ResultViewContext = viewContext;
            }
        }
    }
}
