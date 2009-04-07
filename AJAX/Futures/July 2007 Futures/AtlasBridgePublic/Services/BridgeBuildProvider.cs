namespace Microsoft.Web.Preview.Services {
    using System;
    using System.CodeDom;
    using System.CodeDom.Compiler;
    using System.Collections;
    using System.Collections.Generic;
    using System.IO;
    using System.Net;
    using System.Reflection;
    using System.Web;
    using System.Web.Compilation;
    using System.Web.Script.Services;
    using System.Web.Services;
    using Microsoft.Web.Util;

    class BridgeBuildProvider : BuildProvider {
        // Store the xml push it into the file for reparsing later
        private string _bridgeXml = String.Empty;

        private BridgeService _service;
        protected BridgeService Service {
            get {
                if (_service == null) {
                    using (Stream stream = OpenStream(VirtualPath)) {
                        using (TextReader reader = new StreamReader(stream)) {
                            _bridgeXml = reader.ReadToEnd();
                        }
                    }

                    if (String.IsNullOrEmpty(_bridgeXml))
                        throw new InvalidOperationException("ASBX file contents are empty");

                    _service = new BridgeService(_bridgeXml);

                    // Update the service cache
                    BridgeService.UpdateCache(VirtualPath, _service);
                }
                return _service;
            }
        }

        public BridgeBuildProvider() {
        }

        public override CompilerType CodeCompilerType {
            get {
                return GetDefaultCompilerTypeForLanguage(Service.Language);
            }
        }

        public override System.Collections.ICollection VirtualPathDependencies {
            get {
                List<string> depedencies = new List<string>(2);
                depedencies.Add(VirtualPath);
                if (!string.IsNullOrEmpty(Service.PartialClassFile))
                    depedencies.Add(Service.PartialClassFile);
                return depedencies;
            }
        }

        /*
         *  Line 12:   namespace Hao.Kung.Samples {
            Line 13:       using System;
            Line 14:       using System.Net;
            Line 15:       using System.Web.Services;
            Line 16:       using System.Collections;
            Line 17:       using System.Xml.Serialization;
            Line 18:       using System.Web.Services;
            Line 19:
            Line 20:
            Line 21:       [WebService(Name="http://tempuri.org/")]
            Line 22:       [WebServiceBinding(ConformsTo=System.Web.Services.WsiProfiles.BasicProfile1_1)]
            Line 23:       public partial class MSNSearch {
            Line 24:
            Line 25:           public MSNSearch() {
            Line 26:               this.VirtualPath = "/atlas/BCL/msn.asbx";
                                   this.BridgeXml = <contents of MSN.asbx>
            Line 27:           }
            Line 28:           [System.Web.Script.Services.ScriptService()]
            Line 29:           [System.Web.Services.WebMethodAttribute()]
            Line 30:           [System.Xml.Serialization.XmlIncludeAttribute(typeof(MSN.SearchRequest))]
            Line 31:           public object Search(System.Collections.IDictionary args) {
            Line 32:               return this.Invoke(new System.Web.Services.BridgeRequest("Search", args));
            Line 33:           }
            Line 34:
         */
        public override void GenerateCode(AssemblyBuilder assemBuilder) {
            CodeNamespace ns = new CodeNamespace(Service.Namespace);
            ns.Imports.Add(new CodeNamespaceImport("System"));
            ns.Imports.Add(new CodeNamespaceImport("System.Net"));
            ns.Imports.Add(new CodeNamespaceImport("System.Web.Services"));
            ns.Imports.Add(new CodeNamespaceImport("System.Collections"));
            ns.Imports.Add(new CodeNamespaceImport("System.Xml.Serialization"));
            ns.Imports.Add(new CodeNamespaceImport("Microsoft.Web.Preview.Services"));
            ns.Imports.Add(new CodeNamespaceImport("System.Web.Script.Services"));

            CodeCompileUnit unit = new CodeCompileUnit();
            unit.Namespaces.Add(ns);

            CodeTypeDeclaration classType = new CodeTypeDeclaration(Service.Classname);
            classType.BaseTypes.Add(typeof(BridgeHandler));
            classType.IsPartial = true;
            ns.Types.Add(classType);
            classType.CustomAttributes.Add(new CodeAttributeDeclaration("ScriptService"));
            classType.CustomAttributes.Add(new CodeAttributeDeclaration("WebService", new CodeAttributeArgument("Name", new CodePrimitiveExpression("http://tempuri.org/"))));
            classType.CustomAttributes.Add(new CodeAttributeDeclaration("WebServiceBinding", new CodeAttributeArgument("ConformsTo", new CodeFieldReferenceExpression(new CodeTypeReferenceExpression(typeof(WsiProfiles)), "BasicProfile1_1"))));

            /**
             * public ClassName() {
             *   VirtualPath = <the virtual path>
             * }
             */
            CodeConstructor constructor = new CodeConstructor();
            constructor.Attributes = MemberAttributes.Public;
            constructor.Statements.Add(new CodeAssignStatement(
                new CodeFieldReferenceExpression(new CodeThisReferenceExpression(), "VirtualPath"),
                new CodePrimitiveExpression(VirtualPath)));
            constructor.Statements.Add(new CodeAssignStatement(
                new CodeFieldReferenceExpression(new CodeThisReferenceExpression(), "BridgeXml"),
                new CodePrimitiveExpression(_bridgeXml)));
            classType.Members.Add(constructor);

            ServiceInfo serviceInfo = Service.ServiceInfo;
            foreach (BridgeMethodInfo methodInfo in serviceInfo.Methods.Values) {
                classType.Members.Add(GenerateWebMethodCode(methodInfo.Name, methodInfo.XmlIncludes, methodInfo.GetEnabled, methodInfo.ResponseFormat, false));
            }

            // Skip the proxy method code generation for REST
            if (!string.Equals(serviceInfo.ServiceClass, "Microsoft.Web.Preview.Services.BridgeRestProxy"))
                classType.Members.Add(GenerateProxyMethodCode());

            // BridgeMethod hook
            classType.Members.Add(GenerateWebMethodCode("__invokeBridge", null, false, ResponseFormat.Json, true));

            assemBuilder.AddCodeCompileUnit(this, unit);
            if (!string.IsNullOrEmpty(Service.PartialClassFile)) {
                using (TextReader reader = OpenReader(Service.PartialClassFile)) {
                    CodeSnippetCompileUnit snippet = new CodeSnippetCompileUnit(reader.ReadToEnd());
                    if (HttpContext.Current != null) {
                        snippet.LinePragma = new CodeLinePragma(HttpContext.Current.Request.MapPath(Service.PartialClassFile), 1);
                    }
                    assemBuilder.AddCodeCompileUnit(this, snippet);
                }
            }
        }

        /**
         * public virtual object CallServiceClassMethod(string method, Dictionary<string, object> args, ICredentials credentials, string serviceUrl) {
         *   if (method.Equals("method")) {
         *        ServiceProxy proxy = new ServiceProxy();
         *        proxy.Url = serviceUrl (if Proxy has a Url property)
         *        proxy.Credentials = credentials (if Proxy has a Credentials property)
         *        object arg = args[paramInfo.ServiceName];
         *        ArgType1 arg1;
         *        if (arg is argType1) arg1 = (ArgType1)arg;
         *        return new Proxy().Method1(arg1, arg2, ...);
         */
        private CodeMemberMethod GenerateProxyMethodCode() {
            CodeMemberMethod method = new CodeMemberMethod();
            method.Attributes = MemberAttributes.Override | MemberAttributes.Public;
            method.Name = "CallServiceClassMethod";
            method.ReturnType = new CodeTypeReference(typeof(object));
            method.Parameters.Add(new CodeParameterDeclarationExpression(typeof(string), "method"));
            method.Parameters.Add(new CodeParameterDeclarationExpression(typeof(Dictionary<String, object>), "args"));
            method.Parameters.Add(new CodeParameterDeclarationExpression(typeof(ICredentials), "credentials"));
            method.Parameters.Add(new CodeParameterDeclarationExpression(typeof(string), "url"));

            // Prevent duplicate methods, as many bridge methods can map to the same server call
            Dictionary<string, NGenWrapper<bool>> methodMap = new Dictionary<string, NGenWrapper<bool>>();
            Type serviceType = BridgeService.GetType(_service.ServiceInfo.ServiceClass);
            foreach (BridgeMethodInfo methodInfo in _service.ServiceInfo.Methods.Values) {
                if (methodMap.ContainsKey(methodInfo.ServerName))
                    continue;

                methodMap[methodInfo.ServerName] = true;

                MethodInfo serviceMethodInfo = serviceType.GetMethod(methodInfo.ServerName);
                if (serviceMethodInfo == null) {
                    throw new ArgumentException("No such method on service proxy class: " + methodInfo.ServerName);
                }
                ParameterInfo[] paramData = serviceMethodInfo.GetParameters();

                // if (method == "Method1"
                CodeConditionStatement ifStmt = new CodeConditionStatement();
                ifStmt.Condition = new CodeMethodInvokeExpression(new CodePrimitiveExpression(methodInfo.ServerName), "Equals", new CodeArgumentReferenceExpression("method"));
                // <ServiceClass> proxy = new <ServiceClass>()
                ifStmt.TrueStatements.Add(new CodeVariableDeclarationStatement(serviceType, "proxy"));
                ifStmt.TrueStatements.Add(new CodeAssignStatement(new CodeVariableReferenceExpression("proxy"), new CodeObjectCreateExpression(serviceType)));

                // check if the proxy class has a Credentials property
                PropertyInfo credProp = serviceType.GetProperty("Credentials");
                if (credProp != null) {
                    // proxy.Credentials = credentials;
                    CodeConditionStatement ifCredNotNull = new CodeConditionStatement();
                    ifCredNotNull.Condition = new CodeBinaryOperatorExpression(new CodeVariableReferenceExpression("credentials"), CodeBinaryOperatorType.IdentityInequality, new CodePrimitiveExpression(null));
                    ifCredNotNull.TrueStatements.Add(new CodeAssignStatement(new CodePropertyReferenceExpression(new CodeVariableReferenceExpression("proxy"), "Credentials"), new CodeVariableReferenceExpression("credentials")));
                    ifStmt.TrueStatements.Add(ifCredNotNull);
                }

                // check if the proxy class has a Url property
                PropertyInfo urlProp = serviceType.GetProperty("Url");
                if (urlProp != null) {
                    // proxy.Credentials = credentials;
                    CodeConditionStatement ifUrlNotEmpty = new CodeConditionStatement();
                    ifUrlNotEmpty.Condition = new CodeMethodInvokeExpression(new CodeMethodReferenceExpression(new CodeTypeReferenceExpression(typeof(string)), "IsNullOrEmpty"), new CodeVariableReferenceExpression("url"));
                    ifUrlNotEmpty.FalseStatements.Add(new CodeAssignStatement(new CodePropertyReferenceExpression(new CodeVariableReferenceExpression("proxy"), "Url"), new CodeVariableReferenceExpression("url")));
                    ifStmt.TrueStatements.Add(ifUrlNotEmpty);
                }

                // object obj holder for args dictionary lookup
                ifStmt.TrueStatements.Add(new CodeVariableDeclarationStatement(typeof(object), "obj"));

                // <method>(ConvertValue(arg1, argType1...), ConvertValue(arg2, argType1...), ...)
                CodeMethodInvokeExpression methodCallExpr = new CodeMethodInvokeExpression();
                methodCallExpr.Method = new CodeMethodReferenceExpression(new CodeVariableReferenceExpression("proxy"), methodInfo.ServerName);
                for (int i = 0; i < paramData.Length; ++i) {
                    Type argType = paramData[i].ParameterType;
                    string argName = "arg" + i;

                    // ArgTypeN argN
                    ifStmt.TrueStatements.Add(new CodeVariableDeclarationStatement(argType, argName));

                    // if (!Dict<name>.TryGet(argName, argn)) throw new ArgumentException()
                    CodeConditionStatement findArgStmt = new CodeConditionStatement();
                    findArgStmt.Condition = new CodeMethodInvokeExpression(new CodeArgumentReferenceExpression("args"),
                        "TryGetValue",
                        new CodePrimitiveExpression(paramData[i].Name),
                        new CodeDirectionExpression(FieldDirection.Out, new CodeVariableReferenceExpression("obj")));
                    findArgStmt.FalseStatements.Add(new CodeThrowExceptionStatement(new CodeObjectCreateExpression(typeof(ArgumentException),
                        new CodePrimitiveExpression("Argument not found: " + paramData[i].Name))));
                    ifStmt.TrueStatements.Add(findArgStmt);

                    // argn = (argtype)ConvertToType(obj, argtype, "paramname");
                    ifStmt.TrueStatements.Add(new CodeAssignStatement(new CodeVariableReferenceExpression(argName),
                        new CodeCastExpression(argType, new CodeMethodInvokeExpression(new CodeSnippetExpression("BridgeHandler"),
                            "ConvertToType",
                            new CodeVariableReferenceExpression("obj"),
                            new CodeTypeOfExpression(argType)))));

                    // Build up the arg1, arg2, argn for the method call;
                    methodCallExpr.Parameters.Add(new CodeVariableReferenceExpression(argName));
                }

                ifStmt.TrueStatements.Add(new CodeMethodReturnStatement(methodCallExpr));

                method.Statements.Add(ifStmt);
            }
            method.Statements.Add(new CodeThrowExceptionStatement(new CodeObjectCreateExpression(typeof(ArgumentException),
                new CodePrimitiveExpression("CallServiceClassMethod: Unknown method"))));
            return method;
        }

        /*
         * [WebMethod]
         * [ScriptMethod(UseHttpGet=true,ResponseFormat=ResponseFormat.Json|Xml)]
         * [XmlInclude(typeof<>) ...
         * public object Method(IDictionary args) {
         *   return Invoke(new BridgeRequest("Method", args);
         * }
         *
         * __invokeBridge takes the method as an argument
         */
        private static CodeMemberMethod GenerateWebMethodCode(String methodName, List<string> xmlIncludes, bool getEnabled, ResponseFormat responseFormat, bool invokeBridge) {
            CodeMemberMethod method = new CodeMemberMethod();
            method.Attributes = MemberAttributes.Public;
            method.Name = methodName;
            method.CustomAttributes.Add(new CodeAttributeDeclaration(new CodeTypeReference(typeof(WebMethodAttribute))));

            string format = "Json";
            if (responseFormat != ResponseFormat.Json) {
                format = "Xml";
            }
            CodeAttributeDeclaration webOp = new CodeAttributeDeclaration(new CodeTypeReference(typeof(ScriptMethodAttribute)),
                new CodeAttributeArgument("UseHttpGet", new CodePrimitiveExpression(getEnabled)),
                new CodeAttributeArgument("ResponseFormat", new CodeFieldReferenceExpression(new CodeTypeReferenceExpression(typeof(ResponseFormat)), format)));
            method.CustomAttributes.Add(webOp);

            if (invokeBridge) {
                method.Parameters.Add(new CodeParameterDeclarationExpression(typeof(string), "method"));
            }
            else {
                for (int i = 0; i < xmlIncludes.Count; ++i) {
                    method.CustomAttributes.Add(new CodeAttributeDeclaration(new CodeTypeReference(typeof(GenerateScriptTypeAttribute)),
                        new CodeAttributeArgument(new CodeTypeOfExpression(xmlIncludes[i]))));
                }
            }

            method.Parameters.Add(new CodeParameterDeclarationExpression(typeof(IDictionary), "args"));

            method.ReturnType = new CodeTypeReference(typeof(object));
            CodeMethodInvokeExpression expr = new CodeMethodInvokeExpression();
            expr.Method = new CodeMethodReferenceExpression(new CodeThisReferenceExpression(), "Invoke");
            CodeExpression methodExpr = null;
            if (invokeBridge) {
                methodExpr = new CodeVariableReferenceExpression("method");
            }
            else {
                methodExpr = new CodePrimitiveExpression(methodName);
            }
            expr.Parameters.Add(new CodeObjectCreateExpression(typeof(BridgeRequest), methodExpr, new CodeVariableReferenceExpression("args")));
            method.Statements.Add(new CodeMethodReturnStatement(expr));
            return method;
        }

        public override Type GetGeneratedType(CompilerResults results) {
            return results.CompiledAssembly.GetType(Service.Namespace + "." + Service.Classname);
        }
    }
}
