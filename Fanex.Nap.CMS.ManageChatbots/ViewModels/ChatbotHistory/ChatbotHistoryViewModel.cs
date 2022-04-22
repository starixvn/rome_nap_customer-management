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
            => new[]
            {
                 new SelectListItem{Text = "All", Value = "0"},
                 new SelectListItem{Text = "Sent", Value = "1"},
                 new SelectListItem{Text = "Failed", Value = "2"}
            };

        public DateTime DefaultFromDate => new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);

        public DateTime DefaultToDate => DateTime.Now;
    }
}
