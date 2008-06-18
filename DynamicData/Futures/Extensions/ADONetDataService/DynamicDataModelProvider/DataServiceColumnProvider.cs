using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web.DynamicData.ModelProviders;

namespace Microsoft.Web.DynamicData.Extensions {
    internal class DataServiceColumnProvider : ColumnProvider {

        private PropertyInfo _prop;

        public DataServiceColumnProvider(TableProvider table, PropertyInfo prop, bool first)
            : base(table) {
            _prop = prop;
            Name = _prop.Name;
            ColumnType = _prop.PropertyType;

            // Make the first column the PK, as a simple simplifying assumption
            IsPrimaryKey = first;

            IsSortable = true;
        }

        internal void Initialize() {
            // Check if there is an Entity that has the same type as this column. If so, we treak this column as an entity ref
            TableProvider parentTable = Table.DataModel.Tables.Where(t => t.EntityType == ColumnType).SingleOrDefault();
            if (parentTable == null)
                return;

            DataServiceColumnProvider toColumn = null;

            ColumnProvider parentPK = null;

            // Look for the matching Entity Set column in the parent table
            foreach (DataServiceColumnProvider parentColumn in parentTable.Columns) {

                // Pick up the parent table's PK column on the way
                if (parentColumn.IsPrimaryKey)
                    parentPK = parentColumn;

                // The Entity Set column is expected to be of a type like IList<AcmeProduct>

                if (!parentColumn.ColumnType.IsGenericType)
                    continue;

                Type childrenType = parentColumn.ColumnType.GetGenericArguments()[0];
                if (childrenType == Table.EntityType) {
                    toColumn = parentColumn;
                    break;
                }
            }

            if (toColumn == null) {
                throw new Exception(String.Format("Can't find To column for column '{0}'", Name));
            }

            var foreignKeyNames = new List<string>();
            foreignKeyNames.Add(Name + "." + parentPK.Name);

            Association = new DataServiceAssociationProvider(AssociationDirection.ManyToOne, this, toColumn, foreignKeyNames);

            // Create the reverse association
            var reverseAssociation = new DataServiceAssociationProvider(AssociationDirection.OneToMany, toColumn, this, foreignKeyNames);
            toColumn.Association = reverseAssociation;
        }
    }
}

