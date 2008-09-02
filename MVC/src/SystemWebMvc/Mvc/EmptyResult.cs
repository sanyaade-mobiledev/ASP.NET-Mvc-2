namespace System.Web.Mvc {
    using System.Web;

    // represents a result that doesn't do anything, like a controller action returning null
    [AspNetHostingPermission(System.Security.Permissions.SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
    [AspNetHostingPermission(System.Security.Permissions.SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
    public class EmptyResult : ActionResult {

        private static readonly EmptyResult _singleton = new EmptyResult();

        internal static EmptyResult Instance {
            get {
                return _singleton;
            }
        }

        public override void ExecuteResult(ControllerContext context) {
        }
    }
}
