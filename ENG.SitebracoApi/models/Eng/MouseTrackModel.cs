using MyUtils.Validations;

namespace SitebracoApi.Models.Eng
{
    public class MouseTrackModel : EngBase
    {
        /// <summary>
        /// Client ID
        /// </summary>
        [Required]        
        public string ClientId_s { get; set; }

        /// <summary>
        /// Currently Page URL
        /// </summary>
        [Required]
        public string PageUrl_tsd { get; set; }
        /// <summary>
        /// Point X of the mouse position on screen
        /// </summary>
        [Required]
        public int PageX_i { get; set; }
        /// <summary>
        /// Point Y of the mouse position on screen
        /// </summary>
        [Required]
        public int PageY_i { get; set; }

        [Required]
        public int Point_i { get; set; }

        public string Position_s { get; set; }

        /// <summary>
        /// Target Name when click or hover
        /// </summary>
        public string TargetName_tsd { get; set; }
        /// <summary>
        /// Class Name when click or hover
        /// </summary>
        public string TargetClassName_tsd { get; set; }
        /// <summary>
        /// Target ID when click or hover
        /// </summary>
        public string TargetID_tsd { get; set; }

        /// <summary>
        /// Window Width
        /// </summary>
        public int WindowW_i { get; set; }

        /// <summary>
        /// Window Height
        /// </summary>
        public int WindowH_i { get; set; }
        /// <summary>
        /// Screen Width
        /// </summary>
        public int ScreenW_i { get; set; }
        /// <summary>
        /// Screen Height
        /// </summary>
        public int ScreenH_i { get; set; }

        /// <summary>
        /// Name of the Action (Click on ...)
        /// </summary>
        [Required]
        public string ActionName_s { get; set; }

        /// <summary>
        ///  IP Address
        /// </summary>
        public string IPAddress_s { get; set; }
    }
}
