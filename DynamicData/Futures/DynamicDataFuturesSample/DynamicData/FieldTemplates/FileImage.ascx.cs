using System;
using System.Linq;
using System.Web.DynamicData;
using System.Web.UI;
using Microsoft.Web.DynamicData;

namespace DynamicDataFuturesSample {
    public partial class FileImage : FieldTemplateUserControl {
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

        public override Control DataControl {
            get {
                return Image1;
            }
        }

        protected override void OnDataBinding(EventArgs e) {
            base.OnDataBinding(e);

            //check if image exists
            if (FieldValue == null) {
                return;
            }

            //format image url
            string url;
            var metadata = MetadataAttributes.OfType<ImageUrlAttribute>().FirstOrDefault();
            if (metadata != null) {
                url = String.Format(metadata.UrlFormat, FieldValueString);
            } else {
                url = FieldValueString;
            }
            Image1.ImageUrl = url;

            int width = -1;
            int height = -1;
            var imageFormat = MetadataAttributes.OfType<ImageFormatAttribute>().FirstOrDefault();
            if (imageFormat != null) {
                width = imageFormat.DisplayWidth;
                height = imageFormat.DisplayHeight;
            }

            if (ImageWidth != -1) {
                Image1.Width = ImageWidth;
            } else if (width != -1) {
                Image1.Width = width;
            }

            if (ImageHeight != -1) {
                Image1.Height = ImageHeight;
            } else if (height != -1) {
                Image1.Height = height;
            }
        }
    }
}
