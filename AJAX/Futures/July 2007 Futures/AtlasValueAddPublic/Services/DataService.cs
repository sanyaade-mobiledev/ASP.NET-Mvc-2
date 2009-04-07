namespace Microsoft.Web.Preview.Services {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Data;
    using System.Globalization;
    using System.Reflection;
    using System.Web.Services;
    using System.Xml;

    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [DataObject]
    public abstract class DataService : WebService {

        private static Hashtable _dataObjectFieldCache = Hashtable.Synchronized(new Hashtable());
        private static Hashtable _dataMethodCache = Hashtable.Synchronized(new Hashtable());
        private static Hashtable _propertyInfoCache = Hashtable.Synchronized(new Hashtable());
        private static Hashtable _fillMethodCache = Hashtable.Synchronized(new Hashtable());
        private static Hashtable _defaultFillMethodCache = Hashtable.Synchronized(new Hashtable());
        private static Hashtable _updateMethodCache = Hashtable.Synchronized(new Hashtable());

        private readonly object _adapter;
        private readonly Hashtable _fillMethods;
        private readonly MethodInfo _defaultFillMethod;
        private readonly MethodInfo _updateMethod;

        private const BindingFlags PublicBindingFlags =
            BindingFlags.Public |
            BindingFlags.FlattenHierarchy |
            BindingFlags.Static |
            BindingFlags.Instance;

        private const BindingFlags PublicIgnoreCaseBindingFlags = PublicBindingFlags | BindingFlags.IgnoreCase;

        protected DataService() {
            Type type = this.GetType();
            AttributeCollection attrs = TypeDescriptor.GetAttributes(type);
            DataAdapterAttribute attr = (DataAdapterAttribute)attrs[typeof(DataAdapterAttribute)];
            if (attr != null) {
                _adapter = Activator.CreateInstance(attr.DataAdapterType);
            }
            _fillMethods = _fillMethodCache[type] as Hashtable;
            if (_fillMethods == null) {
                if (attr != null) {
                    string[] methodNames = attr.GetDataMethodNames;
                    _fillMethods = Hashtable.Synchronized(new Hashtable(methodNames.Length));
                    foreach (string methodName in methodNames) {
                        MethodInfo method = attr.DataAdapterType.GetMethod(methodName, PublicIgnoreCaseBindingFlags);
                        _fillMethods[methodName] = method;
                        if (_defaultFillMethod == null) {
                            _defaultFillMethod = method;
                        }
                    }
                    if (!String.IsNullOrEmpty(attr.UpdateMethodName)) {
                        MemberFilter filter = new MemberFilter(FilterMethodWithDataTableParameter);
                        MemberInfo[] updateMethods = attr.DataAdapterType.FindMembers(
                            MemberTypes.Method,
                            PublicIgnoreCaseBindingFlags,
                            filter,
                            attr.UpdateMethodName);
                        if (updateMethods.Length != 0) {
                            _updateMethod = (MethodInfo)updateMethods[0];
                        }
                    }
                }
                else {
                    _fillMethods = Hashtable.Synchronized(new Hashtable());
                }
                _updateMethodCache[type] = _updateMethod;
                _defaultFillMethodCache[type] = _defaultFillMethod;
                _fillMethodCache[type] = _fillMethods;
            }
            else {
                _updateMethod = _updateMethodCache[type] as MethodInfo;
                _defaultFillMethod = _defaultFillMethodCache[type] as MethodInfo;
            }
        }

        [WebMethod]
        public DataTable GetData(object parameters, string loadMethod) {
            return GetDataImplementation(parameters, loadMethod);
        }

        protected virtual DataTable GetDataImplementation(object parameters, string loadMethod) {
            MethodInfo selectMethod;
            object target;
            if (_adapter != null) {
                target = _adapter;
                if (String.IsNullOrEmpty(loadMethod)) {
                    selectMethod = _defaultFillMethod;
                }
                else {
                    selectMethod = _fillMethods[loadMethod] as MethodInfo;
                    if (selectMethod == null) {
                        throw new ArgumentOutOfRangeException("loadMethod",
                            String.Format(CultureInfo.CurrentCulture, "Method {0} is not available on the data service.", loadMethod));
                    }
                }
            }
            else {
                target = this;
                selectMethod = GetDataMethod(DataObjectMethodType.Select, loadMethod);
            }
            if (selectMethod == null) {
                throw new InvalidOperationException("The Data Service must implement one method marked with the DataObjectMethod(DataObjectMethodType.Select) attribute or have a DataAdapterAttribute.");
            }
            object data = InvokeMethod(selectMethod, target, parameters);
            DataTable table = data as DataTable;
            if (table == null) {
                table = new DataTable("data");
                table.Locale = CultureInfo.CurrentCulture;
                IEnumerable enumerableData = data as IEnumerable;
                if (enumerableData == null) {
                    throw new InvalidOperationException("The data returned by a select method must be IEnumerable or a DataTable.");
                }
                IEnumerator enumerator = enumerableData.GetEnumerator();
                if (enumerator.MoveNext()) {
                    object firstRow = enumerator.Current;
                    MemberInfo[] dataColumns = GetDataObjectFields(firstRow);
                    if (dataColumns.Length == 0) {
                        // Just construct a one-column table
                        DataColumn column = new DataColumn("_rowObject", firstRow.GetType());
                        table.Columns.Add(column);
                        foreach (object row in enumerableData) {
                            DataRow dataRow = table.NewRow();
                            dataRow[0] = row;
                            table.Rows.Add(dataRow);
                        }
                    }
                    else {
                        List<DataColumn> keys = new List<DataColumn>();
                        foreach (PropertyInfo propertyInfo in dataColumns) {
                            DataColumn column = new DataColumn(propertyInfo.Name, propertyInfo.PropertyType);
                            if (((DataObjectFieldAttribute)Attribute.GetCustomAttribute(propertyInfo, typeof(DataObjectFieldAttribute))).PrimaryKey) {
                                keys.Add(column);
                            }
                            DefaultValueAttribute defaultAttribute =
                                (DefaultValueAttribute)Attribute.GetCustomAttribute(propertyInfo, typeof(DefaultValueAttribute));
                            if (defaultAttribute != null) {
                                column.DefaultValue = defaultAttribute.Value;
                            }
                            table.Columns.Add(column);
                        }
                        table.PrimaryKey = keys.ToArray();
                        foreach (object row in enumerableData) {
                            DataRow dataRow = table.NewRow();
                            foreach (PropertyInfo propertyInfo in dataColumns) {
                                dataRow[propertyInfo.Name] = propertyInfo.GetValue(row, null);
                            }
                            table.Rows.Add(dataRow);
                        }
                    }
                }
            }
            return table;
        }

        [WebMethod]
        public DataTable SaveData(ChangeList changeList, object parameters, string loadMethod) {
            return SaveDataImplementation(changeList, parameters, loadMethod);
        }

        protected virtual DataTable SaveDataImplementation(ChangeList changeList, object parameters, string loadMethod) {
            if ((_adapter != null) && (_updateMethod != null)) {
                DataTable dataTable = GetData(parameters, loadMethod);
                foreach (object rowToDelete in changeList.Deleted) {
                    FindRow(GetDictionary(rowToDelete, dataTable.Columns), dataTable).Delete();
                }
                foreach (object rowToUpdate in changeList.Updated) {
                    BaseTypedDictionary rowDict = GetDictionary(rowToUpdate, dataTable.Columns);
                    DataRow tableRow = FindRow(rowDict, dataTable);
                    CopyToRow(rowDict, tableRow);
                }
                foreach (object rowToInsert in changeList.Inserted) {
                    DataRow newRow = dataTable.NewRow();
                    dataTable.Rows.Add(newRow);
                    BaseTypedDictionary rowDict = GetDictionary(rowToInsert, dataTable.Columns);
                    CopyToRow(rowDict, newRow);
                }
                _updateMethod.Invoke(_adapter, new object[] { dataTable });
                return dataTable;
            }
            else {
                MethodInfo deleteMethod = GetDataMethod(DataObjectMethodType.Delete);
                MethodInfo updateMethod = GetDataMethod(DataObjectMethodType.Update);
                MethodInfo insertMethod = GetDataMethod(DataObjectMethodType.Insert);

                if (deleteMethod != null) {
                    foreach (object rowToDelete in changeList.Deleted) {
                        InvokeMethod(deleteMethod, this, rowToDelete);
                    }
                }
                else if (changeList.Deleted.Length != 0) {
                    throw new InvalidOperationException("The Data Service must implement one method marked with the DataObjectMethod(DataObjectMethodType.Delete) attribute to handle deleted rows.");
                }

                if (updateMethod != null) {
                    foreach (object rowToUpdate in changeList.Updated) {
                        InvokeMethod(updateMethod, this, rowToUpdate);
                    }
                }
                else if (changeList.Updated.Length != 0) {
                    throw new InvalidOperationException("The Data Service must implement one method marked with the DataObjectMethod(DataObjectMethodType.Update) attribute to handle updated rows.");
                }

                if (insertMethod != null) {
                    foreach (object rowToInsert in changeList.Inserted) {
                        InvokeMethod(insertMethod, this, rowToInsert);
                    }
                }
                else if (changeList.Inserted.Length != 0) {
                    throw new InvalidOperationException("The Data Service must implement one method marked with the DataObjectMethod(DataObjectMethodType.Insert) attribute to handle inserted rows.");
                }
                return GetData(parameters, loadMethod);
            }
        }

        private static void CopyToRow(BaseTypedDictionary dictionary, DataRow row) {
            foreach (KeyValuePair<string, object> kvp in dictionary) {
                DataColumn col = row.Table.Columns[kvp.Key];
                if ((col != null) && !col.ReadOnly && !col.AutoIncrement) {
                    row[col] = kvp.Value;
                }
            }
        }

        private static bool FilterDataObjectProperty(MemberInfo propertyInfo, object unused) {
            DataObjectFieldAttribute dataObjectFieldAttribute =
                Attribute.GetCustomAttribute(propertyInfo, typeof(DataObjectFieldAttribute)) as DataObjectFieldAttribute;
            return (dataObjectFieldAttribute != null);
        }

        private static bool FilterMethodByDataObjectMethodType(MemberInfo methodInfo, object objSearch) {
            DataObjectMethodAttribute dataObjectMethodAttribute =
                Attribute.GetCustomAttribute(methodInfo, typeof(DataObjectMethodAttribute)) as DataObjectMethodAttribute;
            if (dataObjectMethodAttribute == null) {
                return false;
            }
            MethodTypeAndName typeAndName = (MethodTypeAndName)objSearch;
            if (String.IsNullOrEmpty(typeAndName.MethodName)) {
                return (dataObjectMethodAttribute.IsDefault || typeAndName.FirstFoundIsDefault) &&
                    (dataObjectMethodAttribute.MethodType == typeAndName.MethodType);
            }
            return (methodInfo.Name.Equals(typeAndName.MethodName, StringComparison.OrdinalIgnoreCase) &&
                (dataObjectMethodAttribute.MethodType == typeAndName.MethodType));
        }

        private static bool FilterMethodWithDataTableParameter(MemberInfo memberInfo, object objSearch) {
            string methodName = objSearch as string;
            if (!String.Equals(methodName, memberInfo.Name, StringComparison.OrdinalIgnoreCase)) {
                return false;
            }
            MethodInfo method = memberInfo as MethodInfo;
            ParameterInfo[] parameters = method.GetParameters();
            return ((parameters.Length == 1) && (
                (parameters[0].ParameterType == typeof(DataTable))
                || (parameters[0].ParameterType.IsSubclassOf(typeof(DataTable)))));
        }

        private static DataRow FindRow(BaseTypedDictionary row, DataTable table) {
            DataColumn[] tableKeys = table.PrimaryKey;
            object[] rowKeys = new object[tableKeys.Length];
            for (int i = 0; i < tableKeys.Length; i++) {
                DataColumn tableKey = tableKeys[i];
                string keyName = tableKey.ColumnName;
                rowKeys[i] = row[keyName];
            }
            return table.Rows.Find(rowKeys);
        }

        private MethodInfo GetDataMethod(DataObjectMethodType methodType) {
            return GetDataMethod(methodType, null);
        }

        private MethodInfo GetDataMethod(DataObjectMethodType methodType, string methodName) {
            if (methodName == null) {
                methodName = String.Empty;
            }
            Type thisType = this.GetType();
            Hashtable methods = _dataMethodCache[thisType] as Hashtable;
            if (methods == null) {
                methods = Hashtable.Synchronized(new Hashtable());
                _dataMethodCache[thisType] = methods;
            }
            Hashtable infos = methods[methodType] as Hashtable;
            if (infos == null) {
                infos = Hashtable.Synchronized(new Hashtable());
                methods[methodType] = infos;
            }
            MethodInfo method;
            if (!infos.ContainsKey(methodName)) {
                MemberFilter filter = new MemberFilter(FilterMethodByDataObjectMethodType);
                MemberInfo[] dataMethods = thisType.FindMembers(
                    MemberTypes.Method,
                    PublicBindingFlags,
                    filter,
                    new MethodTypeAndName(methodType, methodName, false));
                if (dataMethods.Length == 0) {
                    dataMethods = thisType.FindMembers(
                        MemberTypes.Method,
                        PublicBindingFlags,
                        filter,
                        new MethodTypeAndName(methodType, methodName, true));
                    if (dataMethods.Length == 0) {
                        infos[methodName] = null;
                        return null;
                    }
                }
                method = (MethodInfo)dataMethods[0];
                infos[methodName] = method;
            }
            else {
                method = (MethodInfo)infos[methodName];
            }
            return method;
        }

        private static MemberInfo[] GetDataObjectFields(object dataObject) {
            Type dataObjectType = dataObject.GetType();
            MemberInfo[] dataObjectFields = _dataObjectFieldCache[dataObjectType] as MemberInfo[];
            if (dataObjectFields == null) {
                dataObjectFields = dataObject.GetType().FindMembers(
                    MemberTypes.Property,
                    BindingFlags.Public | BindingFlags.Instance,
                    new MemberFilter(FilterDataObjectProperty),
                    null);
                _dataObjectFieldCache[dataObjectType] = dataObjectFields;
            }
            return dataObjectFields;
        }

        internal static PropertyInfo GetPropertyInfo(Type type, string propertyName) {
            Hashtable properties = _propertyInfoCache[type] as Hashtable;
            if (properties == null) {
                properties = Hashtable.Synchronized(new Hashtable());
                _propertyInfoCache[type] = properties;
            }
            PropertyInfo property = properties[propertyName] as PropertyInfo;
            if (property == null) {
                property = type.GetProperty(propertyName,
                    BindingFlags.IgnoreCase | BindingFlags.Instance | BindingFlags.Public);
                properties[propertyName] = property;
            }
            return property;
        }

        /// <summary>
        /// Abstracts an object's property collection, an array of XmlNodes and a dictionary of string/values
        /// as an IDictionary&lt;string, object&gt;.
        /// </summary>
        /// <param name="dictionaryStructure">The object, XmlNode[] or dictionary to abstract</param>
        /// <param name="targetParameterInfos">A list of ParameterInfos used to infer the types of properties</param>
        /// <returns>An IDictionary&lt;string, object&gt; that represents the object</returns>
        private static BaseTypedDictionary GetDictionary(object dictionaryStructure, IEnumerable targetParameterInfos) {
            IDictionary<string, object> dict = dictionaryStructure as IDictionary<string, object>;
            if (dict != null) {
                return new TypedDictionary(dict, targetParameterInfos);
            }
            XmlNode[] dictAsXml = dictionaryStructure as XmlNode[];
            if (dictAsXml != null) {
                return new XmlDictionary(dictAsXml, targetParameterInfos);
            }
            return new ObjectDictionary(dictionaryStructure);
        }

        /// <summary>
        /// Invokes a method on an object by dynamically constructing its argument list.
        /// </summary>
        /// <param name="method">The method to invoke.</param>
        /// <param name="target">The target of the method invocation.</param>
        /// <param name="parameter">An object or IDictionary that describes the method parameters.</param>
        /// <returns>The return value of the method.</returns>
        private static object InvokeMethod(MethodInfo method, object target, object parameter) {
            ParameterInfo[] targetMethodParameters = method.GetParameters();
            object[] parameterValues = new object[targetMethodParameters.Length];
            if (targetMethodParameters.Length > 0) {
                if (parameter == null) {
                    throw new ArgumentNullException("parameter");
                }
                if (targetMethodParameters.Length == 1) {
                    Type parameterType = targetMethodParameters[0].ParameterType;
                    if (parameterType.IsAssignableFrom(parameter.GetType())) {
                        // 1. There is one destination parameter whose type is directly compatible with the parameter we have
                        parameterValues[0] = parameter;
                    }
                    else {
                        // Create a dictionary from the parameter
                        BaseTypedDictionary parameterDictionary = GetDictionary(parameter, targetMethodParameters);
                        object value;
                        if (parameterDictionary.TryGetValue(targetMethodParameters[0].Name, out value)) {
                            // 2. There is one destination parameter whose name exists in our parameter dictionary
                            parameterValues[0] = value;
                        }
                        else {
                            // 3. There is one destination parameter with a complex type of which we must reconstruct
                            // an instance from the dictionary

                            // Construct the instance
                            object parameterValue = Activator.CreateInstance(parameterType);

                            // Copy property values from dictionary entries
                            // DataService does NOT support fields because DataObjectFieldAttribute
                            // can't be applied to fields.
                            // DataService implementations that use the attributes won't be able to use fields
                            // and implementations that don't won't use InvokeMethod.
                            foreach (KeyValuePair<string, object> entry in parameterDictionary) {
                                PropertyInfo propertyInfo = GetPropertyInfo(parameterType, entry.Key);
                                if (propertyInfo != null && propertyInfo.CanWrite) {
                                    propertyInfo.SetValue(parameterValue, entry.Value, null);
                                }
                            }
                            parameterValues[0] = parameterValue;
                        }
                    }
                }
                else {
                    // 4. There are multiple simple type parameters that we copy from the parameter dictionary
                    BaseTypedDictionary parameterDictionary = GetDictionary(parameter, targetMethodParameters);
                    for (int i = 0; i < targetMethodParameters.Length; i++) {
                        ParameterInfo parameterInfo = targetMethodParameters[i];
                        object value;
                        if (parameterDictionary.TryGetValue(parameterInfo.Name, out value)) {
                            parameterValues[i] = value;
                        }
                        else {
                            throw new InvalidOperationException();
                        }
                    }
                }
            }
            return method.Invoke(target, parameterValues);
        }

        private sealed class MethodTypeAndName {
            private DataObjectMethodType _methodType;
            private string _name;
            private bool _firstFoundIsDefault;

            private MethodTypeAndName() { }

            public MethodTypeAndName(DataObjectMethodType methodType, string methodName, bool firstFoundIsDefault) {
                _methodType = methodType;
                _name = methodName;
                _firstFoundIsDefault = firstFoundIsDefault;
            }

            public bool FirstFoundIsDefault {
                get {
                    return _firstFoundIsDefault;
                }
            }

            public string MethodName {
                get {
                    return _name;
                }
            }

            public DataObjectMethodType MethodType {
                get {
                    return _methodType;
                }
            }
        }
    }
}