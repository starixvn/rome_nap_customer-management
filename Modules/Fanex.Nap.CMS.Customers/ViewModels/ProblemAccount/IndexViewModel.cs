namespace Fanex.Nap.CMS.Customers.ViewModels.ProblemAccount
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using Constants;
    using Infrastructure.Constants;
    using Infrastructure.Models;
    using Resources;

    public class IndexViewModel
    {
        public const int MaxInputUsernames = 100;

        public const int MaxPerEvent = 5000;
        public const int MaxPerBet = 1000;

        // The release date of this feature.
        public static readonly DateTime DefaultMinDate = new DateTime(2019, 9, 12);

        public IndexViewModel(
            bool hasHostPermission,
            SearchViewModel searchViewModel,
            IEnumerable<IdNameModel> filterReasons,
            IEnumerable<IdNameModel> markNewReasons,
            string updater)
        {
            HasHostPermission = hasHostPermission;
            AutoReload = searchViewModel != null;
            SearchFilter = searchViewModel ?? new SearchViewModel();
            Reasons = BuildFilterReasons(filterReasons, SearchFilter.Reason);
            MarkNewReasons = BuildMarkNewReasons(markNewReasons);
            Updater = updater;
        }

        public bool HasHostPermission { get; }

        public bool AutoReload { get; }

        public SearchViewModel SearchFilter { get; }

        public DateTime CurrentDate { get; } = DateTime.Today;

        public string Updater { get; }

        public IEnumerable<SelectListItem> Levels { get; } = new[]
        {
            BuildSelectListItem(RoleIdConstants.Super, nameof(RoleIdConstants.Super)),
            BuildSelectListItem(RoleIdConstants.Master, nameof(RoleIdConstants.Master)),
            BuildSelectListItem(RoleIdConstants.Agent, nameof(RoleIdConstants.Agent)),
            BuildSelectListItem(RoleIdConstants.Member, nameof(RoleIdConstants.Member))
        };

        public IEnumerable<SelectListItem> MarkTypes { get; } = new[]
        {
            BuildSelectListItem(MarkTypeConst.All,ProblemAccountResource.All),
            BuildSelectListItem(MarkTypeConst.Yes,ProblemAccountResource.Yes),
            BuildSelectListItem(MarkTypeConst.No, ProblemAccountResource.No)
        };


        public IEnumerable<SelectListItem> Currencies
            => CurrencyConst.CurrencyList.Select(kvp => BuildSelectListItem(kvp.Key, kvp.Value));

        public IEnumerable<SelectListItem> AccountStatus
            => AccountStatusConst.AccountStatusList.Select(kvp => BuildSelectListItem(kvp.Key, kvp.Value));

        public IEnumerable<SelectListItem> IsReduced
            => IsReducedConst.IsReducedList.Select(kvp => BuildSelectListItem(kvp.Key, kvp.Value));

        public IEnumerable<SelectListItem> Reasons { get; }

        public IEnumerable<SelectListItem> OrderBy
            => OrderByConst.OrderByList.Select(kvp => BuildSelectListItem(kvp.Key, kvp.Value));

        public IEnumerable<SelectListItem> MarkNewLevels
            => new[] { BuildSelectListItem(0, ProblemAccountResource.PleaseSelect) }.Concat(Levels).ToList();

        public IEnumerable<SelectListItem> MarkNewReasons { get; }

        private static SelectListItem BuildSelectListItem(int value, string name, bool selected = false)
            => new SelectListItem { Value = value.ToString(), Text = name, Selected = selected };

        private static IEnumerable<SelectListItem> BuildFilterReasons(IEnumerable<IdNameModel> reasons, string selectedReasons)
        {
            var reasonHash = ToDictionary(selectedReasons);
            var isSelectAll = reasonHash.Count == 0;

            return reasons
                .Select(r => BuildSelectListItem(r.Id, r.Name, isSelectAll || reasonHash.ContainsKey(r.Id)))
                .ToList();
        }

        private static IDictionary<int, bool> ToDictionary(string text)
            => string.IsNullOrWhiteSpace(text)
            ? new Dictionary<int, bool>()
            : text
                .Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                .ToDictionary(kvp => int.Parse(kvp), kvp => true);

        private static IEnumerable<SelectListItem> BuildMarkNewReasons(IEnumerable<IdNameModel> reasons)
            => new[] { BuildSelectListItem(0, ProblemAccountResource.PleaseSelect) }
            .Concat(reasons.Select(r => BuildSelectListItem(r.Id, r.Name)))
            .ToList();
    }
}
