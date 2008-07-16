namespace System.Web.Mvc.Test {
    using System;
    using System.Web.Routing;
    using System.Web.TestUtil;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;

    [TestClass]
    public class MvcHttpHandlerTest {
        [TestMethod]
        public void ConstructorDoesNothing() {
            new MvcHttpHandler();
        }

        [TestMethod]
        public void VerifyAndProcessRequestWithNullHandlerThrows() {
            // Setup
            PublicMvcHttpHandler handler = new PublicMvcHttpHandler();

            // Execute
            ExceptionHelper.ExpectArgumentNullException(
                delegate {
                    handler.PublicVerifyAndProcessRequest(null, null);
                },
                "httpHandler");
        }

        [TestMethod]
        public void VerifyAndProcessRequestWithNonMvcHandlerThrows() {
            // Setup
            PublicMvcHttpHandler handler = new PublicMvcHttpHandler();
            DummyHttpHandler dummyHandler = new DummyHttpHandler();

            // Execute
            ExceptionHelper.ExpectArgumentException(
                delegate {
                    handler.PublicVerifyAndProcessRequest(dummyHandler, null);
                },
                @"The MvcHttpHandler does not work with the handler of type 'System.Web.Mvc.Test.MvcHttpHandlerTest+DummyHttpHandler' since it does not derive from MvcHandler.
Parameter name: httpHandler");
        }

        [TestMethod]
        public void ProcessRequestCallsExecute() {
            // Setup
            PublicMvcHttpHandler handler = new PublicMvcHttpHandler();
            Mock<MvcHandler> mockTargetHandler = new Mock<MvcHandler>(new RequestContext(new Mock<HttpContextBase>().Object, new RouteData()));
            mockTargetHandler.Expect(h => h.ProcessRequest(It.IsAny<HttpContextBase>())).Verifiable();

            // Execute
            handler.PublicVerifyAndProcessRequest(mockTargetHandler.Object, null);

            // Verify
            mockTargetHandler.Verify();
        }

        private sealed class DummyHttpHandler : IHttpHandler {
            bool IHttpHandler.IsReusable {
                get {
                    throw new NotImplementedException();
                }
            }

            void IHttpHandler.ProcessRequest(HttpContext context) {
                throw new NotImplementedException();
            }
        }

        private sealed class PublicMvcHttpHandler : MvcHttpHandler {
            public void PublicVerifyAndProcessRequest(IHttpHandler httpHandler, HttpContextBase httpContext) {
                base.VerifyAndProcessRequest(httpHandler, httpContext);
            }
        }
    }
}
