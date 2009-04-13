using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.Web.Data.UI.WebControls;
using DataSourcesDemo.Entities;

namespace DataSourcesDemo {
    public partial class WithQuery : System.Web.UI.Page {
        public void Page_Load() {
            
        }

        public static IQueryable DoSomeStuff(IQueryable<Products> products, int reorderLevel) {
            return products.Where(p => p.ReorderLevel >= reorderLevel);
        }
    }
}
