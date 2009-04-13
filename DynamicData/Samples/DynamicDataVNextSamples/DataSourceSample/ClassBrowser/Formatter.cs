namespace DataSourcesDemo.ClassBrowser {
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System.Text;
    using System.Reflection;
    using System.Web.Compilation;

    public static class Formatter {
        private static Type[] primitives = { typeof(int),
                                             typeof(ushort),
                                             typeof(ulong),                                             
                                             typeof(short), 
                                             typeof(uint), 
                                             typeof(byte), 
                                             typeof(double), 
                                             typeof(decimal), 
                                             typeof(long), 
                                             typeof(bool), 
                                             typeof(void)
                                           };

        private static char[] separators = { '&', '[', '*' };

        public static string GetImage(Type type) {
            string imageType = "class";
            if (type.IsInterface) {
                imageType = "interface";
            }
            else if (type.IsEnum) {
                imageType = "enum";
            }
            return String.Format(@"<img src=""/images/{0}.png"" align=""bottom"" border=""0"" />", imageType);
        }

        public static string FormatType(Type type) {            
            return FormatType(type, true);
        }

        public static string FormatBaseType(Type type) {
            string baseTypeName = type.BaseType != null ? type.BaseType.Name.ToLower() : String.Empty;            
            if (!String.IsNullOrEmpty(baseTypeName) && baseTypeName != "attribute" && baseTypeName != "object") {
                return " : " + FormatType(type.BaseType);
            }
            return String.Empty;
        }

        public static string FormatAssemblyName(Assembly assembly) {
            string name = assembly.FullName;
            int pos = name.IndexOf(',');
            return CreateAssemblyLink(assembly, name.Substring(0, pos).Trim()) + name.Substring(pos);
        }

        private static string CreateAssemblyLink(Assembly a, string text) {
            return String.Format(@"<a href=""ClassBrowser.aspx?assemblyName={0}"">{1}</a>", a.GetName().Name, text);
        }

        public static string FormatType(Type type, bool applyFormatting) {
            if (type == null) {
                return String.Empty;
            }

            if (type.IsGenericParameter) {
                return Name(type.Name);
            }

            StringBuilder sb = new StringBuilder();
            if (type.IsGenericType) {
                string genericName = type.GetGenericTypeDefinition().Name;
                int index = genericName.IndexOf('`');
                string realTypeName = genericName.Substring(0, index >= 0 ? index : genericName.Length);                
                sb.Append(applyFormatting ? CreateTypeLink(type, NonPrimitive(realTypeName)) : realTypeName);
                sb.Append(Symbol(HttpUtility.HtmlEncode("<")));
                bool first = true;
                foreach (var t in type.GetGenericArguments()) {
                    if (!first) {
                        sb.Append(Symbol(","));
                        sb.Append(" ");
                    }
                    if (t.IsGenericParameter) {
                        sb.Append(Name(t.Name));
                    }
                    else {
                        sb.Append(FormatType(t, applyFormatting));
                    }
                    first = false;
                }
                sb.Append(Symbol(HttpUtility.HtmlEncode(">")));
            }
            else {
                sb.Append(FormatLeafType(type, applyFormatting));
            }
            return sb.ToString();
        }

        private static string CreateTypeLink(Type type, string currentMarkup) {
            return String.Format(@"<a href=""ClassBrowser.aspx?typeName={0}"">{1}</a>",
                                HttpUtility.UrlEncode(type.Name),
                                currentMarkup);
        }

        public static string FormatLeafType(Type type, bool applyFormatting) {
            if (type.IsByRef || type.IsArray || type.IsPointer) {
                string typeName = type.FullName ?? type.Name;
                int sepIndex = typeName.IndexOfAny(separators);
                string realTypeName = typeName.Substring(0, sepIndex);
                Type realType = BuildManager.GetType(realTypeName, false, true);
                return (realType != null ? FormatType(realType, applyFormatting) : realTypeName) + Symbol(typeName.Substring(sepIndex));
            }

            if (applyFormatting) {
                string markup = String.Empty;
                if (primitives.Contains(type)) {
                    markup = Primitive(type.Name);
                }
                else {
                    markup = NonPrimitive(type.Name);
                }
                return CreateTypeLink(type, markup);
            }
            return type.Name;
        }

        public static string FormatProperty(PropertyInfo pi) {
            StringBuilder sb = new StringBuilder();
            sb.Append(Name(pi.Name));
            sb.Append(" ");
            sb.Append(Symbol(":"));
            sb.Append(" ");
            sb.Append(FormatType(pi.PropertyType));
            return sb.ToString();
        }

        public static string FormatField(FieldInfo fi) {
            StringBuilder sb = new StringBuilder();
            sb.Append(Name(fi.Name));
            sb.Append(" ");
            sb.Append(Symbol(":"));
            sb.Append(" ");
            sb.Append(FormatType(fi.FieldType));
            return sb.ToString();
        }

        public static string FormatMethod(MethodInfo mi) {
            StringBuilder sb = new StringBuilder();
            sb.Append(Name(mi.Name));
            sb.Append(Symbol("("));
            var parameters = mi.GetParameters();
            foreach (var p in parameters) {
                if (p.Position > 0) {
                    sb.Append(Symbol(","));
                    sb.Append(" ");
                }
                sb.Append(FormatType(p.ParameterType));
            }
            sb.Append(Symbol(")"));

            sb.Append(" ");
            sb.Append(Symbol(":"));
            sb.Append(" ");
            sb.Append(FormatType(mi.ReturnType));

            return sb.ToString();
        }

        private static string Symbol(string value) {
            return String.Format(@"<span class=""symbol"">{0}</span>", value);
        }

        private static string Name(string value) {
            return String.Format(@"<span class=""name"">{0}</span>", value);
        }

        private static string Primitive(string value) {
            return String.Format(@"<span class=""primitive"">{0}</span>", value);
        }

        private static string NonPrimitive(string value) {
            return String.Format(@"<span class=""nonprimitive"">{0}</span>", value);
        }

    }
}