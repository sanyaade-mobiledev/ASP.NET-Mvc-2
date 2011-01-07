using System;
using System.Runtime.InteropServices;
using Microsoft.VisualStudio.TextManager.Interop;

namespace JScriptPowerTools.Shared
{
    [ComImport, InterfaceType(ComInterfaceType.InterfaceIsIUnknown), Guid("54755270-B8CE-45AB-8F08-F78677E3B4FD")]
    public interface IHtmServiceManager
    {
        void AddService(ref Guid guidService, [MarshalAs(UnmanagedType.Interface)] object serviceObject);
        void RemoveService(ref Guid guidService);
        void AddServiceProvider(IHtmServiceProvider sp);
        void RemoveServiceProvider(IHtmServiceProvider pSP);
        [return: MarshalAs(UnmanagedType.Interface)]
        object FindService(ref Guid guidService, ref Guid riid);
        [return: MarshalAs(UnmanagedType.Interface)]
        object TryFindService(ref Guid guidService, ref Guid riid);
        void FireServiceEvent(ref Guid guidService, [In, MarshalAs(UnmanagedType.Interface)] object sink);
    }

    [ComImport, InterfaceType(ComInterfaceType.InterfaceIsIUnknown), Guid("A78C7314-C0AF-4704-966D-46B1DC317F82")]
    public interface IHtmServiceProvider
    {
        [return: MarshalAs(UnmanagedType.Interface)]
        object GetService(ref Guid guidService, ref Guid riid);
    }

    [ComImport, InterfaceType(ComInterfaceType.InterfaceIsIUnknown), Guid("C806EA50-96AB-4e32-9476-50882EBFF4AA")]
    public interface ILanguageBlockManager
    {
        void AddBlock(ILanguageBlock block);
        void AddBlockEx([In, MarshalAs(UnmanagedType.LPWStr)] string lang, int startIndex, int endIndex);
        void RemoveBlock(ILanguageBlock block);
        ILanguageBlockCollection GetLanguageBlockCollection();
    }

    [ComImport, Guid("6AE44FB8-BC43-4ffe-B63F-65E010698F43"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface ILanguageBlockCollection
    {
        ILanguageBlock GetAtPos(int pos);
        ILanguageBlock GetAtIndex(int index);
        int GetCount();
    }

    [ComImport, Guid("A9AB0CA9-AFF4-4cdf-AE3F-84564B64B3A3"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface ILanguageBlock
    {
        IHtmContainedLanguage GetContainedLanguage();
        void GetExtent(out int startIndex, out int length, out bool isEndInclusive);
    }

    [ComImport, InterfaceType(ComInterfaceType.InterfaceIsIUnknown), Guid("641187AB-6519-430b-8ABC-2DF9E9E5CD09")]
    public interface IHtmContainedLanguage
    {
        IntPtr QueryContainedInterface(IVsTextBuffer buffer, ref Guid riid);
        Guid GetLanguageServiceGuid();
        uint GetFlags();
    }
}
