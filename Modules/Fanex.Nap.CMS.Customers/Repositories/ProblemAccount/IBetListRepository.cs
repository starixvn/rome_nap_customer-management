namespace Fanex.Nap.CMS.Customers.Repositories.ProblemAccount
{
    using System;
    using System.Collections.Generic;
    using Data.Repository;
    using Models.BetList;

    public interface IBetListRepository
    {
        Tuple<int, IEnumerable<AbnormalBetTicket>> GetAbnormalBetTickets(ICriteria criteria);

        int GetTotalSporksbookBetTickets(ICriteria criteria);
    }
}
