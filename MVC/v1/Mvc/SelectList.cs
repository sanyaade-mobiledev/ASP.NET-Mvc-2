﻿namespace System.Web.Mvc {
    using System.Collections;

    [AspNetHostingPermission(System.Security.Permissions.SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
    [AspNetHostingPermission(System.Security.Permissions.SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
    public class SelectList : MultiSelectList {

        public SelectList(IEnumerable items)
            : this(items, null /* selectedValue */) {
        }

        public SelectList(IEnumerable items, object selectedValue)
            : this(items, null /* dataValuefield */, null /* dataTextField */, selectedValue) {
        }

        public SelectList(IEnumerable items, string dataValueField, string dataTextField)
            : this(items, dataValueField, dataTextField, null /* selectedValue */) {
        }

        public SelectList(IEnumerable items, string dataValueField, string dataTextField, object selectedValue)
            : base(items, dataValueField, dataTextField, ToEnumerable(selectedValue)) {
            SelectedValue = selectedValue;
        }

        public object SelectedValue {
            get;
            private set;
        }

        private static IEnumerable ToEnumerable(object selectedValue) {
            return (selectedValue != null) ? new object[] { selectedValue } : null;
        }
    }
}