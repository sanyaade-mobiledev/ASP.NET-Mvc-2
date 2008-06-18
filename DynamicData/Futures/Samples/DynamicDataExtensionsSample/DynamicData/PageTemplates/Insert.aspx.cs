using System;
using System.Web.DynamicData;
using System.Web.UI.WebControls;
using Microsoft.Web.DynamicData.Extensions;

namespace DynamicDataExtensionsSample {
    public partial class Insert : System.Web.UI.Page {
        protected MetaTable table;

        protected void Page_Init(object sender, EventArgs e) {
            DynamicDataManager1.RegisterControl(DetailsView1);
            ImageHelper.DisablePartialRenderingForUpload(this, DetailsDataSource.GetTable());
            DefaultValueHelper.RegisterInsertDefaults(DetailsDataSource, DetailsView1, false);

            LocalizationHelper.Register(DetailsView1);
        }

        protected void Page_Load(object sender, EventArgs e) {
            table = DetailsDataSource.GetTable();
            Title = table.GetDisplayName();
        }

        protected void DetailsView1_ItemCommand(object sender, DetailsViewCommandEventArgs e) {
            if (e.CommandName == DataControlCommands.CancelCommandName) {
                Response.Redirect(table.ListActionPath);
            }
        }

        protected void DetailsView1_ItemInserted(object sender, DetailsViewInsertedEventArgs e) {
            if (e.Exception == null || e.ExceptionHandled) {
                Response.Redirect(table.ListActionPath);
            }
        }
    }
}
