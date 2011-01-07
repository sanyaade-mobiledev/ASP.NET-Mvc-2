using System;
using System.Reflection;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.TextManager.Interop;
using Microsoft.VisualStudio.Text.Classification;

namespace JScriptPowerTools.Shared
{
    public static class VsServiceManager
    {
        static Assembly _html = Assembly.Load("Microsoft.VisualStudio.Web.HTML");
        static Type _IHtmServiceMangerType = _html.GetType("Microsoft.VisualStudio.Web.HTML.IHtmServiceManager");
        static Guid _IScriptColorizerGuid = new Guid("AE53377E-D59A-4021-9C5C-071FB291771C");

        public static ILanguageBlockManager GetLanguageBlockManager(ITextBuffer buffer)
        {
            Guid lbmGuid = typeof(ILanguageBlockManager).GUID;
            return GetService(buffer, lbmGuid) as ILanguageBlockManager;
        }

        public static IClassifier GetScriptColorizer(ITextBuffer buffer)
        {
            return GetService(buffer, _IScriptColorizerGuid) as IClassifier;
        }

        public static object GetService(ITextBuffer buffer, Guid serviceGuid)
        {
            IHtmServiceManager serviceManager = null;
            IVsTextBuffer vsTextBuffer;
            buffer.Properties.TryGetProperty<IVsTextBuffer>(typeof(IVsTextBuffer), out vsTextBuffer);

            if (vsTextBuffer != null)
            {
                var guid = typeof(IHtmServiceManager).GUID;
                var userData = vsTextBuffer as IVsUserData;

                object serviceManagerObject;
                int hr = userData.GetData(ref guid, out serviceManagerObject);

                if (VSConstants.S_OK == hr && serviceManagerObject != null)
                {
                    serviceManager = serviceManagerObject as IHtmServiceManager;
                }
            }
            else
            {
                buffer.Properties.TryGetProperty(_IHtmServiceMangerType, out serviceManager);
            }

            return serviceManager.TryFindService(ref serviceGuid, ref serviceGuid);
        }
    }
}