using CMS.DocumentEngine.Types.CMS;
using CMS.SiteProvider;

namespace DancingGoat.Repositories.Implementation
{
    public class KenticoMenuItemRepository : IMenuItemRepository
    {
        private readonly string _cultureName;
        private readonly bool _latestVersionEnabled;

        public KenticoMenuItemRepository(string cultureName, bool latestVersionEnabled)
        {
            _cultureName = cultureName;
            _latestVersionEnabled = latestVersionEnabled;
        }

        public MenuItem GetMenuItem(string pageAlias)
        {
            return MenuItemProvider.GetMenuItems()
                .LatestVersion(_latestVersionEnabled)
                .Published(!_latestVersionEnabled)
                .OnSite(SiteContext.CurrentSiteName)
                .Culture(_cultureName)
                .CombineWithDefaultCulture()
                .WhereEquals("NodeAlias", pageAlias)
                .TopN(1);
        }
    }
}