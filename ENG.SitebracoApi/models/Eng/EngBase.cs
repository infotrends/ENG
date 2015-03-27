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
            return ObjectUtil.GetPropertyName<Constant.RiakSolr.BucketType>(x=>x.InfoTrendsLog);
        }

        public override IRiakClient GetRiakClient()
        {
            var cluster = RiakCluster.FromConfig(ObjectUtil.GetPropertyName < Constant.RiakSolr.ConfigSection>(x=>x.riakSolrConfig));
            return cluster.CreateClient();
        }
    }
}
