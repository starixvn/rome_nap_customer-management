namespace Fanex.Nap.CMS.Customers.ViewModels.ProblemAccount
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using Constants;
    using Infrastructure.Constants;
    using Resources;

    public class IndexViewModel
    {
        public const int MaxInputUsernames = 100;
        public const int MaxPerEvent = 5000;
        public const int MaxPerBet = 1000;
        // The release date of this feature.
        public static readonly DateTime DefaultMinDate = new DateTime(2019, 8, 1);

        private IEnumerable<SelectListItem> markNewReasons;

        public IndexViewModel(bool hasHostPermission, SearchViewModel searchViewModel)
        {
            HasHostPermission = hasHostPermission;
            AutoReload = searchViewModel != null;
            SearchFilter= searchViewModel ?? new SearchViewModel();
        }

        public bool HasHostPermission { get; }

        public bool AutoReload { get; }

        public SearchViewModel SearchFilter { get; }

        public DateTime CurrentDate { get; } = DateTime.Today;

        public IEnumerable<SelectListItem> Levels
        {
            get
            {
                var selectedLevel = SearchFilter.Level;

                return new[]
                {
                    BuildSelectListItem(RoleIdConstants.Headline, nameof(RoleIdConstants.Headline), RoleIdConstants.Headline == selectedLevel),
                    BuildSelectListItem(RoleIdConstants.Super, nameof(RoleIdConstants.Super), RoleIdConstants.Super == selectedLevel),
                    BuildSelectListItem(RoleIdConstants.Master, nameof(RoleIdConstants.Master), RoleIdConstants.Master == selectedLevel),
                    BuildSelectListItem(RoleIdConstants.Agent, nameof(RoleIdConstants.Agent), RoleIdConstants.Agent == selectedLevel),
                    BuildSelectListItem(RoleIdConstants.Member, nameof(RoleIdConstants.Member), RoleIdConstants.Member == selectedLevel)
                };
            }
        }

        public IEnumerable<SelectListItem> Currencies
        {
            get
            {
                var selectedCurrency = SearchFilter.Currency;

                return new[]
                {
                    BuildSelectListItem(CurrencyConst.CY, nameof(CurrencyConst.CY), CurrencyConst.CY == selectedCurrency),
                    BuildSelectListItem(CurrencyConst.UUS, nameof(CurrencyConst.UUS), CurrencyConst.UUS == selectedCurrency)
                };
            }
        }

        public IEnumerable<SelectListItem> AccountStatus
        {
            get
            {
                var selectedStatus = SearchFilter.Status;

                return new[]
                {
                    BuildSelectListItem(0, ProblemAccountResource.All, 0 == selectedStatus),
                    BuildSelectListItem(AccountStatusConst.Active, nameof(AccountStatusConst.Active), AccountStatusConst.Active == selectedStatus),
                    BuildSelectListItem(AccountStatusConst.ViewOnly, ProblemAccountResource.ViewOnly, AccountStatusConst.ViewOnly == selectedStatus),
                    BuildSelectListItem(AccountStatusConst.Suspended, nameof(AccountStatusConst.Suspended), AccountStatusConst.Suspended == selectedStatus),
                    BuildSelectListItem(AccountStatusConst.Inactive, nameof(AccountStatusConst.Inactive), AccountStatusConst.Inactive == selectedStatus)
                };
            }
        }

        public IEnumerable<SelectListItem> IsReduced
        {
            get
            {
                var selectedIsReduced = SearchFilter.IsReduced;

                return new[]
                {
                    BuildSelectListItem(0, ProblemAccountResource.All, null == selectedIsReduced),
                    BuildSelectListItem(IsReducedConst.Yes, nameof(IsReducedConst.Yes), true == selectedIsReduced),
                    BuildSelectListItem(IsReducedConst.No, nameof(IsReducedConst.No), false == selectedIsReduced)
                };
            }
        }

        public IEnumerable<SelectListItem> Reasons
        {
            get
            {
                var reasonHash = ToDictionary(SearchFilter.Reason);

                return new[]
                {
                    BuildSelectListItem(1, "all reasons of void abnormal", reasonHash.ContainsKey(1)),
                    BuildSelectListItem(2, "add manual problem reasons define by CSD", reasonHash.ContainsKey(1))
                };
            }
        }

        public IEnumerable<SelectListItem> OrderBy
        {
            get
            {
                var selectedOrderBy = SearchFilter.OrdeBy;

                return new[]
                {
                    BuildSelectListItem(OrderByConst.SMALine, ProblemAccountResource.SMALine, OrderByConst.SMALine == selectedOrderBy),
                    BuildSelectListItem(OrderByConst.MarkDate, ProblemAccountResource.MarkDate, OrderByConst.MarkDate == selectedOrderBy),
                    BuildSelectListItem(OrderByConst.AccountStatus, ProblemAccountResource.AccountStatus, OrderByConst.AccountStatus == selectedOrderBy)
                };
            }
        }

        public IEnumerable<SelectListItem> MarkNewLevels { get; } = new[]
        {
            BuildSelectListItem(0, ProblemAccountResource.PleaseSelect, true),
            BuildSelectListItem(RoleIdConstants.Super, nameof(RoleIdConstants.Super)),
            BuildSelectListItem(RoleIdConstants.Master, nameof(RoleIdConstants.Master)),
            BuildSelectListItem(RoleIdConstants.Agent, nameof(RoleIdConstants.Agent)),
            BuildSelectListItem(RoleIdConstants.Member, nameof(RoleIdConstants.Member))
        };

        public IEnumerable<SelectListItem> MarkNewReasons
        {
            get
            {
                if (markNewReasons == null)
                {
                    markNewReasons = new[]
                        { BuildSelectListItem(0, ProblemAccountResource.PleaseSelect, true) }
                        .Concat(Reasons.Select(r =>
                        {
                            r.Selected = false;
                            return r;
                        })).ToList();
                }

                return markNewReasons;
            }
        }

        private static SelectListItem BuildSelectListItem(int value, string name, bool selected = false)
            => new SelectListItem { Value = value.ToString(), Text = name, Selected = selected };

        private static IDictionary<byte, bool> ToDictionary(string text)
            => string.IsNullOrWhiteSpace(text)
            ? new Dictionary<byte, bool>()
            : text
                .Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                .ToDictionary(kvp => byte.Parse(kvp), kvp => true);
    }
}
