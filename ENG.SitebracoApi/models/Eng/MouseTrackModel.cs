using MyUtils.Validations;
using SitebracoApi.Models.Eng;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SitebracoApi.Models.Eng
{
    public class MouseTrackModel : EngBase
    {
        [Required]
        public string ClientId_s { get; set; }

        [Required]
        public string PageUrl_tsd { get; set; }

        [Required]
        public int PageX_i { get; set; }

        [Required]
        public int PageY_i { get; set; }

        [Required]
        public int Point_i { get; set; }

        public string Position_s { get; set; }

        public string TargetName_tsd { get; set; }

        public string TargetClassName_tsd { get; set; }

        public string TargetID_tsd { get; set; }

        public int WindowW_i { get; set; }

        public int WindowH_i { get; set; }

        public int ScreenW_i { get; set; }

        public int ScreenH_i { get; set; }

        [Required]
        public string ActionName_s { get; set; }

        public string IPAddress_s { get; set; }
    }
}
