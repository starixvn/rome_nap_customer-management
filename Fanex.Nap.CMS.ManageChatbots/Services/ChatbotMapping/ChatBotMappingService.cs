namespace Fanex.Nap.CMS.ManageChatbots.Services.ChatBotMapping
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Threading.Tasks;
    using Criteria.ChatbotMapping;
    using Fanex.Logging.Extensions.Web;
    using Fanex.Nap.CMS.Infrastructure.Configurations;
    using Fanex.Nap.CMS.Infrastructure.Models;
    using Infrastructure;
    using Models.ChatbotMapping;
    using Repositories.ChatbotMapping;
    using Resources;
    using TelegramBot.Client.Services;
    using ViewModels.ChatbotMapping;
    using Criteria.Share;

    public class ChatBotMappingService : IChatBotMappingService
    {
        private readonly IChatbotMappingRepository _repository;

        public ChatBotMappingService(IChatbotMappingRepository repository = null)
        {
            _repository = repository ?? new ChatBotMappingRepository();
        }

        public IEnumerable<IdNameModel> GetCurrencies()
        {
            return _repository.GetCurrencies(new GetCurrencyListCriteria { Mode = 1 });
        }

        public IEnumerable<IdNameModel> GetSites()
        {
            return _repository.GetSites(new GetSiteCriteria { Mode = 0 });
        }

        public IEnumerable<IdNameModel> GetChatBotNames()
        {
            return _repository.GetChatBotNames(new GetChatbotNamesCriteria());
        }

        public ChatBotsConfigModel GetConfig(GetConfigViewModel paramModel)
        {
            var criteria = new GetConfigChatBotCriteria
            {
                CustID = paramModel.CusId,
                BotID = paramModel.BotId,
                SubAccID = paramModel.SubAccId
            };

            var chatBotsConfig = _repository.GetConfigChatBot(criteria);
            return chatBotsConfig;
        }

        public ChatBotsMappingModel SearchChatBots(SearchChatBotsViewModel paramModel, int customerId)
        {
            var criteria = new GetChatBotsCriteria
            {
                ChatID = paramModel.ChatID,
                Username = paramModel.Username,
                IsSubAccOnly = paramModel.IsSubAccOnly,
                Site = paramModel.Site,
                LinkStatus = paramModel.LinkStatus,
                Level = paramModel.Level,
                Currency = paramModel.Currency,
                Status = paramModel.AccStatus,
                PageIndex = paramModel.PageIndex + 1,
                PageSize = paramModel.PageSize,
                IsSpecialSite = HasSpecialSitePermission(customerId),
                BotIDs = paramModel.BotIDs
            };

            var rawData = _repository.GetChatBots(criteria);
            return rawData;
        }

        public Result UpdateFunctionsConfig(
            ChatBotFunctionConfig[] functionConfigs,
            int creatorID,
            string ip,
            int userID,
            int botID)
        {
            var criteria = new UpdFunctionsChatBotCriteria
            {
                Creator = creatorID,
                IP = ip,
                FunctionConfigs = functionConfigs,
                UserID = userID,
                BotID = botID
            };

            var result = _repository.ExecuteNonQuery(criteria);

            if (result.IsSuccess)
            {
                result.Message = string.Format(Labels.UpdateSuccess, "Functions");
            }

            return result;
        }

        public Result UpdateLanguageConfig(int creatorID, string ip, int userID, int botID, string language)
        {
            var criteria = new UpdLanguageChatBotCriteria
            {
                Creator = creatorID,
                IP = ip,
                Language = language,
                UserID = userID,
                BotID = botID
            };

            var result = _repository.ExecuteNonQuery(criteria);

            if (result.IsSuccess)
            {
                result.Message = string.Format(Labels.UpdateSuccess, "Language");
            }

            return result;
        }

        public Result UnlinkChatBot(int creatorID, string ip, int userId, int botId, long chatId, string language)
        {
            var criteria = new UnlinkChatBotCriteria
            {
                Creator = creatorID,
                IP = ip,
                Status = false,
                IsVerifyOTP = false,
                UserID = userId,
                BotID = botId,
            };

            var result = _repository.ExecuteNonQuery(criteria);

            if (result.IsSuccess)
            {
                result.Message = string.Format(Labels.UpdateSuccess, "Status");

                Task.Run(() => NotifyUnlink(userId, chatId, botId, language));
            }

            return result;
        }

        private async Task NotifyUnlink(int userId, long chatId, int botId, string language)
        {
            try
            {
                await GetNotificationService().Unlink(userId, chatId, botId, language);
            }
            catch (Exception ex)
            {
                ExceptionHandler.Handle(ex);
            }
        }

        private static INotificationService GetNotificationService()
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            var telegramClient = HttpClientFactory.Create();
            telegramClient.BaseAddress = AppSettings.TelegramBotClientUri;

            return new NotificationService(telegramClient);
        }

        public bool HasSpecialSitePermission(int customerId)
        {
            var accountingPolicy =
                _repository.GetNapAccountingPolicy(new GetNapAccountingPolicy { CustID = customerId });
            var isHasAccessSpecialSites =
                accountingPolicy.Any(policy => policy.FunctionName == CMSFunctionKeys.SpecialSites);

            return isHasAccessSpecialSites;
        }
    }
}