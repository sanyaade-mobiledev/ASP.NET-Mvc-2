namespace System.Web.Mvc.Test {
    using System.Collections.Generic;
    using System.Collections;
    using System.Web.Routing;
    using System.Web.TestUtil;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    
    [TestClass]
    public class SessionStateTempDataProviderTest {

        [TestMethod]
        public void SessionProviderThrowsOnDisabledSessionState() {
            // Arrange
            SessionStateTempDataProvider testProvider = new SessionStateTempDataProvider();

            // Act & Assert
            ExceptionHelper.ExpectInvalidOperationException(
                delegate { 
                    IDictionary<string, object> tempDataDictionary = testProvider.LoadTempData(GetControllerContext()); 
                },
                "The provider requires SessionState to be enabled.");

            ExceptionHelper.ExpectInvalidOperationException(
                delegate { 
                    testProvider.SaveTempData(GetControllerContext(), new Dictionary<string, object>()); 
                },
                "The provider requires SessionState to be enabled.");
        }

        private static ControllerContext GetControllerContext() {
            Mock<HttpContextBase> mockContext = new Mock<HttpContextBase>();
            mockContext.Expect(o => o.Session).Returns((HttpSessionStateBase)null);
            RouteData rd = new RouteData();
            return new ControllerContext(mockContext.Object, rd, new Mock<ControllerBase>().Object);
        }
    }
}
