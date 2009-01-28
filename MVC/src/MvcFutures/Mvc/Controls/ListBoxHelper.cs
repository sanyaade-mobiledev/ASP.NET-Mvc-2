namespace Microsoft.Web.Mvc.Controls {
    using System;
    using System.Globalization;
    using System.Web;
    using System.Web.Mvc;

    [AspNetHostingPermission(System.Security.Permissions.SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
    public static class ListBoxHelper {
        public static TList GetSelectData<TList>(ViewDataDictionary viewData, string name) where TList : MultiSelectList {
            object o = null;
            if (viewData != null) {
                o = viewData.Eval(name);
            }
            if (o == null) {
                throw new InvalidOperationException(
                    String.Format(
                        CultureInfo.CurrentUICulture,
                        "There is no ViewData item with the key '{0}' of type '{1}'.",
                        name,
                        typeof(TList)));
            }
            TList selectList = o as TList;
            if (selectList == null) {
                throw new InvalidOperationException(
                    String.Format(
                        CultureInfo.CurrentUICulture,
                        "The ViewData item with the key '{0}' is of type '{1}' but needs to be of type '{2}'.",
                        name,
                        o.GetType().FullName,
                        typeof(TList)));
            }
            return selectList;
        }
    }
}
