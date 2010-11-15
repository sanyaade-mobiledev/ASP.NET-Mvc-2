using System;
using System.Linq;
using System.Collections.Generic;
using System.Web.Mvc;

namespace VsDocBuilder.Models
{
    public class BuildModel
    {
        public IEnumerable<SelectListItem> Versions { get; set; }
        public string Version { get; set; }
        public bool GenerateParaTags { get; set; }
    }
}