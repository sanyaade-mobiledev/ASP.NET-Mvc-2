namespace Microsoft.Web.Mvc {
    using System.Web;

    [AspNetHostingPermission(System.Security.Permissions.SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
    [AspNetHostingPermission(System.Security.Permissions.SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
    public abstract class AntiForgeryTokenSerializer {

        public abstract string Serialize(AntiForgeryToken token);
        public abstract AntiForgeryToken Deserialize(string serializedToken);

    }
}
