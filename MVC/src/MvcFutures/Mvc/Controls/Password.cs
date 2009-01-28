namespace Microsoft.Web.Mvc.Controls {
    using System.Web;

    [AspNetHostingPermission(System.Security.Permissions.SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
    [AspNetHostingPermission(System.Security.Permissions.SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
    public class Password : MvcInputControl {
        public Password() :
            base("password") {
        }
    }
}
