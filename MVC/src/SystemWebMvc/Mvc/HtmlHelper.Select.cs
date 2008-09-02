namespace System.Web.Mvc {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Globalization;
    using System.Text;
    using System.Web;
    using System.Web.Mvc.Resources;
    using System.Web.Routing;

    public partial class HtmlHelper {

        public string DropDownList(string optionLabel, string name) {
            return DropDownList(optionLabel, name, (IDictionary<string, object>)null);
        }

        public string DropDownList(string optionLabel, string name, object htmlAttributes) {
            return DropDownList(optionLabel, name, new RouteValueDictionary(htmlAttributes));
        }

        public string DropDownList(string optionLabel, string name, IDictionary<string, object> htmlAttributes) {
            SelectList selectList = GetSelectData<SelectList>(name);
            return SelectInternal(optionLabel, name, selectList, true /* usedViewData */, false /* allowMultiple */, htmlAttributes);
        }

        public string DropDownList(string optionLabel, string name, SelectList selectList) {
            return DropDownList(optionLabel, name, selectList, (IDictionary<string, object>)null);
        }

        public string DropDownList(string optionLabel, string name, SelectList selectList, object htmlAttributes) {
            return DropDownList(optionLabel, name, selectList, new RouteValueDictionary(htmlAttributes));
        }

        [SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters",
            Justification = "This type is appropriate for indicating a single selection.")]
        public string DropDownList(string optionLabel, string name, SelectList selectList, IDictionary<string, object> htmlAttributes) {
            return SelectInternal(optionLabel, name, selectList, false /* usedViewData */, false /* allowMultiple */, htmlAttributes);
        }

        public string ListBox(string name) {
            return ListBox(name, (IDictionary<string, object>)null);
        }

        public string ListBox(string name, object htmlAttributes) {
            return ListBox(name, new RouteValueDictionary(htmlAttributes));
        }

        public string ListBox(string name, IDictionary<string, object> htmlAttributes) {
            MultiSelectList selectList = GetSelectData<MultiSelectList>(name);
            return SelectInternal(null /* optionLabel */, name, selectList, true /* usedViewData */, true /* allowMultiple */, htmlAttributes);
        }

        public string ListBox(string name, MultiSelectList selectList) {
            return ListBox(name, selectList, (IDictionary<string, object>)null);
        }

        public string ListBox(string name, MultiSelectList selectList, object htmlAttributes) {
            return ListBox(name, selectList, new RouteValueDictionary(htmlAttributes));
        }

        public string ListBox(string name, MultiSelectList selectList, IDictionary<string, object> htmlAttributes) {
            return SelectInternal(null /* optionLabel */, name, selectList, false /* usedViewData */, true /* allowMultiple */, htmlAttributes);
        }

        private TList GetSelectData<TList>(string name) where TList : MultiSelectList {
            object o = null;
            if (ViewData != null) {
                o = ViewData.Eval(name);
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

        private string SelectInternal(string optionLabel, string name, MultiSelectList selectList, bool usedViewData, bool allowMultiple, IDictionary<string, object> htmlAttributes) {
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
                if (ViewData.TryGetValue(name, out defaultValue)) {
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
