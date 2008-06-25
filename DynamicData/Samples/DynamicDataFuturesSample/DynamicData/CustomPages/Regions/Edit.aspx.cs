using System;
using System.Web.DynamicData;
using System.Web.UI.WebControls;

namespace DynamicDataFuturesSample {
    public partial class Regions_Edit : System.Web.UI.Page {
        protected MetaTable table;

        protected void Page_Init(object sender, EventArgs e) {
            DynamicDataManager1.RegisterControl(FormView1);
        }

        protected void Page_Load(object sender, EventArgs e) {
            table = DetailsDataSource.GetTable();
            Title = table.DisplayName;

        }

        protected void DetailsView1_ItemCommand(object sender, DetailsViewCommandEventArgs e) {
            if (e.CommandName == DataControlCommands.CancelCommandName) {
                Response.Redirect(table.ListActionPath);
            }
        }

        protected void DetailsView1_ItemUpdated(object sender, DetailsViewUpdatedEventArgs e) {
            if (e.Exception == null || e.ExceptionHandled) {
                Response.Redirect(table.ListActionPath);
            }
        }
    }
}
