namespace System.Web.Mvc {
    using System.Web;

    [AspNetHostingPermission(System.Security.Permissions.SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
    [AspNetHostingPermission(System.Security.Permissions.SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
    [Serializable]
    public class ModelState {

        private ModelErrorCollection _errors = new ModelErrorCollection();

        public ValueProviderResult Value {
            get;
            set;
        }

        public ModelErrorCollection Errors {
            get {
                return _errors;
            }
        }
    }
}
