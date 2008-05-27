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
            // Setup
            HtmlHelper helper = GetHtmlHelper(new ViewDataDictionary());
            SelectList selectList = new SelectList(MultiSelectListTest.GetSampleAnonymousObjects(), "Letter", "FullWord", "C");

            // Execute
            string html = helper.DropDownList("foo", selectList);

            // Verify
            Assert.AreEqual(
                @"<select name=""foo"" id=""foo""><option value=""A"">Alpha</option>
<option value=""B"">Bravo</option>
<option value=""C"" selected=""selected"">Charlie</option>
</select>",
                html);
        }

        [TestMethod]
        public void DropDownListUsesViewDataDefaultValue() {
            // Setup
            HtmlHelper helper = GetHtmlHelper(_dropDownListViewData);
            SelectList selectList = new SelectList(MultiSelectListTest.GetSampleStrings(), "Charlie");

            // Execute
            string html = helper.DropDownList("foo", selectList);

            // Verify
            Assert.AreEqual(
                @"<select name=""foo"" id=""foo""><option>Alpha</option>
<option selected=""selected"">Bravo</option>
<option>Charlie</option>
</select>",
                html);
        }

        [TestMethod]
        public void DropDownListWithAttributesDictionary() {
            // Setup
            HtmlHelper helper = GetHtmlHelper(new ViewDataDictionary());
            SelectList selectList = new SelectList(MultiSelectListTest.GetSampleStrings());

            // Execute
            string html = helper.DropDownList("foo", selectList, _attributesDictionary);

            // Verify
            Assert.AreEqual(
                @"<select baz=""BazValue"" name=""foo"" id=""foo""><option>Alpha</option>
<option>Bravo</option>
<option>Charlie</option>
</select>",
                html);
        }

        [TestMethod]
        public void DropDownListWithEmptyNameThrows() {
            // Setup
            HtmlHelper helper = GetHtmlHelper(new ViewDataDictionary());

            // Execute & verify
            ExceptionHelper.ExpectArgumentExceptionNullOrEmpty(
                delegate {
                    helper.DropDownList(String.Empty, (SelectList)null /* selectList */);
                },
                "name");
        }

        [TestMethod]
        public void DropDownListWithNullNameThrows() {
            // Setup
            HtmlHelper helper = GetHtmlHelper(new ViewDataDictionary());

            // Execute & verify
            ExceptionHelper.ExpectArgumentExceptionNullOrEmpty(
                delegate {
                    helper.DropDownList(null /* name */, (SelectList)null /* selectList */);
                },
                "name");
        }

        [TestMethod]
        public void DropDownListWithNullSelectListThrows() {
            // Setup
            HtmlHelper helper = GetHtmlHelper(new ViewDataDictionary());

            // Execute & verify
            ExceptionHelper.ExpectArgumentNullException(
                delegate {
                    helper.DropDownList("foo", (SelectList)null /* selectList */);
                },
                "selectList");
        }

        [TestMethod]
        public void DropDownListWithObjectDictionary() {
            // Setup
            HtmlHelper helper = GetHtmlHelper(new ViewDataDictionary());
            SelectList selectList = new SelectList(MultiSelectListTest.GetSampleStrings());

            // Execute
            string html = helper.DropDownList("foo", selectList, _attributesObjectDictionary);

            // Verify
            Assert.AreEqual(
                @"<select baz=""BazObjValue"" name=""foo"" id=""foo""><option>Alpha</option>
<option>Bravo</option>
<option>Charlie</option>
</select>",
                html);
        }

        [TestMethod]
        public void DropDownListUsesViewDataSelectList() {
            // Setup
            HtmlHelper helper = GetHtmlHelper(GetViewDataWithSelectList());

            // Execute
            string html = helper.DropDownList("foo");

            // Verify
            Assert.AreEqual(
                @"<select name=""foo"" id=""foo""><option value=""A"">Alpha</option>
<option value=""B"">Bravo</option>
<option value=""C"" selected=""selected"">Charlie</option>
</select>",
                html);
        }

        [TestMethod]
        public void DropDownListWithNullViewDataValueThrows() {
            // Setup
            HtmlHelper helper = GetHtmlHelper(new ViewDataDictionary());

            // Execute
            ExceptionHelper.ExpectException<InvalidOperationException>(
                delegate {
                    helper.DropDownList("foo");
                },
                "There is no ViewData item with the key 'foo' of type 'System.Web.Mvc.SelectList'.");
        }

        [TestMethod]
        public void DropDownListWithWrongViewDataTypeValueThrows() {
            // Setup
            HtmlHelper helper = GetHtmlHelper(new ViewDataDictionary { { "foo", 123 } });

            // Execute
            ExceptionHelper.ExpectException<InvalidOperationException>(
                delegate {
                    helper.DropDownList("foo");
                },
                "The ViewData item with the key 'foo' is of type 'System.Int32' but needs to be of type 'System.Web.Mvc.SelectList'.");
        }

        [TestMethod]
        public void ListBoxUsesExplicitValueIfNotProvidedInViewData() {
            // Setup
            HtmlHelper helper = GetHtmlHelper(new ViewDataDictionary());
            MultiSelectList selectList = new MultiSelectList(MultiSelectListTest.GetSampleAnonymousObjects(), "Letter", "FullWord", new[] { "A", "C" });

            // Execute
            string html = helper.ListBox("foo", selectList);

            // Verify
            Assert.AreEqual(
                @"<select name=""foo"" id=""foo"" multiple=""multiple""><option value=""A"" selected=""selected"">Alpha</option>
<option value=""B"">Bravo</option>
<option value=""C"" selected=""selected"">Charlie</option>
</select>",
                html);
        }

        [TestMethod]
        public void ListBoxUsesViewDataDefaultValue() {
            // Setup
            HtmlHelper helper = GetHtmlHelper(_listBoxViewData);
            MultiSelectList selectList = new MultiSelectList(MultiSelectListTest.GetSampleStrings(), new[] { "Charlie" });

            // Execute
            string html = helper.ListBox("foo", selectList);

            // Verify
            Assert.AreEqual(
                @"<select name=""foo"" id=""foo"" multiple=""multiple""><option>Alpha</option>
<option selected=""selected"">Bravo</option>
<option>Charlie</option>
</select>",
                html);
        }

        [TestMethod]
        public void ListBoxWithAttributesDictionary() {
            // Setup
            HtmlHelper helper = GetHtmlHelper(new ViewDataDictionary());
            MultiSelectList selectList = new MultiSelectList(MultiSelectListTest.GetSampleStrings());

            // Execute
            string html = helper.ListBox("foo", selectList, _attributesDictionary);

            // Verify
            Assert.AreEqual(
                @"<select baz=""BazValue"" name=""foo"" id=""foo"" multiple=""multiple""><option>Alpha</option>
<option>Bravo</option>
<option>Charlie</option>
</select>",
                html);
        }

        [TestMethod]
        public void ListBoxWithEmptyNameThrows() {
            // Setup
            HtmlHelper helper = GetHtmlHelper(new ViewDataDictionary());

            // Execute & verify
            ExceptionHelper.ExpectArgumentExceptionNullOrEmpty(
                delegate {
                    helper.ListBox(String.Empty, (MultiSelectList)null /* selectList */);
                },
                "name");
        }

        [TestMethod]
        public void ListBoxWithNullNameThrows() {
            // Setup
            HtmlHelper helper = GetHtmlHelper(new ViewDataDictionary());

            // Execute & verify
            ExceptionHelper.ExpectArgumentExceptionNullOrEmpty(
                delegate {
                    helper.ListBox(null /* name */, (MultiSelectList)null /* selectList */);
                },
                "name");
        }

        [TestMethod]
        public void ListBoxWithNullSelectListThrows() {
            // Setup
            HtmlHelper helper = GetHtmlHelper(new ViewDataDictionary());

            // Execute & verify
            ExceptionHelper.ExpectArgumentNullException(
                delegate {
                    helper.ListBox("foo", (MultiSelectList)null /* selectList */);
                },
                "selectList");
        }


        [TestMethod]
        public void ListBoxWithObjectDictionary() {
            // Setup
            HtmlHelper helper = GetHtmlHelper(new ViewDataDictionary());
            MultiSelectList selectList = new MultiSelectList(MultiSelectListTest.GetSampleStrings());

            // Execute
            string html = helper.ListBox("foo", selectList, _attributesObjectDictionary);

            // Verify
            Assert.AreEqual(
                @"<select baz=""BazObjValue"" name=""foo"" id=""foo"" multiple=""multiple""><option>Alpha</option>
<option>Bravo</option>
<option>Charlie</option>
</select>",
                html);
        }
    }
}
