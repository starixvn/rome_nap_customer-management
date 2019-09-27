namespace Fanex.Nap.CMS.Customers.ViewModels.PossessStatistics
{
    using System.Collections.Generic;
    using System.Web.Mvc;
    using Constants;
    using Resources;

    public class UpdateDangerLevelViewModel : BaseViewModel
    {
        public UpdateDangerLevelViewModel()
        {
            Category = DangerLevelCategoryConst.ABI;
            DangerLevel = -1;
        }

        public IDictionary<int, string> SelectedCustomers { get; set; }

        public byte Category { get; set; }

        public int DangerLevel { get; set; }

        public bool ApplyForAllMembers { get; set; }

        public IEnumerable<SelectListItem> Categories { get; } = new[] {
            BuildSelectListItem(DangerLevelCategoryConst.Original, PossessStatisticsResource.Original),
            BuildSelectListItem(DangerLevelCategoryConst.ABI, PossessStatisticsResource.ABI, true),
            BuildSelectListItem(DangerLevelCategoryConst.OtherSports, PossessStatisticsResource.OtherSports)
        };

        public IEnumerable<SelectListItem> DangerLevels { get; } = BuildDangerLevels();

        private static IEnumerable<SelectListItem> BuildDangerLevels()
        {
            var values = new List<SelectListItem>();
            values.Add(BuildSelectListItem(-1, PossessStatisticsResource.PleaseSelect, true));
            values.Add(BuildSelectListItem(0, PossessStatisticsResource.None));

            for (var i = 1; i <= DangerLevelConst.MaxDangerLevel; i++)
            {
                values.Add(BuildSelectListItem(i, i.ToString()));
            }

            return values;
        }
    }
}