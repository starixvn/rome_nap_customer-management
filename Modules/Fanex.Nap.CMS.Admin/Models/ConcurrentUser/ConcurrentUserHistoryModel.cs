using System;

namespace Fanex.Nap.CMS.Admin.Models.ConcurrentUser
{
    public class ConcurrentUserHistoryModel
    {
        public string DayOfWeek => Date.ToString("ddd");

        public string Day => Date.ToString("MMM-d");

        public DateTime Date { get; set; }

        public int Hour { get; set; }

        public int UserCount { get; set; }

        public string SiteLabel { get; set; }

        public string SiteName { get; set; }

        public string RoleName { get; set; }
    }
}
