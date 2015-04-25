using CorrugatedIron.Models;
using RestSharp;
using SitebracoApi.DbEntities;
using SitebracoApi.Models;
using SitebracoApi.Models.Eng;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Http;

namespace SitebracoApi.Controllers.Eng
{
    public class EngTrackController : BaseController
    {
        [HttpPost, HttpGet]
        public object CollectClientInfo(ClientInfoParams param)
        {
            var userLocation = GetUserLocation(HttpContext.Current.Request.UserHostAddress);

            var data = new ClientInfoModel
            {
                ClientId_s = param.clientId,
                IPAddress_s = HttpContext.Current.Request.UserHostAddress,
                PageUrl_tsd = param.pageUrl,
                Browser_s = HttpContext.Current.Request.Browser.Browser,
                BrowserMajorVersion_i = HttpContext.Current.Request.Browser.MajorVersion,
                BrowserMinnorVersion_d = HttpContext.Current.Request.Browser.MinorVersion,
                BrowserVersion_s = HttpContext.Current.Request.Browser.Version,
                Platform_tsd = HttpContext.Current.Request.Browser.Platform,
                UserAgent_tsd = HttpContext.Current.Request.UserAgent,
                OperatingSystem_s = GetOperatingSystem(),
                ScreenResolution_tsd = param.width + "x" + param.height,
                CountryCode_s = userLocation == null ? "Unknown" : userLocation.Country,
                City_s = userLocation == null ? "Unknown" : userLocation.City,
                Latitude_f = userLocation == null ? 0 : userLocation.Latitude,
                Longitude_f = userLocation == null ? 0 : userLocation.Longitude,
                Device_s = GetDevice(),
                DeviceBrand_s = GetDeviceBrand(),
                UrlReferrer_tsd = param.referer,
            };
            return new { success = data.Save() };
        }

        [HttpPost, HttpGet]
        public object CollectClientInfoTest(ClientInfoParams param)
        {
            var userLocation = GetUserLocation(HttpContext.Current.Request.UserHostAddress);

            var data = new ClientInfoModel
            {
                ClientId_s = param.clientId,
                IPAddress_s = HttpContext.Current.Request.UserHostAddress,
                PageUrl_tsd = param.pageUrl,
                Browser_s = GetBrowser(),
                BrowserMajorVersion_i = HttpContext.Current.Request.Browser.MajorVersion,
                BrowserMinnorVersion_d = HttpContext.Current.Request.Browser.MinorVersion,
                BrowserVersion_s = HttpContext.Current.Request.Browser.Version,
                Platform_tsd = HttpContext.Current.Request.Browser.Platform,
                UserAgent_tsd = HttpContext.Current.Request.UserAgent,
                OperatingSystem_s = GetOperatingSystem(),
                ScreenResolution_tsd = param.width + "x" + param.height,
                CountryCode_s = userLocation == null ? "Unknown" : userLocation.Country,
                City_s = userLocation == null ? "Unknown" : userLocation.City,
                Latitude_f = userLocation == null ? 0 : userLocation.Latitude,
                Longitude_f = userLocation == null ? 0 : userLocation.Longitude,
                Device_s = GetDevice(),
                DeviceBrand_s = GetDeviceBrand(),
                UrlReferrer_tsd = param.referer,
            };
            return new
            {
                success = true, data,
                userlocation = userLocation
            };
        }

        [HttpPost, HttpGet]
        public SimpleObjectModel<bool> CollectMouseActionInfo(MouseTrackModel data)
        {
            data.PageUrl_tsd = HttpContext.Current.Request.Url.AbsolutePath;
            var ret = new SimpleObjectModel<bool>(data.Save());
            return ret;
        }

        [HttpPost, HttpGet]
        public object CollectMouseActionInfoTest(MouseTrackModel data)
        {
            data.PageUrl_tsd = HttpContext.Current.Request.Url.AbsolutePath;
            return new { success = true, data };
        }

        [HttpPost, HttpGet]
        public object CollectSetOfMouseActionInfo(IEnumerable<MouseTrackModel> data)
        {
            var mouseTrackModels = data as MouseTrackModel[] ?? data.ToArray();
            if (data == null || !mouseTrackModels.Any())
            {
                //To Track if User is online
                var model = new MouseTrackModel
                {
                    ActionName_s = "Online",
                    IPAddress_s = HttpContext.Current.Request.UserHostAddress,
                    ClientId_s = "Infotrends",
                    PageUrl_tsd = "http://infotrends.com/public/home.html",
                    PageX_i = 0,
                    PageY_i = 0,
                    Point_i = 0
                };
                return new
                {
                    success = model.Save()
                };
            }

            var bucketName = ObjectUtil.GetClassName<MouseTrackModel>();
            var bucketType = ObjectUtil.GetPropertyName<Constant.RiakSolr.BucketType>(x => x.InfoTrendsLog);
            var list = new List<RiakObject>();
            foreach (var item in mouseTrackModels)
            {
                item.IPAddress_s = HttpContext.Current.Request.UserHostAddress;
                item.Position_s = string.Format("{0},{1}", item.PageX_i, item.PageY_i);
                var riakObjId = new RiakObjectId(bucketType, bucketName, item.Id_s);
                var riakObj = new RiakObject(riakObjId, item);
                list.Add(riakObj);
            }

