namespace System.Web.Mvc.Test {
    using System;
    using System.Collections.Generic;
    using System.Web.TestUtil;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class WebFormViewLocatorTest {
        [TestMethod]
        public void GetMasterLocationWithLocationsAndSpecificPathExistsReturnsSpecificPath() {
            // Setup
            MockVirtualPathProvider vppMock = new MockVirtualPathProvider(new List<KeyValuePair<string, bool>> {
                new KeyValuePair<string, bool>("~/master.specific", true)
            });

            WebFormViewLocator vl = new WebFormViewLocator();
            vl.VirtualPathProvider = vppMock;

            // Execute
            string path = ((IViewLocator)vl).GetMasterLocation(ViewLocatorTest.GetRequestContext(), "~/master.specific");

            // Verify
            Assert.AreEqual<string>("~/master.specific", path);
            vppMock.Verify();
        }

        [TestMethod]
        public void GetMasterLocationWithLocationsAndSpecificPathNotExistsThrows() {
            // Setup
            MockVirtualPathProvider vppMock = new MockVirtualPathProvider(new List<KeyValuePair<string, bool>> {
                new KeyValuePair<string, bool>("~/master.specific", false)
            });

            WebFormViewLocator vl = new WebFormViewLocator();
            vl.VirtualPathProvider = vppMock;

            // Execute
            ExceptionHelper.ExpectException<InvalidOperationException>(
                delegate {
                    ((IViewLocator)vl).GetMasterLocation(ViewLocatorTest.GetRequestContext(), "~/master.specific");
                },
                "The master '~/master.specific' could not be located at these paths: ~/master.specific");

            // Verify
            vppMock.Verify();
        }

        [TestMethod]
        public void GetMasterLocationWithLocationsAndMappedPathExistsReturnsSpecificPath() {
            // Setup
            MockVirtualPathProvider vppMock = new MockVirtualPathProvider(new List<KeyValuePair<string, bool>> {
                new KeyValuePair<string, bool>("~/Views/MyController/master.master", true)
            });

            WebFormViewLocator vl = new WebFormViewLocator();
            vl.VirtualPathProvider = vppMock;

            // Execute
            string path = ((IViewLocator)vl).GetMasterLocation(ViewLocatorTest.GetRequestContext(), "master");

            // Verify
            Assert.AreEqual<string>("~/Views/MyController/master.master", path);
            vppMock.Verify();
        }

        [TestMethod]
        public void GetMasterLocationWithLocationsAndMappedPathNotExistsThrows() {
            // Setup
            MockVirtualPathProvider vppMock = new MockVirtualPathProvider(new List<KeyValuePair<string, bool>> {
                new KeyValuePair<string, bool>("~/Views/MyController/master.master", false),
                new KeyValuePair<string, bool>("~/Views/Shared/master.master", false)
            });
            WebFormViewLocator vl = new WebFormViewLocator();
            vl.VirtualPathProvider = vppMock;

            // Execute
            ExceptionHelper.ExpectException<InvalidOperationException>(
                delegate {
                    string path = ((IViewLocator)vl).GetMasterLocation(ViewLocatorTest.GetRequestContext(), "master");
                },
                "The master 'master' could not be located at these paths: ~/Views/MyController/master.master, ~/Views/Shared/master.master");

            // Verify
            vppMock.Verify();
        }

        [TestMethod]
        public void GetViewLocationWithLocationsAndSpecificPathExistsReturnsSpecificPath() {
            // Setup
            MockVirtualPathProvider vppMock = new MockVirtualPathProvider(new List<KeyValuePair<string, bool>> {
                new KeyValuePair<string, bool>("~/view.specific", true)
            });

            WebFormViewLocator vl = new WebFormViewLocator();
            vl.VirtualPathProvider = vppMock;

            // Execute
            string path = ((IViewLocator)vl).GetViewLocation(ViewLocatorTest.GetRequestContext(), "~/view.specific");

            // Verify
            Assert.AreEqual<string>("~/view.specific", path);
            vppMock.Verify();
        }

        [TestMethod]
        public void GetViewLocationWithLocationsAndSpecificPathNotExistsThrows() {
            // Setup
            MockVirtualPathProvider vppMock = new MockVirtualPathProvider(new List<KeyValuePair<string, bool>> {
                new KeyValuePair<string, bool>("~/view.specific", false)
            });

            WebFormViewLocator vl = new WebFormViewLocator();
            vl.VirtualPathProvider = vppMock;

            // Execute
            ExceptionHelper.ExpectException<InvalidOperationException>(
                delegate {
                    ((IViewLocator)vl).GetViewLocation(ViewLocatorTest.GetRequestContext(), "~/view.specific");
                },
                "The view '~/view.specific' could not be located at these paths: ~/view.specific");

            // Verify
            vppMock.Verify();
        }

        [TestMethod]
        public void GetViewLocationWithLocationsAndMappedPathExistsReturnsSpecificPath() {
            // Setup
            MockVirtualPathProvider vppMock = new MockVirtualPathProvider(new List<KeyValuePair<string, bool>> {
                new KeyValuePair<string, bool>("~/Views/MyController/view.aspx", true)
            });

            WebFormViewLocator vl = new WebFormViewLocator();
            vl.VirtualPathProvider = vppMock;

            // Execute
            string path = ((IViewLocator)vl).GetViewLocation(ViewLocatorTest.GetRequestContext(), "view");

            // Verify
            Assert.AreEqual<string>("~/Views/MyController/view.aspx", path);
            vppMock.Verify();
        }

        [TestMethod]
        public void GetViewLocationWithLocationsAndMappedPathNotExistsThrows() {
            // Setup
            MockVirtualPathProvider vppMock = new MockVirtualPathProvider(new List<KeyValuePair<string, bool>> {
                new KeyValuePair<string, bool>("~/Views/MyController/view.aspx", false),
                new KeyValuePair<string, bool>("~/Views/MyController/view.ascx", false),
                new KeyValuePair<string, bool>("~/Views/Shared/view.aspx", false),
                new KeyValuePair<string, bool>("~/Views/Shared/view.ascx", false)
            });
            WebFormViewLocator vl = new WebFormViewLocator();
            vl.VirtualPathProvider = vppMock;

            // Execute
            ExceptionHelper.ExpectException<InvalidOperationException>(
                delegate {
                    string path = ((IViewLocator)vl).GetViewLocation(ViewLocatorTest.GetRequestContext(), "view");
                },
                "The view 'view' could not be located at these paths: ~/Views/MyController/view.aspx, ~/Views/MyController/view.ascx, ~/Views/Shared/view.aspx, ~/Views/Shared/view.ascx");

            // Verify
            vppMock.Verify();
        }
    }
}
