namespace System.Web.Mvc {

    // represents a result that doesn't do anything, like a controller action returning null
    [AspNetHostingPermission(System.Security.Permissions.SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
    [AspNetHostingPermission(System.Security.Permissions.SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
    public class EmptyResult : ActionResult {

        public override void ExecuteResult(ControllerContext context) {
        }

    }

}
