namespace Fanex.Nap.CMS.Customers.Criteria.VirtualRateEngine
{
    using Fanex.Data.Repository;
    using Fanex.Nap.CMS.Customers.ViewModels.VirtualRateEngine;

    public class CheckVREngineRuleCriteria : NonQueryCommand
    {
        public CheckVREngineRuleCriteria(AddNewRuleViewModel vm, string jsonSportList)
        {
            CustomerClassList = vm.CustomerClass;
            CustomerClassDetailList = vm.CustomerClassDetail;
            CCSportTypeList = vm.CCSportType;
            SporttypeList = vm.SportType;
            BettypeList = vm.BetType;
            LeagueGroupList = vm.LeagueGroup;
            LeagueList = vm.League;
            DateOfWeekList = vm.DayOfWeek;
            CurrencyIDList = vm.CurrencyIDList;
            Criteria_NotApply = vm.CriteriaNotApply;
            SiteIDList = vm.Site;
            JsonSportList = jsonSportList;
        }

        public string CustomerClassList { get; }
        public string CustomerClassDetailList { get; }
        public string CCSportTypeList { get; }
        public string SporttypeList { get; }
        public string SiteIDList { get; }
        public string BettypeList { get; }
        public string LeagueGroupList { get; }
        public string LeagueList { get; }
        public string DateOfWeekList { get; }
        public string CurrencyIDList { get; set; }
        public string Criteria_NotApply { get; }
        public string JsonSportList { get; }
        public int ErrorCode { get; set; }
        public string ErrorMsg { get; set; }
        public override string GetSettingKey()
            => "NAP_VREngine_CriteriaMatrix_Check";

        public override bool IsValid()
            => true;
    }
}
