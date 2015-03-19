using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using MyUtils.Validations;
using SitebracoApi.Models;
using SitebracoApi.Models.ContentEditor;
using SitebracoApi.Types;
using umbraco.NodeFactory;
using Umbraco.Core.Models;
using Umbraco.Web.Mvc;

namespace SitebracoApi.Controllers.ContentEditor
{

    [PluginController(Constant.SitebracoApiModule.ContentEditor)]
    [Umbraco.Web.WebApi.MemberAuthorize(AllowGroup = Constant.SitebracoBasicRole.Employee)]
    public class ContentMediaController : BaseController
    {

        [HttpGet]
        public List<ContentMediaRetModel> ListAll([FromUri]ListAllParam p)
        {
            ValidateParams(p);
            
            if (p.IncludeData == null) p.IncludeData = false;
            var content = Services.ContentService.GetById((int)p.ContentId);
            if (content == null) throw new Exception("Invalid Content");
            
            // Find all medias
            var medias = content.GetDescendantOrSelfContents(null, false,
                ObjectUtil.GetClassName<UmbracoDoctypes.Media>());
            
            var ret = ContentMediaRetModel.Transfer(medias);

            // Return
            return ret;
        }
        public class ListAllParam
        {
            [Required]
            public int? ContentId { get; set; }

            public bool? IncludeData { get; set; }
        }


        [HttpPost]
        public ContentMediaRetModel Create([FromBody]CreateParam p)
        {
            ValidateParams(p);

            // Validate content
            var content = GetAndValidateContent((int)p.ContentId);
            
            // Retrieve media container
            var mediaFolderId = GetMediaFolderIdFrom(content);

            var segs = p.Data.Split(new string[] { ";base64," }, StringSplitOptions.RemoveEmptyEntries);
            var ext = Path.GetExtension(p.Filename);
            var contentType = segs[0].Replace("data:", "");
            var data64Base = segs[1];

            // Create media
            var mediaCt = ObjectUtil.GetClassName<UmbracoDoctypes.Media>();
            var media = Services.ContentService.CreateContent(p.Filename, mediaFolderId, mediaCt);
            Services.ContentService.SaveAndPublishWithStatus(media);

            // Convert to media model & set new value
            var mediaModel = Extlib.ConvertFromUmbracoModel<UmbracoDoctypes.Media>(media);
            mediaModel.media_contentType = contentType;
            mediaModel.media_extension = ext;
            mediaModel.media_sourceFile = data64Base;
            mediaModel.media_uploadedBy = p.UploadedBy.ToString();

            // Convert back to IContent to save
            media = Extlib.ConvertToUmbracoModel<IContent>(mediaModel, media);
            Services.ContentService.SaveAndPublishWithStatus(media);

            // Construct a return
            var ret = ContentMediaRetModel.TransferSingle(media, true);

            // Return
            return ret;

        }
        public class CreateParam
        {
            [RequiredNumberGreaterThanZero]
            public int? ContentId { get; set; }
            [Required]
            public string Filename { get; set; }
            [Required]
            public string Data { get; set; }
            [Required]
            public UploadedByType? UploadedBy { get; set; }
        }



        [HttpPost]
        public ContentMediaRetModel Duplicate([FromBody]DuplicateParam p)
        {
            ValidateParams(p);


            // Retrieve the media to be copied
            var mediaToDuplicate = GetAndValidateMedia((int)p.MediaId);
            var mediaToDuplicateModel = Extlib.ConvertFromUmbracoModel<UmbracoDoctypes.Media>(mediaToDuplicate);


            // Validate content
            var content = GetAndValidateContent((int)p.ContentId);


            // Retrieve media container
            var mediaFolderId = GetMediaFolderIdFrom(content);


            // Create new media
            var mediaCt = ObjectUtil.GetClassName<UmbracoDoctypes.Media>();
            var media = Services.ContentService.CreateContent(mediaToDuplicateModel.NodeName, mediaFolderId, mediaCt);
            Services.ContentService.SaveAndPublishWithStatus(media);


            // Set new value
            media = Extlib.ConvertToUmbracoModel<IContent>(mediaToDuplicateModel, media);
            Services.ContentService.SaveAndPublishWithStatus(media);


            // Construct a return
            var ret = ContentMediaRetModel.TransferSingle(media, true);

            // Return
            return ret;
            
        }
        public class DuplicateParam
        {
            [RequiredNumberGreaterThanZero]
            public int? ContentId { get; set; }
            [RequiredNumberGreaterThanZero]
            public int? MediaId { get; set; }
        }



