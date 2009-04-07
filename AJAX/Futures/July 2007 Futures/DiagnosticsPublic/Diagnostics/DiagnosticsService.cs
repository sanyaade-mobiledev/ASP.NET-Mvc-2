namespace Microsoft.Web.Preview.Diagnostics {
    using System;
    using System.Configuration;
    using System.Diagnostics.CodeAnalysis;
    using System.Security.Permissions;
    using System.Web;
    using System.Web.Script.Services;
    using System.Web.Services;
    using Microsoft.Web.Preview.Configuration;

    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [ScriptService]
    public class DiagnosticsService {
        static DiagnosticsSection _diagnosticsSection;

        [
        ConfigurationPermission(SecurityAction.Assert, Unrestricted = true),
        SuppressMessage("Microsoft.Security", "CA2106",
            Justification = "The method only exposes the presence of the diagnostics section. No information can be passed in.")
        ]
        internal static bool IsEnabled() {
            if (_diagnosticsSection == null) {
                _diagnosticsSection = DiagnosticsSection.GetConfigurationSection();
            }
            return _diagnosticsSection.Enabled;
        }

        public static event EventHandler<ClientExceptionEventArgs> OnClientException;

        [WebMethod]
        [ScriptMethod]
        public void ReportExceptions(ExceptionInfo[] exceptionInfoArray) {
            if (exceptionInfoArray == null)
                return;

            foreach (ExceptionInfo exceptionInfo in exceptionInfoArray) {
                ReportException(exceptionInfo);
            }
        }

        [WebMethod]
        [ScriptMethod]
        public void ReportException(ExceptionInfo exceptionInfo) {
            if (!IsEnabled())
                return;

            if (exceptionInfo == null)
                return;

            ExceptionServerInfo serverInfo = new ExceptionServerInfo();
            serverInfo.FileName = exceptionInfo.FileName;
            serverInfo.LineNumber = exceptionInfo.LineNumber;
            serverInfo.Message = exceptionInfo.Message;
            serverInfo.Data = exceptionInfo.Data;
            serverInfo.IPAddress = HttpContext.Current.Request.UserHostAddress;
            serverInfo.UserAgent = HttpContext.Current.Request.UserAgent;

            //Raise Event
            if (OnClientException != null)
                OnClientException(new object(), new ClientExceptionEventArgs(serverInfo));

        }
    }
}