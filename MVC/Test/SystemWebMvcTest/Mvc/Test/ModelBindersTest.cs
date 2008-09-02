namespace System.Web.Mvc.Test {
    using System.Collections.Generic;
    using System.Web.Mvc;
    using System.Web.TestUtil;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class ModelBindersTest {

        [TestMethod]
        public void ConvertersProperty() {
            // The converters dictionary should come pre-populated with DateTime-specific converters

            // Act
            IDictionary<Type, IModelBinder> converters = ModelBinders.Binders;

            // Assert
            Assert.AreEqual(2, converters.Count);
            Assert.IsInstanceOfType(converters[typeof(DateTime)], typeof(DateTimeModelBinder));
            Assert.IsInstanceOfType(converters[typeof(DateTime?)], typeof(DateTimeModelBinder));
        }

        [TestMethod]
        public void DefaultConverterProperty() {
            // Act
            DefaultModelBinder converter = ModelBinders.DefaultBinder as DefaultModelBinder;

            // Assert
            Assert.IsNotNull(converter);
        }

        [TestMethod]
        public void GetBinderThrowsIfModelTypeIsNull() {
            // Act & Assert
            ExceptionHelper.ExpectArgumentNullException(
                delegate {
                    ModelBinders.GetBinder(null);
                }, "modelType");
        }

    }
}
