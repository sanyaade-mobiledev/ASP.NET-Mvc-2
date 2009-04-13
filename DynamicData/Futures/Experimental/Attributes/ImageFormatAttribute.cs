using System;

namespace Microsoft.Web.DynamicData {
    public class ImageFormatAttribute : Attribute {
        public bool DisplayInEdit {
            get;
            set;
        }

        public int DisplayWidth {
            get;
            private set;
        }

        public int DisplayHeight {
            get;
            private set;
        }

        public ImageFormatAttribute(int displayWidth, int displayHeight) {
            DisplayWidth = displayWidth;
            DisplayHeight = displayHeight;
            DisplayInEdit = true;
        }
    }
}