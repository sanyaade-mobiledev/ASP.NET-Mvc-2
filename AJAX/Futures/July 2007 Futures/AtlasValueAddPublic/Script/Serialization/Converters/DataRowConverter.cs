namespace Microsoft.Web.Preview.Script.Serialization.Converters {
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Data;
    using System.Web.Script.Serialization;
    using Microsoft.Web.Preview.Resources;

    public class DataRowConverter : JavaScriptConverter {
        private ReadOnlyCollection<Type> _supportedTypes = new ReadOnlyCollection<Type>(new Type[] { typeof(DataRow) });

        public override IEnumerable<Type> SupportedTypes {
            get { return _supportedTypes; }
        }

        public override object Deserialize(IDictionary<string, object> dictionary, Type type, JavaScriptSerializer serializer) {
            throw new NotSupportedException();
        }

        public override IDictionary<string, object> Serialize(object obj, JavaScriptSerializer serializer) {
            DataRow dataRow = obj as DataRow;
            if (dataRow == null) {
                throw new ArgumentException(PreviewWeb.ArgumentShouldBeDataRow, "obj");
            }

            Dictionary<string, object> rowJSON = new Dictionary<string, object>(dataRow.ItemArray == null ? 0 : dataRow.ItemArray.Length);

            foreach (DataColumn column in dataRow.Table.Columns) {
                rowJSON[column.ColumnName] = dataRow[column];
            }

            return rowJSON;
        }
    }
}
