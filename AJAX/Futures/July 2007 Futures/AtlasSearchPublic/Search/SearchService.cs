namespace Microsoft.Web.Preview.Search {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Configuration;
    using System.Diagnostics.CodeAnalysis;
    using System.Security.Permissions;
    using System.Web.Configuration;
    using Microsoft.Web.Preview.Configuration;

    [DataObjectAttribute]
    public static class SearchService {
        static List<SearchProviderBase> _searchProviders;

        [
        ConfigurationPermission(SecurityAction.Assert, Unrestricted = true),
        SuppressMessage("Microsoft.Security", "CA2106",
            Justification = "The method instantiate providers from the information in the configuration section but doesn't directly expose information nor takes parameters or allow for modification of the critical information.")
        ]
        internal static List<SearchProviderBase> InitFromConfig() {
            List<SearchProviderBase> searchProviders = new List<SearchProviderBase>();
            SearchSection sectionSection = SearchSection.GetConfigurationSection();

            foreach (ProviderSettings ps in sectionSection.Providers) {
                SearchProviderBase _provider = ProvidersHelper.InstantiateProvider(ps, Type.GetType(ps.Type)) as SearchProviderBase;
                searchProviders.Add(_provider);
            }
            return searchProviders;
        }

        internal static List<SearchProviderBase> GetSearchProviders() {
            if (_searchProviders == null) {
                _searchProviders = InitFromConfig();
            }

            return _searchProviders;
        }

        [DataObjectMethodAttribute(DataObjectMethodType.Select, true)]
        public static IEnumerable Search(String query) {
            SearchQuery searchQuery = new SearchQuery();
            searchQuery.Query = query;
            return Search(searchQuery);
        }

        public static IEnumerable Search(SearchQuery query) {
            List<SearchResult> searchResults = new List<SearchResult>();

            List<SearchProviderBase> searchProviders = GetSearchProviders();

            if ((query == null) || (String.IsNullOrEmpty(query.Query)))
                return searchResults;

            foreach (SearchProviderBase searchProvider in searchProviders) {
                SearchResult[] providerSearchResults = null;
                providerSearchResults = searchProvider.Search(query);

                if ((providerSearchResults != null) && (providerSearchResults.Length > 0))
                    searchResults.AddRange(providerSearchResults);
            }

            return searchResults;
        }
    }
}