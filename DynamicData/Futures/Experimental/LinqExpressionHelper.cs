using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Web.DynamicData;

namespace Microsoft.Web.DynamicData {
    public static class LinqExpressionHelper {

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
    }
}
