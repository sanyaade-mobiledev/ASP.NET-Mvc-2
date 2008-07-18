using System.Collections.Generic;
using System.Security.Permissions;
using System.Web;
using System.Web.Mvc;

namespace Microsoft.Web.DynamicData.Mvc {
    [AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
    public static class ValidationExtensions {
        const string VIEWDATA_MODELERRORS = "__MODEL_ERRORS__";
        const string VIEWDATA_MODELSTATE = "__MODEL_STATE__";

        public static IList<ModelError> ModelErrors(this ViewDataDictionary viewData) {
            if (!viewData.ContainsKey(VIEWDATA_MODELERRORS))
                viewData[VIEWDATA_MODELERRORS] = new List<ModelError>();

            return (IList<ModelError>)viewData[VIEWDATA_MODELERRORS];
        }

        public static ModelStateList ModelState(this ViewDataDictionary viewData) {
            if (!viewData.ContainsKey(VIEWDATA_MODELSTATE))
                viewData[VIEWDATA_MODELSTATE] = new ModelStateList();

            return (ModelStateList)viewData[VIEWDATA_MODELSTATE];
        }
    }
}