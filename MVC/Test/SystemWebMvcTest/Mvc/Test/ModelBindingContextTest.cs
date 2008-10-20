namespace System.Web.Mvc.Test {
    using System;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Routing;
    using System.Web.TestUtil;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;

    [TestClass]
    public class ModelBindingContextTest {

        [TestMethod]
        public void ConstructorThrowsIfControllerContextIsNull() {
            // Act & assert
            ExceptionHelper.ExpectArgumentNullException(
                delegate {
                    new ModelBindingContext(null, null, null, null, null, null, null);
                }, "controllerContext");
        }

        [TestMethod]
        public void ConstructorThrowsIfModelTypeIsNull() {
            // Act & assert
            ExceptionHelper.ExpectArgumentNullException(
                delegate {
                    new ModelBindingContext(GetControllerContext(), new Mock<IValueProvider>().Object, null, null, null, null, null);
                }, "modelType");
        }

        [TestMethod]
        public void ConstructorThrowsIfValueProviderIsNull() {
            // Act & assert
            ExceptionHelper.ExpectArgumentNullException(
                delegate {
                    new ModelBindingContext(GetControllerContext(), null, null, null, null, null, null);
                }, "valueProvider");
        }

        [TestMethod]
        public void ModelNameProperty() {
            // Arrange
            ControllerContext controllerContext = GetControllerContext();
            IValueProvider valueProvider = new Mock<IValueProvider>().Object;
            Func<object> modelProvider = null;
            string modelName = "someName";
            Type modelType = typeof(Guid);
            ModelStateDictionary modelState = null;
            Predicate<string> propertyFilter = null;

            ModelBindingContext bindingContext = new ModelBindingContext(controllerContext, valueProvider, modelType, modelName, modelProvider, modelState, propertyFilter);

            // Act
            string returned = bindingContext.ModelName;

            // Assert
            Assert.AreEqual("someName", returned);
        }

        [TestMethod]
        public void ModelNamePropertyDefaultsToEmptyStringIfNotProvided() {
            // Arrange
            ControllerContext controllerContext = GetControllerContext();
            IValueProvider valueProvider = new Mock<IValueProvider>().Object;
            Func<object> modelProvider = null;
            string modelName = null;
            Type modelType = typeof(Guid);
            ModelStateDictionary modelState = null;
            Predicate<string> propertyFilter = null;

            ModelBindingContext bindingContext = new ModelBindingContext(controllerContext, valueProvider, modelType, modelName, modelProvider, modelState, propertyFilter);

            // Act
            string returned = bindingContext.ModelName;

            // Assert
            Assert.AreEqual(String.Empty, returned);
        }

        [TestMethod]
        public void ModelPropertyGetsValueFromModelProviderIfProvided() {
            // Arrange
            int numTimesCalled = 0;
            ControllerContext controllerContext = GetControllerContext();
            IValueProvider valueProvider = new Mock<IValueProvider>().Object;
            Func<object> modelProvider = () => "theModel_" + (++numTimesCalled);
            string modelName = null;
            Type modelType = typeof(Guid);
            ModelStateDictionary modelState = null;
            Predicate<string> propertyFilter = null;

            ModelBindingContext bindingContext = new ModelBindingContext(controllerContext, valueProvider, modelType, modelName, modelProvider, modelState, propertyFilter);

            // Act
            object returned1 = bindingContext.Model;
            object returned2 = bindingContext.Model;

            // Assert
            Assert.AreEqual(1, numTimesCalled, "Model provider should have been called only once.");
            Assert.AreEqual("theModel_1", returned1);
            Assert.AreEqual("theModel_1", returned2, "Model provider result should have been cached.");
        }

        [TestMethod]
        public void ModelPropertyReturnsNullIfModelProviderNotProvided() {
            // Arrange
            ControllerContext controllerContext = GetControllerContext();
            IValueProvider valueProvider = new Mock<IValueProvider>().Object;
            Func<object> modelProvider = null;
            string modelName = null;
            Type modelType = typeof(Guid);
            ModelStateDictionary modelState = null;
            Predicate<string> propertyFilter = null;

            ModelBindingContext bindingContext = new ModelBindingContext(controllerContext, valueProvider, modelType, modelName, modelProvider, modelState, propertyFilter);

            // Act
            object returned = bindingContext.Model;

            // Assert
            Assert.IsNull(returned);
        }

        [TestMethod]
        public void ModelTypeProperty() {
            // Arrange
            ControllerContext controllerContext = GetControllerContext();
            IValueProvider valueProvider = new Mock<IValueProvider>().Object;
            Func<object> modelProvider = null;
            string modelName = null;
            Type modelType = typeof(Guid);
            ModelStateDictionary modelState = null;
            Predicate<string> propertyFilter = null;

            ModelBindingContext bindingContext = new ModelBindingContext(controllerContext, valueProvider, modelType, modelName, modelProvider, modelState, propertyFilter);

            // Act
            Type returned = bindingContext.ModelType;

            // Assert
            Assert.AreEqual(modelType, returned);
        }

        [TestMethod]
        public void ModelStateProperty() {
            // Arrange
            ControllerContext controllerContext = GetControllerContext();
            IValueProvider valueProvider = new Mock<IValueProvider>().Object;
            Func<object> modelProvider = null;
            string modelName = "someName";
            Type modelType = typeof(Guid);
            ModelStateDictionary modelState = new ModelStateDictionary();
            Predicate<string> propertyFilter = null;

            ModelBindingContext bindingContext = new ModelBindingContext(controllerContext, valueProvider, modelType, modelName, modelProvider, modelState, propertyFilter);

            // Act
            ModelStateDictionary returned = bindingContext.ModelState;

            // Assert
            Assert.AreEqual(modelState, returned);
        }

        [TestMethod]
        public void ModelStatePropertyDefaultsToEmptyDictionaryIfNotProvided() {
            // Arrange
            ControllerContext controllerContext = GetControllerContext();
            IValueProvider valueProvider = new Mock<IValueProvider>().Object;
            Func<object> modelProvider = null;
            string modelName = null;
            Type modelType = typeof(Guid);
            ModelStateDictionary modelState = null;
            Predicate<string> propertyFilter = null;

            ModelBindingContext bindingContext = new ModelBindingContext(controllerContext, valueProvider, modelType, modelName, modelProvider, modelState, propertyFilter);

            // Act
            ModelStateDictionary returned = bindingContext.ModelState;

            // Assert
            Assert.IsNotNull(returned, "Property should have returned a default instance.");
            Assert.AreEqual(0, returned.Count);
        }

        [TestMethod]
        public void ShouldUpdatePropertyCallsPropertyFilterIfProvided() {
            // Arrange
            ControllerContext controllerContext = GetControllerContext();
            IValueProvider valueProvider = new Mock<IValueProvider>().Object;
            Func<object> modelProvider = null;
            string modelName = null;
            Type modelType = typeof(Guid);
            ModelStateDictionary modelState = null;
            Predicate<string> propertyFilter = Convert.ToBoolean;

            ModelBindingContext bindingContext = new ModelBindingContext(controllerContext, valueProvider, modelType, modelName, modelProvider, modelState, propertyFilter);

            // Act
            bool returnedTrue = bindingContext.ShouldUpdateProperty("true");
            bool returnedFalse = bindingContext.ShouldUpdateProperty("false");

            // Assert
            Assert.IsTrue(returnedTrue);
            Assert.IsFalse(returnedFalse);
        }

        [TestMethod]
        public void ShouldUpdatePropertyReturnsTrueIfPropertyFilterNotProvided() {
            // Arrange
            ControllerContext controllerContext = GetControllerContext();
            IValueProvider valueProvider = new Mock<IValueProvider>().Object;
            Func<object> modelProvider = null;
            string modelName = null;
            Type modelType = typeof(Guid);
            ModelStateDictionary modelState = null;
            Predicate<string> propertyFilter = null;

            ModelBindingContext bindingContext = new ModelBindingContext(controllerContext, valueProvider, modelType, modelName, modelProvider, modelState, propertyFilter);

            // Act
            bool returned = bindingContext.ShouldUpdateProperty("someProperty");

            // Assert
            Assert.IsTrue(returned);
        }

        [TestMethod]
        public void ValueProviderProperty() {
            // Arrange
            ControllerContext controllerContext = GetControllerContext();
            IValueProvider valueProvider = new Mock<IValueProvider>().Object;
            Func<object> modelProvider = null;
            string modelName = null;
            Type modelType = typeof(Guid);
            ModelStateDictionary modelState = null;
            Predicate<string> propertyFilter = null;

            ModelBindingContext bindingContext = new ModelBindingContext(controllerContext, valueProvider, modelType, modelName, modelProvider, modelState, propertyFilter);

            // Act
            IValueProvider returned = bindingContext.ValueProvider;

            // Assert
            Assert.AreEqual(valueProvider, returned);
        }

        private static ControllerContext GetControllerContext() {
            return new ControllerContext(new Mock<HttpContextBase>().Object, new RouteData(), new Mock<ControllerBase>().Object);
        }

    }

    public class ModelBindingContextHelper {

        public ModelBindingContextHelper() {
            ControllerContext = new ControllerContext(new Mock<HttpContextBase>().Object, new RouteData(), new Mock<ControllerBase>().Object);
            ModelName = "modelName";
            ModelType = typeof(Guid);
            ModelState = new ModelStateDictionary();
            ValueProvider = new Mock<IValueProvider>().Object;
        }

        public ControllerContext ControllerContext {
            get;
            set;
        }

        public object Model {
            get {
                return (ModelThunk != null) ? ModelThunk() : null;
            }
            set {
                ModelThunk = () => value;
            }
        }

        public string ModelName {
            get;
            set;
        }

        public ModelStateDictionary ModelState {
            get;
            set;
        }

        public Func<object> ModelThunk {
            get;
            set;
        }

        public Type ModelType {
            get;
            set;
        }

        public Predicate<string> PropertyFilter {
            get;
            set;
        }

        public IValueProvider ValueProvider {
            get;
            set;
        }

        public ModelBindingContext CreateBindingContext() {
            return new ModelBindingContext(ControllerContext, ValueProvider, ModelType, ModelName, ModelThunk, ModelState, PropertyFilter);
        }

    }
}
