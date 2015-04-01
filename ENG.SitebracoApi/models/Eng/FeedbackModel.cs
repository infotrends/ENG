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
        public string name_tsd { get; set; }

        [Required]
        public string email_tsd { get; set; }

        [Required]
        public string feedback_tsd { get; set; }

        public string ClientId_s { get; set; }
    }
}
