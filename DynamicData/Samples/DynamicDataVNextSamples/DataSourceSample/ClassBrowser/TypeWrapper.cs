namespace DataSourcesDemo.ClassBrowser {
    using System;
using System.Web.DomainServices;

    public class TypeWrapper {
        [Exclude]
        public Type Type { get; private set; }
        public TypeWrapper() {
        }

        public TypeWrapper(Type type) {
            Type = type;
        }
    }
}
