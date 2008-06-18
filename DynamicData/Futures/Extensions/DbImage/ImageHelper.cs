using System;
using System.Web.DynamicData;
using System.Web.UI;

namespace Microsoft.Web.DynamicData.Extensions {
    public static class ImageHelper {

        public static void DisablePartialRenderingForUpload(Page page, MetaTable table) {
            foreach (var column in table.Columns) {
                if (String.Equals(column.UIHint, "DbImage", StringComparison.OrdinalIgnoreCase)) {
                    var sm = ScriptManager.GetCurrent(page);
                    if (sm != null) {
                        sm.EnablePartialRendering = false;
                    }
                    break;
                }
            }
        }
    }
}
