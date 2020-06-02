namespace Fanex.NAP.CMS.BetLimitControl.Criteria
{
    using Fanex.Data.Repository;

    public class GetAllBetLimitTypeCriteria : CriteriaBase
    {
        public override string GetSettingKey() => "NAP_BLControl_AllTypeSelect";

        public override bool IsValid() => true;
    }
}
