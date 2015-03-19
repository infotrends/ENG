using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SitebracoApi.UmbracoDoctypes
{
    public class Member
    {
        public string member_contactId { get; set; }
        public string member_smsRolecacheJson { get; set; }
        public string member_ugRolecacheJson { get; set; }
        public string member_resetPasswordGuid { get; set; }
        public string member_resetPasswordGuidExpiration { get; set; }

    }
}
