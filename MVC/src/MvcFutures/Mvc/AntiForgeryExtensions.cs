namespace Microsoft.Web.Mvc {
    using System.Web;
    using System.Web.Mvc;

    [AspNetHostingPermission(System.Security.Permissions.SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
    public static class AntiForgeryExtensions {

        private static readonly AntiForgeryHelper _helper = new AntiForgeryHelper();

        public static string AntiForgeryToken(this HtmlHelper helper) {
            return AntiForgeryToken(helper, null /* salt */);
        }

        public static string AntiForgeryToken(this HtmlHelper helper, string salt) {
            return _helper.AntiForgeryToken(helper, salt);
        }

    }
}
