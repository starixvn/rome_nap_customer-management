namespace Fanex.Nap.CMS.ColossusBets.Repositories
{
    using System.Collections.Generic;
    using Fanex.Data.Repository;
    using Fanex.Nap.CMS.ColossusBets.Criteria;
    using Fanex.Nap.CMS.ColossusBets.Models;
    using Fanex.Nap.CMS.Infrastructure.Models;

    public interface IPaymentRepository
    {
        IList<Ticket> GetTickets(GetTicketsCriteria criteria);

        IList<TicketDetail> GetTicketDetails(ICriteria criteria);

        IList<TicketNote> GetTicketNotes(ICriteria criteria);

        Result UpdateTicketStatus(ICriteria criteria);
    }
}
