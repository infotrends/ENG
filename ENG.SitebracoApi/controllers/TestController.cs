using System.Collections.Generic;
using System.Web.Http;
using SitebracoApi.Models.Eng;
using CorrugatedIron.Models;
using MyRiak;

namespace SitebracoApi.Controllers
{
    public class TestController : BaseController
    {
        [HttpGet]
        public object PutListToRiak()
        {
            var bucketName = ObjectUtil.GetClassName<MouseTrackModel>();
            var bucketType = ObjectUtil.GetPropertyName<Constant.RiakSolr.BucketType>(x => x.InfoTrendsLog);
            var list = new List<RiakObject>();
            //To Track if User is online
            var item = new MouseTrackModel
            {
                ActionName_s = "ThangPC3 Test",
                IPAddress_s = "ThangPC3 Test",
                ClientId_s = "ThangPC3 Test",
                PageUrl_tsd = "ThangPC3 Test",
                PageX_i = 0,
                PageY_i = 0,
                Point_i = 0
            };

            for (var i = 0; i < 2; i++)
            {
                var riakObjId = new RiakObjectId(bucketType, bucketName, item.Id_s);
                var riakObj = new RiakObject(riakObjId, item);
                list.Add(riakObj);
            }

            var client = RiakHelper.CreateClient(ObjectUtil.GetPropertyName<Constant.RiakSolr.ConfigSection>(x => x.riakSolrConfig));           

            return new
            {
                data = client.Put(list)
            };
        }
    }
}
