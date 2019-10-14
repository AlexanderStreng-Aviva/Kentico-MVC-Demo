using CMS.DocumentEngine;

namespace DancingGoat.Models.Widgets
{
    /// <summary>
    /// View model for Banner widget.
    /// </summary>
    public class BannerWidgetViewModel
    {
        /// <summary>
        /// Banner background image.
        /// </summary>
        public DocumentAttachment Image { get; set; }


        /// <summary>
        /// Banner text.
        /// </summary>
        public string Text { get; set; }
    }
}