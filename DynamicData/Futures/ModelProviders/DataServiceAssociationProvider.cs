using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Web.DynamicData.ModelProviders;

namespace Microsoft.Web.DynamicData {
    internal class DataServiceAssociationProvider : AssociationProvider {
        public DataServiceAssociationProvider(AssociationDirection direction,
            ColumnProvider fromColumn, ColumnProvider toColumn, IList<String> foreignKeyNames) {
            Direction = direction;
            FromColumn = fromColumn;
            ToColumn = toColumn;
            ForeignKeyNames = new ReadOnlyCollection<string>(foreignKeyNames);
        }
    }
}

