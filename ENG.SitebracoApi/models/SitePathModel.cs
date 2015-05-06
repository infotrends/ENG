using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SitebracoApi.Models.Eng;

namespace SitebracoApi.Models
{
    public class SitePathModel : EngBase
    {
        /// <summary>
        /// Client ID
        /// </summary>
        public string ClientId_s { get; set; }

        /// <summary>
        /// Site Path URL
        /// </summary>
        public string SitePath_s { get; set; }

        /// <summary>
        /// Viewer ID
        /// </summary>
        public string ViewerID_s { get; set; }

        /// <summary>
        /// Session ID
        /// </summary>
        public string SessionID_s { get; set; }

        /// <summary>
        /// IP Address
        /// </summary>
        public string IPAddress_s { get; set; }
    }
}
