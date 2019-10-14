using System.Web.Mvc;

using DancingGoat.Repositories;

using Kentico.PageBuilder.Web.Mvc;
using Kentico.Web.Mvc;

namespace DancingGoat.Controllers
{
    public class LandingPageController : Controller
    {
        private readonly ILandingPageRepository mRepository;


        public LandingPageController(ILandingPageRepository repository)
        {
            mRepository = repository;
        }


        // GET: LandingPage
        public ActionResult Index(string pageAlias)
        {
            var landingPage = mRepository.GetLandingPage(pageAlias);
            if (landingPage == null)
            {
                return HttpNotFound();
            }

            HttpContext.Kentico().PageBuilder().Initialize(landingPage.DocumentID);

            return View();
        }
    }
}