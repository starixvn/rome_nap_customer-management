namespace Fanex.Nap.CMS.Customers.Services
{
    using System;
    using System.Collections.Generic;
    using Fanex.Nap.CMS.Customers.Models.ProblemAccount;
    using Infrastructure.Models;

    public interface IProblemAccountCreditSiteService
    {
        IEnumerable<IdNameModel> GetCurrencies();

        IEnumerable<IdNameModel> GetReasons();

        IEnumerable<IdNameModel> GetPreventiveActions();

        IDictionary<int, IList<IdNameModel>> GetPreventiveActionReasons();

        ProblemAccountModel GetProblemAccounts(
            DateTime from,
            DateTime to,
            byte roleId,
            int currencyId,
            byte accountStatus,
            byte isReduced,
            string reasonIds,
            string username,
            bool isSearchDownlines,
            byte orderBy,
            byte isDirectlyMarked,
            byte preventiveActionId,
            int pageIndex,
            int pageSize);

        Result MarkNew(
            string usernames,
            byte roleId,
            byte preventiveActionId,
            byte reasonId,
            string comment,
            int userId,
            string ip);

        Result Update(
            string usernames,
            byte roleId,
            byte preventiveActionId,
            byte reasonId,
            string comment,
            int userId,
            string ip);

        Result Unmark(
            string usernames,
            byte roleId,
            bool excludeAbnormalMember,
            bool excludeDirectMark,
            int userId,
            string ip);

        Result ReduceBL(
            string usernames,
            byte roleId,
            int maxPerEvent,
            int maxPerBet,
            int userId,
            string ip);

        Result Close(string usernames, byte roleId, bool confirmed, int userId, string ip);

        Result RemoveReduceBL(
            string usernames,
            byte roleId,
            bool isAllDownlines,
            bool confirmed,
            int userId,
            string ip);
    }
}
