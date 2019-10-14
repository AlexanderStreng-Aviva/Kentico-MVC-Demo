using System.Linq;
using System.Web.Mvc;

using CMS.DocumentEngine;

using Kentico.PageBuilder.Web.Mvc;

using DancingGoat.Controllers.Widgets;
using DancingGoat.Models.Widgets;

[assembly: RegisterWidget("DancingGoat.HomePage.ImageWidget", typeof(ImageWidgetController), "{$dancinggoatmvc.widget.image.name$}", Description = "{$dancinggoatmvc.widget.image.description$}", IconClass = "icon-picture")]

namespace DancingGoat.Controllers.Widgets
{
    public class ImageWidgetController : WidgetController<ImageWidgetProperties>
    {
        /// <summary>
        /// Creates an instance of <see cref="ImageWidgetController"/> class.
        /// </summary>
        public ImageWidgetController()
        {
        }


        /// <summary>
        /// Creates an instance of <see cref="ImageWidgetController"/> class.
        /// </summary>
        /// <param name="propertiesRetriever">Retriever for widget properties.</param>
        /// <param name="currentPageRetriever">Retriever for current page where is the widget used.</param>
        /// <remarks>Use this constructor for tests to handle dependencies.</remarks>
        public ImageWidgetController(IWidgetPropertiesRetriever<ImageWidgetProperties> propertiesRetriever,
                                        ICurrentPageRetriever currentPageRetriever) : base(propertiesRetriever, currentPageRetriever)
        {
        }


        // GET: ImagesWidget
        public ActionResult Index()
        {
            var properties = GetProperties();
            var image = GetImage(properties);

            return PartialView("Widgets/_ImageWidget", new ImageWidgetViewModel
            {
                Image = image
            });
        }


        private DocumentAttachment GetImage(ImageWidgetProperties properties)
        {
            var page = GetPage();
            return page?.AllAttachments.FirstOrDefault(x => x.AttachmentGUID == properties.ImageGuid);
            
        }
    } 
}