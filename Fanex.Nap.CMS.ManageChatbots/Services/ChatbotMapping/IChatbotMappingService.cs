namespace Fanex.Nap.CMS.ManageChatbots.Services.ChatBotMapping
{
    using System.Collections.Generic;
    using Fanex.Nap.CMS.Infrastructure.Models;
    using Models.ChatbotMapping;
    using ViewModels.ChatbotMapping;

    public interface IChatBotMappingService
    {
        IEnumerable<IdNameModel> GetCurrencies();

        IEnumerable<SiteViewModel> GetSites();
        
        IEnumerable<IdNameModel> GetChatBotNames();

        bool HasSpecialSitePermission(int customerId);

        ChatBotsMappingModel SearchChatBots(SearchChatBotsViewModel paramModel, int customerId);

        ChatBotsConfigModel GetConfig(GetConfigViewModel paramModel);

        Result UpdateFunctionsConfig(ChatBotFunctionConfig[] functionConfigs, int creatorId, string ip, int userId,int botId);

        Result UpdateLanguageConfig(int creatorId, string ip, int userId, int botId, string language);

        Result  UnlinkChatBot(int creatorId, string ip, int userId, int botId, long chatId, string language);
    }
}