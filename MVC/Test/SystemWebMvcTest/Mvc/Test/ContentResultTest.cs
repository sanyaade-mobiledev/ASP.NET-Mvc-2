namespace System.Web.Mvc.Test {
    using System.Linq.Expressions;
    using System.Reflection;
    using System.Text;
    using System.Web;
    using System.Web.Routing;
    using System.Web.TestUtil;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using Moq.Language.Flow;

    [TestClass]
    public class ContentResultTest {

        [TestMethod]
        public void AllPropertiesDefaultToNull() {
            // Setup & execute
            ContentResult result = new ContentResult();

            // Verify
            Assert.IsNull(result.Content);
            Assert.IsNull(result.ContentEncoding);
            Assert.IsNull(result.ContentType);
        }

        [TestMethod]
        public void EmptyContentTypeIsNotOutput() {
            // Setup
            string content = "Some content.";
            Encoding contentEncoding = Encoding.UTF8;
            Mock<HttpResponseBase> mockResponse = new Mock<HttpResponseBase>(MockBehavior.Strict);

            // Setup expectations
            mockResponse.ExpectSetProperty(response => response.ContentEncoding, contentEncoding).Verifiable();
            mockResponse.Expect(response => response.Write(content)).Verifiable();

            ControllerContext controllerContext = GetControllerContext(mockResponse.Object);
            ContentResult result = new ContentResult {
                Content = content,
                ContentType = String.Empty,
                ContentEncoding = contentEncoding
            };

            // Execute
            result.ExecuteResult(controllerContext);

            // Verify
            mockResponse.Verify();
        }

        [TestMethod]
        public void ExecuteResult() {
            // Setup
            string content = "Some content.";
            string contentType = "Some content type.";
            Encoding contentEncoding = Encoding.UTF8;
            Mock<HttpResponseBase> mockResponse = new Mock<HttpResponseBase>(MockBehavior.Strict);
            
            // Setup expectations
            mockResponse.ExpectSetProperty(response => response.ContentType, contentType).Verifiable();
            mockResponse.ExpectSetProperty(response => response.ContentEncoding, contentEncoding).Verifiable();
            mockResponse.Expect(response => response.Write(content)).Verifiable();

            ControllerContext controllerContext = GetControllerContext(mockResponse.Object);
            ContentResult result = new ContentResult {
                Content = content,
                ContentType = contentType,
                ContentEncoding = contentEncoding
            };

            // Execute
            result.ExecuteResult(controllerContext);

            // Verify
            mockResponse.Verify();
        }

        [TestMethod]
        public void ExecuteResultWithNullContextThrows() {
            ExceptionHelper.ExpectArgumentNullException(
                delegate {
                    new ContentResult().ExecuteResult(null /* context */);
                }, "context");
        }

        [TestMethod]
        public void NullContentIsNotOutput() {
            // Setup
            string contentType = "Some content type.";
            Encoding contentEncoding = Encoding.UTF8;
            Mock<HttpResponseBase> mockResponse = new Mock<HttpResponseBase>(MockBehavior.Strict);

            // Setup expectations
            mockResponse.ExpectSetProperty(response => response.ContentType, contentType).Verifiable();
            mockResponse.ExpectSetProperty(response => response.ContentEncoding, contentEncoding).Verifiable();

            ControllerContext controllerContext = GetControllerContext(mockResponse.Object);
            ContentResult result = new ContentResult {
                ContentType = contentType,
                ContentEncoding = contentEncoding
            };

            // Execute
            result.ExecuteResult(controllerContext);

            // Verify
            mockResponse.Verify();
        }

        [TestMethod]
        public void NullContentEncodingIsNotOutput() {
            // Setup
            string content = "Some content.";
            string contentType = "Some content type.";
            Mock<HttpResponseBase> mockResponse = new Mock<HttpResponseBase>(MockBehavior.Strict);

            // Setup expectations
            mockResponse.ExpectSetProperty(response => response.ContentType, contentType).Verifiable();
            mockResponse.Expect(response => response.Write(content)).Verifiable();

            ControllerContext controllerContext = GetControllerContext(mockResponse.Object);
            ContentResult result = new ContentResult {
                Content = content,
                ContentType = contentType,
            };

            // Execute
            result.ExecuteResult(controllerContext);

            // Verify
            mockResponse.Verify();
        }

        [TestMethod]
        public void NullContentTypeIsNotOutput() {
            // Setup
            string content = "Some content.";
            Encoding contentEncoding = Encoding.UTF8;
            Mock<HttpResponseBase> mockResponse = new Mock<HttpResponseBase>(MockBehavior.Strict);

            // Setup expectations
            mockResponse.ExpectSetProperty(response => response.ContentEncoding, contentEncoding).Verifiable();
            mockResponse.Expect(response => response.Write(content)).Verifiable();

            ControllerContext controllerContext = GetControllerContext(mockResponse.Object);
            ContentResult result = new ContentResult {
                Content = content,
                ContentEncoding = contentEncoding
            };

            // Execute
            result.ExecuteResult(controllerContext);

            // Verify
            mockResponse.Verify();
        }

        private static ControllerContext GetControllerContext(HttpResponseBase httpResponse) {
            Mock<HttpContextBase> httpContext = new Mock<HttpContextBase>();
            httpContext.Expect(ctx => ctx.Response).Returns(httpResponse);
            return new ControllerContext(httpContext.Object, new RouteData(), new Mock<IController>().Object);
        }
    }

    internal static class MoqHelpers {
        // this is only requires until Moq can support property setters
        public static IExpect ExpectSetProperty<T, TResult>(this Mock<T> mock, Expression<Func<T, TResult>> property, TResult value) where T : class {
            // get the property info
            var oldLambdaExpr = property as LambdaExpression;
            var memberExpr = oldLambdaExpr.Body as MemberExpression;
            var propInfo = memberExpr.Member as PropertyInfo;

            // now gen a call to the setter
            var setter = propInfo.GetSetMethod();
            var paramExpr = Expression.Parameter(typeof(T), null);
            var newCallExpr = Expression.Call(paramExpr, setter, Expression.Constant(value, typeof(TResult)));
            var newLambdaExpr = Expression.Lambda<Action<T>>(newCallExpr, paramExpr);
            return mock.Expect(newLambdaExpr);
        }
    }
}
