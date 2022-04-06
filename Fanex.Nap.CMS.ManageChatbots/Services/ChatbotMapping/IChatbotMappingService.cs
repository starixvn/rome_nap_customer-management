namespace Fanex.Nap.CMS.ManageChatbots.Services.ChatBotMapping
{
    using System.Collections.Generic;
    using System.Web.Mvc;
    using ManageChatBots.Models.ChatBotMapping;
    using Models.ChatbotMapping;
    public interface IChatBotMappingService
    {
        IEnumerable<SelectListItem> GetCurrencies();

        IEnumerable<SelectListItem> GetSites(bool hasSpecialSitePermission);
        
        bool HasSpecialSitePermission(int customerId);
        ChatBotsMappingModel SearchChatBots();
    }
}