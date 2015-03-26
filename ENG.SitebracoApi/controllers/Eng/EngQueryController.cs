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
            var client = RiakHelper.CreateClient(Constant.EngRiakConfig.SOLR_CONFIG);

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
            return new { success = true, data = new { ClientId = clientId, TotalView = 216, UniqueView = 158 } };
        }
    }
}
