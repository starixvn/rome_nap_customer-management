namespace Fanex.Nap.CMS.ColossusBets.Repositories
{
    using System.Collections.Generic;
    using Data.Repository;
    using Infrastructure.Models;
    using Models;

    public interface IBonusTicketRepository
    {
        BonusTicketDetail GetBonusTicketDetail(ICriteria criteria);

        BonusTicketDetail GetBonusTicketDetailByRefNo(ICriteria criteria);

        IEnumerable<BonusTicket> GetBonusTickets(ICriteria criteria);

        Result SaveBonusTicket(ICriteria criteria);
    }
}
