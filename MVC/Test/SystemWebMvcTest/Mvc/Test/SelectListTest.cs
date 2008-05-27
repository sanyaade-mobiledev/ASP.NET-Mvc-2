namespace System.Web.Mvc.Test {
    using System;
    using System.Collections;
    using System.Reflection;
    using System.Web.TestUtil;
    using System.Web.Mvc;
    using System.Linq;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System.Collections.Generic;

    [TestClass]
    public class SelectListTest {

        [TestMethod]
        public void Constructor1SetsProperties() {
            // Setup
            IEnumerable items = new object[0];

            // Execute
            SelectList selectList = new SelectList(items);

            // Verify
            Assert.AreSame(items, selectList.Items);
            Assert.IsNull(selectList.DataValueField);
            Assert.IsNull(selectList.DataTextField);
            Assert.IsNull(selectList.SelectedValues);
            Assert.IsNull(selectList.SelectedValue);
        }

        [TestMethod]
        public void Constructor2SetsProperties() {
            // Setup
            IEnumerable items = new object[0];
            object selectedValue = new object();

            // Execute
            SelectList selectList = new SelectList(items, selectedValue);
            List<object> selectedValues = selectList.SelectedValues.Cast<object>().ToList();

            // Verify
            Assert.AreSame(items, selectList.Items);
            Assert.IsNull(selectList.DataValueField);
            Assert.IsNull(selectList.DataTextField);
            Assert.AreSame(selectedValue, selectList.SelectedValue);
            Assert.AreEqual(1, selectedValues.Count);
            Assert.AreSame(selectedValue, selectedValues[0]);
        }

        [TestMethod]
        public void Constructor3SetsProperties() {
            // Setup
            IEnumerable items = new object[0];

            // Execute
            SelectList selectList = new SelectList(items, "SomeValueField", "SomeTextField");

            // Verify
            Assert.AreSame(items, selectList.Items);
            Assert.AreEqual("SomeValueField", selectList.DataValueField);
            Assert.AreEqual("SomeTextField", selectList.DataTextField);
            Assert.IsNull(selectList.SelectedValues);
            Assert.IsNull(selectList.SelectedValue);
        }

        [TestMethod]
        public void Constructor4SetsProperties() {
            // Setup
            IEnumerable items = new object[0];
            object selectedValue = new object();

            // Execute
            SelectList selectList = new SelectList(items, "SomeValueField", "SomeTextField", selectedValue);
            List<object> selectedValues = selectList.SelectedValues.Cast<object>().ToList();

            // Verify
            Assert.AreSame(items, selectList.Items);
            Assert.AreEqual("SomeValueField", selectList.DataValueField);
            Assert.AreEqual("SomeTextField", selectList.DataTextField);
            Assert.AreSame(selectedValue, selectList.SelectedValue);
            Assert.AreEqual(1, selectedValues.Count);
            Assert.AreSame(selectedValue, selectedValues[0]);
        }
    }
}
