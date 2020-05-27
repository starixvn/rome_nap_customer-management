namespace Fanex.NAP.CMS.BetLimitTypeControl.Criteria
{
    using Fanex.Data.Repository;

    public class SearchCriteria : CriteriaBase
    {
        public int UserId { get; set; }

        public byte ProductId { get; set; }

        public short SportType { get; set; }

        public short BetType { get; set; }

        public byte LiveIndicator { get; set; }

        public override bool IsValid() => UserId > 0 && ProductId >= 0 && SportType >= 0 && BetType >= 0 && LiveIndicator >= 0;

        public override string GetSettingKey() => "NAP_BLControl_TypeSelect";
    }
}
