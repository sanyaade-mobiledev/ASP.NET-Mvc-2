﻿namespace Microsoft.Web.Mvc.Controls.Test {
    using System.Web.Mvc;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Microsoft.Web.Mvc.Controls;

    [TestClass]
    public class DropDownListTest {
        [TestMethod]
        public void NameProperty() {
            // TODO: This
        }

        [TestMethod]
        public void RenderWithNoNameNotInDesignModeThrows() {
            // TODO: This
        }

        [TestMethod]
        public void RenderWithNoNameInDesignModeRendersWithSampleData() {
            // Setup
            DropDownList c = new DropDownList();

            // Execute
            string html = MvcTestHelper.GetControlRendering(c, true);

            // Verify
            Assert.AreEqual<string>(@"<select>
	<option>
		Sample Item
	</option>
</select>", html);
        }

        [TestMethod]
        public void RenderWithNoAttributes() {
            // Setup
            DropDownList c = new DropDownList();
            c.Name = "nameKey";

            ViewDataContainer vdc = new ViewDataContainer();
            vdc.Controls.Add(c);
            vdc.ViewData = new ViewDataDictionary();
            vdc.ViewData["nameKey"] = new SelectList(new[] { "aaa", "bbb", "ccc" }, "bbb");

            // Execute
            string html = MvcTestHelper.GetControlRendering(c, false);

            // Verify
            Assert.AreEqual<string>(@"<select name=""nameKey"" id=""nameKey"">
	<option>
		aaa
	</option><option selected=""selected"">
		bbb
	</option><option>
		ccc
	</option>
</select>", html);
        }

        [TestMethod]
        public void RenderWithTextsAndValues() {
            // Setup
            DropDownList c = new DropDownList();
            c.Name = "nameKey";

            ViewDataContainer vdc = new ViewDataContainer();
            vdc.Controls.Add(c);
            vdc.ViewData = new ViewDataDictionary();
            vdc.ViewData["nameKey"] = new SelectList(
                new[] {
                    new { Text = "aaa", Value = "111" },
                    new { Text = "bbb", Value = "222" },
                    new { Text = "ccc", Value = "333" }
                },
                "Value",
                "Text",
                "222");

            // Execute
            string html = MvcTestHelper.GetControlRendering(c, false);

            // Verify
            Assert.AreEqual<string>(@"<select name=""nameKey"" id=""nameKey"">
	<option value=""111"">
		aaa
	</option><option value=""222"" selected=""selected"">
		bbb
	</option><option value=""333"">
		ccc
	</option>
</select>", html);
        }

        [TestMethod]
        public void RenderWithNameAttributeRendersNameAttribute() {
            // Setup
            DropDownList c = new DropDownList();
            c.Name = "nameKey";
            c.Attributes["NAME"] = "nameAttribute";

            ViewDataContainer vdc = new ViewDataContainer();
            vdc.Controls.Add(c);
            vdc.ViewData = new ViewDataDictionary();
            vdc.ViewData["nameKey"] = new SelectList(new[] { "aaa", "bbb", "ccc" }, "bbb");

            // Execute
            string html = MvcTestHelper.GetControlRendering(c, false);

            // Verify
            Assert.AreEqual<string>(@"<select NAME=""nameAttribute"" id=""nameKey"">
	<option>
		aaa
	</option><option selected=""selected"">
		bbb
	</option><option>
		ccc
	</option>
</select>", html);
        }

        [TestMethod]
        public void RenderWithIdAttributeRendersIdAttribute() {
            // Setup
            DropDownList c = new DropDownList();
            c.Name = "nameKey";
            c.Attributes["ID"] = "idAttribute";

            ViewDataContainer vdc = new ViewDataContainer();
            vdc.Controls.Add(c);
            vdc.ViewData = new ViewDataDictionary();
            vdc.ViewData["nameKey"] = new SelectList(new[] { "aaa", "bbb", "ccc" }, "bbb");

            // Execute
            string html = MvcTestHelper.GetControlRendering(c, false);

            // Verify
            Assert.AreEqual<string>(@"<select ID=""idAttribute"" name=""nameKey"">
	<option>
		aaa
	</option><option selected=""selected"">
		bbb
	</option><option>
		ccc
	</option>
</select>", html);
        }
    }
}
