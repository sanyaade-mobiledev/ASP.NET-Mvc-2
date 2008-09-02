namespace MvcFuturesTest.Mvc.Test {
    using System.Web.Mvc;
    using System.Web.Mvc.Test;
    using System.Web.TestUtil;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Microsoft.Web.Mvc;

    [TestClass]
    public class FormCollectionModelBinderTest {

        [TestMethod]
        public void CollectionProperty() {
            // Arrange
            FormCollection collection = new FormCollection();

            // Act
            FormCollectionModelBinder binder = new FormCollectionModelBinder(collection);

            // Assert
            Assert.AreSame(collection, binder.Collection);
        }

        [TestMethod]
        public void ConstructorThrowsIfCollectionIsNull() {
            // Act & Assert
            ExceptionHelper.ExpectArgumentNullException(
                delegate {
                    new FormCollectionModelBinder(null);
                }, "collection");
        }

        [TestMethod]
        public void GetValueBubblesExceptionOnFailureIfModelStateParameterIsNull() {
            // Arrange
            FormCollection collection = new FormCollection() { { "foo", "fooValue" } };
            FormCollectionModelBinder binder = new FormCollectionModelBinder(collection);

            // Act & Assert
            ExceptionHelper.ExpectInvalidOperationException(
                delegate {
                    binder.GetValue(null, "foo", typeof(int), null);
                },
                "The parameter conversion from type 'System.String' to type 'System.Int32' failed. See the inner exception for more information.");
        }

        [TestMethod]
        public void GetValuePerformsCultureAwareConversion() {
            // Arrange
            FormCollection collection = new FormCollection() { { "foo", "fooValue" } };
            FormCollectionModelBinder binder = new FormCollectionModelBinder(collection);

            // Act
            DefaultModelBinderTest.StringContainer stringContainer;
            using (DefaultModelBinderTest.ReplaceCurrentCulture("fr-FR")) {
                stringContainer = (DefaultModelBinderTest.StringContainer)binder.GetValue(null, "foo", typeof(DefaultModelBinderTest.StringContainer), null);
            }

            // Assert
            Assert.AreEqual("fooValue (fr-FR)", stringContainer.Value);
        }

        [TestMethod]
        public void GetValueThrowsIfModelNameIsEmpty() {
            // Arrange
            FormCollection collection = new FormCollection();
            FormCollectionModelBinder binder = new FormCollectionModelBinder(collection);

            // Act & Assert
            ExceptionHelper.ExpectArgumentExceptionNullOrEmpty(
                delegate {
                    binder.GetValue(null, "", typeof(object), null);
                }, "modelName");
        }

        [TestMethod]
        public void GetValueThrowsIfModelNameIsNull() {
            // Arrange
            FormCollection collection = new FormCollection();
            FormCollectionModelBinder binder = new FormCollectionModelBinder(collection);

            // Act & Assert
            ExceptionHelper.ExpectArgumentExceptionNullOrEmpty(
                delegate {
                    binder.GetValue(null, null, typeof(object), null);
                }, "modelName");
        }

        [TestMethod]
        public void GetValueThrowsIfModelTypeIsNull() {
            // Arrange
            FormCollection collection = new FormCollection();
            FormCollectionModelBinder binder = new FormCollectionModelBinder(collection);

            // Act & Assert
            ExceptionHelper.ExpectArgumentNullException(
                delegate {
                    binder.GetValue(null, "some name", null, null);
                }, "modelType");
        }

        [TestMethod]
        public void GetValueUpdatesModelStateIfProvidedOnFailure() {
            // Arrange
            FormCollection collection = new FormCollection() { { "foo", "fooValue" } };
            FormCollectionModelBinder binder = new FormCollectionModelBinder(collection);
            ModelStateDictionary modelStateDictionary = new ModelStateDictionary();

            // Act
            object oFoo = binder.GetValue(null, "foo", typeof(int), modelStateDictionary);

            // Assert
            Assert.AreEqual(1, modelStateDictionary.Count);

            Assert.IsNull(oFoo);
            ModelState fooModelState = modelStateDictionary["foo"];
            Assert.AreEqual("fooValue", fooModelState.AttemptedValue);
            Assert.AreEqual(1, fooModelState.Errors.Count);
            Assert.IsNull(fooModelState.Errors[0].Exception, "Should not propagate the exception to ModelState since this might be information disclosure.");
            Assert.AreEqual("The value 'fooValue' is not a valid 'System.Int32'.", fooModelState.Errors[0].ErrorMessage);
        }

    }
}
