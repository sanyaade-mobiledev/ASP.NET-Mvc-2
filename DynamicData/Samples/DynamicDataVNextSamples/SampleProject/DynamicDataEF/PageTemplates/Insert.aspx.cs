using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.DynamicData;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace DynamicDataEFProject {
    public partial class Insert : System.Web.UI.Page {
        protected MetaTable table;
    
        protected void Page_Init(object sender, EventArgs e) {
            table = DetailsDataSource.SetTableFromRoute();
            DetailsDataSource.EntityTypeFilter = table.Name;
        }
    
        protected void Page_Load(object sender, EventArgs e) {
            Title = table.DisplayName;
        }
    
        protected void FormView1_ItemCommand(object sender, FormViewCommandEventArgs e) {
            if (e.CommandName == DataControlCommands.CancelCommandName) {
                Response.Redirect(table.ListActionPath);
            }
        }
    
        protected void FormView1_ItemInserted(object sender, FormViewInsertedEventArgs e) {
            if (e.Exception == null || e.ExceptionHandled) {
                Response.Redirect(table.ListActionPath);
            }
        }
    
    }
}
