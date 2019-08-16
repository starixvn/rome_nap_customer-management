namespace Fanex.Nap.CMS.Customers.Services
{
    using System;
    using System.Collections.Generic;
    using Criteria.BetList;
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
            int pageSize)
            => repository.GetAbnormalBetTickets(new GetAbnormalBetTicketCriteria(custId, from, to, pageIndex, pageSize));

        public int GetTotalSporksbookBetTickets(
            int custId,
            DateTime from,
            DateTime to,
            DateTime moveBetsDay)
            => repository.GetTotalSporksbookBetTickets(new GetTotalSportsbookTicketCriteria(custId, from, to, moveBetsDay));
    }
}
