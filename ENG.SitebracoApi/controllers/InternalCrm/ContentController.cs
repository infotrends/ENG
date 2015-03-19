using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

using SitebracoApi.DbEntities;
using SitebracoApi.Models;
using SitebracoApi.Models.ContentEditor;
using SitebracoApi.Services;
using SitebracoApi.Types;

using Umbraco.Web.Mvc;
using Umbraco.Core;
using Umbraco.Core.Models;
using umbraco.NodeFactory;
using MyUtils.Validations;


namespace SitebracoApi.Controllers.InternalCrm
{

    [PluginController(Constant.SitebracoApiModule.InternalCrm)]
    public class ContentController : BaseController
    {

        [HttpPost]
        public ContentRetModel Create([FromBody]CreateParam p)
        {
            try
            {
                ValidateContextAccessToken();
                ValidateParams(p);

                // Get structural folder id
                var folderId = Utility.Content.GetStructuralFolder(DateTime.Now);

                // Create content
                var shortenName = p.Title.Substring(0, p.Title.Length > Constant.ContentNodeNameMaxLength ? Constant.ContentNodeNameMaxLength : p.Title.Length);
                IContent content = Services.ContentService.CreateContent(shortenName, folderId, ObjectUtil.GetClassName<UmbracoDoctypes.Content>());

                // Set properties
                if (!p.Abstract.IsNullOrWhiteSpace()) content.SetValue(ObjectUtil.GetPropertyName<UmbracoDoctypes.Content>(x => x.content_abstract), p.Abstract);
                if (!p.Authors.IsNullOrWhiteSpace()) content.SetValue(ObjectUtil.GetPropertyName<UmbracoDoctypes.Content>(x => x.contentBase_authors), p.Authors);
                content.SetValue(ObjectUtil.GetPropertyName<UmbracoDoctypes.Content>(x => x.contentBase_type), p.ContentType);
                if (!p.Keywords.IsNullOrWhiteSpace()) content.SetValue(ObjectUtil.GetPropertyName<UmbracoDoctypes.Content>(x => x.contentBase_keywords), p.Keywords);
                content.SetValue(ObjectUtil.GetPropertyName<UmbracoDoctypes.Content>(x => x.content_title), p.Title);

                // Save
                Services.ContentService.Save(content);

                // Return
                var ret = ContentRetModel.TransferSingle(content);
                return ret;
            }
            catch (Exception e)
            {
                var msg = Request.CreateErrorResponse(HttpStatusCode.BadRequest, e.Message);
                throw new HttpResponseException(msg);
            }
        }
        public class CreateParam
        {
            [Required]
            public string Title { get; set; }

            [Required]
            public string ContentType { get; set; }

            public string Abstract { get; set; }
            public string Authors { get; set; }
            public string Keywords { get; set; }
        }


        [HttpPost]
        public SimpleObjectModel<bool> Delete([FromBody]DeleteParam p)
        {
            try
            {
                ValidateContextAccessToken();
                ValidateParams(p);

                var contentId = (int)p.ContentId;
                var content = Services.ContentService.GetById(contentId);
                Utility.Content.ValidateContentNode(content);

                // Publish
                Services.ContentService.Delete(content);
                return new SimpleObjectModel<bool>(true);
            }
            catch (Exception e)
            {
                var error = Request.CreateErrorResponse(HttpStatusCode.BadRequest, e.Message);
                throw new HttpResponseException(error);
            }
        }
        public class DeleteParam
        {
            [RequiredNumberGreaterThanZero]
            public int? ContentId { get; set; }
        }


        [HttpGet]
        public ContentRetModel Get([FromUri]GetParam p)
        {
            try
            {
                ValidateContextAccessToken();
                ValidateParams(p);

                var contentId = (int)p.ContentId;
                var content = Services.ContentService.GetById(contentId);
                Utility.Content.ValidateContentNode(content);

                // Return
                var ret = ContentRetModel.TransferSingle(content);
                return ret;
            }
            catch (Exception e)
            {
                var msg = Request.CreateErrorResponse(HttpStatusCode.BadRequest, e.Message);
                throw new HttpResponseException(msg);
            }
        }
        public class GetParam
        {
            [RequiredNumberGreaterThanZero]
            public int? ContentId { get; set; }
        }


