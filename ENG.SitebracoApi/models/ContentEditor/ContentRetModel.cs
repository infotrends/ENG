using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using SitebracoApi.Controllers.Meta;
using SitebracoApi.Models.Meta;
using umbraco.NodeFactory;
using Umbraco.Core.Models;

namespace SitebracoApi.Models.ContentEditor
{
    public class ContentRetModel
    {
        public int? Id { get; set; }
        public string Guid { get; set; }

        public string Type { get; set; }
        public string Title { get; set; }
        public DateTime? PubDate { get; set; }
        public double? Price { get; set; }
        public DateTime? AvailableForPurchaseDate { get; set; }
        public DateTime? CreateDate { get; set; }

        public List<string> Authors { get; set; }
        public List<string> Keywords { get; set; }
        public List<string> Company { get; set; }
        public List<string> TopicSegments { get; set; }

        public string Availability { get; set; }
        public string PriceType { get; set; }

        public string Abstract { get; set; }
        public string SourceFile { get; set; }
        public string SourceFileType { get; set; }

        public bool? IsPublished { get; set; }

        public List<ContentSpinDoctor> MetaClass { get; set; }
        public List<ContentSpinDoctor> MetaClient { get; set; }
        public List<ContentSpinDoctor> MetaFocus { get; set; }
        public List<ContentSpinDoctor> MetaIndustry { get; set; }
        public List<ContentSpinDoctor> MetaMarket { get; set; }
        public List<ContentSpinDoctor> MetaPrimary { get; set; }
        public List<ContentSpinDoctor> MetaRegion { get; set; }
        public List<ContentSpinDoctor> MetaRole { get; set; }
        public List<ContentSpinDoctor> MetaTopic { get; set; }
        

        /* Custom */
        public int? SourceFileLength { get; set; }
        public string DownloadUrl { get; set; }
        public string Link { get; set; }


        /// <summary>
        /// Constructor
        /// </summary>
        public ContentRetModel()
        {
            Authors = new List<string>();
            Keywords = new List<string>();
            Company = new List<string>();
            TopicSegments = new List<string>();

            MetaClass = new List<ContentSpinDoctor>();
            MetaClient = new List<ContentSpinDoctor>();
            MetaFocus = new List<ContentSpinDoctor>();
            MetaIndustry = new List<ContentSpinDoctor>();
            MetaMarket = new List<ContentSpinDoctor>();
            MetaPrimary = new List<ContentSpinDoctor>();
            MetaRegion = new List<ContentSpinDoctor>();
            MetaRole = new List<ContentSpinDoctor>();
            MetaTopic = new List<ContentSpinDoctor>();
        }


