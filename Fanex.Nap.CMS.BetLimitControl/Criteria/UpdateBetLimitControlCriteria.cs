namespace Fanex.NAP.CMS.BetLimitControl.Criteria
{
    using Fanex.Data.Repository;

    public class UpdateBetLimitControlCriteria : CriteriaBase
    {
        public int CustID { get; set; }

        public int BetLimitTypeID { get; set; }

        public int NewBetLimitTypeID { get; set; }

        public decimal MaxBet { get; set; }

        public decimal MaxPerMatch { get; set; }

        public byte Mode { get; set; }

        public int UserId { get; set; }

        public string IP { get; set; }

        public int ErrorCode { get; set; }

        public string ErrorMsg { get; set; }

        public override string GetSettingKey()
            => "NAP_BLControl_CustUpdate";

        public override bool IsValid()
            => CustID > 0 && BetLimitTypeID > 0 && NewBetLimitTypeID > 0 && MaxBet <= MaxPerMatch && Utils.IsValidMode(Mode) && UserId > 0 && !string.IsNullOrWhiteSpace(IP);
    }
}