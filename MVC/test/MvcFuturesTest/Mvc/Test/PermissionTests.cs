namespace Microsoft.Web.Mvc.Test {
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using System.Security;
    using System.Security.Permissions;
    using System.Web;
    using System.Web.Mvc;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Microsoft.Web.Mvc;

    [TestClass]
    public class AspNetHostingPermissionAttributeTest {
        private List<string> _failures;

        [TestInitialize]
        public void Setup() {
            _failures = new List<string>();
        }

        private static bool AccessibleFromPartiallyTrustedClientApplication(Type type) {
            return (type.Namespace.StartsWith("System.Web.ClientServices") ||
                    type.Assembly.GetName().Name == "System.ComponentModel.DataAnnotations");
        }

        private static bool AssemblyIsSecurityTransparent(Assembly assembly) {
            Attribute[] attrs = Attribute.GetCustomAttributes(assembly, typeof(SecurityTransparentAttribute));
            return attrs.Length > 0;
        }

        [TestMethod]
        // Verifies that all types in System.Web.Mvc have the correct AspNetHostingPermission attributes.
        public void AspNetHostingPermissionAttribute() {
            VerifyRuntimeAssembly(typeof(ViewExtensions).Assembly);

            Assert.AreEqual(0, _failures.Count,
                Environment.NewLine + String.Join(Environment.NewLine, _failures.ToArray()));
        }

        private void VerifyRuntimeAssembly(Assembly asm) {
            foreach (Type type in asm.GetTypes()) {
                object[] attrs = type.GetCustomAttributes(typeof(AspNetHostingPermissionAttribute), false);
                VerifyActions(type, attrs);

                if (IsPublic(type)) {
                    if (AccessibleFromPartiallyTrustedClientApplication(type)) {
                        VerifyNoLinkDemand(type, attrs, "accessible from partially-trusted client applications",
                            "Types accessible from partially-trusted client applications");
                        VerifyNoInheritanceDemand(type, attrs, "accessible from partially-trusted client applications",
                            "Types accessible from partially-trusted client applications");
                    }
                    else if (AssemblyIsSecurityTransparent(type.Assembly)) {
                        // We should eventually ensure there are no link or inheritance demands for these assemblies.
                        // However, we do a no-op for now, since I don't want to remove the attributes until we are sure
                        // we are shipping as SecurityTransparent.
                        /*
                        VerifyNoLinkDemand(type, attrs, "in a SecurityTransparent assembly",
                            "Types in SecurityTransparent assemblies");
                        VerifyNoInheritanceDemand(type, attrs, "in a SecurityTransparent assembly",
                            "Types in SecurityTransparent assemblies");
                        */
                    }
                    else if (!type.IsValueType) {
                        VerifyLinkDemand(type, attrs);
                        if (!type.IsSealed) {
                            VerifyInheritanceDemand(type, attrs);
                        }
                        else {
                            VerifyNoInheritanceDemand(type, attrs, "sealed", "Sealed types");
                        }
                    }
                    else {
                        VerifyNoLinkDemand(type, attrs, "a value type", "Value types");
                        VerifyNoInheritanceDemand(type, attrs, "a value type", "Value types");
                    }
                }
                else {
                    VerifyNoLinkDemand(type, attrs, "non-public", "Non-public types");
                    VerifyNoInheritanceDemand(type, attrs, "non-public", "Non-public types");
                }

                VerifyMembers(type);
            }
        }

        private void AssertIsTrue(bool condition, string message, params object[] parameters) {
            if (!condition) {
                _failures.Add(String.Format(message, parameters));
            }
        }

        private void AssertIsFalse(bool condition, string message, params object[] parameters) {
            AssertIsTrue(!condition, message, parameters);
        }

        private static bool HasLinkDemand(Type type, object[] attrs) {
            foreach (AspNetHostingPermissionAttribute attr in attrs) {
                if (attr.Action == SecurityAction.LinkDemand && attr.Level >= AspNetHostingPermissionLevel.Minimal) {
                    return true;
                }
            }
            return false;
        }

        private static bool HasInheritanceDemand(Type type, object[] attrs) {
            foreach (AspNetHostingPermissionAttribute attr in attrs) {
                if (attr.Action == SecurityAction.InheritanceDemand && attr.Level >= AspNetHostingPermissionLevel.Minimal) {
                    return true;
                }
            }
            return false;
        }

        private static bool IsPublic(Type type) {
            // We only consider a nested type public if its enclosing type is also considered public.
            return (type.IsPublic || (type.IsNestedPublic && IsPublic(type.DeclaringType)));
        }

        private void VerifyActions(Type type, object[] attrs) {
            bool hasLinkDemand = false;
            bool hasInheritanceDemand = false;
            foreach (AspNetHostingPermissionAttribute attr in attrs) {
                AssertIsTrue(
                    attr.Action == SecurityAction.LinkDemand || attr.Action == SecurityAction.InheritanceDemand,
                    "Type '{0}' uses the AspNetHostingPermission attribute with SecurityAction.{1}. " +
                    "It should only be used with SecurityAction.LinkDemand or SecurityAction.InheritanceDemand.",
                    type.FullName, attr.Action.ToString());
                if (attr.Action == SecurityAction.LinkDemand) {
                    AssertIsFalse(
                        hasLinkDemand,
                        "Type '{0}' specifies AspNetHostingPermission(SecurityAction.LinkDemand) multiple times. " +
                        "The attribute should be specified at most one time.",
                        type.FullName);
                    hasLinkDemand = true;
                }
                if (attr.Action == SecurityAction.InheritanceDemand) {
                    AssertIsFalse(
                        hasInheritanceDemand,
                        "Type '{0}' specifies AspNetHostingPermission(SecurityAction.InheritanceDemand) multiple times. " +
                        "The attribute should be specified at most one time.",
                        type.FullName);
                    hasInheritanceDemand = true;
                }
            }
        }

        private void VerifyInheritanceDemand(Type type, object[] attrs) {
            AssertIsTrue(HasInheritanceDemand(type, attrs),
                "Type '{0}' is missing AspNetHostingPermission(SecurityAction.InheritanceDemand) with Level >= Minimal. " +
                "All public unsealed reference types in assembly '{1}' must have this attribute.",
                type.FullName, type.Assembly.GetName().Name);
        }

        private void VerifyLinkDemand(Type type, object[] attrs) {
            AssertIsTrue(HasLinkDemand(type, attrs),
                "Type '{0}' is missing AspNetHostingPermission(SecurityAction.LinkDemand) with Level >= Minimal. " +
                "All public reference types in assembly '{1}' must have this attribute.",
                type.FullName, type.Assembly.GetName().Name);
        }

        private void VerifyMembers(Type type) {
            MemberInfo[] members = type.GetMembers(BindingFlags.Public | BindingFlags.NonPublic |
                BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly);
            foreach (MemberInfo member in members) {
                object[] attrs = member.GetCustomAttributes(typeof(AspNetHostingPermissionAttribute), false);
                object[] allCasAttrs = member.GetCustomAttributes(typeof(CodeAccessSecurityAttribute), false);

                // Members should only have the AspNetHostingPermission attribute if they have other CAS attributes
                // as well (FxCop rule CA2114:MethodSecurityShouldBeASupersetOfType).
                AssertIsTrue((attrs.Length == 0) || (attrs.Length < allCasAttrs.Length),
                    "Member '{0}.{1}' has the AspNetHostingPermission attribute.  Members should only have this " +
                    "attribute if they have other CodeAccessSecurityAttributes as well.",
                    member.DeclaringType.FullName, member.Name);
            }
        }

        private void VerifyNoInheritanceDemand(Type type, object[] attrs, string reason1, string reason2) {
            AssertIsFalse(HasInheritanceDemand(type, attrs),
                "Type '{0}' is {1}, but it has AspNetHostingPermission(SecurityAction.InheritanceDemand) with Level >= Minimal. " +
                "{2} should not have this attribute.",
                type.FullName, reason1, reason2);
        }

        private void VerifyNoLinkDemand(Type type, object[] attrs, string reason1, string reason2) {
            AssertIsFalse(HasLinkDemand(type, attrs),
                "Type '{0}' is {1}, but it has AspNetHostingPermission(SecurityAction.LinkDemand) with Level >= Minimal. " +
                "{2} should not have this attribute.",
                type.FullName, reason1, reason2);
        }
    }
}
