namespace Fanex.Nap.CMS.Customers.Criteria.VirtualRateEngine
{
    public class AddNewVREngineRuleCriteria : VREngineCriteriaBase
    {
        public string RuleName { get; set; }

        public int Priority { get; set; }

        public byte? SiteType { get; set; }

        public string SiteIDList { get; set; }

        public string CustomerClassList { get; set; }

        public string CustomerClassDetailList { get; set; }

        public string CCSportTypeList { get; set; }

        public decimal? VR_From { get; set; }

        public decimal? VR_To { get; set; }

        public byte? VRStatus { get; set; }

        public string SporttypeList { get; set; }

        public string BettypeList { get; set; }

        public byte? LiveIndicator { get; set; }

        public string LeagueGroupList { get; set; }

        public string LeagueList { get; set; }

        public string Betfrom { get; set; }

        public decimal? StakeRM_From { get; set; }

        public decimal? StakeRM_To { get; set; }

        public string DateOfWeekList { get; set; }

        public string BeginTime { get; set; }

        public string EndTime { get; set; }

        public string CurrencyIDList { get; set; }

        public decimal TimeBeforeKO { get; set; }

        public byte Adjustmenttype { get; set; }

        public decimal AdjustmentValue { get; set; }

        public byte AccumulatedBalance { get; set; }

        public byte Status { get; set; }

        public string Criteria_NotApply { get; set; }

        public string Enabled_Columns { get; set; }

        public string JsonCheckData { get; set; }

        public override string GetSettingKey()
            => "NAP_VREngine_Rule_Insert";
    }
}
