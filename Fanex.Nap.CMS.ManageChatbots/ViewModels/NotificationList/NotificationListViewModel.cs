namespace Fanex.Nap.CMS.ManageChatbots.ViewModels.NotificationList
{
    using System;
    using System.Collections.Generic;
    using System.Web.Mvc;
    using Fanex.Nap.CMS.ManageChatbots.Constants;

    public class NotificationListViewModel
    {
        private const int OneWeek = 7;

        public IEnumerable<SelectListItem> NotificationType { get; set; }

        public IEnumerable<SelectListItem> Bots { get; set; }

#pragma warning disable S2325
        public IEnumerable<SelectListItem> SendStatus
            => new SelectList(NotificationStatus.NotificationStatusList, "Key", "Value");

        public DateTime DefaultFromDate => DateTime.Today.AddDays(-OneWeek);

        public DateTime DefaultToDate => DateTime.Now;
#pragma warning restore S2325
    }
}
