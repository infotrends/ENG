using System;
using System.Collections.Generic;
using System.Linq;
using SitebracoApi.Models;
using SitebracoApi.Models.Eng;
using SitebracoApi.DbEntities;
using System.Web.Http;

namespace SitebracoApi.Controllers.DashboardSetting
{
    public class DashboardSettingController : BaseController
    {
        [HttpGet, HttpPost]
        public Response<string> SaveDashboard(List<DashboardSettingModel> setting)
        {
            using (var db = new SitebracoEntities())
            {
                //Change status for old data
                var clientId = setting.ElementAt(0).ClientId;
                var columnList = db.ENG_DashboardSettingColumn.Where(x => x.ClientID.Equals(clientId) && (x.Status == null || !x.Status.Equals("Old"))).ToList();
                foreach (var item in columnList)
                {
                    item.Status = "Old";
                    db.SaveChanges();
                }

                foreach (var item in setting)
                {
                    var column = new ENG_DashboardSettingColumn
                    {
                        ClientID = item.ClientId,
                        Order = item.Order,
                        Size = item.Size,
                        CreateOn = DateTime.UtcNow,
                        ModifyOn = DateTime.UtcNow,
                        Status = "Latest"
                    };

                    column = db.ENG_DashboardSettingColumn.Add(column);

                    db.SaveChanges();

                    foreach (var newReport in item.Reports.Select(report => new ENG_DashboardSettingReport
                    {
                        DashboardSettingColumnID = column.ID,
                        Order = report.Order,
                        Name = report.Name,
                        Collapse = report.Collapse,
                        CreateOn = DateTime.UtcNow,
                        ModifyOn = DateTime.UtcNow
                    }))
                    {
                        db.ENG_DashboardSettingReport.Add(newReport);
                        db.SaveChanges();
                    }
                }

                return new Response<string>
                {
                    success = true,
                    message = "Save Setting Successfully!!"
                };
            }
        }

        [HttpGet, HttpPost]
        public object SaveDashboardTest(List<DashboardSettingModel> setting)
        {
            return new
            {
                success = true,
                data = setting
            };
        }

        // TODO
        public Response<List<ENG_DashboardSettingView>> GetDashboard(string clientId)
        {
            using (var db = new SitebracoEntities())
            {
                var result = db.ENG_DashboardSettingView.
                    Where(x => x.ClientID.Equals(clientId) && (x.Status == null || !x.Status.Equals("Old")));

                var list = result.ToList();


                return new Response<List<ENG_DashboardSettingView>>
                {
                    success = true,
                    data = list
                };
            }                      
        }
    }
}
