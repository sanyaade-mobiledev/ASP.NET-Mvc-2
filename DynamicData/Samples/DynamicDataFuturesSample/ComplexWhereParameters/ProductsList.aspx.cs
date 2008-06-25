using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.DynamicData;
using System.Text;
using Microsoft.Web.DynamicData;

namespace DynamicDataFuturesSample {
    public partial class ProductsList : System.Web.UI.Page {
        protected MetaTable table;

        protected void Page_Init(object sender, EventArgs e) {
            DynamicDataManager1.RegisterControl(GridView1, true /*setSelectionFromUrl*/);
            // add event handler to allow for modifying the WHERE clause before the select is executed
            GridDataSource.Selecting += new EventHandler<LinqDataSourceSelectEventArgs>(GridDataSource_Selecting);
        }

        protected void Page_Load(object sender, EventArgs e) {

            table = GridDataSource.GetTable();
            Title = table.DisplayName;

            InsertHyperLink.NavigateUrl = table.GetActionPath(PageAction.Insert);

            // Disable various options if the table is readonly
            if (table.IsReadOnly) {
                GridView1.Columns[0].Visible = false;
                InsertHyperLink.Visible = false;
            }
        }

        void GridDataSource_Selecting(object sender, LinqDataSourceSelectEventArgs e) {
            LinqDataSourceView dataSourceView = (LinqDataSourceView)sender;

            dataSourceView.Where = DynamicDataFutures.GetWhereClauseWithDynamicDataParameters(dataSourceView, e);
        }

        protected void OnFilterSelectedIndexChanged(object sender, EventArgs e) {
            GridView1.PageIndex = 0;
        }
    }
}
