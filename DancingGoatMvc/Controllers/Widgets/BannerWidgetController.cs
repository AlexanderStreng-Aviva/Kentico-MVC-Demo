using System.Linq;
using System.Web.Mvc;

using CMS.DocumentEngine;

using DancingGoat.Controllers.Widgets;
using DancingGoat.Models.Widgets;

using Kentico.PageBuilder.Web.Mvc;

[assembly: RegisterWidget("DancingGoat.HomePage.BannerWidget", typeof(BannerWidgetController), "{$dancinggoatmvc.widget.banner.name$}", Description = "{$dancinggoatmvc.widget.banner.description$}", IconClass = "icon-ribbon")]

namespace DancingGoat.Controllers.Widgets
{
    public class BannerWidgetController : WidgetController<BannerWidgetProperties>
    {
        /// <summary>
        /// Creates an instance of <see cref="BannerWidgetController"/> class.
        /// </summary>
        public BannerWidgetController()
        {
        }


        /// <summary>
        /// Creates an instance of <see cref="BannerWidgetController"/> class.
        /// </summary>
        /// <param name="propertiesRetriever">Retriever for widget properties.</param>
        /// <param name="currentPageRetriever">Retriever for current page where is the widget used.</param>
        /// <remarks>Use this constructor for tests to handle dependencies.</remarks>
        public BannerWidgetController(IWidgetPropertiesRetriever<BannerWidgetProperties> propertiesRetriever,
                                        ICurrentPageRetriever currentPageRetriever) : base(propertiesRetriever, currentPageRetriever)
        {
        }


        // GET: BannerWidget
        public ActionResult Index()
        {
            var properties = GetProperties();
            var image = GetImage(properties);

            return PartialView("Widgets/_BannerWidget", new BannerWidgetViewModel
            {
                Image = image,
                Text = properties.Text
            });
        }


        private DocumentAttachment GetImage(BannerWidgetProperties properties)
        {
            var page = GetPage();
            return page?.AllAttachments.FirstOrDefault(x => x.AttachmentGUID == properties.ImageGuid);
        }
    }
}