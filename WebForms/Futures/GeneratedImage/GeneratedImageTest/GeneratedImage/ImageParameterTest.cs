using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Web.Test {
    [TestClass()]
    public class ImageParameterTest {
        [TestMethod()]
        public void DataBind() {
            ImageParameter target = new ImageParameter();
            bool dataBound = false;
            target.DataBinding += new EventHandler(delegate(object sender, EventArgs e) {
                dataBound = true;
            });
            target.DataBind();
            Assert.AreEqual(true, dataBound);
        }
    }
}