        [HttpGet]
        public List<ContentMediaRetModel> Search([FromUri]SearchParam p)
        {
            ValidateParams(p);

            var contentSearchParam = new ContentController.SearchParam { Keyword = p.Keyword };
            var contentSearchResults = new ContentController().Search(contentSearchParam);

            var ret = new List<ContentMediaRetModel>();

            foreach (var result in contentSearchResults.Data)
            {
                var contentNode = new Node((int)result.Id);
                var medias = contentNode.GetDescendantOrSelfNodes(null, false, ObjectUtil.GetClassName<UmbracoDoctypes.Media>());
                var items = ContentMediaRetModel.Transfer(medias);
                ret.AddRange(items);
            }

            return ret;
        }
        public class SearchParam
        {
            [Required]
            public string Keyword { get; set; }
        }



        [HttpPost]
        public SimpleObjectModel<bool> Destroy([FromBody]DestroyParam p)
        {
            ValidateParams(p);

            var media = Services.ContentService.GetById((int)p.Id);
            if (media == null || media.ContentType.Alias != ObjectUtil.GetClassName<UmbracoDoctypes.Media>())
                throw new Exception("Invalid Media");

            var content = media.Parent().Parent();
            var rtf = content.GetValue<string>(ObjectUtil.GetPropertyName<UmbracoDoctypes.Content>(x => x.content_autoExtractedRichtext), "");

            var isReferenced = rtf.ToLower().IndexOf("itemid=" + media.Id) > -1;

            if (isReferenced)
            {
                throw new Exception("The item is currently referenced. Can't delete.");
            }

            Services.ContentService.Delete(media);

            return new SimpleObjectModel<bool>(true);
        }
        public class DestroyParam
        {
            [Required]
            public int? Id { get; set; }
        }



        private IContent GetAndValidateContent(int contentId)
        {
            // Validate content
            var content = Services.ContentService.GetById(contentId);
            if (content == null || content.ContentType.Alias != ObjectUtil.GetClassName<UmbracoDoctypes.Content>())
                throw new Exception("Invalid Content");

            return content;
        }
        private IContent GetAndValidateMedia(int mediaId)
        {
            // Validate content
            var media = Services.ContentService.GetById(mediaId);
            if (media == null || media.ContentType.Alias != ObjectUtil.GetClassName<UmbracoDoctypes.Media>())
                throw new Exception("Invalid Media");

            return media;
        }
        private int GetMediaFolderIdFrom(IContent content)
        {
            // Retrieve media container
            var mediaFolderId = (int?)null;
            var mediaContainerCt = ObjectUtil.GetClassName<UmbracoDoctypes.MediaContainer>();
            var searchResults = content.GetDescendantOrSelfContents(2, false, mediaContainerCt);
            if (searchResults.Count > 0) mediaFolderId = searchResults[0].Id;


            // Create folder if not yet existed
            if (mediaFolderId == null)
            {
                //var media = IContent
                var mediaFolder = Services.ContentService.CreateContent("Media", content.Id, mediaContainerCt);
                Services.ContentService.SaveAndPublishWithStatus(mediaFolder);
                mediaFolderId = mediaFolder.Id;
            }


            // Return
            return (int)mediaFolderId;
        }

    }


}
