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

        public const int CY = 81;
        private const int UUS = 20;

        private const byte No = 1;
        private const byte Yes = 2;

        public const int OrderBySMALine = 1;
        private const int OrderByMarkDate = 2;
        private const int OrderByAccountStatus = 3;

        // The release date of this feature.
        public static readonly DateTime DefaultMinDate = new DateTime(2019, 8, 1);

        public IndexViewModel(
            bool hasHostPermission,
            SearchViewModel searchViewModel,
            IEnumerable<IdNameModel> filterReasons,
            IEnumerable<IdNameModel> markNewReasons)
        {
            HasHostPermission = hasHostPermission;
            AutoReload = searchViewModel != null;
            SearchFilter = searchViewModel ?? new SearchViewModel();
            Reasons = BuildFilterReasons(filterReasons, SearchFilter.Reason);
            MarkNewReasons = BuildMarkNewReasons(markNewReasons);
        }

        public bool HasHostPermission { get; }

        public bool AutoReload { get; }

        public SearchViewModel SearchFilter { get; }

        public DateTime CurrentDate { get; } = DateTime.Today;

        public IEnumerable<SelectListItem> Levels { get; } = new[]
        {
            BuildSelectListItem(RoleIdConstants.Super, nameof(RoleIdConstants.Super)),
            BuildSelectListItem(RoleIdConstants.Master, nameof(RoleIdConstants.Master)),
            BuildSelectListItem(RoleIdConstants.Agent, nameof(RoleIdConstants.Agent)),
            BuildSelectListItem(RoleIdConstants.Member, nameof(RoleIdConstants.Member))
        };

        public IEnumerable<SelectListItem> Currencies { get; } = new[]
        {
            BuildSelectListItem(CY, nameof(CY)),
            BuildSelectListItem(UUS, nameof(UUS))
        };

        public IEnumerable<SelectListItem> AccountStatus { get; } = new[]
        {
            BuildSelectListItem(0, ProblemAccountResource.All),
            BuildSelectListItem(AccountStatusConst.Active, nameof(AccountStatusConst.Active)),
            BuildSelectListItem(AccountStatusConst.ViewOnly, ProblemAccountResource.ViewOnly),
            BuildSelectListItem(AccountStatusConst.Suspended, nameof(AccountStatusConst.Suspended)),
            BuildSelectListItem(AccountStatusConst.Inactive, nameof(AccountStatusConst.Inactive))
        };

        public IEnumerable<SelectListItem> IsReduced { get; } = new[]
        {
            BuildSelectListItem(0, ProblemAccountResource.All),
            BuildSelectListItem(Yes, nameof(Yes)),
            BuildSelectListItem(No, nameof(No))
        };

        public IEnumerable<SelectListItem> Reasons { get; }

        public IEnumerable<SelectListItem> OrderBy { get; } = new[]
        {
            BuildSelectListItem(OrderBySMALine, ProblemAccountResource.SMALine),
            BuildSelectListItem(OrderByMarkDate, ProblemAccountResource.MarkDate),
            BuildSelectListItem(OrderByAccountStatus, ProblemAccountResource.AccountStatus)
        };

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
