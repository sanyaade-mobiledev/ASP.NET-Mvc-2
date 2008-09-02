namespace System.Web.Mvc {
    using System;

    internal static class TypeHelpers {
        public static bool TypeAllowsNullValue(Type type) {
            // Only reference types and Nullable<> types allow null values
            return (type.IsClass || type.IsInterface ||
                (type.IsGenericType &&
                !type.IsGenericTypeDefinition &&
                type.GetGenericTypeDefinition() == typeof(Nullable<>)));
        }
    }
}
