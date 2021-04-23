using System.Collections.Generic;
using Fanex.Nap.CMS.Customers.Constants.ProblemAccount;
using Fanex.Nap.CMS.Customers.Constants.VirtualRateEngine;
using Fanex.Nap.CMS.Customers.Models.ManageRobots2;
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
            IEnumerable<IdNameModel> licenseeSites)
        {
            HasHostVirtualRateEnginePermission = hasHostVirtualRateEnginePermission;
            HasHostFullPermission = hasHostFullPermission;
            CreditSites = creditSites;
            LicenseeSites = licenseeSites;
        }

        public bool HasHostVirtualRateEnginePermission { get; }

        public bool HasHostFullPermission { get; }

        public IEnumerable<IdNameModel> Conditions
            => new[]
            {
                new IdNameModel { Id = ConditionConst.In, Name = "in" },
                new IdNameModel { Id = ConditionConst.NotIn, Name = "not in" }
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

        public IEnumerable<IdNameModel> Adjustments
            => new[]
            {
                BuildIdNameModel(AdjustmentConst.AdjustCurrentVirtualRate, "Adjust Current Virtual Rate"),
                BuildIdNameModel(AdjustmentConst.FixedValue, "Fixed Value")
            };

        public IEnumerable<IdNameModel> Actives
            => new[]
            {
                BuildIdNameModel(ActiveConst.PROD, nameof(ActiveConst.PROD)),
                BuildIdNameModel(ActiveConst.UAT, nameof(ActiveConst.UAT)),
                BuildIdNameModel(ActiveConst.Inactive, nameof(ActiveConst.Inactive))
            };

        private static IdNameModel BuildIdNameModel(int id, string name)
            => new IdNameModel { Id = id, Name = name };
    }
}
