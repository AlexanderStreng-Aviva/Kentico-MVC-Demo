using System.Linq;
using System.Web.Mvc;

using CMS.DocumentEngine;

using DancingGoat.Controllers.Widgets;
using DancingGoat.Models.Widgets;

using Kentico.PageBuilder.Web.Mvc;

[assembly: RegisterWidget("DancingGoat.LandingPage.CardWidget", typeof(CardWidgetController), "{$dancinggoatmvc.widget.card.name$}", Description = "{$dancinggoatmvc.widget.card.description$}", IconClass = "icon-rectangle-paragraph")]

namespace DancingGoat.Controllers.Widgets
{
    public class CardWidgetController : WidgetController<CardWidgetProperties>
    {
        /// <summary>
        /// Creates an instance of <see cref="CardWidgetController"/> class.
        /// </summary>
        public CardWidgetController()
        {
        }


        /// <summary>
        /// Creates an instance of <see cref="CardWidgetController"/> class.
        /// </summary>
        /// <param name="propertiesRetriever">Retriever for widget properties.</param>
        /// <param name="currentPageRetriever">Retriever for current page where is the widget used.</param>
        /// <remarks>Use this constructor for tests to handle dependencies.</remarks>
        public CardWidgetController(IComponentPropertiesRetriever<CardWidgetProperties> propertiesRetriever,
                                        ICurrentPageRetriever currentPageRetriever) : base(propertiesRetriever, currentPageRetriever)
        {
        }


        // GET: BannerWidget
        public ActionResult Index()
        {
            var properties = GetProperties();
            var image = GetImage(properties);

            return PartialView("Widgets/_CardWidget", new CardWidgetViewModel
            {
                Image = image,
                Text = properties.Text
            });
        }


        private DocumentAttachment GetImage(CardWidgetProperties properties)
        {
            var page = GetPage();
            return page?.AllAttachments.FirstOrDefault(x => x.AttachmentGUID == properties.ImageGuid);
        }
    }
}