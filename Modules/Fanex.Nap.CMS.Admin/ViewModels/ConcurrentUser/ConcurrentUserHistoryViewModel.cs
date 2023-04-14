using Fanex.Nap.CMS.Admin.Models.ConcurrentUser;
using System;
using System.Linq;
using System.Collections.Generic;

namespace Fanex.Nap.CMS.Admin.ViewModels.ConcurrentUser
{
    public class ConcurrentUserHistoryViewModel
    {
        const int zoneGMT8 = 8;
        private readonly DateTime minDate = new DateTime(2023, 5, 8);

        public ConcurrentUserHistoryViewModel(DateTime date)
        {
            FromDate = ToDate = date.AddHours(zoneGMT8);
            MaxDate = DateTime.UtcNow.AddHours(zoneGMT8);
            Summary = new List<ConcurrentUserHistoryModel>();
        }

        public ConcurrentUserHistoryViewModel(DateTime fromDate, DateTime toDate)
        {
            FromDate = fromDate.AddHours(zoneGMT8);
            ToDate = toDate.AddHours(zoneGMT8);
            MaxDate = DateTime.UtcNow.AddHours(zoneGMT8);
            Summary = new List<ConcurrentUserHistoryModel>();
        }

        public ConcurrentUserHistoryViewModel(
            DateTime fromDate,
            DateTime toDate,
            IEnumerable<ConcurrentUserHistoryModel> summary,
            IEnumerable<ConcurrentUserHistoryModel> dataBySiteLabel,
            IEnumerable<ConcurrentUserHistoryModel> dataBySiteName,
            IEnumerable<ConcurrentUserHistoryModel> dataByRoleName)
        {
            FromDate = fromDate;
            ToDate = toDate;
            Summary = summary;
            DataBySiteLabel = dataBySiteLabel.GroupBy(i => i.SiteLabel);
            DataBySiteName = dataBySiteName.GroupBy(i => (i.SiteName));
            DataByRoleName = dataByRoleName.OrderByDescending(i=>i.RoleName).GroupBy(i => (i.RoleName));            
        }

        public DateTime FromDate { get; }

        public DateTime ToDate { get; }

        public DateTime MaxDate { get; }

        public DateTime MinDate => minDate;

        public int MaxDaySupporting => 31;

        public IEnumerable<ConcurrentUserHistoryModel> Summary { get; set; }

        public IEnumerable<IGrouping<string, ConcurrentUserHistoryModel>> DataBySiteLabel { get; set; }

        public IEnumerable<IGrouping<string, ConcurrentUserHistoryModel>> DataBySiteName { get; set; }

        public IEnumerable<IGrouping<string, ConcurrentUserHistoryModel>> DataByRoleName { get; set; }
    }
}
