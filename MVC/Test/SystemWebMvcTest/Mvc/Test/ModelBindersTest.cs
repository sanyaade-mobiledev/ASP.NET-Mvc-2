namespace System.Web.Mvc.Test {
    using System;
    using System.Collections.Generic;
    using System.Web.Mvc;
    using System.Web.TestUtil;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class ModelBindersTest {

        [TestMethod]
        public void BindersProperty() {
            // Act
            IDictionary<Type, IModelBinder> binders = ModelBinders.Binders;

            // Assert
            Assert.AreEqual(0, binders.Count);
        }

        [TestMethod]
        public void DefaultBinderProperty() {
            // Act
            DefaultModelBinder binder = ModelBinders.DefaultBinder as DefaultModelBinder;

            // Assert
            Assert.IsNotNull(binder);
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
