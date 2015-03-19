using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using umbraco.NodeFactory;
using Umbraco.Core.Models;

namespace SitebracoApi.Models.ContentEditor
{
    public class ContentMediaRetModel
    {
        public int? Id { get; set; }
        public int? ContentId { get; set; }
        public string ContentName { get; set; }

        public string IconUrl { get; set; }
        public string Link { get; set; }
        public string Name { get; set; }
        
        public DateTime? CreateOn { get; set; }

        public string Data { get; set; }
        public string ContentType { get; set; }
        public string Extension { get; set; }

        public static ContentMediaRetModel TransferSingle(Node node, bool includeData = false)
        {
            var c = node.Parent.Parent;
            var d = Extlib.ConvertFromUmbracoModel<UmbracoDoctypes.Media>(node);
            return TransferSingle(d, c.Id, c.Name, node.CreateDate, includeData);
        }
        public static ContentMediaRetModel TransferSingle(IContent content, bool includeData = false)
        {
            var c = content.Parent().Parent();
            var d = Extlib.ConvertFromUmbracoModel<UmbracoDoctypes.Media>(content);
            return TransferSingle(d, c.Id, c.Name, content.CreateDate, includeData);
        }
        public static ContentMediaRetModel TransferSingle(UmbracoDoctypes.Media media, int contentId, string contentName, DateTime createDate, bool includeData = false)
        {
            var iRet = new ContentMediaRetModel
            {
                ContentId = contentId,
                ContentName = contentName,
                ContentType = media.media_contentType,
                CreateOn = createDate,
                Data = includeData == true ? media.media_sourceFile : null,
                Extension = media.media_extension,
                Id = media.Id,
                Link = Utility.ContentMedia.ConstructUrl((int)media.Id),
                Name = media.NodeName
            };

            return iRet;
        }
        public static List<ContentMediaRetModel> Transfer(List<Node> nodes, bool includeData = false)
        {
            var ret = new List<ContentMediaRetModel>();
            foreach (var item in nodes)
            {
                var o = TransferSingle(item, includeData);
                ret.Add(o);
            }
            return ret;
        }
        public static List<ContentMediaRetModel> Transfer(List<IContent> contents, bool includeData = false)
        {
            var ret = new List<ContentMediaRetModel>();
            foreach (var item in contents)
            {
                var o = TransferSingle(item, includeData);
                ret.Add(o);
            }
            return ret;
        }

    }
}
