namespace Fanex.Nap.CMS.ManageChatbots.ViewModels.ChatbotMapping
{
    public class SearchChatBotsViewModel
    {
        public string Username { get; set; }
        
        public bool IsSubAccOnly { get; set; }
        
        public string Site { get; set; }
        
        public short LinkStatus { get; set; }
        
        public short Level { get; set; }
        
        public string Currency { get; set; }
        
        public short AccStatus { get; set; }
        
        public int PageIndex { get; set; }
        
        public int PageSize { get; set; }
        
        public long? ChatID { get; set; }
        
        public string BotIDs { get; set; }
        
    }
}