using SitebracoApi.DbEntities;
using System.Linq;
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
        public ENG_UserSetting Read(string clientId)
        {
            using (var db = new SitebracoEntities())
            {
                
                var tmp = db.ENG_UserSetting.Where(x => x.ClientId.Equals(clientId)).ToList();
                if (tmp.Count > 0)
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
                if (string.IsNullOrEmpty(data.ClientId)) return null;
                var tmp = db.ENG_UserSetting.FirstOrDefault(x => x.ClientId.Equals(data.ClientId));
                //update data
                if (data.MouseClickTracking != 0)
                    if (tmp != null) tmp.MouseClickTracking = data.MouseClickTracking;

                if (data.MouseMoveTracking != 0)
                    if (tmp != null) tmp.MouseMoveTracking = data.MouseMoveTracking;

                if (data.PageViewsCounter != 0)
                    if (tmp != null) tmp.PageViewsCounter = data.PageViewsCounter;

                if (data.PageViewsRankingHigh != 0)
                    if (tmp != null) tmp.PageViewsRankingHigh = data.PageViewsRankingHigh;

                if (data.PageViewsRankingLow != 0)
                    if (tmp != null) tmp.PageViewsRankingLow = data.PageViewsRankingLow;

                if (data.PageViewsRankingMedium != 0)
                    if (tmp != null) tmp.PageViewsRankingMedium = data.PageViewsRankingMedium;

                db.SaveChanges();
            }
            return null;
        }
    }
}
