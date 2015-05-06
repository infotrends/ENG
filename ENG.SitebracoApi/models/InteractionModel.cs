using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SitebracoApi.Models.Eng;

namespace SitebracoApi.Models
{
    public class InteractionModel : EngBase
    {
        public string ClientId_s { get; set; }

        public string ViewerID_s { get; set; }

        public string FormData_tsd { get; set; }

    }
}
