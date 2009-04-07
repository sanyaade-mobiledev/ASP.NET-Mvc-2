namespace Microsoft.Web.Preview.Search {
    using System.Diagnostics.CodeAnalysis;

    public class SearchResult {
        string _title;
        string _description;
        string _url;

        public string Title {
            get { return _title; }
            set { _title = value; }
        }

        public string Description {
            get { return _description; }
            set { _description = value; }
        }

        [SuppressMessage("Microsoft.Design", "CA1056",
            Justification = "Consistent with other URL properties in ASP.NET.")]
        public string Url {
            get { return _url; }
            set { _url = value; }
        }

        public SearchResult() {
        }
    }
}