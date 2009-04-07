namespace ExtractJsFromAssembly {
    using System;
    using System.Collections.Generic;
    using System.Text;

    public class ScriptResourceInfo {
        private string _resourceName;
        private string _scriptResourceName;
        private string _typeName;

        public ScriptResourceInfo(string resourceName, string scriptResourceName, string typeName) {
            _resourceName = resourceName;
            if (scriptResourceName != null) {
                _scriptResourceName = scriptResourceName.EndsWith(".resources", StringComparison.OrdinalIgnoreCase) ?
                    scriptResourceName.Substring(0, scriptResourceName.Length - 10) : scriptResourceName;
            }
            _typeName = typeName;
        }

        public string ResourceName {
            get { return _resourceName; }
        }

        public string ScriptResourceName {
            get { return _scriptResourceName; }
        }

        public string TypeName {
            get { return _typeName; }
        }
    }
}
