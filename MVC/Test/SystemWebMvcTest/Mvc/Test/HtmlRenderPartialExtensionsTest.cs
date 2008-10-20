namespace System.Web.Mvc.Html.Test {
    using System.IO;
    using System.Web.Mvc;
    using System.Web.Routing;
    using System.Web.TestUtil;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;

    [TestClass]
    public class HtmlRenderPartialExtensionsTest {
        const string _parentViewName = "parent-view";
        const string _partialViewName = "partial-view";

        // RenderPartial(string) tests

        [TestMethod]
        public void RenderPartialWithViewName() {
            // Arrange
            SpyHtmlHelper helper = SpyHtmlHelper.Create();

            // Act
            helper.RenderPartial(_partialViewName);

            // Assert
            Assert.AreSame(helper.ViewData, helper.SpiedViewData);
            Assert.AreEqual(_partialViewName, helper.SpiedPartialViewName);
            Assert.AreSame(ViewEngines.DefaultEngine, helper.SpiedEngine);
        }

        // RenderPartial(string, ViewDataDictionary) tests

        [TestMethod]
        public void RenderPartialWithViewNameAndNullViewDataThrows() {
            // Arrange
            SpyHtmlHelper helper = SpyHtmlHelper.Create();

            // Act & Assert
            ExceptionHelper.ExpectArgumentNullException(
                () => helper.RenderPartial(_partialViewName, (ViewDataDictionary)null),
                "viewData"
            );
        }

        [TestMethod]
        public void RenderPartialWithViewNameAndViewData() {
            // Arrange
            SpyHtmlHelper helper = SpyHtmlHelper.Create();
            ViewDataDictionary viewData = new ViewDataDictionary();

            // Act
            helper.RenderPartial(_partialViewName, viewData);

            // Assert
            Assert.AreSame(viewData, helper.SpiedViewData);
            Assert.AreEqual(_partialViewName, helper.SpiedPartialViewName);
            Assert.AreSame(ViewEngines.DefaultEngine, helper.SpiedEngine);
        }

        // RenderPartial(string, object) tests

        [TestMethod]
        public void RenderPartialWithViewNameAndNullModelThrows() {
            // Arrange
            SpyHtmlHelper helper = SpyHtmlHelper.Create();

            // Act & Assert
            ExceptionHelper.ExpectArgumentNullException(
                () => helper.RenderPartial(_partialViewName, (object)null),
                "model"
            );
        }

        [TestMethod]
        public void RenderPartialWithViewNameAndModel() {
            // Arrange
            object newModel = new object();
            SpyHtmlHelper helper = SpyHtmlHelper.Create();
            helper.ViewData.Model = "dummy model";
            helper.ViewData["SampleKey"] = "SampleValue";

            // Act
            helper.RenderPartial(_partialViewName, newModel);

            // Assert
            Assert.AreEqual(_partialViewName, helper.SpiedPartialViewName);
            Assert.AreNotSame(helper.ViewData, helper.SpiedViewData);
            Assert.IsNull(helper.SpiedViewData["SampleKey"]);
            Assert.AreSame(newModel, helper.SpiedViewData.Model);
            Assert.AreSame(ViewEngines.DefaultEngine, helper.SpiedEngine);
        }

        // RenderPartial(string, object, ViewDataDictionary) tests

        [TestMethod]
        public void RenderPartialWithViewNameAndNullModelAndViewDataThrows() {
            // Arrange
            SpyHtmlHelper helper = SpyHtmlHelper.Create();
            ViewDataDictionary viewData = new ViewDataDictionary();

            // Act & Assert
            ExceptionHelper.ExpectArgumentNullException(
                () => helper.RenderPartial(_partialViewName, null, viewData),
                "model"
            );
        }

        [TestMethod]
        public void RenderPartialWithViewNameAndModelAndNullViewDataThrows() {
            // Arrange
            SpyHtmlHelper helper = SpyHtmlHelper.Create();
            object model = new object();

            // Act & Assert
            ExceptionHelper.ExpectArgumentNullException(
                () => helper.RenderPartial(_partialViewName, model, null),
                "viewData"
            );
        }

        [TestMethod]
        public void RenderPartialWithViewNameAndModelAndViewData() {
            // Arrange
            object model = new object();
            SpyHtmlHelper helper = SpyHtmlHelper.Create();
            ViewDataDictionary viewData = new ViewDataDictionary("dummy model");
            viewData["SampleKey"] = "SampleValue";

            // Act
            helper.RenderPartial(_partialViewName, model, viewData);

            // Assert
            Assert.AreNotSame(viewData, helper.SpiedViewData, "Expected a copy of the model, not the original");
            Assert.AreEqual("SampleValue", helper.SpiedViewData["SampleKey"]);
            Assert.AreSame(model, helper.SpiedViewData.Model);
            Assert.AreEqual(_partialViewName, helper.SpiedPartialViewName);
            Assert.AreSame(ViewEngines.DefaultEngine, helper.SpiedEngine);
        }

        // RenderPartialInternal tests

        [TestMethod]
        public void NullPartialViewNameThrows() {
            // Arrange
            TestableHtmlHelper helper = TestableHtmlHelper.Create();
            ViewDataDictionary viewData = new ViewDataDictionary();

            // Act & Assert
            ExceptionHelper.ExpectArgumentExceptionNullOrEmpty(
                () => helper.RenderPartialInternal(null, viewData),
                "partialViewName");
        }

        [TestMethod]
        public void EmptyPartialViewNameThrows() {
            // Arrange
            TestableHtmlHelper helper = TestableHtmlHelper.Create();
            ViewDataDictionary viewData = new ViewDataDictionary();

            // Act & Assert
            ExceptionHelper.ExpectArgumentExceptionNullOrEmpty(
                () => helper.RenderPartialInternal(String.Empty, viewData),
                "partialViewName");
        }

        [TestMethod]
        public void EngineLookupSuccessCallsRender() {
            // Arrange
            TestableHtmlHelper helper = TestableHtmlHelper.Create();
            ViewDataDictionary viewData = new ViewDataDictionary();
            Mock<IViewEngine> engine = new Mock<IViewEngine>(MockBehavior.Strict);
            Mock<IView> view = new Mock<IView>(MockBehavior.Strict);
            engine
                .Expect(e => e.FindPartialView(It.IsAny<ControllerContext>(), _partialViewName))
                .Returns(new ViewEngineResult(view.Object, engine.Object))
                .Verifiable();
            view
                .Expect(v => v.Render(It.IsAny<ViewContext>(), helper.ViewContext.HttpContext.Response.Output))
                .Callback<ViewContext, TextWriter>(
                    (viewContext, writer) => {
                        Assert.AreSame(helper.ViewContext.View, viewContext.View);
                        Assert.AreSame(viewData, viewContext.ViewData);
                        Assert.AreSame(helper.ViewContext.TempData, viewContext.TempData);
                    })
                .Verifiable();

            // Act
            helper.RenderPartialInternal(_partialViewName, viewData, engine.Object);

            // Assert
            engine.Verify();
            view.Verify();
        }

        [TestMethod]
        public void EngineLookupFailureThrows() {
            // Arrange
            TestableHtmlHelper helper = TestableHtmlHelper.Create();
            ViewDataDictionary viewData = new ViewDataDictionary();
            Mock<IViewEngine> engine = new Mock<IViewEngine>(MockBehavior.Strict);
            engine
                .Expect(e => e.FindPartialView(It.IsAny<ControllerContext>(), _partialViewName))
                .Returns(new ViewEngineResult(new[] { "location1", "location2" }))
                .Verifiable();

            // Act & Assert
            ExceptionHelper.ExpectInvalidOperationException(
                () => helper.RenderPartialInternal(_partialViewName, viewData, engine.Object),
                "The partial view '" + _partialViewName + "' could not be found. The following locations were searched:\r\nlocation1\r\nlocation2");

            engine.Verify();
        }

        private class SpyHtmlHelper : HtmlHelper {
            public IViewEngine SpiedEngine;
            public string SpiedPartialViewName;
            public ViewDataDictionary SpiedViewData;

            public SpyHtmlHelper(ViewContext viewContext, IViewDataContainer viewDataContainer)
                : base(viewContext, viewDataContainer) { }

            public new ViewDataDictionary ViewData {
                get { return base.ViewData; }
            }

            public static SpyHtmlHelper Create() {
                ViewDataDictionary viewData = new ViewDataDictionary();

                ViewContext viewContext = new ViewContext(CreateHttpContext(), new RouteData(),
                    new Mock<ControllerBase>().Object, new Mock<IView>().Object, viewData, new TempDataDictionary());

                Mock<IViewDataContainer> container = new Mock<IViewDataContainer>();
                container.Expect(c => c.ViewData).Returns(viewData);

                return new SpyHtmlHelper(viewContext, container.Object);
            }

            internal override void RenderPartialInternal(string partialViewName, ViewDataDictionary viewData,
                                                         IViewEngine engine) {
                SpiedPartialViewName = partialViewName;
                SpiedViewData = viewData;
                SpiedEngine = engine;
            }
        }

        private class TestableHtmlHelper : HtmlHelper {
            TestableHtmlHelper(ViewContext viewContext, IViewDataContainer viewDataContainer)
                : base(viewContext, viewDataContainer) { }

            public static TestableHtmlHelper Create() {
                ViewDataDictionary viewData = new ViewDataDictionary();

                ViewContext viewContext = new ViewContext(CreateHttpContext(),
                                                          new RouteData(),
                                                          new Mock<ControllerBase>().Object,
                                                          new Mock<IView>().Object,
                                                          viewData,
                                                          new TempDataDictionary());

                Mock<IViewDataContainer> container = new Mock<IViewDataContainer>();
                container.Expect(c => c.ViewData).Returns(viewData);

                return new TestableHtmlHelper(viewContext, container.Object);
            }

            public void RenderPartialInternal(string partialViewName,
                                              ViewDataDictionary viewData,
                                              params IViewEngine[] engines) {
                base.RenderPartialInternal(partialViewName, viewData, new AutoViewEngine(new ViewEngineCollection(engines)));
            }
        }

        private static HttpContextBase CreateHttpContext() {
            TextWriter writer = new Mock<TextWriter>().Object;
            Mock<HttpResponseBase> httpResponse = new Mock<HttpResponseBase>();
            httpResponse.Expect(r => r.Output).Returns(writer);
            Mock<HttpContextBase> result = new Mock<HttpContextBase>();
            result.Expect(c => c.Response).Returns(httpResponse.Object);
            return result.Object;
        }
    }
}