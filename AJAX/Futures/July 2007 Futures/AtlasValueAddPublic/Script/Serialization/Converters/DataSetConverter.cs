namespace Microsoft.Web.Preview.Script.Serialization.Converters {
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Data;
    using System.Web.Script.Serialization;
    using Microsoft.Web.Preview.Resources;

    public class DataSetConverter : JavaScriptConverter {
        private ReadOnlyCollection<Type> _supportedTypes = new ReadOnlyCollection<Type>(new Type[] { typeof(DataSet) });

        public override IEnumerable<Type> SupportedTypes {
            get { return _supportedTypes; }
        }

        public override object Deserialize(IDictionary<string, object> dictionary, Type type, JavaScriptSerializer serializer) {
            throw new NotSupportedException();
        }

        public override IDictionary<string, object> Serialize(object obj, JavaScriptSerializer serializer) {
            DataSet dataSet = obj as DataSet;
            if(dataSet == null) {
                throw new ArgumentException(PreviewWeb.ArgumentShouldBeDataSet, "obj");
            }

            IDictionary<string, object> dataSetJSON = new Dictionary<string, object>();
            
            DataTable[] tables = new DataTable[dataSet.Tables.Count];
            for(int i = 0; i < tables.Length; i++) {
                tables[i] = dataSet.Tables[i];
            }
            dataSetJSON["tables"] = tables;

            return dataSetJSON;
        }
    }
}
