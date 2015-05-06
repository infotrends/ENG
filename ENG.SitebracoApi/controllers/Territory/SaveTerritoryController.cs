using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using SitebracoApi.DbEntities;
using SitebracoApi.Models;

namespace SitebracoApi.Controllers.Territory
{
    public class SaveTerritoryController : BaseController
    {
        #region Action

        /// <summary>
        /// List SavedTerritories of current user
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public Response<IEnumerable<ENG_Territory>> Get(string clientId)
        {
            using (var database = new SitebracoEntities())
            {
                var tmp = database.ENG_Territory.Where(x => x.clientId.Equals(clientId))
                        .OrderByDescending(x => x.dateCreated);

                var list = tmp.ToList().Where(x => x.parentId == null);

                return new Response<IEnumerable<ENG_Territory>>
                {
                    success = true,
                    data = list
                };
            }

        }

        [HttpPost]
        public Response<ENG_Territory> Post(ENG_Territory t)
        {
            using (var database = new SitebracoEntities())
            {
                if (t.name.IsNullOrEmpty())
                {
                    throw new Exception("Territory Name invalid");
                }

                t.dateCreated = DateTime.Now;

                if (t.parentId != null)
                {
                    var parent = database.ENG_Territory.Single(x => x.territoryID == t.parentId);
                    parent.children.Add(t);
                }
                else
                {
                    database.ENG_Territory.Add(t);
                }

                database.SaveChanges();

                //foreach (var territoryGrid in t.territoryGrids)
                //{
                //    // territoryGrid.Territory = null;
                //    territoryGrid.ENG_Territory = null;
                //}
                //var response = new RestResponse<IEnumerable<ENG_Territory>>
                //{

                //    success = true,
                //    message = "Added new territory",
                //    data = new List<ENG_Territory> { t },
                //    rows = new List<ENG_Territory> { t },
                //    items = new List<ENG_Territory> { t },
                //    records = new List<ENG_Territory> { t },
                //};
                //var test = JsonConvert.SerializeObject((response));

                return new Response<ENG_Territory>()
                {
                    success = true,
                    data = t
                };
            }
            
        }

        [HttpPost, HttpPut]
        public Response<ENG_Territory> Put(ENG_Territory t)
        {
            using (var database = new SitebracoEntities())
            {
                var territory = database.ENG_Territory.SingleOrDefault(x => x.territoryID == t.territoryID);

                if (territory == null)
                {
                    return new Response<ENG_Territory>
                    {
                        success = false,
                        message = "Territory does not exist !!!"
                    };
                }

                territory.name = t.name ?? territory.name;
                territory.color = t.color ?? territory.color;
                territory.note = t.note ?? territory.note;
                territory.parentId = t.parentId;

                database.SaveChanges();
                return new Response<ENG_Territory>
                {
                    success = true,
                    data = territory
                };
            }

        }

        [HttpDelete, HttpPost]
        public Response<ENG_Territory> Delete(ENG_Territory territory)
        {
            using (var database = new SitebracoEntities())
            {
                var id = territory.territoryID;

                var result = new Response<ENG_Territory>();
                if (database.ENG_Territory.Any(x => x.territoryID == id))
                {
                    var t = database.ENG_Territory.Single(x => x.territoryID == id);
                    database.ENG_Territory.Remove(t);

                    database.SaveChanges();
                    result.success = true;
                    result.message = "Deleted";

                }
                else
                {
                    result.success = false;
                    result.message = "Record doesn't exist";
                }

                return result;
            }
            
        }
       
        #endregion
    }
}
