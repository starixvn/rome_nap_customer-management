namespace Fanex.NAP.CMS.BetLimitControl.Criteria.BetLimitIncrease
{
    public class UpdateBetLimitIncreaseCriteria : AddBetLimitIncreaseCriteria
    {
        public override string GetSettingKey()
            => "NAP_CM_SVIPMaxBetPlayerUpd";
    }
}
