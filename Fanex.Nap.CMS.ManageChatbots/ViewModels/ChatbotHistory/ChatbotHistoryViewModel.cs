using Fanex.Nap.CMS.ManageChatbots.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Fanex.Nap.CMS.ManageChatbots.ViewModels.ChatbotHistory
{
    public class ChatbotHistoryViewModel
    {
        public ChatbotHistoryViewModel(IEnumerable<SelectListItem> functions)
        {
            Functions = functions;
        }

        public IEnumerable<SelectListItem> Functions { get; }

        public IEnumerable<SelectListItem> SendStatus
            => new SelectList(SendStatusConst.SendStatusList, "Key", "Value");

        public DateTime DefaultFromDate => new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);

        public DateTime DefaultToDate => DateTime.Now;
    }
}
