using System;
using Microsoft.Web.DynamicData.Extensions;
using System.ComponentModel;

namespace DynamicDataExtensionsSample {
    public partial class SimpleDynamicDataSourceTest : System.Web.UI.Page {

        protected void Page_Init(object sender, EventArgs e) {
            var o = new MyTestClass() { MyString = "Hello", MyInteger=45, MySecondInteger=101, MyDateTime = DateTime.Now };

            DataSource1.SetDataObject(o, DetailsView1);
            DataSource1.Complete += new EventHandler<SimpleDynamicDataSourceCompleteEventArgs>(LinqDataSource1_Complete);
        }

        void LinqDataSource1_Complete(object sender, SimpleDynamicDataSourceCompleteEventArgs e) {
            var newObject = (MyTestClass)e.NewObject;

            DetailsView2.DataSource = new object[] { newObject };
            DetailsView2.DataBind();
        }
    }
}
