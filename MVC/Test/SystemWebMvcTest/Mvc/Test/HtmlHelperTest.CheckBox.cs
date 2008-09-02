namespace System.Web.Mvc.Test {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Web.Mvc;
    using System.Web.Routing;
    using System.Web.TestUtil;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    public partial class HtmlHelperTest {

        private static ViewDataDictionary GetCheckBoxViewData() {
            ViewDataDictionary viewData = new ViewDataDictionary { { "foo", true }, { "bar", "NotTrue"} };
            viewData.Model = new { foo = "ViewItemFoo", bar = "ViewItemBar" };
            return viewData;
        }

        [TestMethod]
        public void CheckBoxWithEmptyNameThrows() {
            // Arrange
            HtmlHelper helper = GetHtmlHelper(GetCheckBoxViewData());

            // Act & Assert
            ExceptionHelper.ExpectArgumentExceptionNullOrEmpty(
                delegate {
                    helper.CheckBox(String.Empty);
                },
                "name");
        }

        [TestMethod]
        public void CheckBoxWithInvalidBooleanThrows() {
            // Arrange
            HtmlHelper helper = GetHtmlHelper(GetCheckBoxViewData());

            // Act & Assert
            ExceptionHelper.ExpectException<FormatException>(
                delegate {
                    helper.CheckBox("bar");
                },
                "String was not recognized as a valid Boolean.");
        }

        [TestMethod]
        public void CheckBoxCheckedWithOnlyName() {
            // Arrange
            HtmlHelper helper = GetHtmlHelper();

            // Act
            string html = helper.CheckBox("foo", true /* isChecked */);

            // Assert
            Assert.AreEqual(@"<input checked=""checked"" id=""foo"" name=""foo"" type=""checkbox"" value=""true"" />
<input name=""foo"" type=""hidden"" value=""false"" />
", html);
        }

        [TestMethod]
        public void CheckBoxWithOnlyName() {
            // Arrange
            HtmlHelper helper = GetHtmlHelper(GetCheckBoxViewData());

            // Act
            string html = helper.CheckBox("foo");

            Assert.AreEqual(@"<input checked=""checked"" id=""foo"" name=""foo"" type=""checkbox"" value=""true"" />
<input name=""foo"" type=""hidden"" value=""false"" />
", html);
        }

        [TestMethod]
        public void CheckBoxWithNameAndObjectAttribute() {
            // Arrange
            HtmlHelper helper = GetHtmlHelper(GetCheckBoxViewData());

            // Act
            string html = helper.CheckBox("foo", _attributesObjectDictionary);

            // Assert
            Assert.AreEqual(@"<input baz=""BazObjValue"" checked=""checked"" id=""foo"" name=""foo"" type=""checkbox"" value=""true"" />
<input baz=""BazObjValue"" name=""foo"" type=""hidden"" value=""false"" />
", html);
        }

        [TestMethod]
        public void CheckBoxWithObjectAttribute() {
            // Arrange
            HtmlHelper helper = GetHtmlHelper();

            // Act
            string html = helper.CheckBox("foo", false /* isChecked */,_attributesObjectDictionary);

            // Assert
            Assert.AreEqual(@"<input baz=""BazObjValue"" id=""foo"" name=""foo"" type=""checkbox"" value=""true"" />
<input baz=""BazObjValue"" name=""foo"" type=""hidden"" value=""false"" />
", html);
        }

        [TestMethod]
        public void CheckBoxWithAttributeDictionary() {
            // Arrange
            HtmlHelper helper = GetHtmlHelper();

            // Act
            string html = helper.CheckBox("foo", false /* isChecked */, _attributesDictionary);

            // Assert
            Assert.AreEqual(@"<input baz=""BazValue"" id=""foo"" name=""foo"" type=""checkbox"" value=""true"" />
<input baz=""BazValue"" name=""foo"" type=""hidden"" value=""false"" />
", html);
        }
    }
}
