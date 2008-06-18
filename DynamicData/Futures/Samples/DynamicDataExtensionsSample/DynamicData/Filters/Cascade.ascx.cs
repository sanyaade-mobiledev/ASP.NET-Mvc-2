using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Web.DynamicData;
using System.Web.UI.WebControls;
using Microsoft.Web.DynamicData.Extensions;

namespace DynamicDataExtensionsSample {
    public partial class Cascade_Filter : FilterUserControlBase, ISelectionChangedAware {

        private DropDownList filterDropDown;
        private DropDownList parentDropDown;

        private MetaTable filterTable;
        private string filterTableColumnName;
        private MetaTable parentTable;

        private object context;

        public event EventHandler SelectionChanged {
            add {
                filterDropDown.SelectedIndexChanged += value;
            }
            remove {
                filterDropDown.SelectedIndexChanged -= value;
            }
        }

        protected void Page_Init(object sender, EventArgs e) {
            var cascadeAttribute = this.Column.Attributes.OfType<CascadeAttribute>().FirstOrDefault();
            if (cascadeAttribute == null) {
                throw new InvalidOperationException("Was expecting a CascadeAttribute.");
            }

            filterTableColumnName = cascadeAttribute.ParentColumn;

            filterTable = ((MetaForeignKeyColumn)Column).ParentTable;
            var filterTableParentColumn = (MetaForeignKeyColumn)filterTable.GetColumn(filterTableColumnName);
            parentTable = filterTableParentColumn.ParentTable;

            parentDropDown = new DropDownList() {
                AutoPostBack = true
            };
            parentDropDown.SelectedIndexChanged += new EventHandler(parentDropDown_SelectedIndexChanged);

            context = filterTable.CreateContext();

            parentDropDown.Items.Add(new ListItem(String.Format("[{0}]", filterTableParentColumn.DisplayName), ""));
            foreach (var row in parentTable.GetQuery(context)) {
                parentDropDown.Items.Add(new ListItem(parentTable.GetDisplayString(row), parentTable.GetPrimaryKeyString(row)));
            }
            Controls.Add(parentDropDown);

            filterDropDown = new DropDownList() {
                AutoPostBack = true,
                Enabled = false,
            };
            filterDropDown.Items.Add(new ListItem("------", ""));
            Controls.Add(filterDropDown);
        }

        void parentDropDown_SelectedIndexChanged(object sender, EventArgs e) {
            if (!String.IsNullOrEmpty(parentDropDown.SelectedValue)) {
                filterDropDown.Enabled = true;
                filterDropDown.Items.Clear();

                var selectedParent = GetSelectedParent();

                var filterItems = GetChildListFilteredByParent(selectedParent);

                foreach (var row in filterItems) {
                    filterDropDown.Items.Add(new ListItem(filterTable.GetDisplayString(row), filterTable.GetPrimaryKeyString(row)));
                }
            } else {
                filterDropDown.Enabled = false;
                filterDropDown.Items.Clear();
                filterDropDown.Items.Add(new ListItem("------", ""));
            }
        }

        private object GetSelectedParent() {
            Type parentType = parentTable.EntityType;
            IList<MetaColumn> primaryKeyColumns = parentTable.PrimaryKeyColumns;

            string[] primaryKeyValues = parentDropDown.SelectedValue.Split(',');

            Debug.Assert(primaryKeyValues.Length == primaryKeyColumns.Count);

            var query = parentTable.GetQuery(context);
            // category
            var parameter = Expression.Parameter(parentType, "category");

            var predicate = BuildPredicateFragment(parameter, primaryKeyColumns[0], primaryKeyValues[0]);
            for (int i = 1; i < primaryKeyValues.Length; i++) {
                predicate = Expression.AndAlso(predicate, BuildPredicateFragment(parameter, primaryKeyColumns[i], primaryKeyValues[i]));
            }

            // category => category.CategoryID == 10
            var lambda = Expression.Lambda(predicate, parameter);
            // Categories.Where(category => category.CategoryID == 10)
            var whereCall = Expression.Call(typeof(Queryable), "Where", new Type[] { parentType }, query.Expression, lambda);
            // Categories.Where(category => category.CategoryID == 10).Single()
            var singleCall = Expression.Call(typeof(Queryable), "Single", new Type[] { parentType }, whereCall);

            return query.Provider.Execute(singleCall);
        }

        private static BinaryExpression BuildPredicateFragment(ParameterExpression parameter, MetaColumn pkColumn, string pkValue) {
            // category.CategoryID
            var property = Expression.Property(parameter, pkColumn.Name);
            // category.CategoryID == 10
            return Expression.Equal(property, Expression.Constant(Convert.ChangeType(pkValue, pkColumn.ColumnType)));
        }

        private IQueryable GetChildListFilteredByParent(object selectedParent) {
            var query = filterTable.GetQuery(context);
            // product
            var parameter = Expression.Parameter(filterTable.EntityType, "product");
            // product.Category
            var property = Expression.Property(parameter, filterTableColumnName);
            // selectedCategory
            var constant = Expression.Constant(selectedParent);
            // product.Category == selectedCategory
            var predicate = Expression.Equal(property, constant);
            // product => product.Category == selectedCategory
            var lambda = Expression.Lambda(predicate, parameter);
            // Products.Where(product => product.Category == selectedCategory)
            var whereCall = Expression.Call(typeof(Queryable), "Where", new Type[] { filterTable.EntityType }, query.Expression, lambda);

            return query.Provider.CreateQuery(whereCall);
        }

        public override string SelectedValue {
            get {
                return filterDropDown.SelectedValue;
            }
        }
    }
}