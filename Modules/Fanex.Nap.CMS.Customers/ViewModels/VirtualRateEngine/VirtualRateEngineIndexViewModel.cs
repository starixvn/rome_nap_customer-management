using System;
using System.Collections.Generic;
using Fanex.Nap.CMS.Customers.Constants.ProblemAccount;
using Fanex.Nap.CMS.Customers.Constants.VirtualRateEngine;
using Fanex.Nap.CMS.Customers.Resources;
using Fanex.Nap.CMS.Infrastructure.Models;

namespace Fanex.Nap.CMS.Customers.ViewModels.VirtualRateEngine
{
    public class VirtualRateEngineIndexViewModel
    {
        public VirtualRateEngineIndexViewModel(
            bool hasHostVirtualRateEnginePermission,
            bool hasHostFullPermission,
            IEnumerable<IdNameModel> creditSites,
            IEnumerable<IdNameModel> licenseeSites,
            IEnumerable<IdNameModel> customerClasses,
            IEnumerable<IdNameModel> sports,
            IEnumerable<IdNameModel> betTypes,
            IEnumerable<IdNameModel> leagueGroups,
            IEnumerable<IdNameModel> leagues,
            IEnumerable<IdNameModel> betfromList)
        {
            HasHostVirtualRateEnginePermission = hasHostVirtualRateEnginePermission;
            HasHostFullPermission = hasHostFullPermission;
            CreditSites = creditSites;
            LicenseeSites = licenseeSites;
            CustomerClasses = customerClasses;
            Sports = sports;
            BetTypes = betTypes;
            LeagueGroups = leagueGroups;
            Leagues = leagues;
            BetfromList = betfromList;
        }

        public bool HasHostVirtualRateEnginePermission { get; }

        public bool HasHostFullPermission { get; }

        public IEnumerable<IdNameModel> Conditions
            => new[]
            {
                new IdNameModel { Id = ConditionConst.In, Name = VirtualRateEngineResource.In },
                new IdNameModel { Id = ConditionConst.NotIn, Name = VirtualRateEngineResource.NotIn }
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

        public IEnumerable<IdNameModel> BetfromList { get; }

        public IEnumerable<IdNameModel> Events
            => new[]
            {
                BuildIdNameModel(EventConst.Live, nameof(EventConst.Live)),
                BuildIdNameModel(EventConst.NonLive, VirtualRateEngineResource.NonLive)
            };

        public IEnumerable<IdNameModel> LeagueGroups { get; }

        public IEnumerable<IdNameModel> Leagues { get; }

        public IEnumerable<IdNameModel> DateOfWeeks
             => new[]
            {
                BuildIdNameModel((byte)DayOfWeek.Monday, nameof(DayOfWeek.Monday)),
                BuildIdNameModel((byte)DayOfWeek.Tuesday, nameof(DayOfWeek.Tuesday)),
                BuildIdNameModel((byte)DayOfWeek.Wednesday, nameof(DayOfWeek.Wednesday)),
                BuildIdNameModel((byte)DayOfWeek.Thursday, nameof(DayOfWeek.Thursday)),
                BuildIdNameModel((byte)DayOfWeek.Friday, nameof(DayOfWeek.Friday)),
                BuildIdNameModel((byte)DayOfWeek.Saturday, nameof(DayOfWeek.Saturday)),
                BuildIdNameModel((byte)DayOfWeek.Sunday, nameof(DayOfWeek.Sunday))
            };

        public IEnumerable<IdNameModel> Adjustments
            => new[]
            {
                BuildIdNameModel(AdjustmentConst.AdjustCurrentVirtualRate, VirtualRateEngineResource.AdjustCurrentVirtualRate),
                BuildIdNameModel(AdjustmentConst.FixedValue, VirtualRateEngineResource.FixedValue)
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
