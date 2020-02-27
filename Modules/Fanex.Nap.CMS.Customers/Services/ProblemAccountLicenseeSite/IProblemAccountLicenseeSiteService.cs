namespace Fanex.Nap.CMS.Customers.Services.ProblemAccountLicenseeSite
{
    using System;
    using System.Collections.Generic;
    using Infrastructure.Models;
    using Models.ProblemAccount;
    using Models.ProblemAccountLicenseeSite;

    public interface IProblemAccountLicenseeSiteService
    {
        IEnumerable<LicenseeAcknowledgement> GetLicenseeAcknowledgements();

        Result UpdateLicenseeAcknowledgement(int licenseeId, bool isAcknowledgement, bool confirmed, int userId, string ip);

        IEnumerable<IdNameModel> GetCurrencies();

        IEnumerable<IdNameModel> GetReasons();

        IEnumerable<IdNameModel> GetPreventiveActions();

        ProblemAccountLicenseeSiteModel GetProblemAccounts(
            DateTime from,
            DateTime to,
            byte acknowledge,
            byte vip,
            int currencyId,
            byte isReduced,
            string reasonIds,
            byte preventiveActionId,
            string member,
            string licensee,
            int pageIndex,
            int pageSize);

        Result MarkNew(
            string usernames,
            byte preventiveActionId,
            byte reasonId,
            string comment,
            int userId,
            string ip);

        Result Update(
            string usernames,
            byte preventiveActionId,
            byte reasonId,
            string comment,
            int userId,
            string ip);

        Result Unmark(string usernames, int userId, string ip);

        Result ReduceBL(
            string usernames,
            int maxPerMatch,
            int maxBet,
            int userId,
            string ip);

        Result RemoveReduceBL(string usernames, bool confirmed, int userId, string ip);

        Result UpdateInformingStatus(string usernames, byte informType, int userId, string ip);
    }
}
