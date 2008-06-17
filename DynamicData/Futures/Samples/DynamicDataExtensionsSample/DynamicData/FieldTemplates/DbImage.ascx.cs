using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Web.DynamicData;
using Microsoft.Web.DynamicData.Extensions;

namespace DynamicDataExtensionsSample {
    public partial class DbImage : FieldTemplateUserControl {
        private int _ImageWidth = -1;
        private int _ImageHeight = -1;

        public int ImageWidth {
            get { return _ImageWidth; }
            set { _ImageWidth = value; }
        }

        public int ImageHeight {
            get { return _ImageHeight; }
            set { _ImageHeight = value; }
        }

        protected override void OnDataBinding(EventArgs e) {
            base.OnDataBinding(e);

            //check if image exists
            if (this.FieldValue == null) {
                return;
            }

            //metadata
            int width = 100;
            int height = 100;
            var metadata = MetadataAttributes.OfType<ImageFormatAttribute>().FirstOrDefault();
            if (metadata != null) {
                width = metadata.DisplayWidth;
                height = metadata.DisplayHeight;
            }
            if (ImageWidth != -1) {
                width = ImageWidth;
            }
            if (ImageHeight != -1) {
                height = ImageHeight;
            }

            //build url
            string tablename = Column.Table.Name;
            string primarykeyname = Column.Table.PrimaryKeyColumns[0].Name;
            string primarykeyvalue = DataBinder.GetPropertyValue(Row, primarykeyname).ToString();

            //set image properties
            Image1.Visible = true;
            Image1.ImageUrl = "~/ImageHandler.ashx?table=" + tablename + "&column=" + Column.Name + "&pkv=" + primarykeyvalue + "&width=" + width.ToString() + "&height=" + height.ToString();
        }
    }
}