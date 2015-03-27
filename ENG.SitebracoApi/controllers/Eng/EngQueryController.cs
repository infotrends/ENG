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
        public object GetPageviewByDate(string clientId)
        {
            var client = RiakHelper.CreateClient(ObjectUtil.GetPropertyName<Constant.RiakSolr.ConfigSection>(x=>x.riakSolrConfig));

            uint totalCount = 0;

            var buketName = ObjectUtil.GetClassName<ClientInfoModel>();

            //var query = new RiakFluentSearch(buketName, ObjectUtil.GetPropertyName<ClientInfoModel>(x=>x.CreateOn_dt)).
            //    Between(startDate.ToString(), endDate.ToString()).
            //    And(ObjectUtil.GetPropertyName<ClientInfoModel>(x => x.ClientId_s), clientId).
            //    And(ObjectUtil.GetPropertyName<ClientInfoModel>(x => x.PageUrl_tsd), url).Build();

            var query = new RiakFluentSearch(buketName, ObjectUtil.GetPropertyName<ClientInfoModel>(x => x.ClientId_s)).Search(clientId).Build();
            
            var searchRequest = new RiakSearchRequest
            {
                Query = query
            };
            

            var searchResult = RiakHelper.SearchRiak(client, searchRequest, out totalCount);


            return new { success = true, data = new { ClientId = clientId, TotalView = totalCount, UniqueView = 15 } };
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
        public object GetFeedback(string clientId)
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
