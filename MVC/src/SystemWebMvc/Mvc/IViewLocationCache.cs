namespace System.Web.Mvc {
    using System.Web;

    // TODO: This interface should be refactored to make it more usable before we make it public.

    /// <summary>
    /// Describes a location cache used by <see cref="VirtualPathProviderViewEngine"/>.
    /// </summary>
    internal interface IViewLocationCache {
        string Get(string cacheKey);
        void Set(string cacheKey, string virtualPath);
    }
}
