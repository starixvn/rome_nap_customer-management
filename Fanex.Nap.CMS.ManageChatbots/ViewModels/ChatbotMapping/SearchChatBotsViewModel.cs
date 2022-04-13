namespace Fanex.Nap.CMS.ManageChatbots.ViewModels.ChatbotMapping
{
    public class SearchChatBotsViewModel
    {
        public string MessengerName { get; set; }
        
        public string Username { get; set; }
        
        public bool IsSubAccOnly { get; set; }
        
        public string Site { get; set; }
        
        public short LinkStatus { get; set; }
        
        public short Level { get; set; }
        
        public string Currency { get; set; }
        
        public short Status { get; set; }
        
        public int PageIndex { get; set; }
        
        public int PageSize { get; set; }
        
        public int TotalRows { get; set; }
        
        public bool Cache { get; set; }
        
        public short SportType { get; set; }
        public string SortField { get; set; }
        public string SortDirection { get; set; }

        /*public bool IsValid()
            => ExchangeId >= 0
               && !string.IsNullOrWhiteSpace(SortField)
               && !string.IsNullOrWhiteSpace(SortDirection);*/
    }
}