using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Net;
using System.Web;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Microsoft.Web.Test {
    [TestClass]
    public class ImageHandlerTest {
        private StubImageHandler _imageHandler;
        private Mock<StubImageHandler> _imageHandlerMock;
        private Mock<HttpContextBase> _contextMock;
        private Mock<HttpResponseBase> _responseMock;
        private Mock<HttpCachePolicyBase> _cachePolicyMock;
        private DateTime _now;
        private Mock<StubImageStore> _imageStoreMock;

        [TestInitialize]
        public void SetUp() {
            _imageStoreMock = new Mock<StubImageStore>();

            _now = DateTime.Now;
            _imageHandlerMock = new Mock<StubImageHandler>(_imageStoreMock.Object, _now);
            _imageHandler = _imageHandlerMock.Object;

            var requestMock = new Mock<HttpRequestBase>();
            requestMock.ExpectGet(r => r.QueryString).Returns(new NameValueCollection());

            _cachePolicyMock = new Mock<HttpCachePolicyBase>();

            _responseMock = new Mock<HttpResponseBase>();
            _responseMock.ExpectGet(r => r.Cache).Returns(_cachePolicyMock.Object);
            _responseMock.ExpectGet(r => r.OutputStream).Returns(new MemoryStream());

            _contextMock = new Mock<HttpContextBase>();
            _contextMock.ExpectGet(c => c.Request).Returns(requestMock.Object);
            _contextMock.ExpectGet(c => c.Response).Returns(_responseMock.Object);
        }

        [TestCleanup]
        public void TearDown() {
            _imageHandler = null;
            _imageHandlerMock = null;
            _imageStoreMock = null;
            _cachePolicyMock = null;
            _contextMock = null;
            _responseMock = null;
        }

        [TestMethod]
        public void ClientCacheExpiration() {
            Assert.AreEqual(new TimeSpan(0, 10, 0), _imageHandler.ClientCacheExpiration);
        }

        [TestMethod]
        public void ClientCacheExpiration_EnablesClientCache() {
            _imageHandler.ClientCacheExpiration = new TimeSpan(1, 0, 0);

            Assert.AreEqual(true, _imageHandler.EnableClientCache);
            Assert.AreEqual(new TimeSpan(1, 0, 0), _imageHandler.ClientCacheExpiration);
        }

        [TestMethod]
        public void ClientCacheExpiration_NegativeTimeSpan() {
            ExceptionHelper.ExpectArgumentOutOfRangeException(delegate() {
                _imageHandler.ClientCacheExpiration = new TimeSpan(-10, 0, 0);
            }, "value", null);
        }

        [TestMethod]
        public void ContentType() {
            Assert.AreEqual(ImageFormat.Jpeg, _imageHandler.ContentType);

            _imageHandler.ContentType = ImageFormat.Gif;
            Assert.AreEqual(ImageFormat.Gif, _imageHandler.ContentType);
        }

        [TestMethod]
        public void EnableClientCache() {
            Assert.AreEqual(false, _imageHandler.EnableClientCache);

            _imageHandler.EnableClientCache = true;
            Assert.AreEqual(true, _imageHandler.EnableClientCache);
        }

        [TestMethod]
        public void EnableServerCache() {
            Assert.AreEqual(false, _imageHandler.EnableServerCache);

            _imageHandler.EnableServerCache = true;
            Assert.AreEqual(true, _imageHandler.EnableServerCache);
        }

        [TestMethod]
        public void ImageTransforms() {
            Assert.IsNotNull(_imageHandler.ImageTransforms);

            _imageHandler.ImageTransforms.Add(null);
            Assert.AreEqual(1, _imageHandler.ImageTransforms.Count);
        }

        [TestMethod]
        public void IsReusable() {
            Assert.AreEqual(false, new StubImageHandler().IsReusable);
        }

        [TestMethod]
        public void GetImageMimeType() {
            Assert.AreEqual("image/jpeg", ImageHandlerInternal.GetImageMimeType(ImageFormat.Jpeg));
            Assert.AreEqual("image/gif", ImageHandlerInternal.GetImageMimeType(ImageFormat.Gif));
            Assert.AreEqual("image/png", ImageHandlerInternal.GetImageMimeType(ImageFormat.Png));
            Assert.AreEqual("image/bmp", ImageHandlerInternal.GetImageMimeType(ImageFormat.Bmp));
            Assert.AreEqual("image/bmp", ImageHandlerInternal.GetImageMimeType(ImageFormat.MemoryBmp));
            Assert.AreEqual("image/tiff", ImageHandlerInternal.GetImageMimeType(ImageFormat.Tiff));
            Assert.AreEqual("image/x-icon", ImageHandlerInternal.GetImageMimeType(ImageFormat.Icon));

            Assert.AreEqual("image/x-unknown", ImageHandlerInternal.GetImageMimeType(ImageFormat.Emf));
            Assert.AreEqual("image/x-unknown", ImageHandlerInternal.GetImageMimeType(ImageFormat.Exif));
            Assert.AreEqual("image/x-unknown", ImageHandlerInternal.GetImageMimeType(ImageFormat.Wmf));

            Assert.AreEqual("image/x-unknown", ImageHandlerInternal.GetImageMimeType(new ImageFormat(new Guid())));
        }

        [TestMethod]
        public void ProcessResuest_NullContext() {
            ExceptionHelper.ExpectArgumentNullException(delegate() {
                new StubImageHandler().ProcessRequest((HttpContext)null);
            }, "context");
        }

        [TestMethod]
        public void ProcessRequest_CallbackReturnsNull() {
            _imageHandlerMock.Expect(h => h.GenerateImage(It.IsAny<NameValueCollection>())).Returns((ImageInfo)null);
            ExceptionHelper.ExpectException<InvalidOperationException>(delegate() {
                _imageHandler.ProcessRequest(_contextMock.Object);
            }, "The image generation handler cannot return null.");
        }

        [TestMethod]
        public void ProcessRequest_ClientCacheSet() {
            _imageHandlerMock.Expect(h => h.GenerateImage(It.IsAny<NameValueCollection>())).Returns(new ImageInfo(CreateDummyImage()));
            _imageHandler.EnableClientCache = true;
            _imageHandler.ClientCacheExpiration = new TimeSpan(1, 2, 3);

            _cachePolicyMock.Expect(cp => cp.SetCacheability(HttpCacheability.Public)).Verifiable();
            _cachePolicyMock.Expect(cp => cp.SetExpires(_now + new TimeSpan(1, 2, 3))).Verifiable();

            _imageHandler.ProcessRequest(_contextMock.Object);

            _cachePolicyMock.Verify();
        }

        [TestMethod]
        public void ProcessRequest_ServerCacheOff() {
            _imageHandlerMock.Expect(h => h.GenerateImage(It.IsAny<NameValueCollection>())).Returns(new ImageInfo(CreateDummyImage())).AtMostOnce().Verifiable();

            _imageStoreMock.Expect(ims => ims.Add(It.IsAny<string>(), It.IsAny<byte[]>())).Throws<Exception>();
            _imageStoreMock.Expect(ims => ims.TryTransmitIfContains(It.IsAny<string>(), It.IsAny<HttpResponseBase>())).Throws<Exception>();
            
            _imageHandler.ProcessRequest(_contextMock.Object);
            _imageHandlerMock.Verify();
        }

        [TestMethod]
        public void ProcessRequest_ServerCacheOn_ImageInCache() {
            _imageHandler.EnableServerCache = true;
            _imageHandlerMock.Expect(h => h.GenerateImage(It.IsAny<NameValueCollection>())).Throws<Exception>();

            _imageStoreMock.Expect(ims => ims.Add(It.IsAny<string>(), It.IsAny<byte[]>())).Throws<Exception>();
            _imageStoreMock.Expect(ims => ims.TryTransmitIfContains(It.IsAny<string>(), It.IsAny<HttpResponseBase>())).Returns(true).AtMostOnce().Verifiable();

            _imageHandler.ProcessRequest(_contextMock.Object);

            _imageStoreMock.Verify();
        }

        [TestMethod]
        public void ProcessRequest_ServerCacheOn_ImageNotInCache() {
            _imageHandler.EnableServerCache = true;
            _imageHandlerMock.Expect(h => h.GenerateImage(It.IsAny<NameValueCollection>())).Returns(new ImageInfo(CreateDummyImageBytes())).AtMostOnce().Verifiable();

            _responseMock.Expect(r => r.End()).AtMostOnce().Verifiable();

            _imageStoreMock.Expect(ims => ims.Add(It.IsAny<string>(), It.IsAny<byte[]>())).AtMostOnce().Verifiable();
            _imageStoreMock.Expect(ims => ims.TryTransmitIfContains(It.IsAny<string>(), It.IsAny<HttpResponseBase>())).Returns(false);

            _imageHandler.ProcessRequest(_contextMock.Object);

            _imageHandlerMock.Verify();
            _responseMock.Verify();
            _imageStoreMock.Verify();
        }

        [TestMethod]
        public void ProcessRequest_HttpStatusCodeReturned() {
            _imageHandlerMock.Expect(h => h.GenerateImage(It.IsAny<NameValueCollection>())).Returns(new ImageInfo(HttpStatusCode.Forbidden)).AtMostOnce().Verifiable();

            _responseMock.ExpectSet(r => r.StatusCode).Callback(delegate(int value) {
                Assert.AreEqual((int)HttpStatusCode.Forbidden, value);
            }).AtMostOnce().Verifiable();
            _responseMock.Expect(r => r.End()).AtMostOnce().Verifiable();

            _imageHandler.ProcessRequest(_contextMock.Object);

            _imageHandlerMock.Verify();
            _responseMock.Verify();
        }

        private Image CreateDummyImage() {
            return new Bitmap(1, 1);
        }

        private byte[] CreateDummyImageBytes() {
            MemoryStream m = new MemoryStream();
            CreateDummyImage().Save(m, ImageFormat.Gif);
            return m.GetBuffer();
        }

        public abstract class StubImageStore : IImageStore {
            public abstract void Add(string id, byte[] data);

            public abstract bool TryTransmitIfContains(string id, HttpResponseBase response);
        }

        public class StubImageHandler : ImageHandler {
            public StubImageHandler() { }

            public StubImageHandler(StubImageStore imageStore, DateTime now)
                : base(imageStore, now) { }

            public new List<ImageTransform> ImageTransforms {
                get {
                    return base.ImageTransforms;
                }
            }

            public override ImageInfo GenerateImage(NameValueCollection parameters) {
                throw new NotImplementedException();
            }
        }
    }
}
