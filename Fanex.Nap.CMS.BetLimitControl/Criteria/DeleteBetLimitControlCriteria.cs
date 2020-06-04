using Fanex.Data.Repository;

namespace Fanex.NAP.CMS.BetLimitControl.Criteria
{
    public class DeleteBetLimitControlCriteria : CriteriaBase
    {
        public int CustID { get; set; }

        public int BetLimitTypeID { get; set; }

        public int UserId { get; set; }

        public string IP { get; set; }

        public int ErrorCode { get; set; }

        public string ErrorMsg { get; set; }

        public override string GetSettingKey()
            => "NAP_BLControl_CustDelete";

        public override bool IsValid()
            => CustID > 0 && UserId > 0 && BetLimitTypeID > 0 && !string.IsNullOrWhiteSpace(IP);
    }
}