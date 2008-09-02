namespace Microsoft.Web.Mvc {
    using System;
    using System.Runtime.Serialization;
    using System.Web;

    [Serializable]
    [AspNetHostingPermission(System.Security.Permissions.SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
    [AspNetHostingPermission(System.Security.Permissions.SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
    public class AntiForgeryTokenValidationException : Exception {
        //
        // For guidelines regarding the creation of new exception types, see
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/cpgenref/html/cpconerrorraisinghandlingguidelines.asp
        // and
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/dncscol/html/csharp07192001.asp
        //

        public AntiForgeryTokenValidationException() {
        }

        public AntiForgeryTokenValidationException(string message)
            : base(message) {
        }

        public AntiForgeryTokenValidationException(string message, Exception inner)
            : base(message, inner) {
        }

        protected AntiForgeryTokenValidationException(SerializationInfo info, StreamingContext context)
            : base(info, context) {
        }
    }
}
