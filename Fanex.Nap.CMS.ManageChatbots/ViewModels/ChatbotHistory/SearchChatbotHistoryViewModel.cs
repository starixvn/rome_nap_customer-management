using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fanex.Nap.CMS.ManageChatbots.ViewModels.ChatbotHistory
{
    public class SearchChatbotHistoryViewModel
    {
        public string Username { get; set; }

        public string MessengerUsername { get; set; }

        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }

        public string Function { get; set; }

        public string SendStatus { get; set; }
    }
}
