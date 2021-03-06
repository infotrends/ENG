﻿using CorrugatedIron;
using CorrugatedIron.Comms;
using CorrugatedIron.Models.Search;
using MyRiak;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using SitebracoApi.Models.Eng;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace SitebracoApi.Controllers.Eng
{
    public class EngQueryController : BaseController
    {
        [HttpPost, HttpGet]
        public object GetPageviewByDate(string clientId, DateTime startDate, DateTime endDate)
        {
            var availabelUrl = GetAvailableUrl();

            var restClient = new RestClient(availabelUrl);
            var request = new RestRequest(Method.GET) {Resource = "/search/query/{BucketType}"};
            request.AddParameter("BucketType", ObjectUtil.GetClassName<ClientInfoModel>(), RestSharp.ParameterType.UrlSegment);
            request.AddParameter("wt", "json");
            request.AddParameter("q", string.Format("ClientId_s:{0}", clientId));
            request.AddParameter("facet", "true");
            request.AddParameter("facet.date", "CreateOn_dt");
            request.AddParameter("facet.date.start", startDate.ToString("s") + "Z");
            request.AddParameter("facet.date.end", endDate.ToString("s") + "Z");
            request.AddParameter("facet.date.gap", "+1DAY");
            request.AddParameter("rows", 0);
            request.AddParameter("omitHeader", "true");

            var response = restClient.Execute<object>(request);

            return new
            {
                success = true,
                data = (response.StatusCode == System.Net.HttpStatusCode.OK) ?
                    JsonConvert.DeserializeObject(response.Content) : null
            };
        }

        [HttpPost, HttpGet]
        public object GetPageviewByDateTest(string clientId, DateTime startDate, DateTime endDate)
        {
            var cluster = (RiakCluster)RiakHelper.GetCluster(ObjectUtil.GetPropertyName<Constant.RiakSolr.ConfigSection>(x => x.riakSolrConfig));
            var node = cluster.SelectNode();

            return new
            {
                success = true,
                ClientId = clientId, node,
                data = new[] { 
                new { Date = "03/24/2015", PageViews = 457, UniqueViews = 158 },
                new { Date = "03/25/2015", PageViews = 557, UniqueViews = 258 },
                new { Date = "03/26/2015", PageViews = 657, UniqueViews = 358 }}
            };
        }

        [HttpPost, HttpGet]
        public object GetPageviewByBrowser(string clientId, DateTime startDate, DateTime endDate)
        {
            var availabelUrl = GetAvailableUrl();
            var restClient = new RestClient(availabelUrl);

            var request = ConstructRequestWithDate
                (ObjectUtil.GetClassName<ClientInfoModel>(), clientId, "Browser_s", startDate, endDate);
            var response = restClient.Execute<object>(request);

            return new
            {
                success = true,
                data = (response.StatusCode == System.Net.HttpStatusCode.OK) ?
                    JsonConvert.DeserializeObject(response.Content) : null
            };
        }

        [HttpPost, HttpGet]
        public object GetPageviewByBrowserTest(string clientId)
        {
            return new
            {
                success = true,
                ClientId = clientId,
                data = new[] { 
                new { Browser = "FireFox", PageViews = 457 },
                new { Browser = "Chrome", PageViews = 657 },
                new { Browser = "IE", PageViews = 851 },
                new { Browser = "Opera", PageViews = 257 },
                new { Browser = "Safari", PageViews = 187 }}
            };
        }

        [HttpPost, HttpGet]
        public object GetPageviewByCountry(string clientId, DateTime startDate, DateTime endDate)
        {
            var availabelUrl = GetAvailableUrl();
            var restClient = new RestClient(availabelUrl);

            var request = ConstructRequestWithDate(
                    ObjectUtil.GetClassName<ClientInfoModel>(), clientId, "CountryCode_s", startDate, endDate);
            var response = restClient.Execute<object>(request);

            return new
            {
                success = true,
                data = (response.StatusCode == System.Net.HttpStatusCode.OK) ?
                    JsonConvert.DeserializeObject(response.Content) : null
            };
        }

        [HttpPost, HttpGet]
        public object GetPageviewByCountryTest(string clientId)
        {
            return new
            {
                success = true,
                ClientId = clientId,
                data = new[] { 
                    new { Country = "The United States", PageViews = 15 },
                    new { Country = "The United Kingdom", PageViews = 10 },
                    new { Country = "India", PageViews = 8 },
                    new { Country = "Republic of Korea", PageViews = 7 },
                    new { Country = "Spain", PageViews = 6 }, 
                    new { Country = "France", PageViews = 6 },
                    new { Country = "France", PageViews = 6 },
                    new { Country = "Belgium", PageViews = 4 },
                    new { Country = "Denmark", PageViews = 4 },
                    new { Country = "Ukraine", PageViews = 4 },
                    new { Country = "Slovenia", PageViews = 3 },
                    new { Country = "Ireland", PageViews = 2 },
                    new { Country = "Finland", PageViews = 1 },
                    new { Country = "Vietnam", PageViews = 1 },
                    new { Country = "Japan", PageViews = 1 },
                }
            };
        }

        [HttpPost, HttpGet]
        public object GetPageviewByCity(string clientId, DateTime startDate, DateTime endDate)
        {
            var availabelUrl = GetAvailableUrl();
            var restClient = new RestClient(availabelUrl);

            var request = ConstructRequestWithDate(
                ObjectUtil.GetClassName<ClientInfoModel>(), clientId, "City_s", startDate, endDate);
            var response = restClient.Execute<object>(request);

            return new
            {
                success = true,
                data = (response.StatusCode == System.Net.HttpStatusCode.OK) ?
                    JsonConvert.DeserializeObject(response.Content) : null
            };
        }

        [HttpPost, HttpGet]
        public object GetPageviewByCityTest(string clientId)
        {
            return new
            {
                success = true,
                ClientId = clientId,
                data = new[] { 
                    new { City = "Alabama", PageViews = 15 },
                    new { City = "Alaska", PageViews = 10 },
                    new { City = "Arizona", PageViews = 8 },
                    new { City = "Arkansas", PageViews = 7 },
                    new { City = "California", PageViews = 6 }, 
                    new { City = "Colorado", PageViews = 6 },
                    new { City = "Connecticut", PageViews = 6 },
                    new { City = "Delaware", PageViews = 4 },
                    new { City = "District of Columbia", PageViews = 4 },
                    new { City = "Georgia", PageViews = 4 },
                    new { City = "Hawaii", PageViews = 3 },
                    new { City = "Illinois", PageViews = 2 },
                    new { City = "Indiana", PageViews = 1 },
                    new { City = "Kansas", PageViews = 1 },
                    new { City = "Iowa", PageViews = 1 },
                }
            };
        }

        [HttpPost, HttpGet]
        public object GetPageviewByOS(string clientId, DateTime startDate, DateTime endDate)
        {
            var availabelUrl = GetAvailableUrl();
            var restClient = new RestClient(availabelUrl);

            var request = ConstructRequestWithDate(
                ObjectUtil.GetClassName<ClientInfoModel>(), clientId, "OperatingSystem_s", startDate, endDate);
            var response = restClient.Execute<object>(request);

            return new
            {
                success = true,
                data = (response.StatusCode == System.Net.HttpStatusCode.OK) ?
                    JsonConvert.DeserializeObject(response.Content) : null
            };
        }

        [HttpPost, HttpGet]
        public object GetPageviewByOSTest(string clientId)
        {
            return new
            {
                success = true,
                ClientId = clientId,
                data = new[] { 
                    new { OperatingSystem = "Windows XP", PageViews = 15 },
                    new { OperatingSystem = "Windows Vista", PageViews = 14 },
                    new { OperatingSystem = "Windows 7", PageViews = 13 },
                    new { OperatingSystem = "Windows 8", PageViews = 10 },
                }
            };
        }

        [HttpPost, HttpGet]
        public object GetPageviewByScreenResolution(string clientId, DateTime startDate, DateTime endDate)
        {
            var availabelUrl = GetAvailableUrl();
            var restClient = new RestClient(availabelUrl);

            var request = ConstructRequestWithDate(
                ObjectUtil.GetClassName<ClientInfoModel>(), clientId, "ScreenResolution_tsd", startDate, endDate);
            var response = restClient.Execute<object>(request);

            return new
            {
                success = true,
                data = (response.StatusCode == System.Net.HttpStatusCode.OK) ?
                    JsonConvert.DeserializeObject(response.Content) : null
            };
        }

        [HttpPost, HttpGet]
        public object GetPageviewByDevice(string clientId, DateTime startDate, DateTime endDate)
        {
            var availabelUrl = GetAvailableUrl();
            var restClient = new RestClient(availabelUrl);

            var request = ConstructRequestWithDate(
                ObjectUtil.GetClassName<ClientInfoModel>(), clientId, "Device_s", startDate, endDate);
            var response = restClient.Execute<object>(request);

            return new
            {
                success = true,
                data = (response.StatusCode == System.Net.HttpStatusCode.OK) ?
                    JsonConvert.DeserializeObject(response.Content) : null
            };
        }

        [HttpPost, HttpGet]
        public object GetPageviewByDeviceBrand(string clientId)
        {
            var availabelUrl = GetAvailableUrl();
            var restClient = new RestClient(availabelUrl);

            var request = ConstructRequest(ObjectUtil.GetClassName<ClientInfoModel>(), clientId, "DeviceBrand_s");
            var response = restClient.Execute<object>(request);

            return new
            {
                success = true,
                data = (response.StatusCode == System.Net.HttpStatusCode.OK) ?
                    JsonConvert.DeserializeObject(response.Content) : null
            };
        }

        [HttpPost, HttpGet]
        public object GetFeedback(string clientId)
        {
            var availabelUrl = GetAvailableUrl();

            var restClient = new RestClient(availabelUrl);
            var request = new RestRequest(Method.GET) {Resource = "/search/query/{BucketType}"};
            request.AddParameter("BucketType", ObjectUtil.GetClassName<FeedbackModel>(), RestSharp.ParameterType.UrlSegment);
            request.AddParameter("wt", "json");
            request.AddParameter("q", "*:*");
            request.AddParameter("sort", "CreateOn_dt desc");
            request.AddParameter("rows", "100000000");
            request.AddParameter("omitHeader", "true");

            var response = restClient.Execute<object>(request);

            return new
            {
                success = true,
                data = (response.StatusCode == System.Net.HttpStatusCode.OK) ?
                    JsonConvert.DeserializeObject(response.Content) : null
            };
        }

        [HttpPost, HttpGet]
        public object GetVisitorLog(string clientId, DateTime startDate, DateTime endDate, int pageNumber, int itemsPerPage = 10)
        {
            var availabelUrl = GetAvailableUrl();

            var restClient = new RestClient(availabelUrl);
            var request = new RestRequest(Method.GET) {Resource = "/search/query/{BucketType}"};
            request.AddParameter("BucketType", ObjectUtil.GetClassName<VisitorLogModel>(), RestSharp.ParameterType.UrlSegment);
            request.AddParameter("wt", "json");
            request.AddParameter("q", string.Format("ClientId_s:{0} AND CreateOn_dt:[{1} TO {2}]", 
                clientId, startDate.ToString("s") + "Z", endDate.ToString("s") + "Z"));
            request.AddParameter("start", (pageNumber - 1) * itemsPerPage);
            request.AddParameter("rows", itemsPerPage);
            request.AddParameter("sort", "CreateOn_dt desc");
            request.AddParameter("omitHeader", "true");

            var response = restClient.Execute<object>(request);

            return new
            {
                success = true,
                data = (response.StatusCode == System.Net.HttpStatusCode.OK) ?
                    JsonConvert.DeserializeObject(response.Content) : null
            };
        }

        [HttpPost, HttpGet]
        public object DeleteUglyData()
        {
            var client = RiakHelper.CreateClient(ObjectUtil.GetPropertyName<Constant.RiakSolr.ConfigSection>(x => x.riakSolrConfig));

            var bucketType = ObjectUtil.GetPropertyName<Constant.RiakSolr.BucketType>(x => x.InfoTrendsLog);

            var id = new List<string>()
            {
                "201504070222314cc35893f3844f62a019c3ea408e2cc5",
                 
            };

            RiakHelper.Delete<ClientInfoModel>(client, bucketType, id);

            return new
            {
                success = true,
            };
        }

        [HttpPost, HttpGet]
        public object GetFeedbackTest(string clientId)
        {
            return new
            {
                success = true,
                ClientId = clientId,
                data = new[] { 
                    new { name = "Angelia", email = "angelia@gmail.com", feedback = "It looks great !" },
                    new { name = "John", email = "john@hotmail.com", feedback = "Should we have a better design ?" },
                    new { name = "Savatage", email = "SavATage@gmail.com", feedback = "Awesome !" },
                    new { name = "Johny Cash", email = "cashj@gmail.com", feedback = "Where can I purchase more widgets !" }
                }
            };
        }

        [HttpPost, HttpGet]
        public object GetMouseTrack(string clientId, string pageUrl, int startX = 0, int startY = 0, int endX = 1920, int endY = 1080)
        {
            var availabelUrl = GetAvailableUrl();
            var restClient = new RestClient(availabelUrl);
            //Revise pageUrl
            pageUrl = ReviseUrl(pageUrl);

            var request = new RestRequest(Method.GET) {Resource = "/search/query/{BucketType}"};
            request.AddParameter("BucketType", ObjectUtil.GetClassName<MouseTrackModel>(), RestSharp.ParameterType.UrlSegment);
            request.AddParameter("wt", "json");
            request.AddParameter("q",
                string.Format("PageX_i:[{0} TO {1}] AND PageY_i:[{2} TO {3}] AND ClientId_s:{4} AND ActionName_s:mousemove AND (Position_s:*25,*25 OR Position_s:*25,*75 OR Position_s:*75,*25 OR Position_s:*75,*75) AND PageUrl_tsd:\"{5}\"", 
                startX, endX, startY, endY, clientId, pageUrl));
            request.AddParameter("facet", "true");
            request.AddParameter("facet.field", "Position_s");
            request.AddParameter("facet.mincount", 1);
            request.AddParameter("facet.limit", -1);

            request.AddParameter("rows", 0);
            request.AddParameter("omitHeader", "true");            
            var response = restClient.Execute<object>(request);

            return new { success = true, data = JsonConvert.DeserializeObject(response.Content) };
        }

        [HttpPost, HttpGet]
        public object GetMouseClick(string clientId, string pageUrl, int startX = 0, int startY = 0, int endX = 1920, int endY = 1080)
        {
            var availabelUrl = GetAvailableUrl();
            var restClient = new RestClient(availabelUrl);
            //Revise PageUrl
            pageUrl = ReviseUrl(pageUrl);

            var request = new RestRequest(Method.GET) {Resource = "/search/query/{BucketType}"};
            request.AddParameter("BucketType", ObjectUtil.GetClassName<MouseTrackModel>(), RestSharp.ParameterType.UrlSegment);
            request.AddParameter("wt", "json");
            request.AddParameter("q", string.Format("PageX_i:[{0} TO {1}] AND PageY_i:[{2} TO {3}] AND ClientId_s:{4} AND ActionName_s:mouseclick AND (Position_s:*25,*25 OR Position_s:*25,*75 OR Position_s:*75,*25 OR Position_s:*75,*75) AND PageUrl_tsd:\"{5}\"", 
                startX, endX, startY, endY, clientId, pageUrl));
            request.AddParameter("facet", "true");
            request.AddParameter("facet.field", "Position_s");
            request.AddParameter("facet.mincount", 1);
            request.AddParameter("facet.limit", -1);
            request.AddParameter("rows", 0);
            request.AddParameter("omitHeader", "true");
            
            var response = restClient.Execute<object>(request);

            return new { success = true, data = JsonConvert.DeserializeObject(response.Content) };
        }

        [HttpPost, HttpGet]
        public object GetUniquePageviewByDate(string clientId, DateTime startDate)
        {
            var availabelUrl = GetAvailableUrl();
            var restClient = new RestClient(availabelUrl);

            var request = new RestRequest(Method.GET) {Resource = "/search/query/{BucketType}"};
            request.AddParameter("BucketType", ObjectUtil.GetClassName<ClientInfoModel>(), RestSharp.ParameterType.UrlSegment);
            request.AddParameter("wt", "json");
            request.AddParameter("q", string.Format("ClientId_s:{0} AND CreateOn_dt: [{1} TO {2}]",
                clientId, startDate.ToString("s") + "Z", startDate.AddDays(1).ToString("s") + "Z"));

            request.AddParameter("facet", "true");
            request.AddParameter("facet.field", "IPAddress_s");
            request.AddParameter("facet.mincount", "1");
            request.AddParameter("rows", 0);
            request.AddParameter("omitHeader", "true");
            //request.AddParameter("facet.mincount", 1);
            var response = restClient.Execute<object>(request);

            return new { success = true, data = JsonConvert.DeserializeObject(response.Content) };
        }

        [HttpPost, HttpGet]
        public object GetUsersOnline(string clientId)
        {
            var availabelUrl = GetAvailableUrl();
            var restClient = new RestClient(availabelUrl);

            var request = new RestRequest(Method.GET) {Resource = "/search/query/{BucketType}"};
            request.AddParameter("BucketType", ObjectUtil.GetClassName<MouseTrackModel>(), RestSharp.ParameterType.UrlSegment);
            request.AddParameter("wt", "json");
            request.AddParameter("omitHeader", "true");
            request.AddParameter("q", string.Format("ClientId_s:{0} AND CreateOn_dt:[{1} TO {2}]",
                clientId, DateTime.UtcNow.AddMinutes(-1).ToString("s") + "Z", DateTime.UtcNow.ToString("s") + "Z"));
            request.AddParameter("rows", "1000000");
            request.AddParameter("group", "true");
            request.AddParameter("group.field", "IPAddress_s");            
            
            var response = restClient.Execute<object>(request);

            return new { success = true, data = JsonConvert.DeserializeObject(response.Content) };
        }

        [HttpPost, HttpGet]
        public object GetNotification(string clientId)
        {
            var availabelUrl = GetAvailableUrl();
            var restClient = new RestClient(availabelUrl);

            var request = new RestRequest(Method.GET) {Resource = "/search/query/{BucketType}"};
            request.AddParameter("BucketType", ObjectUtil.GetClassName<NotificationModel>(), RestSharp.ParameterType.UrlSegment);
            request.AddParameter("wt", "json");
            request.AddParameter("omitHeader", "true");

            request.AddParameter("q", string.Format("ClientId_s:{0}", clientId));
            request.AddParameter("sort", "Seen_b,CreateOn_dt desc");
            request.AddParameter("rows", "1000000");

            var response = restClient.Execute<object>(request);

            return new
            {
                success = true,
                data = (response.StatusCode == System.Net.HttpStatusCode.OK) ?
                    JsonConvert.DeserializeObject(response.Content) : null
            };
        }

        [HttpPost, HttpGet]
        public object GetMostView(string clientId)
        {
            var retObj = GetPageviewByDate(clientId, new DateTime(2015, 01, 01), DateTime.UtcNow);

            var obj = JObject.Parse(retObj.ToJsonString());

            var data = obj["data"]["facet_counts"]["facet_dates"]["CreateOn_dt"] as JObject;
            var maxDate = "";
            var maxViews = 0;

            if (data == null)
                return new
                {
                    success = false
                };
            foreach (var item in data.Properties())
            {
                int view;
                if (!int.TryParse(item.Value.ToString(), out view)) continue;
                if (view <= maxViews) continue;
                maxDate = item.Name;
                maxViews = view;
            }
            return new 
            {
                success = true,
                data = string.Format(@"{0}: {1}", maxDate, maxViews)
            };
        }

        private static RestRequest ConstructRequest(string bucketType, string clientId, string fieldName)
        {
            var request = new RestRequest(Method.GET) {Resource = "/search/query/{BucketType}"};
            request.AddParameter("BucketType", bucketType, RestSharp.ParameterType.UrlSegment);
            request.AddParameter("wt", "json");
            request.AddParameter("q", string.Format("ClientId_s:{0}", clientId));
            request.AddParameter("facet", "true");
            request.AddParameter("facet.field", fieldName);
            request.AddParameter("facet.mincount", "1");
            request.AddParameter("facet.missing", "false");

            request.AddParameter("rows", "0");
            request.AddParameter("omitHeader", "true");

            return request;
        }

        private static RestRequest ConstructRequestWithDate(string bucketType, string clientId, string fieldName, DateTime startDate, DateTime endDate)
        {
            var request = new RestRequest(Method.GET) {Resource = "/search/query/{BucketType}"};
            request.AddParameter("BucketType", bucketType, RestSharp.ParameterType.UrlSegment);
            request.AddParameter("wt", "json");
            request.AddParameter("q", string.Format("ClientId_s:{0} AND CreateOn_dt:[{1} TO {2}]",
                clientId, startDate.ToString("s") + "Z", endDate.ToString("s") + "Z"));

            request.AddParameter("facet", "true");
            request.AddParameter("facet.field", fieldName);
            request.AddParameter("facet.mincount", "1");
            request.AddParameter("facet.missing", "false");

            request.AddParameter("rows", "0");
            request.AddParameter("omitHeader", "true");

            return request;
        }

        private static string GetAvailableUrl()
        {
            var cluster = (RiakCluster)RiakHelper.GetCluster(ObjectUtil.GetPropertyName<Constant.RiakSolr.ConfigSection>(x => x.riakSolrConfig));

            var node = (RiakNode)cluster.SelectNode();
            var nodeUrlList = node.GetRestRootUrl();
            var availabelUrl = nodeUrlList[0];

            return availabelUrl;
        }

        private static string ReviseUrl(string pageUrl)
        {
            var url = pageUrl;

            if (url.ElementAt(url.Length - 1) == '#')
                url = url.Substring(0, url.Length - 1);

            //if (url.ElementAt(url.Length - 1) == '/')
            //    url = url.Substring(0, url.Length - 1);
            return url;
        }
    }
}
