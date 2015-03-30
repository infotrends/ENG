using CorrugatedIron.Models.Search;
using MyRiak;
using SitebracoApi.Models.Eng;
using System;
using System.Collections.Generic;
using System.Linq;
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
            var client = RiakHelper.CreateClient(ObjectUtil.GetPropertyName<Constant.RiakSolr.ConfigSection>(x=>x.riakSolrConfig));

            uint totalCount = 0;

            var bucketName = ObjectUtil.GetClassName<ClientInfoModel>();

            List<PageView> result = new List<PageView>();

            //var query = new RiakFluentSearch(bucketName, ObjectUtil.GetPropertyName<ClientInfoModel>(x=>x.CreateOn_dt)).
            //    Between(startDate.ToString(), endDate.ToString()).
            //    And(ObjectUtil.GetPropertyName<ClientInfoModel>(x => x.ClientId_s), clientId).
            //    And(ObjectUtil.GetPropertyName<ClientInfoModel>(x => x.PageUrl_tsd), url).Build();

            for (DateTime i = startDate; i <= endDate; i.AddDays(1))
            {
                var query = new RiakFluentSearch(bucketName, ObjectUtil.GetPropertyName<ClientInfoModel>(x => x.ClientId_s)).
                    Search(clientId).
                    AndBetween(ObjectUtil.GetPropertyName<ClientInfoModel>(x => x.CreateOn_dt), i.ToString("s"), i.AddDays(1).ToString("s")).
                    Build();

                var searchRequest = new RiakSearchRequest
                {
                    Query = query
                };

                var searchResult = RiakHelper.SearchRiak(client, searchRequest, out totalCount);

                var pageViewModel = new PageView
                {
                    Date = i,
                    PageViews = totalCount,
                    UniqueViews = totalCount
                };
                result.Add(pageViewModel);
            }

            
            //var searchRequest = new RiakSearchRequest
            //{
            //    Query = new RiakFluentSearch(bucketName).RawQuery(
            //    string.Format("ClientId_s:{0}&facet=true&facet.date=CreateOn_dt&facet.date.start={1}Z&facet.date.end={2}Z&facet.date.gap=%2B1DAY", 
            //    clientId, startDate.ToString("s"), endDate.ToString("s")))
            //};
          
            return new { success = true, data = result};
        }

        [HttpPost, HttpGet]
        public object GetPageviewByDateTest(string clientId)
        {
            return new
            {
                success = true,
                ClientId = clientId,
                data = new[] { 
                new { Date = "03/24/2015", PageViews = 457, UniqueViews = 158 },
                new { Date = "03/25/2015", PageViews = 557, UniqueViews = 258 },
                new { Date = "03/26/2015", PageViews = 657, UniqueViews = 358 }}
            };
        }

        [HttpPost, HttpGet]
        public object GetPageviewByBrowser(string clientId)
        {
            var client = RiakHelper.CreateClient(ObjectUtil.GetPropertyName<Constant.RiakSolr.ConfigSection>(x => x.riakSolrConfig));

            uint totalCount = 0;

            var bucketName = ObjectUtil.GetClassName<ClientInfoModel>();

            List<PageView> result = new List<PageView>();

            var browsers = new []{ "FireFox", "Chrome", "IE", "Opera", "Safari" };

            foreach (var browser in browsers)
            {
                var query = new RiakFluentSearch(bucketName, ObjectUtil.GetPropertyName<ClientInfoModel>(x => x.ClientId_s)).
                    Search(clientId).
                    And(ObjectUtil.GetPropertyName<ClientInfoModel>(x => x.Browser_tsd), browser).
                    Build();

                var searchRequest = new RiakSearchRequest
                {
                    Query = query
                };

                var searchResult = RiakHelper.SearchRiak(client, searchRequest, out totalCount);

                var pageViewModel = new PageView
                {
                    Browser = browser,
                    PageViews = totalCount
                };
                result.Add(pageViewModel);
            }

            result = result.OrderBy(x => x.PageViews).ToList();

            return new { success = true, data = result };
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
        public object GetPageviewByCountry(string clientId)
        {
            var client = RiakHelper.CreateClient(ObjectUtil.GetPropertyName<Constant.RiakSolr.ConfigSection>(x => x.riakSolrConfig));

            uint totalCount = 0;

            var bucketName = ObjectUtil.GetClassName<ClientInfoModel>();

            List<PageView> result = new List<PageView>();

            //TODO

            return new { success = true, data = result };
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
        public object GetPageviewByOS(string clientId)
        {
            var client = RiakHelper.CreateClient(ObjectUtil.GetPropertyName<Constant.RiakSolr.ConfigSection>(x => x.riakSolrConfig));

            uint totalCount = 0;

            var bucketName = ObjectUtil.GetClassName<ClientInfoModel>();

            List<PageView> result = new List<PageView>();

            var osList = new[] {"Windows 3.11", "Windows 95", "Windows ME", "Windows 98",
                    "Windows CE", "Windows 2000", "Windows XP", "Windows Server 2003", "Windows Vista", "Windows 7",
                    "Windows 8.1", "Windows 8", "Windows NT 4.0", "Windows ME", "Android", "Open BSD",
                    "Sun OS", "Linux", "iOS", "Mac OS X", "Mac OS", "QNX", "UNIX", "BeOS", "OS 2", "Search Bot" };

            foreach (var os in osList)
            {
                var query = new RiakFluentSearch(bucketName, ObjectUtil.GetPropertyName<ClientInfoModel>(x => x.ClientId_s)).
                    Search(clientId).
                    And(ObjectUtil.GetPropertyName<ClientInfoModel>(x => x.OperatingSystem_tsd), os).
                    Build();

                var searchRequest = new RiakSearchRequest
                {
                    Query = query
                };

                var searchResult = RiakHelper.SearchRiak(client, searchRequest, out totalCount);

                var pageViewModel = new PageView
                {
                    OperatingSystem = os,
                    PageViews = totalCount
                };
                result.Add(pageViewModel);
            }

            result = result.OrderBy(x => x.PageViews).ToList();

            return new { success = true, data = result };
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
    }
}
