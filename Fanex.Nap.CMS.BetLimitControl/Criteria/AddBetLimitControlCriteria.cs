using Fanex.Data.Repository;

namespace Fanex.NAP.CMS.BetLimitControl.Criteria
{
    public class AddBetLimitControlCriteria : CriteriaBase
    {
        public string Username { get; set; }

        public int BetLimitTypeID { get; set; }

        public decimal MaxBet { get; set; }

        public decimal MaxPerMatch { get; set; }

        public int UserId { get; set; }

        public string IP { get; set; }

        public int ErrorCode { get; set; }

        public string ErrorMsg { get; set; }

        public override string GetSettingKey()
            => "NAP_BLControl_CustInsert";

        public override bool IsValid()
            => !string.IsNullOrWhiteSpace(Username) && UserId > 0;
    }
}