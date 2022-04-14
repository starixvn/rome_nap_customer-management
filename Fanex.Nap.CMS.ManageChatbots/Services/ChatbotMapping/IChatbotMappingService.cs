namespace Fanex.Nap.CMS.ManageChatbots.Services.ChatBotMapping
{
    using System.Collections.Generic;
    using System.Web.Mvc;
    using Models.ChatbotMapping;
    using Fanex.Nap.CMS.ManageChatbots.ViewModels.ChatbotMapping;

    public interface IChatBotMappingService
    {
        IEnumerable<SelectListItem> GetCurrencies();

        IEnumerable<SelectListItem> GetSites(bool hasSpecialSitePermission);
        
        bool HasSpecialSitePermission(int customerId);
        
        ChatBotsMappingModel SearchChatBots(SearchChatBotsViewModel paramModel);
        ChatBotsConfigModel GetConfig(GetConfigViewModel paramModel);
        
    }
}