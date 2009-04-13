using System;
using System.Collections;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Web.DynamicData;
using System.Web.UI;

namespace DynamicDataProject {
    public partial class ForeignKeyFilter : System.Web.DynamicData.QueryableFilterUserControl {
        private new MetaForeignKeyColumn Column {
            get {
                return (MetaForeignKeyColumn)base.Column;
            }
        }
    
        public override Control FilterControl {
            get {
                return DropDownList1;
            }
        }
    
        protected void Page_Init(object sender, EventArgs e) {
            if (!Page.IsPostBack) {
                PopulateListControl(DropDownList1);
    
                // Set the initial value if there is one
                string initialValue = GetInitialForeignKeyValueFromQueryString(Column, Context);
                if (!String.IsNullOrEmpty(initialValue)) {
                    DropDownList1.SelectedValue = initialValue;
                }
            }
        }
    
        protected void DropDownList1_SelectedIndexChanged(object sender, EventArgs e) {
            OnFilterChanged();
        }
    
        public override IQueryable GetQueryable(IQueryable source) {
            if (String.IsNullOrEmpty(DropDownList1.SelectedValue)) {
                return source;
            }
    
            ParameterExpression parameterExpression = Expression.Parameter(source.ElementType, "item");
            Expression body = BuildQueryBody(parameterExpression, DropDownList1.SelectedValue);
    
            LambdaExpression lambda = Expression.Lambda(body, parameterExpression);
            MethodCallExpression whereCall = Expression.Call(typeof(Queryable), "Where", new Type[] { source.ElementType }, source.Expression, Expression.Quote(lambda));
            return source.Provider.CreateQuery(whereCall);
        }
    
        private Expression BuildQueryBody(ParameterExpression parameterExpression, string selectedValue) {
            IDictionary dict = new Hashtable();
            Column.ExtractForeignKey(dict, selectedValue);
    
            ArrayList andFragments = new ArrayList();
            foreach (DictionaryEntry entry in dict) {
                Expression propertyExpression = CreatePropertyExpression(parameterExpression, (string)entry.Key);
                object value = ChangeType(entry.Value, propertyExpression.Type);
                Expression equalsExpression = Expression.Equal(propertyExpression, Expression.Constant(value, propertyExpression.Type));
    
                andFragments.Add(equalsExpression);
            }
    
            Expression result = null;
            foreach (Expression e in andFragments) {
                if (result == null) {
                    result = e;
                } else {
                    result = Expression.AndAlso(result, e);
                }
            }
            return result;
        }
    
    }
}
