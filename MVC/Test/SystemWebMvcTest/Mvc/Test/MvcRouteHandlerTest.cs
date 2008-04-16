namespace System.Web.Mvc.Test {
    using System.Web.Routing;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;

    [TestClass]
    public class MvcRouteHandlerTest {
        [TestMethod]
        public void GetHttpHandlerReturnsMvcHandlerWithRouteData() {
            // Setup
            RequestContext context =new RequestContext(new Mock<HttpContextBase>().Object, new RouteData());
            IRouteHandler rh = new MvcRouteHandler();

            // Execute
            IHttpHandler httpHandler = rh.GetHttpHandler(context);

            // Verify
            MvcHandler h = httpHandler as MvcHandler;
            Assert.IsNotNull(h, "The handler should be a valid MvcHandler instance");
            Assert.AreEqual<RequestContext>(context, h.RequestContext);
        }
    }
}
