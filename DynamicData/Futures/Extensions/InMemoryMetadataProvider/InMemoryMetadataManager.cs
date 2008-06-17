using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;

namespace Microsoft.Web.DynamicData.Extensions {
    public static class InMemoryMetadataManager {
        private static Dictionary<Type, List<Attribute>> s_tableAttributes = new Dictionary<Type, List<Attribute>>();
        private static Dictionary<PropertyInfo, List<Attribute>> s_columnAttributes = new Dictionary<PropertyInfo, List<Attribute>>();

        public static void AddTableAttributes<T>(params Attribute[] attributes) where T : class {
            AddTableAttributes(typeof(T), attributes);
        }

        public static void AddTableAttributes(Type table, params Attribute[] attributes) {
            if (attributes == null) throw new ArgumentNullException("attribute");

            List<Attribute> propAttributes ;
            if (!s_tableAttributes.TryGetValue(table, out propAttributes)) {
                propAttributes = new List<Attribute>();
                s_tableAttributes[table] = propAttributes;
            }
            propAttributes.AddRange(attributes);
        }

        internal static List<Attribute> GetTableAttributes(Type type) {
            List<Attribute> attributes;
            return s_tableAttributes.TryGetValue(type, out attributes) ? attributes : new List<Attribute>();
        }

        // Allows for strongly type property references:
        // AddColumnAttribute<Product>( p => p.ProductName, ...)
        public static void AddColumnAttributes<T>(Expression<Func<T, object>> propertyAccessor, params Attribute[] attributes) where T : class {
            AddColumnAttributes(GetProperty<T>(propertyAccessor), attributes);
        }

        public static void AddColumnAttributes(PropertyInfo prop, params Attribute[] attributes) {
            if (attributes == null) throw new ArgumentNullException("attribute");

            List<Attribute> attributeCollection;
            if (!s_columnAttributes.TryGetValue(prop, out attributeCollection)) {
                attributeCollection = new List<Attribute>();
                s_columnAttributes[prop] = attributeCollection;
            }
            attributeCollection.AddRange(attributes);
        }

        internal static List<Attribute> GetColumnAttributes(PropertyInfo property) {
            List<Attribute> attributes;
            return s_columnAttributes.TryGetValue(property, out attributes) ? attributes : new List<Attribute>();
        }

        private static PropertyInfo GetProperty<T>(Expression<Func<T, object>> propertyAccessor) {
            if (propertyAccessor == null) throw new ArgumentNullException("propertyAccessor");
            try {
                // o => o.Property
                LambdaExpression lambda = (LambdaExpression)propertyAccessor;

                MemberExpression member;
                if (lambda.Body is UnaryExpression) {
                    // If the property is not an Object, then the member access expression will be wrapped in a conversion expression
                    // (object)o.Property
                    UnaryExpression convert = (UnaryExpression)lambda.Body;
                    // o.Property
                    member = (MemberExpression)convert.Operand;
                } else {
                    // o.Property
                    member = (MemberExpression)lambda.Body;
                }
                // Property
                PropertyInfo property = (PropertyInfo)member.Member;

                return property;
            } catch(Exception e) {
                throw new ArgumentException("The property accessor expression is not in the expected format 'o => o.Property'.", e);
            }
        }
    }
}
