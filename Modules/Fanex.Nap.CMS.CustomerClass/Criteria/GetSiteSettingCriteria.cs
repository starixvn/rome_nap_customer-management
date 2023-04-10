namespace Fanex.Nap.CMS.CustomerClass.Criteria
{
    using Fanex.Nap.CMS.CustomerClass.Requests;

    internal class GetSiteSettingCriteria : QueryCriteriaBase
    {
        public GetSiteSettingCriteria(GetSiteSettingRequest request)
        {
            Site = request.Sites ?? string.Empty;
            PolicyId = request.PolicyId;
            Platform = request.Platforms ?? string.Empty;
            PageIndex = request.PageIndex;
            PageSize = request.PageSize;
        }

        public string Site { get; }

        public int PolicyId { get; }

        public string Platform { get; }

        public int PageIndex { get; }

        public int PageSize { get; }

        public int TotalItems { get; }

        public override string GetSettingKey()
            => "NAP_LM_CustomerClassPolicySiteSel";
    }
}
