namespace ExtractJsFromAssembly {

    using System;
    using System.Collections.Generic;
    using System.Collections;
    using System.Diagnostics;
    using System.Globalization;
    using System.IO;
    using System.Reflection;
    using System.Resources;
    using System.Security;
    using System.Text;
    using System.Threading;
    using System.Web.UI;

    public class Program {

        private static void EnsureDirectory(string path) {
            if (!Directory.Exists(path)) {
                Directory.CreateDirectory(path);
            }
        }

        static void Main(string[] args) {
            if (args.Length == 0) {
                Console.WriteLine(@"
Utility to extract embedded script resources (libraries) to disk version " +
                    typeof(Program).Assembly.GetName().Version.ToString());
                Console.WriteLine(@"Copyright (c) Microsoft Corporation 2008.");
                Console.WriteLine(@"All rights reserved.");
                Console.WriteLine();
                Console.WriteLine(@"Run 'ExtractJsFromAssembly -?' for a list of valid options.");
                return;
            }
            if ((args[0].Length == 2) &&
                ((args[0][0] == '-') || (args[0][0] == '/')) &&
                ((args[0][1] == '?') || (args[0][1] == 'h'))) {

                Console.WriteLine(@"
Utility to extract embedded script resources (libraries) to disk.
Copyright (C) Microsoft Corporation 2008.
All rights reserved.

Usage:
------
ExtractJsFromAssembly -?
    Prints this help text.

ExtractJsFromAssembly [-v] assemblyPath outputPath
    Extracts all script resources from an assembly.

    -v            If specified, does not include the file version in the
                  output path. Use this option for ASP.NET 2.0 assemblies.
    assemblyPath  The path to the assembly from which to extract embedded
                  script resources from. This includes the assembly name.
    outputPath    The path to the root folder into which the embedded
                  script resources will be extracted.
                  Note that the tool will create a folder structure under
                  this root.

Example:
--------
    extractjsfromassembly customassembly.dll scripts
");
                return;
            }

            string assemblyPath, outputPath;
            bool outputVersion = !((args[0].Length == 2) &&
                ((args[0][0] == '-') || (args[0][0] == '/')) &&
                (args[0][1] == 'v'));

            if ((outputVersion && (args.Length != 2)) ||
                (!outputVersion && (args.Length != 3))) {

                Console.WriteLine("Invalid number of arguments.");
                return;
            }

            if (outputVersion) {
                assemblyPath = args[0];
                outputPath = args[1];
            }
            else {
                assemblyPath = args[1];
                outputPath = args[2];
            }

            try {
                assemblyPath = Path.GetFullPath(assemblyPath);
            }
            catch (ArgumentException) {
                WriteAssemblyPathError();
                return;
            }
            catch (SecurityException) {
                WriteAssemblyPathError();
                return;
            }
            catch (NotSupportedException) {
                WriteAssemblyPathError();
                return;
            }
            catch (PathTooLongException) {
                WriteAssemblyPathError();
                return;
            }

            Assembly assembly;
            try {
                assembly = Assembly.LoadFrom(assemblyPath);
            }
            catch (FileNotFoundException) {
                WriteAssemblyLoadError();
                return;
            }
            catch (FileLoadException) {
                WriteAssemblyLoadError();
                return;
            }
            catch (BadImageFormatException) {
                WriteAssemblyLoadError();
                return;
            }
            catch (SecurityException) {
                WriteAssemblyLoadError();
                return;
            }
            catch (PathTooLongException) {
                WriteAssemblyLoadError();
                return;
            }

            List<CultureInfo> cultures = new List<CultureInfo>();
            cultures.Add(CultureInfo.InvariantCulture);
            foreach (CultureInfo culture in CultureInfo.GetCultures(CultureTypes.AllCultures)) {
                try {
                    assembly.GetSatelliteAssembly(culture);
                    cultures.Add(culture);
                }
                catch (FileLoadException) { }
                catch (FileNotFoundException) { }
            }

            try {
                outputPath = Path.GetFullPath(outputPath);
            }
            catch (ArgumentException) {
                WriteOutputPathError();
                return;
            }
            catch (SecurityException) {
                WriteOutputPathError();
                return;
            }
            catch (NotSupportedException) {
                WriteOutputPathError();
                return;
            }
            catch (PathTooLongException) {
                WriteOutputPathError();
                return;
            }
            EnsureDirectory(outputPath);

            if (assembly != null) {
                // Create the name and version directories
                AssemblyName assemblyName = assembly.GetName();
                outputPath = Path.Combine(outputPath, assemblyName.Name);
                EnsureDirectory(outputPath);
                outputPath = Path.Combine(outputPath, assemblyName.Version.ToString());
                EnsureDirectory(outputPath);

                string fileVersion = null;
                // Get the assembly file version
                AssemblyFileVersionAttribute[] attributes =
                    (AssemblyFileVersionAttribute[])assembly.GetCustomAttributes(typeof(AssemblyFileVersionAttribute), false);
                if (attributes.Length > 0) {
                    fileVersion = attributes[0].Version;
                    if (String.IsNullOrEmpty(fileVersion)) {
                        fileVersion = "0.0.0.0";
                    }
                }
                else {
                    fileVersion = "0.0.0.0";
                }
                if (outputVersion) {
                    outputPath = Path.Combine(outputPath, fileVersion);
                    EnsureDirectory(outputPath);
                }

                // Find the neutral resource language for the assembly
                object[] attrs = assembly.GetCustomAttributes(typeof(NeutralResourcesLanguageAttribute), true);
                CultureInfo assemblyDefaultCulture = CultureInfo.InvariantCulture;
                if (attrs.Length != 0) {
                    assemblyDefaultCulture =
                        new CultureInfo(((NeutralResourcesLanguageAttribute)attrs[0]).CultureName);
                    Console.WriteLine();
                    Console.WriteLine("Default assembly culture: " + assemblyDefaultCulture);
                }
                else {
                    Console.WriteLine();
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("* WARNING * Assembly doesn't define the neutral culture of the resources, which may result in localized script files not being created. Please add a NeutralResourcesLanguageAttribute to the assembly.");
                    Console.ResetColor();
                }

                // Set up the list of resource infos
                Dictionary<string, ScriptResourceInfo> scriptResourceInfos =
                    new Dictionary<string, ScriptResourceInfo>();
                attrs = assembly.GetCustomAttributes(true);
                for (int i = 0; i < attrs.Length; i++) {
                    object attr = attrs[i];
                    Type attrType = attr.GetType();
                    if (attrType.Name.IndexOf("ScriptResourceAttribute") != -1) {
                        PropertyInfo scriptNameProperty = attrType.GetProperty("ScriptName");
                        string scriptName = (string)scriptNameProperty.GetValue(attr, null);
                        PropertyInfo scriptResourceNameProperty = attrType.GetProperty("ScriptResourceName");
                        string scriptResourceName = (string)scriptResourceNameProperty.GetValue(attr, null);
                        PropertyInfo typeNameProperty = attrType.GetProperty("TypeName");
                        string typeName = (string)typeNameProperty.GetValue(attr, null);
                        scriptResourceInfos.Add(scriptName,
                            new ScriptResourceInfo(scriptName, scriptResourceName, typeName));
                    }
                }
                // Also create entries for web resources that are not localized
                attrs = assembly.GetCustomAttributes(typeof(WebResourceAttribute), true);
                for (int i = 0; i < attrs.Length; i++) {
                    WebResourceAttribute wra = (WebResourceAttribute)attrs[i];
                    if ((wra != null) &&
                        (String.Equals(wra.ContentType, "application/x-javascript", StringComparison.OrdinalIgnoreCase) ||
                        String.Equals(wra.ContentType, "application/javascript", StringComparison.OrdinalIgnoreCase) ||
                        String.Equals(wra.ContentType, "text/javascript", StringComparison.OrdinalIgnoreCase)) &&
                        !scriptResourceInfos.ContainsKey(wra.WebResource)) {

                        scriptResourceInfos.Add(wra.WebResource,
                            new ScriptResourceInfo(wra.WebResource, null, null));
                    }
                }

                foreach (ScriptResourceInfo resource in scriptResourceInfos.Values) {

                    string scriptPath = Path.Combine(outputPath, resource.ResourceName);

                    Console.WriteLine();
                    Console.WriteLine(Path.GetFileName(scriptPath));

                    bool debug = resource.ResourceName.EndsWith(".debug.js", StringComparison.OrdinalIgnoreCase);

                    ScriptResourceInfo releaseResource = debug ?
                        scriptResourceInfos[resource.ResourceName.Substring(0, resource.ResourceName.Length - 9) + ".js"] :
                        null;

                    ResourceManager resourceManager = String.IsNullOrEmpty(resource.ScriptResourceName) ?
                        null : new ResourceManager(resource.ScriptResourceName, assembly);
                    ResourceManager releaseResourceManager = (releaseResource != null) &&
                        !String.IsNullOrEmpty(releaseResource.ScriptResourceName) ?
                        new ResourceManager(releaseResource.ScriptResourceName, assembly) :
                        null;

                    if (String.IsNullOrEmpty(resource.ScriptResourceName) &&
                        ((releaseResource == null) || String.IsNullOrEmpty(releaseResource.ScriptResourceName))) {

                        using (Stream scriptStream =
                            assembly.GetManifestResourceStream(resource.ResourceName)) {

                            if (scriptStream == null) {
                                Console.WriteLine("Can't find resource stream " + resource.ResourceName);
                                continue;
                            }

                            using (StreamReader reader = new StreamReader(scriptStream, true)) {
                                using (StreamWriter writer =
                                    new StreamWriter(scriptPath, false, reader.CurrentEncoding)) {

                                    if (debug) {
                                        WriteVersion(writer, resource.ResourceName, assemblyName, fileVersion);
                                    }
                                    string script = reader.ReadToEnd();
                                    writer.WriteLine(script);
                                    WriteLoadedNotification(writer, debug);
                                }
                            }
                        }
                        continue;
                    }

                    try {
                        ResourceSet neutralSet = (resourceManager != null) ?
                            resourceManager.GetResourceSet(CultureInfo.InvariantCulture, true, true) : null;
                        ResourceSet releaseNeutralSet = (releaseResourceManager != null) ?
                            releaseResourceManager.GetResourceSet(CultureInfo.InvariantCulture, true, true) :
                            null;

                        foreach (CultureInfo culture in cultures) {
                            // Skip the assembly's default culture, invariant will work the same.
                            if (String.Equals(culture.Name, assemblyDefaultCulture.Name,
                                StringComparison.OrdinalIgnoreCase)) continue;

                            System.Threading.Thread.CurrentThread.CurrentUICulture = culture;

                            // Try to read the local set to check if the culture was explicitly defined
                            ResourceSet localSet = (resourceManager != null) ?
                                resourceManager.GetResourceSet(culture, true, false) : null;
                            if (localSet == null) {
                                if (releaseResourceManager == null) continue;
                                localSet = releaseResourceManager.GetResourceSet(culture, true, false);
                                // Culture not defined explicitly, try the next one.
                                if (localSet == null) continue;
                            }

                            Console.WriteLine(" - " + culture.DisplayName);

                            string localizedScriptPath =
                                (!String.IsNullOrEmpty(culture.Name)) ?
                                scriptPath.Substring(0, scriptPath.Length - 2) + culture.Name + ".js" :
                                scriptPath;

                            using (Stream scriptStream =
                                assembly.GetManifestResourceStream(resource.ResourceName)) {

                                if (scriptStream == null) {
                                    Console.WriteLine("Can't find resource stream " + resource.ResourceName);
                                    return;
                                }

                                using (StreamReader reader = new StreamReader(scriptStream, true)) {
                                    using (StreamWriter writer =
                                        new StreamWriter(localizedScriptPath, false, reader.CurrentEncoding)) {

                                        if (debug) {
                                            WriteVersion(writer, resource.ResourceName, assemblyName, fileVersion);
                                        }
                                        string script = reader.ReadToEnd();
                                        writer.Write(script);
                                        if (!String.IsNullOrEmpty(resource.ScriptResourceName)) {
                                            writer.WriteLine();
                                            int lastDot = resource.TypeName.LastIndexOf('.');
                                            if (lastDot != -1) {
                                                writer.Write("Type.registerNamespace('");
                                                writer.Write(resource.TypeName.Substring(0, lastDot));
                                                writer.Write("');");
                                                if (debug) {
                                                    writer.WriteLine();
                                                    writer.WriteLine();
                                                }
                                            }
                                            writer.Write(resource.TypeName);
                                            writer.Write("={");
                                            bool first = true;
                                            if (resourceManager != null) {
                                                first = WriteResource(
                                                    resourceManager, neutralSet,
                                                    writer, debug, first);
                                            }
                                            if (releaseResourceManager != null) {
                                                first = WriteResource(
                                                    releaseResourceManager, releaseNeutralSet,
                                                    writer, debug, first);
                                            }
                                            if (debug) writer.WriteLine();
                                            writer.WriteLine("};");
                                        }
                                        WriteLoadedNotification(writer, debug);
                                    }
                                }

                            }
                        }

                        if (releaseNeutralSet != null) {
                            releaseNeutralSet.Dispose();
                        }
                        if (neutralSet != null) {
                            neutralSet.Dispose();
                        }
                    }
                    catch (Exception e) {
                        Console.WriteLine("Exception while reading resource " + resource.ScriptResourceName);
                        Console.WriteLine(e.Message);
                    }
                }
            }
        }

        private static string QuoteString(string value) {
            StringBuilder b = null;

            if (String.IsNullOrEmpty(value)) {
                return String.Empty;
            }

            int startIndex = 0;
            int count = 0;
            for (int i = 0; i < value.Length; i++) {
                Char c = value[i];

                // Append the unhandled characters (that do not require special treament)
                // to the string builder when special characters are detected.
                if (c == '\r' || c == '\t' || c == '\"' || c == '\'' ||
                    c == '\\' || c == '\r' || c == '\b' || c == '\f' || c < ' ') {
                    if (b == null) {
                        b = new StringBuilder(value.Length + 5);
                    }

                    if (count > 0) {
                        b.Append(value, startIndex, count);
                    }

                    startIndex = i + 1;
                    count = 0;
                }

                switch (c) {
                    case '\r':
                        b.Append("\\r");
                        break;
                    case '\t':
                        b.Append("\\t");
                        break;
                    case '\"':
                        b.Append("\\\"");
                        break;
                    case '\'':
                        b.Append("\\\'");
                        break;
                    case '\\':
                        b.Append("\\\\");
                        break;
                    case '\n':
                        b.Append("\\n");
                        break;
                    case '\b':
                        b.Append("\\b");
                        break;
                    case '\f':
                        b.Append("\\f");
                        break;
                    default:
                        if (c < ' ') {
                            b.Append("\\u");
                            b.Append(String.Format(CultureInfo.InvariantCulture, "{0:x4}", (int)c));
                        }
                        else {
                            count++;
                        }
                        break;
                }
            }

            if (b == null) {
                return value;
            }

            if (count > 0) {
                b.Append(value, startIndex, count);
            }

            return b.ToString();
        }

        private static void WriteAssemblyLoadError() {
            Console.WriteLine("Could not load the assembly.");
        }

        private static void WriteAssemblyPathError() {
            Console.WriteLine("Invalid assembly path.");
        }

        private static void WriteLoadedNotification(StreamWriter writer, bool debug) {
            if (debug) writer.WriteLine();
            writer.WriteLine(
                "if(typeof(Sys)!=='undefined')Sys.Application.notifyScriptLoaded();");
        }

        private static void WriteOutputPathError() {
            Console.WriteLine("Invalid output path.");
        }

        private static bool WriteResource(
            ResourceManager resourceManager,
            ResourceSet neutralSet,
            StreamWriter writer,
            bool debug,
            bool first) {

            foreach (DictionaryEntry res in neutralSet) {
                string key = (string)res.Key;
                string value = resourceManager.GetString(key);
                if (value != null) {
                    if (first) {
                        first = false;
                    }
                    else {
                        writer.Write(',');
                    }
                    if (debug) writer.WriteLine();
                    writer.Write('\'');
                    writer.Write(QuoteString(key));
                    writer.Write("':'");
                    writer.Write(QuoteString(value));
                    writer.Write('\'');
                }
            }
            return first;
        }

        private static void WriteVersion(StreamWriter writer, string resourceName, AssemblyName assemblyName, string fileVersion) {
            // Output version information
            writer.WriteLine("// Name:        " + resourceName);
            writer.WriteLine("// Assembly:    " + assemblyName.Name);
            writer.WriteLine("// Version:     " + assemblyName.Version.ToString());
            writer.WriteLine("// FileVersion: " + fileVersion);
        }
    }
}