namespace System.Web.Mvc.Test {
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Globalization;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Routing;
    using System.Web.TestUtil;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;

    [TestClass]
    [CLSCompliant(false)]
    public class DefaultModelBinderTest {

        [TestMethod]
        public void BindModelCallsBindModelCorePassingNewInstanceThunkIfNoInstanceThunkProvided() {
            // Arrange
            ModelBindingContextHelper bcHelper = new ModelBindingContextHelper() { ModelType = typeof(List<string>) };
            List<string> expected = null;

            Mock<DefaultModelBinderHelper> mockHelper = new Mock<DefaultModelBinderHelper>() { CallBase = true };
            mockHelper
                .Expect(h => h.PublicBindModelCore(It.IsAny<ModelBindingContext>()))
                .Returns(
                    delegate(ModelBindingContext bindingContext) {
                        Assert.AreEqual(bcHelper.ModelName, bindingContext.ModelName);
                        Assert.AreEqual(bcHelper.ModelState, bindingContext.ModelState);
                        Assert.AreEqual(bcHelper.ModelType, bindingContext.ModelType);
                        Assert.AreEqual(bcHelper.ValueProvider, bindingContext.ValueProvider);
                        expected = (List<string>)bindingContext.Model;
                        return new ModelBinderResult(expected);
                    });

            DefaultModelBinder binder = mockHelper.Object;

            // Act
            ModelBinderResult returned = binder.BindModel(bcHelper.CreateBindingContext());

            // Assert
            Assert.IsNotNull(expected);
            Assert.AreEqual(expected, returned.Value);
        }

        [TestMethod]
        public void BindModelCallsBindModelCorePassingProvidedModelThunk() {
            // Arrange
            ModelBindingContextHelper bcHelper = new ModelBindingContextHelper() { Model = new List<string>(), ModelType = typeof(List<string>) };

            Mock<DefaultModelBinderHelper> mockHelper = new Mock<DefaultModelBinderHelper>() { CallBase = true };
            mockHelper
                .Expect(h => h.PublicBindModelCore(It.IsAny<ModelBindingContext>()))
                .Returns(
                    delegate(ModelBindingContext bindingContext) {
                        Assert.AreEqual(bcHelper.ModelName, bindingContext.ModelName);
                        Assert.AreEqual(bcHelper.ModelState, bindingContext.ModelState);
                        Assert.AreEqual(bcHelper.ModelType, bindingContext.ModelType);
                        Assert.AreEqual(bcHelper.ValueProvider, bindingContext.ValueProvider);
                        return new ModelBinderResult(bindingContext.Model);
                    });

            DefaultModelBinder binder = mockHelper.Object;

            // Act
            ModelBinderResult returned = binder.BindModel(bcHelper.CreateBindingContext());

            // Assert
            Assert.AreEqual(bcHelper.Model, returned.Value);
        }

        [TestMethod]
        public void BindModelCanBindComplexArrays() {
            // Arrange
            ModelBindingContextHelper bcHelper = new ModelBindingContextHelper() { ValueProvider = GetValueProviderForArray(), ModelType = typeof(MyModel[]) };
            DefaultModelBinder binder = new DefaultModelBinder();

            // Act
            MyModel[] returned = (MyModel[])binder.BindModel(bcHelper.CreateBindingContext()).Value;

            // Assert
            Assert.AreEqual(3, returned.Length);
            Assert.AreEqual(100, returned[0].ReadWriteProperty);
            Assert.AreEqual(101, returned[0].ReadWriteProperty2);
            Assert.AreEqual(200, returned[1].ReadWriteProperty);
            Assert.AreEqual(201, returned[1].ReadWriteProperty2);
            Assert.AreEqual(300, returned[2].ReadWriteProperty);
            Assert.AreEqual(301, returned[2].ReadWriteProperty2);
        }

        [TestMethod]
        public void BindModelCanBindComplexLists() {
            // Arrange
            ModelBindingContextHelper bcHelper = new ModelBindingContextHelper() { ValueProvider = GetValueProviderForArray(), ModelType = typeof(IList<MyModel>) };
            DefaultModelBinder binder = new DefaultModelBinder();

            // Act
            IList<MyModel> returned = (IList<MyModel>)binder.BindModel(bcHelper.CreateBindingContext()).Value;

            // Assert
            Assert.AreEqual(3, returned.Count);
            Assert.AreEqual(100, returned[0].ReadWriteProperty);
            Assert.AreEqual(101, returned[0].ReadWriteProperty2);
            Assert.AreEqual(200, returned[1].ReadWriteProperty);
            Assert.AreEqual(201, returned[1].ReadWriteProperty2);
            Assert.AreEqual(300, returned[2].ReadWriteProperty);
            Assert.AreEqual(301, returned[2].ReadWriteProperty2);
        }

        [TestMethod]
        public void BindModelCanBindSimpleArrays() {
            // Arrange
            Mock<IValueProvider> mockValueProvider = new Mock<IValueProvider>();
            mockValueProvider.Expect(p => p.GetValue("modelName")).Returns(new ValueProviderResult(new int[] { 0, 1, 2 }, "0,1,2", null));
            ModelBindingContextHelper bcHelper = new ModelBindingContextHelper() { ValueProvider = mockValueProvider.Object, ModelType = typeof(string[]) };
            DefaultModelBinder binder = new DefaultModelBinder();

            // Act
            string[] returned = (string[])binder.BindModel(bcHelper.CreateBindingContext()).Value;

            // Assert
            Assert.AreEqual(3, returned.Length);
            Assert.AreEqual("0", returned[0]);
            Assert.AreEqual("1", returned[1]);
            Assert.AreEqual("2", returned[2]);
        }

        [TestMethod]
        public void BindModelCanBindSimpleArraysUsingIndexWireFormat() {
            // Arrange
            Mock<IValueProvider> mockValueProvider = new Mock<IValueProvider>();
            mockValueProvider.Expect(p => p.GetValue("modelName.index")).Returns(new ValueProviderResult(new string[] { "foo", "bar", "baz" }, null, null));
            mockValueProvider.Expect(p => p.GetValue("modelName[foo]")).Returns(new ValueProviderResult(0, null, null));
            mockValueProvider.Expect(p => p.GetValue("modelName[bar]")).Returns(new ValueProviderResult(1, null, null));
            mockValueProvider.Expect(p => p.GetValue("modelName[baz]")).Returns(new ValueProviderResult(2, null, null));

            ModelBindingContextHelper bcHelper = new ModelBindingContextHelper() { ValueProvider = mockValueProvider.Object, ModelType = typeof(string[]) };
            DefaultModelBinder binder = new DefaultModelBinder();

            // Act
            string[] returned = (string[])binder.BindModel(bcHelper.CreateBindingContext()).Value;

            // Assert
            Assert.AreEqual(3, returned.Length);
            Assert.AreEqual("0", returned[0]);
            Assert.AreEqual("1", returned[1]);
            Assert.AreEqual("2", returned[2]);
        }

