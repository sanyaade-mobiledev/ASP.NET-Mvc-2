using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace Microsoft.Web {
    /// <summary>
    /// An abstract ImageTransform class
    /// </summary>
    public abstract class ImageTransform {
        public abstract Image ProcessImage(Image image);
        
        // REVIEW: should this property be abstract?
        [Browsable(false)]
        public virtual string UniqueString {
            get {
                return GetType().FullName;
            }
        }
    }

    public enum ImageResizeMode {
        /// <summary>
        /// Fit mode maintains the aspect ratio of the original image while ensuring that the dimensions of the result
        /// do not exceed the maximum values for the resize transformation.
        /// </summary>
        Fit,
        /// <summary>
        /// Crop resizes the image and removes parts of it to ensure that the dimensions of the result are exactly 
        /// as specified by the transformation.
        /// </summary>
        Crop
    }

    public class ImageResizeTransform : ImageTransform {
        private int _width, _height;

        /// <summary>
        /// Sets the resize mode. The default value is Fit.
        /// </summary>
        [DefaultValue(ImageResizeMode.Fit)]
        [Category("Behavior")]
        public ImageResizeMode Mode { get; set; }
        
        /// <summary>
        /// Sets the maximum width of the resulting image
        /// </summary>
        [DefaultValue(0)]
        [Category("Behavior")]
        public int Width {
            get {
                return _width;
            }
            set {
                CheckValue(value);
                _width = value;
            }
        }

        /// <summary>
        /// Sets the maximum height of the resulting image
        /// </summary>
        [DefaultValue(0)]
        [Category("Behavior")]
        public int Height {
            get {
                return _height;
            }
            set {
                CheckValue(value);
                _height = value;
            }
        }

        /// <summary>
        /// Sets the interpolation mode used for resizing images. The default is HighQualityBicubic.
        /// </summary>
        [DefaultValue(InterpolationMode.HighQualityBicubic)]
        [Category("Behavior")]
        public InterpolationMode InterpolationMode { get; set; }

        public ImageResizeTransform() {
            InterpolationMode = InterpolationMode.HighQualityBicubic;
            Mode = ImageResizeMode.Fit;
        }

        private static void CheckValue(int value) {
            if (value < 0) {
                throw new ArgumentOutOfRangeException("value");
            }
        }

        public override Image ProcessImage(Image img) {
            int scaledHeight = (int)(img.Height * ((float)this.Width / (float)img.Width));
            int scaledWidth = (int)(img.Width * ((float)this.Height / (float)img.Height));

            switch (Mode) {
                case ImageResizeMode.Fit:
                    return FitImage(img, scaledHeight, scaledWidth);
                case ImageResizeMode.Crop:
                    return CropImage(img, scaledHeight, scaledWidth);
                default:
                    Debug.Fail("Should not reach this");
                    return null;
            }
        }

        private Image FitImage(Image img, int scaled_height, int scaled_width) {
            int resizeWidth = 0;
            int resizeHeight = 0;
            if (this.Height == 0) {
                resizeWidth = this.Width;
                resizeHeight = scaled_height;
            }
            else if (this.Width == 0) {
                resizeWidth = scaled_width;
                resizeHeight = this.Height;
            }
            else {
                if (((float)this.Width / (float)img.Width < this.Height / (float)img.Height)) {
                    resizeWidth = this.Width;
                    resizeHeight = scaled_height;
                }
                else {
                    resizeWidth = scaled_width;
                    resizeHeight = this.Height;
                }
            }

            Bitmap newimage = new Bitmap(resizeWidth, resizeHeight);
            Graphics gra = Graphics.FromImage(newimage);
            SetupGraphics(gra);
            gra.DrawImage(img, 0, 0, resizeWidth, resizeHeight);
            return newimage;
        }

        private Image CropImage(Image img, int scaledHeight, int scaledWidth) {
            int resizeWidth = 0;
            int resizeHeight = 0;
            if (((float)this.Width / (float)img.Width > this.Height / (float)img.Height)) {
                resizeWidth = this.Width;
                resizeHeight = scaledHeight;
            }
            else {
                resizeWidth = scaledWidth;
                resizeHeight = this.Height;
            }

            Bitmap newImage = new Bitmap(this.Width, this.Height);
            Graphics graphics = Graphics.FromImage(newImage);
            SetupGraphics(graphics);
            graphics.DrawImage(img, (this.Width - resizeWidth) / 2, (this.Height - resizeHeight) / 2, resizeWidth, resizeHeight);
            return newImage;
        }

        private void SetupGraphics(Graphics graphics) {
            graphics.CompositingMode = CompositingMode.SourceCopy;
            graphics.CompositingQuality = CompositingQuality.HighSpeed;
            graphics.InterpolationMode = InterpolationMode;
        }

        [Browsable(false)]
        public override string UniqueString {
            get {
                return base.UniqueString + Width + InterpolationMode.ToString() + Height + Mode.ToString();
            }
        }

        public override string ToString() {
            return "ImageResizeTransform";
        }
    }

    public class ImageTransformCollectionEditor : CollectionEditor {
        private static readonly Type[] s_newItemTypes = new Type[] {
            typeof(ImageResizeTransform)
        };

        public ImageTransformCollectionEditor(Type type)
            : base(type) {
        }

        protected override Type[] CreateNewItemTypes() {
            return s_newItemTypes;
        }

        protected override Type CreateCollectionItemType() {
            return typeof(ImageTransform);
        }

        private sealed class ImageTransform : Microsoft.Web.ImageTransform {

            public override Image ProcessImage(Image image) {
                throw new NotImplementedException();
            }

            public int DummyProperty {
                get {
                    Debug.Fail("Should not be called");
                    return 1;
                }
                set {
                    Debug.Fail("Should not be called");
                }
            }
        }
    }


}
