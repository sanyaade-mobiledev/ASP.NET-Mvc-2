using WebFormRouting;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Web.Routing;
using System.Web;
using Moq;
using System;

namespace WebFormRoutingTests
{
    [TestClass]
    public class WebFormRouteHandlerTests
    {
        [TestMethod]
        public void Ctor_NullVirtualPathThrowsArgumentNullException() {
            try {
                var handler = new WebFormRouteHandler(null);
            }
            catch (ArgumentNullException) {
                return;
            }
            Assert.Fail();
        }

        [TestMethod]
        public void Ctor_VirtualPathStartingWithoutTildeSlashThrowsException() {
            try {
                var handler = new WebFormRouteHandler("/products");
            }
            catch (ArgumentException) {
                return;
            }
            Assert.Fail();
        }

        [TestMethod]
        public void Ctor_VirtualPathStartingWithoutTildeSlashThrowsException2()
        {
            try {
                var handler = new WebFormRouteHandler("products");
            }
            catch (ArgumentException) {
                return;
            }
            Assert.Fail();
        }

        [TestMethod]
        public void CanSetVirtualPathFromCtor() {
            var handler = new WebFormRouteHandler("~/Foo/Bar.aspx");
            Assert.AreEqual("~/Foo/Bar.aspx", handler.VirtualPath, "If this test fails, that would be really bad.");
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
        public void CanSubstituteRouteUrlParameterWithValueFromContext() {
            var handler = new WebFormRouteHandler("~/products/{action}.aspx");
            var httpContext = new Mock<HttpContextBase>();
            var routeData = new RouteData();
            routeData.Values.Add("action", "show");
            var requestContext = new RequestContext(httpContext.Object, routeData);
            string result = handler.GetSubstitutedVirtualPath(requestContext);
            Assert.AreEqual("~/products/show.aspx", result);
            
        }
    }
}
