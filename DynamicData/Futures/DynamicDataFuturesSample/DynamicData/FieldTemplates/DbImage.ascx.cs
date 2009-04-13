using System;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.DynamicData;
using System.Web.UI;
using Microsoft.Web.DynamicData;

namespace DynamicDataFuturesSample {
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

            ////check if image exists
            //if (this.FieldValue == null) {
            //    return;
            //}

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
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat(
                "~/ImageHandler.axd?table={0}&column={1}&width={2}&height={3}",
                HttpUtility.UrlEncode(Column.Table.Name),
                HttpUtility.UrlEncode(Column.Name),
                width,
                height
            );
            for (int i = 0; i < Column.Table.PrimaryKeyColumns.Count; i++) {
                sb.AppendFormat("&pk{0}={1}", i, DataBinder.GetPropertyValue(Row, Column.Table.PrimaryKeyColumns[i].Name));
            }

            //set image properties
            Image1.ImageUrl = sb.ToString();
        }
    }
}