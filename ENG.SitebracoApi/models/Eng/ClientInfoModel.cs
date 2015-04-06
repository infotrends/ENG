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
        [Required]
        public string ClientId_s { get; set; }

        public string Host_tsd { get; set; }

        public string PageUrl_tsd { get; set; }

        [Required]
        public string IPAddress_s { get; set; }

        public string Platform_tsd { get; set; }

        public string UserAgent_tsd { get; set; }

        [Required]
        public string Browser_s { get; set; }

        [Required]
        public string OperatingSystem_s { get; set; }

        [Required]
        public string ScreenResolution_tsd { get; set; }

        public string BrowserVersion_s { get; set; }

        public int BrowserMajorVersion_i { get; set; }

        public double BrowserMinnorVersion_d { get; set; }

        public string UrlReferrer_tsd { get; set; }

        public string CountryName_s { get; set; }

        public string CountryCode_s { get; set; }

        public string City_s { get; set; }

        public float Latitude_f { get; set; }

        public float Longitude_f { get; set; }

        public string Device_s { get; set; }

        public string DeviceBrand_s { get; set; }
    }

    public class ClientIpInfo
    {
        public string ip { get; set; }
        public string country_code { get; set; }
        public string country_name { get; set; }
        public string region_code { get; set; }
        public string region_name { get; set; }
        public string city { get; set; }
        public string zip_code { get; set; }
        public string time_zone { get; set; }
        public float latitude { get; set; }
        public float longitude { get; set; }
        public int metro_code { get; set; }
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
    }

}
