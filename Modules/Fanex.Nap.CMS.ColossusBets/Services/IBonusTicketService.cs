namespace Fanex.Nap.CMS.ColossusBets.Services
{
    using System.Collections.Generic;
    using Data.Repository;
    using Infrastructure.Models;
    using Models;

    public interface IBonusTicketService
    {
        BonusTicketDetail GetBonusTicketDetail(long refno);

        BonusTicketDetail GetBonusTicketDetailByRefNo(long refno);

        IEnumerable<BonusTicket> GetBonusTickets(ICriteria criteria);

        Result SaveBonusTicket(ICriteria criteria);

        Result DoSettlementAPI(string eventId, string ticketId, int customerId, decimal amount);
    }
}
