namespace System.Web.Mvc {
    using System;
    using System.Web;
    using System.Web.Mvc.Resources;

    [AspNetHostingPermission(System.Security.Permissions.SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
    [AspNetHostingPermission(System.Security.Permissions.SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
    public class FilePathResult : FileResult {

        public FilePathResult(string fileName, string contentType)
            : base(contentType) {
            if (String.IsNullOrEmpty(fileName)) {
                throw new ArgumentException(MvcResources.Common_NullOrEmpty, "fileName");
            }

            FileName = fileName;
        }

        public string FileName {
            get;
            private set;
        }

        protected override void WriteFile(HttpResponseBase response) {
            response.TransmitFile(FileName);
        }

    }
}
