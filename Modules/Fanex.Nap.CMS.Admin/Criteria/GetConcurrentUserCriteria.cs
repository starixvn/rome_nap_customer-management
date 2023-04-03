using Fanex.Data.Repository;
using System;

namespace Fanex.Nap.CMS.Admin.Criteria
{
    public class GetConcurrentUserCriteria : CriteriaBase
    {
        public GetConcurrentUserCriteria()
        {
            const int latestMode = 1;
            ReportType = latestMode;
            FromDate = ToDate = DateTime.Now;
        }

        public GetConcurrentUserCriteria(DateTime fromDate, DateTime toDate)
        {
            const int historyMode = 2;
            ReportType = historyMode;
            FromDate = fromDate;
            ToDate = toDate;
        }

        public int ReportType { get; set; }

        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }

        public override string GetSettingKey()
            => "Age_rpt_ConcurrentOnlineUser";
    }
}
