namespace Microsoft.Web.Preview.Search {
    using System.Configuration.Provider;

    public class SearchProviderBase : ProviderBase {
        public SearchProviderBase()
            : base() {
        }

        public virtual SearchResult[] Search(SearchQuery query) {
            return null;
        }
    }
}