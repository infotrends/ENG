using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using MyRiak;
using CorrugatedIron;
using CorrugatedIron.Models;
using MyUtils.Validations;
using CorrugatedIron.Models.Search;
using Newtonsoft.Json;
using SitebracoApi.DbEntities;
using System.Data.SqlClient;

namespace SitebracoApi.Controllers.WidgetContent
{

    public class WidgetContentController : BaseController
    {
        [HttpGet]
        public List<ENG_WidgetDataType> GetAllContentType()
        {
            using (var db = new SitebracoEntities())
            {
                return db.ENG_WidgetDataType.ToList();
            }
        }

        [HttpGet]
        public List<WidgetData> GetWidget(string ClientId)
        {
            using (var db = new SitebracoEntities())
            {
                var widget = db.Database.SqlQuery<WidgetData>("GetWidgetList @ClientId", new SqlParameter("@ClientId", ClientId)).ToList();

                if (widget.Count > 0) return widget;

                return null;
            }
        }

        [HttpGet]
        public List<ENG_WidgetLookupView> GetAllWidget(string ClientId)
        {
            using (var db = new SitebracoEntities())
            {
                var widget = db.ENG_WidgetLookupView.Where(x => x.ClientID.Equals(ClientId)).
                            Where(x => x.Position != null).ToList();

                if (widget.Count > 0) return widget;

                return null;
            }
        }

        [HttpPost][HttpGet]
        public List<ENG_WidgetData> GetAllContent(List<int> id)
        {
            using (var db = new SitebracoEntities())
            {
                List<ENG_WidgetData> content = null;

                if (id == null || id.Count == 0)
                {
                    content = db.ENG_WidgetData.ToList();
                }
                else
                {
                    var sql = @"SELECT * FROM [dbo].[ENG_WidgetData]
                            WHERE [WidgetDataTypeID] IN (";
                    for (int i = 0; i < id.Count; i++)
                    {
                        sql += id[i];
                        if (i != id.Count - 1) sql += ",";
                        else sql += ")";
                    }
                    content = db.Database.SqlQuery<ENG_WidgetData>(sql).ToList();
                }

                if (content.Count > 0) return content;

                return null;
            }
        }

        [HttpPost]
        public object AddWidget(WidgetParam param)
        {
            using (var db = new SitebracoEntities())
            {
                ENG_WidgetSetting setting = new ENG_WidgetSetting();
                setting.Color = param.Color;

                //if (param.Height != 0)
                setting.Height = param.Height;

                setting.Name = param.Name;
                //if (param.Width != 0)
                setting.Width = param.Width;

                setting = db.ENG_WidgetSetting.Add(setting);
                db.SaveChanges();

                //Find widget
                var widget = db.ENG_Widget.Where(x => x.WidgetTypeName.Equals(param.WidgetTypeName)).FirstOrDefault();

                //add widget content
                ENG_WidgetContent widgetContent = new ENG_WidgetContent();

                widgetContent.WidgetSettingID = setting.ID;
                widgetContent.Position = param.Position;
                widgetContent.URL = param.URL;
                widgetContent.WidgetId = widget.ID;
                widgetContent.ClientID = param.ClientID;
                widgetContent.WidgetDataTypeID = param.WidgetDataTypeId;

                db.ENG_WidgetContent.Add(widgetContent);
                db.SaveChanges();

                return new
                {
                    success = true
                };
            }
        }

        [HttpPost]
        public object UpdateWidget(WidgetParam param)
        {
            using (var db = new SitebracoEntities())
            {
                var widget = db.ENG_WidgetContent.Where(x => x.ID == param.ID).FirstOrDefault();
                if (widget != null)
                {
                    var setting = db.ENG_WidgetSetting.Where(x => x.ID == widget.WidgetSettingID).FirstOrDefault();

                    if (!param.Name.Equals(""))
                    {
                        setting.Name = param.Name;
                    }
                    if (!param.Position.Equals(""))
                    {
                        widget.Position = param.Position;
                    }
                    if (!param.Color.Equals(""))
                    {
                        setting.Color = param.Color;
                    }
                    if (param.Height != 0)
                    {
                        setting.Height = param.Height;
                    }
                    if (param.Width != 0)
                    {
                        setting.Width = param.Width;
                    }
                    db.SaveChanges();

                    return new
                    {
                        success = true,
                        message = "Update Successfully"
                    };
                }
                else
                {
                    return new
                    {
                        success = false,
                        message = "Invalid Widget"
                    };
                }
            }
        }

        //[HttpPost, HttpGet]
        //public object Subscribe(string email)
        //{
        //    using (var db = new SitebracoEntities)
        //    {
                
        //    }
        //}
    }

    public class WidgetContent
    {
        public string WidgetId { get; set; }
        public string ContentId { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public string Color { get; set; }
        public string Data { get; set; }
    }

    public class WidgetParam
    {
        public string WidgetTypeName { get; set; }

        public string URL { get; set; }

        public string Position { get; set; }

        public string Color { get; set; }

        public int Width { get; set; }

        public int Height { get; set; }

        public string Name { get; set; }

        public string ClientID { get; set; }

        public int ID { get; set; }

        public int WidgetDataTypeId { get; set; }
    }

    public class WidgetData
    {
        public string WidgetTypeName { get; set; }

        public string Color { get; set; }

        public int Width { get; set; }

        public int Height { get; set; }

        public string Name { get; set; }
    }
}
