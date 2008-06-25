using System;
using System.ComponentModel;

namespace Microsoft.Web.DynamicData {
    public class InMemoryMetadataTypeDescriptionProvider : TypeDescriptionProvider {
        private Type Type { get; set; }

        // Creates an instance for the given type. The InMemoryMetadataTypeDescriptionProvider will fall back
        // to default reflection-based behavior when retrieving attributes.
        public InMemoryMetadataTypeDescriptionProvider(Type type)
            : this(type, TypeDescriptor.GetProvider(type)) {
            Type = type;
        }

        // Creates an instance for the given type. The InMemoryMetadataTypeDescriptionProvider will use the given
        // parent provider for chaining 
        public InMemoryMetadataTypeDescriptionProvider(Type type, TypeDescriptionProvider parentProvider)
            : base(parentProvider) {
            Type = type;
        }
        
        public override ICustomTypeDescriptor GetTypeDescriptor(Type objectType, object instance) {
            return new InMemoryMetadataTypeDescriptor(base.GetTypeDescriptor(objectType, instance), Type);
        }
    }
}
