namespace Fanex.Nap.CMS.CustomerClass.Criteria
{
    internal class GetSiteSettingCriteria : QueryCriteriaBase
    {
        public string Site { get; set; }

        public int PolicyId { get; set; }

        public string Platform { get; set; }

        public string SportTypes { get; set; }

        public int PageIndex { get; set; }

        public int PageSize { get; set; }

        public int TotalItems { get; set; }

        public override string GetSettingKey()
            => "NAP_LM_CustomerClassPolicySiteSel";
    }
}
