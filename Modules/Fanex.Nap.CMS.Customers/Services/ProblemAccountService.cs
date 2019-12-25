namespace Fanex.Nap.CMS.Customers.Services
{
    using System;
    using System.Collections.Generic;
    using System.Net;
    using Constants;
    using Criteria.ProblemAccount;
    using Infrastructure.Extensions;
    using Infrastructure.Models;
    using Models.ProblemAccount;
    using Repositories;
    using Resources;

    public class ProblemAccountService : IProblemAccountService
    {
        private const int NeedTobeConfirmedCode = 101;
        private const int AlreadyInactiveOrRemoveReduceBL = 102;
        private readonly IProblemAccountRepository repository;

        public ProblemAccountService(IProblemAccountRepository repository = null)
        {
            this.repository = repository ?? new ProblemAccountRepository();
        }

        public IEnumerable<IdNameModel> GetReasons(bool isAllReasons)
            => repository.GetReasons(new GetReasonCriteria(isAllReasons ? (byte)0 : (byte)1));

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
            int pageIndex,
            int pageSize,
            int directlyMarked)
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
                pageIndex,
                pageSize,
                directlyMarked);

            var rawData = repository.GetProblemAccounts(criteria);
            var reasonHash = BuildProblemAccountReasons(rawData.Item2);
            var problemAccounts = rawData.Item1;

            foreach (var item in problemAccounts)
            {
                item.StatusName = GetStatusName(item.CustStatus);
                item.MarkDateString = ToDateTimeString(item.MarkDate);
                item.Reasons = reasonHash.ContainsKey(item.CustId) ? reasonHash[item.CustId] : null;
            }

            return new ProblemAccountModel(criteria.TotalItems, problemAccounts);
        }

        public Result MarkNew(
            string usernames,
            byte roleId,
            byte reasonId,
            string comment,
            int userId,
            string ip)
        {
            var criteria = new MarkNewCriteria(usernames, roleId, reasonId, HtmlEncodeText(comment), userId, ip);
            repository.MarkNew(criteria);

            return new MarkNewResult(criteria.ErrCode, criteria.ErrMsg, criteria.SuccessList, criteria.FailList);
        }

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

            if (IsSuccessResult(result))
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

            if (IsSuccessResult(result))
            {
                result.Message = ProblemAccountResource.ReduceBLSuccessfully;
            }

            return result;
        }

        public Result Inactivate(string usernames, byte roleId, bool confirmed, int userId, string ip)
        {
            var result = repository.ExecuteNonQuery(
                new InactivateCriteria(usernames, roleId, confirmed, userId, ip));

            if (IsSuccessResult(result))
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

            if (IsSuccessResult(result))
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
            int informType,
            bool applyDownlines,
            int userId,
            string ip)
        {
            var result = repository.ExecuteNonQuery(
                new UpdateInformingStatusCriteria(usernames, roleId, informType, applyDownlines, userId, ip));

            if (IsSuccessResult(result))
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

        private static string GetStatusName(byte status)
        {
            switch (status)
            {
                case AccountStatusConst.Inactive:
                    return nameof(AccountStatusConst.Inactive);

                case AccountStatusConst.Suspended:
                    return nameof(AccountStatusConst.Suspended);

                case AccountStatusConst.ViewOnly:
                    return ProblemAccountResource.ViewOnly;

                case AccountStatusConst.Active:
                    return nameof(AccountStatusConst.Active);

                default:
                    return string.Empty;
            }
        }

        private static string ToDateTimeString(DateTime value)
            => value.ToString("MM/dd/yyyy HH:mm:ss");

        private static string HtmlEncodeText(string text)
            => string.IsNullOrWhiteSpace(text) ? text : WebUtility.HtmlEncode(text);

        private static bool IsSuccessResult(Result result)
            => result.Code == 0;

        private static bool IsNeedTobeConfirmedResult(Result result)
            => result.Code == NeedTobeConfirmedCode;

        private static bool IsAlreadyInactiveOrRemoveReduceBL(Result result)
            => result.Code == AlreadyInactiveOrRemoveReduceBL;
    }
}
