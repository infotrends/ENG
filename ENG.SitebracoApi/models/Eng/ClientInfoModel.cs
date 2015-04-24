using MyUtils.Validations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SitebracoApi.Models.Eng
{
    public class ClientInfoModel : EngBase
    {
        /// <summary>
        /// Client ID
        /// </summary>
        [Required]
        public string ClientId_s { get; set; }

        /// <summary>
        /// Host Name
        /// </summary>
        public string Host_tsd { get; set; }

        /// <summary>
        ///  Currently Page URL
        /// </summary>
        public string PageUrl_tsd { get; set; }

        /// <summary>
        /// IP Address
        /// </summary>
        [Required]
        public string IPAddress_s { get; set; }

        /// <summary>
        /// Platform Name (WinNT,...)
        /// </summary>
        public string Platform_tsd { get; set; }

        /// <summary>
        /// User Agent String
        /// </summary>
        public string UserAgent_tsd { get; set; }

        /// <summary>
        /// Browser Name
        /// </summary>
        [Required]
        public string Browser_s { get; set; }


        /// <summary>
        /// Operating System Name
        /// </summary>
        [Required]
        public string OperatingSystem_s { get; set; }

        /// <summary>
        /// Screen Resolution
        /// </summary>
        [Required]
        public string ScreenResolution_tsd { get; set; }


        /// <summary>
        /// Currently Browser Version
        /// </summary>
        public string BrowserVersion_s { get; set; }

        public int BrowserMajorVersion_i { get; set; }

        public double BrowserMinnorVersion_d { get; set; }


        /// <summary>
        /// URL of the page which refer to this page (from google.com, ...)
        /// </summary>
        public string UrlReferrer_tsd { get; set; }


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
        /// Lattitude
        /// </summary>
        public float Latitude_f { get; set; }

        /// <summary>
        /// Longitude
        /// </summary>
        public float Longitude_f { get; set; }

        /// <summary>
        /// Device Name
        /// </summary>
        public string Device_s { get; set; }

        /// <summary>
        /// Device Brand Name
        /// </summary>
        public string DeviceBrand_s { get; set; }

    }

    public class ClientIpInfo
    {
        public string Country { get; set; }

        public string StateProvince { get; set; }

        public string City { get; set; }

        public float Latitude { get; set; }

        public float Longitude { get; set; }

        public int Timezone_Offset { get; set; }

        public string Timezone_Name { get; set; }

        public string Isp_Name { get; set; }

        public string Connection_Type { get; set; }

        public string Organization_Name { get; set; }
    }

    public class OSModel
    {
        public string name { get; set; }

        public string alias { get; set; }
    }

    public class ClientInfoParams
    {
        public string clientId { get; set; }

        public int width { get; set; }

        public int height { get; set; }

        public string pageUrl { get; set; }

        public string referer { get; set; }
    }

}
