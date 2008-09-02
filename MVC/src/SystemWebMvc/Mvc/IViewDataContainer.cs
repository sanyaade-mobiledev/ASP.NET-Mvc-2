namespace System.Web.Mvc {
    using System.Diagnostics.CodeAnalysis;
    using System.Web;

    [AspNetHostingPermission(System.Security.Permissions.SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
    [AspNetHostingPermission(System.Security.Permissions.SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
    public interface IViewDataContainer {
        [SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly",
            Justification = "This is the mechanism by which the ViewPage / ViewUserControl get their ViewDataDictionary objects.")]
        ViewDataDictionary ViewData { get; set; }
    }
}
