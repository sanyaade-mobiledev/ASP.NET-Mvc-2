using System;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Web.DynamicData;

namespace Microsoft.Web.DynamicData.Extensions {

    internal class LinqImageHelper {

        public static object LoadImage(string tablename, string primarykeyvalue, string imagecolumn) {
            //get iqueryable for table
            MetaTable metatable = MetaModel.Default.GetTable(tablename);
            IQueryable query = metatable.GetQuery();

            //convert primary key to proper type
            MetaColumn member = metatable.PrimaryKeyColumns[0];
            PropertyInfo columnpropertyinfo = member.EntityTypeProperty;
            object value = Convert.ChangeType(primarykeyvalue, member.TypeCode, CultureInfo.InvariantCulture);

            //build linq expression tree for where clause
            var entityparam = Expression.Parameter(metatable.EntityType, "e");
            var columnexpresson = Expression.MakeMemberAccess(entityparam, columnpropertyinfo);
            var equalexpresson = Expression.Equal(columnexpresson, Expression.Constant(value));
            var wherelambda = Expression.Lambda(equalexpresson, entityparam);
            var wherecall = Expression.Call(typeof(Queryable), "Where", new Type[] { metatable.EntityType }, query.Expression, wherelambda);
            query = query.Provider.CreateQuery(wherecall);

            //get record
            var enumerator = query.GetEnumerator();
            enumerator.MoveNext();
            object o = enumerator.Current;
            return o.GetType().GetProperty(imagecolumn).GetValue(o, null);
        }

    }

}