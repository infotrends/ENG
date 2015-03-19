using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SitebracoApi.Models.ContentEditor
{
    public class ContentSpinDoctor
    {
        public int? Id { get; set; }
        public string Name { get; set; }
        public int? Value { get; set; }

        /// <summary>
        /// Return Id:Value pair
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return "{0}:{1}".Fmt(Id, Value);
        }
    }
}
