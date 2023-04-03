using Fanex.Nap.CMS.Admin.Models.ConcurrentUser;
using System;
using System.Linq;
using System.Collections.Generic;

namespace Fanex.Nap.CMS.Admin.ViewModels.ConcurrentUser
{
    public class ConcurrentUserHistoryViewModel
    {
        public ConcurrentUserHistoryViewModel()
        {
            const int zoneGMT8 = 8;
            FromDate = ToDate = DateTime.UtcNow.AddHours(zoneGMT8);
            Summary = new List<ConcurrentUserHistoryModel>();
        }
        public ConcurrentUserHistoryViewModel(
            IEnumerable<ConcurrentUserHistoryModel> summary,
            IEnumerable<ConcurrentUserHistoryModel> dataBySiteLabel,
            IEnumerable<ConcurrentUserHistoryModel> dataBySiteName,
            IEnumerable<ConcurrentUserHistoryModel> dataByRoleName)
            : this()
        {
            Summary = summary;
            DataBySiteLabel = dataBySiteLabel.GroupBy(i => i.SiteLabel);
            DataBySiteName = dataBySiteName.GroupBy(i => (i.SiteName));
            DataByRoleName = dataByRoleName.GroupBy(i => (i.RoleName));
        }

        public DateTime FromDate { get; }

        public DateTime ToDate { get; }

        public IEnumerable<ConcurrentUserHistoryModel> Summary { get; set; }

        public IEnumerable<IGrouping<string, ConcurrentUserHistoryModel>> DataBySiteLabel { get; set; }

        public IEnumerable<IGrouping<string, ConcurrentUserHistoryModel>> DataBySiteName { get; set; }

        public IEnumerable<IGrouping<string, ConcurrentUserHistoryModel>> DataByRoleName { get; set; }
       
    }
}
