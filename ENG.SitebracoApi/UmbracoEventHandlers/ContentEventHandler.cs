using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CorrugatedIron.Models;
using SitebracoApi.Models.ContentEditor;
using umbraco.BusinessLogic;
using Umbraco.Core;
using Umbraco.Core.Logging;
using Umbraco.Core.Models;
using Umbraco.Core.Services;

namespace SitebracoApi.UmbracoEventHandlers
{
    public class ContentEventHandler : ApplicationEventHandler
    {
        /// <summary>
        /// On Application Started
        /// </summary>
        /// <param name="umbracoApplication"></param>
        /// <param name="applicationContext"></param>
        protected override void ApplicationStarted(UmbracoApplicationBase umbracoApplication, ApplicationContext applicationContext)
        {
            // Init base
            base.ApplicationStarted(umbracoApplication, applicationContext);

            // Add events on saving
            ContentService.Saving += ContentService_Saving;
            ContentService.Published += ContentService_Published;
            ContentService.UnPublished += ContentService_UnPublished;
        }


        


        /// <summary>
        /// On Content Published
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void ContentService_Published(Umbraco.Core.Publishing.IPublishingStrategy sender, Umbraco.Core.Events.PublishEventArgs<IContent> e)
        {
            var validContentList = FilterValidContents(e.PublishedEntities);

            // Base case
            if (validContentList.Count == 0)
                return;

            // Put into solr
            PutIntoSolr(validContentList);
            
        }


        /// <summary>
        /// On Content UnPublished
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void ContentService_UnPublished(Umbraco.Core.Publishing.IPublishingStrategy sender, Umbraco.Core.Events.PublishEventArgs<IContent> e)
        {
            var validContentList = FilterValidContents(e.PublishedEntities);

            // Base case
            if (validContentList.Count == 0)
                return;

            // Remove from solr
            RemoveFromSolr(validContentList);
        }


        /// <summary>
        /// On Content Saving
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void ContentService_Saving(IContentService sender, Umbraco.Core.Events.SaveEventArgs<IContent> e)
        {
            foreach (var content in e.SavedEntities)
            {
                bool isContent = Utility.Content.ValidateContentNode(content, false);
                if (isContent == false) continue;
                SetNodeName(content);
                SetNodeGuid(content);
            }
        }


        /// <summary>
        /// Set node name
        /// </summary>
        /// <param name="content"></param>
        void SetNodeName(IContent content)
        {
            // Node name
            var title = content.GetValue(ObjectUtil.GetPropertyName<UmbracoDoctypes.Content>(x => x.content_title), "");

            if (title.Length == 0)
            {
                var msg = "Title cannot be empty";
                Log.Add(LogTypes.Error, content.Id, msg);
                throw new Exception(msg);
            }

            var nodeNameVal = title.Substring(0, title.Length > Constant.ContentNodeNameMaxLength ? Constant.ContentNodeNameMaxLength : title.Length);
            content.Name = nodeNameVal;
        }


        /// <summary>
        /// Set node guid
        /// </summary>
        /// <param name="content"></param>
        void SetNodeGuid(IContent content)
        {
            var guidKey = ObjectUtil.GetPropertyName<UmbracoDoctypes.Content>(x => x.contentBase_guid);
            var guid = content.GetValue<Guid>(guidKey, Guid.Empty);
            if (guid == Guid.Empty)
            {
                guid = Guid.NewGuid();
                content.SetValue(guidKey, guid.ToString());
            }
        }


        /// <summary>
        /// Put this document into solr index
        /// </summary>
        /// <param name="content"></param>
        void PutIntoSolr(List<IContent> contentList)
        {
            var bucketType = ObjectUtil.GetPropertyName<Constant.RiakSolr.BucketType>(x => x.Solr);
            var bucketName = ObjectUtil.GetClassName<SitebracoApi.RiakSolr.Models.ContentModel>();

            var list = new List<RiakObject>();
            
            foreach (var item in contentList)
            {
                var o = ConvertToRiakSolrContent(item);
                var riakObjId = new RiakObjectId(bucketType, bucketName, o.Id_s);
                var riakObj = new RiakObject(riakObjId, o.ToSolrDictionary());
                list.Add(riakObj);
            }

            //System.IO.File.WriteAllText("c:\\inetpub\\sitebraco\\_sinh\\solr.txt", l.ToJsonString());

            var client = SitebracoApi.RiakSolr.RiakSolrUtil.CreateRiakClient();
            client.Put(list);
        }


