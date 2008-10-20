namespace System.Web.Mvc.Test {
    using System;
    using System.Web.Mvc;
    using System.Web.TestUtil;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class ModelStateDictionaryTest {

        [TestMethod]
        public void AddModelErrorCreatesModelStateIfNotPresent() {
            // Arrange
            ModelStateDictionary dictionary = new ModelStateDictionary();

            // Act
            dictionary.AddModelError("some key", "some error");

            // Assert
            Assert.AreEqual(1, dictionary.Count);
            ModelState modelState = dictionary["some key"];

            Assert.AreEqual(1, modelState.Errors.Count);
            Assert.AreEqual("some error", modelState.Errors[0].ErrorMessage);
        }

        [TestMethod]
        public void AddModelErrorThrowsIfKeyIsEmpty() {
            // Arrange
            ModelStateDictionary dictionary = new ModelStateDictionary();

            // Act & Assert
            ExceptionHelper.ExpectArgumentExceptionNullOrEmpty(
                delegate {
                    dictionary.AddModelError(String.Empty, (string)null);
                }, "key");
        }

        [TestMethod]
        public void AddModelErrorThrowsIfKeyIsNull() {
            // Arrange
            ModelStateDictionary dictionary = new ModelStateDictionary();

            // Act & Assert
            ExceptionHelper.ExpectArgumentExceptionNullOrEmpty(
                delegate {
                    dictionary.AddModelError(null, (string)null);
                }, "key");
        }

        [TestMethod]
        public void AddModelErrorUsesExistingModelStateIfPresent() {
            // Arrange
            ModelStateDictionary dictionary = new ModelStateDictionary();
            dictionary.AddModelError("some key", "some error");
            Exception ex = new Exception();

            // Act
            dictionary.AddModelError("some key", ex);

            // Assert
            Assert.AreEqual(1, dictionary.Count);
            ModelState modelState = dictionary["some key"];

            Assert.AreEqual(2, modelState.Errors.Count);
            Assert.AreEqual("some error", modelState.Errors[0].ErrorMessage);
            Assert.AreSame(ex, modelState.Errors[1].Exception);
        }

        [TestMethod]
        public void Constructor() {
            // Act
            ModelStateDictionary dictionary = new ModelStateDictionary();

            // Assert
            Assert.AreEqual(0, dictionary.Count);
            Assert.AreEqual(StringComparer.OrdinalIgnoreCase, dictionary.Comparer);
        }

        [TestMethod]
        public void ConstructorWithDictionaryParameter() {
            // Arrange
            ModelStateDictionary oldDictionary = new ModelStateDictionary() {
                { "foo", new ModelState() { AttemptedValue = "bar" } }
            };

            // Act
            ModelStateDictionary newDictionary = new ModelStateDictionary(oldDictionary);

            // Assert
            Assert.AreEqual(1, newDictionary.Count);
            Assert.AreEqual(StringComparer.OrdinalIgnoreCase, newDictionary.Comparer);
            Assert.AreEqual("bar", newDictionary["foo"].AttemptedValue);
        }

        [TestMethod]
        public void IsValidPropertyReturnsFalseIfErrors() {
            // Arrange
            ModelState errorState = new ModelState() { AttemptedValue = "quux" };
            errorState.Errors.Add("some error");
            ModelStateDictionary dictionary = new ModelStateDictionary() {
                { "foo", new ModelState() { AttemptedValue = "bar" } },
                { "baz", errorState }
            };

            // Act
            bool isValid = dictionary.IsValid;

            // Assert
            Assert.IsFalse(isValid);
        }

        [TestMethod]
        public void IsValidPropertyReturnsTrueIfNoErrors() {
            // Arrange
            ModelStateDictionary dictionary = new ModelStateDictionary() {
                { "foo", new ModelState() { AttemptedValue = "bar" } },
                { "baz", new ModelState() { AttemptedValue = "quux" } }
            };

            // Act
            bool isValid = dictionary.IsValid;

            // Assert
            Assert.IsTrue(isValid);
        }

        [TestMethod]
        public void MergeCopiesDictionaryEntries() {
            // Arrange
            ModelStateDictionary fooDict = new ModelStateDictionary() { { "foo", new ModelState() } };
            ModelStateDictionary barDict = new ModelStateDictionary() { { "bar", new ModelState() } };

            // Act
            fooDict.Merge(barDict);

            // Assert
            Assert.AreEqual(2, fooDict.Count);
            Assert.AreEqual(barDict["bar"], fooDict["bar"]);
        }

        [TestMethod]
        public void SetAttemptedValueCreatesModelStateIfNotPresent() {
            // Arrange
            ModelStateDictionary dictionary = new ModelStateDictionary();

            // Act
            dictionary.SetAttemptedValue("some key", "some value");

            // Assert
            Assert.AreEqual(1, dictionary.Count);
            ModelState modelState = dictionary["some key"];

            Assert.AreEqual(0, modelState.Errors.Count);
            Assert.AreEqual("some value", modelState.AttemptedValue);
        }

        [TestMethod]
        public void SetAttemptedValueUsesExistingModelStateIfPresent() {
            // Arrange
            ModelStateDictionary dictionary = new ModelStateDictionary();
            dictionary.AddModelError("some key", "some error");
            Exception ex = new Exception();

            // Act
            dictionary.SetAttemptedValue("some key", "some value");

            // Assert
            Assert.AreEqual(1, dictionary.Count);
            ModelState modelState = dictionary["some key"];

            Assert.AreEqual(1, modelState.Errors.Count);
            Assert.AreEqual("some error", modelState.Errors[0].ErrorMessage);
            Assert.AreEqual("some value", modelState.AttemptedValue);
        }

    }
}
