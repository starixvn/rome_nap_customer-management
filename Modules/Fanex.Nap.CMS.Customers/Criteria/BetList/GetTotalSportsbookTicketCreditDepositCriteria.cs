namespace Fanex.Nap.CMS.Customers.Criteria.BetList
{
    using System;

    public class GetTotalSportsbookTicketCreditDepositCriteria : GetTotalSportsbookTicketCriteria
    {
        public GetTotalSportsbookTicketCreditDepositCriteria(
            int custId,
            DateTime from,
            DateTime to,
            DateTime moveBetsDay,
            DateTime balanceUpDay) : base(custId, from, to, moveBetsDay)
        {
            BalanceUpDay = balanceUpDay;
        }

        public DateTime BalanceUpDay { get; }

        public override string GetSettingKey()
            => "NAP_ProblemAccount_TotalSportsbookTickets";
    }
}
