namespace Fanex.Nap.CMS.Customers.Criteria.VirtualRateEngine
{
    using Fanex.Nap.CMS.Customers.ViewModels.VirtualRateEngine;

    internal class AddNewVREngineRuleCriteria : VREngineCriteriaBase
    {
        public AddNewVREngineRuleCriteria(AddNewRuleViewModel vm)
            : base(vm.UserId, vm.IP)
        {
            RuleName = vm.RuleTitle;
            Priority = vm.PriorityOrder;

            SiteType = vm.SiteType;
            SiteIDList = vm.Site;
            CustomerClassList = vm.CustomerClass;
            CustomerClassDetailList = vm.CustomerClassDetail;
            CCSportTypeList = vm.CCSportType;
            VR_From = vm.VRFrom;
            VR_To = vm.VRTo;
            VRStatus = 2;
            SporttypeList = vm.SportType;
            BettypeList = vm.BetType;
            LiveIndicator = vm.LiveIndicator;
            LeagueGroupList = vm.LeagueGroup;
            LeagueList = vm.League;
            Betfrom = vm.Betfrom;
            StakeRM_From = vm.StakeFrom;
            StakeRM_To = vm.StakeTo;
            DateOfWeekList = vm.DayOfWeek;
            BeginTime = vm.BeginTime;
            EndTime = vm.EndTime;
            CurrencyIDList = vm.CurrencyIDList;

            Adjustmenttype = vm.AdjustmentType;
            AdjustmentValue = vm.AdjustmentValue;
            AccumulatedBalance = vm.AccumulatedBalance;
            Status = vm.Active;

            Criteria_NotApply = vm.CriteriaNotApply;
            Enabled_Columns = vm.EnabledColumns;
        }

        public string RuleName { get; }

        public int Priority { get; }

        public byte? SiteType { get; }

        public string SiteIDList { get; }

        public string CustomerClassList { get; }

        public string CustomerClassDetailList { get; }

        public string CCSportTypeList { get; }

        public decimal? VR_From { get; }

        public decimal? VR_To { get; }

        public byte? VRStatus { get; }

        public string SporttypeList { get; }

        public string BettypeList { get; }

        public byte? LiveIndicator { get; }

        public string LeagueGroupList { get; }

        public string LeagueList { get; }

        public string Betfrom { get; }

        public decimal? StakeRM_From { get; }

        public decimal? StakeRM_To { get; }

        public string DateOfWeekList { get; }

        public string BeginTime { get; }

        public string EndTime { get; }

        public string CurrencyIDList { get; set; }

        public byte Adjustmenttype { get; }

        public decimal AdjustmentValue { get; }

        public byte AccumulatedBalance { get; }

        public byte Status { get; }

        public string Criteria_NotApply { get; }

        public string Enabled_Columns { get; }

        public override string GetSettingKey()
            => "NAP_VREngine_Rule_Insert";
    }
}
