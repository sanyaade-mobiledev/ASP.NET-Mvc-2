namespace System.Web.Mvc.Test {
    using System.Collections;
    using System.Collections.Generic;
    using System.Web.Mvc;
    using System.Web.TestUtil;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class MultiSelectListTest {

        [TestMethod]
        public void Constructor1SetsProperties() {
            // Setup
            IEnumerable items = new object[0];

            // Execute
            MultiSelectList multiSelect = new MultiSelectList(items);

            // Verify
            Assert.AreSame(items, multiSelect.Items);
            Assert.IsNull(multiSelect.DataValueField);
            Assert.IsNull(multiSelect.DataTextField);
            Assert.IsNull(multiSelect.SelectedValues);
        }

        [TestMethod]
        public void Constructor2SetsProperties() {
            // Setup
            IEnumerable items = new object[0];
            IEnumerable selectedValues = new object[0];

            // Execute
            MultiSelectList multiSelect = new MultiSelectList(items, selectedValues);

            // Verify
            Assert.AreSame(items, multiSelect.Items);
            Assert.IsNull(multiSelect.DataValueField);
            Assert.IsNull(multiSelect.DataTextField);
            Assert.AreSame(selectedValues, multiSelect.SelectedValues);
        }

        [TestMethod]
        public void Constructor3SetsProperties() {
            // Setup
            IEnumerable items = new object[0];

            // Execute
            MultiSelectList multiSelect = new MultiSelectList(items, "SomeValueField", "SomeTextField");

            // Verify
            Assert.AreSame(items, multiSelect.Items);
            Assert.AreEqual("SomeValueField", multiSelect.DataValueField);
            Assert.AreEqual("SomeTextField", multiSelect.DataTextField);
            Assert.IsNull(multiSelect.SelectedValues);
        }

        [TestMethod]
        public void Constructor4SetsProperties() {
            // Setup
            IEnumerable items = new object[0];
            IEnumerable selectedValues = new object[0];

            // Execute
            MultiSelectList multiSelect = new MultiSelectList(items, "SomeValueField", "SomeTextField", selectedValues);

            // Verify
            Assert.AreSame(items, multiSelect.Items);
            Assert.AreEqual("SomeValueField", multiSelect.DataValueField);
            Assert.AreEqual("SomeTextField", multiSelect.DataTextField);
            Assert.AreSame(selectedValues, multiSelect.SelectedValues);
        }

        [TestMethod]
        public void ConstructorWithNullItemsThrows() {
            ExceptionHelper.ExpectArgumentNullException(
                delegate {
                    new MultiSelectList(null /* items */, "dataValueField", "dataTextField", null /* selectedValues */);
                }, "items");
        }

        [TestMethod]
        public void GetListItemsSetsEmptyStringValueOnDataBinderFailure() {
            // Setup
            MultiSelectList multiSelect = new MultiSelectList(new[] { "Foo" }, "NonExistentValueField", null /* dataTextField */);

            // Execute
            IList<ListItem> listItems = multiSelect.GetListItems();

            // Verify
            Assert.AreEqual(1, listItems.Count);
            Assert.AreEqual("", listItems[0].Value);
            Assert.AreEqual("Foo", listItems[0].Text);
        }

        [TestMethod]
        public void GetListItemsWithoutValueField() {
            // Setup
            MultiSelectList multiSelect = new MultiSelectList(GetSampleStrings());

            // Execute
            IList<ListItem> listItems = multiSelect.GetListItems();

            // Verify
            Assert.AreEqual(3, listItems.Count);
            Assert.IsNull(listItems[0].Value);
            Assert.AreEqual("Alpha", listItems[0].Text);
            Assert.IsFalse(listItems[0].Selected);
            Assert.IsNull(listItems[1].Value);
            Assert.AreEqual("Bravo", listItems[1].Text);
            Assert.IsFalse(listItems[1].Selected);
            Assert.IsNull(listItems[2].Value);
            Assert.AreEqual("Charlie", listItems[2].Text);
            Assert.IsFalse(listItems[2].Selected);
        }

        [TestMethod]
        public void GetListItemsWithoutValueFieldWithSelections() {
            // Setup
            MultiSelectList multiSelect = new MultiSelectList(GetSampleStrings(), new string[] { "Alpha", "Charlie", "Tango" });

            // Execute
            IList<ListItem> listItems = multiSelect.GetListItems();

            // Verify
            Assert.AreEqual(3, listItems.Count);
            Assert.IsNull(listItems[0].Value);
            Assert.AreEqual("Alpha", listItems[0].Text);
            Assert.IsTrue(listItems[0].Selected);
            Assert.IsNull(listItems[1].Value);
            Assert.AreEqual("Bravo", listItems[1].Text);
            Assert.IsFalse(listItems[1].Selected);
            Assert.IsNull(listItems[2].Value);
            Assert.AreEqual("Charlie", listItems[2].Text);
            Assert.IsTrue(listItems[2].Selected);
        }

        [TestMethod]
        public void GetListItemsWithValueField() {
            // Setup
            MultiSelectList multiSelect = new MultiSelectList(GetSampleAnonymousObjects(), "Letter", "FullWord");

            // Execute
            IList<ListItem> listItems = multiSelect.GetListItems();

            // Verify
            Assert.AreEqual(3, listItems.Count);
            Assert.AreEqual("A", listItems[0].Value);
            Assert.AreEqual("Alpha", listItems[0].Text);
            Assert.IsFalse(listItems[0].Selected);
            Assert.AreEqual("B", listItems[1].Value);
            Assert.AreEqual("Bravo", listItems[1].Text);
            Assert.IsFalse(listItems[1].Selected);
            Assert.AreEqual("C", listItems[2].Value);
            Assert.AreEqual("Charlie", listItems[2].Text);
            Assert.IsFalse(listItems[2].Selected);
        }

        [TestMethod]
        public void GetListItemsWithValueFieldWithSelections() {
            // Setup
            MultiSelectList multiSelect = new MultiSelectList(GetSampleAnonymousObjects(),
                "Letter", "FullWord", new string[] { "A", "C", "T" });

            // Execute
            IList<ListItem> listItems = multiSelect.GetListItems();

            // Verify
            Assert.AreEqual(3, listItems.Count);
            Assert.AreEqual("A", listItems[0].Value);
            Assert.AreEqual("Alpha", listItems[0].Text);
            Assert.IsTrue(listItems[0].Selected);
            Assert.AreEqual("B", listItems[1].Value);
            Assert.AreEqual("Bravo", listItems[1].Text);
            Assert.IsFalse(listItems[1].Selected);
            Assert.AreEqual("C", listItems[2].Value);
            Assert.AreEqual("Charlie", listItems[2].Text);
            Assert.IsTrue(listItems[2].Selected);
        }

        internal static IEnumerable GetSampleAnonymousObjects() {
            return new[] {
                new { Letter = 'A', FullWord = "Alpha" },
                new { Letter = 'B', FullWord = "Bravo" },
                new { Letter = 'C', FullWord = "Charlie" }
            };
        }

        internal static IEnumerable GetSampleStrings() {
            return new string[] { "Alpha", "Bravo", "Charlie" };
        }
    }
}
