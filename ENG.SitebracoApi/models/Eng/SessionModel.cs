using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SitebracoApi.Models.Eng
{
    public class SessionModel : EngBase
    {
        /// <summary>
        /// Client ID
        /// </summary>
        public string ClientId_s { get; set; }
        /// <summary>
        /// IP Address
        /// </summary>
        public string IPAddress_s { get; set; }
        /// <summary>
        /// Viewer Session ID
        /// </summary>
        public string ViewerID_s { get; set; }
        /// <summary>
        /// Session ID
        /// </summary>
        public string SessionID_s { get; set; }
    }
   
}
