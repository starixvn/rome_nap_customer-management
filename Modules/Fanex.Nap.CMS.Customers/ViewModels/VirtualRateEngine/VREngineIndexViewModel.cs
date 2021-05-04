using System;
using System.Collections.Generic;
using Fanex.Nap.CMS.Customers.Constants.ProblemAccount;
using Fanex.Nap.CMS.Customers.Constants.VirtualRateEngine;
using Fanex.Nap.CMS.Customers.Models.VirtualRateEngine;
using Fanex.Nap.CMS.Customers.Resources;
using Fanex.Nap.CMS.Infrastructure.Models;

namespace Fanex.Nap.CMS.Customers.ViewModels.VirtualRateEngine
{
    public class VREngineIndexViewModel
    {
        public VREngineIndexViewModel(
            bool hasHostVirtualRateEnginePermission,
            bool hasHostFullPermission,
            IEnumerable<IdNameModel> creditSites,
            IEnumerable<IdNameModel> licenseeSites,
            IEnumerable<IdNameModel> customerClasses,
            IEnumerable<IdNameModel> sports,
            IEnumerable<IdNameModel> betTypes,
            IEnumerable<IdNameModel> leagueGroups,
            IEnumerable<VREngineBetfrom> betfromList)
        {
            HasHostVirtualRateEnginePermission = hasHostVirtualRateEnginePermission;
            HasHostFullPermission = hasHostFullPermission;
            CreditSites = creditSites;
            LicenseeSites = licenseeSites;
            CustomerClasses = customerClasses;
            Sports = sports;
            BetTypes = betTypes;
            LeagueGroups = leagueGroups;
            BetfromList = betfromList;
        }

        public bool HasHostVirtualRateEnginePermission { get; }

        public bool HasHostFullPermission { get; }

        public IEnumerable<IdNameModel> Conditions
            => new[]
            {
                new IdNameModel { Id = ConditionConst.In, Name = VREngineResource.In },
                new IdNameModel { Id = ConditionConst.NotIn, Name = VREngineResource.NotIn }
            };

        public IEnumerable<IdNameModel> SiteTypes
            => new[]
            {
                new IdNameModel { Id = 0, Name = Labels.All },
                new IdNameModel { Id = SiteTypeConst.Credit, Name = nameof(SiteTypeConst.Credit) },
                new IdNameModel { Id = SiteTypeConst.Licensee, Name = nameof(SiteTypeConst.Licensee) }
            };

        public IEnumerable<IdNameModel> CreditSites { get; }

        public IEnumerable<IdNameModel> LicenseeSites { get; }

        public IEnumerable<IdNameModel> CustomerClasses { get; }

        public IEnumerable<IdNameModel> CurrentVRStatus
            => new[]
            {
                BuildIdNameModel(CurrentVRStatusConst.Enable, nameof(CurrentVRStatusConst.Enable)),
                BuildIdNameModel(CurrentVRStatusConst.Disable, nameof(CurrentVRStatusConst.Disable))
            };

        public IEnumerable<IdNameModel> Sports { get; }

        public IEnumerable<IdNameModel> BetTypes { get; }

        public IEnumerable<IdNameModel> Events
            => new[]
            {
                BuildIdNameModel(EventConst.Live, nameof(EventConst.Live)),
                BuildIdNameModel(EventConst.NonLive, VREngineResource.NonLive)
            };

        public IEnumerable<IdNameModel> LeagueGroups { get; }

        public IEnumerable<VREngineBetfrom> BetfromList { get; }

        public IEnumerable<IdNameModel> DateOfWeeks
             => new[]
            {
                BuildIdNameModel((int)DayOfWeek.Monday + 1, nameof(DayOfWeek.Monday)),
                BuildIdNameModel((int)DayOfWeek.Tuesday + 1, nameof(DayOfWeek.Tuesday)),
                BuildIdNameModel((int)DayOfWeek.Wednesday + 1, nameof(DayOfWeek.Wednesday)),
                BuildIdNameModel((int)DayOfWeek.Thursday + 1, nameof(DayOfWeek.Thursday)),
                BuildIdNameModel((int)DayOfWeek.Friday + 1, nameof(DayOfWeek.Friday)),
                BuildIdNameModel((int)DayOfWeek.Saturday + 1, nameof(DayOfWeek.Saturday)),
                BuildIdNameModel((int)DayOfWeek.Sunday + 1, nameof(DayOfWeek.Sunday))
            };

        public IEnumerable<IdNameModel> Adjustments
            => new[]
            {
                BuildIdNameModel(AdjustmentConst.AdjustCurrentVirtualRate, VREngineResource.AdjustCurrentVirtualRate),
                BuildIdNameModel(AdjustmentConst.FixedValue, VREngineResource.FixedValue)
            };

        public IEnumerable<IdNameModel> Actives
            => new[]
            {
                BuildIdNameModel(ActiveConst.UAT, nameof(ActiveConst.UAT)),
                BuildIdNameModel(ActiveConst.PROD, nameof(ActiveConst.PROD)),
                BuildIdNameModel(ActiveConst.Inactive, nameof(ActiveConst.Inactive))
            };

        private static IdNameModel BuildIdNameModel(int id, string name)
            => new IdNameModel { Id = id, Name = name };
    }
}
