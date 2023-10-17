namespace Fanex.Nap.CMS.Infrastructure.Criteria
{
    using Fanex.NAP.Framework.Data;

    internal class GetSiteMarketCodeCriteria : CriteriaBase
    {
        public override string GetSettingKey()
            => "NAP_Domain_SiteMarket";
    }
}
