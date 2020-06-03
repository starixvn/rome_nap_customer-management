using Fanex.Data.Repository;

namespace Fanex.NAP.CMS.BetLimitControl.Criteria
{
    public class UpdateBetLimitControlCriteria : CriteriaBase
    {
        public int CustID { get; set; }

        public int? BetLimitTypeID { get; set; }

        public int? NewBetLimitTypeID { get; set; }

        public decimal MaxBet { get; set; }

        public decimal MaxPerMatch { get; set; }

        public int UserId { get; set; }

        public string IP { get; set; }

        public int ErrorCode { get; set; }

        public string ErrorMsg { get; set; }

        public override string GetSettingKey()
            => "NAP_BLControl_CustUpdate";

        public override bool IsValid()
            => CustID > 0;
    }
}