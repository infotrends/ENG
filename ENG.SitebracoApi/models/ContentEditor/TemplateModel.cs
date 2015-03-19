using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SitebracoApi.DbEntities;

namespace SitebracoApi.Models.ContentEditor
{
    public class TemplateModel
    {
        public Guid? Id{ get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Type { get; set; }
        public DateTime? MarkDeleteOn { get; set; }
        public DateTime? CreateOn { get; set; }
        public DateTime? ModifiedOn { get; set; }


        public static TemplateModel Apply(TemplateModel v)
        {
            if (v == null)
                return null;

            // Return
            return v;
        }
        public static List<TemplateModel> Transfer(List<new_OfficeTemplate> entities)
        {
            var ret = new List<TemplateModel>();

            foreach (var item in entities)
            {
                var o = TransferSingle(item);
                ret.Add(o);
            }

            // Return
            return ret;
        }
        public static TemplateModel TransferSingle(new_OfficeTemplate entity)
        {
            if (entity == null)
                return null;

            var ret = new TemplateModel
            {
                CreateOn = entity.CreateOn,
                Description = entity.Description,
                Id = entity.Id,
                MarkDeleteOn = entity.MarkDeleteOn,
                ModifiedOn = entity.ModifiedOn,
                Name = entity.Name,
                Type = entity.Type
            };

            Apply(ret);

            return ret;
        }

    }
}
