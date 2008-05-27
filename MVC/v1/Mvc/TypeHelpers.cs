namespace System.Web.Mvc {
    using System;

    internal static class TypeHelpers {
        public static bool TypeAllowsNullValue(Type type) {
            // Only classes and Nullable<> types allow null values
            return (type.IsClass ||
                (type.IsGenericType &&
                !type.IsGenericTypeDefinition &&
                type.GetGenericTypeDefinition() == typeof(Nullable<>)));
        }
    }
}
