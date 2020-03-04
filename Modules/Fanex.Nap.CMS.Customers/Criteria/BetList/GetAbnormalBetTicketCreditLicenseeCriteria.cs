namespace Fanex.Nap.CMS.Customers.Criteria.BetList
{
    using System;

    public class GetAbnormalBetTicketCreditLicenseeCriteria : GetAbnormalBetTicketCriteria
    {
        public GetAbnormalBetTicketCreditLicenseeCriteria(
            int custId,
            DateTime from,
            DateTime to,
            int pageIndex,
            int pageSize) : base(custId, from, to, pageIndex, pageSize)
        {
        }

        public override string GetSettingKey()
            => "NAP_ProblemAccount_VoidedTransactionList";
    }
}
