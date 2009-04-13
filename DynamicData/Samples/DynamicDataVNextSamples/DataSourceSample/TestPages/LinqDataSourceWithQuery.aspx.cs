using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.Web.Data.UI.WebControls.Expressions;

namespace DataSourcesDemo {
    public partial class LinqDataSourceWithQuery : System.Web.UI.Page {
        protected void Page_Load(object sender, EventArgs e) {

        }

        protected void FilterProducts(object sender, CustomExpressionEventArgs e) {
            e.Query = from p in e.Query.Cast<Product>()
                      where p.UnitPrice >= 10
                      select p;
        }
    }
}
