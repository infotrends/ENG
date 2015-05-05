using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SitebracoApi.Models.Territory
{
    public class BoundaryParams
    {
        public List<int> layersId { get; set; }

        public string layerType { get; set; }

        public List<string> nutsId { get; set; }

        public List<int> canId { get; set; }
    }
}
