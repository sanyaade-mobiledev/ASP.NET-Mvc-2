using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Compilation;
using System.Web.DynamicData;
using System.Web.Routing;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Microsoft.Web.DynamicData {
    /// <summary>
    /// A control that displays links to table actions based on routing rules. In addition, it can data bind
    /// to table items and generate links for actions specific to a given item.
    /// </summary>
    [DefaultProperty("Action")]
    public class DynamicHyperLink : HyperLink, IAttributeAccessor {
        private bool _dataBound;
        private Dictionary<string, string> _extraRouteParams = new Dictionary<string, string>();

        // REVIEW: do we need to store anything in ViewState?

        /// <summary>
        /// The name of the context type
        /// </summary>
        [DefaultValue("")]
        [Category("Navigation")]
        public string ContextTypeName { get; set; }

        /// <summary>
        /// The name of the table
        /// </summary>
        [DefaultValue("")]
        [Category("Navigation")]
        public string TableName { get; set; }

        /// <summary>
        /// The name of the action
        /// </summary>
        [TypeConverter(typeof(ActionConverter))]
        [DefaultValue("")]
        [Category("Navigation")]
        public string Action { get; set; }

        /// <summary>
        /// The name of the column whose value will be used to populate the Text
        /// property if it is not already set in data binding scenarios.
        /// </summary>
        [DefaultValue("")]
        [Category("Navigation")]
        public string DataField { get; set; }

        protected override void OnDataBinding(EventArgs e) {
            base.OnDataBinding(e);

            if (DesignMode) {
                if (!String.IsNullOrEmpty(NavigateUrl)) {
                    NavigateUrl = "DesignTimeUrl";
                }
                return;
            }

            if (!String.IsNullOrEmpty(NavigateUrl)) {
                // stop processing if there already is a URL
                return;
            }

            if (!String.IsNullOrEmpty(TableName)) {
                // code path where the current item is not a member of a MetaTable collection, but
                // instead sets the table name, etc

                MetaTable table = GetTableFromTableName();
                string action = GetActionOrDefaultTo(PageAction.List);
                NavigateUrl = table.GetActionPath(action, GetRouteValues());
                if (String.IsNullOrEmpty(Text) && !String.IsNullOrEmpty(DataField)) {
                    Text = DataBinder.GetPropertyValue(Page.GetDataItem(), DataField).ToString();
                }
            } else {
                // code path where the current item is a MetaTable collection member

                IDynamicDataSource datasource = this.FindDataSourceControl();
                MetaTable table = datasource.GetTable();
                if (table == null)
                    return;

                object dataItem = Page.GetDataItem();
                string action = GetActionOrDefaultTo(PageAction.Details);
                NavigateUrl = table.GetActionPath(action, GetRouteValues(table, dataItem));

                if (String.IsNullOrEmpty(Text)) {
                    if (!String.IsNullOrEmpty(DataField)) {
                        Text = DataBinder.GetPropertyValue(dataItem, DataField).ToString();
                    } else {
                        Text = table.GetDisplayString(dataItem);
                    }
                }
            }

            _dataBound = true;
        }

        protected override void OnPreRender(EventArgs e) {
            base.OnPreRender(e);

            if (DesignMode) {
                if (!String.IsNullOrEmpty(NavigateUrl)) {
                    NavigateUrl = "DesignTimeUrl";
                }
                return;
            }

            // check both _dataBound and NavigateUrl cause NavigateUrl might be empty if routing/scaffolding
            // does not allow a particular action
            if (!_dataBound && String.IsNullOrEmpty(NavigateUrl)) {
                MetaTable table;
                string action;
                if (!String.IsNullOrEmpty(TableName)) {
                    table = GetTableFromTableName();
                } else {
                    table = DynamicDataRouteHandler.GetRequestMetaTable(HttpContext.Current);
                }

                if (table == null) {
                    // can't find table, quit
                    return;
                }

                action = GetActionOrDefaultTo(PageAction.List);
                NavigateUrl = table.GetActionPath(action, GetRouteValues());
            }
        }

        private RouteValueDictionary GetRouteValues() {
            var routeValues = new RouteValueDictionary();
            foreach (var entry in _extraRouteParams) {
                string key = entry.Key;
                if (key.StartsWith("__")) {
                    key = key.Substring(2);
                }

                routeValues[key] = entry.Value;
            }
            return routeValues;
        }

        private RouteValueDictionary GetRouteValues(MetaTable table, object row) {
            RouteValueDictionary routeValues = GetRouteValues();
            foreach (var pk in table.GetPrimaryKeyDictionary(row)) {
                routeValues[pk.Key] = pk.Value;
            }
            return routeValues;
        }

        private bool IsAnyNotEmpty(params string[] values) {
            return values.Any(value => !String.IsNullOrEmpty(value));
        }

        private bool AreAllNotEmpty(params string[] values) {
            return values.All(value => !String.IsNullOrEmpty(value));
        }

        private string GetActionOrDefaultTo(string defaultAction) {
            return String.IsNullOrEmpty(Action) ? defaultAction : Action;
        }

        private MetaTable GetTableFromTableName() {
            Debug.Assert(!String.IsNullOrEmpty(TableName));

            if (!String.IsNullOrEmpty(ContextTypeName)) {
                // context type allows to disambiguate table names
                Type contextType = BuildManager.GetType(ContextTypeName, true);
                MetaModel model = MetaModel.GetModel(contextType);
                MetaTable table = model.GetTable(TableName, contextType);
                return table;
            } else {
                // try the default model if there is no context type
                return MetaModel.Default.GetTable(TableName);
            }
        }

        #region IAttributeAccessor Members

        string IAttributeAccessor.GetAttribute(string key) {
            return (string)_extraRouteParams[key];
        }

        void IAttributeAccessor.SetAttribute(string key, string value) {
            _extraRouteParams[key] = value;
        }

        #endregion

        // REVIEW: should this class be private?
        public class ActionConverter : StringConverter {
            private static string[] _targetValues = {
                                                       PageAction.Details,
                                                       PageAction.Edit,
                                                       PageAction.Insert,
                                                       PageAction.List
                                                   };

            private StandardValuesCollection _values;

            public override StandardValuesCollection GetStandardValues(ITypeDescriptorContext context) {
                if (_values == null) {
                    _values = new StandardValuesCollection(_targetValues);
                }
                return _values;
            }

            public override bool GetStandardValuesExclusive(ITypeDescriptorContext context) {
                return false;
            }

            public override bool GetStandardValuesSupported(ITypeDescriptorContext context) {
                return true;
            }
        }
    }
}
