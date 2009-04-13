namespace DataSourcesDemo.ClassBrowser.Expressions {
    using System;
    using System.Collections;
    using System.Globalization;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Web.UI;
    using System.Web.UI.WebControls;
    using Microsoft.Web.Data.UI.WebControls.Expressions;

    public class IncludeExpression : ParameterDataSourceExpression {        
        public override IQueryable GetQueryable(IQueryable source) {
            if (Parameters.Count == 0) {
                return source;
            }

            IDictionary values = Parameters.GetValues(Context, Owner);
            Expression queryResult = null;
            ParameterExpression pe = Expression.Parameter(source.ElementType, String.Empty);

            foreach (DictionaryEntry de in values) {
                bool include = Convert.ToBoolean(de.Value, CultureInfo.InvariantCulture);
                if (include) {
                    Expression me = CreatePropertyExpression(pe, (string)de.Key);
                    
                    if (queryResult == null) {
                        queryResult = me;
                    }
                    else {
                        queryResult = Expression.OrElse(queryResult, me);
                    }
                }
            }

            return queryResult == null 
                    ? source : 
                    source.Provider.CreateQuery(
                                    Expression.Call(typeof(Queryable), 
                                    "Where", 
                                    new [] { source.ElementType },
                                    source.Expression, 
                                    Expression.Quote(Expression.Lambda(queryResult, pe)))
                                    );
        }

        private static Expression CreatePropertyExpression(Expression exp, string propertyName) {
            Expression propExpression = null;
            string[] props = propertyName.Split('.');
            foreach (var p in props) {
                if (propExpression == null) {
                    propExpression = Expression.PropertyOrField(exp, p);
                }
                else {
                    propExpression = Expression.PropertyOrField(propExpression, p);
                }
            }
            return propExpression;
        }
    }
}