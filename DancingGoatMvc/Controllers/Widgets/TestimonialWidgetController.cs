using System.Web.Mvc;

using DancingGoat.Controllers.Widgets;
using DancingGoat.Models.Widgets;

using Kentico.PageBuilder.Web.Mvc;

[assembly: RegisterWidget("DancingGoat.LandingPage.TestimonialWidget", typeof(TestimonialWidgetController), "{$dancinggoatmvc.widget.testimonial.name$}", Description = "{$dancinggoatmvc.widget.testimonial.description$}", IconClass = "icon-right-double-quotation-mark")]

namespace DancingGoat.Controllers.Widgets
{
    public class TestimonialWidgetController : WidgetController<TestimonialWidgetProperties>
    {
        /// <summary>
        /// Creates an instance of <see cref="TestimonialWidgetController"/> class.
        /// </summary>
        public TestimonialWidgetController()
        {
        }


        /// <summary>
        /// Creates an instance of <see cref="TestimonialWidgetController"/> class.
        /// </summary>
        /// <param name="propertiesRetriever">Retriever for widget properties.</param>
        /// <param name="currentPageRetriever">Retriever for current page where is the widget used.</param>
        /// <remarks>Use this constructor for tests to handle dependencies.</remarks>
        public TestimonialWidgetController(IWidgetPropertiesRetriever<TestimonialWidgetProperties> propertiesRetriever,
                                        ICurrentPageRetriever currentPageRetriever) : base(propertiesRetriever, currentPageRetriever)
        {
        }


        // GET: TestimonialWidget
        public ActionResult Index()
        {
            var properties = GetProperties();
            return PartialView("Widgets/_TestimonialWidget", new TestimonialWidgetViewModel
            {
                QuotationText = properties.QuotationText,
                AuthorText = properties.AuthorText,
                ColorCssClass = properties.ColorCssClass
            }); 
        }
    }
}