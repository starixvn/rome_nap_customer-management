namespace Fanex.Nap.CMS.ManageChatbots.Services.ChatBotMapping
{
    using System.Collections.Generic;
    using System.Web.Mvc;
    using Models.ChatbotMapping;
    using ViewModels.ChatbotMapping;
    using Fanex.Nap.CMS.Infrastructure.Models;
    
    public interface IChatBotMappingService
    {
        IEnumerable<SelectListItem> GetCurrencies();

        IEnumerable<SelectListItem> GetSites(bool hasSpecialSitePermission);
        
        bool HasSpecialSitePermission(int customerId);
        
        ChatBotsMappingModel SearchChatBots(SearchChatBotsViewModel paramModel,  int customerId );
        
        ChatBotsConfigModel GetConfig(GetConfigViewModel paramModel);
        
        Result UpdateFunctionsConfig(ChatBotFunctionConfig[] functionConfigs, int CreatorId, string IP, int UserID);

    }
}