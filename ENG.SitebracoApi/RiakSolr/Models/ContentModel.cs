using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace SitebracoApi.RiakSolr.Models
{
    public class ContentModel : RiakSolrBase
    {
        /// <summary>
        /// Id of eprise document (legacy).
        ///  This is only used to plugin the new search with old website interface.
        ///  Should be removed after completely transitted to Umbraco.
        /// </summary>
        public int? LegacyId_i { get; set; }


        /// <summary>
        /// Abstract
        ///  case-insensitive
        /// </summary>
        public string Abstract_tsd { get; set; }


        /// <summary>
        /// Multi-valued field 
        ///  with case-insensitive
        /// </summary>
        public List<string> Authors_tssd { get; set; }


        /// <summary>
        /// Automatically populate.
        /// Multi-valued field 
        ///  with case-insensitive
        /// </summary>
        public List<string> AuthorsFacet_tssd
        {
            get { return RiakSolrUtil.ReplaceSpaceWithUnderscore(Authors_tssd); }
            set { }
        }



        /// <summary>
        /// Available for purchase if not null
        /// </summary>
        public DateTime? AvailableForPurchaseDate_dt { get; set; }



        /// <summary>
        /// Content (raw content to be indexed)
        ///  case-insensitive
        /// </summary>
        public string Content_tsd { get; set; }



        /// <summary>
        /// Description about the item (different from the abstract)
        ///  case-insensitive
        /// </summary>
        public string ItemDescription_tsd { get; set; }


        /// <summary>
        /// Multi-valued field
        ///  with case-insensitive
        /// </summary>
        public List<string> Keywords_tssd { get; set; }


        /// <summary>
        /// Automatically populate.
        /// Multi-valued field 
        ///  with case-insensitive
        /// </summary>
        public List<string> KeywordsFacet_tssd
        {
            get { return RiakSolrUtil.ReplaceSpaceWithUnderscore(Keywords_tssd); }
            set { }
        }


        /// <summary>
        /// Publish date
        /// </summary>
        public DateTime? PubDate_dt { get; set; }


        /// <summary>
        /// Multi-valued field
        ///  with case-insensitive
        /// </summary>
        public List<string> TaxonomyFacet_tssd { get; set; }


        /// <summary>
        /// Title
        ///  case-insensitive
        /// </summary>
        public string Title_tsd { get; set; }


        /// <summary>
        /// Type (Blog, News, Research, etc)
        ///  case-insensitive
        /// </summary>
        public string Type_tsd { get; set; }


        /// <summary>
        /// Automatically populate
        ///  case-insensitive
        /// </summary>
        public string TypeFacet_t
        {
            get { return RiakSolrUtil.ReplaceSpaceWithUnderscore(Type_tsd); }
            set { }
        }
        

        /// <summary>
        /// Constructor
        /// </summary>
        public ContentModel()
        {
            _ExtraProperties = new Dictionary<string, object>();
            TaxonomyFacet_tssd = new List<string>();
        }


        /// <summary>
        /// Contains extra properties
        /// </summary>
        Dictionary<string, object> _ExtraProperties { get; set; }


        /// <summary>
        /// Set extra property to SolrDocument and add to TaxonomyFacets
        /// </summary>
        /// <param name="propertyName">Name of property to add</param>
        /// <param name="value">Property value</param>
        public void SetTaxonomyProperty(string propertyName, int value)
        {
            SetTaxonomyProperty(propertyName, value, true);
        }


        /// <summary>
        /// Add extra property to SolrDocument
        /// </summary>
        /// <param name="propertyName">Name of property to add</param>
        /// <param name="value">Property value</param>
        /// <param name="isTaxonomy">Whether to add to Taxonomy facets</param>
        public void SetTaxonomyProperty(string propertyName, int value, bool isTaxonomy)
        {
            if (value == 0)
                return;

            if (isTaxonomy)
                TaxonomyFacet_tssd.Add(propertyName.ToLower());

            var key = "{0}_i".Fmt(propertyName.ToLower());
            _ExtraProperties[key] = value;
        }


        /// <summary>
        /// Convert to SolrDictionary of all properties
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, object> ToSolrDictionary()
        {
            // Distinct the taxonomy list before transforming
            TaxonomyFacet_tssd = TaxonomyFacet_tssd.Distinct().ToList();


            // Serialize
            var json = this.ToJsonString(new Newtonsoft.Json.Converters.IsoDateTimeConverter());
            var dict = json.ToObject<Dictionary<string, object>>(new Newtonsoft.Json.Converters.IsoDateTimeConverter());


            // Add extra properties
            foreach (var item in _ExtraProperties)
            {
                if (item.Value != null)
                    dict[item.Key] = item.Value;
            }


            // Remove null from the collection
            for (var i = dict.Keys.Count - 1; i >= 0; i--)
            {
                var key = dict.Keys.ElementAt(i);
                var val = dict[key];

                if (val == null)
                {
                    dict.Remove(key);
                }
                else
                {
                    if (val is JArray)
                    {
                        dict[key] = val.ToJsonString().ToObject<List<string>>();
                    }
                }
            }

            // Return
            return dict;
        }



    }

}
