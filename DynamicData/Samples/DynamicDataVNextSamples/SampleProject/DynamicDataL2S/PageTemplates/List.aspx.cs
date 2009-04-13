using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.DynamicData;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace DynamicDataProject {
    public partial class List : System.Web.UI.Page {
        protected MetaTable table;
    
        protected void Page_Init(object sender, EventArgs e) {
            table = GridQueryExtender.SetTableFromRoute();
            GridDataSource.EntityTypeName = table.EntityType.AssemblyQualifiedName;
        }

        protected void Page_Load(object sender, EventArgs e) {
            Title = table.DisplayName;

            // Disable various options if the table is readonly
            if (table.IsReadOnly) {
                GridView1.Columns[0].Visible = false;
                InsertHyperLink.Visible = false;
                //GridView1.EnablePersistedSelection = false;
            }
        }
    
        protected void Label_PreRender(object sender, EventArgs e) {
            Label label = (Label)sender;
            DynamicFilter dynamicFilter = (DynamicFilter)label.FindControl("DynamicFilter");
            QueryableFilterUserControl fuc = dynamicFilter.FilterTemplate as QueryableFilterUserControl;
            if (fuc != null && fuc.FilterControl != null) {
                label.AssociatedControlID = fuc.FilterControl.GetUniqueIDRelativeTo(label);
            }
        }
    
        protected void DynamicFilter_FilterChanged(object sender, EventArgs e) {
            GridView1.PageIndex = 0;
        }
    }
}
