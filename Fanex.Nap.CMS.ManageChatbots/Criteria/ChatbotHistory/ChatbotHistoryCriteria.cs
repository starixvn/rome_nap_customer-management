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
            string messengerUsername,
            DateTime fromDate,
            DateTime toDate,
            string function,
            string sendStatus)
        {
            Username = userName;
            MessengerUsername = messengerUsername;
            FromDate = fromDate;
            ToDate = toDate;
            Function = function;
            SendStatus = sendStatus;
        }

        public string Username { get; }

        public string MessengerUsername { get; }

        public DateTime FromDate { get; }

        public DateTime ToDate { get; }

        public string Function { get; }

        public string SendStatus { get; }

        public override string GetSettingKey()
            => "";
    }
}
