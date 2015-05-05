using System;
using CorrugatedIron;
using CorrugatedIron.Comms;
using MyRiak;
using RestSharp;
using System.Web.Http;
using SitebracoApi.Models.Eng;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SitebracoApi.DbEntities;
using SitebracoApi.Models;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Linq.Dynamic;
using umbraco;

namespace SitebracoApi.Controllers.LeadManagement
{
    public class LeadManagementController : BaseController
    {
        [HttpGet]
        public Response<List<ENG_Lead>> GetLeadList(string clientId, DateTime startDate, DateTime endDate, int pageNumber, int itemsPerPage = 10)
        {
            var availabelUrl = GetAvailableUrl();
            var restClient = new RestClient(availabelUrl);

            //Construct Request
            var request = new RestRequest(Method.GET) { Resource = "/search/query/{BucketType}" };
            request.AddParameter("BucketType", ObjectUtil.GetClassName<ClientInfoModel>(), RestSharp.ParameterType.UrlSegment);
            request.AddParameter("wt", "json");
            request.AddParameter("omitHeader", "true");

            request.AddParameter("q", string.Format("ClientId_s:{0} AND CreateOn_dt:[{1} TO {2}]", clientId,
                startDate.ToString("s") + "Z", endDate.ToString("s") + "Z"));

            request.AddParameter("start", (pageNumber - 1) * itemsPerPage);
            request.AddParameter("rows", itemsPerPage);
            request.AddParameter("sort", "CreateOn_dt desc");

            request.AddParameter("group", "true");
            request.AddParameter("group.field", "ViewerID_s");

            var response = restClient.Execute<object>(request);

            var retObj = JsonConvert.DeserializeObject(response.Content);
            var obj = JObject.Parse(retObj.ToJsonString());
            var data = obj["grouped"]["ViewerID_s"]["groups"] as JArray;

            if (data == null)
            {
                return new Response<List<ENG_Lead>>
                {
                    success =  false
                };
            }

            var result = new List<ENG_Lead>();
            
            using (var db = new SitebracoEntities())
            {
                foreach (var item in data.Children<JObject>())
                {
                    var lead = new ENG_Lead();
                    foreach (var prop in item.Properties())
                    {
                        if (prop.Name.Equals("groupValue"))
                        {
                            var viewerId = prop.Value.ToString();
                            lead =
                                db.ENG_Lead.FirstOrDefault(
                                    x => x.ClientID.Equals(clientId) && x.ViewerID.Equals(viewerId)) ?? new ENG_Lead
                                    {
                                        ViewerID = viewerId
                                    };
                        }
                        else
                        {
                            var numFound = (int) JObject.Parse(prop.Value.ToJsonString())["numFound"];

                            if (lead.MemberType == null || lead.MemberType.Equals(""))
                            {
                                lead.MemberType = numFound > 1 ? "Anonymous" : "First Time";
                            }
                            lead.IPAddress =
                                (string)JObject.Parse(prop.Value.ToJsonString())["docs"][0]["IPAddress_s"];
                        }
                    }
                    result.Add(lead);

                }
            }            
                           
            return new Response<List<ENG_Lead>>
            {
                success = true,
                data = result
            };
        }

        [HttpPost]
        public Response<ENG_Lead> SaveLead(ENG_Lead param)
        {
            var model = new ENG_Lead()
            {
                ClientID = param.ClientID,
                IPAddress = param.IPAddress,
                ViewerID = param.ViewerID
            };
            if (param.MemberType != null && !param.MemberType.Equals(""))
            {
                model.MemberType = param.MemberType;
            }

            if (param.Name != null && !param.Name.Equals(""))
            {
                model.Name = param.Name;
            }

            if (param.Note != null && !param.Note.Equals(""))
            {
                model.Note = param.Note;
            }

            if (param.ID != 0) model.ID = param.ID;

            using (var db = new SitebracoEntities())
            {
                db.ENG_Lead.AddOrUpdate(model);
                db.SaveChanges();
            }
            return new Response<ENG_Lead>()
            {
                success = true,
                data = model
            };
        }

        [HttpGet]
        public object GetVisitorLog(string clientId, string viewerId)
        {
            var availabelUrl = GetAvailableUrl();
            var restClient = new RestClient(availabelUrl);

            //Construct Request
            var request = new RestRequest(Method.GET) { Resource = "/search/query/{BucketType}" };
            request.AddParameter("BucketType", ObjectUtil.GetClassName<VisitorLogModel>(), RestSharp.ParameterType.UrlSegment);
            request.AddParameter("wt", "json");
            request.AddParameter("omitHeader", "true");

            request.AddParameter("q", string.Format("ClientId_s:{0} AND ViewerID_s: {1}", clientId, viewerId));
            request.AddParameter("rows", "1000000");

            var response = restClient.Execute<object>(request);

            return new
            {
                success = true,
                data = (response.StatusCode == System.Net.HttpStatusCode.OK) ?
                    JsonConvert.DeserializeObject(response.Content) : null
            };
        }

        private static string GetAvailableUrl()
        {
            var cluster = (RiakCluster)RiakHelper.GetCluster(ObjectUtil.GetPropertyName<Constant.RiakSolr.ConfigSection>(x => x.riakSolrConfig));

            var node = (RiakNode)cluster.SelectNode();
            var nodeUrlList = node.GetRestRootUrl();
            var availabelUrl = nodeUrlList[0];

            return availabelUrl;
        }

    }


}
