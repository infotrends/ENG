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
using SitebracoApi.DbEntities;

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
        public List<WidgetContent> GetWidget(string ClientId)
        {
            using (var db = new SitebracoEntities())
            {
                var widget = db.ENG_WidgetLookupView.Where(x => x.ClientID.Equals(ClientId)).ToList();
                WidgetContent result = new WidgetContent();

                return new List<WidgetContent> { result };
            }
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
