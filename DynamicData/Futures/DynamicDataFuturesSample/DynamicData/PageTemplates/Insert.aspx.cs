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
    public partial class Insert : System.Web.UI.Page {
        protected MetaTable table;

        protected void Page_Init(object sender, EventArgs e) {
            table = DetailsDataSource.GetTable();
            DynamicDataManager1.RegisterControl(DetailsView1);
            DynamicDataFutures.DisablePartialRenderingForUpload(this, table);

            DetailsView1.RowsGenerator = new AdvancedFieldGenerator(table, false);
            DynamicDataFutures.RegisterInsertDefaults(DetailsDataSource, DetailsView1, false /* hide defaults */);
        }

        protected void Page_Load(object sender, EventArgs e) {
            Title = table.DisplayName;
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
