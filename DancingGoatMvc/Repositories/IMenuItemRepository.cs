using CMS.DocumentEngine.Types.CMS;

namespace DancingGoat.Repositories
{
    public interface IMenuItemRepository : IRepository
    {
        MenuItem GetMenuItem(string pageAlias);
    }
}