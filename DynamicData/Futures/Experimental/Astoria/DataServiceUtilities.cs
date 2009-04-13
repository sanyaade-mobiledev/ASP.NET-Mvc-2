using System;
using System.Diagnostics;
using System.Data.Services.Common;
using System.Reflection;
using System.Linq;
using System.Xml.Linq;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.Web.Data.Services.Client {
    // TODO: review what should be publicly exposed
    public static class DataServiceUtilities {
        private static readonly Type[] __primitives = { typeof(byte), typeof(sbyte), typeof(short), typeof(ushort),
                                                         typeof(int), typeof(uint), typeof(long), typeof(ulong),
                                                         typeof(string), typeof(byte[]), typeof(Guid), typeof(DateTime),
                                                         typeof(XElement), typeof(XDocument) };

        public static bool IsPrimitiveType(Type t) {
            return __primitives.Contains(t);
        }

        internal static bool IsNullableType(Type type) {
            // It's nullable if it's a Nullable<T>
            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
                return true;
            
            // It's nullable unless it's a value type
            return !type.IsValueType;
        }

        public static bool IsEntityType(Type t, Type contextType) {
            // Astoria convention: a type 't' is an entity if
            // 1) 't' has at least one key column
            // 2) there is a top level IQueryable<T> property in the context where T is 't' or a supertype of 't'
            // Non-primitive types that are not entity types become nested structures ("complex types" in EDM)

            if (!t.GetProperties().Any(p => IsKeyColumn(p))) return false;

            foreach (PropertyInfo pi in contextType.GetProperties()) {
                if (typeof(IQueryable).IsAssignableFrom(pi.PropertyType)) {
                    // TODO: this is clearly over-simplified...assuming simple IQueryable<T>
                    if (pi.PropertyType.GetGenericArguments()[0].IsAssignableFrom(t)) {
                        return true;
                    }
                }
            }

            return false;
        }

        public static bool IsKeyColumn(PropertyInfo pi) {
            // Astoria convention:
            // 1) try the DataServiceKey attribute
            // 2) if not attribute, try <typename>ID
            // 3) finally, try just ID

            object[] attribs = pi.DeclaringType.GetCustomAttributes(typeof(DataServiceKeyAttribute), true);
            if (attribs != null && attribs.Length > 0) {
                Debug.Assert(attribs.Length == 1);
                return ((DataServiceKeyAttribute)attribs[0]).KeyNames.Contains(pi.Name);
            }

            if (pi.Name.Equals(pi.DeclaringType.Name + "ID", System.StringComparison.OrdinalIgnoreCase)) {
                return true;
            }

            if (pi.Name == "ID") {
                return true;
            }

            return false;
        }

        public static IEnumerable<PropertyInfo> EnumerateEntitySetProperties(Type contextType) {
            foreach (PropertyInfo prop in contextType.GetProperties()) {
                if (typeof(IQueryable).IsAssignableFrom(prop.PropertyType) &&
                    prop.PropertyType.GetGenericArguments().Length > 0 &&
                    DataServiceUtilities.IsEntityType(prop.PropertyType.GetGenericArguments()[0], contextType)) {
                    yield return prop;
                }
            }
        }

        public static PropertyInfo FindEntitySetProperty(Type contextType, Type entityType) {
            return EnumerateEntitySetProperties(contextType).Where(
                pi => pi.PropertyType.GetGenericArguments()[0] == entityType).FirstOrDefault();
        }

        public static IEnumerable<string> EnumerateEntitySetNames(Type contextType) {
            return EnumerateEntitySetProperties(contextType).Select(p => p.Name);
        }

        public static string BuildCompositeKey(object entity) {
            StringBuilder buffer = new StringBuilder();
            foreach (PropertyInfo pi in entity.GetType().GetProperties()) {
                if (IsKeyColumn(pi)) {
                    if (buffer.Length != 0) buffer.Append("##");
                    var keyValue = pi.GetValue(entity, null);
                    if (keyValue!=null)
                        buffer.Append(keyValue.ToString());
                }
            }
            return buffer.ToString();
        }
    }
}
