namespace Fanex.Nap.CMS.ManageChatbots.Criteria.ChatbotHistory
{
    using System;
    using Fanex.Data.Repository;

    public class ChatbotHistoryCriteria : CriteriaBase
    {
        public ChatbotHistoryCriteria(
            string userName,
            int chatID,
            int functionID,
            DateTime fromDate,
            DateTime toDate,
            byte sendStatus,
            bool isSpecial,
            int pageIndex,
            int pageSize)
        {
            Username = userName ?? String.Empty;
            ChatID = chatID;
            FunctionID = functionID;
            FromDate = fromDate;
            ToDate = toDate;
            Status = sendStatus;
            IsSpecial = isSpecial;
            PageIndex = pageIndex;
            PageSize = pageSize;
        }

        public string Username { get; set; }

        public int ChatID { get; }

        public int FunctionID { get; }

        public DateTime FromDate { get; }

        public DateTime ToDate { get; }

        public byte Status { get; }

        public bool IsSpecial { get; }

        public int PageIndex { get; }

        public int PageSize { get; }

        public int TotalRows { get; set; }

        public override string GetSettingKey()
            => "NAP_Chatbot_CustBotMessageSel";
    }
}
