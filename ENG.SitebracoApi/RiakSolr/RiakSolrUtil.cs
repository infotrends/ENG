using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CorrugatedIron;

namespace SitebracoApi.RiakSolr
{
    public class RiakSolrUtil
    {
        public static IRiakClient CreateRiakClient()
        {
            var configName = ObjectUtil.GetPropertyName<Constant.RiakSolr.ConfigSection>(x => x.riakSolrConfig);
            var client = MyRiak.RiakHelper.CreateClient(configName);
            return client;
        }


        /// <summary>
        /// Replace all spaces with underscores
        /// </summary>
        /// <param name="input">List of string to be replaced</param>
        /// <returns></returns>
        public static List<string> ReplaceSpaceWithUnderscore(List<string> input)
        {
            var output = new List<string>();

            if (input != null)
            {
                foreach (var item in input)
                {
                    var t = ReplaceSpaceWithUnderscore(item);
                    output.Add(t);
                }
            }

            return output;
        }


        /// <summary>
        /// Replace all spaces with underscores
        /// </summary>
        /// <param name="input">Input to be replaced</param>
        /// <returns></returns>
        public static string ReplaceSpaceWithUnderscore(string input)
        {
            var output = input;
            if (!string.IsNullOrEmpty(input))
                output = input.Replace(" ", "_");
            return output;
        }



        /// <summary>
        /// Serialize taxonomy
        /// </summary>
        /// <param name="category">Category of taxonomy (Primary, Class, Industry, etc...)</param>
        /// <param name="taxonomy">Taxonomy to be serialize</param>
        /// <returns></returns>
        public static string SerializeTaxonomy(string category, string taxonomy)
        {
            var categoryTransformed = "";
            if (!category.IsNullOrWhiteSpace())
            {
                var categorySp = category.Trim().Split(new[] { " " }, StringSplitOptions.RemoveEmptyEntries);
                categoryTransformed = new List<string>(categorySp).JoinString("_");
            }

            var taxonomyTransformed = "";
            if (!taxonomy.IsNullOrWhiteSpace())
            {
                var taxonomySp = taxonomy.Split(new[] { " " }, StringSplitOptions.RemoveEmptyEntries);
                taxonomyTransformed = new List<string>(taxonomySp).JoinString("_");
            }

            return "{0}__{1}"
                .Fmt(categoryTransformed, taxonomyTransformed);

        }


    }
}
