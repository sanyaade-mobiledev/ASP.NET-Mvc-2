using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.DynamicData;

namespace DynamicDataProject {
    public partial class ProductsList : System.Web.UI.Page {
        protected MetaTable table;

        protected void Page_Init(object sender, EventArgs e) {
            DynamicDataManager1.RegisterControl(ListView1, true /*setSelectionFromUrl*/);
        }

        protected void Page_Load(object sender, EventArgs e) {
            table = GridDataSource.GetTable();
            Title = table.DisplayName;

            // Disable various options if the table is readonly
            //if (table.IsReadOnly) {
            //    //GridView1.Columns[0].Visible = false;
            //}
        }
    }
}
