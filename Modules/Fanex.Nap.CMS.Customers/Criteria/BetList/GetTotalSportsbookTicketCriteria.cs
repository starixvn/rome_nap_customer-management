namespace Fanex.Nap.CMS.Customers.Criteria.BetList
{
    using System;
    using Data.Repository;

    public class GetTotalSportsbookTicketCriteria : CriteriaBase
    {
        public GetTotalSportsbookTicketCriteria(
            int custId,
            DateTime from,
            DateTime to,
            DateTime moveBetsDay)
        {
            CustId = custId;
            FromDate = from;
            ToDate = to;
            MoveBetsDay = moveBetsDay;
        }

        public int CustId { get; }

        public DateTime FromDate { get; }

        public DateTime ToDate { get; }

        public DateTime MoveBetsDay { get; }

        public int TotalBetTickets { get; set; }

        public override string GetSettingKey()
            => "CS_ProblemAccount_TotalSportsbookTickets";
    }
}
