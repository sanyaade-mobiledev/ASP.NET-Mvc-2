using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Web.DynamicData.ModelProviders;

namespace Microsoft.Web.Data.Services.Client {
    internal class DataServiceAssociationProvider : AssociationProvider {
        public DataServiceAssociationProvider(AssociationDirection direction,
            ColumnProvider fromColumn, ColumnProvider toColumn, IList<String> foreignKeyNames) {
            Direction = direction;
            FromColumn = fromColumn;
            ToColumn = toColumn;
            ForeignKeyNames = new ReadOnlyCollection<string>(foreignKeyNames);
        }

        public DataServiceAssociationProvider(AssociationDirection direction,
            ColumnProvider fromColumn, TableProvider toTable, IList<String> foreignKeyNames) {
            Direction = direction;
            FromColumn = fromColumn;
            ToTable = toTable;
            ForeignKeyNames = new ReadOnlyCollection<string>(foreignKeyNames);
        }
    }
}

