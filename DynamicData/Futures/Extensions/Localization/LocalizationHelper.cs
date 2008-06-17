using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI.WebControls;
using System.Web.UI;
using System.Collections;
using System.Web.DynamicData;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Microsoft.Web.DynamicData.Extensions {
    public static class LocalizationHelper {
        public static void Register(BaseDataBoundControl control) {
            if (control is GridView) {
                Register((GridView)control);
            } else if (control is DetailsView) {
                Register((DetailsView)control);
            }
        }

        private static void Register(GridView control) {
            control.ColumnsGenerator = new FieldGenerator(control.ColumnsGenerator);
        }

        private static void Register(DetailsView control) {
            control.RowsGenerator = new FieldGenerator(control.RowsGenerator);
        }

        private class FieldGenerator : IAutoFieldGenerator {
            private IAutoFieldGenerator Parent { get; set; }
            public FieldGenerator(IAutoFieldGenerator parent) {
                Parent = parent;
            }

            public ICollection GenerateFields(Control control) {
                DataBoundControl databound = control as DataBoundControl;
                IDynamicDataSource dynamicDataSource = databound.DataSourceObject as IDynamicDataSource;
                MetaTable table = dynamicDataSource.GetTable();

                var fields = Parent.GenerateFields(control);
                foreach (var field in fields.OfType<DynamicField>()) {
                    MetaColumn column = table.GetColumn(field.DataField);
                    field.HeaderText = column.GetDisplayName();
                }
                return fields;
            }
        }

        public static string GetDescription(this MetaColumn column) {
            var description = column.GetAttribute<DescriptionAttribute>();
            return description == null ? column.Description : description.Description;
        }

        public static string GetDisplayName(this MetaColumn column) {
            var displayName = column.GetAttribute<DisplayNameAttribute>();
            return displayName == null ? column.DisplayName : displayName.DisplayName;
        }

        public static string GetDisplayName(this MetaTable table) {
            var displayName = table.Attributes.OfType<DisplayNameAttribute>().FirstOrDefault();
            return displayName == null ? table.DisplayName : displayName.DisplayName;
        }

        public static void SetUpValidator(MetaColumn column , params BaseValidator[] validators) {
            string displayName = column.GetDisplayName();
            foreach (var validator in validators) {
                if (validator is RangeValidator) {
                    var attribute = column.GetAttribute<RangeAttribute>();
                    if(attribute != null) {
                        validator.ErrorMessage = HttpUtility.HtmlEncode(attribute.FormatErrorMessage(displayName));
                    }
                } else if (validator is RegularExpressionValidator) {
                    var attribute = column.GetAttribute<RegularExpressionAttribute>();
                    if (attribute != null) {
                        validator.ErrorMessage = HttpUtility.HtmlEncode(attribute.FormatErrorMessage(displayName));
                    }
                } else if (validator is RequiredFieldValidator) {
                    var attribute = column.GetAttribute<RequiredAttribute>() ?? new RequiredAttribute();
                    validator.ErrorMessage = HttpUtility.HtmlEncode(attribute.FormatErrorMessage(displayName));
                }
            }
        }

        private static T GetAttribute<T>(this MetaColumn column) where T : Attribute {
            return column.Attributes.OfType<T>().FirstOrDefault();
        }
    }
}
