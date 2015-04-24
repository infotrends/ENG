using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SitebracoApi.Models.Eng
{
    public class VisitorLogModel : EngBase
    {
        /// <summary>
        /// ClientID
        /// </summary>
        public string ClientId_s { get; set; }

        /// <summary>
        /// Action Type: Click or Enter
        /// </summary>
        public string type_tsd { get; set; }
        /// <summary>
        /// Element 
        /// </summary>
        public string element_tsd { get; set; }
        /// <summary>
        /// Parent Name
        /// </summary>
        public string parent_tsd { get; set; }
        /// <summary>
        /// Element Name
        /// </summary>
        public string elementName_tsd { get; set; }
        /// <summary>
        /// Element HTML
        /// </summary>
        public string elementHtml_tsd { get; set; }
        /// <summary>
        /// IP Address
        /// </summary>
        public string IPAddress_s { get; set; }
        /// <summary>
        /// Country Name
        /// </summary>
        public string CountryName_s { get; set; }
        /// <summary>
        /// Country Code
        /// </summary>
        public string CountryCode_s { get; set; }
        /// <summary>
        /// City Name
        /// </summary>
        public string City_s { get; set; }
        /// <summary>
        /// Latitude
        /// </summary>
        public float Latitude_f { get; set; }
        /// <summary>
        /// Longitude
        /// </summary>
        public float Longitude_f { get; set; }
        /// <summary>
        /// URL of the page which refer to this page (from google.com, ...)
        /// </summary>
        public string UrlReferrer_tsd { get; set; }

    }
}
