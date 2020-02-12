namespace Fanex.Nap.CMS.Customers.Services
{
    using System;
    using System.Collections.Generic;
    using Infrastructure.Models;
    using Models.ProblemAccount;

    public interface IProblemAccountService
    {
        IEnumerable<IdNameModel> GetReasons(bool isAllReasons);

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
            int pageIndex,
            int pageSize);

        Result MarkNew(
            string usernames,
            byte roleId,
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

        Result Inactivate(string usernames, byte roleId, bool confirmed, int userId, string ip);

        Result RemoveReduceBL(
            string usernames,
            byte roleId,
            bool isAllDownlines,
            bool confirmed,
            int userId,
            string ip);

        Result UpdateInformingStatus(
            string usernames,
            byte roleId,
            byte informType,
            bool isApplyDownlines,
            int userId,
            string ip);

        IEnumerable<IdNameModel> GetCurrencies();

        IEnumerable<IdNameModel> GetReasonsCreditSites(bool isAllReasons);

        ProblemAccountModel GetProblemAccountsCreditSites(
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
            int pageIndex,
            int pageSize);

        Result MarkNewCreditSites(
            string usernames,
            byte roleId,
            byte reasonId,
            string comment,
            int userId,
            string ip);

        Result UnmarkCreditSites(
            string usernames,
            byte roleId,
            bool excludeAbnormalMember,
            bool excludeDirectMark,
            int userId,
            string ip);

        Result ReduceBLCreditSites(
            string usernames,
            byte roleId,
            int maxPerMatch,
            int maxBet,
            int userId,
            string ip);

        Result Close(string usernames, byte roleId, bool confirmed, int userId, string ip);

        Result RemoveReduceBLCreditSites(
            string usernames,
            byte roleId,
            bool isAllDownlines,
            bool confirmed,
            int userId,
            string ip);
    }
}
