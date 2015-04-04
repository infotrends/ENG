using CorrugatedIron.Models;
using SitebracoApi.Models;
using SitebracoApi.Models.Eng;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Xml;

namespace SitebracoApi.Controllers.Eng
{
    public class EngTrackController : BaseController
    {
        [HttpPost, HttpGet]
        public object CollectClientInfo(string clientId, int width, int height)
        {
            var referrer = HttpContext.Current.Request.UrlReferrer;
            var UrlReferrer = referrer == null ? string.Empty : referrer.Scheme;

            var userLocation = GetUserLocation();
          
            var data = new ClientInfoModel
            {
                ClientId_s = clientId,
                IPAddress_s = HttpContext.Current.Request.UserHostAddress,
                Browser_s = HttpContext.Current.Request.Browser.Browser,
                BrowserMajorVersion_i = HttpContext.Current.Request.Browser.MajorVersion,
                BrowserMinnorVersion_d = HttpContext.Current.Request.Browser.MinorVersion,
                BrowserVersion_s = HttpContext.Current.Request.Browser.Version,
                Platform_tsd = HttpContext.Current.Request.Browser.Platform,
                UserAgent_tsd = HttpContext.Current.Request.UserAgent,
                OperatingSystem_s = GetOperatingSystem(),
                ScreenResolution_tsd = width + "x" + height,
                CountryName_s = userLocation.Rows[0]["CountryName"].ToString(),
                City_s = userLocation.Rows[0]["City"].ToString(),
                Latitude_f = float.Parse(userLocation.Rows[0]["Latitude"].ToString()),
                Longitude_f = float.Parse(userLocation.Rows[0]["Longitude"].ToString()),
                Device_s = GetDevice()
            };
            return new { success = data.Save() };
        }

        [HttpGet]
        public object CollectClientInfoTest(string clientId, int width, int height)
        {
            var userLocation = GetUserLocation();
            
            var data = new ClientInfoModel
            {
                ClientId_s = clientId,
                IPAddress_s = HttpContext.Current.Request.UserHostAddress,
                Browser_s = HttpContext.Current.Request.Browser.Browser,
                BrowserMajorVersion_i = HttpContext.Current.Request.Browser.MajorVersion,
                BrowserMinnorVersion_d = HttpContext.Current.Request.Browser.MinorVersion,
                BrowserVersion_s = HttpContext.Current.Request.Browser.Version,
                Platform_tsd = HttpContext.Current.Request.Browser.Platform,
                UserAgent_tsd = HttpContext.Current.Request.UserAgent,
                OperatingSystem_s = GetOperatingSystem(),
                ScreenResolution_tsd = width + "x" + height,
                CountryName_s = userLocation.Rows[0]["CountryName"].ToString(),
                City_s = userLocation.Rows[0]["City"].ToString(),
                Latitude_f = float.Parse(userLocation.Rows[0]["Latitude"].ToString()),
                Longitude_f = float.Parse(userLocation.Rows[0]["Longitude"].ToString()),
                Device_s = GetDevice(),
            };
            return new { success = true, data = data};
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
            if (data == null || data.Count() == 0)
                return new { success = true };

            foreach (var item in data)
            {
                item.PageUrl_tsd = HttpContext.Current.Request.Url.AbsolutePath;
                item.Position_s = string.Format("{0},{1}", item.PageX_i, item.PageY_i);
            }

            return new { success = true, data = data };
        }

        [HttpPost, HttpGet]
        public object CollectFeedback(FeedbackModel data)
        {
            return new { success = data.Save() };
        }

        [HttpPost, HttpGet]
        public object CollectFeedbackTest(FeedbackModel data)
        {
            return new { success = true, data = data };
        }

        [HttpPost, HttpGet]
        public object CollectVisitorLogTest(VisitorLogModel data)
        {
            return new { success = true, data = data };
        }

        [HttpPost, HttpGet]
        public object CollectVisitorLog(VisitorLogModel data)
        {
            return new { success = data.Save() };
        }

        private string GetOperatingSystem()
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
            return operatingSystem;
        }

        private DataTable GetUserLocation()
        {

            //Create a WebRequest with the current Ip 
            WebRequest _objWebRequest =
                WebRequest.Create("http://freegeoip.net/xml/");
            //Create a Web Proxy 
            WebProxy _objWebProxy =
               new WebProxy("http://freegeoip.net/xml/", true);

            //Assign the proxy to the WebRequest 
            _objWebRequest.Proxy = _objWebProxy;

            //Set the timeout in Seconds for the WebRequest 
            _objWebRequest.Timeout = 2000;

            try
            {
                //Get the WebResponse  
                WebResponse _objWebResponse = _objWebRequest.GetResponse();
                //Read the Response in a XMLTextReader 
                XmlTextReader _objXmlTextReader
                    = new XmlTextReader(_objWebResponse.GetResponseStream());

                //Create a new DataSet 
                DataSet _objDataSet = new DataSet();
                //Read the Response into the DataSet 
                _objDataSet.ReadXml(_objXmlTextReader);

                return _objDataSet.Tables[0];
            }
            catch
            {
                return null;
            }
        } // End of GetLocation   

