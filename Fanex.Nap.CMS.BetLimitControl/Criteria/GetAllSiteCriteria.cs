namespace Fanex.NAP.CMS.BetLimitControl.Criteria
{
    using Fanex.Data.Repository;

    public class GetAllSiteCriteria : CriteriaBase
    {
        public override string GetSettingKey() => "NAP_BetRestriction_SelAllSite";

        public override bool IsValid() => true;
    }
}
