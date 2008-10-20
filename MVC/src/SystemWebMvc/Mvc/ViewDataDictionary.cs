namespace System.Web.Mvc {
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Diagnostics.CodeAnalysis;
    using System.Globalization;
    using System.Reflection;
    using System.Web;
    using System.Web.Mvc.Resources;

    // TODO: Unit test ModelState interaction with VDD

    [SuppressMessage("Microsoft.Usage", "CA2237:MarkISerializableTypesWithSerializable",
        Justification = "This type is not meant to be serialized.")]
    [AspNetHostingPermission(System.Security.Permissions.SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
    [AspNetHostingPermission(System.Security.Permissions.SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
    public class ViewDataDictionary : Dictionary<string, object> {
        private object _model;
        private ViewDataEvaluator _evaluator;

        public ViewDataDictionary()
            : base(StringComparer.OrdinalIgnoreCase) {
            ModelState = new ModelStateDictionary();
            _evaluator = new ViewDataEvaluator(this);
        }

        [SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ViewDataDictionary(object model)
            : base(StringComparer.OrdinalIgnoreCase) {
            Model = model;
            ModelState = new ModelStateDictionary();
            _evaluator = new ViewDataEvaluator(this);
        }

        [SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ViewDataDictionary(ViewDataDictionary viewDataDictionary)
            : base(viewDataDictionary, StringComparer.OrdinalIgnoreCase) {
            ModelState = new ModelStateDictionary(viewDataDictionary.ModelState);
            _evaluator = new ViewDataEvaluator(this);
            Model = viewDataDictionary.Model;
        }

        public object Model {
            get {
                return _model;
            }
            set {
                SetModel(value);
            }
        }

        public ModelStateDictionary ModelState {
            get;
            private set;
        }

        protected virtual void SetModel(object value) {
            _model = value;
        }

        // If a user tries to index into the dictionary using a ViewDataDictionary reference, we don't throw an exception if
        // the key doesn't exist.  If he uses an IDictionary or Dictionary reference, its implementation throws.
        public new object this[string key] {
            get {
                object value;
                TryGetValue(key, out value);
                return value;
            }
            set {
                base[key] = value;
            }
        }

        [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Eval",
            Justification = "Commonly used shorthand for Evaluate.")]
        public object Eval(string expression) {
            if (String.IsNullOrEmpty(expression)) {
                throw new ArgumentException(MvcResources.Common_NullOrEmpty, "expression");
            }

            return _evaluator.Eval(expression);
        }

        [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Eval",
            Justification = "Commonly used shorthand for Evaluate.")]
        public string Eval(string expression, string format) {
            object value = Eval(expression);

            if (value == null) {
                return String.Empty;
            }

            if (String.IsNullOrEmpty(format)) {
                return Convert.ToString(value, CultureInfo.CurrentCulture);
            }
            else {
                return String.Format(CultureInfo.CurrentCulture, format, value);
            }
        }

        internal sealed class ViewDataEvaluator {
            private ViewDataDictionary _dictionary;

            public ViewDataEvaluator(ViewDataDictionary dictionary) {
                _dictionary = dictionary;
            }

            public object Eval(string expression) {
                //Given an expression "foo.bar.baz" we look up the following (pseudocode):
                //  this["foo.bar.baz.quux"]
                //  this["foo.bar.baz"]["quux"]
                //  this["foo.bar"]["baz.quux]
                //  this["foo.bar"]["baz"]["quux"]
                //  this["foo"]["bar.baz.quux"]
                //  this["foo"]["bar.baz"]["quux"]
                //  this["foo"]["bar"]["baz.quux"]
                //  this["foo"]["bar"]["baz"]["quux"]
                object evaluated = EvalIndexerExpression(_dictionary, expression);
                if (evaluated != null)
                    return evaluated;

                if (_dictionary.Model == null)
                    return null;

                return Eval(_dictionary.Model, expression);
            }

            private static object EvalIndexerExpression(object indexableObject, string expression) {
                foreach (var tuple in GetRightToLeftExpressions(expression)) {
                    string subExpression = tuple.Key;
                    string postExpression = tuple.Value;

                    object subtarget = GetPropertyValue(indexableObject, subExpression);
                    if (subtarget != null) {
                        if (String.IsNullOrEmpty(postExpression))
                            return subtarget;

                        object potential = EvalIndexerExpression(subtarget, postExpression);
                        if (potential != null) {
                            return potential;
                        }
                    }
                }
                return null;
            }

            private static IEnumerable<KeyValuePair<string, string>> GetRightToLeftExpressions(string expression) {
                yield return new KeyValuePair<string, string>(expression, string.Empty);

                int lastDot = expression.LastIndexOf('.');

                string subExpression = expression;
                string postExpression = string.Empty;

                while (lastDot > -1) {
                    subExpression = expression.Substring(0, lastDot);
                    postExpression = expression.Substring(lastDot + 1);
                    yield return new KeyValuePair<string, string>(subExpression, postExpression);

                    lastDot = subExpression.LastIndexOf('.');
                }
            }

            private static object GetIndexedPropertyValue(object indexableObject, string key) {
                Type indexableType = indexableObject.GetType();

                ViewDataDictionary vdd = indexableObject as ViewDataDictionary;
                if (vdd != null) {
                    return vdd[key];
                }

                MethodInfo containsKeyMethod = indexableType.GetMethod("ContainsKey", BindingFlags.Public | BindingFlags.Instance, null, new Type[] { typeof(string) }, null);
                if (containsKeyMethod != null) {
                    if (!(bool)containsKeyMethod.Invoke(indexableObject, new object[] { key })) {
                        return null;
                    }
                }

                PropertyInfo info = indexableType.GetProperty("Item", BindingFlags.Public | BindingFlags.Instance, null, null, new Type[] { typeof(string) }, null);
                if (info != null) {
                    return info.GetValue(indexableObject, new object[] { key });
                }

                PropertyInfo objectInfo = indexableType.GetProperty("Item", BindingFlags.Public | BindingFlags.Instance, null, null, new Type[] { typeof(object) }, null);
                if (objectInfo != null) {
                    return objectInfo.GetValue(indexableObject, new object[] { key });
                }
                return null;
            }

            //Replacement for DataBinder.Eval
            private static object Eval(object container, string expression) {
                if (container == null) {
                    return null;
                }

                if (string.IsNullOrEmpty(expression)) {
                    return null;
                }

                string[] expressionParts = expression.Split('.');

                object propertyValue = container;
                foreach (string propertyName in expressionParts) {
                    propertyValue = GetPropertyValue(propertyValue, propertyName);

                    if (propertyValue == null)
                        break;
                }

                return propertyValue;
            }

            private static object GetPropertyValue(object container, string propertyName) {

                object value = GetIndexedPropertyValue(container, propertyName);
                if (value != null)
                    return value;

                PropertyDescriptor descriptor = TypeDescriptor.GetProperties(container).Find(propertyName, true);
                if (descriptor == null) {
                    return null;
                }

                return descriptor.GetValue(container);
            }
        }
    }
}
