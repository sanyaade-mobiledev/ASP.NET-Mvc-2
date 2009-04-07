namespace Microsoft.Web.Preview.Search {
    using System;
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.Configuration.Provider;
    using System.Globalization;
    using Microsoft.Web.Preview.com.msn.search.soap;
    using Microsoft.Web.Preview.Resources;

    public class WindowsLiveSearchProvider : SearchProviderBase {
        String _appID = "";
        String _siteDomainName = "";

        public WindowsLiveSearchProvider()
            : base() {
        }

        public String AppId {
            get { return _appID; }
            set { _appID = value; }
        }

        public String SiteDomainName {
            get { return _siteDomainName; }
            set { _siteDomainName = value; }
        }

        public override SearchResult[] Search(SearchQuery query) {
            List<SearchResult> searchResults = new List<SearchResult>();

            int startOffset = 0;
            MSNSearchService msnSearchService = new MSNSearchService();

            SearchRequest searchRequest = new SearchRequest();
            int arraySize = 1;
            SourceRequest[] sr = new SourceRequest[arraySize];
            sr[0] = new SourceRequest();
            sr[0].Source = SourceType.Web;
            sr[0].Offset = startOffset;

            String searchPrefix = String.IsNullOrEmpty(_siteDomainName) ? "" : "site:" + _siteDomainName + " ";

            searchRequest.Query = searchPrefix + query.Query;
            searchRequest.Requests = sr;
            searchRequest.AppID = _appID;
            searchRequest.CultureInfo = System.Threading.Thread.CurrentThread.CurrentUICulture.Name;

            SearchResponse searchResponse = msnSearchService.Search(searchRequest);

            foreach (SourceResponse sourceResponse in searchResponse.Responses) {
                Result[] sourceResults = sourceResponse.Results;
                foreach (Result sourceResult in sourceResults) {
                    SearchResult searchResult = new SearchResult();

                    if (!String.IsNullOrEmpty(sourceResult.Title))
                        searchResult.Title = sourceResult.Title;

                    if (!String.IsNullOrEmpty(sourceResult.Description))
                        searchResult.Description = sourceResult.Description;

                    if (!String.IsNullOrEmpty(sourceResult.Url))
                        searchResult.Url = sourceResult.Url;

                    searchResults.Add(searchResult);
                }
            }

            return searchResults.ToArray();
        }


        public override void Initialize(string name, NameValueCollection config) {
            // Verify that config isn't null
            if (config == null) {
                throw new ArgumentNullException("config");
            }

            // Assign the provider a default name if it doesn't have one
            if (String.IsNullOrEmpty(name)) {
                //name = "WindowsLiveSearchProvider";
                name = PreviewWeb.Search_WindowsLiveSearchProvider_Name;
            }

            // Add a default "description" attribute to config if the
            // attribute doesn't exist or is empty
            if (string.IsNullOrEmpty(config["description"])) {
                //config.Remove("description");
                config.Add("description",
                    PreviewWeb.Search_WindowsLiveSearchProvider_Description);
            }

            // Call the base class's Initialize method
            base.Initialize(name, config);

            // Initialize appID
            _appID = config["appID"];
            if (string.IsNullOrEmpty(_appID))
                _appID = "";
            config.Remove("appID");

            // Initialize siteDomainName
            _siteDomainName = config["siteDomainName"];
            if (string.IsNullOrEmpty(_siteDomainName))
                _siteDomainName = "";
            config.Remove("siteDomainName");

            // Throw an exception if unrecognized attributes remain
            if (config.Count > 0) {
                string attr = config.GetKey(0);
                if (!String.IsNullOrEmpty(attr))
                    throw new ProviderException
                        (String.Format(CultureInfo.InvariantCulture,
                            PreviewWeb.WindowsLiveSearchProvider_UnrecognizedAttribute,
                            attr));
            }
        }
    }
}