namespace System.Web.Mvc.Test {
    using System;
    using System.Collections.Generic;
    using System.Web.Routing;
    using System.Web.TestUtil;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;

    [TestClass]
    public class ViewLocatorTest {
        [TestMethod]
        public void GetMasterLocationWithNullContextThrows() {
            // Setup
            IViewLocator vl = new ViewLocator();

            // Execute
            ExceptionHelper.ExpectArgumentNullException(
                delegate {
                    vl.GetMasterLocation(null, "master");
                },
                "requestContext");
        }

        [TestMethod]
        public void GetMasterLocationWithNoLocationsThrows() {
            // Setup
            IViewLocator vl = new ViewLocator();

            // Execute
            ExceptionHelper.ExpectArgumentException(
                delegate {
                    vl.GetMasterLocation(GetRequestContext(), "master");
                },
                "The parameter must be set to an array of location format strings, such as \"~/Views/{1}/{0}.aspx\".\r\nParameter name: locationFormats");
        }

        [TestMethod]
        public void GetMasterLocationWithLocationsAndSpecificPathExistsReturnsSpecificPath() {
            // Setup
            MockVirtualPathProvider vppMock = new MockVirtualPathProvider(new List<KeyValuePair<string, bool>> {
                new KeyValuePair<string, bool>("~/master.specific", true)
            });

            ViewLocator vl = new ViewLocator();
            vl.MasterLocationFormats = new[] { "~/test/{1}/{0}/whatever" };
            vl.VirtualPathProvider = vppMock;

            // Execute
            string path = ((IViewLocator)vl).GetMasterLocation(GetRequestContext(), "~/master.specific");

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

            ViewLocator vl = new ViewLocator();
            vl.MasterLocationFormats = new[] { "~/test/{1}/{0}/whatever" };
            vl.VirtualPathProvider = vppMock;

            // Execute
            ExceptionHelper.ExpectException<InvalidOperationException>(
                delegate {
                    ((IViewLocator)vl).GetMasterLocation(GetRequestContext(), "~/master.specific");
                },
                "The master '~/master.specific' could not be located at these paths: ~/master.specific");

            // Verify
            vppMock.Verify();
        }

        [TestMethod]
        public void GetMasterLocationWithLocationsAndMappedPathExistsReturnsSpecificPath() {
            // Setup
            MockVirtualPathProvider vppMock = new MockVirtualPathProvider(new List<KeyValuePair<string, bool>> {
                new KeyValuePair<string, bool>("~/test/MyController/master/whatever", true)
            });

            ViewLocator vl = new ViewLocator();
            vl.MasterLocationFormats = new[] { "~/test/{1}/{0}/whatever" };
            vl.VirtualPathProvider = vppMock;

            // Execute
            string path = ((IViewLocator)vl).GetMasterLocation(GetRequestContext(), "master");

            // Verify
            Assert.AreEqual<string>("~/test/MyController/master/whatever", path);
            vppMock.Verify();
        }

        [TestMethod]
        public void GetMasterLocationWithLocationsAndMappedPathNotExistsThrows() {
            // Setup
            MockVirtualPathProvider vppMock = new MockVirtualPathProvider(new List<KeyValuePair<string, bool>> {
                new KeyValuePair<string, bool>("~/test/MyController/master/whatever", false),
                new KeyValuePair<string, bool>("~/test2/MyController/master/whatever2", false)
            });
            ViewLocator vl = new ViewLocator();
            vl.MasterLocationFormats = new[] { "~/test/{1}/{0}/whatever", "~/test2/{1}/{0}/whatever2" };
            vl.VirtualPathProvider = vppMock;

            // Execute
            ExceptionHelper.ExpectException<InvalidOperationException>(
                delegate {
                    string path = ((IViewLocator)vl).GetMasterLocation(GetRequestContext(), "master");
                },
                "The master 'master' could not be located at these paths: ~/test/MyController/master/whatever, ~/test2/MyController/master/whatever2");

            // Verify
            vppMock.Verify();
        }

        [TestMethod]
        public void GetMasterLocationWithLocationsAndMappedPathExistsReturnsSecondSpecificPath() {
            // Setup
            MockVirtualPathProvider vppMock = new MockVirtualPathProvider(new List<KeyValuePair<string, bool>> {
                new KeyValuePair<string, bool>("~/test/MyController/master/whatever", false),
                new KeyValuePair<string, bool>("~/test2/MyController/master/whatever2", true)
            });
            ViewLocator vl = new ViewLocator();
            vl.MasterLocationFormats = new[] { "~/test/{1}/{0}/whatever", "~/test2/{1}/{0}/whatever2", "~/test3/{1}/{0}/whatever3" };
            vl.VirtualPathProvider = vppMock;

            // Execute
            string path = ((IViewLocator)vl).GetMasterLocation(GetRequestContext(), "master");

            // Verify
            Assert.AreEqual<string>("~/test2/MyController/master/whatever2", path);
            vppMock.Verify();
        }

        [TestMethod]
        public void GetViewLocationWithNullContextThrows() {
            // Setup
            IViewLocator vl = new ViewLocator();

            // Execute
            ExceptionHelper.ExpectArgumentNullException(
                delegate {
                    vl.GetViewLocation(null, "view");
                },
                "requestContext");
        }

