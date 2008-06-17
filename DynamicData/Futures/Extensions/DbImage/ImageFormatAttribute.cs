using System;

namespace Microsoft.Web.DynamicData.Extensions {

    public class ImageFormatAttribute : Attribute {
        private int _displayWidth;
        private int _displayHeight;
        private bool _displayInEdit = true;

        public ImageFormatAttribute(int displaywidth, int displayheight) {
            _displayWidth = displaywidth;
            _displayHeight = displayheight;
        }

        public bool DisplayInEdit {
            get { return _displayInEdit; }
            set { _displayInEdit = value; }
        }

        public int DisplayWidth {
            get { return _displayWidth; }
        }

        public int DisplayHeight {
            get { return _displayHeight; }
        }

    }

}