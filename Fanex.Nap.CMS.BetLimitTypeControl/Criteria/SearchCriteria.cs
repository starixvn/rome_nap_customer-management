namespace Fanex.NAP.CMS.BetLimitTypeControl.Criteria
{
    using Fanex.Data.Repository;

    public class SearchCriteria : CriteriaBase
    {
        public byte ProductId { get; set; }

        public short SportType { get; set; }

        public short BetType { get; set; }

        public byte LiveIndicator { get; set; }

        public override bool IsValid() => SportType >= 0 && BetType >= 0 && LiveIndicator >= 0;

        public override string GetSettingKey() => "NAP_BLControl_TypeSelect";
    }
}
