namespace System.Web.Mvc.Test {
    using System;
    using System.Web.TestUtil;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    public partial class HtmlHelperTest {

        private static readonly ViewDataDictionary _listBoxViewData = new ViewDataDictionary { { "foo", new[] { "Bravo" } } };
        private static readonly ViewDataDictionary _dropDownListViewData = new ViewDataDictionary { { "foo", "Bravo" } };

        private static ViewDataDictionary GetViewDataWithSelectList() {
            ViewDataDictionary viewData = new ViewDataDictionary();
            SelectList selectList = new SelectList(MultiSelectListTest.GetSampleAnonymousObjects(), "Letter", "FullWord", "C");
            viewData["foo"] = selectList;
            return viewData;
        }

        [TestMethod]
        public void DropDownListUsesExplicitValueIfNotProvidedInViewData() {
            // Arrange
            HtmlHelper helper = GetHtmlHelper(new ViewDataDictionary());
            SelectList selectList = new SelectList(MultiSelectListTest.GetSampleAnonymousObjects(), "Letter", "FullWord", "C");

            // Act
            string html = helper.DropDownList(null /* optionLabel */, "foo", selectList);

            // Assert
            Assert.AreEqual(
                @"<select id=""foo"" name=""foo""><option value=""A"">Alpha</option>
<option value=""B"">Bravo</option>
<option selected=""selected"" value=""C"">Charlie</option>
</select>",
                html);
        }

        [TestMethod]
        public void DropDownListUsesViewDataDefaultValue() {
            // Arrange
            HtmlHelper helper = GetHtmlHelper(_dropDownListViewData);
            SelectList selectList = new SelectList(MultiSelectListTest.GetSampleStrings(), "Charlie");

            // Act
            string html = helper.DropDownList(null /* optionLabel */, "foo", selectList);

            // Assert
            Assert.AreEqual(
                @"<select id=""foo"" name=""foo""><option>Alpha</option>
<option selected=""selected"">Bravo</option>
<option>Charlie</option>
</select>",
                html);
        }

        [TestMethod]
        public void DropDownListWithAttributesDictionary() {
            // Arrange
            HtmlHelper helper = GetHtmlHelper(new ViewDataDictionary());
            SelectList selectList = new SelectList(MultiSelectListTest.GetSampleStrings());

            // Act
            string html = helper.DropDownList(null /* optionLabel */, "foo", selectList, _attributesDictionary);

            // Assert
            Assert.AreEqual(
                @"<select baz=""BazValue"" id=""foo"" name=""foo""><option>Alpha</option>
<option>Bravo</option>
<option>Charlie</option>
</select>",
                html);
        }

        [TestMethod]
        public void DropDownListWithEmptyNameThrows() {
            // Arrange
            HtmlHelper helper = GetHtmlHelper(new ViewDataDictionary());

            // Act & Assert
            ExceptionHelper.ExpectArgumentExceptionNullOrEmpty(
                delegate {
                    helper.DropDownList(null /* optionLabel */, String.Empty, (SelectList)null /* selectList */);
                },
                "name");
        }

        [TestMethod]
        public void DropDownListWithNullNameThrows() {
            // Arrange
            HtmlHelper helper = GetHtmlHelper(new ViewDataDictionary());

            // Act & Assert
            ExceptionHelper.ExpectArgumentExceptionNullOrEmpty(
                delegate {
                    helper.DropDownList(null /* optionLabel */, null /* name */, (SelectList)null /* selectList */);
                },
                "name");
        }

        [TestMethod]
        public void DropDownListWithNullSelectListThrows() {
            // Arrange
            HtmlHelper helper = GetHtmlHelper(new ViewDataDictionary());

            // Act & Assert
            ExceptionHelper.ExpectArgumentNullException(
                delegate {
                    helper.DropDownList(null /* optionLabel */, "foo", (SelectList)null /* selectList */);
                },
                "selectList");
        }

        [TestMethod]
        public void DropDownListWithObjectDictionary() {
            // Arrange
            SelectList selectList = new SelectList(MultiSelectListTest.GetSampleStrings());
            ViewDataDictionary viewData = new ViewDataDictionary();
            viewData["foo"] = selectList;
            HtmlHelper helper = GetHtmlHelper(viewData);            

            // Act
            string html = helper.DropDownList(null /* optionLabel */, "foo", _attributesObjectDictionary);

            // Assert
            Assert.AreEqual(
                @"<select baz=""BazObjValue"" id=""foo"" name=""foo""><option>Alpha</option>
<option>Bravo</option>
<option>Charlie</option>
</select>",
                html);
        }

        [TestMethod]
        public void DropDownListWithObjectDictionaryAndSelectList() {
            // Arrange
            HtmlHelper helper = GetHtmlHelper(new ViewDataDictionary());
            SelectList selectList = new SelectList(MultiSelectListTest.GetSampleStrings());

            // Act
            string html = helper.DropDownList(null /* optionLabel */, "foo", selectList, _attributesObjectDictionary);

            // Assert
            Assert.AreEqual(
                @"<select baz=""BazObjValue"" id=""foo"" name=""foo""><option>Alpha</option>
<option>Bravo</option>
<option>Charlie</option>
</select>",
                html);
        }

        [TestMethod]
        public void DropDownListWithObjectDictionaryAndEmptyTitle() {
            // Arrange
            HtmlHelper helper = GetHtmlHelper(new ViewDataDictionary());
            SelectList selectList = new SelectList(MultiSelectListTest.GetSampleStrings());

            // Act
            string html = helper.DropDownList(String.Empty /* optionLabel */, "foo", selectList, _attributesObjectDictionary);

            // Assert
            Assert.AreEqual(
                @"<select baz=""BazObjValue"" id=""foo"" name=""foo""><option>Alpha</option>
<option>Bravo</option>
<option>Charlie</option>
</select>",
                html);
        }
        
        [TestMethod]
        public void DropDownListWithObjectDictionaryAndTitle() {
            // Arrange
            HtmlHelper helper = GetHtmlHelper(new ViewDataDictionary());
            SelectList selectList = new SelectList(MultiSelectListTest.GetSampleStrings());

            // Act
            string html = helper.DropDownList("[Select Something]", "foo", selectList, _attributesObjectDictionary);

            // Assert
            Assert.AreEqual(
                @"<select baz=""BazObjValue"" id=""foo"" name=""foo""><option value="""">[Select Something]</option>
<option>Alpha</option>
<option>Bravo</option>
<option>Charlie</option>
</select>",
                html);
        }

        [TestMethod]
        public void DropDownListUsesViewDataSelectList() {
            // Arrange
            HtmlHelper helper = GetHtmlHelper(GetViewDataWithSelectList());

            // Act
            string html = helper.DropDownList(null /* optionLabel */, "foo");

            // Assert
            Assert.AreEqual(
                @"<select id=""foo"" name=""foo""><option value=""A"">Alpha</option>
<option value=""B"">Bravo</option>
<option selected=""selected"" value=""C"">Charlie</option>
</select>",
                html);
        }

        [TestMethod]
        public void DropDownListWithNullViewDataValueThrows() {
            // Arrange
            HtmlHelper helper = GetHtmlHelper(new ViewDataDictionary());

            // Act
            ExceptionHelper.ExpectException<InvalidOperationException>(
                delegate {
                    helper.DropDownList(null /* optionLabel */, "foo");
                },
                "There is no ViewData item with the key 'foo' of type 'System.Web.Mvc.SelectList'.");
        }

        [TestMethod]
        public void DropDownListWithWrongViewDataTypeValueThrows() {
            // Arrange
            HtmlHelper helper = GetHtmlHelper(new ViewDataDictionary { { "foo", 123 } });

            // Act
            ExceptionHelper.ExpectException<InvalidOperationException>(
                delegate {
                    helper.DropDownList(null /* optionLabel */, "foo");
                },
                "The ViewData item with the key 'foo' is of type 'System.Int32' but needs to be of type 'System.Web.Mvc.SelectList'.");
        }

        [TestMethod]
        public void ListBoxUsesExplicitValueIfNotProvidedInViewData() {
            // Arrange
            HtmlHelper helper = GetHtmlHelper(new ViewDataDictionary());
            MultiSelectList selectList = new MultiSelectList(MultiSelectListTest.GetSampleAnonymousObjects(), "Letter", "FullWord", new[] { "A", "C" });

            // Act
            string html = helper.ListBox("foo", selectList);

            // Assert
            Assert.AreEqual(
                @"<select id=""foo"" multiple=""multiple"" name=""foo""><option selected=""selected"" value=""A"">Alpha</option>
<option value=""B"">Bravo</option>
<option selected=""selected"" value=""C"">Charlie</option>
</select>",
                html);
        }

        [TestMethod]
        public void ListBoxUsesViewDataDefaultValue() {
            // Arrange
            HtmlHelper helper = GetHtmlHelper(_listBoxViewData);
            MultiSelectList selectList = new MultiSelectList(MultiSelectListTest.GetSampleStrings(), new[] { "Charlie" });

            // Act
            string html = helper.ListBox("foo", selectList);

            // Assert
            Assert.AreEqual(
                @"<select id=""foo"" multiple=""multiple"" name=""foo""><option>Alpha</option>
<option selected=""selected"">Bravo</option>
<option>Charlie</option>
</select>",
                html);
        }

        [TestMethod]
        public void ListBoxWithAttributesDictionary() {
            // Arrange
            ViewDataDictionary viewData = new ViewDataDictionary();
            MultiSelectList selectList = new MultiSelectList(MultiSelectListTest.GetSampleStrings());
            viewData["foo"] = selectList;
            HtmlHelper helper = GetHtmlHelper(viewData);

            // Act
            string html = helper.ListBox("foo",  _attributesDictionary);

            // Assert
            Assert.AreEqual(
                @"<select baz=""BazValue"" id=""foo"" multiple=""multiple"" name=""foo""><option>Alpha</option>
<option>Bravo</option>
<option>Charlie</option>
</select>",
                html);
        }

        [TestMethod]
        public void ListBoxWithAttributesDictionaryAndMultiSelectList() {
            // Arrange
            HtmlHelper helper = GetHtmlHelper(new ViewDataDictionary());
            MultiSelectList selectList = new MultiSelectList(MultiSelectListTest.GetSampleStrings());

            // Act
            string html = helper.ListBox("foo", selectList, _attributesDictionary);

            // Assert
            Assert.AreEqual(
                @"<select baz=""BazValue"" id=""foo"" multiple=""multiple"" name=""foo""><option>Alpha</option>
<option>Bravo</option>
<option>Charlie</option>
</select>",
                html);
        }

        [TestMethod]
        public void ListBoxWithEmptyNameThrows() {
            // Arrange
            HtmlHelper helper = GetHtmlHelper(new ViewDataDictionary());

            // Act & Assert
            ExceptionHelper.ExpectArgumentExceptionNullOrEmpty(
                delegate {
                    helper.ListBox(String.Empty, (MultiSelectList)null /* selectList */);
                },
                "name");
        }

        [TestMethod]
        public void ListBoxWithNullNameThrows() {
            // Arrange
            HtmlHelper helper = GetHtmlHelper(new ViewDataDictionary());

            // Act & Assert
            ExceptionHelper.ExpectArgumentExceptionNullOrEmpty(
                delegate {
                    helper.ListBox(null /* name */, (MultiSelectList)null /* selectList */);
                },
                "name");
        }

        [TestMethod]
        public void ListBoxWithNullSelectListThrows() {
            // Arrange
            HtmlHelper helper = GetHtmlHelper(new ViewDataDictionary());

            // Act & Assert
            ExceptionHelper.ExpectArgumentNullException(
                delegate {
                    helper.ListBox("foo", (MultiSelectList)null /* selectList */);
                },
                "selectList");
        }

        [TestMethod]
        public void ListBoxWithObjectDictionary() {
            // Arrange
            HtmlHelper helper = GetHtmlHelper(new ViewDataDictionary());
            MultiSelectList selectList = new MultiSelectList(MultiSelectListTest.GetSampleStrings());

            // Act
            string html = helper.ListBox("foo", selectList, _attributesObjectDictionary);

            // Assert
            Assert.AreEqual(
                @"<select baz=""BazObjValue"" id=""foo"" multiple=""multiple"" name=""foo""><option>Alpha</option>
<option>Bravo</option>
<option>Charlie</option>
</select>",
                html);
        }
    }
}
