using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web.DynamicData.ModelProviders;
using System.Collections;
using System.ComponentModel;

namespace Microsoft.Web.Data.Services.Client {
    internal class DataServiceColumnProvider : ColumnProvider {
        private PropertyInfo _prop;

        // Only set for automatic association columns
        private DataServiceColumnProvider _fkColumn;

        private DataServiceColumnProvider(TableProvider table)
            : base(table) {
        }

        public DataServiceColumnProvider(TableProvider table, PropertyInfo prop, bool isKey)
            : base(table) {
            _prop = prop;
            EntityTypeProperty = prop;
            Name = _prop.Name;
            ColumnType = _prop.PropertyType;
            IsPrimaryKey = isKey;
            IsSortable = true;
            IsGenerated = false; // We don't have a way of knowing this...could be an issue
            IsForeignKeyComponent = false; // No FKs in Astoria
            IsCustomProperty = false; // All properties are considered part of the contract in Astoria client
            Nullable = DataServiceUtilities.IsNullableType(ColumnType);
        }

        // The logic that figures out the association is not yet ready.  For now we use the second implementation, which
        // gets us going until we iron things out
#if NOTYETREADY
        internal void Initialize() {
            if (DataServiceUtilities.IsEntityType(this.ColumnType, this.Table.DataModel.ContextType)) {
                TableProvider tp = this.Table.DataModel.Tables
                                       .FirstOrDefault(t => t.EntityType.IsAssignableFrom(this.ColumnType));

                if (tp != null) {
                    DataServiceColumnProvider nullColumn = new DataServiceColumnProvider(tp) {
                        ColumnType = typeof(string),
                        Name = "Null(" + this.Table.Name + "." + this.Name + ")"
                    };
                    ((DataServiceTableProvider)tp).AddColumn(nullColumn);

                    Association = new DataServiceAssociationProvider(
                        AssociationDirection.ManyToOne,
                        this,
                        nullColumn,
                        new List<string>() { this.Table.Name + "({0})/" + this.Name });
                }
            }
            // not the best heuristic, but it'll do for now
            else if (!DataServiceUtilities.IsPrimitiveType(this.ColumnType) &&
                     typeof(IEnumerable).IsAssignableFrom(this.ColumnType) &&
                     this.ColumnType.GetGenericArguments().Length > 0 &&
                     DataServiceUtilities.IsEntityType(this.ColumnType.GetGenericArguments()[0], this.Table.DataModel.ContextType)) {
                TableProvider tp = this.Table.DataModel.Tables
                                       .FirstOrDefault(t => t.EntityType.IsAssignableFrom(this.ColumnType));

                if (tp != null) {
                    DataServiceColumnProvider nullColumn = new DataServiceColumnProvider(this.Table) {
                        ColumnType = typeof(string),
                        Name = "Null(" + this.Table.Name + "." + this.Name + ")"
                    };
                    ((DataServiceTableProvider)tp).AddColumn(nullColumn);

                    Association = new DataServiceAssociationProvider(
                        AssociationDirection.ManyToMany,
                        this,
                        nullColumn,
                        new List<string>() { this.Table.Name + "({0})/" + this.Name });
                }
            }
        }
#else
        internal void Initialize() {

            // If we don't have a PropertyInfo, we're likely dealing with an association column
            // we created in TryCreateAssociationColumn, and we should skip it here
            if (EntityTypeProperty == null)
                return;

            // Check if there is an Entity that has the same type as this column. If so, we treat this column as an entity ref
            TableProvider parentTable = Table.DataModel.Tables.SingleOrDefault(t => t.EntityType == ColumnType);
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

            var foreignKeyNames = new List<string>();
            foreignKeyNames.Add(Name + "." + parentPK.Name);

            if (toColumn == null) {
                Association = new DataServiceAssociationProvider(AssociationDirection.ManyToOne, this, parentTable, foreignKeyNames);
            }
            else {
                Association = new DataServiceAssociationProvider(AssociationDirection.ManyToOne, this, toColumn, foreignKeyNames);

                // Create the reverse association
                var reverseAssociation = new DataServiceAssociationProvider(AssociationDirection.OneToMany, toColumn, this, foreignKeyNames);
                toColumn.Association = reverseAssociation;
            }
        }

        internal DataServiceColumnProvider TryCreateAssociationColumn() {

            if (IsPrimaryKey)
                return null;

            // This applies if the column name ends with Id (e.g. StartCityID)
            if (!Name.EndsWith("id", StringComparison.OrdinalIgnoreCase))
                return null;

            // Get the potential name of the (pseudo) navigation property, e.g. StartCity
            string navigationName = Name.Substring(0, Name.Length - 2);
            if (String.IsNullOrEmpty(navigationName))
                return null;

            // Check if there is an Entity that ends with that name, e.g. City
            var parentTable = (DataServiceTableProvider)Table.DataModel.Tables.Where(
                t => navigationName.EndsWith(t.EntityType.Name, StringComparison.OrdinalIgnoreCase)).SingleOrDefault();
            if (parentTable == null)
                return null;

            // Get the prefix of the navigation property, e.g. Start.  If the column name is
            // just CityID, then the prefix is empty.
            string navPropPrefix = navigationName.Substring(
                0, navigationName.Length - parentTable.EntityType.Name.Length);

            var foreignKeyNames = new List<string>();
            foreignKeyNames.Add(Name);

            // Mark this as a FK column
            this.IsForeignKeyComponent = true;

            // Create a matching association column (e.g. StartCity in the Routes table)
            var associationColumn = new DataServiceColumnProvider(Table);

            associationColumn._fkColumn = this;
            associationColumn.Name = navigationName;
            associationColumn.ColumnType = parentTable.EntityType;

            associationColumn.Association = new DataServiceAssociationProvider(AssociationDirection.ManyToOne, associationColumn, parentTable, foreignKeyNames);

            // Create the reverse association column (e.g. StartProducts in the Cities table)
            var reverseAssociationColumn = new DataServiceColumnProvider(parentTable);
            reverseAssociationColumn.Name = navPropPrefix + Table.Name;
            reverseAssociationColumn.ColumnType = Table.EntityType;
            reverseAssociationColumn.Association = new DataServiceAssociationProvider(AssociationDirection.OneToMany, reverseAssociationColumn, associationColumn, foreignKeyNames);

            parentTable.AddColumn(reverseAssociationColumn);

            return associationColumn;
        }

        public override AttributeCollection Attributes {
            get {
                // Expose the FK's attributes as the pseudo-navigation column's attributes
                if (_fkColumn != null)
                    return _fkColumn.Attributes;

                return base.Attributes;
            }
        }
#endif
    }
}

