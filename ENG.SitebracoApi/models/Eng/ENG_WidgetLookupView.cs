using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SitebracoApi.Models.Eng
{
    public class ENG_WidgetLookupView
    {
        public string WidgetTypeName { get; set; }

        public string Name { get; set; }

        public string Color { get; set; }

        public int ID { get; set; }

        public string Title { get; set; }

        public string Content { get; set; }

        public string ClientID { get; set; }

        public string URL { get; set; }

        public Nullable<int> Width { get; set; }

        public Nullable<int> Height { get; set; }

        public string Position { get; set; }

        public Nullable<int> WidgetDataTypeID { get; set; }

        public Nullable<int> WidgetDataID { get; set; }
    }
}
