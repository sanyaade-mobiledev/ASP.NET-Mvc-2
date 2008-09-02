namespace System.Web.Mvc.Test {
    using System;
    using System.Web.Mvc;
    using System.Web.TestUtil;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    public partial class HtmlHelperTest {
        private static ViewDataDictionary GetRadioButtonViewData() {
            ViewDataDictionary viewData = new ViewDataDictionary { { "foo", "ViewDataFoo" } };
            viewData.Model = new { foo = "ViewItemFoo", bar = "ViewItemBar" };
            return viewData;
        }

        [TestMethod]
        public void RadioButtonWithEmptyNameThrows() {
            // Arrange
            HtmlHelper helper = GetHtmlHelper();

            // Act & Assert
            ExceptionHelper.ExpectArgumentExceptionNullOrEmpty(
                delegate {
                    helper.RadioButton(String.Empty);
                },
                "name");
        }

        [TestMethod]
        public void RadioButtonWithOnlyName() {
            // Arrange
            HtmlHelper helper = GetHtmlHelper(GetRadioButtonViewData());

            // Act
            string html = helper.RadioButton("foo");

            // Assert
            Assert.AreEqual(@"<input id=""foo"" name=""foo"" type=""radio"" value=""ViewDataFoo"" />", html);
        }

        [TestMethod]
        public void RadioButtonCheckedWithOnlyName() {
            // Arrange
            HtmlHelper helper = GetHtmlHelper(GetRadioButtonViewData());

            // Act
            string html = helper.RadioButton("foo", true /* isChecked */);

            // Assert
            Assert.AreEqual(@"<input checked=""checked"" id=""foo"" name=""foo"" type=""radio"" value=""ViewDataFoo"" />", html);
        }

        [TestMethod]
        public void RadioButtonWithObjectAttribute() {
            // Arrange
            HtmlHelper helper = GetHtmlHelper(GetRadioButtonViewData());

            // Act
            string html = helper.RadioButton("foo", false /* isChecked */,_attributesObjectDictionary);

            // Assert
            Assert.AreEqual(@"<input baz=""BazObjValue"" id=""foo"" name=""foo"" type=""radio"" value=""ViewDataFoo"" />", html);
        }

        [TestMethod]
        public void RadioButtonWithAttributeDictionary() {
            // Arrange
            HtmlHelper helper = GetHtmlHelper(GetRadioButtonViewData());

            // Act
            string html = helper.RadioButton("foo", false /* isChecked */, _attributesDictionary);

            // Assert
            Assert.AreEqual(@"<input baz=""BazValue"" id=""foo"" name=""foo"" type=""radio"" value=""ViewDataFoo"" />", html);
        }

        [TestMethod]
        public void RadioButtonWithExplicitValue() {
            // Arrange
            HtmlHelper helper = GetHtmlHelper(GetRadioButtonViewData());

            // Act
            string html = helper.RadioButton("foo", "bar", false /* isChecked */);

            // Assert
            Assert.AreEqual(@"<input id=""foo"" name=""foo"" type=""radio"" value=""bar"" />", html);
        }

        [TestMethod]
        public void RadioButtonWithExplicitValueAndObjectAttribute() {
            // Arrange
            HtmlHelper helper = GetHtmlHelper(GetRadioButtonViewData());

            // Act
            string html = helper.RadioButton("foo", "bar", false /* isChecked */, _attributesObjectDictionary);

            // Assert
            Assert.AreEqual(@"<input baz=""BazObjValue"" id=""foo"" name=""foo"" type=""radio"" value=""bar"" />", html);
        }

        [TestMethod]
        public void RadioButtonWithExplicitValueAndAttributeDictionary() {
            // Arrange
            HtmlHelper helper = GetHtmlHelper(GetRadioButtonViewData());

            // Act
            string html = helper.RadioButton("foo", "bar", false /* isChecked */, _attributesDictionary);

            // Assert
            Assert.AreEqual(@"<input baz=""BazValue"" id=""foo"" name=""foo"" type=""radio"" value=""bar"" />", html);
        }        
    }
}
