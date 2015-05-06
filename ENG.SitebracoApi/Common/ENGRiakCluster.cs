using CorrugatedIron;
using CorrugatedIron.Comms;
using MyRiak;

namespace SitebracoApi.Common
{
    public class EngRiakCluster
    {
        public static string GetAvailableUrl()
        {
            var cluster = (RiakCluster)RiakHelper.GetCluster(ObjectUtil.GetPropertyName<Constant.RiakSolr.ConfigSection>(x => x.riakSolrConfig));

            var node = (RiakNode)cluster.SelectNode();
            var nodeUrlList = node.GetRestRootUrl();
            var availabelUrl = nodeUrlList[0];

            return availabelUrl;
        }
    }
}
