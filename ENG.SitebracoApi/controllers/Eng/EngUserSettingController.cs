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

                db.SaveChanges();
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

        [HttpPost, HttpGet]
        public ENG_UserSetting Update(ENG_UserSetting data)
        {
            using (var db = new SitebracoEntities())
            {
                if (data.ClientId != null && data.ClientId != "")
                {
                    var tmp = db.ENG_UserSetting.Where(x => x.ClientId.Equals(data.ClientId)).FirstOrDefault();
                    //update data
                    if (data.MouseClickTracking != 0)
                        tmp.MouseClickTracking = data.MouseClickTracking;

                    if (data.MouseMoveTracking != 0)
                        tmp.MouseMoveTracking = data.MouseMoveTracking;

                    if (data.PageViewsCounter != 0)
                        tmp.PageViewsCounter = data.PageViewsCounter;

                    if (data.PageViewsRankingHigh != 0)
                        tmp.PageViewsRankingHigh = data.PageViewsRankingHigh;

                    if (data.PageViewsRankingLow != 0)
                        tmp.PageViewsRankingLow = data.PageViewsRankingLow;

                    if (data.PageViewsRankingMedium != 0)
                        tmp.PageViewsRankingMedium = data.PageViewsRankingMedium;

                    db.SaveChanges();
                }

            }
            return null;
        }
    }
}
