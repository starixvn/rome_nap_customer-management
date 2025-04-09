using System.Collections.Generic;
using System.Linq;
using Fanex.Nap.CMS.Customers.Criteria.LimitNewDownline;
using Fanex.Nap.CMS.Customers.Models.LimitNewDownline;
using Fanex.Nap.CMS.Customers.Resources;
using Fanex.Nap.CMS.Infrastructure.Models;
using Fanex.Nap.CMS.Infrastructure.Services.Implementations;

namespace Fanex.Nap.CMS.Customers.Services.LimitNewDownline
{
    public class LimitNewDownlineService : ServiceBase, ILimitNewDownlineService
    {
        public IEnumerable<string> GetSites()
            => DynamicRepository.Fetch<SiteGroup>(new GetCreditSiteCriteria(0)).Select(site => site.Name);

        public LimitDownlineReport GetLimitDownlineReport(string username, byte level, string site, int pageIndex, int pageSize)
        {
            var criteria = new GetLimitDownlineListCriteria(
                username == null ? string.Empty : username.Trim(),
                level,
                site,
                pageIndex,
                pageSize);
            var limitDownlineList = DynamicRepository.Fetch<LimitDownlineItem>(criteria).ToList();

            return new LimitDownlineReport(limitDownlineList, criteria.TotalItems);
        }

        public AddNewOrRemoveLimitResult AddNewLimit(string usernames, int userId, string ip)
        {
            var criteria = new AddNewLimitCriteria(usernames, userId, ip);
            DynamicRepository.Execute(criteria);
            var result = new AddNewOrRemoveLimitResult(
                    criteria.ErrCode,
                    criteria.ErrMsg,
                    criteria.CntMember);

            if (result.IsSuccess)
            {
                result.Message = string.Format(LimitNewDownlineResource.AddOrRemoveLimitSuccessfully, criteria.CntMember);
            }

            return result;
        }

        public AddNewOrRemoveLimitResult RemoveLimit(string usernames, int userId, string ip)
        {
            var  criteria = new RemoveLimitCriteria(usernames, userId, ip);
            DynamicRepository.Execute(criteria);
            var result = new AddNewOrRemoveLimitResult(
                    criteria.ErrCode,
                    criteria.ErrMsg,
                    criteria.CntMember);

            if (result.IsSuccess)
            {
                result.Message = string.Format(LimitNewDownlineResource.AddOrRemoveLimitSuccessfully, criteria.CntMember);
            }

            return result;
        }
    }
}
