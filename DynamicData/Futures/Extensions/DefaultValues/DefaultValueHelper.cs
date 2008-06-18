using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Web;
using System.Web.DynamicData;
using System.Web.Routing;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Microsoft.Web.DynamicData.Extensions {
    public class DefaultValueHelper {

        private class DefaultsFieldGenerator : IAutoFieldGenerator {
            IAutoFieldGenerator OldFieldGenerator;
            List<string> SkipList = new List<string>();

            public DefaultsFieldGenerator(IAutoFieldGenerator oldgenerator, LinqDataSource datasource, RouteValueDictionary routevalues) {
                OldFieldGenerator = oldgenerator;
                MetaTable table = datasource.GetTable();
                foreach (MetaColumn column in table.Columns) {
                    var fkColumn = column as MetaForeignKeyColumn;
                    if (fkColumn != null) {
                        bool foundall = true;
                        foreach (string fkName in fkColumn.ForeignKeyNames) {
                            object val;
                            if (!routevalues.TryGetValue(fkName, out val)) {
                                foundall = false;
                            }
                        }
                        if (foundall) {
                            SkipList.Add(fkColumn.Name);
                        }
                    } else if (column.ColumnType == typeof(bool)) {
                        object val;
                        if (routevalues.TryGetValue(column.Name, out val)) {
                            SkipList.Add(column.Name);
                        }
                    }
                }
            }

            public ICollection GenerateFields(Control control) {
                List<DataControlField> fields = new List<DataControlField>();
                foreach (DataControlField c in OldFieldGenerator.GenerateFields(control)) {
                    var dynamicField = c as DynamicField;
                    if (dynamicField != null) {
                        if (!SkipList.Contains(dynamicField.DataField)) {
                            fields.Add(c);
                        }
                    } else {
                        fields.Add(c);
                    }
                }
                return fields;
            }
        }

        public static void RegisterListDefaults(LinqDataSource datasource, HyperLink hyperlink) {
            hyperlink.PreRender += delegate(object sender, EventArgs e) {
                hyperlink.NavigateUrl = GetRedirectToInsertWithDefaults(datasource);
            };
        }

        public static void RegisterInsertDefaults(LinqDataSource datasource, DetailsView detailsview, bool hidedefaults) {
            RequestContext requestContext = DynamicDataRouteHandler.GetRequestContext(HttpContext.Current);
            if (hidedefaults) {
                DefaultsFieldGenerator fieldgenerator = new DefaultsFieldGenerator(detailsview.RowsGenerator, datasource, requestContext.RouteData.Values);
                detailsview.RowsGenerator = fieldgenerator;
                detailsview.ItemInserting += delegate(object sender, DetailsViewInsertEventArgs e) {
                    SetDetailsViewInsertDefaults(datasource, detailsview, requestContext.RouteData.Values, e.Values);
                };
            } else {
                detailsview.DataBound += delegate(object sender, EventArgs e) {
                    //In the seperate page version we pull the values from the querystring via routing
                    SetDetailsViewDefaults(datasource, detailsview, requestContext.RouteData.Values);
                };
            }
        }

        public static void RegisterListDetailsDefaults(LinqDataSource datasource, DetailsView detailsview) {
            detailsview.DataBound += delegate(object sender, EventArgs e) {
                //In the combined version we stay on the same page and must pull values from the datasource
                if (detailsview.CurrentMode == DetailsViewMode.Insert) {
                    SetDetailsViewDefaults(datasource, detailsview, BuildRoutesFromDatasource(datasource));
                }
            };
        }

        private static void SetDetailsViewDefaults(LinqDataSource datasource, DetailsView detailsview, RouteValueDictionary routeValues) {
            MetaTable table = datasource.GetTable();
            foreach (MetaColumn column in table.Columns) {
                string controlValue = null;

                var fkColumn = column as MetaForeignKeyColumn;
                if (fkColumn != null) {
                    IList<object> list = new List<object>();
                    foreach (string fkName in fkColumn.ForeignKeyNames) {
                        object val;
                        if (routeValues.TryGetValue(fkName, out val)) {
                            list.Add(val);
                        }
                    }
                    if (list.Count != fkColumn.ForeignKeyNames.Count)
                        continue;

                    controlValue = fkColumn.ParentTable.GetPrimaryKeyString(list);
                } else if (column.ColumnType == typeof(bool)) {
                    object val;
                    if (routeValues.TryGetValue(column.Name, out val)) {
                        controlValue = Convert.ToString(val);
                    }
                }

                if (controlValue == null)
                    continue;

                var ftuc = detailsview.FindFieldTemplate(column.Name) as FieldTemplateUserControl;
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

        private static void SetDetailsViewInsertDefaults(LinqDataSource datasource, DetailsView detailsview, RouteValueDictionary routevalues, IOrderedDictionary values) {
            MetaTable table = datasource.GetTable();
            foreach (MetaColumn column in table.Columns) {
                var fkColumn = column as MetaForeignKeyColumn;
                if (fkColumn != null) {
                    IList<object> list = new List<object>();
                    foreach (string fkName in fkColumn.ForeignKeyNames) {
                        object val;
                        if (routevalues.TryGetValue(fkName, out val)) {
                            values[fkName] = val;
                        }
                    }
                } else if (column.ColumnType == typeof(bool)) {
                    object val;
                    if (routevalues.TryGetValue(column.Name, out val)) {
                        values[column.Name] = val;
                    }
                }
            }

        }

        public static RouteValueDictionary BuildRoutesFromDatasource(LinqDataSource datasource) {
            RouteValueDictionary rd = new RouteValueDictionary();
            var paramValues = datasource.WhereParameters.GetValues(HttpContext.Current, datasource);
            foreach (DictionaryEntry entry in paramValues) {
                rd.Add((string)entry.Key, entry.Value);
            }
            return rd;
        }

        public static string GetRedirectToInsertWithDefaults(LinqDataSource datasource) {
            RouteValueDictionary rd = BuildRoutesFromDatasource(datasource);
            MetaTable table = datasource.GetTable();
            return table.GetActionPath(PageAction.Insert, rd);
        }

        public static void RedirectToInsertWithDefaults(LinqDataSource datasource) {
            HttpContext.Current.Response.Redirect(GetRedirectToInsertWithDefaults(datasource));
        }

    }


}
