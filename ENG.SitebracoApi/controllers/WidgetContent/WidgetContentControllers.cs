using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using SitebracoApi.DbEntities;
using System.Data.SqlClient;
using SitebracoApi.Models.Eng;
using SitebracoApi.Models;

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
        public List<WidgetData> GetWidget(string clientId)
        {
            using (var db = new SitebracoEntities())
            {
                var widget = db.Database.SqlQuery<WidgetData>("GetWidgetList @ClientId", new SqlParameter("@ClientId", clientId)).ToList();

                return widget.Count > 0 ? widget : null;
            }
        }

        [HttpGet]
        public List<ENG_WidgetLookupView> GetAllWidget(string clientId)
        {
            using (var db = new SitebracoEntities())
            {
                var sql = string.Format(@"SELECT 
	                        W.WidgetTypeName, 
	                        WC.URL, WS.Color, WS.Width, WS.Height, WS.Name,
	                        WC.ID, WD.Title, WD.Content, WC.ClientID, WC.Position, WDT.ID AS WidgetDataTypeID, WD.ID AS WidgetDataID

	                        FROM ENG_WidgetContent WC
	                        INNER JOIN [dbo].[ENG_Widget] W
		                        ON W.ID = WC.WidgetId
	                        INNER JOIN [dbo].[ENG_WidgetSetting] WS
		                        ON WC.[WidgetSettingID] = WS.ID
	                        LEFT JOIN [dbo].[ENG_WidgetContent_WidgetDataType_Lookup] WWL
		                        ON WWL.[WidgetContentID] = WC.[ID]
	                        LEFT JOIN [dbo].[ENG_WidgetDataType] WDT
		                        ON WDT.ID = WWL.[WidgetDataTypeID]
	                        LEFT JOIN [dbo].[ENG_WidgetData] WD
		                        ON WD.WidgetDataTypeID = WDT.ID
                            WHERE WC.ClientID = {0} AND WC.Position IS NOT NULL", clientId);
                var result = db.Database.SqlQuery<ENG_WidgetLookupView>(sql).ToList();

                return result;
            }
        }

        [HttpPost][HttpGet]
        public List<ENG_WidgetData> GetAllContent(List<int> id)
        {
            using (var db = new SitebracoEntities())
            {
                List<ENG_WidgetData> content;

                if (id == null || id.Count == 0)
                {
                    content = db.ENG_WidgetData.ToList();
                }
                else
                {
                    var sql = @"SELECT * FROM [dbo].[ENG_WidgetData]
                            WHERE [WidgetDataTypeID] IN (";
                    for (var i = 0; i < id.Count; i++)
                    {
                        sql += id[i];
                        if (i != id.Count - 1) sql += ",";
                        else sql += ")";
                    }
                    content = db.Database.SqlQuery<ENG_WidgetData>(sql).ToList();
                }

                return content.Count > 0 ? content : null;
            }
        }

        [HttpPost]
        public object AddWidget(WidgetParam param)
        {
            using (var db = new SitebracoEntities())
            {
                var setting = new ENG_WidgetSetting
                {
                    Color = param.Color,
                    Height = param.Height,
                    Name = param.Name,
                    Width = param.Width
                };

                //if (param.Height != 0)

                //if (param.Width != 0)

                setting = db.ENG_WidgetSetting.Add(setting);

                // Call SaveChanges to get setting's ID
                db.SaveChanges();

                //Find widget
                var widget = db.ENG_Widget.FirstOrDefault(x => x.WidgetTypeName.Equals(param.WidgetTypeName));

                //add widget content
                if (widget != null)
                {
                    var widgetContent = new ENG_WidgetContent
                    {
                        WidgetSettingID = setting.ID,
                        Position = param.Position,
                        URL = param.URL,
                        WidgetId = widget.ID,
                        ClientID = param.ClientID
                    };


                    // If it's another widget: subscribe, search...

                    widgetContent = db.ENG_WidgetContent.Add(widgetContent);
                    db.SaveChanges();

                    if (param.WidgetDataTypeId != null && param.WidgetDataTypeId.Count > 0)
                    {
                        foreach (var lookup in param.WidgetDataTypeId.Select(item => new ENG_WidgetContent_WidgetDataType_Lookup
                        {
                            WidgetContentID = widgetContent.ID,
                            WidgetDataTypeID = item
                        }))
                        {
                            db.ENG_WidgetContent_WidgetDataType_Lookup.Add(lookup);
                        }
                        db.SaveChanges();
                    }
                }

                //Perform Unit Of Work
                db.SaveChanges();

                return new
                {
                    success = true
                };
            }
        }

        [HttpPost]
        public Response<WidgetParam> AddWidgetTest(WidgetParam param)
        {
            return new Response<WidgetParam>
            {
                success = true,
                data = param
            };
        }

        [HttpPost]
        public object UpdateWidget(WidgetParam param)
        {
            using (var db = new SitebracoEntities())
            {
                var widget = db.ENG_WidgetContent.FirstOrDefault(x => x.ID == param.ID);

                if (widget == null)
                    return new
                    {
                        success = false,
                        message = "Invalid Widget"
                    };

                var setting = db.ENG_WidgetSetting.FirstOrDefault(x => x.ID == widget.WidgetSettingID);

                if (!param.Name.Equals(""))
                {
                    if (setting != null) setting.Name = param.Name;
                }
                if (!param.Position.Equals(""))
                {
                    widget.Position = param.Position;
                }
                if (!param.Color.Equals(""))
                {
                    if (setting != null) setting.Color = param.Color;
                }
                if (param.Height != 0)
                {
                    if (setting != null) setting.Height = param.Height;
                }
                if (param.Width != 0)
                {
                    if (setting != null) setting.Width = param.Width;
                }
                db.SaveChanges();

                return new
                {
                    success = true,
                    message = "Update Successfully"
                };
            }
        }

        [HttpPost, HttpGet]
        public object Subscribe(SubcribeParam param)
        {
            using (var db = new SitebracoEntities())
            {
                var subscribe = new ENG_WidgetSubscribe
                {
                    Email = param.Email,
                    ClientId = param.ClientId
                };
                db.ENG_WidgetSubscribe.Add(subscribe);

                db.SaveChanges();
                return new {
                    success = true,
                };
            }
        }
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

        public List<int> WidgetDataTypeId { get; set; }

    }

    public class WidgetData
    {
        public string WidgetTypeName { get; set; }

        public string Color { get; set; }

        public int Width { get; set; }

        public int Height { get; set; }

        public string Name { get; set; }
    }

    public class SubcribeParam{
        public string ClientId { get; set; }

        public string Email { get; set; }
    }
}
