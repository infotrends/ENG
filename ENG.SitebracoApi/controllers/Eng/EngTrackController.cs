using CorrugatedIron.Models;
using RestSharp;
using SitebracoApi.DbEntities;
using SitebracoApi.Models;
using SitebracoApi.Models.Eng;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Xml;

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
                CountryName_s = userLocation == null ? "Unknown" : userLocation.country_name,
                City_s = userLocation == null ? "Unknown" : userLocation.city,
                Latitude_f = userLocation == null ? 0 : userLocation.latitude,
                Longitude_f = userLocation == null ? 0 : userLocation.longitude,
                Device_s = GetDevice(),
                DeviceBrand_s = GetDeviceBrand(),
                UrlReferrer_tsd = param.referer
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
                CountryName_s = userLocation == null ? "Unknown" : userLocation.country_name,
                City_s = userLocation == null ? "Unknown" : userLocation.city,
                Latitude_f = userLocation == null ? 0 : userLocation.latitude,
                Longitude_f = userLocation == null ? 0 : userLocation.longitude,
                Device_s = GetDevice(),
                DeviceBrand_s = GetDeviceBrand(),
                UrlReferrer_tsd = param.referer,
            };
            return new
            {
                success = true,
                data = data,
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
            return new { success = true, data = data };
        }

        [HttpPost, HttpGet]
        public object CollectSetOfMouseActionInfo(IEnumerable<MouseTrackModel> data)
        {
            if (data == null || data.Count() == 0)
            {
                //To Track if User is online
                MouseTrackModel model = new MouseTrackModel();
                model.ActionName_s = "Online";
                model.IPAddress_s = HttpContext.Current.Request.UserHostAddress;
                model.ClientId_s = "Infotrends";
                model.PageUrl_tsd = "http://infotrends.com/public/home.html";
                model.PageX_i = 0;
                model.PageY_i = 0;
                model.Point_i = 0;
                return new
                {
                    success = model.Save()
                };
            }

            var bucketName = ObjectUtil.GetClassName<MouseTrackModel>();
            var bucketType = ObjectUtil.GetPropertyName<Constant.RiakSolr.BucketType>(x => x.InfoTrendsLog);
            var list = new List<RiakObject>();
            foreach (var item in data)
            {
                item.IPAddress_s = HttpContext.Current.Request.UserHostAddress;
                item.Position_s = string.Format("{0},{1}", item.PageX_i, item.PageY_i);
                var riakObjId = new RiakObjectId(bucketType, bucketName, item.Id_s);
                var riakObj = new RiakObject(riakObjId, item);
                list.Add(riakObj);
            }

            var client = MyRiak.RiakHelper.CreateClient(ObjectUtil.GetPropertyName<Constant.RiakSolr.ConfigSection>(x => x.riakSolrConfig));
            var results = client.Put(list);

            return new { success = true };
        }

        [HttpPost, HttpGet]
        public object CollectSetOfMouseActionInfoTest(IEnumerable<MouseTrackModel> data)
        {
            if (data == null || data.Count() == 0)
                return new { success = true };

            foreach (var item in data)
            {
                item.PageUrl_tsd = HttpContext.Current.Request.Url.AbsolutePath;
                item.Position_s = string.Format("{0},{1}", item.PageX_i, item.PageY_i);
            }

            return new { success = true, data = data };
        }

        [HttpPost, HttpGet]
        public object CollectFeedback(FeedbackModel data)
        {
            return new { success = data.Save() };
        }

        [HttpPost, HttpGet]
        public object CollectFeedbackTest(FeedbackModel data)
        {
            return new { success = true, data = data };
        }

        [HttpPost, HttpGet]
        public object CollectVisitorLogTest(VisitorLogModel data)
        {
            var userLocation = GetUserLocation(HttpContext.Current.Request.UserHostAddress);

            data.IPAddress_s = HttpContext.Current.Request.UserHostAddress;
            data.CountryName_s = userLocation.country_name;
            data.City_s = userLocation.city;
            data.Latitude_f = userLocation.latitude;
            data.Longitude_f = userLocation.longitude;

            return new { success = true, data = data };
        }

        [HttpPost, HttpGet]
        public object CollectVisitorLog(VisitorLogModel data)
        {
            var userLocation = GetUserLocation(HttpContext.Current.Request.UserHostAddress);

            data.IPAddress_s = HttpContext.Current.Request.UserHostAddress;
            data.CountryName_s = userLocation == null ? "Unknown" : userLocation.country_name;
            data.City_s = userLocation == null ? "Unknown" : userLocation.city;
            data.Latitude_f = userLocation == null ? 0 : userLocation.latitude;
            data.Longitude_f = userLocation == null ? 0 : userLocation.longitude;

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
            return new { success = true, data = data };
        }

        private string GetOperatingSystem()
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
                    foreach (var item in aliasList)
                    {
                        if (userAgent.Contains(item))
                        {
                            operatingSystem = os.Name;
                            check = true;
                            break;
                        }
                    }
                    if (check) break;
                }
            }
            return operatingSystem;
        }

        private ClientIpInfo GetUserLocation(string ipAddress)
        {
            //var restClient = new RestClient("http://freegeoip.net");
            //var request = new RestRequest(Method.GET);
            //request.Resource = "/json/{Id}";
            //request.AddParameter("Id", ipAddress, RestSharp.ParameterType.UrlSegment);
            //var response = restClient.Execute<ClientIpInfo>(request);
            //return response.Data;
            ClientIpInfo result = new ClientIpInfo();

            try
            {
                var connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["umbracoDbDSN"].ConnectionString;
                var connection = new SqlConnection(connectionString);
                connection.Open();

                var sql = string.Format(@"SELECT * 
	                    FROM ENG_dbiplookup
                        WHERE dbo.ENG_fnBinaryIPv4({0}) BETWEEN ip_start AND ip_end", ipAddress);

                var command = new SqlCommand(sql, connection);
                var reader = command.ExecuteReader();

                while (reader.Read())
                {
                    result.country_code = Convert.ToString(reader["country"]);
                    result.city = Convert.ToString(reader["city"]);
                }
                RegionInfo info = new RegionInfo(result.country_code);
                result.country_name = info.DisplayName;
                //Currently set to default
                result.latitude = 0;
                result.longitude = 0;
                connection.Close();

                return result;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        private string GetDevice()
        {
            if (HttpContext.Current.Request.Browser.IsMobileDevice)
            {
                return (HttpContext.Current.Request.Browser.MobileDeviceManufacturer + " - " + HttpContext.Current.Request.Browser.MobileDeviceModel);
            }
            return "Desktop";
        }

        private string GetBrowser()
        {
            if (HttpContext.Current.Request.Browser.IsMobileDevice)
            {
                var deviceModel = HttpContext.Current.Request.Browser.MobileDeviceModel;
                var userAgent = HttpContext.Current.Request.UserAgent;

                if (deviceModel.ToLower().Equals("ipad")
                    || deviceModel.ToLower().Equals("ipod")
                    || deviceModel.ToLower().Equals("iphone"))
                {
                    if (userAgent.ToLower().Contains("chrome") || userAgent.ToLower().Contains("crios"))
                        return "Chrome";

                }
            }
            return HttpContext.Current.Request.Browser.Browser;
        }

        private string GetDeviceBrand()
        {
            if (HttpContext.Current.Request.Browser.IsMobileDevice)
            {
                return HttpContext.Current.Request.Browser.MobileDeviceManufacturer;
            }
            return "Desktop";
        }
    }

}