        [TestMethod]
        public void BindModelCanBindSimpleLists() {
            // Arrange
            Mock<IValueProvider> mockValueProvider = new Mock<IValueProvider>();
            mockValueProvider.Expect(p => p.GetValue("modelName")).Returns(new ValueProviderResult(new int[] { 0, 1, 2 }, "0,1,2", null));
            ModelBindingContextHelper bcHelper = new ModelBindingContextHelper() { ValueProvider = mockValueProvider.Object, ModelType = typeof(IList<string>) };
            DefaultModelBinder binder = new DefaultModelBinder();

            // Act
            IList<string> returned = (IList<string>)binder.BindModel(bcHelper.CreateBindingContext()).Value;

            // Assert
            Assert.AreEqual(3, returned.Count);
            Assert.AreEqual("0", returned[0]);
            Assert.AreEqual("1", returned[1]);
            Assert.AreEqual("2", returned[2]);
        }

        [TestMethod]
        public void BindModelCanBindSimpleListsUsingIndexWireFormat() {
            // Arrange
            Mock<IValueProvider> mockValueProvider = new Mock<IValueProvider>();
            mockValueProvider.Expect(p => p.GetValue("modelName.index")).Returns(new ValueProviderResult(new string[] { "foo", "bar", "baz" }, null, null));
            mockValueProvider.Expect(p => p.GetValue("modelName[foo]")).Returns(new ValueProviderResult(0, null, null));
            mockValueProvider.Expect(p => p.GetValue("modelName[bar]")).Returns(new ValueProviderResult(1, null, null));
            mockValueProvider.Expect(p => p.GetValue("modelName[baz]")).Returns(new ValueProviderResult(2, null, null));

            ModelBindingContextHelper bcHelper = new ModelBindingContextHelper() { ValueProvider = mockValueProvider.Object, ModelType = typeof(IList<string>) };
            DefaultModelBinder binder = new DefaultModelBinder();

            // Act
            IList<string> returned = (IList<string>)binder.BindModel(bcHelper.CreateBindingContext()).Value;

            // Assert
            Assert.AreEqual(3, returned.Count);
            Assert.AreEqual("0", returned[0]);
            Assert.AreEqual("1", returned[1]);
            Assert.AreEqual("2", returned[2]);
        }

        [TestMethod]
        public void BindModelCollectionReturnsNullIfUsingIndexWireFormatAndNoIndicesFound() {
            // Arrange
            ModelBindingContextHelper bcHelper = new ModelBindingContextHelper() { ModelType = typeof(ICollection<string>) };
            DefaultModelBinder binder = new DefaultModelBinder();

            // Act
            object returned = binder.BindModel(bcHelper.CreateBindingContext());

            // Assert
            Assert.IsNull(returned);
        }

        [TestMethod]
        public void BindModelCoreAddsModelErrorIfRequiredValueIsMissing() {
            // Arrange
            Mock<IValueProvider> mockValueProvider = new Mock<IValueProvider>();
            mockValueProvider.Expect(vp => vp.GetValue("modelName.Integer")).Returns(new ValueProviderResult("", "", null));
            ModelBindingContextHelper bcHelper = new ModelBindingContextHelper() { ValueProvider = mockValueProvider.Object, ModelType = typeof(IntegerContainer) };
            DefaultModelBinderHelper helper = new DefaultModelBinderHelper();

            // Act
            object returned = helper.PublicBindModelCore(bcHelper.CreateBindingContext()).Value;

            // Assert
            Assert.IsNull(returned, "Wrapped return value should be null if model couldn't be hydrated.");

            ModelState modelState = bcHelper.ModelState["modelName.Integer"];
            Assert.AreEqual(1, modelState.Errors.Count);
            Assert.AreEqual("A value is required.", modelState.Errors[0].ErrorMessage);
        }

        [TestMethod]
        public void BindModelCoreContinuesWithSiblingPropertiesIfSomePropertySetterFails() {
            // Arrange
            MyModelWithBadPropertySetter model = new MyModelWithBadPropertySetter();

            Mock<IValueProvider> mockValueProvider = new Mock<IValueProvider>();
            mockValueProvider.Expect(vp => vp.GetValue("modelName.NormalProperty")).Returns(new ValueProviderResult("10", "10", null));
            mockValueProvider.Expect(vp => vp.GetValue("modelName.PropertySetterThrows")).Returns(new ValueProviderResult("20", "20", null));
            mockValueProvider.Expect(vp => vp.GetValue("modelName.PropertySetterThrows2")).Returns(new ValueProviderResult("30", "30", null));

            ModelBindingContextHelper bcHelper = new ModelBindingContextHelper() {
                ValueProvider = mockValueProvider.Object,
                Model = model,
                ModelType = typeof(MyModelWithBadPropertySetter),
            };
            DefaultModelBinderHelper helper = new DefaultModelBinderHelper();

            // Act
            MyModelWithBadPropertySetter returned = (MyModelWithBadPropertySetter)helper.PublicBindModelCore(bcHelper.CreateBindingContext()).Value;

            // Assert
            Assert.AreEqual(model, returned, "BindModelCore() returned wrong model.");
            Assert.AreEqual(10, model.NormalProperty);

            Assert.AreEqual("The value '20' is invalid.", bcHelper.ModelState["modelName.PropertySetterThrows"].Errors[0].ErrorMessage);
            Assert.AreEqual("The value '30' is invalid.", bcHelper.ModelState["modelName.PropertySetterThrows2"].Errors[0].ErrorMessage);
        }

