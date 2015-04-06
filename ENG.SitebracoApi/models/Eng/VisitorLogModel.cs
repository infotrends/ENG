using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SitebracoApi.Models.Eng
{
    public class VisitorLogModel : EngBase
    {
        public string ClientId_s { get; set; }

        public string type_tsd { get; set; }

        public string element_tsd { get; set; }

        public string parent_tsd { get; set; }

        public string elementName_tsd { get; set; }

        public string elementHtml_tsd { get; set; }

        public string IPAddress_s { get; set; }

        public string CountryName_s { get; set; }

        public string CountryCode_s { get; set; }

        public string City_s { get; set; }

        public float Latitude_f { get; set; }

        public float Longitude_f { get; set; }

        public string UrlReferrer_tsd { get; set; }

    }
}