            var client = MyRiak.RiakHelper.CreateClient(ObjectUtil.GetPropertyName<Constant.RiakSolr.ConfigSection>(x => x.riakSolrConfig));
            client.Put(list);

            return new { success = true };
        }

        [HttpPost, HttpGet]
        public object CollectSetOfMouseActionInfoTest(IEnumerable<MouseTrackModel> data)
        {
            var mouseTrackModels = data as MouseTrackModel[] ?? data.ToArray();
            if (data == null || !mouseTrackModels.Any())
                return new { success = true };

            foreach (var item in mouseTrackModels)
            {
                item.PageUrl_tsd = HttpContext.Current.Request.Url.AbsolutePath;
                item.Position_s = string.Format("{0},{1}", item.PageX_i, item.PageY_i);
            }

            return new { success = true, data = mouseTrackModels };
        }

        [HttpPost, HttpGet]
        public object CollectFeedback(FeedbackModel data)
        {
            return new { success = data.Save() };
        }

        [HttpPost, HttpGet]
        public object CollectFeedbackTest(FeedbackModel data)
        {
            return new { success = true, data };
        }

        [HttpPost, HttpGet]
        public object CollectVisitorLogTest(VisitorLogModel data)
        {
            var userLocation = GetUserLocation(HttpContext.Current.Request.UserHostAddress);

            data.IPAddress_s = HttpContext.Current.Request.UserHostAddress;
            data.CountryCode_s = userLocation.Country;
            data.City_s = userLocation.City;
            data.Latitude_f = userLocation.Latitude;
            data.Longitude_f = userLocation.Longitude;

            return new { success = true, data };
        }

        [HttpPost, HttpGet]
        public object CollectVisitorLog(VisitorLogModel data)
        {
            var userLocation = GetUserLocation(HttpContext.Current.Request.UserHostAddress);

            data.IPAddress_s = HttpContext.Current.Request.UserHostAddress;
            data.CountryCode_s = userLocation == null ? "Unknown" : userLocation.Country;
            data.City_s = userLocation == null ? "Unknown" : userLocation.City;
            data.Latitude_f = userLocation == null ? 0 : userLocation.Latitude;
            data.Longitude_f = userLocation == null ? 0 : userLocation.Longitude;

            return new { success = data.Save() };
        }

        [HttpPost, HttpGet]
        public object CollectNotification(NotificationModel data)
        {
            return new { success = data.Save() };
        }

        [HttpPost, HttpGet]
        public object CollectNotificationTest(NotificationModel data)
        {
            return new { success = true, data };
        }

        [HttpPost, HttpGet]
        public object TestIPAddress()
        {
            const string ipAddress = "50.201.58.71";

            return GetUserLocation(ipAddress);
        }

        [HttpPost, HttpGet]
        public object CollectSessionInfo(SessionModel data)
        {
            data.IPAddress_s = HttpContext.Current.Request.UserHostAddress;

            return new
            {
                success = data.Save()
            };
        }

        private static string GetOperatingSystem()
        {
            var operatingSystem = "";
            var userAgent = HttpContext.Current.Request.UserAgent;

            using (var db = new SitebracoEntities())
            {
                var osList = db.ENG_OS.ToList();
                foreach (var os in osList)
                {
                    var aliasList = Regex.Split(os.Alias, @"\,");
                    var check = false;
                    if (aliasList.Any(item => userAgent != null && userAgent.Contains(item)))
                    {
                        operatingSystem = os.Name;
                        check = true;
                    }
                    if (check) break;
                }
            }
            return operatingSystem;
        }

        private static ClientIpInfo GetUserLocation(string ipAddress)
        {
            var restClient = new RestClient("http://engagementdev.infotrends.com:6789");

            var request = new RestRequest(Method.GET) {Resource = "/api/GetClientInfo?IPAddress={IPAddress}"};
            request.AddParameter("IPAddress", ipAddress, RestSharp.ParameterType.UrlSegment);
            
            var response = restClient.Execute<ClientIpInfo>(request);
            
            return response.Data;
        }

        private static string GetDevice()
        {
            if (HttpContext.Current.Request.Browser.IsMobileDevice)
            {
                return (HttpContext.Current.Request.Browser.MobileDeviceManufacturer + " - " + HttpContext.Current.Request.Browser.MobileDeviceModel);
            }
            return "Desktop";
        }

        private static string GetBrowser()
        {
            if (!HttpContext.Current.Request.Browser.IsMobileDevice) return HttpContext.Current.Request.Browser.Browser;
            var deviceModel = HttpContext.Current.Request.Browser.MobileDeviceModel;
            var userAgent = HttpContext.Current.Request.UserAgent;

            if (!deviceModel.ToLower().Equals("ipad") && !deviceModel.ToLower().Equals("ipod") &&
                !deviceModel.ToLower().Equals("iphone")) return HttpContext.Current.Request.Browser.Browser;
            if (userAgent != null && (userAgent.ToLower().Contains("chrome") || userAgent.ToLower().Contains("crios")))
                return "Chrome";
            return HttpContext.Current.Request.Browser.Browser;
        }

        private static string GetDeviceBrand()
        {
            return HttpContext.Current.Request.Browser.IsMobileDevice ? HttpContext.Current.Request.Browser.MobileDeviceManufacturer : "Desktop";
        }
    }

}
