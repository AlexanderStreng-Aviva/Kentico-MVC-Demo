using System;

using Kentico.Forms.Web.Mvc;
using Kentico.PageBuilder.Web.Mvc;

namespace DancingGoat.Models.Widgets
{
    /// <summary>
    /// Hero image widget properties.
    /// </summary>
    public class HeroImageWidgetProperties : IWidgetProperties
    {
        /// <summary>
        /// Guid of background image.
        /// </summary>
        public Guid ImageGuid { get; set; }


        /// <summary>
        /// Text to be displayed.
        /// </summary>
        public string Text { get; set; }


        /// <summary>
        /// Button text.
        /// </summary>
        public string ButtonText { get; set; }


        /// <summary>
        /// Target of button link.
        /// </summary>
        [EditingComponent(TextInputComponent.IDENTIFIER, Label = "{$DancingGoatMVC.Widget.HeroImage.ButtonTarget$}", Order = 1)]
        public string ButtonTarget { get; set; }


        /// <summary>
        /// Theme of the widget.
        /// </summary>
        [EditingComponent(DropDownComponent.IDENTIFIER, Label = "{$DancingGoatMVC.Widget.HeroImage.ColorScheme$}", Order = 2)]
        [EditingComponentProperty(nameof(DropDownProperties.DataSource), "light;{$DancingGoatMVC.Widget.HeroImage.ColorScheme.Light$}\r\ndark;{$DancingGoatMVC.Widget.HeroImage.ColorScheme.Dark$}")]
        public string Theme { get; set; } = "dark";
    }
}