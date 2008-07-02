using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.DynamicData;
using System.Web.Routing;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Microsoft.Web.DynamicData {
    /// <summary>
    /// Contains helper methods for working around bugs and small issues. This includes:
    /// 
    /// - attribute localization. Because Dynamic Data is too aggressive about caching the
    /// error message, only one version of the error message is stored and reused regardles
    /// of a request's locale.
    /// 
    /// - automatic scaffolding of enumerated types. By default enumerated types are not
    /// scaffolded
    /// 
    /// - disabling partial rendering for pages displaying image columns
    /// 
    /// - building complex where clauses
    /// </summary>
    public static class DynamicDataFutures {

        #region Metadata and model helpers

        /// <summary>
        /// Gets a value indicating if the column should be scaffolded. This honors the
        /// ScaffoldColumnAttribute as well as returns true if the column is an enumerated type.
        /// </summary>
        /// <param name="column"></param>
        /// <returns></returns>
        public static bool GetScaffold(this MetaColumn column) {
            // make sure we honor the ScaffoldColumnAttribute. The framework already does this
            // but we want to do this again as the first thing.
            var scaffoldAttribute = column.GetAttribute<ScaffoldColumnAttribute>();
            if (scaffoldAttribute != null) {
                return scaffoldAttribute.Scaffold;
            }

            // always return true for enumerated types
            return column.ColumnType.IsEnum || column.Scaffold;
        }

        /// <summary>
        /// Gets a dictionary with the values of the primary key of the given table for the
        /// given row.
        /// </summary>
        /// <param name="table"></param>
        /// <param name="row"></param>
        /// <returns></returns>
        public static IDictionary<string, object> GetPrimaryKeyDictionary(this MetaTable table, object row) {
            if(row == null) {
                throw new ArgumentNullException("row");
            }

            Dictionary<string, object> result = new Dictionary<string, object>();
            var pks = table.GetPrimaryKeyValues(row);
            int i = 0;
            foreach (var pkColumn in table.PrimaryKeyColumns) {
                result[pkColumn.Name] = pks[i++];
            }

            return result;
        }

        #endregion

        #region Where clause builder helpers

        /// <summary>
        /// Returns the where clause of the given LinqDataSourceView with Dynamic Data parameters (that have a non-null, non-empyt value) appended
        /// to the end using the AND logical operator.
        /// </summary>
        /// <param name="dataSourceView"></param>
        /// <param name="e"></param>
        /// <returns></returns>
        public static string GetWhereClauseWithDynamicDataParameters(LinqDataSourceView dataSourceView, LinqDataSourceSelectEventArgs e) {
            string whereClause = dataSourceView.Where;
            // get rid of parameters that are already used in the where clause
            var parametersToUse = e.WhereParameters.Where(param => !whereClause.Contains(String.Format("@{0}", param.Key)));

            StringBuilder newWhere = new StringBuilder();
            // surround the original where clause with parentheses to make sure the the ANDs don't take precedence
            newWhere.Append("(");
            newWhere.Append(whereClause);
            newWhere.Append(")");
            foreach (var parameter in parametersToUse) {
                // eclude all that have a null or empty value
                if (!(parameter.Value == null || String.IsNullOrEmpty(parameter.Value.ToString()))) {
                    newWhere.Append(" AND ");
                    newWhere.Append(parameter.Key);
                    newWhere.Append(" == @");
                    newWhere.Append(parameter.Key);
                }
            }

            return newWhere.ToString();
        }

        #endregion

        #region Localization helpers

        /// <summary>
        /// Gets the value of the Description property of a DescriptionAttribute associated with the given column.
        /// </summary>
        /// <param name="column">the column</param>
        /// <returns>The Description property of the DescriptionAttribute, or the default value.</returns>
        public static string GetDescription(this MetaColumn column) {
            var description = column.GetAttribute<DescriptionAttribute>();
            return description == null ? column.Description : description.Description;
        }

        /// <summary>
        /// Gets the value of the DisplayName property of a DisplayNameAttribute associated with the given column.
        /// </summary>
        /// <param name="column">the column</param>
        /// <returns>The DisplayName property of the DisplayNameAttribute, or the default value.</returns>
        public static string GetDisplayName(this MetaColumn column) {
            var displayName = column.GetAttribute<DisplayNameAttribute>();
            return displayName == null ? column.DisplayName : displayName.DisplayName;
        }

        /// <summary>
        /// Gets the value of the DisplayName property of a DisplayNameAttribute associated with the given table.
        /// </summary>
        /// <param name="table">the table</param>
        /// <returns>The DisplayName property of the DisplayNameAttribute, or the default value.</returns>
        public static string GetDisplayName(this MetaTable table) {
            var displayName = table.Attributes.OfType<DisplayNameAttribute>().FirstOrDefault();
            return displayName == null ? table.DisplayName : displayName.DisplayName;
        }

        /// <summary>
        /// Updates the error messages of the given validators to include the localized display name.
        /// This method handles only RangeValidator, RegularExpressionValidator, and RequiredFieldValidator. It
        /// ignores all others.
        /// </summary>
        /// <param name="column">the column the validators are meant to validate</param>
        /// <param name="validators">the validators</param>
        public static void SetUpValidator(MetaColumn column, params BaseValidator[] validators) {
            string displayName = column.GetDisplayName();
            foreach (var validator in validators) {
                if (validator is RangeValidator) {
                    var attribute = column.GetAttribute<RangeAttribute>();
                    if (attribute != null) {
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

        #endregion

        #region Page helpers

        /// <summary>
        /// If the given table contains a column that has a UI Hint with the value "DbImage", finds the ScriptManager
        /// for the current page and disables partial rendering
        /// </summary>
        /// <param name="page"></param>
        /// <param name="table"></param>
        public static void DisablePartialRenderingForUpload(Page page, MetaTable table) {
            foreach (var column in table.Columns) {
                // TODO this depends on the name of the field template, need to fix
                if (String.Equals(column.UIHint, "DbImage", StringComparison.OrdinalIgnoreCase)) {
                    var sm = ScriptManager.GetCurrent(page);
                    if (sm != null) {
                        sm.EnablePartialRendering = false;
                    }
                    break;
                }
            }
        }

        #endregion

        #region Default values helpers

        public static void RegisterListDefaults(LinqDataSource dataSource, HyperLink hyperLink) {
            hyperLink.PreRender += delegate(object sender, EventArgs e) {
                hyperLink.NavigateUrl = BuildInsertWithDefaultsUrl(dataSource);
            };
        }

        public static void RegisterListDetailsDefaults(LinqDataSource dataSource, DetailsView detailsView) {
            MetaTable table = dataSource.GetTable();
            detailsView.DataBound += delegate(object sender, EventArgs e) {
                //In the combined version we stay on the same page and must pull values from the datasource
                if (detailsView.CurrentMode == DetailsViewMode.Insert) {
                    SetDefaultInsertControlValues(table, detailsView, BuildRoutesFromDatasource(dataSource));
                }
            };
        }

        public static void RegisterInsertDefaults(IDynamicDataSource dataSource, DetailsView detailsView, bool hideDefaults) {
            RequestContext requestContext = DynamicDataRouteHandler.GetRequestContext(HttpContext.Current);
            MetaTable table = dataSource.GetTable();
            if (hideDefaults) {
                var fieldGenerator = detailsView.RowsGenerator as AdvancedFieldGenerator;
                if (fieldGenerator != null) {
                    fieldGenerator.SkipList.AddRange(BuildSkipList(table, requestContext.RouteData.Values));
                } else {
                    throw new InvalidOperationException(String.Format(CultureInfo.CurrentCulture, "Expected a field generator of type {0}", typeof(AdvancedFieldGenerator).FullName));
                }

                detailsView.ItemInserting += delegate(object sender, DetailsViewInsertEventArgs e) {
                    SetDefaultInsertValues(table, requestContext.RouteData.Values, e.Values);
                };
            } else {
                detailsView.DataBound += delegate(object sender, EventArgs e) {
                    //In the seperate page version we pull the values from the querystring via routing
                    SetDefaultInsertControlValues(table, detailsView, requestContext.RouteData.Values);
                };
            }
        }

        private static IEnumerable<MetaColumn> BuildSkipList(MetaTable table, IDictionary<string, object> parameterValues) {
            foreach (MetaColumn column in table.Columns) {
                var fkColumn = column as MetaForeignKeyColumn;
                if (fkColumn != null
                    && fkColumn.ForeignKeyNames.All(fkName => parameterValues.Keys.Contains(fkName))) {
                    yield return fkColumn;
                } else if (column.ColumnType == typeof(bool)
                    && parameterValues.Keys.Contains(column.Name)) {
                    // TODO limiting to bool types restricts the advanced filter repeater scenario, make more generic
                    yield return column;
                }
            }
        }

        private static void SetDefaultInsertControlValues(MetaTable table, DetailsView detailsView, IDictionary<string, object> parameterValues) {
            foreach (MetaColumn column in table.Columns) {
                string controlValue = null;

                var fkColumn = column as MetaForeignKeyColumn;
                if (fkColumn != null) {
                    IList<object> list = new List<object>();
                    foreach (string fkName in fkColumn.ForeignKeyNames) {
                        object val;
                        if (parameterValues.TryGetValue(fkName, out val)) {
                            list.Add(val);
                        }
                    }
                    if (list.Count != fkColumn.ForeignKeyNames.Count)
                        continue;

                    controlValue = fkColumn.ParentTable.GetPrimaryKeyString(list);
                } else if (column.ColumnType == typeof(bool)) {
                    object val;
                    if (parameterValues.TryGetValue(column.Name, out val)) {
                        controlValue = Convert.ToString(val);
                    }
                }

                if (controlValue == null)
                    continue;

                var ftuc = detailsView.FindFieldTemplate(column.Name) as FieldTemplateUserControl;
                if (ftuc == null)
                    continue;
                if (ftuc.DataControl is ListControl) {
                    var ddl = ftuc.DataControl as ListControl;
                    ListItem item = ddl.Items.FindByValue(controlValue);
                    if (item != null) {
                        item.Selected = true;
                    }
                } else if (ftuc.DataControl is CheckBox) {
                    var cb = ftuc.DataControl as CheckBox;
                    cb.Checked = string.Compare(controlValue, "true", true) == 0;
                }
            }
        }

        private static void SetDefaultInsertValues(MetaTable table, IDictionary<string, object> parameterValues, IOrderedDictionary insertValues) {
            foreach (MetaColumn column in table.Columns) {
                var fkColumn = column as MetaForeignKeyColumn;
                if (fkColumn != null) {
                    IList<object> list = new List<object>();
                    foreach (string fkName in fkColumn.ForeignKeyNames) {
                        object val;
                        if (parameterValues.TryGetValue(fkName, out val)) {
                            insertValues[fkName] = val;
                        }
                    }
                } else if (column.ColumnType == typeof(bool)) {
                    object val;
                    if (parameterValues.TryGetValue(column.Name, out val)) {
                        insertValues[column.Name] = val;
                    }
                }
            }
        }

        private static RouteValueDictionary BuildRoutesFromDatasource(LinqDataSource dataSource) {
            RouteValueDictionary routeValues = new RouteValueDictionary();
            var paramValues = dataSource.WhereParameters.GetValues(HttpContext.Current, dataSource);
            foreach (DictionaryEntry entry in paramValues) {
                routeValues[(string)entry.Key] = entry.Value;
            }
            return routeValues;
        }

        private static string BuildInsertWithDefaultsUrl(LinqDataSource datasource) {
            RouteValueDictionary routeValues = BuildRoutesFromDatasource(datasource);
            MetaTable table = datasource.GetTable();
            return table.GetActionPath(PageAction.Insert, routeValues);
        }

        private static void RedirectToInsertWithDefaults(LinqDataSource datasource) {
            HttpContext.Current.Response.Redirect(BuildInsertWithDefaultsUrl(datasource));
        }

        #endregion

        private static T GetAttribute<T>(this MetaColumn column) where T : Attribute {
            return column.Attributes.OfType<T>().FirstOrDefault();
        }
    }
}
