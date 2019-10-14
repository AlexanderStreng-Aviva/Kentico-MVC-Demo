using System.Collections.Generic;

using CMS.DocumentEngine.Types.DancingGoatMvc;

namespace DancingGoat.Models.Home
{
    public class IndexViewModel
    {
        public IEnumerable<HomeSectionViewModel> HomeSections { get; set; }

        public IEnumerable<Cafe> CompanyCafes { get; set; }
    }
}