        /// <summary>
        /// Remove document from solr index
        /// </summary>
        /// <param name="content"></param>
        void RemoveFromSolr(List<IContent> contentList)
        {
            var bucketType = ObjectUtil.GetPropertyName<Constant.RiakSolr.BucketType>(x => x.Solr);
            var bucketName = ObjectUtil.GetClassName<SitebracoApi.RiakSolr.Models.ContentModel>();

            var list = new List<RiakObjectId>();

            var guidKey = ObjectUtil.GetPropertyName<UmbracoDoctypes.Content>(x => x.contentBase_guid);

            foreach (var item in contentList)
            {
                var guid = item.GetValue<Guid>(guidKey, Guid.Empty);
                if (guid == Guid.Empty) continue;
                var riakObjId = new RiakObjectId(bucketType, bucketName, guid.ToString());
                list.Add(riakObjId);
            }

            if (list.Count > 0)
            {
                var client = SitebracoApi.RiakSolr.RiakSolrUtil.CreateRiakClient();
                client.Delete(list);
            }
        }


        /// <summary>
        /// Convert to solr document
        /// </summary>
        /// <param name="content">Content to be converted</param>
        /// <returns></returns>
        SitebracoApi.RiakSolr.Models.ContentModel ConvertToRiakSolrContent(IContent content)
        {
            // Auto extracted content (raw content to index )
            var autoExtractedContentKey = ObjectUtil.GetPropertyName<UmbracoDoctypes.Content>(x => x.content_autoExtractedContent);
            var autoExtractedContent = content.GetValue<string>(autoExtractedContentKey, null);

            
            // Legacy id
            var legacyIdKey = ObjectUtil.GetPropertyName<UmbracoDoctypes.Content>(x => x.contentBase_legacyObjectId);
            var legacyId = (int?)null;
            legacyId = content.GetValue<int>(legacyIdKey, 0);
            if (legacyId == 0) legacyId = null;


            // Convert
            var t = ContentRetModel.TransferSingle(content);


            // Constructor solr document
            var o = new SitebracoApi.RiakSolr.Models.ContentModel
            {
                Abstract_tsd = t.Abstract,
                Authors_tssd = t.Authors,
                AvailableForPurchaseDate_dt = t.AvailableForPurchaseDate,
                Content_tsd = autoExtractedContent,
                ContentGuid_tsd = t.Guid,
                ContentId_i = content.Id,
                Id_s = t.Guid,
                ItemDescription_tsd = "",
                Keywords_tssd = t.Keywords,
                LegacyId_i = legacyId,
                PubDate_dt = t.PubDate,
                Title_tsd = t.Title,
                Type_tsd = t.Type
            };


            // Add taxonomy
            AddTaxonomy(o, t.MetaClass, "Class");
            AddTaxonomy(o, t.MetaIndustry, "Industry");
            AddTaxonomy(o, t.MetaMarket, "Market");
            AddTaxonomy(o, t.MetaPrimary, "Primary");
            AddTaxonomy(o, t.MetaRegion, "Region");
            AddTaxonomy(o, t.MetaRole, "Role");
            AddTaxonomy(o, t.MetaTopic, "Topic");


            // Return
            return o; 
        }


        /// <summary>
        /// Add taxonomy to solr document
        /// </summary>
        /// <param name="addToDocument"></param>
        /// <param name="metas"></param>
        /// <param name="category"></param>
        void AddTaxonomy(SitebracoApi.RiakSolr.Models.ContentModel addToDocument, List<ContentSpinDoctor> metas, string category)
        {
            foreach (var item in metas)
            {
                var key = SitebracoApi.RiakSolr.RiakSolrUtil.SerializeTaxonomy(category, item.Name);
                var val = item.Value == null ? 0 : (int)item.Value;
                addToDocument.SetTaxonomyProperty(key, val);
            }
        }


        /// <summary>
        /// Filter only valid contents
        /// </summary>
        /// <param name="entities">Raw entities to be filtered</param>
        /// <returns></returns>
        List<IContent> FilterValidContents(IEnumerable<IContent> entities)
        {
            var validContentList = new List<IContent>();

            // Only allow valid content
            foreach (var content in entities)
            {
                bool isContent = Utility.Content.ValidateContentNode(content, false);
                if (isContent == true) validContentList.Add(content); ;
            }

            return validContentList;
        }

    }
}
