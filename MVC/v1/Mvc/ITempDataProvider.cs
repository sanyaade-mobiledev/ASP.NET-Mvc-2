namespace System.Web.Mvc {
    [AspNetHostingPermission(System.Security.Permissions.SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
    [AspNetHostingPermission(System.Security.Permissions.SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
    public interface ITempDataProvider {
        TempDataDictionary LoadTempData();
        void SaveTempData(TempDataDictionary tempDataDictionary);        
    }
}
