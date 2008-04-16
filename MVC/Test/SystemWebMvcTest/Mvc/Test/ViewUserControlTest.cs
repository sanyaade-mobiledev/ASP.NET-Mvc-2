namespace System.Web.Mvc.Test {
    using System.Web.Routing;
    using System.Web.TestUtil;
    using System.Web.UI;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;

    [TestClass]
    public class ViewUserControlTest {
        [TestMethod]
        public void SetViewData() {
            ViewUserControl vp = new ViewUserControl();
            vp.SetViewData(new { a = "123", b = "456" });

            Assert.AreEqual("123", vp.ViewData["a"]);
            Assert.AreEqual("456", vp.ViewData["b"]);
        }

        [TestMethod]
        public void SetViewDataGeneric() {
            MockViewUserControlDummyViewData vp = new MockViewUserControlDummyViewData();
            vp.SetViewData(new MyViewData { IntProp = 123, StringProp = "abc" });

            Assert.AreEqual(123, vp.ViewData.IntProp);
            Assert.AreEqual("abc", vp.ViewData.StringProp);
        }

        [TestMethod]
        public void SetNullViewDataDoesNothing() {
            MockViewUserControlDummyViewData vp = new MockViewUserControlDummyViewData();
            vp.SetViewData(null);
        }

        [TestMethod]
        public void SetViewDataWrongGenericTypeThrows() {
            MockViewUserControlBogusViewData vp = new MockViewUserControlBogusViewData();

            ExceptionHelper.ExpectArgumentException(
                delegate {
                    vp.SetViewData(new DummyViewData { MyInt = 123, MyString = "abc" });
                },
                "The view data passed into the page is of type 'System.Web.Mvc.Test.ViewUserControlTest+DummyViewData' but this page requires view data of type 'System.Int32'.\r\nParameter name: viewData");
        }

        [TestMethod]
        public void GetViewDataWhenNoPageSetThrows() {
            ViewUserControl vuc = new ViewUserControl();
            vuc.AppRelativeVirtualPath = "~/Foo.ascx";

            ExceptionHelper.ExpectException<InvalidOperationException>(
                delegate {
                    var foo = vuc.ViewData["Foo"];
                },
                "The ViewUserControl '~/Foo.ascx' cannot find an IViewDataContainer. The ViewUserControl must be inside a ViewPage, ViewMasterPage, or another ViewUserControl.");
        }

        [TestMethod]
        public void GetViewDataWhenRegularPageSetThrows() {
            Page p = new Page();
            p.Controls.Add(new Control());
            ViewUserControl vuc = new ViewUserControl();
            p.Controls[0].Controls.Add(vuc);
            vuc.AppRelativeVirtualPath = "~/Foo.ascx";

            ExceptionHelper.ExpectException<InvalidOperationException>(
                delegate {
                    var foo = vuc.ViewData["Foo"];
                },
                "The ViewUserControl '~/Foo.ascx' cannot find an IViewDataContainer. The ViewUserControl must be inside a ViewPage, ViewMasterPage, or another ViewUserControl.");
        }

        [TestMethod]
        public void GetViewDataFromViewPage() {
            // Setup
            ViewPage p = new ViewPage();
            p.Controls.Add(new Control());
            ViewUserControl vuc = new ViewUserControl();
            p.Controls[0].Controls.Add(vuc);
            p.SetViewData(new { FirstName = "Joe", LastName = "Schmoe" });

            // Execute
            object firstName = vuc.ViewData["FirstName"];
            object lastName = vuc.ViewData["LastName"];

            // Verify
            Assert.AreEqual("Joe", firstName);
            Assert.AreEqual("Schmoe", lastName);
        }

        [TestMethod]
        public void GetViewDataFromViewPageWithViewDataKey() {
            // Setup
            ViewPage p = new ViewPage();
            p.Controls.Add(new Control());
            ViewUserControl vuc = new ViewUserControl() { ViewDataKey = "SubData" };
            p.Controls[0].Controls.Add(vuc);
            p.SetViewData(new {
                FirstName = "Joe",
                LastName = "Schmoe",
                SubData = new {
                    FirstName = "SubJoe",
                    LastName = "SubSchmoe"
                }
            });

            // Execute
            object firstName = vuc.ViewData["FirstName"];
            object lastName = vuc.ViewData["LastName"];

            // Verify
            Assert.AreEqual("SubJoe", firstName);
            Assert.AreEqual("SubSchmoe", lastName);
        }

        [TestMethod]
        public void GetViewDataFromViewUserControl() {
            // Setup
            ViewPage p = new ViewPage();
            p.Controls.Add(new Control());
            ViewUserControl outerVuc = new ViewUserControl();
            p.Controls[0].Controls.Add(outerVuc);
            outerVuc.Controls.Add(new Control());
            ViewUserControl vuc = new ViewUserControl();
            outerVuc.Controls[0].Controls.Add(vuc);

            p.SetViewData(new { FirstName = "Joe", LastName = "Schmoe" });

            // Execute
            object firstName = vuc.ViewData["FirstName"];
            object lastName = vuc.ViewData["LastName"];

            // Verify
            Assert.AreEqual("Joe", firstName);
            Assert.AreEqual("Schmoe", lastName);
        }

        [TestMethod]
        public void GetViewDataFromViewUserControlWithViewDataKeyOnInnerControl() {
            // Setup
            ViewPage p = new ViewPage();
            p.Controls.Add(new Control());
            ViewUserControl outerVuc = new ViewUserControl();
            p.Controls[0].Controls.Add(outerVuc);
            outerVuc.Controls.Add(new Control());
            ViewUserControl vuc = new ViewUserControl() { ViewDataKey = "SubData" };
            outerVuc.Controls[0].Controls.Add(vuc);

            p.SetViewData(new {
                FirstName = "Joe",
                LastName = "Schmoe",
                SubData = new {
                    FirstName = "SubJoe",
                    LastName = "SubSchmoe"
                }
            });

            // Execute
            object firstName = vuc.ViewData["FirstName"];
            object lastName = vuc.ViewData["LastName"];

            // Verify
            Assert.AreEqual("SubJoe", firstName);
            Assert.AreEqual("SubSchmoe", lastName);
        }

        [TestMethod]
        public void GetViewDataFromViewUserControlWithViewDataKeyOnOuterControl() {
            // Setup
            ViewPage p = new ViewPage();
            p.Controls.Add(new Control());
            ViewUserControl outerVuc = new ViewUserControl() { ViewDataKey = "SubData" };
            p.Controls[0].Controls.Add(outerVuc);
            outerVuc.Controls.Add(new Control());
            ViewUserControl vuc = new ViewUserControl();
            outerVuc.Controls[0].Controls.Add(vuc);

            p.SetViewData(new {
                FirstName = "Joe",
                LastName = "Schmoe",
                SubData = new {
                    FirstName = "SubJoe",
                    LastName = "SubSchmoe"
                }
            });

            // Execute
            object firstName = vuc.ViewData["FirstName"];
            object lastName = vuc.ViewData["LastName"];

            // Verify
            Assert.AreEqual("SubJoe", firstName);
            Assert.AreEqual("SubSchmoe", lastName);
        }

        [TestMethod]
        public void ViewDataKeyProperty() {
            MemberHelper.TestStringProperty(new ViewUserControl(), "ViewDataKey", String.Empty, true);
        }

        [TestMethod]
        public void GetWrongGenericViewDataTypeThrows() {
            // Setup
            ViewPage p = new ViewPage();
            p.Controls.Add(new Control());
            MockViewUserControl<MyViewData> vuc = new MockViewUserControl<MyViewData>();
            p.Controls[0].Controls.Add(vuc);
            p.SetViewData(new { FirstName = "Joe", LastName = "Schmoe" });
            vuc.AppRelativeVirtualPath = "~/Foo.aspx";

            // Execute
            ExceptionHelper.ExpectException<InvalidOperationException>(
                delegate {
                    var foo = vuc.ViewData.IntProp;
                },
                "The view data for ViewUserControl '~/Foo.aspx' could not be found or is not of the type 'System.Web.Mvc.Test.ViewUserControlTest+MyViewData'.");
        }

        [TestMethod]
        public void GetGenericViewDataType() {
            // Setup
            ViewPage p = new ViewPage();
            p.Controls.Add(new Control());
            MockViewUserControl<MyViewData> vuc = new MockViewUserControl<MyViewData>();
            p.Controls[0].Controls.Add(vuc);
            p.SetViewData(new MyViewData { IntProp = 123, StringProp = "miao" });

            // Execute
            int intProp = vuc.ViewData.IntProp;
            string stringProp = vuc.ViewData.StringProp;

            // Verify
            Assert.AreEqual<int>(123, intProp);
            Assert.AreEqual<string>("miao", stringProp);
        }

        [TestMethod]
        public void GetHtmlHelperFromViewPage() {
            // Setup
            ViewUserControl vuc = new ViewUserControl();
            ViewPage containerPage = new ViewPage();
            containerPage.Controls.Add(vuc);
            ViewContext vc = new ViewContext(new Mock<HttpContextBase>().Object,
                                                        new RouteData(),
                                                        new Mock<IController>().Object,
                                                        "view",
                                                        null,
                                                        null,
                                                        new TempDataDictionary(ControllerContextTest.GetEmptyContextForTempData()));
            HtmlHelper htmlHelper = new HtmlHelper(vc);
            containerPage.Html = htmlHelper;

            // Verify
            Assert.AreEqual(vuc.Html, htmlHelper);
        }

        [TestMethod]
        public void GetHtmlHelperFromRegularPage() {
            // Setup
            ViewUserControl vuc = new ViewUserControl();
            Page containerPage = new Page();
            containerPage.Controls.Add(vuc);

            // Verify
            ExceptionHelper.ExpectException<InvalidOperationException>(
                 delegate {
                     HtmlHelper foo = vuc.Html;
                 },
                 "A ViewUserControl can only be used inside pages that derive from ViewPage or ViewPage<TViewData>.");
        }

        [TestMethod]
        public void GetUrlHelperFromViewPage() {
            // Setup
            ViewUserControl vuc = new ViewUserControl();
            ViewPage containerPage = new ViewPage();
            containerPage.Controls.Add(vuc);
            ViewContext vc = new ViewContext(new Mock<HttpContextBase>().Object,
                                                        new RouteData(),
                                                        new Mock<IController>().Object,
                                                        "view",
                                                        null,
                                                        null,
                                                        new TempDataDictionary(ControllerContextTest.GetEmptyContextForTempData()));
            UrlHelper urlHelper = new UrlHelper(vc);
            containerPage.Url = urlHelper;

            // Verify
            Assert.AreEqual(vuc.Url, urlHelper);
        }

        [TestMethod]
        public void GetUrlHelperFromRegularPage() {
            // Setup
            ViewUserControl vuc = new ViewUserControl();
            Page containerPage = new Page();
            containerPage.Controls.Add(vuc);

            // Verify
            ExceptionHelper.ExpectException<InvalidOperationException>(
                 delegate {
                     UrlHelper foo = vuc.Url;
                 },
                 "A ViewUserControl can only be used inside pages that derive from ViewPage or ViewPage<TViewData>.");
        }

        [TestMethod]
        public void GetWriterFromViewPage() {
            // Setup
            MockViewUserControl vuc = new MockViewUserControl();
            MockViewUserControlContainerPage containerPage = new MockViewUserControlContainerPage(vuc);
            bool triggered = false;
            HtmlTextWriter writer = new HtmlTextWriter(System.IO.TextWriter.Null);
            containerPage.RenderCallback = delegate() {
                triggered = true;
                Assert.AreEqual(writer, vuc.Writer);
            };

            // Execute & verify
            Assert.IsNull(vuc.Writer);
            containerPage.RenderControl(writer);
            Assert.IsNull(vuc.Writer);
            Assert.IsTrue(triggered);
        }

        [TestMethod]
        public void GetWriterFromRegularPageThrows() {
            // Setup
            MockViewUserControl vuc = new MockViewUserControl();
            Page containerPage = new Page();
            containerPage.Controls.Add(vuc);

            // Execute
            ExceptionHelper.ExpectException<InvalidOperationException>(
                 delegate {
                     HtmlTextWriter writer = vuc.Writer;
                 },
                 "A ViewUserControl can only be used inside pages that derive from ViewPage or ViewPage<TViewData>.");
        }

        private sealed class DummyViewData {
            public int MyInt { get; set; }
            public string MyString { get; set; }
        }

        private sealed class MockViewPage : ViewPage {
        }

        private sealed class MockViewUserControlContainerPage : ViewPage {

            public Action RenderCallback { get; set; }

            public MockViewUserControlContainerPage(ViewUserControl userControl) {
                Controls.Add(userControl);
            }

            protected override void RenderChildren(HtmlTextWriter writer) {
                if (RenderCallback != null) {
                    RenderCallback();
                }
                base.RenderChildren(writer);
            }

        }

        private sealed class MockViewUserControl : ViewUserControl {
        }

        private sealed class MockViewUserControlBogusViewData : ViewUserControl<int> {
        }

        private sealed class MockViewUserControlDummyViewData : ViewUserControl<MyViewData> {
        }

        private sealed class MockViewUserControl<TViewData> : ViewUserControl<TViewData> {
        }

        private sealed class MyViewData {
            public int IntProp { get; set; }
            public string StringProp { get; set; }
        }
    }
}
