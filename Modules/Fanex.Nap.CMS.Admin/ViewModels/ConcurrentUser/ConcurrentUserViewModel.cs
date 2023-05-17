using Fanex.Nap.CMS.Admin.Models.ConcurrentUser;
using System;
using System.Linq;
using System.Collections.Generic;

namespace Fanex.Nap.CMS.Admin.ViewModels.ConcurrentUser
{
    public class ConcurrentUserViewModel
    {
        public ConcurrentUserViewModel()
        {
            const int zoneGMT8= 8;
            _CurrentTime = DateTime.UtcNow.AddHours(zoneGMT8);
            DataBySiteLabel = new List<ConcurrentUserDetailModel>();
            DataBySiteName = new List<ConcurrentUserDetailModel>();
        }

        private DateTime _CurrentTime { get; }

        public string CurrentTime => _CurrentTime.ToString("yyyy-MM-dd HH:mm:ss.fff");

        public SMANumberModel Summary { get; set; }

        public IEnumerable<ConcurrentUserDetailModel> DataBySiteLabel { get; set; }

        public IEnumerable<ConcurrentUserDetailModel> DataBySiteName { get; set; }
    }
}