        /* Statics */
        public static ContentRetModel TransferSingle(Node o)
        {
            var d = Extlib.ConvertFromUmbracoModel<UmbracoDoctypes.Content>(o);
            var isPublished = true;
            return TransferSingle(d, isPublished);
        }
        public static ContentRetModel TransferSingle(IContent o)
        {
            var d = Extlib.ConvertFromUmbracoModel<UmbracoDoctypes.Content>(o);
            var isPublished = o.Published;
            return TransferSingle(d, isPublished);
        }
        public static ContentRetModel TransferSingle(UmbracoDoctypes.Content o, bool isPublished)
        {
            var sourceFile = o.content_sourceFile;
            if (o.content_sourceFileTypeEnum != Types.ContentEditorTemplateType.RichText)
                sourceFile = null;

            
            if (_ContentMetaController == null)
                _ContentMetaController = new ContentMetaController();


            var authors = new List<string>();
            if (!o.contentBase_authors.IsNullOrWhiteSpace())
            {
                var sp = o.contentBase_authors.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
                authors = new List<string>(sp);
            }


            var keywords = new List<string>();
            if (!o.contentBase_keywords.IsNullOrWhiteSpace())
            {
                var sp = o.contentBase_keywords.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
                keywords = new List<string>(sp);
            }

            var company = new List<string>();
            if (!o.contentBase_company.IsNullOrWhiteSpace())
            {
                var sp = o.contentBase_company.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
                company = new List<string>(sp);
            }

            var topicSegments = new List<string>();
            if (!o.content_topicSegments.IsNullOrWhiteSpace())
            {
                var sp = o.content_topicSegments.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
                topicSegments = new List<string>(sp);
            }


            var contentMetaController = new ContentMetaController();
            var iRet = new ContentRetModel
            {
                Abstract = o.content_abstract,
                Authors = authors,
                AvailableForPurchaseDate = o.content_availableForPurchaseDate,
                Availability = o.contentBase_availability,
                Company = company,
                CreateDate = o.CreateDate,

                Guid = o.contentBase_guid,
                Id = o.Id,

                IsPublished = isPublished,
                Keywords = keywords,
                Price = o.content_price,
                PriceType = o.contentBase_priceType,
                PubDate = o.content_pubDate,

                SourceFile = sourceFile,
                SourceFileLength = o.content_sourceFile.IsNullOrWhiteSpace() ? 0 : o.content_sourceFile.Length,
                SourceFileType = o.content_sourceFileType,
                Title = o.content_title,
                TopicSegments = topicSegments,
                Type = o.contentBase_type,

                /* Meta */
                MetaClass = ParseSpinDoctor(o.content_spindoctor_class, _ContentMetaController.ListClassAttributes),
                MetaClient = ParseSpinDoctor(o.content_spindoctor_client, _ContentMetaController.ListClientAttributes),
                MetaFocus = ParseSpinDoctor(o.content_spindoctor_focus, _ContentMetaController.ListFocusAttributes),
                MetaIndustry = ParseSpinDoctor(o.content_spindoctor_industry, _ContentMetaController.ListIndustryAttributes),
                MetaMarket = ParseSpinDoctor(o.content_spindoctor_market, _ContentMetaController.ListMarketAttributes),
                MetaPrimary = ParseSpinDoctor(o.content_spindoctor_primary, _ContentMetaController.ListPrimaryAttributes),
                MetaRegion = ParseSpinDoctor(o.content_spindoctor_region, _ContentMetaController.ListRegionAttributes),
                MetaRole = ParseSpinDoctor(o.content_spindoctor_role, _ContentMetaController.ListRoleAttributes),
                MetaTopic = ParseSpinDoctor(o.content_spindoctor_topic, _ContentMetaController.ListTopicAttributes),

                /* Custom */
                DownloadUrl = Utility.Content.ConstructDownloadUrl((int)o.Id),
                Link = Utility.Content.ConstructLinkUrl((int)o.Id)
                
            };

            return iRet;
        }
        public static List<ContentRetModel> Transfer(List<Node> nodes)
        {
            var ret = new List<ContentRetModel>();
            foreach (var item in nodes)
            {
                var o = TransferSingle(item);
                ret.Add(o);
            }
            return ret;
        }
        public static List<ContentRetModel> Transfer(List<IContent> contents)
        {
            var ret = new List<ContentRetModel>();
            foreach (var item in contents)
            {
                var o = TransferSingle(item);
                ret.Add(o);
            }
            return ret;
        }

        private static ContentMetaController _ContentMetaController;

        /// <summary>
        /// Parse spin doctor
        /// </summary>
        /// <param name="spinDoctorData">Spin doctor data format string</param>
        /// <param name="metaListFunction">The function which will help to identify the name of the attribute</param>
        /// <returns></returns>
        private static List<ContentSpinDoctor> ParseSpinDoctor(string spinDoctorData, Func<List<ContentMetaModel>> metaListFunction)
        {
            var dict = Parse(spinDoctorData);
            var metaList = metaListFunction();
            var ret = new List<ContentSpinDoctor>();


            foreach (var meta in metaList)
            {
                var val = (int)0;
                if (!dict.TryGetValue(meta.Id, out val)) val = 0;
                var o = new ContentSpinDoctor { Id = meta.Id, Name = meta.Name, Value = val };
                ret.Add(o);
            }


            // Return
            return ret
                .OrderBy(x => x.Name)
                .ToList()
                ;
        }
        private static Dictionary<int, int> Parse(string spinDoctorData)
        {
            /* 
             * Parse example: SpindoctorAttributeComponents|6:0,7:0,8:0,9:0,10:0
             */

            var ret = new Dictionary<int, int>();

            if (spinDoctorData.IsNullOrWhiteSpace())
                return ret;


            var sp1 = spinDoctorData.Split('|')[1];
            var sp2 = sp1.Split(',');


            foreach (var item in sp2)
            {
                var sp3 = item.Split(':');

                int id;
                int val;

                if (!int.TryParse(sp3[0], out id)) id = 0;
                if (!int.TryParse(sp3[1], out val)) val = 0;

                ret[id] = val;
            }


            // Return
            return ret;
        }


    }
}
