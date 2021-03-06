﻿using System;
using System.Collections.Specialized;
using System.ComponentModel.DataAnnotations;
using System.Web.DynamicData;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace DynamicDataProject {
    public partial class ForeignKeyField : System.Web.DynamicData.FieldTemplateUserControl {
        private bool _allowNavigation = true;
        private string _navigateUrl;
    
        public string NavigateUrl { 
            get {
                return _navigateUrl;
            }
            set {
                _navigateUrl = value;
            }
        }
    
        public bool AllowNavigation {
            get {
                return _allowNavigation;
            }
            set {
                _allowNavigation = value;
            }
        }
    
        protected string GetDisplayString() {
            return FormatFieldValue(ForeignKeyColumn.ParentTable.GetDisplayString(FieldValue));
        }
    
        protected string GetNavigateUrl() {
            if (!AllowNavigation) {
                return null;
            }
            
            if (String.IsNullOrEmpty(NavigateUrl)) {
                return ForeignKeyPath;
            }
            else {
                return BuildForeignKeyPath(NavigateUrl);
            }
        }
    
        public override Control DataControl {
            get {
                return HyperLink1;
            }
        }
    
    }
}
