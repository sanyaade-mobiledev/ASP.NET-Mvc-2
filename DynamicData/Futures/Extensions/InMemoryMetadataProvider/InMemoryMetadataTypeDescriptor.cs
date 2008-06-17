using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;

namespace Microsoft.Web.DynamicData.Extensions {
    public class InMemoryMetadataTypeDescriptor : CustomTypeDescriptor {
        private Type Type { get; set; }

        public InMemoryMetadataTypeDescriptor(ICustomTypeDescriptor parent, Type type)
            : base(parent) {
            Type = type;
        }

        // Returns the collection of attributes for a given type (table)
        public override AttributeCollection GetAttributes() {
            AttributeCollection baseAttributes = base.GetAttributes();

            List<Attribute> extraAttributes = InMemoryMetadataManager.GetTableAttributes(Type);
            if (extraAttributes.Count != 0) {
                // only create a new collection if it is necessary
                return AttributeCollection.FromExisting(baseAttributes, extraAttributes.ToArray());
            } else {
                return baseAttributes;
            }
        }

        // Returns a collection of properties (columns) for the type, each with attributes
        // for that table
        public override PropertyDescriptorCollection GetProperties() {
            PropertyDescriptorCollection originalCollection = base.GetProperties();

            bool customDescriptorsCreated = false;
            List<PropertyDescriptor> tempPropertyDescriptors = new List<PropertyDescriptor>();

            foreach (PropertyDescriptor propDescriptor in originalCollection) {
                PropertyInfo propInfo = Type.GetProperty(propDescriptor.Name, propDescriptor.PropertyType);
                List<Attribute> newMetadata = InMemoryMetadataManager.GetColumnAttributes(propInfo);
                if (newMetadata.Count > 0) {
                    tempPropertyDescriptors.Add(new PropertyDescriptorWrapper(propDescriptor, newMetadata.ToArray()));
                    customDescriptorsCreated = true;
                } else {
                    tempPropertyDescriptors.Add(propDescriptor);
                }
            }

            if (customDescriptorsCreated) {
                // only create a new collection if it is necessary
                return new PropertyDescriptorCollection(tempPropertyDescriptors.ToArray(), true);
            } else {
                return originalCollection;
            }
        }

        // PropertyDescriptor does not have a straightforward extensibility model that would
        // allow for easy addition of Attributes, so a derived class wrapping an another
        // instance has to be used.
        private class PropertyDescriptorWrapper : PropertyDescriptor {
            private PropertyDescriptor _wrappedPropertyDescriptor;

            public PropertyDescriptorWrapper(PropertyDescriptor wrappedPropertyDescriptor, Attribute[] newAttributes)
                : base(wrappedPropertyDescriptor, newAttributes) {
                _wrappedPropertyDescriptor = wrappedPropertyDescriptor;
            }

            public override bool CanResetValue(object component) {
                return _wrappedPropertyDescriptor.CanResetValue(component);
            }

            public override Type ComponentType {
                get { return _wrappedPropertyDescriptor.ComponentType; }

            }

            public override object GetValue(object component) {
                return _wrappedPropertyDescriptor.GetValue(component);
            }

            public override bool IsReadOnly {
                get { return _wrappedPropertyDescriptor.IsReadOnly; }
            }

            public override Type PropertyType {
                get { return _wrappedPropertyDescriptor.PropertyType; }
            }

            public override void ResetValue(object component) {
                _wrappedPropertyDescriptor.ResetValue(component);
            }

            public override void SetValue(object component, object value) {
                _wrappedPropertyDescriptor.SetValue(component, value);
            }

            public override bool ShouldSerializeValue(object component) {
                return _wrappedPropertyDescriptor.ShouldSerializeValue(component);
            }
        }
    }
}
