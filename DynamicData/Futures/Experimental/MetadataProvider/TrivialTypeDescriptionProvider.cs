using System;
using System.ComponentModel;
using System.Reflection;
using System.Diagnostics;

namespace Microsoft.Web.DynamicData {
    /// <summary>
    /// TypeDescriptionProvider that always returns a fixed ICustomTypeDescriptor
    /// </summary>
    internal class TrivialTypeDescriptionProvider : TypeDescriptionProvider {
        private ICustomTypeDescriptor _typeDescriptor;

        public TrivialTypeDescriptionProvider(ICustomTypeDescriptor typeDescriptor) {
            _typeDescriptor = typeDescriptor;
        }

        public override ICustomTypeDescriptor GetTypeDescriptor(Type objectType, object instance) {
            return _typeDescriptor;
        }
    }
}
