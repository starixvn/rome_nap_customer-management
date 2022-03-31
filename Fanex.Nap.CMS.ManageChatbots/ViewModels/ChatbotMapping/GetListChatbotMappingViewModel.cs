namespace Fanex.Nap.CMS.ManageChatbots.ViewModels.ChatbotMapping
{
    public class GetListChatbotMappingViewModel
    {
        public string Username { get; set; }

        public short SportType { get; set; }

        public int ExchangeId { get; set; }

        public string SortField { get; set; }

        public string SortDirection { get; set; }

        public int Page { get; set; }

        public int Size { get; set; }

        public bool Cache { get; set; }

        public bool IsValid()
            =>  ExchangeId >= 0
               && !string.IsNullOrWhiteSpace(SortField)
               && !string.IsNullOrWhiteSpace(SortDirection);
    }
}