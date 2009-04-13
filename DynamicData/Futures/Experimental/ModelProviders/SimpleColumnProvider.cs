using System.Reflection;
using System.Web.DynamicData.ModelProviders;
using System.ComponentModel;

namespace Microsoft.Web.DynamicData {

    internal class SimpleColumnProvider : ColumnProvider {

        private PropertyDescriptor _prop;

        public SimpleColumnProvider(TableProvider table, PropertyDescriptor prop)
            : base(table) {
            _prop = prop;
            Name = _prop.Name;
            ColumnType = _prop.PropertyType;
            // TODO: do we really need to set EntityTypeProperty
            //EntityTypeProperty = prop;
            Nullable = true;

            IsSortable = true;
        }
    }
}

