namespace DataSourcesDemo.ClassBrowser {
    using System.Reflection;
    using System.Web.DomainServices;

    public class MethodInfoWrapper {
        [Exclude]
        public MethodInfo MethodInfo { get; private set; }
        public MethodInfoWrapper() {
        }

        public MethodInfoWrapper(MethodInfo mi) {
            MethodInfo = mi;
        }
    }
}
