using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Web.DynamicData.ModelProviders;
using System.Web.UI;

namespace Microsoft.Web.Data.Services.Client {
    internal class DataServiceTableProvider : TableProvider {
        private PropertyInfo _prop;
        private Collection<ColumnProvider> _columns;
        private ReadOnlyCollection<ColumnProvider> _readOnlyColumns;

        public DataServiceTableProvider(DataModelProvider model, PropertyInfo prop)
            : base(model) {
            this._prop = prop;
            Name = prop.Name;
            EntityType = _prop.PropertyType.GetGenericArguments()[0];

            _columns = new Collection<ColumnProvider>();
            _readOnlyColumns = new ReadOnlyCollection<ColumnProvider>(_columns);

            AddColumnsRecursive(EntityType);

        }

        /// <summary>
        /// The reason we use this recursive approach instead of simply not specifying BindingFlags.DeclaredOnly
        /// is that we want to make sure that we get the base type columns before the derive type columns, and the
        /// default reflection behavior does it the other way around
        /// </summary>
        private void AddColumnsRecursive(Type entityType) {
            // First add all the base type's columns
            if (entityType != typeof(object)) {
                AddColumnsRecursive(entityType.BaseType);
            }

            // Then add the columns for the current type
            foreach (PropertyInfo columnProp in entityType.GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly)) {
                _columns.Add(new DataServiceColumnProvider(
                    this, columnProp, IsKeyColumn(columnProp)));
            }
        }

        private bool IsKeyColumn(PropertyInfo columnProp) {
            return DataServiceUtilities.IsKeyColumn(columnProp);
        }

        public override IQueryable GetQuery(object context) {
            object contextPropValue = _prop.GetValue(context, null);

            if (!(contextPropValue is IQueryable)) {
                throw new NotSupportedException("Entry points representing tables must implement IQueryable");
            }

            return (IQueryable)contextPropValue;
        }

        public override ReadOnlyCollection<ColumnProvider> Columns {
            get {
                return _readOnlyColumns;
            }
        }

        internal void AddColumn(ColumnProvider cp) {
            _columns.Add(cp);
        }

        public override object EvaluateForeignKey(object row, string foreignKeyName) {
            return DataBinder.Eval(row, foreignKeyName);
        }

        internal void Initialize() {
            for (int i=0; i<_columns.Count; i++) {
                var column = (DataServiceColumnProvider)_columns[i];

                var associationColumn = column.TryCreateAssociationColumn();
                if (associationColumn != null) {
                    // Replace the FK column by the association column
                    _columns.RemoveAt(i);
                    _columns.Insert(i, associationColumn);
                }
                else {
                    column.Initialize();
                }
            }
        }

    }
}

