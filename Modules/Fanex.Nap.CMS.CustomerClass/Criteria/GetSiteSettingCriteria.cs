namespace Fanex.Nap.CMS.CustomerClass.Criteria
{
    using Fanex.Nap.CMS.CustomerClass.Requests;

    internal class GetSiteSettingCriteria : QueryCriteriaBase
    {
        public GetSiteSettingCriteria(GetSiteSettingRequest request, string sportTypes)
        {
            Site = request.Sites ?? string.Empty;
            PolicyId = request.PolicyId;
            Platform = request.Platforms ?? string.Empty;
            PageIndex = request.PageIndex;
            PageSize = request.PageSize;
            SportTypes = sportTypes;
        }

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
