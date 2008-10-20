namespace System.Web.Mvc {
    using System;

    internal static class TypeHelpers {
        public static bool TypeAllowsNullValue(Type type) {
            // Only reference types and Nullable<> types allow null values
            return (!type.IsValueType ||
                (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>)));
        }
    }
}
