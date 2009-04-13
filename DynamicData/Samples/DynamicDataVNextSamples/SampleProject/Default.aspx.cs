using System;
using System.ComponentModel.DataAnnotations;
using System.Web.DynamicData;

namespace DynamicDataProject {
    public partial class _Default : System.Web.UI.Page {
        protected void Page_Load(object sender, EventArgs e) {
            System.Collections.IList l2sTables = Global.L2Smodel.VisibleTables;
            System.Collections.IList efTables = Global.EFmodel.VisibleTables;
            if (l2sTables.Count == 0 && efTables.Count == 0) {
                throw new InvalidOperationException("There are no accessible tables. Make sure that at least one data model is registered in Global.asax and scaffolding is enabled or implement custom pages.");
            }
            Menu1.DataSource = l2sTables;
            Menu1.DataBind();

            Menu2.DataSource = efTables;
            Menu2.DataBind();
        }
    }
}