        [TestMethod]
        public void BindModelCoreCanUpdateComplexTypes() {
            // Arrange
            Mock<IModelBinder> mockPropertyBinder = new Mock<IModelBinder>();
            mockPropertyBinder
                .Expect(b => b.BindModel(It.IsAny<ModelBindingContext>()))
                .Returns(
                    delegate(ModelBindingContext bindingContext) {
                        switch (bindingContext.ModelName) {
                            case "modelName.ReadWriteProperty":
                                return new ModelBinderResult(10);
                            case "modelName.ReadWriteProperty2":
                                return null;
                            default:
                                Assert.Fail("We called into a model binder for a property we had no hope of updating, such as a read-only value type.");
                                throw new Exception();
                        }
                    });

            Mock<DefaultModelBinderHelper> mockHelper = new Mock<DefaultModelBinderHelper>() { CallBase = true };
            mockHelper.Expect(h => h.PublicGetBinder(typeof(int))).Returns(mockPropertyBinder.Object);
            DefaultModelBinderHelper helper = mockHelper.Object;

            MyModel oldModel = new MyModel() { ReadWriteProperty2 = 3 };
            ModelBindingContextHelper bcHelper = new ModelBindingContextHelper() { ModelType = typeof(MyModel), Model = oldModel };

            // Act
            object newModel = helper.PublicBindModelCore(bcHelper.CreateBindingContext()).Value;

            // Assert
            Assert.AreSame(oldModel, newModel, "BindModelCore() should return the same object it was given.");
            Assert.AreEqual(4, oldModel.ReadOnlyProperty, "Can't update a read-only property.");
            Assert.AreEqual(10, oldModel.ReadWriteProperty, "Should be able to update a read+write property.");
            Assert.AreEqual(3, oldModel.ReadWriteProperty2, "Shouldn't have called setter if model binder returned null.");
            mockPropertyBinder.Verify();
        }

        [TestMethod]
        public void BindModelCoreCanUpdateComplexTypesWithReadOnlyReferenceTypeProperties() {
            // Arrange
            ModelBindingContextHelper bcHelper = new ModelBindingContextHelper() {
                ValueProvider = GetReflectingValueProvider(),
                Model = new Customer(),
                ModelType = typeof(Customer),
                ModelName = "customer"
            };
            DefaultModelBinderHelper helper = new DefaultModelBinderHelper();

            // Act
            Customer customer = (Customer)helper.PublicBindModelCore(bcHelper.CreateBindingContext()).Value;

            // Assert
            Assert.IsNotNull(customer);
            Assert.AreEqual("customer.Address.StreetValue", customer.Address.Street);
            Assert.AreEqual("customer.Address.ZipValue", customer.Address.Zip);
        }

        [TestMethod]
        public void BindModelCoreCanUpdateGenericCollections() {
            // Arrange
            List<string> model = new List<string>();
            ModelBindingContextHelper bcHelper = new ModelBindingContextHelper() {
                ValueProvider = GetValueProviderForCollection(),
                Model = model,
                ModelType = typeof(List<string>)
            };
            DefaultModelBinderHelper helper = new DefaultModelBinderHelper();

            // Act
            object returned = helper.PublicBindModelCore(bcHelper.CreateBindingContext()).Value;

            // Assert
            Assert.AreSame(model, returned);
            Assert.AreEqual(3, model.Count, "List does not contain the correct number of entries.");
            Assert.AreEqual("fooValue", model[0]);
            Assert.AreEqual("barValue", model[1]);
            Assert.AreEqual("bazValue", model[2]);
        }

        [TestMethod]
        public void BindModelCoreCanUpdateGenericDictionaries() {
            // Arrange
            Dictionary<string, string> model = new Dictionary<string, string>();
            ModelBindingContextHelper bcHelper = new ModelBindingContextHelper() {
                ValueProvider = GetValueProviderForDictionary(),
                Model = model,
                ModelType = typeof(Dictionary<string, string>)
            };
            DefaultModelBinderHelper helper = new DefaultModelBinderHelper();

            // Act
            object returned = helper.PublicBindModelCore(bcHelper.CreateBindingContext()).Value;

            // Assert
            Assert.AreSame(model, returned);
            Assert.AreEqual(3, model.Count, "Dictionary does not contain the correct number of entries.");
            Assert.AreEqual("fooValue", model["fooKey"]);
            Assert.AreEqual("barValue", model["barKey"]);
            Assert.AreEqual("bazValue", model["bazKey"]);
        }

        [TestMethod]
        public void BindModelCorePreservesExistingModelErrorIfRequiredValueConversionFailed() {
            // Arrange
            Mock<IValueProvider> mockValueProvider = new Mock<IValueProvider>();
            mockValueProvider.Expect(vp => vp.GetValue("modelName.Integer")).Returns(new ValueProviderResult("dog", "dog", null));
            ModelBindingContextHelper bcHelper = new ModelBindingContextHelper() { ValueProvider = mockValueProvider.Object, ModelType = typeof(IntegerContainer) };
            DefaultModelBinderHelper helper = new DefaultModelBinderHelper();

            // Act
            object newModel = helper.PublicBindModelCore(bcHelper.CreateBindingContext()).Value;

            // Assert
            Assert.IsNull(newModel, "Return value should be null if model couldn't be hydrated.");

            ModelState modelState = bcHelper.ModelState["modelName.Integer"];
            Assert.AreEqual(1, modelState.Errors.Count);
            Assert.AreEqual("The value 'dog' is invalid.", modelState.Errors[0].ErrorMessage);
        }

        [TestMethod]
        public void BindModelCoreRespectsIntersectionOfAllowedPropertyFilters() {
            // Arrange
            MyOtherModel model = new MyOtherModel();
            ModelBindingContextHelper bcHelper = new ModelBindingContextHelper() {
                ValueProvider = GetReflectingValueProvider(),
                Model = model,
                ModelType = typeof(MyOtherModel),
                PropertyFilter = new BindAttribute { Include = "Alpha, Bravo, Echo, Foxtrot" }.IsPropertyAllowed
            };

            DefaultModelBinderHelper helper = new DefaultModelBinderHelper();

            // Act
            helper.PublicBindModelCore(bcHelper.CreateBindingContext());

            // Assert
            Assert.IsNull(model.Alpha, "Alpha is explicitly on the exclude list on the Type.");
            Assert.AreEqual("modelName.BravoValue", model.Bravo, "Bravo is explicitly on the include list in the Property Filter.");
            Assert.IsNull(model.Charlie, "Charlie is not on the include list in the Property Filter.");
            Assert.IsNull(model.Delta, "Delta is not on the include list in the Property Filter.");
            Assert.IsNull(model.Echo, "Echo is explicitly on the exclude list on the Type.");
            Assert.AreEqual("modelName.FoxtrotValue", model.Foxtrot, "Foxtrot is explicitly on the include list in the Property Filter.");
        }

