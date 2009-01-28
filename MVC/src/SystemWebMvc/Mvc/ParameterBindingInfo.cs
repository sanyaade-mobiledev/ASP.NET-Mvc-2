namespace System.Web.Mvc {
    using System;
    using System.Collections.Generic;
    using System.Web;

    [AspNetHostingPermission(System.Security.Permissions.SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
    [AspNetHostingPermission(System.Security.Permissions.SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
    public abstract class ParameterBindingInfo {

        public virtual IModelBinder Binder {
            get {
                return null;
            }
        }

        public virtual ICollection<string> Exclude {
            get {
                return new string[0];
            }
        }

        public virtual ICollection<string> Include {
            get {
                return new string[0];
            }
        }

        public virtual string Prefix {
            get {
                return null;
            }
        }

    }
}
