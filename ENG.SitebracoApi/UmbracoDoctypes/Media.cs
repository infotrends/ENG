using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SitebracoApi.UmbracoDoctypes
{
    public class Media : ContentBase
    {
        public string media_sourceFile { get; set; }
        public string media_extension { get; set; }
        public string media_contentType { get; set; }
        public string media_uploadedBy { get; set; }

    }
}