        [TestMethod]
        public void GetViewLocationWithNoLocationsThrows() {
            // Setup
            IViewLocator vl = new ViewLocator();

            // Execute
            ExceptionHelper.ExpectArgumentException(
                delegate {
                    vl.GetViewLocation(GetRequestContext(), "view");
                },
                "The parameter must be set to an array of location format strings, such as \"~/Views/{1}/{0}.aspx\".\r\nParameter name: locationFormats");
        }

        [TestMethod]
        public void GetViewLocationWithLocationsAndSpecificPathExistsReturnsSpecificPath() {
            // Setup
            MockVirtualPathProvider vppMock = new MockVirtualPathProvider(new List<KeyValuePair<string, bool>> {
                new KeyValuePair<string, bool>("~/view.specific", true)
            });

            ViewLocator vl = new ViewLocator();
            vl.ViewLocationFormats = new[] { "~/test/{1}/{0}/whatever" };
            vl.VirtualPathProvider = vppMock;

            // Execute
            string path = ((IViewLocator)vl).GetViewLocation(GetRequestContext(), "~/view.specific");

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

            ViewLocator vl = new ViewLocator();
            vl.ViewLocationFormats = new[] { "~/test/{1}/{0}/whatever" };
            vl.VirtualPathProvider = vppMock;

            // Execute
            ExceptionHelper.ExpectException<InvalidOperationException>(
                delegate {
                    ((IViewLocator)vl).GetViewLocation(GetRequestContext(), "~/view.specific");
                },
                "The view '~/view.specific' could not be located at these paths: ~/view.specific");

            // Verify
            vppMock.Verify();
        }

        [TestMethod]
        public void GetViewLocationWithLocationsAndMappedPathExistsReturnsSpecificPath() {
            // Setup
            MockVirtualPathProvider vppMock = new MockVirtualPathProvider(new List<KeyValuePair<string, bool>> {
                new KeyValuePair<string, bool>("~/test/MyController/view/whatever", true)
            });

            ViewLocator vl = new ViewLocator();
            vl.ViewLocationFormats = new[] { "~/test/{1}/{0}/whatever" };
            vl.VirtualPathProvider = vppMock;

            // Execute
            string path = ((IViewLocator)vl).GetViewLocation(GetRequestContext(), "view");

            // Verify
            Assert.AreEqual<string>("~/test/MyController/view/whatever", path);
            vppMock.Verify();
        }

        [TestMethod]
        public void GetViewLocationWithLocationsAndMappedPathNotExistsThrows() {
            // Setup
            MockVirtualPathProvider vppMock = new MockVirtualPathProvider(new List<KeyValuePair<string, bool>> {
                new KeyValuePair<string, bool>("~/test/MyController/view/whatever", false),
                new KeyValuePair<string, bool>("~/test2/MyController/view/whatever2", false)
            });
            ViewLocator vl = new ViewLocator();
            vl.ViewLocationFormats = new[] { "~/test/{1}/{0}/whatever", "~/test2/{1}/{0}/whatever2" };
            vl.VirtualPathProvider = vppMock;

            // Execute
            ExceptionHelper.ExpectException<InvalidOperationException>(
                delegate {
                    string path = ((IViewLocator)vl).GetViewLocation(GetRequestContext(), "view");
                },
                "The view 'view' could not be located at these paths: ~/test/MyController/view/whatever, ~/test2/MyController/view/whatever2");

            // Verify
            vppMock.Verify();
        }

        [TestMethod]
        public void GetViewLocationWithLocationsAndMappedPathExistsReturnsSecondSpecificPath() {
            // Setup
            MockVirtualPathProvider vppMock = new MockVirtualPathProvider(new List<KeyValuePair<string, bool>> {
                new KeyValuePair<string, bool>("~/test/MyController/view/whatever", false),
                new KeyValuePair<string, bool>("~/test2/MyController/view/whatever2", true)
            });
            ViewLocator vl = new ViewLocator();
            vl.ViewLocationFormats = new[] { "~/test/{1}/{0}/whatever", "~/test2/{1}/{0}/whatever2", "~/test3/{1}/{0}/whatever3" };
            vl.VirtualPathProvider = vppMock;

            // Execute
            string path = ((IViewLocator)vl).GetViewLocation(GetRequestContext(), "view");

            // Verify
            Assert.AreEqual<string>("~/test2/MyController/view/whatever2", path);
            vppMock.Verify();
        }

        [TestMethod]
        public void GetPathWithNullContextThrows() {
            // Setup
            ViewLocatorWrapper vl = new ViewLocatorWrapper();

            // Execute
            ExceptionHelper.ExpectArgumentNullException(
                delegate {
                    vl.GetPathPublic(null, null, "name");
                },
                "requestContext");
        }

        internal static RequestContext GetRequestContext() {
            RouteData routeData = new RouteData();
            routeData.Values["controller"] = "MyController";
            return new RequestContext(new Mock<HttpContextBase>().Object, routeData);
        }

        private sealed class ViewLocatorWrapper : ViewLocator {
            public string GetPathPublic(RequestContext requestContext, string[] locationFormats, string name) {
                return GetPath(requestContext, locationFormats, name);
            }
        }
    }
}
