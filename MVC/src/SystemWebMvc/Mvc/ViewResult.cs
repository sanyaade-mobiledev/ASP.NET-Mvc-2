﻿namespace System.Web.Mvc {
    using System;
    using System.Globalization;
    using System.Text;
    using System.Web;
    using System.Web.Mvc.Resources;

    [AspNetHostingPermission(System.Security.Permissions.SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
    [AspNetHostingPermission(System.Security.Permissions.SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
    public class ViewResult : ViewResultBase {
        private string _masterName;

        public string MasterName {
            get {
                return _masterName ?? String.Empty;
            }
            set {
                _masterName = value;
            }
        }

        protected override ViewEngineResult FindView(ControllerContext context) {
            ViewEngineResult result = ViewEngine.FindView(context, ViewName, MasterName);
            if (result.View != null) {
                return result;
            }

            // we need to generate an exception containing all the locations we searched
            StringBuilder locationsText = new StringBuilder();
            foreach (string location in result.SearchedLocations) {
                locationsText.AppendLine();
                locationsText.Append(location);
            }
            throw new InvalidOperationException(String.Format(CultureInfo.CurrentUICulture,
                MvcResources.Common_ViewNotFound, ViewName, locationsText));
        }
    }
}