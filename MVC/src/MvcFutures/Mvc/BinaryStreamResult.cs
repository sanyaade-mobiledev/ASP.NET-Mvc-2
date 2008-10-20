namespace Microsoft.Web.Mvc {
    using System;
    using System.IO;
    using System.Web;
    using System.Web.Mvc;

    [AspNetHostingPermission(System.Security.Permissions.SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
    [AspNetHostingPermission(System.Security.Permissions.SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
    public class BinaryStreamResult : BinaryResult {

        // default buffer size as defined in BufferedStream type
        private const int _bufferSize = 0x1000;

        public BinaryStreamResult(Stream stream) {
            if (stream == null) {
                throw new ArgumentNullException("stream");
            }

            Stream = stream;
        }

        public Stream Stream {
            get;
            private set;
        }

        public override void ExecuteResult(ControllerContext context) {
            base.ExecuteResult(context);

            // grab chunks of data and write to the output stream
            Stream outputStream = context.HttpContext.Response.OutputStream;
            using (Stream) {
                byte[] buffer = new byte[_bufferSize];

                while (true) {
                    int bytesRead = Stream.Read(buffer, 0, _bufferSize);
                    if (bytesRead == 0) {
                        // no more data
                        break;
                    }

                    outputStream.Write(buffer, 0, bytesRead);
                }
            }
        }

    }
}
