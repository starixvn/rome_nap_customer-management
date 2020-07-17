namespace Fanex.Nap.CMS.Customers.ViewModels.ProblemAccount
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using Constants.ProblemAccount;
    using Fanex.Nap.CMS.Customers.Resources;
    using Infrastructure.Constants;
    using Infrastructure.Models;

    public abstract class IndexBaseViewModel
    {
        public const int MaxInputUsernames = 100;

        protected IndexBaseViewModel(
            bool hasHostPermission,
            bool autoReload,
            SearchBaseViewModel searchViewModel,
            IEnumerable<IdNameModel> filterReasons,
            IEnumerable<IdNameModel> markNewReasons,
            DateTime minFromDate,
            IEnumerable<IdNameModel> currencies,
            string updater)
        {
            HasHostPermission = hasHostPermission;
            AutoReload = autoReload;
            SearchFilter = searchViewModel;
            Reasons = BuildFilterReasons(filterReasons, SearchFilter.Reason);
            MarkNewReasons = BuildSelectListItem(new IdNameModel { Id = 0, Name = ProblemAccountResource.PleaseSelect }, markNewReasons);
            MinFromDate = minFromDate;
            Currencies = currencies.Select(c => BuildSelectListItem(c));
            Updater = updater;
        }

        public bool HasHostPermission { get; }

        public bool AutoReload { get; }

        public DateTime MinFromDate { get; }

        public DateTime CurrentDate { get; } = DateTime.Today;

        public SearchBaseViewModel SearchFilter { get; }

        public IEnumerable<SelectListItem> Levels { get; } = new[]
        {
            BuildSelectListItem(RoleIdConstants.Super, nameof(RoleIdConstants.Super)),
            BuildSelectListItem(RoleIdConstants.Master, nameof(RoleIdConstants.Master)),
            BuildSelectListItem(RoleIdConstants.Agent, nameof(RoleIdConstants.Agent)),
            BuildSelectListItem(RoleIdConstants.Member, nameof(RoleIdConstants.Member))
        };

        public IEnumerable<SelectListItem> MarkNewLevels
            => new[] { BuildSelectListItem(0, ProblemAccountResource.PleaseSelect) }.Concat(Levels).ToList();

        public IEnumerable<SelectListItem> Currencies { get; }

        public IEnumerable<SelectListItem> IsReduced
            => IsReducedConst.IsReducedList().Select(r => BuildSelectListItem(r));

        public IEnumerable<SelectListItem> Reasons { get; }

        public IEnumerable<SelectListItem> MarkNewReasons { get; }

        public IEnumerable<SelectListItem> DirectlyMarked
            => DirectlyMarkedConst.DirectlyMarkedList().Select(m => BuildSelectListItem(m));

        public IEnumerable<SelectListItem> OrderBy
            => OrderByConst.OrderByList().Select(o => BuildSelectListItem(o));

        public string Updater { get; }

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

        protected static SelectListItem BuildSelectListItem(IdNameModel model)
            => BuildSelectListItem(model.Id, model.Name);

        protected static SelectListItem BuildSelectListItem(int value, string name, bool selected = false)
            => new SelectListItem { Value = value.ToString(), Text = name, Selected = selected };

        protected static SelectListItem BuildSelectListItem(string name, bool selected = false)
            => new SelectListItem { Value = name, Text = name, Selected = selected };

        protected static IEnumerable<SelectListItem> BuildSelectListItem(IdNameModel firstItem, IEnumerable<IdNameModel> models)
        {
            var listItems = models.Select(r => BuildSelectListItem(r)).ToList();
            listItems.Insert(0, BuildSelectListItem(firstItem));

            return listItems;
        }

        protected static IEnumerable<SelectListItem> BuildWinningTimes()
        {
            var listItems = new List<SelectListItem>();
            listItems.Add(new SelectListItem { Text = ProblemAccountResource.PleaseSelect, Value = "0", Selected = true });
            listItems.Add(new SelectListItem { Text = "23:59", Value = "23:59" });

            return listItems;
        }

        protected static IEnumerable<SelectListItem> BuildSelectListItem(IdNameModel firstItem, IEnumerable<string> models)
        {
            var listItems = models.Select(r => BuildSelectListItem(r)).ToList();
            listItems.Insert(0, BuildSelectListItem(firstItem));

            return listItems;
        }

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
    }
}
