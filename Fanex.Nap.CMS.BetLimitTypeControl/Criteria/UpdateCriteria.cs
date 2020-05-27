namespace Fanex.NAP.CMS.BetLimitTypeControl.Criteria
{
    public class UpdateCriteria : InsertCriteria
    {
        public int BetLimitTypeId { get; set; }

        public override bool IsValid() => base.IsValid() && BetLimitTypeId > 0;

        public override string GetSettingKey() => "NAP_BLControl_TypeUpdate";
    }
}
