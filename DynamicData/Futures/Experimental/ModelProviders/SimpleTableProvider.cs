using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Web.DynamicData.ModelProviders;
using System.ComponentModel;

namespace Microsoft.Web.DynamicData {
    internal class SimpleTableProvider : TableProvider {

        private ReadOnlyCollection<ColumnProvider> _columns;

        public SimpleTableProvider(DataModelProvider model, Type entityType, object dataObject)
            : base(model) {
            Name = entityType.Name;
            EntityType = entityType;

            var columns = new Collection<ColumnProvider>();

            // Add a column for each public property we find
            foreach (PropertyDescriptor columnProp in TypeDescriptor.GetProperties(dataObject)) {
                columns.Add(new SimpleColumnProvider(this, columnProp));
            }

            _columns = new ReadOnlyCollection<ColumnProvider>(columns);
        }

        public override IQueryable GetQuery(object context) {
            throw new NotImplementedException();
        }

        public override ReadOnlyCollection<ColumnProvider> Columns {
            get {
                return _columns;
            }
        }
    }
}

