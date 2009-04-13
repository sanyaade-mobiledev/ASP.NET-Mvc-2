using System;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Web.DynamicData;
using System.Web.Services;
using AjaxControlToolkit;

namespace DynamicDataProject {
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.Web.Script.Services.ScriptService]
    public class AutocompleteFilterService : System.Web.Services.WebService {

        [System.Web.Services.WebMethod]
        [System.Web.Script.Services.ScriptMethod]
        public string[] GetCompletionList(string prefixText, int count, string contextKey) {
            MetaTable table = GetTable(contextKey);

            IQueryable queryable = BuildFilterQuery(table, prefixText, count);

            return queryable.Cast<object>().Select(row => CreateAutoCompleteItem(table, row)).ToArray();
        }

        private static IQueryable BuildFilterQuery(MetaTable table, string prefixText, int maxCount) {
            IQueryable query = table.GetQuery();

            // row
            var entityParam = Expression.Parameter(table.EntityType, "row");
            // row.DisplayName
            var property = Expression.Property(entityParam, table.DisplayColumn.Name);
            // "prefix"
            var constant = Expression.Constant(prefixText);
            // row.DisplayName.StartsWith("prefix")
            var startsWithCall = Expression.Call(property, typeof(string).GetMethod("StartsWith", new Type[] { typeof(string) }), constant);
            // row => row.DisplayName.StartsWith("prefix")
            var whereLambda = Expression.Lambda(startsWithCall, entityParam);
            // Customers.Where(row => row.DisplayName.StartsWith("prefix"))
            var whereCall = Expression.Call(typeof(Queryable), "Where", new Type[] { table.EntityType }, query.Expression, whereLambda);
            // Customers.Where(row => row.DisplayName.StartsWith("prefix")).Take(20)
            var takeCall = Expression.Call(typeof(Queryable), "Take", new Type[] { table.EntityType }, whereCall, Expression.Constant(maxCount));

            return query.Provider.CreateQuery(takeCall);
        }

        public static string GetContextKey(MetaTable parentTable) {
            return String.Format("{0}#{1}", parentTable.DataContextType.FullName, parentTable.Name);
        }

        public static MetaTable GetTable(string contextKey) {
            string[] param = contextKey.Split('#');
            Debug.Assert(param.Length == 2, String.Format("The context key '{0}' is invalid", contextKey));
            Type type = Type.GetType(param[0]);
            return MetaModel.GetModel(type).GetTable(param[1], type);
        }

        private static string CreateAutoCompleteItem(MetaTable table, object row) {
            return AutoCompleteExtender.CreateAutoCompleteItem(table.GetDisplayString(row), table.GetPrimaryKeyString(row));
        }
    }
}