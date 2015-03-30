using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SitebracoApi.Models.Eng
{
    public class PageView
    {
        public DateTime Date { get; set; }

        public uint PageViews { get; set; }

        public uint UniqueViews { get; set; }
    }
}
