using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Globalization;
using System.Linq;
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
    /// </summary>
    public static class DynamicDataFutures {

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

        #region Enumeration type helpers

        public static string GetUnderlyingTypeValueString(Type enumType, object enumValue) {
            return Convert.ChangeType(enumValue, Enum.GetUnderlyingType(enumType)).ToString();
        }

        private static IOrderedDictionary GetEnumNamesAndValues(Type enumType) {
            OrderedDictionary result = new OrderedDictionary();
            foreach (object enumValue in Enum.GetValues(enumType)) {
                // TODO: add way to localize the displayed name
                string name = Enum.GetName(enumType, enumValue);
                string value = DynamicDataFutures.GetUnderlyingTypeValueString(enumType, enumValue);
                result.Add(name, value);
            }
            return result;
        }

        public static void FillEnumListControl(ListControl list, Type enumType) {
            foreach (DictionaryEntry entry in DynamicDataFutures.GetEnumNamesAndValues(enumType)) {
                list.Items.Add(new ListItem((string)entry.Key, (string)entry.Value));
            }
        }

        public static bool IsEnumTypeInFlagsMode(Type enumType) {
            return enumType.GetCustomAttributes(typeof(FlagsAttribute), false).Length != 0;
        }

        #endregion

        private static T GetAttribute<T>(this MetaColumn column) where T : Attribute {
            return column.Attributes.OfType<T>().FirstOrDefault();
        }
    }
}
