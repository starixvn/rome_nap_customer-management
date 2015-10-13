namespace Fanex.Nap.CMS.ColossusBets.Services
{
    using System.Collections.Generic;
    using Fanex.Data.Repository;
    using Fanex.Nap.CMS.ColossusBets.Criteria;
    using Fanex.Nap.CMS.ColossusBets.Models;
    using Fanex.Nap.CMS.Infrastructure.Models;

    public interface IPayoutService
    {
        IList<IdNameModel>[] GetStatusStateMachine();

        IList<Ticket> GetTickets(GetTicketsCriteria criteria);

        TicketDetailReport GetTicketDetailReport(ICriteria criteria);

        IList<TicketNote> GetTicketNotes(ICriteria criteria);

        Result UpdateTicketStatus(ICriteria criteria);
    }
}
