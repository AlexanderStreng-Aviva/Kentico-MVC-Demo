using System;

using Kentico.PageBuilder.Web.Mvc;

namespace DancingGoat.Models.Widgets
{
    /// <summary>
    /// Banner widget properties.
    /// </summary>
    public class BannerWidgetProperties : IWidgetProperties
    {
        /// <summary>
        /// Guid of an image to be displayed.
        /// </summary>
        public Guid ImageGuid { get; set; }


        /// <summary>
        /// Text to be displayed.
        /// </summary>
        public string Text { get; set; }
    }
}