        [TestMethod]
        public void BindModelCanBindSimpleTypes() {
            // Arrange
            Mock<IValueProvider> mockValueProvider = new Mock<IValueProvider>();
            mockValueProvider.Expect(p => p.GetValue("modelName")).Returns(new ValueProviderResult("42", "attemptedValue", null));

            ModelBindingContextHelper bcHelper = new ModelBindingContextHelper() { ValueProvider = mockValueProvider.Object, ModelType = typeof(int) };
            DefaultModelBinder binder = new DefaultModelBinder();

            // Act
            object returned = binder.BindModel(bcHelper.CreateBindingContext()).Value;

            // Assert
            Assert.AreEqual(42, returned);
        }

        [TestMethod]
        public void BindModelCoreReturnsNullIfNoPropertiesOnComplexTypeCanBeUpdated() {
            // Arrange
            ModelBindingContextHelper bcHelper = new ModelBindingContextHelper() {
                Model = new Customer(),
                ModelThunk = () => {
                    Assert.Fail("The thunk should never be called if we never have to read the model.");
                    throw new Exception();
                },
                ModelType = typeof(Customer),
                ModelName = "customer"
            };
            DefaultModelBinderHelper helper = new DefaultModelBinderHelper();

            // Act
            object returned = helper.PublicBindModelCore(bcHelper.CreateBindingContext());

            // Assert
            Assert.IsNull(returned, "BindModelCore() should return null if no properties can be updated.");
        }

        [TestMethod]
        public void BindModelCoreSetsNullablePropertiesToNullIfUserSpecifiedEmptyStringForValue() {
            // Arrange
            IntegerContainer model = new IntegerContainer() { Integer = 42, NullableInteger = 84 };

            Mock<IValueProvider> mockValueProvider = new Mock<IValueProvider>();
            mockValueProvider.Expect(vp => vp.GetValue("modelName.NullableInteger")).Returns(new ValueProviderResult("", "", null));
            ModelBindingContextHelper bcHelper = new ModelBindingContextHelper() {
                ValueProvider = mockValueProvider.Object,
                Model = model,
                ModelType = typeof(IntegerContainer),
            };
            DefaultModelBinderHelper helper = new DefaultModelBinderHelper();

            // Act
            IntegerContainer returned = (IntegerContainer)helper.PublicBindModelCore(bcHelper.CreateBindingContext()).Value;

            // Assert
            Assert.AreEqual(model, returned, "BindModelCore() returned wrong model.");
            Assert.AreEqual(42, model.Integer);
            Assert.IsNull(model.NullableInteger, "BindModelCore() should set a nullable property to null if the user specified empty string.");
        }

        [TestMethod]
        public void BindModelDictionaryReturnsNullIfUsingIndexWireFormatAndNoIndicesFound() {
            // Arrange
            ModelBindingContextHelper bcHelper = new ModelBindingContextHelper() { ModelType = typeof(Dictionary<int, string>) };
            DefaultModelBinder binder = new DefaultModelBinder();

            // Act
            object returned = binder.BindModel(bcHelper.CreateBindingContext());

            // Assert
            Assert.IsNull(returned);
        }

        [TestMethod]
        public void BindModelReturnsEmptyArrayIfUsingIndexWireFormatAndNoIndicesFound() {
            // Arrange
            ModelBindingContextHelper bcHelper = new ModelBindingContextHelper() { ModelType = typeof(string[]) };
            DefaultModelBinder binder = new DefaultModelBinder();

            // Act
            string[] returned = (string[])binder.BindModel(bcHelper.CreateBindingContext()).Value;

            // Assert
            Assert.IsNotNull(returned);
            Assert.AreEqual(0, returned.Length);
        }

        [TestMethod]
        public void BindModelReturnsRawValueIfAlreadyInstanceOfDestinationType() {
            // Arrange
            MyClassWithoutConverter expectedModel = new MyClassWithoutConverter();
            Mock<IValueProvider> mockValueProvider = new Mock<IValueProvider>();
            mockValueProvider.Expect(p => p.GetValue("modelName")).Returns(new ValueProviderResult(expectedModel, "some attempted value", null));

            ModelBindingContextHelper bcHelper = new ModelBindingContextHelper() { ValueProvider = mockValueProvider.Object, ModelType = typeof(MyClassWithoutConverter) };
            DefaultModelBinder binder = new DefaultModelBinder();

            // Act
            object returnedModel = binder.BindModel(bcHelper.CreateBindingContext()).Value;

            // Assert
            Assert.AreSame(expectedModel, returnedModel, "Binder should have short-circuited evaluation if value provider returned instance of type.");

            // even in the event of success, we want to populate the attempted value
            Assert.AreEqual(1, bcHelper.ModelState.Count);
            ModelState modelState = bcHelper.ModelState["modelName"];
            Assert.AreEqual("some attempted value", modelState.AttemptedValue);
            Assert.AreEqual(0, modelState.Errors.Count);
        }

        [TestMethod]
        public void BindModelThrowsIfBindingContextIsNull() {
            // Arrange
            DefaultModelBinder binder = new DefaultModelBinder();

            // Act & assert
            ExceptionHelper.ExpectArgumentNullException(
                delegate {
                    binder.BindModel(null);
                }, "bindingContext");
        }

        [TestMethod]
        public void BindProperty() {
            // Arrange
            ModelBindingContextHelper bcHelper = new ModelBindingContextHelper();

            Mock<IModelBinder> mockPropertyBinder = new Mock<IModelBinder>();
            mockPropertyBinder
                .Expect(b => b.BindModel(It.IsAny<ModelBindingContext>()))
                .Returns(
                    delegate(ModelBindingContext bindingContext) {
                        return new ModelBinderResult(bindingContext.ModelName + "_Value");
                    });

            Mock<DefaultModelBinderHelper> mockHelper = new Mock<DefaultModelBinderHelper>() { CallBase = true };
            mockHelper.Expect(h => h.PublicGetBinder(typeof(string))).Returns(mockPropertyBinder.Object);
            DefaultModelBinderHelper helper = mockHelper.Object;

            // Act
            object returned = helper.PublicBindProperty(bcHelper.CreateBindingContext(), typeof(string), () => null, "propertyName").Value;

            // Assert
            Assert.AreEqual("modelName.propertyName_Value", returned);
        }

