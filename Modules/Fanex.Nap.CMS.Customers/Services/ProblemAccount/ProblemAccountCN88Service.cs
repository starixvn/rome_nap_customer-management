namespace Fanex.Nap.CMS.Customers.Services.ProblemAccount
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using Constants.ProblemAccount;
    using Criteria.ProblemAccount;
    using Infrastructure.Extensions;
    using Infrastructure.Models;
    using Infrastructure.Web;
    using Models.ProblemAccount;
    using Repositories.ProblemAccount;
    using Resources;

    public class ProblemAccountCN88Service : IProblemAccountCN88Service
    {
        private const string ReasonCacheKey = "Fanex.Nap.CMS.Customers.Services.ProblemAccountCreditSiteService.Reason_{0}";

        private readonly IProblemAccountRepository repository;

        public ProblemAccountCN88Service(IProblemAccountRepository repository = null)
        {
            this.repository = repository ?? new ProblemAccountCN88Repository();
        }

        public IEnumerable<IdNameModel> GetReasons(bool isAllReasons)
        {
            var mode = isAllReasons ? (byte)2 : (byte)1;
            var cacheKey = string.Format(ReasonCacheKey, mode);

            var reasons = CacheManager.Get<List<IdNameModel>>(cacheKey);

            if (reasons == null)
            {
                reasons = repository.GetReasons(new GetReasonCriteria(mode)).ToList();
                CacheManager.Set(cacheKey, reasons);
            }

            return reasons;
        }

        public ProblemAccountModel GetProblemAccounts(
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
            int pageSize)
        {
            var criteria = new SearchCriteria(
                from,
                to.GetEndOfDate(),
                roleId,
                currencyId,
                accountStatus,
                isReduced,
                reasonIds ?? string.Empty,
                username,
                isSearchDownlines,
                orderBy,
                isDirectlyMarked,
                pageIndex,
                pageSize);

            var rawData = repository.GetProblemAccounts(criteria);
            var accountStatusHash = AccountStatusConst.AccountStatusList;
            var reasonHash = BuildProblemAccountReasons(rawData.Item2);
            var problemAccounts = rawData.Item1;

            foreach (var item in problemAccounts)
            {
                item.StatusName = GetStatusName(accountStatusHash, item.CustStatus);
                item.MarkDateString = ToDateTimeString(item.MarkDate);
                item.Reasons = reasonHash.ContainsKey(item.CustId) ? reasonHash[item.CustId] : null;
            }

            return new ProblemAccountModel(rawData.Item3, problemAccounts);
        }

        public Result MarkNew(
            string usernames,
            byte roleId,
            byte reasonId,
            string comment,
            int userId,
            string ip)
            => repository.MarkNew(new MarkNewCriteria(usernames, roleId, reasonId, HtmlEncodeText(comment), userId, ip));

        public Result Unmark(
            string usernames,
            byte roleId,
            bool excludeAbnormalMember,
            bool excludeDirectMark,
            int userId,
            string ip)
        {
            var result = repository.ExecuteNonQuery(
                new UnmarkCriteria(usernames, roleId, excludeAbnormalMember, excludeDirectMark, userId, ip));

            if (result.IsSuccess)
            {
                result.Message = ProblemAccountResource.UnmarkSuccessfully;
            }

            return result;
        }

        public Result ReduceBL(
            string usernames,
            byte roleId,
            int maxPerEvent,
            int maxPerBet,
            int userId,
            string ip)
        {
            var result = repository.ExecuteNonQuery(
                new ReduceBLCriteria(usernames, roleId, maxPerEvent, maxPerBet, userId, ip));

            if (result.IsSuccess)
            {
                result.Message = ProblemAccountResource.ReduceBLSuccessfully;
            }

            return result;
        }

        public Result Inactivate(string usernames, byte roleId, bool confirmed, int userId, string ip)
        {
            var result = repository.ExecuteNonQuery(
                new InactivateCriteria(usernames, roleId, confirmed, userId, ip));

            if (result.IsSuccess)
            {
                result.Message = ProblemAccountResource.InactivateSuccessfully;
            }
            else if (IsNeedTobeConfirmedResult(result))
            {
                result.Message = string.Format(ProblemAccountResource.InactivateConfirmMessage, result.Message);
            }
            else if (IsAlreadyInactiveOrRemoveReduceBL(result))
            {
                result.Message = ProblemAccountResource.AllAccountsAlreadyInactivate;
            }

            return result;
        }

        public Result RemoveReduceBL(
            string usernames,
            byte roleId,
            bool isAllDownlines,
            bool confirmed,
            int userId,
            string ip)
        {
            var result = repository.ExecuteNonQuery(
                new RemoveReduceBLCriteria(usernames, roleId, isAllDownlines, confirmed, userId, ip));

            if (result.IsSuccess)
            {
                result.Message = ProblemAccountResource.RemoveReduceBLSuccessfully;
            }
            else if (IsNeedTobeConfirmedResult(result))
            {
                result.Message = string.Format(ProblemAccountResource.RemoveReduceBLConfirmMessage, result.Message);
            }
            else if (IsAlreadyInactiveOrRemoveReduceBL(result))
            {
                result.Message = ProblemAccountResource.AllAccountsAlreadyRemoveReduceBL;
            }

            return result;
        }

        public Result UpdateInformingStatus(
            string usernames,
            byte roleId,
            byte informType,
            bool isApplyDownlines,
            int userId,
            string ip)
        {
            var result = repository.ExecuteNonQuery(
                new UpdateInformingStatusCriteria(usernames, roleId, informType, isApplyDownlines, userId, ip));

            if (result.IsSuccess)
            {
                result.Message = ProblemAccountResource.UpdateInformingStatusSuccessfully;
            }

            return result;
        }

        private static IDictionary<int, IList<string>> BuildProblemAccountReasons(
            IEnumerable<ProblemAccountReason> reasons)
        {
            IList<string> reasonList;
            IDictionary<int, IList<string>> reasonHash = new Dictionary<int, IList<string>>();

            foreach (var item in reasons)
            {
                if (!reasonHash.TryGetValue(item.CustId, out reasonList))
                {
                    reasonList = new List<string>();
                    reasonHash.Add(item.CustId, reasonList);
                }

                reasonList.Add(
                    string.Format("{0}{1}: {2}",
                    item.CreateDate.ToShortFormatedDateWithSlash(),
                    string.IsNullOrWhiteSpace(item.Username) ? string.Empty : $" ({item.Username})",
                    item.Reason));
            }

            return reasonHash;
        }

        private static string GetStatusName(IDictionary<byte, string> accountStatus, byte status)
        {
            accountStatus.TryGetValue(status, out var statusName);

            return statusName;
        }

        private static string ToDateTimeString(DateTime value)
            => value.ToString("MM/dd/yyyy HH:mm:ss");

        private static string HtmlEncodeText(string text)
            => string.IsNullOrWhiteSpace(text) ? text : WebUtility.HtmlEncode(text);

        private static bool IsNeedTobeConfirmedResult(Result result)
            => result.Code == SQLErrorCodeConst.NeedTobeConfirmedCode;

        private static bool IsAlreadyInactiveOrRemoveReduceBL(Result result)
            => result.Code == SQLErrorCodeConst.AlreadyInactiveOrRemoveReduceBL;

    }
}
