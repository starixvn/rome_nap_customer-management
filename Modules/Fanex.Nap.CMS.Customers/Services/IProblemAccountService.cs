namespace Fanex.Nap.CMS.Customers.Services
{
    using System.Collections.Generic;
    using Infrastructure.Models;

    public interface IProblemAccountService
    {
        IEnumerable<IdNameModel> GetReasons();

        Result MarkNew(string usernames, byte roleId, byte reasonId, string comment, int userId, string ip);

        Result Unmark(string usernames, byte roleId, bool? abnormalBet, bool? unmarkByOPS, int userId, string ip);

        Result ReduceBL(string usernames, byte roleId, int maxPerEvent, int maxPerBet, int userId, string ip);

        Result Inactivate(string usernames, byte roleId, int userId, string ip);

        Result RemoveReduceBL(string usernames, byte roleId, bool? abnormalBet, bool? unmarkByOPS, int userId, string ip);
    }
}
