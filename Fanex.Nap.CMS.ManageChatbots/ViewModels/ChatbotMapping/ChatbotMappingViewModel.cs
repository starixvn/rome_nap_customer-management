namespace Fanex.Nap.CMS.ManageChatbots.ViewModels.ChatbotMapping
{
    using System.Collections.Generic;
    using Fanex.Nap.CMS.Infrastructure.Models;


    public class ChatbotMappingViewModel
    {
        public IEnumerable<IdNameModel> Currencies { get; }

        public ChatbotMappingViewModel(
            IEnumerable<IdNameModel> currencies = null)
        {
            Currencies = currencies;
        }
    }
}