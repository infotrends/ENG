using CorrugatedIron;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SitebracoApi.Models.Eng
{
    public abstract class EngBase : MyRiak.Models.Base
    {
        public override string GetBucketType()
        {
            return Constant.EngRiakConfig.BUKET_TYPE;
        }

        public override IRiakClient GetRiakClient()
        {
            var cluster = RiakCluster.FromConfig(Constant.EngRiakConfig.SOLR_CONFIG);
            return cluster.CreateClient();
        }
    }
}
