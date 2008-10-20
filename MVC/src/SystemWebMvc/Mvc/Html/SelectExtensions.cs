namespace System.Web.Mvc.Html {
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Globalization;
    using System.Text;
    using System.Web;
    using System.Web.Mvc.Resources;
    using System.Web.Routing;

    [AspNetHostingPermission(System.Security.Permissions.SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
    public static class SelectExtensions {
        public static string DropDownList(this HtmlHelper htmlHelper, string optionLabel, string name) {
            return DropDownList(htmlHelper, optionLabel, name, (IDictionary<string, object>)null);
        }

        public static string DropDownList(this HtmlHelper htmlHelper, string optionLabel, string name, object htmlAttributes) {
            return DropDownList(htmlHelper, optionLabel, name, new RouteValueDictionary(htmlAttributes));
        }

        public static string DropDownList(this HtmlHelper htmlHelper, string optionLabel, string name, IDictionary<string, object> htmlAttributes) {
            SelectList selectList = htmlHelper.GetSelectData<SelectList>(name);
            return htmlHelper.SelectInternal(optionLabel, name, selectList, true /* usedViewData */, false /* allowMultiple */, htmlAttributes);
        }

        public static string DropDownList(this HtmlHelper htmlHelper, string optionLabel, string name, SelectList selectList) {
            return DropDownList(htmlHelper, optionLabel, name, selectList, (IDictionary<string, object>)null);
        }

        public static string DropDownList(this HtmlHelper htmlHelper, string optionLabel, string name, SelectList selectList, object htmlAttributes) {
            return DropDownList(htmlHelper, optionLabel, name, selectList, new RouteValueDictionary(htmlAttributes));
        }

        public static string DropDownList(this HtmlHelper htmlHelper, string name) {
            return DropDownList(htmlHelper, name, (object)null /* htmlAttributes */);
        }

        public static string DropDownList(this HtmlHelper htmlHelper, string name, object htmlAttributes) {
            return DropDownList(htmlHelper, name, new RouteValueDictionary(htmlAttributes));
        }

        public static string DropDownList(this HtmlHelper htmlHelper, string name, IDictionary<string, object> htmlAttributes) {
            SelectList selectList = htmlHelper.GetSelectData<SelectList>(name);
            return htmlHelper.SelectInternal(null /* optionLabel */, name, selectList, true /* usedViewData */, false /* allowMultiple */, htmlAttributes);
        }

        public static string DropDownList(this HtmlHelper htmlHelper, string name, SelectList selectList) {
            return DropDownList(htmlHelper, name, selectList, (object)null /* htmlAttributes */);
        }

        public static string DropDownList(this HtmlHelper htmlHelper, string name, SelectList selectList, object htmlAttributes) {
            return DropDownList(htmlHelper, name, selectList, new RouteValueDictionary(htmlAttributes));
        }

        [SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters",
            Justification = "This type is appropriate for indicating a single selection.")]
        public static string DropDownList(this HtmlHelper htmlHelper, string name, SelectList selectList, IDictionary<string, object> htmlAttributes) {
            return htmlHelper.SelectInternal(null /* optionLabel */, name, selectList, false /* usedViewData */, false /* allowMultiple */, htmlAttributes);
        }

        [SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters",
            Justification = "This type is appropriate for indicating a single selection.")]
        public static string DropDownList(this HtmlHelper htmlHelper, string optionLabel, string name, SelectList selectList, IDictionary<string, object> htmlAttributes) {
            return htmlHelper.SelectInternal(optionLabel, name, selectList, false /* usedViewData */, false /* allowMultiple */, htmlAttributes);
        }

        public static string ListBox(this HtmlHelper htmlHelper, string name) {
            return ListBox(htmlHelper, name, (IDictionary<string, object>)null);
        }

        public static string ListBox(this HtmlHelper htmlHelper, string name, object htmlAttributes) {
            return ListBox(htmlHelper, name, new RouteValueDictionary(htmlAttributes));
        }

        public static string ListBox(this HtmlHelper htmlHelper, string name, IDictionary<string, object> htmlAttributes) {
            MultiSelectList selectList = htmlHelper.GetSelectData<MultiSelectList>(name);
            return htmlHelper.SelectInternal(null /* optionLabel */, name, selectList, true /* usedViewData */, true /* allowMultiple */, htmlAttributes);
        }

        public static string ListBox(this HtmlHelper htmlHelper, string name, MultiSelectList selectList) {
            return ListBox(htmlHelper, name, selectList, (IDictionary<string, object>)null);
        }

        public static string ListBox(this HtmlHelper htmlHelper, string name, MultiSelectList selectList, object htmlAttributes) {
            return ListBox(htmlHelper, name, selectList, new RouteValueDictionary(htmlAttributes));
        }

        public static string ListBox(this HtmlHelper htmlHelper, string name, MultiSelectList selectList, IDictionary<string, object> htmlAttributes) {
            return htmlHelper.SelectInternal(null /* optionLabel */, name, selectList, false /* usedViewData */, true /* allowMultiple */, htmlAttributes);
        }

        private static TList GetSelectData<TList>(this HtmlHelper htmlHelper, string name) where TList : MultiSelectList {
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
                        typeof(TList)));
            }
            TList selectList = o as TList;
            if (selectList == null) {
                throw new InvalidOperationException(
                    String.Format(
                        CultureInfo.CurrentUICulture,
                        MvcResources.HtmlHelper_WrongSelectDataType,
                        name,
                        o.GetType().FullName,
                        typeof(TList)));
            }
            return selectList;
        }

        private static string ListItemToOption(ListItem item) {
            TagBuilder builder = new TagBuilder("option") {
                InnerHtml = HttpUtility.HtmlEncode(item.Text)
            };
            if (item.Value != null) {
                builder.Attributes["value"] = item.Value;
            }
            if (item.Selected) {
                builder.Attributes["selected"] = "selected";
            }
            return builder.ToString(TagRenderMode.Normal);
        }

        private static string SelectInternal(this HtmlHelper htmlHelper, string optionLabel, string name, MultiSelectList selectList, bool usedViewData, bool allowMultiple, IDictionary<string, object> htmlAttributes) {
            if (String.IsNullOrEmpty(name)) {
                throw new ArgumentException(MvcResources.Common_NullOrEmpty, "name");
            }
            if (selectList == null) {
                throw new ArgumentNullException("selectList");
            }

            // If we haven't already used ViewData to get the entire list of items then we need to
            // use the ViewData-supplied value before using the parameter-supplied value.
            if (!usedViewData) {
                object defaultValue;
                if (htmlHelper.ViewData.TryGetValue(name, out defaultValue)) {
                    selectList = new MultiSelectList(selectList.Items, selectList.DataValueField, selectList.DataTextField,
                        (allowMultiple) ? defaultValue as IEnumerable : new[] { defaultValue });
                }
            }

            // Convert each ListItem to an <option> tag
            StringBuilder listItemBuilder = new StringBuilder();
            IList<ListItem> listItems = selectList.GetListItems();

            // Make optionLabel the first item that gets rendered.
            if (!String.IsNullOrEmpty(optionLabel)) {
                listItemBuilder.AppendLine(ListItemToOption(new ListItem() { Text = optionLabel, Value = String.Empty, Selected = false }));
            }

            foreach (ListItem item in listItems) {
                listItemBuilder.AppendLine(ListItemToOption(item));
            }

            TagBuilder builder = new TagBuilder("select") {
                InnerHtml = listItemBuilder.ToString()
            };
            builder.MergeAttributes(htmlAttributes);
            builder.MergeAttribute("name", name);
            builder.MergeAttribute("id", name);
            if (allowMultiple) {
                builder.MergeAttribute("multiple", "multiple");
            }
            return builder.ToString(TagRenderMode.Normal);
        }
    }
}