        [TestMethod]
        public void ConvertSimpleArrayTypeCanConvertArraysToArrays() {
            // Arrange
            int[] original = new int[] { 1, 20, 42 };

            // Act
            string[] converted = (string[])DefaultModelBinderHelper.PublicConvertSimpleArrayType(CultureInfo.InvariantCulture, original, typeof(string[]));

            // Assert
            Assert.IsNotNull(converted);
            Assert.AreEqual(3, converted.Length);
            Assert.AreEqual("1", converted[0]);
            Assert.AreEqual("20", converted[1]);
            Assert.AreEqual("42", converted[2]);
        }

        [TestMethod]
        public void ConvertSimpleArrayTypeCanConvertArraysToSingleElements() {
            // Arrange
            int[] original = new int[] { 1, 20, 42 };

            // Act
            string converted = (string)DefaultModelBinderHelper.PublicConvertSimpleArrayType(CultureInfo.InvariantCulture, original, typeof(string));

            // Assert
            Assert.AreEqual("1", converted);
        }

        [TestMethod]
        public void ConvertSimpleArrayTypeCanConvertSingleElementsToArrays() {
            // Arrange
            int original = 42;

            // Act
            string[] converted = (string[])DefaultModelBinderHelper.PublicConvertSimpleArrayType(CultureInfo.InvariantCulture, original, typeof(string[]));

            // Assert
            Assert.IsNotNull(converted);
            Assert.AreEqual(1, converted.Length);
            Assert.AreEqual("42", converted[0]);
        }

        [TestMethod]
        public void ConvertSimpleArrayTypeCanConvertSingleElementsToSingleElements() {
            // Arrange
            int original = 42;

            // Act
            string converted = (string)DefaultModelBinderHelper.PublicConvertSimpleArrayType(CultureInfo.InvariantCulture, original, typeof(string));

            // Assert
            Assert.IsNotNull(converted);
            Assert.AreEqual("42", converted);
        }

        [TestMethod]
        public void ConvertSimpleArrayTypeReturnsNullIfTryingToConvertEmptyArrayToSingleElement() {
            // Act
            object outValue = DefaultModelBinderHelper.PublicConvertSimpleArrayType(CultureInfo.InvariantCulture, new int[0], typeof(int));

            // Assert
            Assert.IsNull(outValue);
        }

        [TestMethod]
        public void ConvertSimpleArrayTypeReturnsNullIfValueIsNull() {
            // Act
            object outValue = DefaultModelBinderHelper.PublicConvertSimpleArrayType(CultureInfo.InvariantCulture, null /* value */, typeof(int[]));

            // Assert
            Assert.IsNull(outValue);
        }

        [TestMethod]
        public void ConvertSimpleArrayTypeReturnsValueIfInstanceOfDestinationType() {
            // Arrange
            string[] original = new string[] { "some string" };

            // Act
            object outValue = DefaultModelBinderHelper.PublicConvertSimpleArrayType(CultureInfo.InvariantCulture, original, typeof(string[]));

            // Assert
            Assert.AreSame(original, outValue);
        }

        [TestMethod]
        public void ConvertSimpleTypeChecksCanConvertFrom() {
            // Arrange
            object original = "someValue";

            // Act
            StringContainer returned = (StringContainer)DefaultModelBinderHelper.PublicConvertSimpleType(CultureInfo.GetCultureInfo("fr-FR"), original, typeof(StringContainer));

            // Assert
            Assert.AreEqual(returned.Value, "someValue (fr-FR)");
        }

        [TestMethod]
        public void ConvertSimpleTypeChecksCanConvertTo() {
            // Arrange
            object original = new StringContainer("someValue");

            // Act
            string returned = (string)DefaultModelBinderHelper.PublicConvertSimpleType(CultureInfo.GetCultureInfo("en-US"), original, typeof(string));

            // Assert
            Assert.AreEqual(returned, "someValue (en-US)");
        }

        [TestMethod]
        public void ConvertSimpleTypeReturnsNullIfValueIsEmptyString() {
            // Act
            object outValue = DefaultModelBinderHelper.PublicConvertSimpleType(CultureInfo.InvariantCulture, "", typeof(int));

            // Assert
            Assert.IsNull(outValue);
        }

        [TestMethod]
        public void ConvertSimpleTypeReturnsNullIfValueIsNull() {
            // Act
            object outValue = DefaultModelBinderHelper.PublicConvertSimpleType(CultureInfo.InvariantCulture, null /* value */, typeof(int));

            // Assert
            Assert.IsNull(outValue);
        }

        [TestMethod]
        public void ConvertSimpleTypeReturnsValueIfInstanceOfDestinationType() {
            // Act
            object outValue = DefaultModelBinderHelper.PublicConvertSimpleType(CultureInfo.InvariantCulture, "some string", typeof(object));

            // Assert
            Assert.AreEqual("some string", outValue);
        }

        [TestMethod]
        public void ConvertSimpleTypeThrowsIfConverterThrows() {
            // Arrange
            Type destinationType = typeof(StringContainer);

            // Act & Assert
            // Will throw since the custom converter assumes the first 5 characters to be digits
            InvalidOperationException exception = ExceptionHelper.ExpectInvalidOperationException(
                delegate {
                    DefaultModelBinderHelper.PublicConvertSimpleType(CultureInfo.InvariantCulture, "x", destinationType);
                },
                "The parameter conversion from type 'System.String' to type 'System.Web.Mvc.Test.DefaultModelBinderTest+StringContainer' failed. See the inner exception for more information.");

            Exception innerException = exception.InnerException;
            Assert.AreEqual("Value must have at least 3 characters.", innerException.Message);
        }

        [TestMethod]
        public void ConvertSimpleTypeThrowsIfNoConverterExists() {
            // Arrange
            Type destinationType = typeof(MyClassWithoutConverter);

            // Act & Assert
            ExceptionHelper.ExpectInvalidOperationException(
                delegate {
                    DefaultModelBinderHelper.PublicConvertSimpleType(CultureInfo.InvariantCulture, "x", destinationType);
                },
                "The parameter conversion from type 'System.String' to type 'System.Web.Mvc.Test.DefaultModelBinderTest+MyClassWithoutConverter' failed because no TypeConverter can convert between these types.");
        }

