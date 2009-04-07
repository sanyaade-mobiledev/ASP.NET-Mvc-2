namespace Microsoft.Web.Preview.Diagnostics {
    using System;

    public class ClientExceptionEventArgs : EventArgs {
        private ExceptionServerInfo _exceptionServerInfo;

        public ExceptionServerInfo ExceptionServerInfo {
            get {
                return _exceptionServerInfo;
            }
        }

        public ClientExceptionEventArgs(ExceptionServerInfo exceptionServerInfo) {
            _exceptionServerInfo = exceptionServerInfo;
        }
    }
}