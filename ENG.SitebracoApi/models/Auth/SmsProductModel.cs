using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SitebracoApi.Models.Auth
{
    public class SmsProductModel : BaseProductModel
    {
        public int ExportLimit { get; set; }
        public Guid? OrderProductId { get; set; }
        public bool HasAccess { get; set; }
        public string CountryLimit { get; set; }
        public List<FeatureModel> Features { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        public SmsProductModel()
        {
            ExportLimit = 1000;
            HasAccess = false;
            Features = new List<FeatureModel>();
        }


    }
}
