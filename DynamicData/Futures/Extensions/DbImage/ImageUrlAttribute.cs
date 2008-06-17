using System;

namespace Microsoft.Web.DynamicData.Extensions {

    public class ImageUrlAttribute : Attribute {

        public ImageUrlAttribute() {
        }

        public ImageUrlAttribute(string urlformat) {
            UrlFormat = urlformat;
        }

        public string UrlFormat {
            get; set; 
        }

    }

}