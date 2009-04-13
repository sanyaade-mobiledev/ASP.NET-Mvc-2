using System;
using Microsoft.Web.DynamicData;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.UI;

namespace DynamicDataFuturesSample {
    public partial class SimpleDynamicDataSourceTestNonCLR : System.Web.UI.Page {

        protected void Page_Init(object sender, EventArgs e) {
            var o = new CustomObject();

            o.Properties.Add(new CustomPropertyDescriptor(
                "SomeString",
                typeof(string),
                null,
                "Hello" ));

            o.Properties.Add(new CustomPropertyDescriptor(
                "SomeInt",
                typeof(int),
                new Attribute[] {
                    new RangeAttribute(0, 100) },
                34));

            o.Properties.Add(new CustomPropertyDescriptor(
                "SomeDate",
                typeof(DateTime),
                new Attribute[] {
                    new DataTypeAttribute(DataType.Date) },
                DateTime.Now));

            DataSource1.SetDataObject(o, DetailsView1);
            DataSource1.Complete += new EventHandler<SimpleDynamicDataSourceCompleteEventArgs>(LinqDataSource1_Complete);
        }

        void LinqDataSource1_Complete(object sender, SimpleDynamicDataSourceCompleteEventArgs e) {
            var newObject = e.NewObject;

            //var someInt = DataBinder.GetPropertyValue(newObject, "SomeInt");

            DetailsView2.DataSource = new object[] { newObject };
            DetailsView2.DataBind();
        }

    }


}
