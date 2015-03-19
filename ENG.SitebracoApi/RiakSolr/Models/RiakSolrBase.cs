using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CorrugatedIron;

namespace SitebracoApi.RiakSolr
{
    public abstract class RiakSolrBase : MyRiak.Models.Base
    {
        public string ContentGuid_tsd { get; set; }
        public int ContentId_i{ get; set; }


        //public override string GetBucketType()
        //{
        //    return ObjectUtil.GetPropertyName<Constant.RiakSolr.BucketType>(x => x.Solr);
        //}

        //public override IRiakClient GetRiakClient()
        //{
        //    return RiakSolrUtil.CreateRiakClient();
        //}

        public override bool Destroy(CorrugatedIron.Models.RiakDeleteOptions options = null)
        {
            throw new NotImplementedException();
        }
        public override bool Save(CorrugatedIron.Models.RiakPutOptions options = null)
        {
            throw new NotImplementedException();
        }

        

    }
}
