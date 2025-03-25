namespace Fanex.Nap.CMS.ManageChatbots.Criteria.ChatbotMapping
{
    using Fanex.Data.Repository;

    public class GetChatBotsCriteria : CriteriaBase
    {
        public long? ChatID { get; set; }
        
        public string Username { get; set; }
        
        public bool IsSubAccOnly { get; set; }
        
        public string Site { get; set; }
        
        public short LinkStatus { get; set; }
        
        public short Level { get; set; }
        
        public string Currency { get; set; }
        
        public string BotIDs { get; set; }
        
        public short Status { get; set; }
        
        public int PageIndex { get; set; }
        
        public int PageSize { get; set; }
        
        public bool IsSpecialSite { get; set; }

        public int TotalRows { get; set; }
        
        public override string GetSettingKey()
            => "NAP_Chatbot_CustBotAccountListSel";

        public override bool IsValid()
            => true;
    }
}