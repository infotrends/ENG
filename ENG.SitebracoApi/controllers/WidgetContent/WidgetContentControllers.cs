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

namespace SitebracoApi.Controllers.WidgetContent
{

    public class WidgetContentController : BaseController
    {
        [HttpPost]
        public IEnumerable<string> GetAllContent(WidgetContent param)
        {
            return new[] { "Table", "Chair", "Desk", "Computer", "Beer fridge1", param.ContentId, param.WidgetId };
        }



        [HttpGet]
        public List<ENG_WidgetLookupView> GetWidget(string ClientId)
        {
            using (var db = new SitebracoEntities())
            {
                var widget = db.ENG_WidgetLookupView.Where(x => x.ClientID.Equals(ClientId)).ToList();

                if (widget.Count > 0) return widget;

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
                setting.Height = param.Height;
                setting.Name = param.Name;
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

                db.ENG_WidgetContent.Add(widgetContent);
                db.SaveChanges();

                return new
                {
                    success = true
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
    }
}
