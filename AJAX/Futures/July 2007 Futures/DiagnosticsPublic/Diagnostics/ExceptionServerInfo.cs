namespace Microsoft.Web.Preview.Diagnostics {
    using System;
    using System.Collections;

    public class ExceptionServerInfo : ExceptionInfo {
        private string timestamp;
        private string userAgent;
        private string ipAddress;

        public string Timestamp {
            get { return timestamp; }
            set { timestamp = value; }
        }

        public string UserAgent {
            get { return userAgent; }
            set { userAgent = value; }
        }

        public string IPAddress {
            get { return ipAddress; }
            set { ipAddress = value; }
        }
    }
}