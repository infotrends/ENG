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
using Newtonsoft.Json;

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
            var data = new List<WidgetData>
            {
                new WidgetData{
                     Title =  "Aliquam erat volutpat", 
                     Content ="Aliquam dapibus tincidunt metus. Praesent justo dolor, lobortis quis, lobortis dignissim, pulvinar ac, lorem. Lorem ipsum dolor sit amet, consectetuer adipiscing elit. Praesent vestibulum molestie lacus. Aenean nonummy hendrerit mauris. Phasellus porta. Fusce suscipit varius mi. Cum sociis natoque penatibus et magnis dis parturient montes, nascetur ridiculus mus. Nulla dui"
                },
                 new WidgetData{
                  Title = "Mauris posuere", 
                  Content =  "Aliquam dapibus tincidunt metus. Praesent justo dolor, lobortis quis, lobortis dignissim, pulvinar ac, lorem. Lorem ipsum dolor sit amet, consectetuer adipiscing elit. Praesent vestibulum molestie lacus. Aenean nonummy hendrerit mauris. Phasellus porta. Fusce suscipit varius mi. Cum sociis natoque penatibus et magnis dis parturient montes, nascetur ridiculus mus. Nulla dui. Fusce feugiat malesuada odio"
                 },
                 new WidgetData
                 {
                  Title = "Donec tempor libero", 
                  Content = "Aliquam dapibus tincidunt metus. Praesent justo dolor, lobortis quis, lobortis dignissim, pulvinar ac, lorem. Lorem ipsum dolor sit amet, consectetuer adipiscing elit. Praesent vestibulum molestie lacus. Aenean nonummy hendrerit mauris. Phasellus porta. Fusce suscipit varius mi. Cum sociis natoque penatibus et magnis dis parturient montes, nascetur ridiculus mus. Nulla dui"
                 },
            };

            var widget = new WidgetContent
            {
                WidgetId = "12345",
                ContentId = "12345",
                Name = "Blog Name",
                Type = "Blog",
                Color = "yellow",
                Data = JsonConvert.SerializeObject(data)
            };
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
            public string Data { get; set; }
        }

        public class WidgetData
        {
            public string Title { get; set; }

            public string Content { get; set; }
        }
    }
}
