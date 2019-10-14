using CMS.DocumentEngine;

namespace DancingGoat.Models.Widgets
{
    /// <summary>
    /// View model for Card widget.
    /// </summary>
    public class CardWidgetViewModel
    {
        /// <summary>
        /// Card background image.
        /// </summary>
        public DocumentAttachment Image { get; set; }


        /// <summary>
        /// Card text.
        /// </summary>
        public string Text { get; set; }
    }
}