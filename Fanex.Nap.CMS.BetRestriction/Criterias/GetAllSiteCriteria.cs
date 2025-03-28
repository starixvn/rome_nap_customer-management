namespace Fanex.Nap.CMS.BetRestriction.Criterias
{
    using Fanex.Data.Repository;

    public class GetAllSiteCriteria : CriteriaBase
    {
        public override string GetSettingKey()
        {
            return "NAP_BetRestriction_SelAllSite";
        }

        public override bool IsValid()
            => true;
    }
}
