namespace System.Web.Mvc.Test {
    using System;
    using System.Globalization;
    using System.Web.TestUtil;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class DateTimeModelBinderTest {

        [TestMethod]
        public void ConvertTypeCallsBaseIfDestinationTypeIsNotDateTime() {
            // Arrange
            DateTimeModelBinder converter = new DateTimeModelBinder();

            // Act
            string result = (string)converter.ConvertType(CultureInfo.InvariantCulture, 4, typeof(string));

            // Assert
            Assert.AreEqual("4", result);
        }

        [TestMethod]
        public void ConvertTypeCallsBaseIfValueIsNotDateTimeOrString() {
            // Arrange
            DateTimeModelBinder converter = new DateTimeModelBinder();

            // Act & Assert
            ExceptionHelper.ExpectInvalidOperationException(
                delegate {
                    converter.ConvertType(CultureInfo.InvariantCulture, 4, typeof(DateTime));
                },
                "The parameter conversion from type 'System.Int32' to type 'System.DateTime' failed because no TypeConverter can convert between these types.");
        }

        [TestMethod]
        public void ConvertTypeConvertsStringValueToDateTimeUsingCurrentCultureIfNoCultureProvided() {
            // Arrange
            DateTimeModelBinder converter = new DateTimeModelBinder();
            string original = "01/02/2008"; // February 1, 2008 using the en-GB culture
            DateTime expected = new DateTime(2008, 2, 1);
            DateTime result;

            // Act
            using (DefaultModelBinderTest.ReplaceCurrentCulture("en-GB")) {
                result = (DateTime)converter.ConvertType(null /* culture */, original, typeof(DateTime));
            }

            // Assert
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void ConvertTypeConvertsStringValueToDateTimeUsingGivenCulture() {
            // Arrange
            DateTimeModelBinder converter = new DateTimeModelBinder();
            string original = "01/02/2008"; // January 2, 2008 using the invariant culture
            DateTime expected = new DateTime(2008, 1, 2);

            // Act
            DateTime result = (DateTime)converter.ConvertType(CultureInfo.InvariantCulture, original, typeof(DateTime));

            // Assert
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void ConvertTypeReturnsValueIfDateTime() {
            // Arrange
            DateTimeModelBinder converter = new DateTimeModelBinder();
            DateTime original = new DateTime(2008, 1, 1);

            // Act
            DateTime result = (DateTime)converter.ConvertType(CultureInfo.InvariantCulture, original, typeof(DateTime));

            // Assert
            Assert.AreEqual(original, result);
        }

    }
}
