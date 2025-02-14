namespace Fanex.NAP.CMS.BetLimitTypeControl.Criteria
{
    using Fanex.Data.Repository;

    public class GetSportBettypeCriteria : CriteriaBase
    {
        public override string GetSettingKey()
            => "Acc_CustomerBLCGetSportBettype";

        public override bool IsValid()
            => true;
    }
}
