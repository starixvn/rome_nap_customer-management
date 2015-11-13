namespace Fanex.Nap.CMS.PayoutManagement.Repositories
{
    using Fanex.Data.Repository;
    using Fanex.Nap.CMS.PayoutManagement.Models;

    public interface IBetTicketRepository
    {
        BetTicket GetMicrogaming(ICriteria criteria);

        ColossusBetsTicket GetColossusBets(ICriteria criteria);
    }
}
