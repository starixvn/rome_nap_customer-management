namespace Fanex.Nap.CMS.ManageChatbots.ViewModels.ChatbotHistory
{
    using System;

    public class SearchChatbotHistoryViewModel
    {
        public string Username { get; set; }

        public long ChatID { get; set; }

        public int FunctionID { get; set; }

        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }

        public byte Status { get; set; }

        public int PageIndex { get; set; }

        public int PageSize { get; set; }
    }
}
