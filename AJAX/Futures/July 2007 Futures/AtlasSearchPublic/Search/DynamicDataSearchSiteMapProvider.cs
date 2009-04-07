namespace Microsoft.Web.Preview.Search {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.Configuration.Provider;
    using System.Diagnostics.CodeAnalysis;
    using System.Globalization;
    using System.Reflection;
    using System.Web;
    using System.Xml;

    public class DynamicDataSearchSiteMapProvider : SearchSiteMapProviderBase {
        bool _pathInfoFormat;
        String _targetUrl;
        String _targetUrlseparator = "?";
        String _queryStringDataFormatString;

        List<String> _queryStringDataFields;
        String _queryStringDataFieldSeparator = "&";

        String _lastModifiedDataField = "SiteMapLastModified";
        String _changeFrequencyDataField = "SiteMapChangeFrequency";
        String _priorityDataField = "SiteMapPriority";

        public DynamicDataSearchSiteMapProvider()
            : base() {
            _queryStringDataFields = new List<String>();
        }

        public override void Initialize(string name, NameValueCollection config) {
            // Verify that config isn't null
            if (config == null) {
                throw new ArgumentNullException("config");
            }

            // Assign the provider a default name if it doesn't have one
            if (String.IsNullOrEmpty(name)) {
                name = "DynamicDataSearchSiteMapProvider";
            }

            // Add a default "description" attribute to config if the
            // attribute doesn't exist or is empty
            if (string.IsNullOrEmpty(config["description"])) {
                //config.Remove("description");
                config.Add("description",
                    "DynamicDataSearchSiteMapProvider");
            }

            // Call the base class's Initialize method
            base.Initialize(name, config);

            // Initialize _targetUrl
            _targetUrl = config["targetUrl"];
            if (string.IsNullOrEmpty(_targetUrl)) {
                _targetUrl = "";
            }
            config.Remove("targetUrl");

            // Initialize _targetUrlseparator
            String targetUrlseparatorString = config["targetUrlseparator"];
            if (string.IsNullOrEmpty(targetUrlseparatorString)) {
                _targetUrlseparator = "?";
            }
            else {
                _targetUrlseparator = targetUrlseparatorString;
            }
            config.Remove("targetUrlseparator");

            // Initialize _targetUrlFormatString
            _queryStringDataFormatString = config["queryStringDataFormatString"];
            if (string.IsNullOrEmpty(_queryStringDataFormatString)) {
                _queryStringDataFormatString = "";
            }
            config.Remove("queryStringDataFormatString");

            // Initialize _queryStringDataFields
            String _queryStringDataFieldsString;
            _queryStringDataFieldsString = config["queryStringDataFields"];
            if (!string.IsNullOrEmpty(_queryStringDataFieldsString)) {
                String[] _queryDataFields = _queryStringDataFieldsString.Split(',');
                _queryStringDataFields.AddRange(_queryDataFields);
            }
            config.Remove("queryStringDataFields");

            // Initialize _pathInfoFormat
            String _pathInfoFormatString = "false";
            _pathInfoFormatString = config["pathInfoFormat"];
            if (string.IsNullOrEmpty(_pathInfoFormatString)) {
                _pathInfoFormat = false;
            }
            else {
                _pathInfoFormat = Boolean.Parse(_pathInfoFormatString);
            }
            config.Remove("pathInfoFormat");

            // Initialize _lastmodifiedDataField
            String _lastModifiedDataFieldString = config["lastModifiedDataField"];
            if (!string.IsNullOrEmpty(_lastModifiedDataFieldString)) {
                _lastModifiedDataField = _lastModifiedDataFieldString;
            }
            config.Remove("lastModifiedDataField");

            // Initialize _changeFrequencyDataField
            String _changeFrequencyDataFieldString = config["changeFrequencyDataField"];
            if (!string.IsNullOrEmpty(_changeFrequencyDataFieldString)) {
                _changeFrequencyDataField = _changeFrequencyDataFieldString;
            }
            config.Remove("changeFrequencyDataField");

            // Initialize _priorityDataField
            String _priorityDataFieldString = config["priorityDataField"];
            if (!string.IsNullOrEmpty(_priorityDataFieldString)) {
                _priorityDataField = _priorityDataFieldString;
            }
            config.Remove("priorityDataField");

            // Throw an exception if unrecognized attributes remain
            if (config.Count > 0) {
                string attr = config.GetKey(0);
                if (!String.IsNullOrEmpty(attr))
                    throw new ProviderException
                        ("Unrecognized attribute: " + attr);
            }
        }

        public override void WriteNodes(SearchSiteMapHandler handler, XmlTextWriter writer) {
            IEnumerable results = DataQuery();

            if (results != null) {
                string appUrl = SearchSiteMapProviderBase.GenerateUrl(HttpContext.Current.Request.ApplicationPath + "/");

                //Format is : http://server/site/page.aspx/value1/value2 etc
                if (_pathInfoFormat == true) {
                    _targetUrlseparator = "/";
                    _queryStringDataFieldSeparator = "/";
                }

                foreach (object o in results) {
                    String targetUrlSuffix = "";

                    //Do this once once
                    //Generate query string data fields
                    if ((_queryStringDataFields == null) || (_queryStringDataFields.Count == 0)) {
                        FieldInfo[] fis = o.GetType().GetFields();
                        foreach (FieldInfo fi in fis) {
                            //Skip sitemap specific fields
                            if (String.Compare(fi.Name, _lastModifiedDataField, StringComparison.Ordinal) == 0)
                                continue;

                            if (String.Compare(fi.Name, _changeFrequencyDataField, StringComparison.Ordinal) == 0)
                                continue;

                            if (String.Compare(fi.Name, _priorityDataField, StringComparison.Ordinal) == 0)
                                continue;

                            _queryStringDataFields.Add(fi.Name);
                        }
                    }

                    if ((_queryStringDataFields != null) && (_queryStringDataFields.Count > 0)) {
                        // Is format string present?
                        if (!String.IsNullOrEmpty(_queryStringDataFormatString)) {
                            List<object> _queryStringValues = new List<object>();

                            foreach (String queryStringName in _queryStringDataFields) {
                                _queryStringValues.Add(DataEval(o, queryStringName).ToString());
                            }

                            targetUrlSuffix = String.Format(CultureInfo.InvariantCulture, _queryStringDataFormatString, _queryStringValues.ToArray());
                        }
                        else
                            foreach (String queryStringName in _queryStringDataFields) {
                                String queryStringValue = DataEval(o, queryStringName).ToString();
                                String queryStringNameValue = "";

                                if (!_pathInfoFormat) {
                                    queryStringNameValue = queryStringName + "=" + queryStringValue;
                                }
                                else {
                                    queryStringNameValue = queryStringValue;
                                }

                                if (targetUrlSuffix.Length > 0)
                                    targetUrlSuffix += _queryStringDataFieldSeparator;

                                targetUrlSuffix += queryStringNameValue;
                            }
                    }

                    String url = _targetUrl + _targetUrlseparator + targetUrlSuffix;

                    writer.WriteStartElement("url");
                    writer.WriteElementString("loc", appUrl + url);

                    if (!String.IsNullOrEmpty(_lastModifiedDataField)) {
                        string lastmodifiedValue = DataEval(o, _lastModifiedDataField).ToString();
                        if (!String.IsNullOrEmpty(lastmodifiedValue)) {
                            writer.WriteElementString("lastmod", lastmodifiedValue);
                        }
                    }

                    if (!String.IsNullOrEmpty(_changeFrequencyDataField)) {
                        string changeFrequencyValue = DataEval(o, _changeFrequencyDataField).ToString();
                        if (!String.IsNullOrEmpty(changeFrequencyValue)) {
                            writer.WriteElementString("changefreq", changeFrequencyValue);
                        }
                    }

                    if (!String.IsNullOrEmpty(_priorityDataField)) {
                        string priorityValue = DataEval(o, _priorityDataField).ToString();
                        if (!String.IsNullOrEmpty(priorityValue)) {
                            writer.WriteElementString("priority", priorityValue);
                        }
                    }

                    writer.WriteEndElement(); // url
                }
            }
        }

        protected object DataEval(Object instance, String memberName) {
            if (instance == null) {
                throw new ArgumentNullException("instance");
            }
            FieldInfo fi = instance.GetType().GetField(memberName);
            if (fi != null)
                return fi.GetValue(instance);
            else {
                PropertyInfo pi = instance.GetType().GetProperty(memberName);
                if (pi != null)
                    return pi.GetValue(instance, null);
            }

            return "";
        }
    }
}