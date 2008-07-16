namespace System.Web.Mvc.Test {
    using System;
    using System.Text;
    using System.Collections.Generic;
    using System.Collections;
    using System.Linq;
    using System.Web.TestUtil;
    using System.Web.Mvc.Resources;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    
    [TestClass]
    public class SessionStateTempDataProviderTest {

        [TestMethod]
        public void ConstructProviderThrowsOnNullHttpContext() {
            ExceptionHelper.ExpectArgumentNullException(
                delegate { 
                    new SessionStateTempDataProvider(null); 
                },
                "httpContext");
        }

        [TestMethod]
        public void SessionProviderThrowsOnDisabledSessionState() {
            // Setup
            TestTempDataHttpContext mockContext = new TestTempDataHttpContext();
            SessionStateTempDataProvider testProvider = new SessionStateTempDataProvider(mockContext);

            // Execute & Verify
            ExceptionHelper.ExpectInvalidOperationException(
                delegate { 
                    TempDataDictionary tempDataDictionary = testProvider.LoadTempData(); 
                },
                "The provider requires SessionState to be enabled.");

            ExceptionHelper.ExpectInvalidOperationException(
                delegate { 
                    testProvider.SaveTempData(new TempDataDictionary()); 
                },
                "The provider requires SessionState to be enabled.");
        }

        private sealed class TestTempDataHttpSessionState : HttpSessionStateBase {
 
        }

        private sealed class TestTempDataHttpContext : HttpContextBase {
            private TestTempDataHttpSessionState _sessionState = null;

            public override HttpSessionStateBase Session {
                get {
                    return _sessionState;
                }
            }
        }
    }
}
