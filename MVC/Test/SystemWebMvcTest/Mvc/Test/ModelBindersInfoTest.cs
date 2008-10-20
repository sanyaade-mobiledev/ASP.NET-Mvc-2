namespace System.Web.Mvc.Test {
    using System.Collections.Generic;
    using System.Web.Mvc;
    using System.Web.TestUtil;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;

    [TestClass]
    public class ModelBindersInfoTest {

        [TestMethod]
        public void BindersProperty() {
            // Arrange
            ModelBindersInfo bindersInfo = new ModelBindersInfo();

            // Act
            IDictionary<Type, IModelBinder> binders = bindersInfo.Binders;

            // Assert
            Assert.IsNotNull(binders);
        }

        [TestMethod]
        public void DefaultBinderProperty() {
            // Arrange
            ModelBindersInfo bindersInfo = new ModelBindersInfo();
            IModelBinder binder = new Mock<IModelBinder>().Object;

            // Act & assert
            MemberHelper.TestPropertyWithDefaultInstance(bindersInfo, "DefaultBinder", binder);
        }

        [TestMethod]
        public void GetBinderDoesNotReturnDefaultBinderIfAskedNotTo() {
            // Proper order of precedence:
            // 1. Binder registered in the global table
            // 2. Binder attribute defined on the type
            // 3. <null>

            // Arrange
            ModelBindersInfo bindersInfo = new ModelBindersInfo();

            IModelBinder registeredFirstBinder = new Mock<IModelBinder>().Object;
            bindersInfo.Binders[typeof(MyFirstConvertibleType)] = registeredFirstBinder;

            // Act
            IModelBinder binder1 = bindersInfo.GetBinder(typeof(MyFirstConvertibleType), false /* fallbackToDefault */);
            IModelBinder binder2 = bindersInfo.GetBinder(typeof(MySecondConvertibleType), false /* fallbackToDefault */);
            IModelBinder binder3 = bindersInfo.GetBinder(typeof(object), false /* fallbackToDefault */);

            // Assert
            Assert.AreSame(registeredFirstBinder, binder1, "First binder should have been matched in the registered binders table.");
            Assert.IsInstanceOfType(binder2, typeof(MySecondBinder), "Second binder should have been matched on the type.");
            Assert.IsNull(binder3, "Third binder should have returned null since asked not to use default.");
        }

        [TestMethod]
        public void GetBinderResolvesBindersWithCorrectPrecedence() {
            // Proper order of precedence:
            // 1. Binder registered in the global table
            // 2. Binder attribute defined on the type
            // 3. Default binder

            // Arrange
            ModelBindersInfo bindersInfo = new ModelBindersInfo();

            IModelBinder registeredFirstBinder = new Mock<IModelBinder>().Object;
            bindersInfo.Binders[typeof(MyFirstConvertibleType)] = registeredFirstBinder;

            IModelBinder defaultBinder = new Mock<IModelBinder>().Object;
            bindersInfo.DefaultBinder = defaultBinder;

            // Act
            IModelBinder binder1 = bindersInfo.GetBinder(typeof(MyFirstConvertibleType), true /* fallbackToDefault */);
            IModelBinder binder2 = bindersInfo.GetBinder(typeof(MySecondConvertibleType), true /* fallbackToDefault */);
            IModelBinder binder3 = bindersInfo.GetBinder(typeof(object), true /* fallbackToDefault */);

            // Assert
            Assert.AreSame(registeredFirstBinder, binder1, "First binder should have been matched in the registered binders table.");
            Assert.IsInstanceOfType(binder2, typeof(MySecondBinder), "Second binder should have been matched on the type.");
            Assert.AreSame(defaultBinder, binder3, "Third binder should have been the fallback.");
        }

        [TestMethod]
        public void GetBinderThrowsIfModelTypeContainsMultipleAttributes() {
            // Arrange
            ModelBindersInfo bindersInfo = new ModelBindersInfo();

            // Act & Assert
            ExceptionHelper.ExpectInvalidOperationException(
                delegate {
                    bindersInfo.GetBinder(typeof(ConvertibleTypeWithSeveralBinders), true /* fallbackToDefault */);
                },
                "The type 'System.Web.Mvc.Test.ModelBindersInfoTest+ConvertibleTypeWithSeveralBinders'"
                + " contains multiple attributes inheriting from CustomModelBinderAttribute.");
        }

        [ModelBinder(typeof(MyFirstBinder))]
        private class MyFirstConvertibleType {
        }

        private class MyFirstBinder : IModelBinder {
            public ModelBinderResult BindModel(ModelBindingContext bindingContext) {
                throw new NotImplementedException();
            }
        }

        [ModelBinder(typeof(MySecondBinder))]
        private class MySecondConvertibleType {
        }

        private class MySecondBinder : IModelBinder {
            public ModelBinderResult BindModel(ModelBindingContext bindingContext) {
                throw new NotImplementedException();
            }
        }

        [ModelBinder(typeof(MySecondBinder))]
        [MySubclassedBinder]
        private class ConvertibleTypeWithSeveralBinders {
        }

        private class MySubclassedBinderAttribute : CustomModelBinderAttribute {
            public override IModelBinder GetBinder() {
                throw new NotImplementedException();
            }
        }

    }
}
