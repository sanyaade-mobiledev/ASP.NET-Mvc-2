using System;
using System.Collections.ObjectModel;
using System.Web.DynamicData.ModelProviders;

namespace Microsoft.Web.DynamicData {

    internal class SimpleModelProvider : DataModelProvider {

        private ReadOnlyCollection<TableProvider> _tables;

        public SimpleModelProvider(Type contextType, Type entityType, object dataObject) {
            ContextType = contextType;

            var tables = new Collection<TableProvider>();

            var table = new SimpleTableProvider(this, entityType, dataObject);
            tables.Add(table);

            _tables = new ReadOnlyCollection<TableProvider>(tables);
        }

        public override object CreateContext() {
            throw new NotImplementedException();
        }

        public override ReadOnlyCollection<TableProvider> Tables {
            get {
                return _tables;
            }
        }
    }
}

