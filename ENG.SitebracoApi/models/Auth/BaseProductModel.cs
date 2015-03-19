using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SitebracoApi.Types;

namespace SitebracoApi.Models.Auth
{
    public abstract class BaseProductModel
    {
        public InfoTrendsProductType Type { get; set; }
        public string TypeName
        {
            get
            {
                return Type.ToString();
            }
            set
            {
            }
        }
        
    }
}
