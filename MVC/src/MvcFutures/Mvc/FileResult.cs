namespace Microsoft.Web.Mvc {
    using System;
    using System.Text;
    using System.Web.Mvc;
    using Microsoft.Web.Mvc.Resources;
    using System.Web;

    [AspNetHostingPermission(System.Security.Permissions.SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
    [AspNetHostingPermission(System.Security.Permissions.SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
    public class FileResult : ActionResult {
        public FileResult(string filePath) {
            if (String.IsNullOrEmpty(filePath)) {
                throw new ArgumentException(MvcResources.Common_NullOrEmpty, "filePath");
            }
            FilePath = filePath;
        }
        
        public string FilePath{ 
            get; 
            private set; 
        }

        public string ContentType {
            get;
            set;
        }

        public Encoding ContentEncoding {
            get;
            set;
        }

        public string FileDownloadName {
            get;
            set;
        }

        public override void ExecuteResult(ControllerContext context) {
            if (context == null) {
                throw new ArgumentNullException("context", MvcResources.Common_NullOrEmpty);
            }

            if (ContentEncoding != null) {
                context.HttpContext.Response.ContentEncoding = ContentEncoding;
            }

            if (!String.IsNullOrEmpty(ContentType)) {
                context.HttpContext.Response.ContentType = ContentType;
            }

            if (!String.IsNullOrEmpty(FileDownloadName)) {
                context.HttpContext.Response.AddHeader("content-disposition", "attachment; filename=" + FileDownloadName);
            }

            context.HttpContext.Response.TransmitFile(FilePath);
        }
    }
}
