namespace System.Web.Mvc {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Globalization;
    using System.Linq;
    using System.Web.UI;

    [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Multi",
        Justification = "Common shorthand for 'multiple'.")]
    [AspNetHostingPermission(System.Security.Permissions.SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
    [AspNetHostingPermission(System.Security.Permissions.SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
    public class MultiSelectList {

        public MultiSelectList(IEnumerable items)
            : this(items, null /* selectedValues */) {
        }

        public MultiSelectList(IEnumerable items, IEnumerable selectedValues)
            : this(items, null /* dataValuefield */, null /* dataTextField */, selectedValues) {
        }

        public MultiSelectList(IEnumerable items, string dataValueField, string dataTextField)
            : this(items, dataValueField, dataTextField, null /* selectedValues */) {
        }

        public MultiSelectList(IEnumerable items, string dataValueField, string dataTextField, IEnumerable selectedValues) {
            if (items == null) {
                throw new ArgumentNullException("items");
            }

            Items = items;
            DataValueField = dataValueField;
            DataTextField = dataTextField;
            SelectedValues = selectedValues;
        }

        public string DataTextField {
            get;
            private set;
        }

        public string DataValueField {
            get;
            private set;
        }

        public IEnumerable Items {
            get;
            private set;
        }

        public IEnumerable SelectedValues {
            get;
            private set;
        }

        [SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate",
            Justification = "Operation performs conversions and returns a unique instance on each call.")]
        public virtual IList<ListItem> GetListItems() {
            return (!String.IsNullOrEmpty(DataValueField)) ?
                GetListItemsWithValueField() :
                GetListItemsWithoutValueField();
        }

        private IList<ListItem> GetListItemsWithValueField() {
            HashSet<string> selectedValues = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            if (SelectedValues != null) {
                selectedValues.UnionWith(from object value in SelectedValues select Convert.ToString(value, CultureInfo.InvariantCulture));
            }

            var listItems = from object item in Items
                            let value = Eval(item, DataValueField)
                            select new ListItem {
                                Value = value,
                                Text = Eval(item, DataTextField),
                                Selected = selectedValues.Contains(value)
                            };
            return listItems.ToList();
        }

        private IList<ListItem> GetListItemsWithoutValueField() {
            HashSet<object> selectedValues = new HashSet<object>();
            if (SelectedValues != null) {
                selectedValues.UnionWith(SelectedValues.Cast<object>());
            }

            var listItems = from object item in Items
                            select new ListItem {
                                Text = Eval(item, DataTextField),
                                Selected = selectedValues.Contains(item)
                            };
            return listItems.ToList();
        }

        private static string Eval(object container, string expression) {
            object value = container;
            if (!String.IsNullOrEmpty(expression)) {
                value = DataBinder.Eval(container, expression);
            }
            return Convert.ToString(value, CultureInfo.InvariantCulture);
        }
    }
}
