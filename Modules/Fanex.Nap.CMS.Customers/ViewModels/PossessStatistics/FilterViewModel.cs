namespace Fanex.Nap.CMS.Customers.ViewModels.PossessStatistics
{
    using System;
    using System.Collections.Generic;
    using System.Web.Mvc;
    using Constants;
    using Constants.ProblemAccount;
    using Resources;

    [Serializable]
    public class FilterViewModel : BaseViewModel
    {
        private const byte DefaultMaxPercent = 100;
        private const int DefaultPageSize = 50;

        public FilterViewModel()
        {
            TotalSBPercentTo = TotalNSPercentTo = DefaultMaxPercent;
            PageIndex = 1;
            PageSize = DefaultPageSize;
            Monitored = 2;
            SortBy = PossessStatisticsSortSequence.TotalSBPercentAsc;
        }

        public byte TotalSBPercentFrom { get; set; }

        public byte TotalSBPercentTo { get; set; }

        public string PTConditionLogic { get; set; }

        public byte TotalNSPercentFrom { get; set; }

        public byte TotalNSPercentTo { get; set; }

        public string Username { get; set; }

        public string AgentStatus { get; set; }

        public string DangerLevelOperator { get; set; }

        public byte DangerLevelValue { get; set; }

        public byte Monitored { get; set; }

        public byte SortBy { get; set; }

        public int PageIndex { get; set; }

        public int PageSize { get; set; }

        public IEnumerable<SelectListItem> PTConditionLogics { get; } = new[]
        {
            new SelectListItem { Value = PTConditionLogicConst.AND, Text = PossessStatisticsResource.And, Selected = true },
            new SelectListItem { Value = PTConditionLogicConst.OR, Text = PossessStatisticsResource.Or }
        };

        public IEnumerable<SelectListItem> AccountStatus { get; } = new[]
        {
            BuildSelectListItem(AccountStatusConst.Active, PossessStatisticsResource.Active, true),
            BuildSelectListItem(AccountStatusConst.ViewOnly, PossessStatisticsResource.ViewOnly, true),
            BuildSelectListItem(AccountStatusConst.Suspended, PossessStatisticsResource.Suspended, true),
            BuildSelectListItem(AccountStatusConst.Inactive, PossessStatisticsResource.Inactive, true)
        };

        public IEnumerable<SelectListItem> DangerLevelOperators { get; } = new[]
        {
            new SelectListItem { Value = ">=", Text = ">=", Selected = true },
            new SelectListItem { Value = "<=", Text = "<=" },
            new SelectListItem { Value = "=", Text = "=" }
        };

        public IEnumerable<SelectListItem> DangerLevelValues { get; } = BuildDangerLevelValues();

        public IEnumerable<SelectListItem> MonitoredOptions { get; } = new[]
        {
            BuildSelectListItem(2, PossessStatisticsResource.All, true),
            BuildSelectListItem(1, PossessStatisticsResource.Yes),
            BuildSelectListItem(0, PossessStatisticsResource.No)
        };

        public IEnumerable<SelectListItem> SortSequences { get; } = new[] {
            BuildSelectListItem(PossessStatisticsSortSequence.TotalSBPercentAsc, PossessStatisticsResource.TotalSBPercentAcs),
            BuildSelectListItem(PossessStatisticsSortSequence.TotalSBPercentDesc, PossessStatisticsResource.TotalSBPercentDesc),
            BuildSelectListItem(PossessStatisticsSortSequence.TotalNSPercentAsc, PossessStatisticsResource.TotalNSPercentAcs),
            BuildSelectListItem(PossessStatisticsSortSequence.TotalNSPercentDesc, PossessStatisticsResource.TotalNSPercentDesc),
            BuildSelectListItem(PossessStatisticsSortSequence.SMALineAsc, PossessStatisticsResource.SMALineAsc),
            BuildSelectListItem(PossessStatisticsSortSequence.SMALineDesc, PossessStatisticsResource.SMALineDesc),
        };

        private static IEnumerable<SelectListItem> BuildDangerLevelValues()
        {
            var values = new List<SelectListItem>
            {
                BuildSelectListItem(0, PossessStatisticsResource.None)
            };

            for (var i = 1; i <= DangerLevelConst.MaxDangerLevel; i++)
            {
                values.Add(BuildSelectListItem(i, i.ToString()));
            }

            return values;
        }
    }
}