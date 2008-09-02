namespace System.Web.Mvc {
    using System;
    using System.Globalization;
    using System.Web;

    [AspNetHostingPermission(System.Security.Permissions.SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
    [AspNetHostingPermission(System.Security.Permissions.SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
    public class DateTimeModelBinder : DefaultModelBinder {

        protected internal override object ConvertType(CultureInfo culture, object value, Type destinationType) {
            if (destinationType == typeof(DateTime) || destinationType == typeof(DateTime?)) {
                // we understand only datetimes and strings
                DateTime? valueAsDateTime = value as DateTime?;
                if (valueAsDateTime != null) {
                    return valueAsDateTime;
                }

                string valueAsString = value as string;
                if (valueAsString != null) {
                    return DateTime.Parse(valueAsString, culture ?? CultureInfo.CurrentCulture);
                }
            }

            // don't know how to handle the type of the current value, so just call the base
            return base.ConvertType(culture, value, destinationType);
        }

    }
}
