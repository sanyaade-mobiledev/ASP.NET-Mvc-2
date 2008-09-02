namespace System.Web.Mvc.Test {
    using System;
    using System.Collections.Specialized;
    using System.ComponentModel;
    using System.Globalization;
    using System.Threading;
    using System.Web.Routing;
    using System.Web.TestUtil;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;

    [TestClass]
    public class DefaultModelBinderTest {

        [TestMethod]
        public void ConvertTypeCanConvertArraysToArrays() {
            // Arrange
            int[] original = new int[] { 1, 20, 42 };
            DefaultModelBinder converter = new DefaultModelBinder();

            // Act
            string[] converted = (string[])converter.ConvertType(CultureInfo.InvariantCulture, original, typeof(string[]));

            // Assert
            Assert.IsNotNull(converted);
            Assert.AreEqual(3, converted.Length);
            Assert.AreEqual("1", converted[0]);
            Assert.AreEqual("20", converted[1]);
            Assert.AreEqual("42", converted[2]);
        }

        [TestMethod]
        public void ConvertTypeCanConvertArraysToSingleElements() {
            // Arrange
            int[] original = new int[] { 1, 20, 42 };
            DefaultModelBinder converter = new DefaultModelBinder();

            // Act
            string converted = (string)converter.ConvertType(CultureInfo.InvariantCulture, original, typeof(string));

            // Assert
            Assert.AreEqual("1", converted);
        }

        [TestMethod]
        public void ConvertTypeCanConvertSingleElementsToArrays() {
            // Arrange
            int original = 42;
            DefaultModelBinder converter = new DefaultModelBinder();

            // Act
            string[] converted = (string[])converter.ConvertType(CultureInfo.InvariantCulture, original, typeof(string[]));

            // Assert
            Assert.IsNotNull(converted);
            Assert.AreEqual(1, converted.Length);
            Assert.AreEqual("42", converted[0]);
        }

        [TestMethod]
        public void ConvertTypeChecksCanConvertFrom() {
            // Arrange
            object valueToConvert = "someValue";
            DefaultModelBinder converter = new DefaultModelBinder();

            // Act
            StringContainer returned = (StringContainer)converter.ConvertType(CultureInfo.InvariantCulture, valueToConvert, typeof(StringContainer));

            // Assert
            Assert.AreEqual(returned.Value, "someValue (IVL)");
        }

        [TestMethod]
        public void ConvertTypeChecksCanConvertTo() {
            // Arrange
            object valueToConvert = new StringContainer("someValue");
            DefaultModelBinder converter = new DefaultModelBinder();

            // Act
            string returned = (string)converter.ConvertType(CultureInfo.InvariantCulture, valueToConvert, typeof(string));

            // Assert
            Assert.AreEqual(returned, "someValue (IVL)");
        }

        [TestMethod]
        public void ConvertTypeUsesCurrentCultureIfCultureNotSpecified() {
            // Arrange
            object valueToConvert = "someValue";
            DefaultModelBinder converter = new DefaultModelBinder();
            StringContainer returned;

            // Act
            using (ReplaceCurrentCulture("fr-FR")) {
                returned = (StringContainer)converter.ConvertType(null, valueToConvert, typeof(StringContainer));
            }

            // Assert
            Assert.AreEqual(returned.Value, "someValue (fr-FR)");
        }

        [TestMethod]
        public void ConvertTypeReturnsValueIfInstanceOfDestinationType() {
            // Arrange
            DefaultModelBinder converter = new DefaultModelBinder();

            // Act
            object outValue = converter.ConvertType(CultureInfo.InvariantCulture, "some string", typeof(object));

            // Assert
            Assert.AreEqual("some string", outValue);
        }

        [TestMethod]
        public void ConvertTypeReturnsValueIfNull() {
            // Arrange
            DefaultModelBinder converter = new DefaultModelBinder();

            // Act
            object outValue = converter.ConvertType(CultureInfo.InvariantCulture, null, typeof(int));

            // Assert
            Assert.IsNull(outValue);
        }

        [TestMethod]
        public void ConvertTypeThrowsIfConverterThrows() {
            // Arrange
            Type destinationType = typeof(StringContainer);
            DefaultModelBinder converter = new DefaultModelBinder();

            // Act & Assert
            // Will throw since the custom converter assumes the first 5 characters to be digits
            InvalidOperationException exception = ExceptionHelper.ExpectInvalidOperationException(
                delegate {
                    converter.ConvertType(CultureInfo.InvariantCulture, "", destinationType);
                },
                "The parameter conversion from type 'System.String' to type 'System.Web.Mvc.Test.DefaultModelBinderTest+StringContainer'"
                + " failed. See the inner exception for more information.");

            Exception innerException = exception.InnerException;
            Assert.AreEqual("Value cannot be null or empty.", innerException.Message);
        }

        [TestMethod]
        public void ConvertTypeThrowsIfDestinationTypeIsNull() {
            // Arrange
            DefaultModelBinder converter = new DefaultModelBinder();

            // Act & Assert
            ExceptionHelper.ExpectArgumentNullException(
                delegate {
                    converter.ConvertType(null, "value", null);
                }, "destinationType");
        }

        [TestMethod]
        public void ConvertTypeThrowsIfNoConverterExists() {
            // Arrange
            Type destinationType = typeof(MyClassWithoutConverter);
            DefaultModelBinder converter = new DefaultModelBinder();

            // Act & Assert
            ExceptionHelper.ExpectInvalidOperationException(
                delegate {
                    converter.ConvertType(CultureInfo.InvariantCulture, "some string", destinationType);
                },
                "The parameter conversion from type 'System.String' to type 'System.Web.Mvc.Test.DefaultModelBinderTest+MyClassWithoutConverter'"
                + " failed because no TypeConverter can convert between these types.");
        }

        [TestMethod]
        public void GetValueBubblesExceptionOnFailureIfModelStateParameterIsNull() {
            // Arrange
            ControllerContext controllerContext = GetControllerContext();
            DefaultModelBinder converter = new DefaultModelBinder();

            // Act & Assert
            ExceptionHelper.ExpectInvalidOperationException(
                delegate {
                    converter.GetValue(controllerContext, "foo", typeof(int), null);
                },
                "The parameter conversion from type 'System.String' to type 'System.Int32' failed. See the inner exception for more information.");
        }

        [TestMethod]
        public void GetValueChecksLocationsWithCorrectPrecedence() {
            // Should use this order of precedence:
            // 1. Values from the RouteData (could be from the typed-in URL or from the route's default values)
            // 2. Request query string
            // 3. Request form submission (which should be culture-aware)

            // Arrange
            ControllerContext controllerContext = GetControllerContext();
            DefaultModelBinder converter = new DefaultModelBinder();

            // Act
            StringContainer oFoo, oBar, oBaz;
            using (ReplaceCurrentCulture("fr-FR")) {
                // DevDiv 209949: Parameter conversion in Request.Form should be culture-aware
                oFoo = (StringContainer)converter.GetValue(controllerContext, "foo", typeof(StringContainer), null);
                oBar = (StringContainer)converter.GetValue(controllerContext, "bar", typeof(StringContainer), null);
                oBaz = (StringContainer)converter.GetValue(controllerContext, "baz", typeof(StringContainer), null);
            }

            // Assert
            Assert.AreEqual("fooRouteData (IVL)", oFoo.Value, "RouteData conversions should be culture-invariant.");
            Assert.AreEqual("barQuery (IVL)", oBar.Value, "Query conversions should be culture-invariant.");
            Assert.AreEqual("bazForm (fr-FR)", oBaz.Value, "Form conversions should be culture-aware.");
        }

        [TestMethod]
        public void GetValueThrowsIfControllerContextIsNull() {
            // Arrange
            DefaultModelBinder converter = new DefaultModelBinder();

            // Act & Assert
            ExceptionHelper.ExpectArgumentNullException(
                delegate {
                    converter.GetValue(null, "some name", typeof(object), null);
                }, "controllerContext");
        }

        [TestMethod]
        public void GetValueThrowsIfModelNameIsEmpty() {
            // Arrange
            DefaultModelBinder converter = new DefaultModelBinder();

            // Act & Assert
            ExceptionHelper.ExpectArgumentExceptionNullOrEmpty(
                delegate {
                    converter.GetValue(GetControllerContext(), "", typeof(object), null);
                }, "modelName");
        }

        [TestMethod]
        public void GetValueThrowsIfModelNameIsNull() {
            // Arrange
            DefaultModelBinder converter = new DefaultModelBinder();

            // Act & Assert
            ExceptionHelper.ExpectArgumentExceptionNullOrEmpty(
                delegate {
                    converter.GetValue(GetControllerContext(), null, typeof(object), null);
                }, "modelName");
        }

        [TestMethod]
        public void GetValueThrowsIfModelTypeIsNull() {
            // Arrange
            DefaultModelBinder converter = new DefaultModelBinder();

            // Act & Assert
            ExceptionHelper.ExpectArgumentNullException(
                delegate {
                    converter.GetValue(GetControllerContext(), "some name", null, null);
                }, "modelType");
        }

        [TestMethod]
        public void GetValueUpdatesModelStateIfProvidedOnFailure() {
            // Arrange
            ControllerContext controllerContext = GetControllerContext();
            DefaultModelBinder converter = new DefaultModelBinder();
            ModelStateDictionary modelStateDictionary = new ModelStateDictionary();

            // Act
            object oFoo = converter.GetValue(controllerContext, "foo", typeof(int), modelStateDictionary);
            object oBar = converter.GetValue(controllerContext, "bar", typeof(int), modelStateDictionary);
            object oBaz = converter.GetValue(controllerContext, "baz", typeof(int), modelStateDictionary);

            // Assert
            Assert.AreEqual(3, modelStateDictionary.Count);

            Assert.IsNull(oFoo);
            ModelState fooModelState = modelStateDictionary["foo"];
            Assert.AreEqual("fooRouteData", fooModelState.AttemptedValue);
            Assert.AreEqual(1, fooModelState.Errors.Count);
            Assert.IsNull(fooModelState.Errors[0].Exception, "Should not propagate the exception to ModelState since this might be information disclosure.");
            Assert.AreEqual("The value 'fooRouteData' is invalid for property 'foo'.", fooModelState.Errors[0].ErrorMessage);

            Assert.IsNull(oBar);
            ModelState barModelState = modelStateDictionary["bar"];
            Assert.AreEqual("barQuery", barModelState.AttemptedValue);
            Assert.AreEqual(1, barModelState.Errors.Count);
            Assert.IsNull(barModelState.Errors[0].Exception, "Should not propagate the exception to ModelState since this might be information disclosure.");
            Assert.AreEqual("The value 'barQuery' is invalid for property 'bar'.", barModelState.Errors[0].ErrorMessage);

            Assert.IsNull(oBaz);
            ModelState bazModelState = modelStateDictionary["baz"];
            Assert.AreEqual("bazForm", bazModelState.AttemptedValue);
            Assert.AreEqual(1, bazModelState.Errors.Count);
            Assert.IsNull(bazModelState.Errors[0].Exception, "Should not propagate the exception to ModelState since this might be information disclosure.");
            Assert.AreEqual("The value 'bazForm' is invalid for property 'baz'.", bazModelState.Errors[0].ErrorMessage);
        }

        private static ControllerContext GetControllerContext() {
            NameValueCollection formCollection = new NameValueCollection {
                { "foo", "fooForm" },
                { "bar", "barForm" },
                { "baz", "bazForm" }
            };
            NameValueCollection queryCollection = new NameValueCollection {
                { "foo", "fooQuery" },
                { "bar", "barQuery" }
            };
            Mock<HttpRequestBase> mockRequest = new Mock<HttpRequestBase>();
            mockRequest.Expect(r => r.Form).Returns(formCollection);
            mockRequest.Expect(r => r.QueryString).Returns(queryCollection);

            Mock<HttpContextBase> mockHttpContext = new Mock<HttpContextBase>();
            mockHttpContext.Expect(c => c.Request).Returns(mockRequest.Object);

            RouteData routeData = new RouteData();
            routeData.Values["foo"] = "fooRouteData";

            return new ControllerContext(mockHttpContext.Object, routeData, new Mock<ControllerBase>().Object);
        }

        public static IDisposable ReplaceCurrentCulture(string culture) {
            CultureInfo newCulture = new CultureInfo(culture);
            CultureInfo originalCulture = Thread.CurrentThread.CurrentCulture;
            Thread.CurrentThread.CurrentCulture = newCulture;
            return new CultureReplacement { OriginalCulture = originalCulture };
        }

        private class CultureReplacement : IDisposable {
            public CultureInfo OriginalCulture;
            public void Dispose() {
                Thread.CurrentThread.CurrentCulture = OriginalCulture;
            }
        }

        private class MyClassWithoutConverter {
        }

        [TypeConverter(typeof(CultureAwareConverter))]
        public class StringContainer {
            public StringContainer(string value) {
                Value = value;
            }
            public string Value {
                get;
                private set;
            }
        }

        private class CultureAwareConverter : TypeConverter {
            public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType) {
                return (sourceType == typeof(string));
            }
            public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType) {
                return (destinationType == typeof(string));
            }
            public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value) {
                string stringValue = value as string;
                if (String.IsNullOrEmpty(stringValue)) {
                    throw new Exception("Value cannot be null or empty.");
                }


                return new StringContainer(AppendCultureName(stringValue, culture));
            }
            public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType) {
                StringContainer container = value as StringContainer;
                if (String.IsNullOrEmpty(container.Value)) {
                    throw new Exception("Value cannot be null or empty.");
                }

                return AppendCultureName(container.Value, culture);
            }

            private static string AppendCultureName(string value, CultureInfo culture) {
                string cultureName = (!String.IsNullOrEmpty(culture.Name)) ? culture.Name : culture.ThreeLetterWindowsLanguageName;
                return value + " (" + cultureName + ")";
            }
        }

    }
}
