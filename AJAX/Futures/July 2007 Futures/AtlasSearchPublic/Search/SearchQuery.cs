namespace Microsoft.Web.Preview.Search {
    using System;

    public class SearchQuery {
        String _query;

        public String Query {
            get { return _query; }
            set { _query = value; }
        }

        public SearchQuery() {
        }
    }
}