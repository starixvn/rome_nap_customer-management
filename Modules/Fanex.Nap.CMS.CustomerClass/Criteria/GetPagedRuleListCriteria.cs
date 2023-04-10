namespace Fanex.Nap.CMS.CustomerClass.Criteria
{
    using Fanex.Nap.CMS.CustomerClass.Requests;

    internal class GetPagedRuleListCriteria : QueryCriteriaBase
    {
        public GetPagedRuleListCriteria(SearchCustomerClassSettingRequest request)
        {
            Site = request.Site;
            CustomerClass = request.CustomerClass;
            SportType = request.SportType;
            PageIndex = request.PageIndex;
            PageSize = request.PageSize;
        }

        public string Site { get; }

        public short CustomerClass { get; }

        public short SportType { get; }

        public int PageIndex { get; }

        public int PageSize { get; }

        public int TotalItems { get; }

        public override string GetSettingKey()
            => "NAP_LM_CustomerClassSetting";
    }
}
