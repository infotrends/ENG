using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using MyRiak;
using CorrugatedIron;
using CorrugatedIron.Models;
using MyUtils.Validations;
using CorrugatedIron.Models.Search;

namespace SitebracoApi.Controllers.WidgetContent
{

    public class WidgetContentController : BaseController
    {
        [HttpPost]
        public IEnumerable<string> GetAllContent(WidgetContent param)
        {
            return new[] { "Table", "Chair", "Desk", "Computer", "Beer fridge1", param.ContentId, param.WidgetId };
        }

        [HttpGet]
        public IEnumerable<object> GetWidget()
        {
            var widget = new WidgetContent { WidgetId = "12345", ContentId = "12345", Name = "Blog Name", Type = "Blog", Color = "yellow"};
            return new List<WidgetContent> { widget };

            //var cluster = RiakCluster.FromConfig("riakSolrConfig");
            //var client = cluster.CreateClient();

            ////var riakObjId = new RiakObjectId("InfoTrendsType", "Users", Guid.NewGuid().ToString());
            ////var riakObj = new RiakObject(riakObjId, widget);
            ////var result = client.Put(riakObj);
            ////if (result.IsSuccess)
            ////{

            ////}
            ////else
            ////{
            ////    throw new Encoder("Sorry error");
            ////}


            //var widget = new Widget
            //{
            //    ContentId_i = 10
            //};


            //var bucketName = ObjectUtil.GetClassName<Widget>();
            //var fieldName = ObjectUtil.GetPropertyName<Widget, Int64>(x => x.Test_i);


            //var returnList = new List<WidgetContent> { widget};
            //return returnList;
        }


        public class WidgetContent
        {
            public string WidgetId { get; set; }
            public string ContentId { get; set; }
            public string Name { get; set; }
            public string Type { get; set; }
            public string Color { get; set; }
        }
    }
}
