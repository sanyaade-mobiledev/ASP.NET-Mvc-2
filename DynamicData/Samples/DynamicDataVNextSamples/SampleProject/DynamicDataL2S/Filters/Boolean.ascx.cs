using System;
using System.Collections;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Web.DynamicData;
using System.Web.UI;

namespace DynamicDataProject {
    public partial class BooleanFilter : System.Web.DynamicData.QueryableFilterUserControl {
        public override Control FilterControl {
            get {
                return DropDownList1;
            }
        }
    
        protected void Page_Init(object sender, EventArgs e) {
            if (!Column.ColumnType.Equals(typeof(bool))) {
                throw new InvalidOperationException(String.Format("A boolean filter was loaded for column '{0}' but the column has an incompatible type '{1}'.", Column.Name, Column.ColumnType));
            }
    
            if (!Page.IsPostBack) {
                // Set the initial value if there is one
                string initialValue = GetInitialValueFromQueryString(Column, Context);
                if (!String.IsNullOrEmpty(initialValue)) {
                    DropDownList1.SelectedValue = initialValue;
                }
            }
        }
    
        public override IQueryable GetQueryable(IQueryable source) {
            if (String.IsNullOrEmpty(DropDownList1.SelectedValue)) {
                return source;
            }
    
            bool value = Convert.ToBoolean(DropDownList1.SelectedValue);
    
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
