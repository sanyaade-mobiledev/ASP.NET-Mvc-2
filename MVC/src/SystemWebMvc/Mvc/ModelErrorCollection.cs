namespace System.Web.Mvc {
    using System;
    using System.Collections.ObjectModel;
    using System.Web;

    [AspNetHostingPermission(System.Security.Permissions.SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
    [AspNetHostingPermission(System.Security.Permissions.SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
    public class ModelErrorCollection : Collection<ModelError> {

        public void Add(Exception exception) {
            Add(new ModelError(exception));
        }

        public void Add(string errorMessage) {
            Add(new ModelError(errorMessage));
        }
    }
}
