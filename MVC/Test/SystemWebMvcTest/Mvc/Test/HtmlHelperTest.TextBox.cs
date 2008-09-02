namespace System.Web.Mvc.Test {
    using System;
    using System.Web.Mvc;
    using System.Web.Routing;
    using System.Web.TestUtil;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    public partial class HtmlHelperTest {

        private static readonly RouteValueDictionary _attributesDictionary = new RouteValueDictionary(new { baz = "BazValue" });
        private static readonly object _attributesObjectDictionary = new { baz = "BazObjValue" };

        private static ViewDataDictionary GetTextBoxViewData() {
            ViewDataDictionary viewData = new ViewDataDictionary { { "foo", "ViewDataFoo" } };
            viewData.Model = new { foo = "ViewItemFoo", bar = "ViewItemBar" };
            return viewData;
        }

        private static ViewDataDictionary GetTextBoxViewDataWithErrors() {
            ViewDataDictionary viewData = new ViewDataDictionary { { "foo", "ViewDataFoo" } };
            viewData.Model = new { foo = "ViewItemFoo", bar = "ViewItemBar" };

            ModelState modelStateFoo = new ModelState();
            modelStateFoo.Errors.Add(new ModelError("foo error 1"));
            modelStateFoo.Errors.Add(new ModelError("foo error 2"));
            viewData.ModelState["foo"] = modelStateFoo;
            modelStateFoo.AttemptedValue = "AttemptedValueFoo";

            return viewData;
        }

        [TestMethod]
        public void TextBoxWithEmptyNameThrows() {
            // Arrange
            HtmlHelper helper = GetHtmlHelper(GetTextBoxViewData());

            // Act & Assert
            ExceptionHelper.ExpectArgumentExceptionNullOrEmpty(
                delegate {
                    helper.TextBox(String.Empty);
                },
                "name");
        }

        [TestMethod]
        public void TextBoxWithExplicitValue() {
            // Arrange
            HtmlHelper helper = GetHtmlHelper(GetTextBoxViewData());

            // Act
            string html = helper.TextBox("foo", "DefaultFoo");

            // Assert
            Assert.AreEqual(@"<input id=""foo"" name=""foo"" type=""text"" value=""DefaultFoo"" />", html);
        }

        [TestMethod]
        public void TextBoxWithExplicitValueAndAttributesDictionary() {
            // Arrange
            HtmlHelper helper = GetHtmlHelper(GetTextBoxViewData());

            // Act
            string html = helper.TextBox("foo", "DefaultFoo", _attributesDictionary);

            // Assert
            Assert.AreEqual(@"<input baz=""BazValue"" id=""foo"" name=""foo"" type=""text"" value=""DefaultFoo"" />", html);
        }

        [TestMethod]
        public void TextBoxWithExplicitValueAndAttributesObject() {
            // Arrange
            HtmlHelper helper = GetHtmlHelper(GetTextBoxViewData());

            // Act
            string html = helper.TextBox("foo", "DefaultFoo", _attributesObjectDictionary);

            // Assert
            Assert.AreEqual(@"<input baz=""BazObjValue"" id=""foo"" name=""foo"" type=""text"" value=""DefaultFoo"" />", html);
        }

        [TestMethod]
        public void TextBoxWithExplicitValueNull() {
            // Arrange
            HtmlHelper helper = GetHtmlHelper(GetTextBoxViewData());

            // Act
            string html = helper.TextBox("foo", (string)null /* value */);

            // Assert
            Assert.AreEqual(@"<input id=""foo"" name=""foo"" type=""text"" value="""" />", html);
        }

        [TestMethod]
        public void TextBoxWithImplicitValue() {
            // Arrange
            HtmlHelper helper = GetHtmlHelper(GetTextBoxViewData());

            // Act
            string html = helper.TextBox("foo");

            // Assert
            Assert.AreEqual(@"<input id=""foo"" name=""foo"" type=""text"" value=""ViewDataFoo"" />", html);
        }

        [TestMethod]
        public void TextBoxWithImplicitValueAndAttributesDictionary() {
            // Arrange
            HtmlHelper helper = GetHtmlHelper(GetTextBoxViewData());

            // Act
            string html = helper.TextBox("foo", _attributesDictionary);

            // Assert
            Assert.AreEqual(@"<input baz=""BazValue"" id=""foo"" name=""foo"" type=""text"" value=""ViewDataFoo"" />", html);
        }

        [TestMethod]
        public void TextBoxWithImplicitValueAndAttributesDictionaryReturnsEmptyValueIfNotFound() {
            // Arrange
            HtmlHelper helper = GetHtmlHelper(GetTextBoxViewData());

            // Act
            string html = helper.TextBox("keyNotFound", _attributesDictionary);

            // Assert
            Assert.AreEqual(@"<input baz=""BazValue"" id=""keyNotFound"" name=""keyNotFound"" type=""text"" value="""" />", html);
        }

        [TestMethod]
        public void TextBoxWithImplicitValueAndAttributesObject() {
            // Arrange
            HtmlHelper helper = GetHtmlHelper(GetTextBoxViewData());

            // Act
            string html = helper.TextBox("foo", _attributesObjectDictionary);

            // Assert
            Assert.AreEqual(@"<input baz=""BazObjValue"" id=""foo"" name=""foo"" type=""text"" value=""ViewDataFoo"" />", html);
        }        

        [TestMethod]
        public void TextBoxWithNullNameThrows() {
            // Arrange
            HtmlHelper helper = GetHtmlHelper(GetTextBoxViewData());

            // Act & Assert
            ExceptionHelper.ExpectArgumentExceptionNullOrEmpty(
                delegate {
                    helper.TextBox(null /* name */);
                },
                "name");
        }

        [TestMethod]
        public void TextBoxWithViewDataErrors() {
            // Arrange
            HtmlHelper helper = GetHtmlHelper(GetTextBoxViewDataWithErrors());

            // Act
            string html = helper.TextBox("foo", _attributesObjectDictionary);

            // Assert
            Assert.AreEqual(@"<input baz=""BazObjValue"" class=""input-validation-error"" id=""foo"" name=""foo"" type=""text"" value=""AttemptedValueFoo"" />", html);
        }

        [TestMethod]
        public void TextBoxWithViewDataErrorsAndCustomClass() {
            // Arrange
            HtmlHelper helper = GetHtmlHelper(GetTextBoxViewDataWithErrors());

            // Act
            string html = helper.TextBox("foo", new { @class = "foo-class" });

            // Assert
            Assert.AreEqual(@"<input class=""input-validation-error foo-class"" id=""foo"" name=""foo"" type=""text"" value=""AttemptedValueFoo"" />", html);
        }
    }
}
