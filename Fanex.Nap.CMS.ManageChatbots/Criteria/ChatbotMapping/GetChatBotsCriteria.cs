namespace Fanex.Nap.CMS.ManageChatbots.Criteria.ChatbotMapping
{
    using Fanex.NAP.Framework.Data;

    public class GetChatBotsCriteria : CriteriaBase
    {
        public GetChatBotsCriteria(
            string messengerName = "",
            string username = "",
            bool isSubAccOnly = false, string site = "", short linkStatus = 0, short level = 0, string currency = "",
            short status = 0, int pageIndex = 1, int pageSize = 100)
        {
            MessengerName = messengerName;
            Username = username;
            IsSubAccOnly = isSubAccOnly;
            this.site = site;
            LinkStatus = linkStatus;
            Level = level;
            Currency = currency;
            Status = status;
            PageIndex = pageIndex;
            PageSize = pageSize;
        }

        public string MessengerName { get; set; }
        public string Username { get; set; }
        public bool IsSubAccOnly { get; set; }
        public string site { get; set; }
        public short LinkStatus { get; set; }
        public short Level { get; set; }
        public string Currency { get; set; }
        public short Status { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }

        public override string GetSettingKey()
            => "NAP_Chatbot_CustBotAccountListSel";
    }
}