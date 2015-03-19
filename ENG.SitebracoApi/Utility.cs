using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using umbraco.NodeFactory;
using Umbraco.Core.Models;

namespace SitebracoApi
{
    public class Utility
    {
        public class Content
        {
            /// <summary>
            /// Construct Download Url for Content
            /// </summary>
            /// <param name="contentItemId">Content Id to construct the url</param>
            /// <returns></returns>
            public static string ConstructDownloadUrl(int contentItemId)
            {
                var handlerKey = ObjectUtil.GetPropertyName<Constant.WebConfig.App>(x => x.ContentHandler);
                var handlerUrl = SitebracoApiSettings.GetConfig().App.GetValue(handlerKey).TrimStart('/');

                var url = System.Web.HttpContext.Current.Request.Url;

                var tpl = "{0}://{1}{2}{3}/{4}?itemId={5}".Fmt(
                    url.Scheme,
                    url.Host,
                    url.Port != 80 ? ":" : "",
                    url.Port != 80 ? url.Port.ToString() : "",
                    handlerUrl,
                    contentItemId
                    );

                return tpl;
            }


            /// <summary>
            /// Construct Link Url for Content
            /// </summary>
            /// <param name="contentItemId">Content Id to construct the url</param>
            /// <returns></returns>
            public static string ConstructLinkUrl(int contentItemId)
            {
                var handlerKey = ObjectUtil.GetPropertyName<Constant.WebConfig.App>(x => x.ContentLinkHandler);
                var handlerUrl = SitebracoApiSettings.GetConfig().App.GetValue(handlerKey).TrimStart('/');

                var url = System.Web.HttpContext.Current.Request.Url;

                var tpl = "{0}://{1}{2}{3}/{4}?itemId={5}".Fmt(
                    url.Scheme,
                    url.Host,
                    url.Port != 80 ? ":" : "",
                    url.Port != 80 ? url.Port.ToString() : "",
                    handlerUrl,
                    contentItemId
                    );

                return tpl;
            }


            /// <summary>
            /// Get structural folder (year => month => day).
            /// Create if not exists.
            /// </summary>
            /// <param name="createDate">What date to create structural folder</param>
            /// <returns>Folder id of day folder</returns>
            public static int GetStructuralFolder(DateTime createDate)
            {
                var contentFolderId = SitebracoApiSettings.GetConfig().App.GetValue<int>(ObjectUtil.GetPropertyName<Constant.WebConfig.App>(x => x.UmbracoContentFolerId), 0);
                
                if (contentFolderId == 0)
                    throw new Exception("Invalid SitebracoWebSettings.App.UmbracoContentFolderId");

                var year = createDate.ToString("yyyy");
                var month = createDate.ToString("MM");
                var day = createDate.ToString("dd");

                var contentService = Umbraco.Core.ApplicationContext.Current.Services.ContentService;
                IContent contentFolder = contentService.GetById(contentFolderId);
                
                if (contentFolder == null)
                    throw new Exception("Invalid UmbracoContentFolderNode of id: " + contentFolderId);


                var contentContainerDoctypeAlias = ObjectUtil.GetClassName<UmbracoDoctypes.ContentContainer>();


                // Year
                var yearFolderList = new List<IContent>(contentFolder.Children());
                var yearFolder = yearFolderList.Find(x => x.Name.Trim() == year);
                if (yearFolder == null)
                {
                    yearFolder = contentService.CreateContent(year, contentFolderId, contentContainerDoctypeAlias);
                    contentService.SaveAndPublishWithStatus(yearFolder);
                }


                // Month
                var monthFolderList = new List<IContent>(yearFolder.Children());
                var monthFolder = monthFolderList.Find(x => x.Name.Trim() == month);
                if (monthFolder == null)
                {
                    monthFolder = contentService.CreateContent(month, yearFolder, contentContainerDoctypeAlias);
                    contentService.SaveAndPublishWithStatus(monthFolder);
                }


                // Day
                var dayFolderList = new List<IContent>(monthFolder.Children());
                var dayFolder = dayFolderList.Find(x => x.Name.Trim() == day);
                if (dayFolder == null)
                {
                    dayFolder = contentService.CreateContent(day, monthFolder, contentContainerDoctypeAlias);
                    contentService.SaveAndPublishWithStatus(dayFolder);
                }


                // Return
                return dayFolder.Id;
            }


            /// <summary>
            /// Get structural folder (year => month => day) of the current date.
            /// Create if not exists.
            /// </summary>
            /// <returns>Folder id of day folder</returns>
            public static int GetStructuralFolder()
            {
                return GetStructuralFolder(DateTime.Now);
            }


            /// <summary>
            /// Validate whether content is a Content Alias node
            /// </summary>
            /// <param name="contentId">Content id to validate</param>
            public static bool ValidateContentNode(int contentId, bool throwError = true)
            {
                var content = Umbraco.Core.ApplicationContext.Current.Services.ContentService.GetById(contentId);
                return ValidateContentNode(content, throwError);
            }
            /// <summary>
            /// Validate whether content is a Content Alias node
            /// </summary>
            /// <param name="contentId">Content to validate</param>
            public static bool ValidateContentNode(IContent content, bool throwError = true)
            {
                if (content == null || content.ContentType.Alias != ObjectUtil.GetClassName<UmbracoDoctypes.Content>())
                {
                    if (throwError) throw new Exception("Invalid Content Node");
                    return false;
                }
                return true;
            }
            
        }
        public class ContentMedia
        {
            public static string ConstructUrl(int mediaItemId)
            {
                var handlerKey = ObjectUtil.GetPropertyName<Constant.WebConfig.App>(x => x.ContentMediaHandler);
                var handlerUrl = SitebracoApiSettings.GetConfig().App.GetValue(handlerKey).TrimStart('/');

                var url = System.Web.HttpContext.Current.Request.Url;

                var tpl = "{0}://{1}{2}{3}/{4}?itemId={5}".Fmt(
                    url.Scheme, 
                    url.Host,
                    url.Port != 80 ? ":" : "",
                    url.Port != 80 ? url.Port.ToString() : "",
                    handlerUrl,
                    mediaItemId
                    );

                return tpl;
            }
        }
    }
}
