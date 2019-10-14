using System.Web.Mvc;

using DancingGoat.Controllers.Widgets;
using DancingGoat.Models.Widgets;

using Kentico.PageBuilder.Web.Mvc;

[assembly: RegisterWidget("DancingGoat.General.TextWidget", typeof(TextWidgetController), "{$dancinggoatmvc.widget.text.name$}", Description = "{$dancinggoatmvc.widget.text.description$}", IconClass = "icon-l-text")]

namespace DancingGoat.Controllers.Widgets
{
    public class TextWidgetController : WidgetController<TextWidgetProperties>
    {
        /// <summary>
        /// Creates an instance of <see cref="TextWidgetController"/> class.
        /// </summary>
        public TextWidgetController()
        {
        }


        /// <summary>
        /// Creates an instance of <see cref="TextWidgetController"/> class.
        /// </summary>
        /// <param name="propertiesRetriever">Retriever for widget properties.</param>
        /// <param name="currentPageRetriever">Retriever for current page where is the widget used.</param>
        /// <remarks>Use this constructor for tests to handle dependencies.</remarks>
        public TextWidgetController(IComponentPropertiesRetriever<TextWidgetProperties> propertiesRetriever,
                                        ICurrentPageRetriever currentPageRetriever) : base(propertiesRetriever, currentPageRetriever)
        {
        }


        // GET: TextWidget
        public ActionResult Index()
        {
            var properties = GetProperties();
            return PartialView("Widgets/_TextWidget", new TextWidgetViewModel { Text = properties.Text }); 
        }
    }
}