using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Xml.Linq;
using System.Web.DynamicData;
using Microsoft.Web.DynamicData;

namespace DynamicDataProject
{
    public partial class List : System.Web.UI.Page {
        protected MetaTable table;

        protected void Page_Init(object sender, EventArgs e) {
            table = GridDataSource.GetTable();
            DynamicDataManager1.RegisterControl(GridView1, true /*setSelectionFromUrl*/);
            GridView1.EnablePersistedSelection();
            GridView1.ColumnsGenerator = new AdvancedFieldGenerator(table, true);
            DynamicDataFutures.RegisterListDefaults(GridDataSource, InsertHyperLink);
        }

        protected void Page_Load(object sender, EventArgs e) {
            Title = table.DisplayName;

            // Disable various options if the table is readonly
            if (table.IsReadOnly) {
                GridView1.Columns[0].Visible = false;
                InsertHyperLink.Visible = false;
            }
        }

        protected void OnFilterSelectionChanged(object sender, EventArgs e) {
            GridView1.PageIndex = 0;
        }
    }
}
