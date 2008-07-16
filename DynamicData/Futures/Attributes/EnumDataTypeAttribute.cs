using System;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;

namespace Microsoft.Web.DynamicData {
    public class EnumDataTypeAttribute : DataTypeAttribute {
        public Type EnumType { get; private set; }

        public EnumDataTypeAttribute(Type enumType)
            : base("Enumeration") {
            if (enumType == null) {
                throw new ArgumentNullException("enumType");
            }
            if (!enumType.IsEnum) {
                throw new ArgumentException("The type needs to be an enum.", "enumType");
            }
            EnumType = enumType;
        }

        public override bool IsValid(object value) {
            if (value == null) {
                return true;
            }

            Type valueType = value.GetType();
            if (valueType.IsEnum && EnumType != valueType) {
                // don't match a different enum that might map to the same underlying integer
                // REVIEW: is this the right thing to do
                return false;
            }

            if (!valueType.IsValueType && valueType != typeof(string)) {
                // non-value types cannot be converted
                return false;
            }

            if (valueType == typeof(bool) ||
                valueType == typeof(float) ||
                valueType == typeof(double) ||
                valueType == typeof(decimal) ||
                valueType == typeof(char)) {
                // non-integral types cannot be converted
                return false;
            }

            object convertedValue;
            if (valueType.IsEnum) {
                Debug.Assert(valueType == value.GetType());
                convertedValue = value;
            } else {
                try {
                    if (value is string) {
                        convertedValue = Enum.Parse(EnumType, (string)value);
                    } else {
                        convertedValue = Enum.ToObject(EnumType, value);
                    }
                } catch (ArgumentException) {
                    // REVIEW: is there a better way to detect this
                    return false;
                }
            }

            if (DynamicDataFutures.IsEnumTypeInFlagsMode(EnumType)) {
                // REVIEW: this seems to be the easiest way to ensure that the value is a valid flag combination
                // If it is, the string representation of the enum value will be something like "A, B", while
                // the string representation of the underlying value will be "3". If the enum value does not
                // match a valid flag combination, then it would also be something like "3".
                string underlying = DynamicDataFutures.GetUnderlyingTypeValueString(EnumType, convertedValue);
                string converted = convertedValue.ToString();
                return !underlying.Equals(converted);
            } else {
                return Enum.IsDefined(EnumType, convertedValue);
            } 
        }
    }
}
