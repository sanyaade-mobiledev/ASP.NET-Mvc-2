namespace System.Web.Mvc.Test {
    using System.Collections.Generic;
    using System.Web.Mvc;
    using System.Web.TestUtil;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;

    [TestClass]
    public class ModelBindersInfoTest {

        [TestMethod]
        public void ConvertersProperty() {
            // Arrange
            ModelBindersInfo convertersInfo = new ModelBindersInfo();

            // Act
            IDictionary<Type, IModelBinder> converters = convertersInfo.Binders;

            // Assert
            Assert.IsNotNull(converters);
        }

        [TestMethod]
        public void DefaultConverterPropertyExplicitlySet() {
            // Arrange
            IModelBinder converter = new Mock<IModelBinder>().Object;
            ModelBindersInfo convertersInfo = new ModelBindersInfo();

            // Act
            convertersInfo.DefaultBinder = converter;
            IModelBinder returned = convertersInfo.DefaultBinder;

            // Assert
            Assert.AreSame(converter, returned);
        }

        [TestMethod]
        public void DefaultConverterPropertyReturnsDefaultConverterIfNull() {
            // Arrange
            ModelBindersInfo convertersInfo = new ModelBindersInfo();

            // Act
            convertersInfo.DefaultBinder = null;
            IModelBinder returned = convertersInfo.DefaultBinder;

            // Assert
            Assert.IsInstanceOfType(returned, typeof(DefaultModelBinder));
        }

        [TestMethod]
        public void GetConverterResolvesConvertersWithCorrectPrecedence() {
            // Proper order of precedence:
            // 1. Converter registered in the global table
            // 2. Converter attribute defined on the type
            // 3. Default converter

            // Arrange
            ModelBindersInfo convertersInfo = new ModelBindersInfo();

            IModelBinder registeredFirstConverter = new Mock<IModelBinder>().Object;
            convertersInfo.Binders[typeof(MyFirstConvertibleType)] = registeredFirstConverter;

            IModelBinder defaultConverter = new Mock<IModelBinder>().Object;
            convertersInfo.DefaultBinder = defaultConverter;

            // Act
            IModelBinder converter1 = convertersInfo.GetBinder(typeof(MyFirstConvertibleType));
            IModelBinder converter2 = convertersInfo.GetBinder(typeof(MySecondConvertibleType));
            IModelBinder converter3 = convertersInfo.GetBinder(typeof(object));

            // Assert
            Assert.AreSame(registeredFirstConverter, converter1, "First converter should have been matched in the registered converters table.");
            Assert.IsInstanceOfType(converter2, typeof(MySecondConverter), "Second converter should have been matched on the type.");
            Assert.AreSame(defaultConverter, converter3, "Third converter should have been the fallback.");
        }

        [TestMethod]
        public void GetConverterThrowsIfParameterTypeContainsMultipleAttributes() {
            // Arrange
            ModelBindersInfo convertersInfo = new ModelBindersInfo();

            // Act & Assert
            ExceptionHelper.ExpectInvalidOperationException(
                delegate {
                    convertersInfo.GetBinder(typeof(ConvertibleTypeWithSeveralConverters));
                },
                "The type 'System.Web.Mvc.Test.ModelBindersInfoTest+ConvertibleTypeWithSeveralConverters'"
                + " contains multiple attributes inheriting from CustomModelBinderAttribute.");
        }

        [ModelBinder(typeof(MyFirstConverter))]
        private class MyFirstConvertibleType {
        }

        private class MyFirstConverter : IModelBinder {
            public object GetValue(ControllerContext controllerContext, string parameterName, Type parameterType, ModelStateDictionary modelState) {
                throw new NotImplementedException();
            }
        }

        [ModelBinder(typeof(MySecondConverter))]
        private class MySecondConvertibleType {
        }

        private class MySecondConverter : IModelBinder {
            public object GetValue(ControllerContext controllerContext, string parameterName, Type parameterType, ModelStateDictionary modelState) {
                throw new NotImplementedException();
            }
        }

        [ModelBinder(typeof(MySecondConverter))]
        [MySubclassedConverter]
        private class ConvertibleTypeWithSeveralConverters {
        }

        private class MySubclassedConverterAttribute : CustomModelBinderAttribute {
            public override IModelBinder GetBinder() {
                throw new NotImplementedException();
            }
        }

    }
}
