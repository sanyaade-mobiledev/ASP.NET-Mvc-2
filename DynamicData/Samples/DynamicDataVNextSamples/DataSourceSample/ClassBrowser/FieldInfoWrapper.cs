namespace DataSourcesDemo.ClassBrowser {
    using System.Reflection;
    using System.Web.DomainServices;

    public class FieldInfoWrapper {
        [Exclude]
        public FieldInfo FieldInfo { get; set; }
        public FieldInfoWrapper() {
        }

        public FieldInfoWrapper(FieldInfo fi) {
            FieldInfo = fi;
        }
    }
}
