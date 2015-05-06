using System.Web.Http;
using RestSharp;
using SitebracoApi.Common;
using SitebracoApi.Models;
using Newtonsoft.Json;

namespace SitebracoApi.Controllers.Eng
{
    public class EngInteractionController : BaseController
    {
        [HttpPost]
        public Response<InteractionModel> CollectInteraction(InteractionModel param)
        {

            return new Response<InteractionModel>
            {
                success = param.Save(),
                data = param
            };
        }

        [HttpGet]
        public object GetInteraction(string clientId, string viewerId)
        {
            var availabelUrl = EngRiakCluster.GetAvailableUrl();
            var restClient = new RestClient(availabelUrl);

            var request = new RestRequest(Method.GET) { Resource = "/search/query/{BucketType}" };
            request.AddParameter("BucketType", ObjectUtil.GetClassName<InteractionModel>(), RestSharp.ParameterType.UrlSegment);
            request.AddParameter("wt", "json");
            request.AddParameter("omitHeader", "true");

            request.AddParameter("q", string.Format("ClientId_s:{0} AND ViewerID_s:{1}",
                clientId, viewerId));

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