        [TestMethod]
        public void CreateInstanceCreatesModelInstance() {
            // Arrange
            DefaultModelBinderHelper helper = new DefaultModelBinderHelper();

            // Act
            object modelObj = helper.PublicCreateModel(null, typeof(Guid));

            // Assert
            Assert.AreEqual(Guid.Empty, modelObj);
        }

        [TestMethod]
        public void CreateInstanceCreatesModelInstanceForGenericICollection() {
            // Arrange
            DefaultModelBinderHelper helper = new DefaultModelBinderHelper();

            // Act
            object modelObj = helper.PublicCreateModel(null, typeof(ICollection<Guid>));

            // Assert
            Assert.IsInstanceOfType(modelObj, typeof(ICollection<Guid>));
        }

        [TestMethod]
        public void CreateInstanceCreatesModelInstanceForGenericIDictionary() {
            // Arrange
            DefaultModelBinderHelper helper = new DefaultModelBinderHelper();

            // Act
            object modelObj = helper.PublicCreateModel(null, typeof(IDictionary<string, Guid>));

            // Assert
            Assert.IsInstanceOfType(modelObj, typeof(IDictionary<string, Guid>));
        }

        [TestMethod]
        public void CreateInstanceCreatesModelInstanceForGenericIList() {
            // Arrange
            DefaultModelBinderHelper helper = new DefaultModelBinderHelper();

            // Act
            object modelObj = helper.PublicCreateModel(null, typeof(IList<Guid>));

            // Assert
            Assert.IsInstanceOfType(modelObj, typeof(IList<Guid>));
        }

        [TestMethod]
        public void CreateSubIndexNameReturnsPrefixPlusIndex() {
            // Arrange
            DefaultModelBinderHelper helper = new DefaultModelBinderHelper();

            // Act
            string newName = helper.PublicCreateSubIndexName("somePrefix", "someIndex");

            // Assert
            Assert.AreEqual("somePrefix[someIndex]", newName);
        }

        [TestMethod]
        public void CreateSubIndexNameReturnsIndexNameIfPrefixIsEmpty() {
            // Arrange
            DefaultModelBinderHelper helper = new DefaultModelBinderHelper();

            // Act
            string newName = helper.PublicCreateSubIndexName(String.Empty, "someIndex");

            // Assert
            Assert.AreEqual("[someIndex]", newName);
        }

        [TestMethod]
        public void CreateSubIndexNameReturnsIndexNameIfPrefixIsNull() {
            // Arrange
            DefaultModelBinderHelper helper = new DefaultModelBinderHelper();

            // Act
            string newName = helper.PublicCreateSubIndexName(null, "someIndex");

            // Assert
            Assert.AreEqual("[someIndex]", newName);
        }

        [TestMethod]
        public void CreateSubPropertyNameReturnsPrefixPlusPropertyName() {
            // Arrange
            DefaultModelBinderHelper helper = new DefaultModelBinderHelper();

            // Act
            string newName = helper.PublicCreateSubPropertyName("somePrefix", "someProperty");

            // Assert
            Assert.AreEqual("somePrefix.someProperty", newName);
        }

        [TestMethod]
        public void CreateSubPropertyNameReturnsPropertyNameIfPrefixIsEmpty() {
            // Arrange
            DefaultModelBinderHelper helper = new DefaultModelBinderHelper();

            // Act
            string newName = helper.PublicCreateSubPropertyName(String.Empty, "someProperty");

            // Assert
            Assert.AreEqual("someProperty", newName);
        }

        [TestMethod]
        public void CreateSubPropertyNameReturnsPropertyNameIfPrefixIsNull() {
            // Arrange
            DefaultModelBinderHelper helper = new DefaultModelBinderHelper();

            // Act
            string newName = helper.PublicCreateSubPropertyName(null, "someProperty");

            // Assert
            Assert.AreEqual("someProperty", newName);
        }

        [TestMethod]
        public void GetBinderReturnsBinderRegisteredForType() {
            // Arrange
            DefaultModelBinderHelper helper = new DefaultModelBinderHelper();

            // Act
            IModelBinder binder = helper.PublicGetBinder(typeof(MyStringModel));

            // Assert
            Assert.IsInstanceOfType(binder, typeof(MyStringModelBinder));
        }

        [TestMethod]
        public void GetSimpleTypePopulatesModelStateOnFailure() {
            // Arrange
            CultureInfo cultureInfo = CultureInfo.GetCultureInfo("en-GB");
            ValueProviderResult result = new ValueProviderResult("rawValue", "attemptedValue", cultureInfo);

            Mock<IValueProvider> mockValueProvider = new Mock<IValueProvider>();
            mockValueProvider.Expect(p => p.GetValue("modelName")).Returns(result);
            DefaultModelBinderHelper helper = new DefaultModelBinderHelper();

            ModelBindingContextHelper bcHelper = new ModelBindingContextHelper() {
                ValueProvider = mockValueProvider.Object,
                ModelType = typeof(int)
            };

            // Act
            object model = helper.PublicGetSimpleType(bcHelper.CreateBindingContext()).Value;

            // Assert
            Assert.IsNull(model, "GetSimpleType() should return null on failure.");

            ModelStateDictionary modelStateDict = bcHelper.ModelState;
            Assert.AreEqual(1, modelStateDict.Count);

            ModelState modelState = modelStateDict["modelName"];
            Assert.AreEqual("attemptedValue", modelState.AttemptedValue);
            Assert.AreEqual(1, modelState.Errors.Count);

            ModelError modelError = modelState.Errors[0];
            Assert.IsNull(modelError.Exception, "Should not propagate exception to model error dictionary - possible information disclosure.");
            Assert.AreEqual("The value 'attemptedValue' is invalid.", modelError.ErrorMessage);
        }

        [TestMethod]
        public void GetSimpleTypeReturnsConvertedValueOnSuccess() {
            // Arrange
            CultureInfo cultureInfo = CultureInfo.GetCultureInfo("en-GB");
            ValueProviderResult result = new ValueProviderResult("input value", "attemptedValue", cultureInfo);

            Mock<IValueProvider> mockValueProvider = new Mock<IValueProvider>();
            mockValueProvider.Expect(p => p.GetValue("modelName")).Returns(result);
            DefaultModelBinderHelper helper = new DefaultModelBinderHelper();

            ModelBindingContextHelper bcHelper = new ModelBindingContextHelper() {
                ValueProvider = mockValueProvider.Object,
                ModelType = typeof(string)
            };

            // Act
            object model = helper.PublicGetSimpleType(bcHelper.CreateBindingContext()).Value;

            // Assert
            Assert.AreEqual("input value", model);
        }

