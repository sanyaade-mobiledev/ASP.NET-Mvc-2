using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.DynamicData;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace DynamicDataProject {
    public partial class Details : System.Web.UI.Page {
        protected MetaTable table;
    
        protected void Page_Init(object sender, EventArgs e) {
            table = DetailsDataSource.SetTableFromRoute();
            DetailsDataSource.EntityTypeName = table.EntityType.AssemblyQualifiedName;
        }

        protected void Page_Load(object sender, EventArgs e) {
            Title = table.DisplayName;
        }
    
        protected void FormView1_ItemDeleted(object sender, FormViewDeletedEventArgs e) {
            if (e.Exception == null || e.ExceptionHandled) {
                Response.Redirect(table.ListActionPath);
            }
        }
    
        protected void FormView1_DataBound(object sender, EventArgs e) {
            object dataItem = FormView1.DataItem;
    
            if (dataItem != null) {
                
                MetaTable itemTypeMetaTable = MetaTable.GetTable(dataItem.GetType());
    
                if (!table.Equals(itemTypeMetaTable)) {
                    Response.Redirect(itemTypeMetaTable.GetActionPath(PageAction.Details, FormView1.DataItem));
                }
            }
        }
    
    }
}
