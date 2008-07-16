namespace System.Web.Mvc {
    using System;
    using System.Web;

    [AttributeUsage(AttributeTargets.Method, Inherited = true, AllowMultiple = false)]
    [AspNetHostingPermission(System.Security.Permissions.SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
    public sealed class NonActionAttribute : Attribute {
    }
}
