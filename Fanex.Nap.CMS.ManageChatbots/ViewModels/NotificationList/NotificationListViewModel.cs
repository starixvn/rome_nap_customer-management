using Fanex.Nap.CMS.ManageChatbots.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Fanex.Nap.CMS.ManageChatbots.ViewModels.NotificationList
{
    public class NotificationListViewModel
    {
        public IEnumerable<SelectListItem> NotificationType { get; set; }

        public IEnumerable<SelectListItem> SendStatus
         => new SelectList(NotificationStatus.NotificationStatusList, "Key", "Value");

        public DateTime DefaultFromDate => DateTime.Today.AddDays(-7);

        public DateTime DefaultToDate => DateTime.Now;
    }
}
