using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyUtils.Validations;

namespace SitebracoApi.Models.Eng
{
    public class FeedbackModel : EngBase
    {
        [Required]
        public string name { get; set; }

        [Required]
        public string email { get; set; }

        [Required]
        public string feedback { get; set; }

    }
}
