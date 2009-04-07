namespace Microsoft.Web.Preview.Diagnostics {
    using System;
    using System.Collections;

    public class ExceptionInfo {
        private string filename;
        private string lineNumber;
        private string message;
        private string data;

        public string FileName {
            get { return filename; }
            set { filename = value; }
        }

        public string LineNumber {
            get { return lineNumber; }
            set { lineNumber = value; }
        }

        public string Message {
            get { return message; }
            set { message = value; }
        }

        public string Data {
            get { return data; }
            set { data = value; }
        }
    }
}