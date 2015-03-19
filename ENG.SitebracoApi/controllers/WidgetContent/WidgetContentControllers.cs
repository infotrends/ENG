using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace SitebracoApi.Controllers.WidgetContent
{
    public class WidgetContentController : BaseController
    {
        [HttpPost]
        public IEnumerable<string> GetAllContent(WidgetContent param)
        {
            return new[] { "Table", "Chair", "Desk", "Computer", "Beer fridge1", param.ContentId, param.WidgetId };
        }

        public class WidgetContent
        {
            public string WidgetId { get; set; }
            public string ContentId { get; set; }
        }
    }
}
