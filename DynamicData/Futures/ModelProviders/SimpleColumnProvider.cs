using System.Reflection;
using System.Web.DynamicData.ModelProviders;

namespace Microsoft.Web.DynamicData {

    internal class SimpleColumnProvider : ColumnProvider {

        private PropertyInfo _prop;

        public SimpleColumnProvider(TableProvider table, PropertyInfo prop)
            : base(table) {
            _prop = prop;
            Name = _prop.Name;
            ColumnType = _prop.PropertyType;
            EntityTypeProperty = prop;
            Nullable = true;

            IsSortable = true;
        }
    }
}

