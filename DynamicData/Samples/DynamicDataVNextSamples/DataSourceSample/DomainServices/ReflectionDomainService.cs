namespace DataSourcesDemo.ClassBrowser {
    using System;
    using Microsoft.Web.Data;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Web.Compilation;
    using System.Web.DomainServices;
    using System.Web.UI;
    using System.Web.UI.WebControls;
    using System.IO;
    using System.Net.Sockets;

    public class ReflectionDomainService : DomainService {
        private static Assembly[] assemblysToLoad = { typeof(Control).Assembly, 
                                                    typeof(object).Assembly,
                                                    typeof(LinqDataSource).Assembly,
                                                    typeof(Socket).Assembly,
                                                    typeof(Enumerable).Assembly
                                                  };

        private static IDictionary<string, IEnumerable<Type>> _assemblyMap = null;

        public static IDictionary<string, IEnumerable<Type>> LoadedAssemblies {
            get {
                if (_assemblyMap == null) {
                    _assemblyMap = new Dictionary<string, IEnumerable<Type>>(StringComparer.OrdinalIgnoreCase);
                    foreach (var a in assemblysToLoad) {
                        _assemblyMap.Add(a.GetName().Name, a.GetExportedTypes());
                    }
                }
                return _assemblyMap;
            }
        }

        [Query]
        public IEnumerable<MethodInfoWrapper> GetMethods(string typeName) {
            Type type = BuildManager.GetType(typeName, false /* throwOnError */, true);
            if (type != null) {
                return from m in type.GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly)
                       where !m.Name.Contains("get_") && !m.Name.Contains("set_") && !m.Name.Contains("add_") && !m.Name.Contains("remove_") && !m.Name.Contains("op_")
                       select new MethodInfoWrapper(m);
            }
            return null;
        }

        [Query]
        public IEnumerable<PropertyInfoWrapper> GetProperties(string typeName) {
            Type type = BuildManager.GetType(typeName, false /* throwOnError */, true);
            if (type != null) {
                return from pi in type.GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly)
                       select new PropertyInfoWrapper(pi);
            }
            return null;
        }

        [Query]
        public IEnumerable<FieldInfoWrapper> GetFields(string typeName) {
            Type type = BuildManager.GetType(typeName, false /* throwOnError */, true);
            if (type != null) {
                return from fi in type.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly)
                       select new FieldInfoWrapper(fi);
            }
            return null;
        }

        [Query]
        public IEnumerable<TypeWrapper> GetTypes(string assemblyName) {
            if (String.IsNullOrEmpty(assemblyName)) {
                return LoadedAssemblies.SelectMany(t => t.Value).Select(t => new TypeWrapper(t));
            }

            IEnumerable<Type> types;
            if (LoadedAssemblies.TryGetValue(assemblyName, out types)) {
                return types.Select(t => new TypeWrapper(t));
            }
            return null;
        }
    }
}
