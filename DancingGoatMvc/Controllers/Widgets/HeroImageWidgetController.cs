using System.Linq;
using System.Web.Mvc;

using CMS.DocumentEngine;

using DancingGoat.Controllers.Widgets;
using DancingGoat.Models.Widgets;

using Kentico.PageBuilder.Web.Mvc;

[assembly: RegisterWidget("DancingGoat.LandingPage.HeroImage", typeof(HeroImageWidgetController), "{$dancinggoatmvc.widget.heroimage.name$}", Description = "{$dancinggoatmvc.widget.heroimage.description$}", IconClass = "icon-badge")]

namespace DancingGoat.Controllers.Widgets
{
    public class HeroImageWidgetController : WidgetController<HeroImageWidgetProperties>
    {
        /// <summary>
        /// Creates an instance of <see cref="BannerWidgetController"/> class.
        /// </summary>
        public HeroImageWidgetController()
        {
        }


        /// <summary>
        /// Creates an instance of <see cref="BannerWidgetController"/> class.
        /// </summary>
        /// <param name="propertiesRetriever">Retriever for widget properties.</param>
        /// <param name="currentPageRetriever">Retriever for current page where is the widget used.</param>
        /// <remarks>Use this constructor for tests to handle dependencies.</remarks>
        public HeroImageWidgetController(IWidgetPropertiesRetriever<HeroImageWidgetProperties> propertiesRetriever,
                                        ICurrentPageRetriever currentPageRetriever) : base(propertiesRetriever, currentPageRetriever)
        {
        }


        // GET: HeroImageWidget
        public ActionResult Index()
        {
            var properties = GetProperties();
            var image = GetImage(properties);

            return PartialView("Widgets/_HeroImageWidget", new HeroImageWidgetViewModel
            {
                Image = image,
                Text = properties.Text,
                ButtonText = properties.ButtonText,
                ButtonTarget = properties.ButtonTarget,
                Theme = properties.Theme
            });
        }


        private DocumentAttachment GetImage(HeroImageWidgetProperties properties)
        {
            var page = GetPage();
            return page?.AllAttachments.FirstOrDefault(x => x.AttachmentGUID == properties.ImageGuid);
        }
    }
}