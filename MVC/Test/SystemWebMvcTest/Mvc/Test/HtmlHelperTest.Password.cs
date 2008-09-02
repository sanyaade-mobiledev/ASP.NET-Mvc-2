namespace System.Web.Mvc.Test {
    using System;
    using System.Web.Routing;
    using System.Web.TestUtil;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    public partial class HtmlHelperTest {

        private static ViewDataDictionary GetPasswordViewData() {
            return new ViewDataDictionary { { "foo", "ViewDataFoo" } };
        }

        private static ViewDataDictionary GetPasswordViewDataWithErrors() {
            ViewDataDictionary viewData = new ViewDataDictionary { { "foo", "ViewDataFoo" } };

            ModelState modelStateFoo = new ModelState();
            modelStateFoo.Errors.Add(new ModelError("foo error 1"));
            modelStateFoo.Errors.Add(new ModelError("foo error 2"));
            viewData.ModelState["foo"] = modelStateFoo;
            modelStateFoo.AttemptedValue = "AttemptedValueFoo";

            return viewData;
        }

        [TestMethod]
        public void PasswordWithEmptyNameThrows() {
            // Arrange
            HtmlHelper helper = GetHtmlHelper(GetPasswordViewData());

            // Act & Assert
            ExceptionHelper.ExpectArgumentExceptionNullOrEmpty(
                delegate {
                    helper.Password(String.Empty);
                },
                "name");
        }

        [TestMethod]
        public void PasswordWithExplicitValue() {
            // Arrange
            HtmlHelper helper = GetHtmlHelper(GetPasswordViewData());

            // Act
            string html = helper.Password("foo", "DefaultFoo");

            // Assert
            Assert.AreEqual(@"<input id=""foo"" name=""foo"" type=""password"" value=""DefaultFoo"" />", html);
        }

        [TestMethod]
        public void PasswordWithExplicitValueAndAttributesDictionary() {
            // Arrange
            HtmlHelper helper = GetHtmlHelper(GetPasswordViewData());

            // Act
            string html = helper.Password("foo", "DefaultFoo", _attributesDictionary);

            // Assert
            Assert.AreEqual(@"<input baz=""BazValue"" id=""foo"" name=""foo"" type=""password"" value=""DefaultFoo"" />", html);
        }

        [TestMethod]
        public void PasswordWithExplicitValueAndAttributesObject() {
            // Arrange
            HtmlHelper helper = GetHtmlHelper(GetPasswordViewData());

            // Act
            string html = helper.Password("foo", "DefaultFoo", _attributesObjectDictionary);

            // Assert
            Assert.AreEqual(@"<input baz=""BazObjValue"" id=""foo"" name=""foo"" type=""password"" value=""DefaultFoo"" />", html);
        }

        [TestMethod]
        public void PasswordWithExplicitValueNull() {
            // Arrange
            HtmlHelper helper = GetHtmlHelper(GetPasswordViewData());

            // Act
            string html = helper.Password("foo", (string)null /* value */);

            // Assert
            Assert.AreEqual(@"<input id=""foo"" name=""foo"" type=""password"" value="""" />", html);
        }

        [TestMethod]
        public void PasswordWithImplicitValue() {
            // Arrange
            HtmlHelper helper = GetHtmlHelper(GetPasswordViewData());

            // Act
            string html = helper.Password("foo");

            // Assert
            Assert.AreEqual(@"<input id=""foo"" name=""foo"" type=""password"" value=""ViewDataFoo"" />", html);
        }

        [TestMethod]
        public void PasswordWithImplicitValueAndAttributesDictionary() {
            // Arrange
            HtmlHelper helper = GetHtmlHelper(GetPasswordViewData());

            // Act
            string html = helper.Password("foo", _attributesDictionary);

            // Assert
            Assert.AreEqual(@"<input baz=""BazValue"" id=""foo"" name=""foo"" type=""password"" value=""ViewDataFoo"" />", html);
        }

        [TestMethod]
        public void PasswordWithImplicitValueAndAttributesDictionaryReturnsEmptyValueIfNotFound() {
            // Arrange
            HtmlHelper helper = GetHtmlHelper(GetPasswordViewData());

            // Act
            string html = helper.Password("keyNotFound", _attributesDictionary);

            // Assert
            Assert.AreEqual(@"<input baz=""BazValue"" id=""keyNotFound"" name=""keyNotFound"" type=""password"" value="""" />", html);
        }

        [TestMethod]
        public void PasswordWithImplicitValueAndAttributesObject() {
            // Arrange
            HtmlHelper helper = GetHtmlHelper(GetPasswordViewData());

            // Act
            string html = helper.Password("foo", _attributesObjectDictionary);

            // Assert
            Assert.AreEqual(@"<input baz=""BazObjValue"" id=""foo"" name=""foo"" type=""password"" value=""ViewDataFoo"" />", html);
        }

        [TestMethod]
        public void PasswordWithNullNameThrows() {
            // Arrange
            HtmlHelper helper = GetHtmlHelper(GetPasswordViewData());

            // Act & Assert
            ExceptionHelper.ExpectArgumentExceptionNullOrEmpty(
                delegate {
                    helper.Password(null /* name */);
                },
                "name");
        }

        [TestMethod]
        public void PasswordWithViewDataErrors() {
            // Arrange
            HtmlHelper helper = GetHtmlHelper(GetPasswordViewDataWithErrors());

            // Act
            string html = helper.Password("foo", _attributesObjectDictionary);

            // Assert
            Assert.AreEqual(@"<input baz=""BazObjValue"" class=""input-validation-error"" id=""foo"" name=""foo"" type=""password"" value=""AttemptedValueFoo"" />", html);
        }

        [TestMethod]
        public void PasswordWithViewDataErrorsAndCustomClass() {
            // Arrange
            HtmlHelper helper = GetHtmlHelper(GetPasswordViewDataWithErrors());

            // Act
            string html = helper.Password("foo", new { @class = "foo-class" });

            // Assert
            Assert.AreEqual(@"<input class=""input-validation-error foo-class"" id=""foo"" name=""foo"" type=""password"" value=""AttemptedValueFoo"" />", html);
        }
    }
}
