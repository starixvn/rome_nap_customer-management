namespace Fanex.Nap.CMS.Customers.Services
{
    using System.Collections.Generic;
    using Criteria.ProblemAccount;
    using Infrastructure.Models;
    using Models.ProblemAccount;
    using Repositories;
    using Resources;

    public class ProblemAccountService : IProblemAccountService
    {
        private readonly IProblemAccountRepository repository;

        public ProblemAccountService(IProblemAccountRepository repository = null)
        {
            this.repository = repository ?? new ProblemAccountRepository();
        }

        public IEnumerable<IdNameModel> GetReasons()
            => repository.GetReasons(new GetReasonCriteria());

        public Result MarkNew(string usernames, byte roleId, byte reasonId, string comment, int userId, string ip)
        {
            var criteria = new MarkNewCriteria(usernames, roleId, reasonId, comment, userId, ip);
            repository.MarkNew(criteria);

            return new MarkNewResult(criteria.ErrCode, criteria.ErrMsg, criteria.SuccessList, criteria.FailList);
        }

        public Result Unmark(string usernames, byte roleId, bool? abnormalBet, bool? unmarkByOPS, int userId, string ip)
        {
            var result = repository.ExecuteNonQuery(new UnmarkCriteria(abnormalBet, unmarkByOPS, usernames, roleId, userId, ip));

            if (IsSuccessResult(result))
            {
                result.Message = ProblemAccountResource.UnmarkSuccessfully;
            }

            return result;
        }

        public Result ReduceBL(string usernames, byte roleId, int maxPerEvent, int maxPerBet, int userId, string ip)
        {
            var result = repository.ExecuteNonQuery(new ReduceBLCriteria(usernames, roleId, maxPerEvent, maxPerBet, userId, ip));

            if (IsSuccessResult(result))
            {
                result.Message = ProblemAccountResource.ReduceBLSuccessfully;
            }

            return result;
        }

        public Result Inactivate(string usernames, byte roleId, int userId, string ip)
        {
            var result = repository.ExecuteNonQuery(new InactivateCriteria(usernames, roleId, userId, ip));

            if (IsSuccessResult(result))
            {
                result.Message = ProblemAccountResource.InactivateSuccessfully;
            }

            return result;
        }

        public Result RemoveReduceBL(string usernames, byte roleId, bool? abnormalBet, bool? unmarkByOPS, int userId, string ip)
        {
            var result = repository.ExecuteNonQuery(new RemoveReduceBLCriteria(abnormalBet, unmarkByOPS, usernames, roleId, userId, ip));

            if (IsSuccessResult(result))
            {
                result.Message = ProblemAccountResource.RemoveReduceBLSuccessfully;
            }

            return result;
        }

        private static bool IsSuccessResult(Result result)
            => result.Code == 0;
    }
}
