using Fanex.Nap.CMS.Admin.Criteria;
using Fanex.Nap.CMS.Admin.Models.ConcurrentUser;
using Fanex.Nap.CMS.Admin.ViewModels.ConcurrentUser;
using Fanex.Nap.CMS.Infrastructure.Services.Implementations;
using System;
using System.Linq;

namespace Fanex.Nap.CMS.Admin.Services.ConcurrentUser
{
    public interface IConcurrentUserService
    {
        ConcurrentUserViewModel GetDataLatest();
        ConcurrentUserHistoryViewModel GetDataHistory(DateTime date);

        ConcurrentUserHistoryViewModel GetDataHistory(DateTime fromDate, DateTime toDate);
    }

    public class ConcurrentUserService : ServiceBase, IConcurrentUserService
    {
        public ConcurrentUserHistoryViewModel GetDataHistory(DateTime date)
        {
            var criteria = new GetConcurrentUserCriteria(date);

            return GetDataHistory(criteria);
        }

        public ConcurrentUserHistoryViewModel GetDataHistory(DateTime fromDate, DateTime toDate)
        {
            var criteria = new GetConcurrentUserCriteria(fromDate, toDate);

            return GetDataHistory(criteria);
        }

        public ConcurrentUserViewModel GetDataLatest()
        {
            var criteria = new GetConcurrentUserCriteria();

            var (summary, bySiteLabel, bySiteName) = DynamicRepository.FetchMultiple<SMANumberModel, ConcurrentUserDetailModel, ConcurrentUserDetailModel>(criteria);

            return new ConcurrentUserViewModel
            {
                DataBySiteLabel = bySiteLabel,
                DataBySiteName = bySiteName,
                Summary = summary.FirstOrDefault() ?? new SMANumberModel()
            };
        }

        private ConcurrentUserHistoryViewModel GetDataHistory(GetConcurrentUserCriteria criteria)
        {
            var (summary, bySiteLabel, bySiteName, byRoleName) = DynamicRepository.FetchMultiple<ConcurrentUserHistoryModel, ConcurrentUserHistoryModel, ConcurrentUserHistoryModel, ConcurrentUserHistoryModel>(criteria);

            return new ConcurrentUserHistoryViewModel(criteria.FromDate, criteria.ToDate, summary, bySiteLabel, bySiteName, byRoleName);
        }
    }
}
