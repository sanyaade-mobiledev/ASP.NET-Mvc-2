namespace Microsoft.Web.Mvc {
    using System;
    using System.Net.Mime;
    using System.Web;
    using System.Web.Mvc;

    [AspNetHostingPermission(System.Security.Permissions.SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
    [AspNetHostingPermission(System.Security.Permissions.SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
    public abstract class BinaryResult : ActionResult {

        private const string _defaultContentType = MediaTypeNames.Application.Octet;

        private string _contentType;
        private string _fileDownloadName;

        public string ContentType {
            get {
                if (String.IsNullOrEmpty(_contentType)) {
                    _contentType = _defaultContentType;
                }
                return _contentType;
            }
            set {
                _contentType = value;
            }
        }

        public string FileDownloadName {
            get {
                return _fileDownloadName ?? String.Empty;
            }
            set {
                _fileDownloadName = value;
            }
        }

        public override void ExecuteResult(ControllerContext context) {
            if (context == null) {
                throw new ArgumentNullException("context");
            }

            context.HttpContext.Response.ContentType = ContentType;

            if (!String.IsNullOrEmpty(FileDownloadName)) {
                // From RFC 1806, Sec. 2.3:
                // The sender may want to suggest a filename to be used if the entity is
                // detached and stored in a separate file. If the receiving MUA writes
                // the entity to a file, the suggested filename should be used as a
                // basis for the actual filename, where possible.
                string headerValue = "attachment; filename=" + HttpUtility.UrlEncode(FileDownloadName);
                context.HttpContext.Response.AddHeader("Content-Disposition", headerValue);
            }
        }

    }
}
