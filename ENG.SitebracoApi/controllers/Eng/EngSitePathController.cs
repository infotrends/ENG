using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using SitebracoApi.Models;
using System.Web;
using SitebracoApi.Common;
using RestSharp;
using Newtonsoft.Json;

namespace SitebracoApi.Controllers.Eng
{
    public class EngSitePathController : BaseController
    {
        [HttpPost]
        public Response<SitePathModel> CollectSitePath(SitePathModel param)
        {
            param.IPAddress_s = HttpContext.Current.Request.UserHostAddress;

            return new Response<SitePathModel>
            {
                success = param.Save(),
                data = param
            };
        }

        [HttpGet]
        public object GetSitePath(string clientId, string viewerId)
        {
            var availabelUrl = EngRiakCluster.GetAvailableUrl();
            var restClient = new RestClient(availabelUrl);

            var request = new RestRequest(Method.GET) { Resource = "/search/query/{BucketType}" };
            request.AddParameter("BucketType", ObjectUtil.GetClassName<SitePathModel>(), RestSharp.ParameterType.UrlSegment);
            request.AddParameter("wt", "json");
            request.AddParameter("omitHeader", "true");

            request.AddParameter("q", string.Format("ClientId_s:{0} AND ViewerID_s:{1}", 
                clientId, viewerId));
            request.AddParameter("group", "true");
            request.AddParameter("group.field", "SessionID_s");
            request.AddParameter("group.limit", "1000000");

            var response = restClient.Execute<object>(request);

            return new
            {
                success = true,
                data = (response.StatusCode == System.Net.HttpStatusCode.OK) ?
                    JsonConvert.DeserializeObject(response.Content) : null
            };
        }
    }
}
