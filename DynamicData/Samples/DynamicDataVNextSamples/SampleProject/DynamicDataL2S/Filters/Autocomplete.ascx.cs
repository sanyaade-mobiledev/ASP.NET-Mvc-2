using System;
using System.Web.DynamicData;
using System.Linq;
using System.Linq.Expressions;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;

namespace DynamicDataProject.DynamicData.Filters {
    public partial class Autocomplete : QueryableFilterUserControl {
        private new MetaForeignKeyColumn Column {
            get {
                return (MetaForeignKeyColumn)base.Column;
            }
        }

        protected void Page_Init(object sender, EventArgs e) {
            // dynamically build the context key so the web service knows which table we're talking about
            autoComplete1.ContextKey = AutocompleteFilterService.GetContextKey(Column.ParentTable);

            // add javascript to create postback when user selects an item in the list
            var method = "function(source, eventArgs ) {\r\n" +
                "var valueField = document.getElementById('" + AutocompleteValue.ClientID + "');\r\n" +
                "valueField.value = eventArgs.get_value();\r\n" +
                "setTimeout('" + Page.ClientScript.GetPostBackEventReference(AutocompleteTextBox, null).Replace("'", "\\'") + "', 0);\r\n" +
                "}";
            autoComplete1.OnClientItemSelected = method;

            // modify behaviorID so it does not clash with other autocomplete extenders on the page
            autoComplete1.Animations = autoComplete1.Animations.Replace(autoComplete1.BehaviorID, AutocompleteTextBox.UniqueID);
            autoComplete1.BehaviorID = AutocompleteTextBox.UniqueID;

            string initialValue = GetInitialForeignKeyValueFromQueryString(Column, Context);

            if (!Page.IsPostBack && !String.IsNullOrEmpty(initialValue)) {
                // set the initial value of the filter if it's present in the request URL

                MetaTable parentTable = Column.ParentTable;
                IQueryable query = parentTable.GetQuery();
                // multi-column PK values are seperated by commas
                Expression singleCall = BuildSingleItemQuery(query, parentTable, initialValue.Split(','));
                object row = query.Provider.Execute(singleCall);
                string display = parentTable.GetDisplayString(row);
                
                AutocompleteTextBox.Text = display;
                AutocompleteValue.Value = initialValue;
            }

            AutocompleteValue.ValueChanged += new EventHandler(AutocompleteValue_ValueChanged);
        }

        void AutocompleteValue_ValueChanged(object sender, EventArgs e) {
            OnFilterChanged();
        }

        public void ClearButton_Click(object sender, EventArgs e) {
            // this would probably be better handled using client javascirpt
            AutocompleteValue.Value = String.Empty;
            AutocompleteTextBox.Text = String.Empty;
            OnFilterChanged();
        }

        public static MethodCallExpression BuildSingleItemQuery(IQueryable query, MetaTable metaTable, string[] primaryKeyValues) {
            // Items.Where(row => row.ID == 1)
            var whereCall = BuildItemsQuery(query, metaTable, metaTable.PrimaryKeyColumns, primaryKeyValues);
            // Items.Where(row => row.ID == 1).Single()
            var singleCall = Expression.Call(typeof(Queryable), "Single", new Type[] { metaTable.EntityType }, whereCall);

            return singleCall;
        }

        public static MethodCallExpression BuildItemsQuery(IQueryable query, MetaTable metaTable, IList<MetaColumn> columns, string[] values) {
            // row
            var rowParam = Expression.Parameter(metaTable.EntityType, "row");
            // row.ID == 1
            var whereBody = BuildWhereBody(rowParam, columns, values);
            // row => row.ID == 1
            var whereLambda = Expression.Lambda(whereBody, rowParam);
            // Items.Where(row => row.ID == 1)
            var whereCall = Expression.Call(typeof(Queryable), "Where", new Type[] { metaTable.EntityType }, query.Expression, whereLambda);

            return whereCall;
        }

        public static BinaryExpression BuildWhereBody(ParameterExpression parameter, IList<MetaColumn> columns, string[] values) {
            Debug.Assert(columns.Count == values.Length);
            Debug.Assert(columns.Count > 0);

            // row.ID == 1
            var whereBody = BuildWhereBodyFragment(parameter, columns[0], values[0]);
            for (int i = 1; i < values.Length; i++) {
                // row.ID == 1 && row.ID2 == 2
                whereBody = Expression.AndAlso(whereBody, BuildWhereBodyFragment(parameter, columns[i], values[i]));
            }

            return whereBody;
        }

        private static BinaryExpression BuildWhereBodyFragment(ParameterExpression parameter, MetaColumn column, string value) {
            // row.ID
            var property = Expression.Property(parameter, column.Name);
            // row.ID == 1
            return Expression.Equal(property, Expression.Constant(ChangeValueType(column, value)));
        }

        private static object ChangeValueType(MetaColumn column, string value) {
            if (column.ColumnType == typeof(Guid)) {
                return new Guid(value);
            } else {
                return Convert.ChangeType(value, column.TypeCode, CultureInfo.InvariantCulture);
            }
        }

        public override IQueryable GetQueryable(IQueryable source) {
            if (String.IsNullOrEmpty(AutocompleteValue.Value)) {
                return source;
            }

            ParameterExpression parameterExpression = Expression.Parameter(source.ElementType, "item");
            Expression body = BuildQueryBody(parameterExpression, AutocompleteValue.Value);

            LambdaExpression lambda = Expression.Lambda(body, parameterExpression);
            MethodCallExpression whereCall = Expression.Call(typeof(Queryable), "Where", new Type[] { source.ElementType }, source.Expression, Expression.Quote(lambda));
            return source.Provider.CreateQuery(whereCall);
        }

        private Expression BuildQueryBody(ParameterExpression parameterExpression, string selectedValue) {
            Dictionary<string, object> dict = new Dictionary<string, object>();
            Column.ExtractForeignKey(dict, selectedValue);

            List<Expression> andFragments = new List<Expression>();
            foreach (var entry in dict) {
                Expression property = CreatePropertyExpression(parameterExpression, entry.Key);
                object value = Convert.ChangeType(entry.Value, GetUnderlyingType(property.Type));
                Expression equalsExpression = Expression.Equal(property, Expression.Constant(value, property.Type));

                andFragments.Add(equalsExpression);
            }
            return Join(andFragments, Expression.AndAlso);
        }
    }
}