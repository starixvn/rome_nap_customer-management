namespace Fanex.Nap.CMS.Customers.ViewModels.VirtualRateEngine
{
    using System.Collections.Generic;
    using System.Linq;
    using Fanex.Nap.CMS.Customers.Constants.ProblemAccount;
    using Fanex.Nap.CMS.Customers.Constants.VirtualRateEngine;
    using Fanex.Nap.CMS.Customers.Models.VirtualRateEngine;
    using Fanex.Nap.CMS.Customers.Resources;
    using Fanex.Nap.CMS.Infrastructure.Models;

    public class VREngineMatrixViewModel : VREngineMatrix
    {
        public VREngineMatrixViewModel(VREngineMatrix matrix) : base(
            matrix.CreditSites,
            matrix.LicenseeSites,
            matrix.DetailSports.Select(s => BuildIdNameModel(s.Id, $"[{s.Id}] {s.Name}")),
            matrix.BetfromList,
            matrix.Sports.Select(s => BuildIdNameModel(s.Id, $"[{s.Id}] {s.Name}")),
            matrix.BetTypes.Select(b => BuildIdNameModel(b.Id, $"[{b.Id}] {b.Name}")),
            matrix.LeagueGroups, matrix.Currencies)
        {
        }

        public IEnumerable<IdNameModel> Conditions { get; }
            = new[]
            {
                new IdNameModel { Id = ConditionConst.In, Name = VREngineResource.In },
                new IdNameModel { Id = ConditionConst.NotIn, Name = VREngineResource.NotIn }
            };

        public IEnumerable<IdNameModel> SiteTypes { get; }
            = new[]
            {
                new IdNameModel { Id = 0, Name = Labels.All },
                new IdNameModel { Id = SiteTypeConst.Credit, Name = nameof(SiteTypeConst.Credit) },
                new IdNameModel { Id = SiteTypeConst.Licensee, Name = nameof(SiteTypeConst.Licensee) }
            };

        public IEnumerable<IdNameModel> CurrentVRStatus { get; }
            = new[]
            {
                BuildIdNameModel(CurrentVRStatusConst.Enable, nameof(CurrentVRStatusConst.Enable)),
                BuildIdNameModel(CurrentVRStatusConst.Disable, nameof(CurrentVRStatusConst.Disable))
            };

        public IEnumerable<IdNameModel> Events { get; }
            = new[]
            {
                BuildIdNameModel(EventConst.Live, nameof(EventConst.Live)),
                BuildIdNameModel(EventConst.NonLive, VREngineResource.NonLive)
            };

        public IEnumerable<IdNameModel> DayOfWeeks { get; } = VREngineDayOfWeek.DayOfWeeks;

        public IEnumerable<IdNameModel> Adjustments { get; }
            = new[]
            {
                BuildIdNameModel(AdjustmentConst.AdjustCurrentVirtualRate, VREngineResource.AdjustCurrentVirtualRate),
                BuildIdNameModel(AdjustmentConst.FixedValue, VREngineResource.FixedValue)
            };

        public IEnumerable<IdNameModel> Actives { get; }
            = new[]
            {
                BuildIdNameModel(ActiveConst.UAT, nameof(ActiveConst.UAT)),
                BuildIdNameModel(ActiveConst.PROD, nameof(ActiveConst.PROD)),
                BuildIdNameModel(ActiveConst.Inactive, nameof(ActiveConst.Inactive))
            };

        private static IdNameModel BuildIdNameModel(int id, string name)
            => new IdNameModel { Id = id, Name = name };
    }
}
