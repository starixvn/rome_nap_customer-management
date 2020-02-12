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

        public IndexViewModel(
            bool hasHostPermission,
            bool autoReload,
            SearchViewModel searchViewModel,
            IEnumerable<IdNameModel> filterReasons,
            IEnumerable<IdNameModel> markNewReasons,
            string updater,
            DateTime minFromDate,
            IEnumerable<IdNameModel> currencies,
            IDictionary<byte, string> accountStatus)
        {
            HasHostPermission = hasHostPermission;
            AutoReload = autoReload;
            SearchFilter = searchViewModel;
            Reasons = BuildFilterReasons(filterReasons, SearchFilter.Reason);
            MarkNewReasons = BuildMarkNewReasons(markNewReasons);
            Updater = updater;
            MinFromDate = minFromDate;
            Currencies = currencies.Select(c => BuildSelectListItem(c.Id, c.Name));
            AccountStatus = accountStatus.Select(kvp => BuildSelectListItem(kvp.Key, kvp.Value));
        }

        public bool HasHostPermission { get; }

        public bool AutoReload { get; }

        public SearchViewModel SearchFilter { get; }

        public DateTime MinFromDate { get; }

        public DateTime CurrentDate { get; } = DateTime.Today;

        public string Updater { get; }

        public IEnumerable<SelectListItem> Levels { get; } = new[]
        {
            BuildSelectListItem(RoleIdConstants.Super, nameof(RoleIdConstants.Super)),
            BuildSelectListItem(RoleIdConstants.Master, nameof(RoleIdConstants.Master)),
            BuildSelectListItem(RoleIdConstants.Agent, nameof(RoleIdConstants.Agent)),
            BuildSelectListItem(RoleIdConstants.Member, nameof(RoleIdConstants.Member))
        };

        public IEnumerable<SelectListItem> DirectlyMarked
            => DirectlyMarkedConst.DirectlyMarkedList.Select(kvp => BuildSelectListItem(kvp.Key, kvp.Value));

        public IEnumerable<SelectListItem> Currencies { get; }

        public IEnumerable<SelectListItem> AccountStatus { get; }

        public IEnumerable<SelectListItem> IsReduced
            => IsReducedConst.IsReducedList.Select(kvp => BuildSelectListItem(kvp.Key, kvp.Value));

        public IEnumerable<SelectListItem> Reasons { get; }

        public IEnumerable<SelectListItem> OrderBy
            => OrderByConst.OrderByList.Select(kvp => BuildSelectListItem(kvp.Key, kvp.Value));

        public IEnumerable<SelectListItem> MarkNewLevels
            => new[] { BuildSelectListItem(0, ProblemAccountResource.PleaseSelect) }.Concat(Levels).ToList();

        public IEnumerable<SelectListItem> MarkNewReasons { get; }

        public IEnumerable<SelectListItem> InformTypes
        {
            get
            {
                return new[]
                {
                    BuildSelectListItem(-1, ProblemAccountResource.PleaseSelect, true),
                    BuildSelectListItem(InformStatusConst.FirstWeek, $"{ProblemAccountResource._1stWeekInformedBy} {Updater}"),
                    BuildSelectListItem(InformStatusConst.SecondWeek, $"{ProblemAccountResource._2ndWeekInformedBy} {Updater}"),
                    BuildSelectListItem(InformStatusConst.BothWeeks, $"{ProblemAccountResource.BothWeeksInformedBy} {Updater}"),
                    BuildSelectListItem(InformStatusConst.NoneWeek, ProblemAccountResource.NoneWeekInformed)
                };
            }
        }

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
