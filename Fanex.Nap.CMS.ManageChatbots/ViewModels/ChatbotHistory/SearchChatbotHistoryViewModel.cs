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

        public int ChatID { get; set; }

        public int FunctionID { get; set; }

        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }

        public byte Status { get; set; }

        public bool IsSpecial { get; set; }

        public int PageIndex { get; set; }

        public int PageSize { get; set; }
    }
}
