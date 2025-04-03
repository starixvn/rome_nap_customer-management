namespace Fanex.Nap.CMS.Customers.Criteria.VirtualRateEngine
{
    using Fanex.Data.Repository;
    public class CheckVREngineRuleCriteria : NonQueryCommand
    {
        public string CustomerClassList { get; set; }
        public string CustomerClassDetailList { get; set; }
        public string CCSportTypeList { get; set; }
        public string SporttypeList { get; set; }
        public string SiteIDList { get; set; }
        public string BettypeList { get; set; }
        public string LeagueGroupList { get; set; }
        public string LeagueList { get; set; }
        public string DateOfWeekList { get; set; }
        public string CurrencyIDList { get; set; }
        public string Criteria_NotApply { get; set; }
        public string JsonSportList { get; set; }
        public int ErrorCode { get; set; }
        public string ErrorMsg { get; set; }
        public override string GetSettingKey()
            => "NAP_VREngine_CriteriaMatrix_Check";

        public override bool IsValid()
            => true;
    }
}
