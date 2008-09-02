namespace System.Web.Mvc.Test {
    using System.Web.Mvc;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class WebFormViewEngineTest {

        [TestMethod]
        public void CreatePartialViewCreatesWebFormView() {
            // Arrange
            TestableWebFormViewEngine engine = new TestableWebFormViewEngine();

            // Act
            WebFormView result = (WebFormView)engine.CreatePartialView("partial path");

            // Assert
            Assert.AreEqual("partial path", result.ViewPath);
            Assert.AreEqual(String.Empty, result.MasterPath);
        }

        [TestMethod]
        public void CreateViewCreatesWebFormView() {
            // Arrange
            TestableWebFormViewEngine engine = new TestableWebFormViewEngine();

            // Act
            WebFormView result = (WebFormView)engine.CreateView("view path", "master path");

            // Assert
            Assert.AreEqual("view path", result.ViewPath);
            Assert.AreEqual("master path", result.MasterPath);
        }

        [TestMethod]
        public void MasterLocationFormatsProperty() {
            // Act
            TestableWebFormViewEngine engine = new TestableWebFormViewEngine();

            // Assert
            Assert.AreEqual(2, engine.MasterLocationFormats.Length);
            Assert.AreEqual("~/Views/{1}/{0}.master", engine.MasterLocationFormats[0]);
            Assert.AreEqual("~/Views/Shared/{0}.master", engine.MasterLocationFormats[1]);
        }

        [TestMethod]
        public void PartialViewLocationFormatsProperty() {
            // Act
            TestableWebFormViewEngine engine = new TestableWebFormViewEngine();

            // Assert
            Assert.AreEqual(4, engine.PartialViewLocationFormats.Length);
            Assert.AreEqual("~/Views/{1}/{0}.aspx", engine.PartialViewLocationFormats[0]);
            Assert.AreEqual("~/Views/{1}/{0}.ascx", engine.PartialViewLocationFormats[1]);
            Assert.AreEqual("~/Views/Shared/{0}.aspx", engine.PartialViewLocationFormats[2]);
            Assert.AreEqual("~/Views/Shared/{0}.ascx", engine.PartialViewLocationFormats[3]);
        }

        [TestMethod]
        public void ViewLocationFormatsProperty() {
            // Act
            TestableWebFormViewEngine engine = new TestableWebFormViewEngine();

            // Assert
            Assert.AreEqual(4, engine.ViewLocationFormats.Length);
            Assert.AreEqual("~/Views/{1}/{0}.aspx", engine.ViewLocationFormats[0]);
            Assert.AreEqual("~/Views/{1}/{0}.ascx", engine.ViewLocationFormats[1]);
            Assert.AreEqual("~/Views/Shared/{0}.aspx", engine.ViewLocationFormats[2]);
            Assert.AreEqual("~/Views/Shared/{0}.ascx", engine.ViewLocationFormats[3]);
        }

        class TestableWebFormViewEngine : WebFormViewEngine {
            public new string[] MasterLocationFormats {
                get { return base.MasterLocationFormats; }
            }

            public new string[] PartialViewLocationFormats {
                get { return base.PartialViewLocationFormats; }
            }

            public new string[] ViewLocationFormats {
                get { return base.ViewLocationFormats; }
            }

            public IView CreatePartialView(string partialPath) {
                return base.CreatePartialView(null, partialPath);
            }

            public IView CreateView(string viewPath, string masterPath) {
                return base.CreateView(null, viewPath, masterPath);
            }
        }
    }
}
