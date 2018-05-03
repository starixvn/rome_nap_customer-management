namespace Fanex.Nap.CMS.PayoutManagement.Criteria
{
    using System;
    using Fanex.Data.Repository;

    public class GetJackpotBetTicketCriteria : CriteriaBase
    {
        public long RefNo { get; set; }

        public DateTime MoveBetsDay { get; set; }

        public DateTime BalanceUpDay { get; set; }

        public override string GetSettingKey()
        {
            return "NAP_PayoutTrans_Jackpot_SelBetTicket";
        }

        public override bool IsValid()
        {
            return RefNo > 0;
        }
    }
}
