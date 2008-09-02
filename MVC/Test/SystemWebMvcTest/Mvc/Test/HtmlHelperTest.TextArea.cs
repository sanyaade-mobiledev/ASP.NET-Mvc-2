namespace System.Web.Mvc.Test {
    using System;
    using System.Web.Mvc;
    using System.Web.Routing;
    using System.Web.TestUtil;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    
    public partial class HtmlHelperTest {
        private static readonly RouteValueDictionary _textAreaAttributesDictionary = new RouteValueDictionary(new { rows = "15", cols = "12" });
        private static readonly object _textAreaAttributesObjectDictionary = new { rows = "15", cols = "12" };

        private static ViewDataDictionary GetTextAreaViewData() {
            ViewDataDictionary viewData = new ViewDataDictionary { { "foo", "ViewDataFoo" } };
            viewData.Model = new { foo = "ViewItemFoo", bar = "ViewItemBar" };
            return viewData;
        }

        private static ViewDataDictionary GetTextAreaViewDataWithErrors() {
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
        public void TextAreaWithEmptyNameThrows() {
            // Arrange
            HtmlHelper helper = GetHtmlHelper();

            // Act & Assert
            ExceptionHelper.ExpectArgumentExceptionNullOrEmpty(
                delegate {
                    helper.TextArea(String.Empty);
                },
                "name");
        }

        [TestMethod]
        public void TextAreaWithOutOfRangeColsThrows() {
            // Arrange
            HtmlHelper helper = GetHtmlHelper();

            // Act & Assert
            ExceptionHelper.ExpectArgumentOutOfRangeException(
                delegate {
                    helper.TextArea("Foo", null /* value */, 1, -1, null /* htmlAttributes */);
                },
                "columns",
                @"The value must be greater than or equal to zero.
Parameter name: columns");
        }

        [TestMethod]
        public void TextAreaWithOutOfRangeRowsThrows() {
            // Arrange
            HtmlHelper helper = GetHtmlHelper();

            // Act & Assert
            ExceptionHelper.ExpectArgumentOutOfRangeException(
                delegate {
                    helper.TextArea("Foo", null /* value */, 0, -1, null /* htmlAttributes */);
                },
                "rows",
                @"The value must be greater than or equal to zero.
Parameter name: rows");
        }

        [TestMethod]
        public void TextAreaWithExplicitValue() {
            // Arrange
            HtmlHelper helper = GetHtmlHelper();

            // Act
            string html = helper.TextArea("foo", "bar");

            // Assert
            Assert.AreEqual(@"<textarea cols=""20"" id=""foo"" name=""foo"" rows=""2"">bar</textarea>", html);
        }

        [TestMethod]
        public void TextAreaWithDefaultAttributes() {
            // Arrange
            HtmlHelper helper = GetHtmlHelper(GetTextAreaViewData());

            // Act
            string html = helper.TextArea("foo");

            // Assert
            Assert.AreEqual(@"<textarea cols=""20"" id=""foo"" name=""foo"" rows=""2"">ViewDataFoo</textarea>", html);
        }

        [TestMethod]
        public void TextAreaWithObjectAttributes() {
            // Arrange
            HtmlHelper helper = GetHtmlHelper(GetTextAreaViewData());

            // Act
            string html = helper.TextArea("foo", _textAreaAttributesObjectDictionary);

            // Assert
            Assert.AreEqual(@"<textarea cols=""12"" id=""foo"" name=""foo"" rows=""15"">ViewDataFoo</textarea>", html);
        }

        [TestMethod]
        public void TextAreaWithDictionaryAttributes() {
            // Arrange
            HtmlHelper helper = GetHtmlHelper(GetTextAreaViewData());

            // Act
            string html = helper.TextArea("foo", _textAreaAttributesDictionary);

            // Assert
            Assert.AreEqual(@"<textarea cols=""12"" id=""foo"" name=""foo"" rows=""15"">ViewDataFoo</textarea>", html);
        }

        [TestMethod]
        public void TextAreaWithExplicitValueAndObjectAttributes() {
            // Arrange
            HtmlHelper helper = GetHtmlHelper(GetTextAreaViewData());

            // Act
            string html = helper.TextArea("foo", "Hello World", _textAreaAttributesObjectDictionary);

            // Assert
            Assert.AreEqual(@"<textarea cols=""12"" id=""foo"" name=""foo"" rows=""15"">Hello World</textarea>", html);
        }

        [TestMethod]
        public void TextAreaWithExplicitValueAndDictionaryAttributes() {
            // Arrange
            HtmlHelper helper = GetHtmlHelper(GetTextAreaViewData());

            // Act
            string html = helper.TextArea("foo", "<Hello World>", _textAreaAttributesDictionary);

            // Assert
            Assert.AreEqual(@"<textarea cols=""12"" id=""foo"" name=""foo"" rows=""15"">&lt;Hello World&gt;</textarea>", html);
        }

        [TestMethod]
        public void TextAreaWithNoValueAndObjectAttributes() {
            // Arrange
            HtmlHelper helper = GetHtmlHelper(GetTextAreaViewData());

            // Act
            string html = helper.TextArea("baz", _textAreaAttributesObjectDictionary);

            // Assert
            Assert.AreEqual(@"<textarea cols=""12"" id=""baz"" name=""baz"" rows=""15""></textarea>", html);
        }

        [TestMethod]
        public void TextAreaWithViewDataErrors() {
            // Arrange
            HtmlHelper helper = GetHtmlHelper(GetTextAreaViewDataWithErrors());

            // Act
            string html = helper.TextArea("foo", _textAreaAttributesObjectDictionary);

            // Assert
            Assert.AreEqual(@"<textarea class=""input-validation-error"" cols=""12"" id=""foo"" name=""foo"" rows=""15"">AttemptedValueFoo</textarea>", html);
        }

        [TestMethod]
        public void TextAreaWithViewDataErrorsAndCustomClass() {
            // Arrange
            HtmlHelper helper = GetHtmlHelper(GetTextAreaViewDataWithErrors());

            // Act
            string html = helper.TextArea("foo", new { @class = "foo-class" });

            // Assert
            Assert.AreEqual(@"<textarea class=""input-validation-error foo-class"" cols=""20"" id=""foo"" name=""foo"" rows=""2"">AttemptedValueFoo</textarea>", html);
        }
    }
}
