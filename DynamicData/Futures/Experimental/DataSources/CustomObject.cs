using System;
using System.ComponentModel;

namespace Microsoft.Web.DynamicData {
    public class CustomObject : ICustomTypeDescriptor {
        PropertyDescriptorCollection _propertyDescriptorCollection;

        public CustomObject() {
            _propertyDescriptorCollection = new PropertyDescriptorCollection(new PropertyDescriptor[0]);
        }

        public PropertyDescriptorCollection Properties {
            get {
                return _propertyDescriptorCollection;
            }
        }

        #region ICustomTypeDescriptor Members

        public AttributeCollection GetAttributes() {
            return new AttributeCollection();
        }

        public string GetClassName() {
            return "TODO: GetClassName()";
        }

        public string GetComponentName() {
            return "TODO: GetComponentName()";
        }

        public TypeConverter GetConverter() {
            return null;
        }

        public EventDescriptor GetDefaultEvent() {
            return null;
        }

        public PropertyDescriptor GetDefaultProperty() {
            return null;
        }

        public object GetEditor(Type editorBaseType) {
            return null;
        }

        public EventDescriptorCollection GetEvents(Attribute[] attributes) {
            throw new NotImplementedException();
        }

        public EventDescriptorCollection GetEvents() {
            throw new NotImplementedException();
        }

        public PropertyDescriptorCollection GetProperties(Attribute[] attributes) {
            throw new NotImplementedException();
        }

        public PropertyDescriptorCollection GetProperties() {
            return _propertyDescriptorCollection;
        }

        public object GetPropertyOwner(PropertyDescriptor pd) {
            throw new NotImplementedException();
        }

        #endregion
    }

    public class CustomPropertyDescriptor : PropertyDescriptor {
        public Type _propertyType { get; set; }

        public object Value { get; set; }

        public CustomPropertyDescriptor(string name, Type type)
            : this(name, type, null) {
        }

        public CustomPropertyDescriptor(string name, Type type, Attribute[] attributes)
            : this(name, type, attributes, null) {
        }

        public CustomPropertyDescriptor(string name, Type type, Attribute[] attributes, object value)
            : base(name, attributes) {
            _propertyType = type;
            Value = value;
        }

        public override bool CanResetValue(object component) {
            return false;
        }

        public override Type ComponentType {
            get { throw new NotImplementedException(); }
        }

        public override object GetValue(object component) {
            return Value;
        }

        public override bool IsReadOnly {
            get {
                return false;
            }
        }

        public override Type PropertyType {
            get {
                return _propertyType;
            }
        }

        public override void ResetValue(object component) {
            throw new NotImplementedException();
        }

        public override void SetValue(object component, object value) {
            Value = value;
        }

        public override bool ShouldSerializeValue(object component) {
            throw new NotImplementedException();
        }
    }
}

