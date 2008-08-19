using System;
using System.Web;
using System.Web.UI;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Microsoft.Web.Test {
    [TestClass]
    public class GeneratedImageTest {
        private TestContext testContextInstance;
        private TestGeneratedImage gi;

        public TestContext TestContext {
            get {
                return testContextInstance;
            }
            set {
                testContextInstance = value;
            }
        }

        [TestInitialize]
        public void SetUp() {
            var requestMock = new Mock<HttpRequestBase>();
            requestMock.ExpectGet(r => r.Path).Returns("basePath");
            
            var contextMock = new Mock<HttpContextBase>();
            contextMock.ExpectGet(c => c.Request).Returns(requestMock.Object);

            gi = new TestGeneratedImage(contextMock.Object) {
                ImageHandlerUrl = "Handler.ashx"
            };
        }

        [TestCleanup]
        public void TearDown() {
            gi = null;
        }

        [TestMethod]
        public void GenerateURL() {
            gi.CallOnPreRender();
            Assert.AreEqual("Handler.ashx", gi.ImageUrl);
        }

        [TestMethod]
        public void GenerateURL_WithParameters() {
            gi.Parameters.Add(new ImageParameter() { Name = "foo bar", Value = "foo?" });
            gi.Parameters.Add(new ImageParameter() { Name = "baz", Value = "123" });

            gi.CallOnPreRender();
            Assert.AreEqual("Handler.ashx?foo+bar=foo%3f&baz=123", gi.ImageUrl);
        }

        [TestMethod]
        public void GenerateURL_WithTimestamp() {
            gi.Timestamp = "DummyTimestamp??";
            gi.CallOnPreRender();
            Assert.AreEqual("Handler.ashx?__timestamp=DummyTimestamp%3f%3f", gi.ImageUrl);
        }

        [TestMethod]
        public void GenerateURL_WithTimestampAndParameters() {
            gi.Timestamp = "DummyTimestamp??";
            gi.Parameters.Add(new ImageParameter() { Name = "foo bar", Value = "foo?" });
            gi.Parameters.Add(new ImageParameter() { Name = "baz", Value = "123" });

            gi.CallOnPreRender();
            Assert.AreEqual("Handler.ashx?foo+bar=foo%3f&baz=123&__timestamp=DummyTimestamp%3f%3f", gi.ImageUrl);
        }

        [TestMethod]
        public void OnDataBinding() {
            var param = new ImageParameter() { Name = "name" , Value="value"};
            
            bool dataBound = false;
            param.DataBinding += new EventHandler(delegate(object sender, EventArgs e) {
                var imageParam = sender as ImageParameter;
                Assert.AreEqual("DummyControl", imageParam.BindingContainer.ID);
                dataBound = true;
            });
            gi.Parameters.Add(param);

            gi.CallOnDataBinding();

            Assert.AreEqual(true, dataBound);
        }

        private class TestGeneratedImage : Microsoft.Web.GeneratedImage {
            public TestGeneratedImage(HttpContextBase context)
                : base(context, new Control() { ID = "DummyControl" }) {
            }

            public void CallOnPreRender() {
                base.OnPreRender(EventArgs.Empty);
            }

            public void CallOnDataBinding() {
                base.OnDataBinding(EventArgs.Empty);
            }
        }
    }
}
