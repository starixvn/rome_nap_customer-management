namespace Fanex.Nap.CMS.ManageChatbots.ViewModels.ChatbotHistory
{
    using System;
    using System.Web.Mvc;
    using System.Collections.Generic;
    using Fanex.Nap.CMS.ManageChatbots.Constants;

    public class ChatbotHistoryViewModel
    {
        public ChatbotHistoryViewModel(IEnumerable<SelectListItem> functions)
        {
            Functions = functions;
        }

        public IEnumerable<SelectListItem> Functions { get; }

        public IEnumerable<SelectListItem> SendStatus
            => new SelectList(SendStatusConst.SendStatusList, "Key", "Value");

        public IEnumerable<SelectListItem> Bots { get; set; }

        public DateTime DefaultFromDate => new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);

        public DateTime DefaultToDate => DateTime.Now;
    }
}
