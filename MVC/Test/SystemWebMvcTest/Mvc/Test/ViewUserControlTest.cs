namespace System.Web.Mvc.Test {
    using System.Web.Routing;
    using System.Web.TestUtil;
    using System.Web.UI;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;

    [TestClass]
    public class ViewUserControlTest {

        [TestMethod]
        public void SetViewItem() {
            // Setup
            ViewUserControl vuc = new ViewUserControl();
            object viewItem = new object();
            vuc.ViewData = new ViewDataDictionary(viewItem);

            // Execute
            vuc.ViewData.Model = viewItem;
            object newViewItem = vuc.ViewData.Model;

            // Verify
            Assert.AreSame(viewItem, newViewItem);
        }

        [TestMethod]
        public void SetViewItemOnBaseClassPropagatesToDerivedClass() {
            // Setup
            ViewUserControl<object> vucInt = new ViewUserControl<object>();
            ViewUserControl vuc = vucInt;
            vuc.ViewData = new ViewDataDictionary();
            object o = new object();

            // Execute
            vuc.ViewData.Model = o;

            // Verify
            Assert.AreEqual(o, vucInt.ViewData.Model);
            Assert.AreEqual(o, vuc.ViewData.Model);
        }

        [TestMethod]
        public void SetViewItemOnDerivedClassPropagatesToBaseClass() {
            // Setup
            ViewUserControl<object> vucInt = new ViewUserControl<object>();
            ViewUserControl vuc = vucInt;
            vucInt.ViewData = new ViewDataDictionary<object>();
            object o = new object();

            // Execute
            vucInt.ViewData.Model = o;

            // Verify
            Assert.AreEqual(o, vucInt.ViewData.Model);
            Assert.AreEqual(o, vuc.ViewData.Model);
        }

        [TestMethod]
        public void SetViewItemToWrongTypeThrows() {
            // Setup
            ViewUserControl<string> vucString = new ViewUserControl<string>();
            vucString.ViewData = new ViewDataDictionary<string>();
            ViewUserControl vuc = vucString;

            // Execute & verify
            ExceptionHelper.ExpectException<InvalidOperationException>(
                delegate {
                    vuc.ViewData.Model = 50;
                },
                "The model item passed into the dictionary is of type 'System.Int32' but this dictionary requires a model item of type 'System.String'.");
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
            p.ViewData = new ViewDataDictionary { { "FirstName", "Joe" }, { "LastName", "Schmoe" } };

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
            ViewUserControl vuc = new ViewUserControl() { SubDataKey = "SubData" };
            p.Controls[0].Controls.Add(vuc);
            p.ViewData = new ViewDataDictionary { { "FirstName", "Joe" }, { "LastName", "Schmoe" } };
            p.ViewData.SubDataItems["SubData"] = new ViewDataDictionary { { "FirstName", "SubJoe" }, { "LastName", "SubSchmoe" } };

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

            p.ViewData = new ViewDataDictionary { { "FirstName", "Joe" }, { "LastName", "Schmoe" } };

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
            ViewUserControl vuc = new ViewUserControl() { SubDataKey = "SubData" };
            outerVuc.Controls[0].Controls.Add(vuc);

            p.ViewData = new ViewDataDictionary { { "FirstName", "Joe" }, { "LastName", "Schmoe" } };
            p.ViewData.SubDataItems["SubData"] = new ViewDataDictionary { { "FirstName", "SubJoe" }, { "LastName", "SubSchmoe" } };

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
            ViewUserControl outerVuc = new ViewUserControl() { SubDataKey = "SubData" };
            p.Controls[0].Controls.Add(outerVuc);
            outerVuc.Controls.Add(new Control());
            ViewUserControl vuc = new ViewUserControl();
            outerVuc.Controls[0].Controls.Add(vuc);

            p.ViewData = new ViewDataDictionary { { "FirstName", "Joe" }, { "LastName", "Schmoe" } };
            p.ViewData.SubDataItems["SubData"] = new ViewDataDictionary { { "FirstName", "SubJoe" }, { "LastName", "SubSchmoe" } };

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
        public void GetWrongGenericViewItemTypeThrows() {
            // Setup
            ViewPage p = new ViewPage();
            p.ViewData = new ViewDataDictionary();
            p.ViewData["Foo"] = new DummyViewData { MyInt = 123, MyString = "Whatever" };

            MockViewUserControl<MyViewData> vuc = new MockViewUserControl<MyViewData>() { ViewDataKey = "FOO" };
            vuc.AppRelativeVirtualPath = "~/Foo.aspx";
            p.Controls.Add(new Control());
            p.Controls[0].Controls.Add(vuc);

            // Execute
            ExceptionHelper.ExpectException<InvalidOperationException>(
                delegate {
                    var foo = vuc.ViewData.Model.IntProp;
                },
                @"The model item passed into the dictionary is of type 'System.Web.Mvc.Test.ViewUserControlTest+DummyViewData' but this dictionary requires a model item of type 'System.Web.Mvc.Test.ViewUserControlTest+MyViewData'.");
        }

        [TestMethod]
        public void GetGenericViewItemType() {
            // Setup
            ViewPage p = new ViewPage();
            p.Controls.Add(new Control());
            MockViewUserControl<MyViewData> vuc = new MockViewUserControl<MyViewData>() { ViewDataKey = "FOO" };
            p.Controls[0].Controls.Add(vuc);
            p.ViewData = new ViewDataDictionary();
            p.ViewData["Foo"] = new MyViewData { IntProp = 123, StringProp = "miao" };

            // Execute
            int intProp = vuc.ViewData.Model.IntProp;
            string stringProp = vuc.ViewData.Model.StringProp;

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
                                                        new ViewDataDictionary(),
                                                        new TempDataDictionary(ControllerContextTest.GetEmptyContextForTempData()));
            vuc.ViewContext = vc;

            // Execute
            HtmlHelper htmlHelper = vuc.Html;

            // Verify
            Assert.AreEqual(vc, htmlHelper.ViewContext);
            Assert.AreEqual(vuc, htmlHelper.ViewDataContainer);
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
                 "A ViewUserControl can only be used inside pages that derive from ViewPage or ViewPage<TViewItem>.");
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
                                                        new ViewDataDictionary(),
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
                 "A ViewUserControl can only be used inside pages that derive from ViewPage or ViewPage<TViewItem>.");
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
                 "A ViewUserControl can only be used inside pages that derive from ViewPage or ViewPage<TViewItem>.");
        }

        private sealed class DummyViewData {
            public int MyInt { get; set; }
            public string MyString { get; set; }
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

        private sealed class MockViewUserControl<TViewData> : ViewUserControl<TViewData> where TViewData : class {
        }

        private sealed class MyViewData {
            public int IntProp { get; set; }
            public string StringProp { get; set; }
        }
    }
}
