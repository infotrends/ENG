using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using SitebracoApi.Models;
using umbraco.NodeFactory;
using Umbraco.Web.Mvc;
using Umbraco.Core;
using SitebracoApi.Types;
using Umbraco.Core.Models;

namespace SitebracoApi.Controllers.Sinh
{
    [PluginController(Constant.SitebracoApiModule.Sinh)]
    public class TestController : BaseController
    {
        [HttpGet]
        public string DoTest([FromUri]DoTestParam p)
        {
            //IContent content = Services.ContentService.GetById(3059);
            //var ret = Extlib.ConvertFromUmbracoModel<UmbracoDoctypes.Media>(content);
            //ret.media_sourceFile = "";
            return System.Web.HttpContext.Current.Server.MapPath("/");
            
        }
        public class DoTestParam
        {
            public int Test { get; set; }
            public int Test2 { get; set; }
        }

    }
}
