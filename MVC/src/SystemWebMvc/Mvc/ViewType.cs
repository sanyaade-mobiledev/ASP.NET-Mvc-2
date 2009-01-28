namespace System.Web.Mvc {
    using System.ComponentModel;
    using System.Web.UI;

    [ControlBuilder(typeof(ViewTypeControlBuilder))]
    [NonVisualControl]
    [AspNetHostingPermission(System.Security.Permissions.SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
    [AspNetHostingPermission(System.Security.Permissions.SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
    public class ViewType : Control {
        private string _typeName;

        [DefaultValue("")]
        public string TypeName {
            get {
                return _typeName ?? String.Empty;
            }
            set {
                _typeName = value;
            }
        }
    }
}
