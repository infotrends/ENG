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

        [Required]
        public string category_tsd { get; set; }

        public string ClientId_s { get; set; }
    }
}
