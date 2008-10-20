namespace System.Web.Mvc.Test {
    using System;
    using System.Globalization;
    using System.Web;
    using System.Web.Mvc;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class ValueProviderResultTest {

        [TestMethod]
        public void ConstructorSetsProperties() {
            // Arrange
            object rawValue = new object();
            string attemptedValue = "some string";
            CultureInfo culture = CultureInfo.GetCultureInfo("fr-FR");

            // Act
            ValueProviderResult result = new ValueProviderResult(rawValue, attemptedValue, culture);

            // Assert
            Assert.AreSame(rawValue, result.RawValue);
            Assert.AreSame(attemptedValue, result.AttemptedValue);
            Assert.AreSame(culture, result.Culture);
        }

        [TestMethod]
        public void CulturePropertyDefaultsToInvariantCulture() {
            // Arrange
            ValueProviderResult result = new ValueProviderResult(null, null, null);

            // Act & assert
            Assert.AreSame(CultureInfo.InvariantCulture, result.Culture);
        }

    }
}