        [TestMethod]
        public void GetSimpleTypeReturnsNullIfValueProviderReturnsNull() {
            // Arrange
            ModelBindingContextHelper bcHelper = new ModelBindingContextHelper();
            DefaultModelBinderHelper binder = new DefaultModelBinderHelper();

            // Act
            object model = binder.PublicGetSimpleType(bcHelper.CreateBindingContext());

            // Assert
            Assert.IsNull(model, "GetSimpleType() should return null if the value provider returns null.");
        }

        [TestMethod]
        public void GetSimpleTypeSetsModelStateAttemptedValue() {
            // Arrange
            CultureInfo cultureInfo = CultureInfo.GetCultureInfo("en-GB");
            ValueProviderResult result = new ValueProviderResult("42", "some attempted value", cultureInfo);

            Mock<IValueProvider> mockValueProvider = new Mock<IValueProvider>();
            mockValueProvider.Expect(p => p.GetValue("modelName")).Returns(result);
            DefaultModelBinderHelper helper = new DefaultModelBinderHelper();

            ModelBindingContextHelper bcHelper = new ModelBindingContextHelper() {
                ValueProvider = mockValueProvider.Object,
                ModelType = typeof(int)
            };

            // Act
            object model = helper.PublicGetSimpleType(bcHelper.CreateBindingContext()).Value;

            // Assert
            Assert.AreEqual(42, model);

            ModelStateDictionary modelStateDict = bcHelper.ModelState;
            Assert.AreEqual(1, modelStateDict.Count);

            ModelState modelState = modelStateDict["modelName"];
            Assert.AreEqual("some attempted value", modelState.AttemptedValue);
            Assert.AreEqual(0, modelState.Errors.Count);
        }

        [TestMethod]
        public void IsSimpleTypeReturnsFalseForArraysOfReferenceTypes() {
            // Act
            bool IsSimpleType = DefaultModelBinderHelper.PublicIsSimpleType(typeof(object[]));

            // Assert
            Assert.IsFalse(IsSimpleType);
        }

        [TestMethod]
        public void IsSimpleTypeReturnsFalseForReferenceTypes() {
            // Act
            bool IsSimpleType = DefaultModelBinderHelper.PublicIsSimpleType(typeof(object));

            // Assert
            Assert.IsFalse(IsSimpleType);
        }

        [TestMethod]
        public void IsSimpleTypeReturnsTrueForArraysOfValueTypes() {
            // Act
            bool IsSimpleType = DefaultModelBinderHelper.PublicIsSimpleType(typeof(int[]));

            // Assert
            Assert.IsTrue(IsSimpleType);
        }

        [TestMethod]
        public void IsSimpleTypeReturnsTrueForArraysOfStrings() {
            // Act
            bool IsSimpleType = DefaultModelBinderHelper.PublicIsSimpleType(typeof(string[]));

            // Assert
            Assert.IsTrue(IsSimpleType);
        }

        [TestMethod]
        public void IsSimpleTypeReturnsTrueForNullableValueTypes() {
            // Act
            bool IsSimpleType = DefaultModelBinderHelper.PublicIsSimpleType(typeof(int?));

            // Assert
            Assert.IsTrue(IsSimpleType);
        }

        [TestMethod]
        public void IsSimpleTypeReturnsTrueForString() {
            // Act
            bool IsSimpleType = DefaultModelBinderHelper.PublicIsSimpleType(typeof(string));

            // Assert
            Assert.IsTrue(IsSimpleType);
        }

        [TestMethod]
        public void IsSimpleTypeReturnsTrueForValueTypes() {
            // Act
            bool IsSimpleType = DefaultModelBinderHelper.PublicIsSimpleType(typeof(int));

            // Assert
            Assert.IsTrue(IsSimpleType);
        }

        private static ControllerContext GetControllerContext() {
            return new ControllerContext(new Mock<HttpContextBase>().Object, new RouteData(), new Mock<ControllerBase>().Object);
        }

        private static IValueProvider GetReflectingValueProvider() {
            Mock<IValueProvider> mockValueProvider = new Mock<IValueProvider>();
            mockValueProvider
                .Expect(p => p.GetValue(It.IsAny<string>()))
                .Returns(
                    delegate(string name) {
                        return new ValueProviderResult(name + "Value", name + "Value", CultureInfo.InvariantCulture);
                    });
            return mockValueProvider.Object;
        }

        private static IValueProvider GetValueProviderForArray() {
            Mock<IValueProvider> mockValueProvider = new Mock<IValueProvider>();
            mockValueProvider.Expect(p => p.GetValue("modelName.index")).Returns(new ValueProviderResult(new string[] { "0", "1", "2" }, "attemptedValue", null));
            mockValueProvider.Expect(p => p.GetValue("modelName[0].ReadWriteProperty")).Returns(new ValueProviderResult("100", "attemptedValue", null));
            mockValueProvider.Expect(p => p.GetValue("modelName[0].ReadWriteProperty2")).Returns(new ValueProviderResult("101", "attemptedValue", null));
            mockValueProvider.Expect(p => p.GetValue("modelName[1].ReadWriteProperty")).Returns(new ValueProviderResult("200", "attemptedValue", null));
            mockValueProvider.Expect(p => p.GetValue("modelName[1].ReadWriteProperty2")).Returns(new ValueProviderResult("201", "attemptedValue", null));
            mockValueProvider.Expect(p => p.GetValue("modelName[2].ReadWriteProperty")).Returns(new ValueProviderResult("300", "attemptedValue", null));
            mockValueProvider.Expect(p => p.GetValue("modelName[2].ReadWriteProperty2")).Returns(new ValueProviderResult("301", "attemptedValue", null));
            return mockValueProvider.Object;
        }

