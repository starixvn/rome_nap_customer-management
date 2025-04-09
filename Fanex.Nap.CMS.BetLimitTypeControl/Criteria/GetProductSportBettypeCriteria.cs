using Fanex.Data.Repository;

namespace Fanex.NAP.CMS.BetLimitTypeControl.Criteria
{
    public class GetProductSportBettypeCriteria : CriteriaBase
    {
        public override string GetSettingKey()
            => "NAP_BLControl_SportBettypeSel";

        public override bool IsValid()
            => true;
    }
}
