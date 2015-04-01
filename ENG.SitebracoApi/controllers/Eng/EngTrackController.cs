using CorrugatedIron.Models;
using SitebracoApi.Models;
using SitebracoApi.Models.Eng;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace SitebracoApi.Controllers.Eng
{
    public class EngTrackController : BaseController
    {
        [HttpPost, HttpGet]
        public object CollectClientInfo(string clientId, int width, int height)
        {
            var osList = new List<OSModel>{
                new OSModel{name="Windows 3.11", alias="Win16"},
	            new OSModel{name="Windows 95", alias="Windows 95,Win95,Windows_95"},
	            new OSModel{name="Windows ME", alias="Win 9x 4.90,Windows ME"},
	            new OSModel{name="Windows 98", alias="Windows 98,Win98"},
	            new OSModel{name="Windows CE", alias="Windows CE"},
	            new OSModel{name="Windows 2000", alias="Windows NT 5.0,Windows 2000"},
	            new OSModel{name="Windows XP", alias="Windows NT 5.1,Windows XP"},
	            new OSModel{name="Windows Server 2003", alias="Windows NT 5.2"},
	            new OSModel{name="Windows Vista", alias="Windows NT 6.0"},
	            new OSModel{name="Windows 7", alias="Windows 7,Windows NT 6.1"},
	            new OSModel{name="Windows 8.1", alias="Windows 8.1,Windows NT 6.3"},
	            new OSModel{name="Windows 8", alias="Windows 8,Windows NT 6.2"},
	            new OSModel{name="Windows NT 4.0", alias="Windows NT 4.0,WinNT4.0,WinNT,Windows NT"},
	            new OSModel{name="Windows ME", alias="Windows ME"},
	            new OSModel{name="Android", alias="Android"},
	            new OSModel{name="Open BSD", alias="OpenBSD"},
	            new OSModel{name="Sun OS", alias="SunOS"},
	            new OSModel{name="Linux", alias="Linux,X11"},
	            new OSModel{name="iOS", alias="iPhone,iPad,iPod"},
	            new OSModel{name="Mac OS X", alias="Mac OS X"},
	            new OSModel{name="Mac OS", alias="MacPPC,MacIntel,Mac_PowerPC,Macintosh"},
	            new OSModel{name="QNX", alias="QNX"},
	            new OSModel{name="UNIX", alias="UNIX"},
	            new OSModel{name="BeOS", alias="BeOS"},
	            new OSModel{name="OS 2", alias=@"OS/2"},
	            new OSModel{name="Search Bot", alias="nuhk,Googlebot,Yammybot,Openbot,Slurp,MSNBot,Ask Jeeves\"Teoma,ia_archiver"}
            };

            var operatingSystem = "";
            var userAgent = HttpContext.Current.Request.UserAgent;
            foreach (var os in osList)
            {
                var aliasList = Regex.Split(os.alias, @"\,");
                var check = false;
                foreach (var alias in aliasList)
                {
                    if (userAgent.Contains(alias))
                    {
                        operatingSystem = os.name;
                        check = true;
                        break;
                    }
                }
                if (check) break;
            }

            var referrer = HttpContext.Current.Request.UrlReferrer;
            var UrlReferrer = referrer == null ? string.Empty : referrer.Scheme;

            var data = new ClientInfoModel
            {
                ClientId_s = clientId,
                IPAddress_s = HttpContext.Current.Request.UserHostAddress,
                Browser_tsd = HttpContext.Current.Request.Browser.Browser,
                BrowserMajorVersion_i = HttpContext.Current.Request.Browser.MajorVersion,
                BrowserMinnorVersion_d = HttpContext.Current.Request.Browser.MinorVersion,
                BrowserVersion_s = HttpContext.Current.Request.Browser.Version,
                Platform_tsd = HttpContext.Current.Request.Browser.Platform,
                UserAgent_tsd = userAgent,
                OperatingSystem_tsd = operatingSystem,
                ScreenResolution_tsd = width + "x" + height
            };            
            return new { success = data.Save() };
        }

        [HttpGet]
        public object CollectClientInfoTest(string clientId, int width, int height)
        {
            var osList = new List<OSModel>{
                new OSModel{name="Windows 3.11", alias="Win16"},
	            new OSModel{name="Windows 95", alias="Windows 95,Win95,Windows_95"},
	            new OSModel{name="Windows ME", alias="Win 9x 4.90,Windows ME"},
	            new OSModel{name="Windows 98", alias="Windows 98,Win98"},
	            new OSModel{name="Windows CE", alias="Windows CE"},
	            new OSModel{name="Windows 2000", alias="Windows NT 5.0,Windows 2000"},
	            new OSModel{name="Windows XP", alias="Windows NT 5.1,Windows XP"},
	            new OSModel{name="Windows Server 2003", alias="Windows NT 5.2"},
	            new OSModel{name="Windows Vista", alias="Windows NT 6.0"},
	            new OSModel{name="Windows 7", alias="Windows 7,Windows NT 6.1"},
	            new OSModel{name="Windows 8.1", alias="Windows 8.1,Windows NT 6.3"},
	            new OSModel{name="Windows 8", alias="Windows 8,Windows NT 6.2"},
	            new OSModel{name="Windows NT 4.0", alias="Windows NT 4.0,WinNT4.0,WinNT,Windows NT"},
	            new OSModel{name="Windows ME", alias="Windows ME"},
	            new OSModel{name="Android", alias="Android"},
	            new OSModel{name="Open BSD", alias="OpenBSD"},
	            new OSModel{name="Sun OS", alias="SunOS"},
	            new OSModel{name="Linux", alias="Linux,X11"},
	            new OSModel{name="iOS", alias="iPhone,iPad,iPod"},
	            new OSModel{name="Mac OS X", alias="Mac OS X"},
	            new OSModel{name="Mac OS", alias="MacPPC,MacIntel,Mac_PowerPC,Macintosh"},
	            new OSModel{name="QNX", alias="QNX"},
	            new OSModel{name="UNIX", alias="UNIX"},
	            new OSModel{name="BeOS", alias="BeOS"},
	            new OSModel{name="OS 2", alias=@"OS/2"},
	            new OSModel{name="Search Bot", alias="nuhk,Googlebot,Yammybot,Openbot,Slurp,MSNBot,Ask Jeeves\"Teoma,ia_archiver"}
            };

            var operatingSystem = "";
            var userAgent = HttpContext.Current.Request.UserAgent;
            foreach (var os in osList)
            {
                var aliasList = Regex.Split(os.alias, @"\,");
                var check = false;
                foreach (var alias in aliasList)
                {
                    if (userAgent.Contains(alias))
                    {
                        operatingSystem = os.name;
                        check = true;
                        break;
                    }
                }
                if (check) break;
            }

            var data = new ClientInfoModel
            {
                ClientId_s = clientId,
                IPAddress_s = HttpContext.Current.Request.UserHostAddress,
                Browser_tsd = HttpContext.Current.Request.Browser.Browser,
                BrowserMajorVersion_i = HttpContext.Current.Request.Browser.MajorVersion,
                BrowserMinnorVersion_d = HttpContext.Current.Request.Browser.MinorVersion,
                BrowserVersion_s = HttpContext.Current.Request.Browser.Version,
                Platform_tsd = HttpContext.Current.Request.Browser.Platform,
                UserAgent_tsd = userAgent,
                OperatingSystem_tsd = operatingSystem,
                ScreenResolution_tsd = width + "x" + height
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
            return new { success = true, data = data };
        }

        [HttpPost, HttpGet]
        public object CollectSetOfMouseActionInfo(IEnumerable<MouseTrackModel> data)
        {
            if (data == null || data.Count() == 0)
                return new { success = true };
            
            var bucketName = ObjectUtil.GetClassName<MouseTrackModel>();
            var bucketType = ObjectUtil.GetPropertyName<Constant.RiakSolr.BucketType>(x => x.InfoTrendsLog);
            var list = new List<RiakObject>();
            foreach (var item in data)
            {
                item.PageUrl_tsd = HttpContext.Current.Request.Url.AbsolutePath;
                item.Position_s = string.Format("{0},{1}", item.PageX_i, item.PageY_i);
                var riakObjId = new RiakObjectId(bucketType, bucketName, item.Id_s);
                var riakObj = new RiakObject(riakObjId, item);
                list.Add(riakObj);
            }

            var client = MyRiak.RiakHelper.CreateClient(ObjectUtil.GetPropertyName<Constant.RiakSolr.ConfigSection>(x => x.riakSolrConfig));
            var results = client.Put(list);
            
            return new { success = true };
        }

        [HttpPost, HttpGet]
        public object CollectSetOfMouseActionInfoTest(IEnumerable<MouseTrackModel> data)
        {
            foreach (var item in data)
            {
                item.PageUrl_tsd = HttpContext.Current.Request.Url.AbsolutePath;
                item.Position_s = string.Format("{0},{1}", item.PageX_i, item.PageY_i);                
            }
            return new { success = true, data = data };
        }

    }

    class OSModel
    {
        public string name { get; set; }

        public string alias { get; set; }
    }
}
