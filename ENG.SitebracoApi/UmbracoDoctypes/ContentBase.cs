using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace SitebracoApi.UmbracoDoctypes
{
    public class ContentBase : Base
    {
        public double? contentBase_searchBoostValue { get; set; }

        public bool? contentBase_secureContent { get; set; }

        public string contentBase_authors { get; set; }
        public string contentBase_type { get; set; }
        public string contentBase_thumbnailImage { get; set; }
        public string contentBase_keywords { get; set; }
        public string contentBase_company { get; set; }

        public string contentBase_availability { get; set; }
        public string contentBase_priceType { get; set; }

        public string contentBase_guid { get; set; }

        
        [Obsolete("Legacy")]
        public int? contentBase_legacyObjectId { get; set; }

    }
}
