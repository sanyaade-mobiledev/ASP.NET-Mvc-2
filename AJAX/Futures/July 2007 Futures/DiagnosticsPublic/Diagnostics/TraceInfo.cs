namespace Microsoft.Web.Preview.Diagnostics {
    using System;
    using System.Collections;

    public class TraceInfo {
        private string message;

        public string Message {
            get { return message; }
            set { message = value; }
        }
    }
}