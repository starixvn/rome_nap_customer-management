namespace Fanex.Nap.CMS.Customers.Services
{
    using System;
    using System.Collections.Generic;
    using Criteria.BetList;
    using Fanex.Data.Repository;
    using Infrastructure.Web;
    using Models.BetList;
    using Repositories;

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
            int pageSize,
            bool isCN88)
        {
            ICriteria criteria = isCN88
                ? new GetAbnormalBetTicketCriteria(custId, from, to, pageIndex, pageSize)
                : new GetAbnormalBetTicketCreditDepositCriteria(custId, from, to, pageIndex, pageSize);

            return repository.GetAbnormalBetTickets(criteria);
        }

        public int GetTotalSporksbookBetTickets(int custId, DateTime from, DateTime to, bool isCN88)
        {
            var reportDays = AppSession.BalanceUpDate;

            ICriteria criteria = isCN88
                ? new GetTotalSportsbookTicketCriteria(custId, from, to, reportDays.MoveBetsDay)
                : new GetTotalSportsbookTicketCreditDepositCriteria(custId, from, to, reportDays.MoveBetsDay, reportDays.BalanceUpDay);

            return repository.GetTotalSporksbookBetTickets(criteria);
        }
    }
}
