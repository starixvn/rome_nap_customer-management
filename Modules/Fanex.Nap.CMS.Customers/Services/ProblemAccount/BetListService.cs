namespace Fanex.Nap.CMS.Customers.Services.ProblemAccount
{
    using System;
    using System.Collections.Generic;
    using Criteria.BetList;
    using Fanex.Data.Repository;
    using Infrastructure.Web;
    using Models.BetList;
    using Repositories.ProblemAccount;

    public class BetListService : IBetListService
    {
        private readonly IBetListRepository repository;

        public BetListService(IBetListRepository repository = null)
        {
            this.repository = repository ?? new BetListRepository();
        }

        public Tuple<int, IEnumerable<AbnormalBetTicket>> GetAbnormalBetTickets(
            int custId,
            DateTime from,
            DateTime to,
            int pageIndex,
            int pageSize)
            => GetAbnormalBetTickets(new GetAbnormalBetTicketCriteria(custId, from, to, pageIndex, pageSize));

        public Tuple<int, IEnumerable<AbnormalBetTicket>> GetAbnormalBetTicketsCreditLicensee(
            int custId,
            DateTime from,
            DateTime to,
            int pageIndex,
            int pageSize)
            => GetAbnormalBetTickets(new GetAbnormalBetTicketCreditLicenseeCriteria(custId, from, to, pageIndex, pageSize));

        private Tuple<int, IEnumerable<AbnormalBetTicket>> GetAbnormalBetTickets(ICriteria criteria)
            => repository.GetAbnormalBetTickets(criteria);

        public int GetTotalSporksbookBetTickets(int custId, DateTime from, DateTime to)
            => GetTotalSporksbookBetTickets(
                new GetTotalSportsbookTicketCriteria(custId, from, to, AppSession.BalanceUpDate.MoveBetsDay));

        public int GetTotalSporksbookBetTicketsCreditLicensee(int custId, DateTime from, DateTime to)
        {
            var reportDays = AppSession.BalanceUpDate;

            return GetTotalSporksbookBetTickets(
                new GetTotalSportsbookTicketCreditLicenseeCriteria(custId, from, to, reportDays.MoveBetsDay, reportDays.BalanceUpDay));
        }

        private int GetTotalSporksbookBetTickets(ICriteria criteria)
            => repository.GetTotalSporksbookBetTickets(criteria);
    }
}
