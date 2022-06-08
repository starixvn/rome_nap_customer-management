using System.Threading.Tasks;

namespace Fanex.Nap.CMS.ManageChatbots.Services.ChatBotMapping
{
    using System.Collections.Generic;
    using Models.ChatbotMapping;
    using ViewModels.ChatbotMapping;
    using Fanex.Nap.CMS.Infrastructure.Models;

    public interface IChatBotMappingService
    {
        IEnumerable<IdNameModel> GetCurrencies();

        IEnumerable<IdNameModel> GetSites();

        bool HasSpecialSitePermission(int customerId);

        ChatBotsMappingModel SearchChatBots(SearchChatBotsViewModel paramModel, int customerId);

        ChatBotsConfigModel GetConfig(GetConfigViewModel paramModel);

        Result UpdateFunctionsConfig(ChatBotFunctionConfig[] functionConfigs, int creatorId, string ip, int userId,int botId);

        Result UpdateLanguageConfig(int creatorId, string ip, int userId, int botId, string language);

      //  Result UnlinkChatBot(int creatorId, string ip, int userId, int botId,int chatId, bool status, bool isVerifyOtp);
        Result  UnlinkChatBot(int creatorId, string ip, int userId, int botId, int subAccId);
    }
}