using System.Web.Mvc;

using DancingGoat.Controllers.Sections;

using Kentico.PageBuilder.Web.Mvc;

[assembly: RegisterSection("DancingGoat.TwoColumnSection", typeof(TwoColumnSectionController), "{$dancinggoatmvc.section.twocolumn.name$}", Description = "{$dancinggoatmvc.section.twocolumn.description$}", IconClass = "icon-l-cols-2")]

namespace DancingGoat.Controllers.Sections
{
    public class TwoColumnSectionController : Controller
    {
        // GET: TwoColumnSection
        public ActionResult Index()
        {
            return PartialView("Sections/_TwoColumnSection");
        }
    }
}