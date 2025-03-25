namespace Fanex.Nap.CMS.Infrastructure.Criteria
{
    using Fanex.Data.Repository;

    internal class GetSiteMarketCodeCriteria : CriteriaBase
    {
        public override string GetSettingKey()
            => "NAP_Domain_SiteMarket";

        public override bool IsValid()
            => true;
    }
}
