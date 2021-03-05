using System.Collections.Generic;
using Fanex.Nap.CMS.Customers.Criteria.LimitNewDownline;
using Fanex.Nap.CMS.Customers.Models.LimitNewDownline;
using Fanex.Nap.CMS.Customers.Repositories.LimitNewDownline;
using Fanex.Nap.CMS.Customers.Resources;

namespace Fanex.Nap.CMS.Customers.Services.LimitNewDownline
{
    public class LimitNewDownlineService : ILimitNewDownlineService
    {
        private readonly ILimitNewDownlineRepository repository;

        public LimitNewDownlineService(ILimitNewDownlineRepository repository = null)
        {
            this.repository = repository ?? new LimitNewDownlineRepository();
        }

        public IEnumerable<string> GetSites()
            => repository.GetSites(new GetCreditSiteCriteria(0));

        public LimitDownlineReport GetLimitDownlineReport(string username, byte level, string site, int pageIndex, int pageSize)
        {
            var criteria = new GetLimitDownlineListCriteria(
                username == null ? string.Empty : username.Trim(),
                level,
                site,
                pageIndex,
                pageSize);
            var limitDownlineList = repository.GetLimitDownlineList(criteria);

            return new LimitDownlineReport(limitDownlineList, criteria.TotalItems);
        }

        public AddNewOrRemoveLimitResult AddNewLimit(string usernames, int userId, string ip)
        {
            var result = repository.ExecuteNonQuery(new AddNewLimitCriteria(usernames, userId, ip));

            if (result.IsSuccess)
            {
                result.Message = string.Format(LimitNewDownlineResource.AddOrRemoveLimitSuccessfully, result.CountMember);
            }

            return result;
        }

        public AddNewOrRemoveLimitResult RemoveLimit(string usernames, int userId, string ip)
        {
            var result = repository.ExecuteNonQuery(new RemoveLimitCriteria(usernames, userId, ip));

            if (result.IsSuccess)
            {
                result.Message = string.Format(LimitNewDownlineResource.AddOrRemoveLimitSuccessfully, result.CountMember);
            }

            return result;
        }
    }
}
