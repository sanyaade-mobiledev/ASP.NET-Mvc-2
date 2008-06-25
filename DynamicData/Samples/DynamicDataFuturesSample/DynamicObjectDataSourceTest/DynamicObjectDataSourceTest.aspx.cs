using System;
using Microsoft.Web.DynamicData;
using System.Web.DynamicData;

namespace DynamicDataFuturesSample.DynamicObjectDataSourceTest {
    public partial class DynamicObjectDataSourceTest : System.Web.UI.Page {
        protected void Page_Init(object sender, EventArgs e) {

            // Set our row generator on the details view so it creates
            // DynamicField instead of the standard BoundFields
            MetaTable table = ObjectDataSource1.GetTable();
            DetailsView1.RowsGenerator = new AdvancedFieldGenerator(table, false);
        }
    }
}
