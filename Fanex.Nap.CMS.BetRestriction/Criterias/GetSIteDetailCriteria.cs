namespace Fanex.Nap.CMS.BetRestriction.Criterias
{
    using Fanex.Data.Repository;

    public class GetSiteDetailCriteria : CriteriaBase
    {
        public int SiteId { get; set; }

        public override string GetSettingKey()
        {
            return "NAP_BetRestriction_SiteSelDetail";
        }
    }
}
