using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WebFormRouting;
using System.Web.Routing;
using System.Web;
using Moq;

namespace UnitTests {
    [TestClass]
    public class WebFormRouteHandlerTests {
        [TestMethod]
        public void CanSetVirtualPathFromCtor() {
            var handler = new WebFormRouteHandler("~/Foo/Bar.aspx");
            Assert.AreEqual("~/Foo/Bar.aspx", handler.VirtualPath, "If this test fails, I'm an idiot!");
        }

        [TestMethod]
        public void CheckPhysicalUrlAccessIsTrueByDefault() {
            var handler = new WebFormRouteHandler("~/Foo/Bar.aspx");
            Assert.IsTrue(handler.CheckPhysicalUrlAccess, "'CheckPhysicalUrlAccess' should be secure by default and set to true.");
        }

        [TestMethod]
        public void CanSetCheckPhysicalUrlAccessToFalse() {
            var handler = new WebFormRouteHandler("~/Foo/Bar.aspx", false);
            Assert.IsFalse(handler.CheckPhysicalUrlAccess, "Could not set 'CheckPhysicalUrlAccess' to false via ctor.");
            handler.CheckPhysicalUrlAccess = true;
            Assert.IsTrue(handler.CheckPhysicalUrlAccess, "Could not set 'CheckPhysicalUrlAccess' to true via prop.");
            handler.CheckPhysicalUrlAccess = false;
            Assert.IsFalse(handler.CheckPhysicalUrlAccess, "Could not set 'CheckPhysicalUrlAccess' to false via prop.");
        }

        [TestMethod]
        public void CanSubstituteVirtualPathParameterViaRouteData() {
            var handler = new WebFormRouteHandler("~/Foo/{bar}.aspx", false);
            var httpContext = new Mock<HttpContextBase>();
            var routeData = new RouteData();
            routeData.Values.Add("bar", "Testing");
            var requestContext = new RequestContext(httpContext.Object, routeData);
            string virtualPath = handler.GetSubstitutedVirtualPath(requestContext);

            Assert.AreEqual("~/Foo/Testing.aspx", virtualPath);
        }
    }
}
