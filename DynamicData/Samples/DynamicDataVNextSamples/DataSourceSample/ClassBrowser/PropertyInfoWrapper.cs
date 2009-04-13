namespace DataSourcesDemo.ClassBrowser {
    using System.Reflection;
    using System.Web.DomainServices;

    public class PropertyInfoWrapper {
        [Exclude]
        public PropertyInfo PropertyInfo { get; private set; }
        public PropertyInfoWrapper() {
        }

        public PropertyInfoWrapper(PropertyInfo pi) {
            PropertyInfo = pi;
        }
    }
}
