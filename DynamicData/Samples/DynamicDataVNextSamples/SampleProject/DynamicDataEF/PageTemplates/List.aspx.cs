using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.DynamicData;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace DynamicDataEFProject {
    public partial class List : System.Web.UI.Page {
        protected MetaTable table;
    
        protected void Page_Init(object sender, EventArgs e) {
            table = GridDataSource.SetTableFromRoute();
            GridDataSource.EntityTypeFilter = table.Name;
        }
    
        protected void Page_Load(object sender, EventArgs e) {
            Title = table.DisplayName;
            GridDataSource.Include = table.ForeignKeyColumnsNames;
    
            // Disable various options if the table is readonly
            if (table.IsReadOnly) {
                GridView1.Columns[0].Visible = false;
                InsertHyperLink.Visible = false;
                //GridView1.EnablePersistedSelection = false;
            }
        }
    
        protected void OnFilterSelectedIndexChanged(object sender, EventArgs e) {
            GridView1.PageIndex = 0;
        }
    
    }
}