        [HttpPost]
        public SimpleObjectModel<bool> Publish([FromBody]PublishParam p)
        {
            try
            {
                ValidateContextAccessToken();
                ValidateParams(p);

                var contentId = (int)p.ContentId;
                var content = Services.ContentService.GetById(contentId);
                Utility.Content.ValidateContentNode(content);

                // Save
                Services.ContentService.PublishWithStatus(content);

                // Return
                var ret = new SimpleObjectModel<bool>(true);
                return ret;
            }
            catch (Exception e)
            {
                var msg = Request.CreateErrorResponse(HttpStatusCode.BadRequest, e.Message);
                throw new HttpResponseException(msg);
            }
        }
        public class PublishParam
        {
            [RequiredNumberGreaterThanZero]
            public int? ContentId { get; set; }
        }


        [HttpPost]
        public SimpleObjectModel<bool> PublishDeep([FromBody]PublishDeepParam p)
        {
            try
            {
                ValidateContextAccessToken();
                ValidateParams(p);

                var contentId = (int)p.ContentId;
                var content = Services.ContentService.GetById(contentId);
                Utility.Content.ValidateContentNode(content);

                // Save
                Services.ContentService.PublishWithChildrenWithStatus(content, 0, true);

                // Return
                var ret = new SimpleObjectModel<bool>(true);
                return ret;
            }
            catch (Exception e)
            {
                var msg = Request.CreateErrorResponse(HttpStatusCode.BadRequest, e.Message);
                throw new HttpResponseException(msg);
            }
        }
        public class PublishDeepParam
        {
            [RequiredNumberGreaterThanZero]
            public int? ContentId { get; set; }
        }


        [HttpPost]
        public SimpleObjectModel<bool> UnPublish([FromBody]UnPublishParam p)
        {
            try
            {
                ValidateContextAccessToken();
                ValidateParams(p);

                var contentId = (int)p.ContentId;
                var content = Services.ContentService.GetById(contentId);
                Utility.Content.ValidateContentNode(content);

                // Save
                Services.ContentService.UnPublish(content);

                // Return
                var ret = new SimpleObjectModel<bool>(true);
                return ret;
            }
            catch (Exception e)
            {
                var msg = Request.CreateErrorResponse(HttpStatusCode.BadRequest, e.Message);
                throw new HttpResponseException(msg);
            }
        }
        public class UnPublishParam
        {
            [RequiredNumberGreaterThanZero]
            public int? ContentId { get; set; }
        }



        [HttpPost]
        public ContentRetModel Update([FromBody]UpdateParam p)
        {
            try
            {
                ValidateContextAccessToken();
                ValidateParams(p);

                var contentId = (int)p.ContentId;
                var content = Services.ContentService.GetById(contentId);
                Utility.Content.ValidateContentNode(content);

                // Set properties
                if (!p.Abstract.IsNullOrWhiteSpace()) content.SetValue(ObjectUtil.GetPropertyName<UmbracoDoctypes.Content>(x => x.content_abstract), p.Abstract);
                if (!p.Authors.IsNullOrWhiteSpace()) content.SetValue(ObjectUtil.GetPropertyName<UmbracoDoctypes.Content>(x => x.contentBase_authors), p.Authors);
                if (!p.Keywords.IsNullOrWhiteSpace()) content.SetValue(ObjectUtil.GetPropertyName<UmbracoDoctypes.Content>(x => x.contentBase_keywords), p.Keywords);
                content.SetValue(ObjectUtil.GetPropertyName<UmbracoDoctypes.Content>(x => x.content_title), p.Title);

                // Save
                Services.ContentService.Save(content);

                // Return
                var ret = ContentRetModel.TransferSingle(content);
                return ret;
            }
            catch (Exception e)
            {
                var msg = Request.CreateErrorResponse(HttpStatusCode.BadRequest, e.Message);
                throw new HttpResponseException(msg);
            }
        }
        public class UpdateParam
        {
            [RequiredNumberGreaterThanZero]
            public int? ContentId { get; set; }

            [Required]
            public string Title { get; set; }

            public string Abstract { get; set; }
            public string Authors { get; set; }
            public string Keywords { get; set; }
        }


        /// <summary>
        /// Validate whether access token using CrmAccessToken
        /// </summary>
        protected void ValidateContextAccessToken()
        {
            var keyName = ObjectUtil.GetPropertyName<Constant.WebConfig.App>(x => x.CrmAccessToken);
            var accessToken = SitebracoApiSettings.GetConfig().App.GetValue(keyName);
            ValidateContextAccessToken(accessToken);
        }


    }


}
