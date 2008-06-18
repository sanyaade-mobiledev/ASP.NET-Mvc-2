using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Web.DynamicData.ModelProviders;

namespace Microsoft.Web.DynamicData.Extensions {
    internal class SimpleTableProvider : TableProvider {

        private ReadOnlyCollection<ColumnProvider> _columns;

        public SimpleTableProvider(DataModelProvider model, Type entityType)
            : base(model) {
            Name = entityType.Name;
            EntityType = entityType;

            var columns = new Collection<ColumnProvider>();

            // Add a column for each public property we find
            foreach (PropertyInfo columnProp in EntityType.GetProperties()) {
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

