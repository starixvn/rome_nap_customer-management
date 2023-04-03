using System;

namespace Fanex.Nap.CMS.Admin.Models.ConcurrentUser
{
    public class ConcurrentUserHistoryModel
    {       
        public int Hour { get; set; }

        public int UserCount { get; set; }

        public string SiteLabel { get; set; }

        public string SiteName { get; set; }

        public string RoleName { get; set; }
    }
}
