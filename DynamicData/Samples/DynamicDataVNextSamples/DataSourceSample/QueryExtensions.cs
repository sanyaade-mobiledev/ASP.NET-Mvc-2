namespace DataSourcesDemo {
    using System;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Data.Objects;
    using DataSourcesDemo.Entities;

    public static class QueryExtensions {
        public static IQueryable<T> SortBy<T>(this IQueryable<T> source, string propertyName) {
            if (source == null) {
                throw new ArgumentNullException("source");
            }
            // DataSource control passes the sort parameter with a direction 
            // if the direction is descending           
            int descIndex = propertyName.IndexOf("DESC", StringComparison.OrdinalIgnoreCase);
            if (descIndex >= 0) {
                propertyName = propertyName.Substring(0, descIndex).Trim();
            }

            if (String.IsNullOrEmpty(propertyName)) {
                return source;
            }

            ParameterExpression parameter = Expression.Parameter(source.ElementType, String.Empty);
            MemberExpression property = Expression.Property(parameter, propertyName);
            LambdaExpression lambda = Expression.Lambda(property, parameter);

            string methodName = (descIndex < 0) ? "OrderBy" : "OrderByDescending";

            Expression methodCallExpression = Expression.Call(typeof(Queryable), methodName,
                                                new Type[] { source.ElementType, property.Type },
                                                source.Expression, Expression.Quote(lambda));

            return (IQueryable<T>)((IQueryable)source).Provider.CreateQuery(methodCallExpression);
        }
    }
}