namespace Fanex.Nap.CMS.ManageChatbots.Criteria.ChatbotHistory
{
    using System;
    using Fanex.Data.Repository;

    public class ChatbotHistoryCriteria : CriteriaBase
    {
#pragma warning disable S107
        public ChatbotHistoryCriteria(
            string userName,
            long chatID,
            int functionID,
            string botIDs,
            DateTime fromDate,
            DateTime toDate,
            byte sendStatus,
            bool isSpecial,
            int pageIndex,
            int pageSize)
#pragma warning restore S107
        {
            Username = userName ?? string.Empty;
            ChatID = chatID;
            FunctionID = functionID;
            BotIDs = botIDs;
            FromDate = fromDate;
            ToDate = toDate;
            Status = sendStatus;
            IsSpecial = isSpecial;
            PageIndex = pageIndex;
            PageSize = pageSize;
        }

        public string Username { get; set; }

        public long ChatID { get; }

        public int FunctionID { get; }

        public string BotIDs { get; }

        public DateTime FromDate { get; }

        public DateTime ToDate { get; }

        public byte Status { get; }

        public bool IsSpecial { get; }

        public int PageIndex { get; }

        public int PageSize { get; }

        public int TotalRows { get; set; }

        public override string GetSettingKey()
            => "NAP_Chatbot_CustBotMessageSel";

        public override bool IsValid()
            => true;
    }
}
