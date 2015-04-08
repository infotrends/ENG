using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SitebracoApi.Models.Auth
{
    public class MemberProfileModel
    {
        public MemberModel UserProfile { get; set; }

        public List<BaseProductModel> Products { get; set; }

        public string SessionKey { get; set; }

        public string CurrentAction { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        public MemberProfileModel()
        {
            Products = new List<BaseProductModel>();
        }

    }
}
