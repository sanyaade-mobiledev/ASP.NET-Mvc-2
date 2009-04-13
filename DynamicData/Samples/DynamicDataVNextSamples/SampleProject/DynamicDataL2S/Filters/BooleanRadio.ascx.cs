using System;
using System.Web.DynamicData;
using System.Linq.Expressions;
using System.Linq;

namespace DynamicDataProject.DynamicData.Filters {
    public partial class BooleanRadio : QueryableFilterUserControl {

        protected void Page_Init(object sender, EventArgs e) {
            if (Column.ColumnType != typeof(bool)) {
                throw new InvalidOperationException(String.Format("A boolean filter was loaded for column '{0}' but the column has an incompatible type '{1}'.", Column.Name, Column.ColumnType));
            }

            if (!Page.IsPostBack) {
                // Set the initial value if there is one
                string initialValue = GetInitialValueFromQueryString(Column, Context);
                if (!String.IsNullOrEmpty(initialValue)) {
                    RadioButtonList1.SelectedValue = initialValue;
                }
            }
        }

        public override IQueryable GetQueryable(IQueryable source) {
            if (String.IsNullOrEmpty(RadioButtonList1.SelectedValue)) {
                return source;
            }

            bool value = Convert.ToBoolean(RadioButtonList1.SelectedValue);

            ParameterExpression parameterExpression = Expression.Parameter(source.ElementType, "item");
            Expression body = BuildQueryBody(parameterExpression, value);

            LambdaExpression lambda = Expression.Lambda(body, parameterExpression);
            MethodCallExpression whereCall = Expression.Call(typeof(Queryable), "Where", new Type[] { source.ElementType }, source.Expression, Expression.Quote(lambda));
            return source.Provider.CreateQuery(whereCall);
        }

        private Expression BuildQueryBody(ParameterExpression parameterExpression, bool value) {
            Expression propertyExpression = GetValue(CreatePropertyExpression(parameterExpression, Column.Name));
            Expression valueExpression = Expression.Constant(value);
            Expression equalExpression = Expression.Equal(propertyExpression, valueExpression);
            return equalExpression;
        }

        protected void DropDownList1_SelectedIndexChanged(object sender, EventArgs e) {
            OnFilterChanged();
        }
    }
}