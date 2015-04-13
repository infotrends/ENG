using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SitebracoApi.Models.Eng
{
    public class NotificationModel : EngBase
    {
        public string Title_s { get; set; }
        
        public string Message_tsd { get; set; }

        public bool Seen_b { get; set; }

        public string ClientId_s { get; set; }
    }
}
