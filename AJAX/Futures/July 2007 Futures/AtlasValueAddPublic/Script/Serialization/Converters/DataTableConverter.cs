namespace Microsoft.Web.Preview.Script.Serialization.Converters {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Data;
    using System.Web.Script.Serialization;
    using Microsoft.Web.Preview.Resources;

    public class DataTableConverter : JavaScriptConverter {
        private ReadOnlyCollection<Type> _supportedTypes = new ReadOnlyCollection<Type>(new Type[] { typeof(DataTable) });

        public override IEnumerable<Type> SupportedTypes {
            get { 
                return _supportedTypes;
            }
        }

        public override object Deserialize(IDictionary<string, object> dictionary, Type type, JavaScriptSerializer serializer) {
            throw new NotSupportedException();
        }

        public override IDictionary<string, object> Serialize(object obj, JavaScriptSerializer serializer) {
            DataTable dataTable = obj as DataTable;

            if (dataTable == null) {
                throw new ArgumentException(PreviewWeb.ArgumentShouldBeDataTable, "obj");
            }

            IDictionary<string, object> tableJSON = new Dictionary<string, object>(2);

            if(dataTable.Columns.Count > 0) {
                IDictionary<string,object>[] columns = new Dictionary<string,object>[dataTable.Columns.Count];
                // Note that there is no DataColumnConverter since columns dont make sense by themselves.
                // We serialize them into dictionaries directly.

                // do primary key columns first
                DataColumn[] primaryKeyColumns = dataTable.PrimaryKey;
                int i = 0;
                foreach(DataColumn column in primaryKeyColumns) {
                    columns[i] = this.SerializeDataColumn(column, true, serializer);
                    i++;
                }
                
                // then iterate over the rest of the columns until the column array is full
                int j = 0;
                while(i < columns.Length) {
                    DataColumn column = dataTable.Columns[j];
                    if(Array.IndexOf(primaryKeyColumns, column) == -1) {
                        columns[i] = this.SerializeDataColumn(column, false, serializer);
                        i++;
                    }
                    j++;
                }
                tableJSON["columns"] = columns;
            }
            else {
                tableJSON["columns"] = null;
            }

            // serialize rows
            if(dataTable.Rows.Count > 0) {
                DataRow[] rows = new DataRow[dataTable.Rows.Count];
                for(int i = 0; i < rows.Length; i++) {
                    // the DataRowConverter will take care of these
                    rows[i] = dataTable.Rows[i];
                }
                tableJSON["rows"] = rows;
            }
            else {
                tableJSON["rows"] = null;
            }

            return tableJSON;
        }

        protected virtual IDictionary<string, object> SerializeDataColumn(DataColumn column, bool isPrimaryKeyColumn, JavaScriptSerializer serializer) {
            IDictionary<string, object> columnJSON = new Dictionary<string, object>(5);
            columnJSON["name"] = column.ColumnName;
            columnJSON["dataType"] = this.GetClientTypeNameForType(column.DataType);
            columnJSON["defaultValue"] = column.DefaultValue == DBNull.Value ? null : column.DefaultValue;
            columnJSON["readOnly"] = column.ReadOnly;
            columnJSON["isKey"] = isPrimaryKeyColumn;

            return columnJSON;
        }

        protected virtual string GetClientTypeNameForType(Type type) {
            if (typeof(string).IsAssignableFrom(type) || typeof(char).IsAssignableFrom(type)) {
                return "String";
            }
            else if (typeof(bool).IsAssignableFrom(type)) {
                return "Boolean";
            }
            else if (typeof(DateTime).IsAssignableFrom(type)) {
                return "Date";
            }
            else if (typeof(System.Int32).IsAssignableFrom(type) ||
                typeof(double).IsAssignableFrom(type) ||
                typeof(System.Single).IsAssignableFrom(type) ||
                typeof(System.Int64).IsAssignableFrom(type) ||
                typeof(System.Int16).IsAssignableFrom(type) ||
                typeof(System.Byte).IsAssignableFrom(type) ||
                typeof(System.UInt32).IsAssignableFrom(type) ||
                typeof(System.UInt64).IsAssignableFrom(type) ||
                typeof(System.UInt16).IsAssignableFrom(type) ||
                typeof(System.SByte).IsAssignableFrom(type)) {
                return "Number";
            }
            return "Object";
        }
    }
}
