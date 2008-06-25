using System;
using System.Web.DynamicData;
using System.Web.UI.WebControls;
using Microsoft.Web.DynamicData;

namespace DynamicDataFuturesSample {
    public partial class Edit : System.Web.UI.Page {
        protected MetaTable table;

        protected void Page_Init(object sender, EventArgs e) {
            DynamicDataManager1.RegisterControl(DetailsView1);
            table = DetailsDataSource.GetTable();
            DynamicDataFutures.DisablePartialRenderingForUpload(this, table);

            DetailsView1.RowsGenerator = new AdvancedFieldGenerator(table, false);
        }

        protected void Page_Load(object sender, EventArgs e) {
            Title = table.GetDisplayName();
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
