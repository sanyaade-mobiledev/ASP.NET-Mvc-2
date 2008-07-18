using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Security.Permissions;
using System.Web;

namespace Microsoft.Web.DynamicData.Mvc {
    [AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
    [AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
    public class ModelStateList : Collection<ModelPropertyState> {
        public ModelPropertyState GetPropertyState(string propertyName) {
            return (from error in this
                    where String.Equals(propertyName, error.PropertyName, StringComparison.OrdinalIgnoreCase)
                    select error).SingleOrDefault();
        }
    }
}
