using System.Web.Mvc;

using DancingGoat.Controllers.Sections;

using Kentico.PageBuilder.Web.Mvc;

[assembly: RegisterSection("DancingGoat.ThreeColumnSection", typeof(ThreeColumnSectionController), "{$dancinggoatmvc.section.threecolumn.name$}", Description = "{$dancinggoatmvc.section.threecolumn.description$}", IconClass = "icon-l-cols-3")]

namespace DancingGoat.Controllers.Sections
{
    public class ThreeColumnSectionController : Controller
    {
        // GET: ThreeColumnSection
        public ActionResult Index()
        {
            return PartialView("Sections/_ThreeColumnSection");
        }
    }
}