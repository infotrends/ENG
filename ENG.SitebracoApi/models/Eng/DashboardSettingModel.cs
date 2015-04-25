using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SitebracoApi.Models.Eng
{
    public class DashboardSettingModel
    {
        public string ClientId { get; set; }

        public int Order { get; set; }

        public int Size { get; set; }

        public List<DashboardSettingReport> Reports { get; set; }
    }

    public class DashboardSettingReport
    {
        public int Order { get; set; }

        public string Name { get; set; }

        public bool Collapse { get; set; }
    }
}
