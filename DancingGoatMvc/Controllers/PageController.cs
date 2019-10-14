using System.Web.Mvc;
using DancingGoat.Repositories;
using Kentico.PageBuilder.Web.Mvc;
using Kentico.Web.Mvc;

namespace DancingGoat.Controllers
{
    public class PageController : Controller
    {
        private IMenuItemRepository _menuItemRepository;

        public PageController(IMenuItemRepository repository)
        {
            _menuItemRepository = repository;
        }


        // GET: LandingPage
        public ActionResult Index(string pageAlias)
        {
            var menuItemPage = _menuItemRepository.GetMenuItem(pageAlias);
            if (menuItemPage == null)
            {
                return HttpNotFound();
            }

            HttpContext.Kentico().PageBuilder().Initialize(menuItemPage.DocumentID);

            return View();
        }
    }
}