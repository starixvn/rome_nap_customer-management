using Fanex.Data;
using Fanex.Nap.CMS.Admin.Criteria;
using Fanex.Nap.CMS.Admin.Models.ConcurrentUser;
using Fanex.Nap.CMS.Admin.ViewModels.ConcurrentUser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fanex.Nap.CMS.Admin.Repositories
{
    interface IConcurrentUserRepository
    {
        ConcurrentUserViewModel GetDataLatest(GetConcurrentUserCriteria criteria);

        ConcurrentUserHistoryViewModel GetDataHistory(GetConcurrentUserCriteria criteria);
    }

    public class ConcurrentUserRepository : IConcurrentUserRepository
    {
        public ConcurrentUserHistoryViewModel GetDataHistory(GetConcurrentUserCriteria criteria)
        {
            using (var objDb = new ObjectDb(criteria.GetSettingKey()))
            {
                var reader = objDb.QueryMultiple(criteria.GetSpParams());
                var summary = reader.Read<ConcurrentUserHistoryModel>();
                var bySiteLabel = reader.Read<ConcurrentUserHistoryModel>();
                var bySiteName = reader.Read<ConcurrentUserHistoryModel>();
                var byRoleName = reader.Read<ConcurrentUserHistoryModel>();

                return new ConcurrentUserHistoryViewModel(criteria.FromDate, criteria.ToDate, summary, bySiteLabel, bySiteName, byRoleName);
            }
        }

        public ConcurrentUserViewModel GetDataLatest(GetConcurrentUserCriteria criteria)
        {
            using (var objDb = new ObjectDb(criteria.GetSettingKey()))
            {
                var reader = objDb.QueryMultiple(criteria.GetSpParams());
                var summary = reader.Read<SMANumberModel>();
                var bySiteLabel = reader.Read<ConcurrentUserDetailModel>();
                var bySiteName = reader.Read<ConcurrentUserDetailModel>();

                return new ConcurrentUserViewModel
                {
                    DataBySiteLabel = bySiteLabel,
                    DataBySiteName = bySiteName,
                    Summary = summary.FirstOrDefault() ?? new SMANumberModel()
                };
            }
        }
    }
}
