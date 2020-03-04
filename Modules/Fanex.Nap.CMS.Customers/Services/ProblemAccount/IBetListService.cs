namespace Fanex.Nap.CMS.Customers.Services.ProblemAccount
{
    using System;
    using System.Collections.Generic;
    using Models.BetList;

    public interface IBetListService
    {
        Tuple<int, IEnumerable<AbnormalBetTicket>> GetAbnormalBetTickets(
            int custId,
            DateTime from,
            DateTime to,
            int pageIndex,
            int pageSize);

        Tuple<int, IEnumerable<AbnormalBetTicket>> GetAbnormalBetTicketsCreditLicensee(
            int custId,
            DateTime from,
            DateTime to,
            int pageIndex,
            int pageSize);

        int GetTotalSporksbookBetTickets(int custId, DateTime from, DateTime to);

        int GetTotalSporksbookBetTicketsCreditLicensee(int custId, DateTime from, DateTime to);
    }
}
