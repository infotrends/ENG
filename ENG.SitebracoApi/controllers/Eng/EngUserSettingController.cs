using SitebracoApi.DbEntities;
using SitebracoApi.Models.Eng;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace SitebracoApi.Controllers.Eng
{
    public class EngUserSettingController : BaseController
    {
        [HttpPost, HttpGet]
        public void Create(ENG_UserSetting data)
        {
            using (var db = new SitebracoEntities())
            {
                db.ENG_UserSetting.Add(data);
            }
        }

        [HttpPost, HttpGet]
        public ENG_UserSetting Read(string ClientId)
        {
            using (var db = new SitebracoEntities())
            {
                
                var tmp = db.ENG_UserSetting.Where(x => x.ClientId.Equals(ClientId)).ToList();
                if (tmp != null && tmp.Count > 0)
                {
                    return tmp.First();
                }
            }
            return null;
        }
    }
}
