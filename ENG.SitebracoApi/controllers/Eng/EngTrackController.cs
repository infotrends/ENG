using SitebracoApi.Models;
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
    public class EngTrackController : BaseController
    {
        [HttpPost,HttpGet]
        public object CollectClientInfo(string clientId)
        {
            var data = new ClientInfoModel
            {
                ClientId_s = clientId,
                IPAddress_s = HttpContext.Current.Request.UserHostAddress,
                PageUrl_tsd = HttpContext.Current.Request.Url.AbsoluteUri,
                Host_tsd = HttpContext.Current.Request.Url.Host,
                Browser_tsd = HttpContext.Current.Request.Browser.Browser,
                BrowserMajorVersion_i = HttpContext.Current.Request.Browser.MajorVersion,
                BrowserMinnorVersion_d = HttpContext.Current.Request.Browser.MinorVersion,
                BrowserVersion_s = HttpContext.Current.Request.Browser.Version,
                Platform_tsd = HttpContext.Current.Request.Browser.Platform,
                UserAgent_tsd = HttpContext.Current.Request.UserAgent
            };            
            return new { success = data.Save() };
        }

        [HttpGet]
        public object CollectClientInfoTest(string clientId)
        {
            var data = new ClientInfoModel
            {
                ClientId_s = clientId,
                IPAddress_s = HttpContext.Current.Request.UserHostAddress,
                PageUrl_tsd = HttpContext.Current.Request.Url.AbsoluteUri,
                Host_tsd = HttpContext.Current.Request.Url.Host,
                Browser_tsd = HttpContext.Current.Request.Browser.Browser,
                BrowserMajorVersion_i = HttpContext.Current.Request.Browser.MajorVersion,
                BrowserMinnorVersion_d = HttpContext.Current.Request.Browser.MinorVersion,
                BrowserVersion_s = HttpContext.Current.Request.Browser.Version,
                Platform_tsd = HttpContext.Current.Request.Browser.Platform,
                UserAgent_tsd = HttpContext.Current.Request.UserAgent
            };
            return new { success = true, data = data };
        }

        [HttpPost, HttpGet]
        public SimpleObjectModel<bool> CollectMouseActionInfo(MouseTrackModel data)
        {
            data.PageUrl_tsd = HttpContext.Current.Request.Url.AbsolutePath;
            var ret = new SimpleObjectModel<bool>(data.Save());
            return ret;
        }


        [HttpPost, HttpGet]
        public object CollectMouseActionInfoTest(MouseTrackModel data)
        {
            data.PageUrl_tsd = HttpContext.Current.Request.Url.AbsolutePath;
            return new {success = true, data = data};
        }

        [HttpPost, HttpGet]
        public object CollectSetOfMouseActionInfo(IEnumerable<MouseTrackModel> data)
        {
            foreach (var item in data)
            {
                item.PageUrl_tsd = HttpContext.Current.Request.Url.AbsolutePath;
                item.Save();
            }
            return new { success = true };
        }

        [HttpPost, HttpGet]
        public object CollectSetOfMouseActionInfoTest(IEnumerable<MouseTrackModel> data)
        {            
            return new { success = true, data = data };
        }

    }
}