        private string GetDevice()
        {
            Regex mobileReg = new Regex(@"/Android|webOS|iPhone|iPad|iPod|BlackBerry|IEMobile|Opera Mini/");

            //Get Device information
            Regex b = new Regex(@"(android|bb\d+|meego).+mobile|avantgo|bada\/|blackberry|blazer|compal|elaine|fennec|hiptop|iemobile|ip(hone|od)|iris|kindle|lge |maemo|midp|mmp|mobile.+firefox|netfront|opera m(ob|in)i|palm( os)?|phone
                |p(ixi|re)\/|plucker|pocket|psp|series(4|6)0|symbian|treo|up\.(browser|link)|vodafone|wap|windows ce|xda|xiino",
                  RegexOptions.IgnoreCase | RegexOptions.Multiline);

            Regex v = new Regex(@"1207|6310|6590|3gso|4thp|50[1-6]i|770s|802s|a wa|abac|ac(er|oo|s\-)|ai(ko|rn)|al(av|ca|co)|amoi|an(ex|ny|yw)|aptu|ar(ch|go)|as(te|us)|
                attw|au(di|\-m|r |s )|avan|be(ck|ll|nq)|bi(lb|rd)|bl(ac|az)|br(e|v)w|bumb|bw\-(n|u)|c55\/|capi|ccwa|cdm\-|cell|chtm|cldc|cmd\-|co(mp|nd)|craw|da(it|ll|ng)|
                dbte|dc\-s|devi|dica|dmob|do(c|p)o|ds(12|\-d)|el(49|ai)|em(l2|ul)|er(ic|k0)|esl8|ez([4-7]0|os|wa|ze)|fetc|fly(\-|_)|g1 u|g560|gene|gf\-5|g\-mo|
                go(\.w|od)|gr(ad|un)|haie|hcit|hd\-(m|p|t)|hei\-|hi(pt|ta)|hp( i|ip)|hs\-c|ht(c(\-| |_|a|g|p|s|t)|tp)|hu(aw|tc)|i\-(20|go|ma)|i230|iac( |\-|\/)|
                ibro|idea|ig01|ikom|im1k|inno|ipaq|iris|ja(t|v)a|jbro|jemu|jigs|kddi|keji|kgt( |\/)|klon|kpt |kwc\-|kyo(c|k)|le(no|xi)|lg( g|\/(k|l|u)|50|54|\-[a-w])|
                libw|lynx|m1\-w|m3ga|m50\/|ma(te|ui|xo)|mc(01|21|ca)|m\-cr|me(rc|ri)|mi(o8|oa|ts)|mmef|mo(01|02|bi|de|do|t(\-| |o|v)|zz)|mt(50|p1|v )|mwbp|mywa|
                n10[0-2]|n20[2-3]|n30(0|2)|n50(0|2|5)|n7(0(0|1)|10)|ne((c|m)\-|on|tf|wf|wg|wt)|nok(6|i)|nzph|o2im|op(ti|wv)|oran|owg1|p800|pan(a|d|t)|pdxg|pg(13|
                \-([1-8]|c))|phil|pire|pl(ay|uc)|pn\-2|po(ck|rt|se)|prox|psio|pt\-g|qa\-a|qc(07|12|21|32|60|\-[2-7]|i\-)|qtek|r380|r600|raks|rim9|ro(ve|zo)|s55\/|
                sa(ge|ma|mm|ms|ny|va)|sc(01|h\-|oo|p\-)|sdk\/|se(c(\-|0|1)|47|mc|nd|ri)|sgh\-|shar|sie(\-|m)|sk\-0|sl(45|id)|sm(al|ar|b3|it|t5)|so(ft|ny)|sp(01|h\-|v\-|v )|
                sy(01|mb)|t2(18|50)|t6(00|10|18)|ta(gt|lk)|tcl\-|tdg\-|tel(i|m)|tim\-|t\-mo|to(pl|sh)|ts(70|m\-|m3|m5)|tx\-9|up(\.b|g1|si)|utst|v400|v750|veri|vi(rg|te)|
                vk(40|5[0-3]|\-v)|vm40|voda|vulc|vx(52|53|60|61|70|80|81|83|85|98)|w3c(\-| )|webc|whit|wi(g |nc|nw)|wmlb|wonu|x700|yas\-|your|zeto|zte\-",
                RegexOptions.IgnoreCase | RegexOptions.Multiline);

            var device = "";

            var isMobile = mobileReg.Match(HttpContext.Current.Request.UserAgent);
            if (isMobile.Success)
            {
                device = isMobile.Value;
            }

            var matchB = b.Match(HttpContext.Current.Request.UserAgent);
            if (matchB.Success)
            {
                device = matchB.Value;
            }

            var matchV = v.Match(HttpContext.Current.Request.UserAgent.Substring(0, 4));

            if (matchV.Success)
            {
                device = matchV.Value;
            }

            if (device == "")
            {
                device = "Desktop";
            }
            return device;
        }
    }

    class OSModel
    {
        public string name { get; set; }

        public string alias { get; set; }
    }
}
