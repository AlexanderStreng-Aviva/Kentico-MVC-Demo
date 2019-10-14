using System.Linq;
using System.Web.Mvc;

using CMS.DocumentEngine.Types.DancingGoatMvc;
using DancingGoat.Infrastructure;
using DancingGoat.Models.Home;
using DancingGoat.Repositories;

using Kentico.PageBuilder.Web.Mvc;
using Kentico.Web.Mvc;

namespace DancingGoat.Controllers
{
    public class HomeController : Controller
    {
        private readonly IPromotedContentRepository mHighlightRepository;
        private readonly IHomeRepository mHomeRepository;
        private readonly IOutputCacheDependencies mOutputCacheDependencies;


        public HomeController(IPromotedContentRepository repository,
                              IHomeRepository homeRepository,
                              IOutputCacheDependencies outputCacheDependencies)
        {
            mHighlightRepository = repository;
            mHomeRepository = homeRepository;
            mOutputCacheDependencies = outputCacheDependencies;
        }


        // GET: Home
        public ActionResult Index()
        {
            var home = mHomeRepository.GetHomePage();
            if (home == null)
            {
                return HttpNotFound();
            }

            HttpContext.Kentico().PageBuilder().Initialize(home.DocumentID);

            var viewModel = new IndexViewModel
            {
                HomeSections = mHomeRepository.GetHomeSections().Select(HomeSectionViewModel.GetViewModel),
                CompanyCafes = mHighlightRepository.GetPromotedCompanyCafes(4)
            };
            
            mOutputCacheDependencies.AddDependencyOnPages<HomeSection>();
            mOutputCacheDependencies.AddDependencyOnPages<Cafe>();
            mOutputCacheDependencies.AddDependencyOnPages<Home>();

            return View(viewModel);
        }
    }
}
