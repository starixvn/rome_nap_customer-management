namespace Fanex.Nap.CMS.ManageChatbots.ViewModels.NotificationList
{
    using System;
    using Fanex.Nap.CMS.ManageChatbots.Constants;
    using System.Collections.Generic;
    using System.Web.Mvc;

    public class NotificationListViewModel
    {
        public IEnumerable<SelectListItem> NotificationType { get; set; }

        public IEnumerable<SelectListItem> SendStatus
         => new SelectList(NotificationStatus.NotificationStatusList, "Key", "Value");

        public DateTime DefaultFromDate => DateTime.Today.AddDays(-7);

        public DateTime DefaultToDate => DateTime.Now;
    }
}
