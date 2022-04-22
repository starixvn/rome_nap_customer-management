using Fanex.Data.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fanex.Nap.CMS.ManageChatbots.Criteria.ChatbotHistory
{
    public class ChatbotHistoryCriteria : CriteriaBase
    {
        public ChatbotHistoryCriteria(
            string userName,
            int chatID,
            int functionID,
            DateTime fromDate,
            DateTime toDate,
            byte sendStatus,
            byte isSpecial,
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

        public byte IsSpecial { get; }

        public int PageIndex { get; }

        public int PageSize { get; }

        public override string GetSettingKey()
            => "NAP_Chatbot_CustBotMessageSel";
    }
}
