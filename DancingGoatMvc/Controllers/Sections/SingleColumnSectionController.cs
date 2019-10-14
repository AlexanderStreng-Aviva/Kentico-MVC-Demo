using System.Web.Mvc;

using DancingGoat.Controllers.Sections;

using Kentico.PageBuilder.Web.Mvc;

[assembly: RegisterSection("DancingGoat.SingleColumnSection", typeof(SingleColumnSectionController), "{$dancinggoatmvc.section.singlecolumn.name$}", Description = "{$dancinggoatmvc.section.singlecolumn.description$}", IconClass = "icon-square")]

namespace DancingGoat.Controllers.Sections
{
    public class SingleColumnSectionController : Controller
    {
        // GET: SingleColumnSection
        public ActionResult Index()
        {
            return PartialView("Sections/_SingleColumnSection");
        }
    }
}