        private static IValueProvider GetValueProviderForCollection() {
            Mock<IValueProvider> mockValueProvider = new Mock<IValueProvider>();
            mockValueProvider.Expect(p => p.GetValue("modelName.index")).Returns(new ValueProviderResult(new string[] { "0", "1", "2" }, "attemptedValue", null));
            mockValueProvider.Expect(p => p.GetValue("modelName[0]")).Returns(new ValueProviderResult("fooValue", "attemptedValue", null));
            mockValueProvider.Expect(p => p.GetValue("modelName[1]")).Returns(new ValueProviderResult("barValue", "attemptedValue", null));
            mockValueProvider.Expect(p => p.GetValue("modelName[2]")).Returns(new ValueProviderResult("bazValue", "attemptedValue", null));
            return mockValueProvider.Object;
        }

        private static IValueProvider GetValueProviderForDictionary() {
            Mock<IValueProvider> mockValueProvider = new Mock<IValueProvider>();
            mockValueProvider.Expect(p => p.GetValue("modelName.index")).Returns(new ValueProviderResult(new string[] { "0", "1", "2" }, "attemptedValue", null));
            mockValueProvider.Expect(p => p.GetValue("modelName[0].key")).Returns(new ValueProviderResult("fooKey", "attemptedValue", null));
            mockValueProvider.Expect(p => p.GetValue("modelName[0].value")).Returns(new ValueProviderResult("fooValue", "attemptedValue", null));
            mockValueProvider.Expect(p => p.GetValue("modelName[1].key")).Returns(new ValueProviderResult("barKey", "attemptedValue", null));
            mockValueProvider.Expect(p => p.GetValue("modelName[1].value")).Returns(new ValueProviderResult("barValue", "attemptedValue", null));
            mockValueProvider.Expect(p => p.GetValue("modelName[2].key")).Returns(new ValueProviderResult("bazKey", "attemptedValue", null));
            mockValueProvider.Expect(p => p.GetValue("modelName[2].value")).Returns(new ValueProviderResult("bazValue", "attemptedValue", null));
            return mockValueProvider.Object;
        }

        [ModelBinder(typeof(DefaultModelBinder))]
        private class MyModel {
            public int ReadOnlyProperty {
                get { return 4; }
            }
            public int ReadWriteProperty { get; set; }
            public int ReadWriteProperty2 { get; set; }
        }

        private class MyModelWithBadPropertySetter {
            public int NormalProperty { get; set; }
            public int PropertySetterThrows {
                get {
                    return 0;
                }
                set {
                    throw new Exception("This property setter throws.");
                }
            }
            public int PropertySetterThrows2 {
                get {
                    return 0;
                }
                set {
                    throw new Exception("This property setter throws.");
                }
            }
        }

        private class MyClassWithoutConverter {
        }

        [Bind(Exclude = "Alpha,Echo")]
        private class MyOtherModel {
            public string Alpha { get; set; }
            public string Bravo { get; set; }
            public string Charlie { get; set; }
            public string Delta { get; set; }
            public string Echo { get; set; }
            public string Foxtrot { get; set; }
        }

        public class Customer {
            private Address _address = new Address();
            public Address Address {
                get {
                    return _address;
                }
            }
        }

        public class Address {
            public string Street { get; set; }
            public string Zip { get; set; }
        }

        public class IntegerContainer {
            public int Integer { get; set; }
            public int? NullableInteger { get; set; }
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
                if (stringValue == null || stringValue.Length < 3) {
                    throw new Exception("Value must have at least 3 characters.");
                }
                return new StringContainer(AppendCultureName(stringValue, culture));
            }
            public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType) {
                StringContainer container = value as StringContainer;
                if (container.Value == null || container.Value.Length < 3) {
                    throw new Exception("Value must have at least 3 characters.");
                }

                return AppendCultureName(container.Value, culture);
            }

            private static string AppendCultureName(string value, CultureInfo culture) {
                string cultureName = (!String.IsNullOrEmpty(culture.Name)) ? culture.Name : culture.ThreeLetterWindowsLanguageName;
                return value + " (" + cultureName + ")";
            }
        }

        [ModelBinder(typeof(MyStringModelBinder))]
        private class MyStringModel {
            public string Value { get; set; }
        }

        private class MyStringModelBinder : IModelBinder {
            public ModelBinderResult BindModel(ModelBindingContext bindingContext) {
                MyStringModel castModel = bindingContext.Model as MyStringModel;
                if (castModel != null) {
                    castModel.Value += "_Update";
                }
                else {
                    castModel = new MyStringModel() { Value = bindingContext.ModelName + "_Create" };
                }
                return new ModelBinderResult(castModel);
            }
        }

        public class DefaultModelBinderHelper : DefaultModelBinder {
            public virtual ModelBinderResult PublicBindModelCore(ModelBindingContext bindingContext) {
                return base.BindModelCore(bindingContext);
            }
            protected override ModelBinderResult BindModelCore(ModelBindingContext bindingContext) {
                return PublicBindModelCore(bindingContext);
            }
            public static object PublicConvertSimpleType(CultureInfo culture, object value, Type destinationType) {
                return ConvertSimpleType(culture, value, destinationType);
            }
            public static object PublicConvertSimpleArrayType(CultureInfo culture, object value, Type destinationType) {
                return ConvertSimpleArrayType(culture, value, destinationType);
            }
            public virtual object PublicCreateModel(ModelBindingContext bindingContext, Type modelType) {
                return base.CreateModel(bindingContext, modelType);
            }
            protected override object CreateModel(ModelBindingContext bindingContext, Type modelType) {
                return PublicCreateModel(bindingContext, modelType);
            }
            public string PublicCreateSubIndexName(string prefix, string indexName) {
                return CreateSubIndexName(prefix, indexName);
            }
            public string PublicCreateSubPropertyName(string prefix, string propertyName) {
                return CreateSubPropertyName(prefix, propertyName);
            }
            public ModelBinderResult PublicGetSimpleType(ModelBindingContext bindingContext) {
                return GetSimpleType(bindingContext);
            }
            public virtual IModelBinder PublicGetBinder(Type modelType) {
                return base.GetBinder(modelType);
            }
            protected override IModelBinder GetBinder(Type modelType) {
                return PublicGetBinder(modelType);
            }
            public static bool PublicIsSimpleType(Type type) {
                return IsSimpleType(type);
            }
            public ModelBinderResult PublicBindProperty(ModelBindingContext parentContext, Type propertyType, Func<object> propertyValueProvider, string propertyName) {
                return BindProperty(parentContext, propertyType, propertyValueProvider, propertyName);
            }
        }

    }
}
