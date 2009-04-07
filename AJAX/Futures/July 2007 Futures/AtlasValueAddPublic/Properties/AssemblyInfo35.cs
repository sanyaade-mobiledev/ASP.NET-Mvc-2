// AssemblyInfo.cs
//

using System;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Resources;
using System.Runtime.InteropServices;
using System.Web.UI;

[assembly: AssemblyTitle("Microsoft.Web.Preview for .NET 3.5")]
[assembly: AssemblyDescription("")]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyCompany("Microsoft Corporation")]
[assembly: AssemblyProduct("Microsoft.Web.Preview")]
[assembly: AssemblyCopyright("Copyright © Microsoft 2007")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]

[assembly: NeutralResourcesLanguage("en", UltimateResourceFallbackLocation.MainAssembly)]

[assembly: AssemblyVersion("2.0.21022.0")]
[assembly: AssemblyFileVersion("2.0.21022.0")]

[assembly: ComVisible(false)]
[assembly: CLSCompliant(true)]

[assembly: WebResource("PreviewScript.js", "application/x-javascript")]
[assembly: WebResource("PreviewScript.debug.js", "application/x-javascript")]

[assembly: WebResource("PreviewGlitz.js", "application/x-javascript")]
[assembly: WebResource("PreviewGlitz.debug.js", "application/x-javascript")]

[assembly: WebResource("PreviewDragDrop.js", "application/x-javascript")]
[assembly: WebResource("PreviewDragDrop.debug.js", "application/x-javascript")]

[assembly: WebResource("PreviewWebParts.js", "application/x-javascript")]
[assembly: WebResource("PreviewWebParts.debug.js", "application/x-javascript")]

[assembly: WebResource("WebParts.js", "application/x-javascript")]
[assembly: WebResource("WebParts.debug.js", "application/x-javascript")]

[assembly: SuppressMessage("Microsoft.Design", "CA1020:AvoidNamespacesWithFewTypes",
    Scope = "namespace", Target = "Microsoft.Web.Preview.UI.Controls.WebParts", Justification="Matches the Preview and WebParts namespaces.")]
[assembly: SuppressMessage("Microsoft.Design", "CA1020:AvoidNamespacesWithFewTypes",
   Scope = "namespace", Target = "Microsoft.Web.Preview.Script.Serialization.Converters", Justification="Matches System.Web.Extensions' namespace for serialization.")]
[assembly: SuppressMessage("Microsoft.Design", "CA1020:AvoidNamespacesWithFewTypes",
   Scope = "namespace", Target = "Microsoft.Web.Preview.UI", Justification="Matches System.Web.Extensions' namespace for UI.")]

