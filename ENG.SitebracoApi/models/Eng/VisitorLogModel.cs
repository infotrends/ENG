using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SitebracoApi.Models.Eng
{
    public class VisitorLogModel : EngBase
    {
        public string type_tsd { get; set; }

        public string element_tsd { get; set; }

        public string parent_tsd { get; set; }

        public string elementName_tsd { get; set; }

        public string elementHtml_tsd { get; set; }
    }
}
