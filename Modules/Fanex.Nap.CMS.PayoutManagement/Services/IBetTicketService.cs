namespace Fanex.Nap.CMS.PayoutManagement.Services
{
    using Fanex.Data.Repository;
    using Fanex.Nap.CMS.PayoutManagement.Models;

    public interface IBetTicketService
    {
        BetTicket GetMicrogaming(ICriteria criteria);

        ColossusBetsTicket GetColossusBets(ICriteria criteria);
    }
}
