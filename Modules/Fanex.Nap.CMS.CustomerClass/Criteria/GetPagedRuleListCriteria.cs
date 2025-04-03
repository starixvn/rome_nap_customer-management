namespace Fanex.Nap.CMS.CustomerClass.Criteria
{
    internal class GetPagedRuleListCriteria : QueryCriteriaBase
    {
        public string Site { get; set; }

        public string CustomerClass { get; set; }

        public string CustomerClassDetail { get; set; }

        public string SportTypes { get; set; }

        public string LeagueGroups { get; set; }

        public string Currencies { get; set; }

        public int PageIndex { get; set; }

        public int PageSize { get; set; }

        public int TotalItems { get; set; }

        public int ErrCode { get; set; }

        public string ErrMsg { get; set; }

        public override string GetSettingKey()
            => "NAP_CM_CustomerClassSetting";
    }
}
