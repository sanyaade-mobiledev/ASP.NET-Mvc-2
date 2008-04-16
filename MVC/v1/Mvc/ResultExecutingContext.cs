namespace System.Web.Mvc {
    using System;
    using System.Web;

    [AspNetHostingPermission(System.Security.Permissions.SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
    [AspNetHostingPermission(System.Security.Permissions.SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
    public class ResultExecutingContext : ControllerContext {

        private ActionResult _result;

        public ResultExecutingContext(ControllerContext controllerContext, ActionResult result)
            : base(controllerContext) {

            if (result == null) {
                throw new ArgumentNullException("result");
            }
            _result = result;
        }

        public bool Cancel {
            get;
            set;
        }

        public ActionResult Result {
            get {
                return _result;
            }
            set {
                if (value == null) {
                    throw new ArgumentNullException("value");
                }
                _result = value;
            }
        }

    }
}
