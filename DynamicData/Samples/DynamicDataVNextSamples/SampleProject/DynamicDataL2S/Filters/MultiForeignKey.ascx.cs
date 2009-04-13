using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web.DynamicData;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;

namespace DynamicDataProject.DynamicData.Filters {
    public partial class MultiForeignKey : QueryableFilterUserControl {
        private List<string> _selectedValues;

        new MetaForeignKeyColumn Column {
            get { return (MetaForeignKeyColumn)base.Column; }
        }

        private List<string> SelectedValues {
            get {
                if (_selectedValues == null) {
                    _selectedValues = new List<string>();
                    foreach (ListItem item in CheckBoxList1.Items) {
                        if (item.Selected) {
                            _selectedValues.Add(item.Value);
                        }
                    }
                }
                return _selectedValues;
            }
        }

        protected void Page_Init(object sender, EventArgs e) {
            Page.LoadComplete += new EventHandler(Page_LoadComplete);

            if (!Page.IsPostBack) {
                PopulateListControl(CheckBoxList1);
                foreach (ListItem item in CheckBoxList1.Items) {
                    item.Selected = true;
                }
            }
        }

        void Page_LoadComplete(object sender, EventArgs e) {
            StringBuilder sb = new StringBuilder();
            foreach (string value in SelectedValues) {
                sb.Append(value);
            }
            int hashCode = sb.ToString().GetHashCode();
            int? oldHashCode = (int?) ViewState["hashCode"];
            if (oldHashCode == null || oldHashCode != hashCode) {
                ViewState["hashCode"] = hashCode;
                OnFilterChanged();
            }
        }

        public override IQueryable GetQueryable(IQueryable source) {
            List<string> selectedValues = SelectedValues;

            ParameterExpression parameterExpression = Expression.Parameter(source.ElementType, "item");
            Expression body = BuildQueryBody(parameterExpression, selectedValues);

            LambdaExpression lambda = Expression.Lambda(body, parameterExpression);
            MethodCallExpression whereCall = Expression.Call(typeof(Queryable), "Where", new Type[] { source.ElementType }, source.Expression, Expression.Quote(lambda));
            return source.Provider.CreateQuery(whereCall);
        }

        private Expression BuildQueryBody(ParameterExpression parameterExpression, List<string> selectedValues) {
            if (selectedValues.Count == 0) {
                // If no subexpressions were contributed, return a false contstant expression to prevent
                // anything from showing up
                return Expression.Constant(false);

            } else {
                List<Expression> orFragments = new List<Expression>();

                foreach (string serializedValue in selectedValues) {
                    Dictionary<string, object> dict = new Dictionary<string, object>();
                    Column.ExtractForeignKey(dict, serializedValue);

                    List<Expression> andFragments = new List<Expression>();
                    foreach (var entry in dict) {
                        Expression property = CreatePropertyExpression(parameterExpression, entry.Key);
                        object value = Convert.ChangeType(entry.Value, GetUnderlyingType(property.Type));
                        Expression equalsExpression = Expression.Equal(property, Expression.Constant(value, property.Type));

                        andFragments.Add(equalsExpression);
                    }
                    var expr = Join(andFragments, Expression.AndAlso);
                    orFragments.Add(expr);
                }

                return Join(orFragments, Expression.OrElse);
            }
        }
    }
}