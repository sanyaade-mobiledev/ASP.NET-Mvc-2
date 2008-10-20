namespace Microsoft.Web.Mvc {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Mvc.Html;
    using System.Web.Routing;
    using Microsoft.Web.Mvc.Resources;

    [AspNetHostingPermission(System.Security.Permissions.SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
    public static class RadioListExtensions {
        public static string[] RadioButtonList(this HtmlHelper htmlHelper, string name) {
            return RadioButtonList(htmlHelper, name, (IDictionary<string, object>)null);
        }

        public static string[] RadioButtonList(this HtmlHelper htmlHelper, string name, object htmlAttributes) {
            return RadioButtonList(htmlHelper, name, new RouteValueDictionary(htmlAttributes));
        }

        public static string[] RadioButtonList(this HtmlHelper htmlHelper, string name, IDictionary<string, object> htmlAttributes) {
            SelectList selectList = htmlHelper.GetSelectData(name);
            return htmlHelper.RadioButtonListInternal(name, selectList, true /* usedViewData */, htmlAttributes);
        }

        public static string[] RadioButtonList(this HtmlHelper htmlHelper, string name, SelectList selectList) {
            return RadioButtonList(htmlHelper, name, selectList, (IDictionary<string, object>)null);
        }

        public static string[] RadioButtonList(this HtmlHelper htmlHelper, string name, SelectList selectList, object htmlAttributes) {
            return RadioButtonList(htmlHelper, name, selectList, new RouteValueDictionary(htmlAttributes));
        }

        public static string[] RadioButtonList(this HtmlHelper htmlHelper, string name, SelectList selectList, IDictionary<string, object> htmlAttributes) {
            return htmlHelper.RadioButtonListInternal(name, selectList, false /* usedViewData */, htmlAttributes);
        }

        private static SelectList GetSelectData(this HtmlHelper htmlHelper, string name) {
            object o = null;
            if (htmlHelper.ViewData != null) {
                o = htmlHelper.ViewData.Eval(name);
            }
            if (o == null) {
                throw new InvalidOperationException(
                    String.Format(
                        CultureInfo.CurrentUICulture,
                        MvcResources.HtmlHelper_MissingSelectData,
                        name,
                        typeof(SelectList)));
            }
            SelectList selectList = o as SelectList;
            if (selectList == null) {
                throw new InvalidOperationException(
                    String.Format(
                        CultureInfo.CurrentUICulture,
                        MvcResources.HtmlHelper_WrongSelectDataType,
                        name,
                        o.GetType().FullName,
                        typeof(SelectList)));
            }
            return selectList;
        }

        private static string[] RadioButtonListInternal(this HtmlHelper htmlHelper, string name, SelectList selectList, bool usedViewData, IDictionary<string, object> htmlAttributes) {
            if (String.IsNullOrEmpty(name)) {
                throw new ArgumentException(MvcResources.Common_NullOrEmpty, "name");
            }
            if (selectList == null) {
                throw new ArgumentNullException("selectList");
            }

            // If we haven't already used ViewData to get the entire list of items then we need to
            // use the ViewData-supplied value before using the parameter-supplied value.
            if (!usedViewData) {
                object defaultValue = htmlHelper.ViewData.Eval(name);
                if (defaultValue != null) {
                    selectList = new SelectList(selectList.Items, selectList.DataValueField, selectList.DataTextField, defaultValue);
                }
            }

            // Convert each ListItem to an <option> tag
            IList<ListItem> listItems = selectList.GetListItems();

            IEnumerable<string> radioButtons = listItems.Select<ListItem, string>(item => htmlHelper.RadioButton(name, item.Value, item.Selected, htmlAttributes));

            return radioButtons.ToArray();
        }
    }
}
