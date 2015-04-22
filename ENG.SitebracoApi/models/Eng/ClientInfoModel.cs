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

        public string ViewerID_s { get; set; }

        public string SessionID_s { get; set; }
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

        public string ViewerID_s { get; set; }

        public string SessionID_s { get; set; }
    }

}
